using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;
using org.estat.PcAxis.helpers;
using org.estat.PcAxis;
using System.Threading;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace org.estat.PcAxis.PcAxisProvider {
    public sealed class PcAxisCommand : DbCommand {
        public PcAxisConnection PcAxisConnection { get; set; }

        public override void Cancel() {
            command.Cancel();
        }

        private SQLiteCommand command;
        // Methods
        public PcAxisCommand() //: this(null, null)
        {
            command = new SQLiteCommand(null, null);
        }

        public PcAxisCommand(SQLiteConnection connection) { //: this(null, connection, null) {
            command = new SQLiteCommand(null, connection, null);
        }

        public PcAxisCommand(string commandText) {
            //: this(commandText, null, null) {
            command = new SQLiteCommand(commandText, null, null);
        }

        public PcAxisCommand(string commandText, SQLiteConnection connection) {
            //: this(commandText, connection, null) {
            command = new SQLiteCommand(commandText, connection, null);
        }

        public PcAxisCommand(string commandText, SQLiteConnection connection, SQLiteTransaction transaction) {
            command = new SQLiteCommand(commandText, connection, transaction);
            this.command.CommandTimeout = 30;
            this.command.UpdatedRowSource = UpdateRowSource.None;
            if (commandText != null) {
                this.command.CommandText = commandText;
            }
            if (connection != null) {
                this.command.Connection = connection;
                this.command.CommandTimeout = connection.DefaultTimeout;
            }
            if (transaction != null) {
                this.command.Transaction = transaction;
            }
        }

        protected override DbParameter CreateDbParameter() {
            return this.command.CreateParameter();
        }

        public new SQLiteParameter CreateParameter() {
            return new SQLiteParameter();
        }

        protected override void Dispose(bool disposing) {
            this.command.Dispose();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) {
            DoLoadData();
            return this.command.ExecuteReader(behavior);
        }

        public override int ExecuteNonQuery() {
            DoLoadData();
            using (SQLiteDataReader reader = this.command.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.SingleResult)) {
                while (reader.NextResult()) {
                }
                return reader.RecordsAffected;
            }
        }

        public new SQLiteDataReader ExecuteReader() {
            DoLoadData();
            return this.command.ExecuteReader(CommandBehavior.Default);
        }

        public override object ExecuteScalar() {
            DoLoadData();
            using (SQLiteDataReader reader = this.command.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.SingleResult)) {
                if (reader.Read()) {
                    return reader[0];
                }
            }
            return null;
        }


        public override void Prepare() {
        }

        //private string ExtractTableNameFromSQLQuery(string query) {
        //    //int pos1 = query.ToLower().IndexOf("where");
        //    //int pos2 = query.ToLower().Substring(0,pos1).LastIndexOf("from");
        //    //string tablename = query.Substring(pos2+4, pos1 - pos2-4);

        //    int pos1 = query.ToLower().IndexOf("from");

        //    string secondpart = query.Substring(pos1 + 4, query.Length - pos1 - 4);
        //    secondpart = secondpart.Trim();

        //    int pos2 = secondpart.IndexOf(" ", 0);
        //    if (pos2 == -1) pos2 = secondpart.Length;
        //    string tablename = secondpart.Substring(0, pos2);
        //    tablename = tablename.Trim();

        //    return tablename;
        //}

        private List<string> ExtractTableNamesFromSQLQuery(string query) {
            //int pos1 = query.ToLower().IndexOf("where");
            //int pos2 = query.ToLower().Substring(0,pos1).LastIndexOf("from");
            //string tablename = query.Substring(pos2+4, pos1 - pos2-4);
            List<string> tablenames = new List<string>();
            Regex startTableSectionRegex = new Regex(@"(([\n]|[ ])from([\n]|[ ]))|(([\n]|[ ])join([\n]|[ ]))", RegexOptions.IgnoreCase);
            MatchCollection matches = startTableSectionRegex.Matches(query);
            int minpos = matches.Count>0 ? matches[0].Index : -1;
            //int pos1 = query.ToLower().IndexOf("from");
            //int pos2 = query.ToLower().IndexOf("join");
            //int minpos = pos1 > pos2 && pos2 >= 0 ? pos2 : pos1;
            string querypart = query;
            while (minpos > 0) {
                querypart = querypart.Substring(minpos + 5, querypart.Length - minpos - 5).Trim();
                //string tablename = querypart.Substring(0, pos).Trim();
                //tablenames.Add(tablename);
                Regex endTableSectionRegex = new Regex(@"[)]|(([\n]|[ ])select([\n]|[ ]))|(([\n]|[ ])union([\n]|[ ]))|(([\n]|[ ])where([\n]|[ ]))", RegexOptions.IgnoreCase);
                MatchCollection endmatches = endTableSectionRegex.Matches(querypart);
                int endOfTablesSection = endmatches.Count > 0 ? endmatches[0].Index : querypart.Length;

                //int[] posend = new int[5];
                //posend[0] = querypart.IndexOf(')');
                //posend[1] = querypart.ToLower().IndexOf("select");
                //posend[2] = querypart.ToLower().IndexOf("union");
                //posend[3] = querypart.ToLower().IndexOf("where");
                //posend[4] = querypart.Length;
                //int endOfTablesSection = int.MaxValue;
                //foreach (int posendoftables in posend) {
                //    if (endOfTablesSection > posendoftables && posendoftables >= 0)
                //        endOfTablesSection = posendoftables;
                //}

                //if (endOfTablesSection != -1) {
                    string tablesSection = querypart.Substring(0, endOfTablesSection).Trim();
                    string[] tables = tablesSection.Split(',');
                    foreach (string table in tables) {
                        string tablenam = table.Trim();
                        if (tablenam.Contains("--")) {
                            string[] tableWithComments = tablenam.Split(new string[] { Environment.NewLine, "\\n" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string tabnam in tableWithComments) {
                                if (!tabnam.Trim().StartsWith("--")) {
                                    tablenam = tabnam;
                                    break;
                                }
                            }
                            //if (tableWithComments.Length > 1)
                            //    tablenam = tableWithComments[1];
                        }
                        int pos = tablenam.IndexOf(" ", 0);
                        if (pos == -1) pos = tablenam.Length;
                        tablenam = tablenam.Substring(0, pos).Trim();
                        if (!tablenames.Contains(tablenam)
                            && !tablenam.Contains("(")
                            && !tablenam.Contains(")")
                            && !tablenam.Contains("-"))
                            tablenames.Add(tablenam);
                    }

                    matches = startTableSectionRegex.Matches(querypart);
                    minpos = matches.Count > 0 ? matches[0].Index : -1;

                    //int pos3 = querypart.ToLower().IndexOf("from");
                    //int pos4 = querypart.ToLower().IndexOf("join");
                    //minpos = pos3 > pos4 && pos4 >= 0 ? pos4 : pos3;
                //} else
                //    minpos = -1;
            }

            return tablenames;
        }


        private void DoLoadData() {

            List<string> tablenames = ExtractTableNamesFromSQLQuery(this.command.CommandText);
            var transaction = this.Connection.BeginTransaction();
            foreach (string tablename in tablenames) {
                if (!PcAxisConnection.LoadedTables.Contains(tablename))
                {
                    
                    DataTable tables = PcAxisConnection.GetSchema("Tables");
                    DataRow[] tableRows = tables.Select("TABLE_NAME ='" + tablename + "'");
                    if (tableRows.Length > 0)
                    { // MAT-249 workaround
                        PcAxisConnection.LoadedTables.Add(tablename);
                        string filepath = tableRows[0]["FILEPATH"].ToString();
                        //string filepath = PcAxisConnection.GetSchema("Tables").Select("TABLE_NAME ='" + tablename + "'")[0]["FILEPATH"].ToString();
                        PcAxis pcaxis = new PcAxis(filepath);

                        //CREATE A TEMPLATE COMMAND FOR INSERT A ROW IN THE MAIN TABLE
                        DataTable columns = PcAxisConnection.GetSchema("ColumnsMain", new string[] { null, null, tablename });
                        SQLiteCommand com = new SQLiteCommand(this.Connection) { Transaction = transaction };
                        StringBuilder insertSQL = new StringBuilder();
                        insertSQL.Append("INSERT INTO \"");
                        insertSQL.Append(tablename + "_DATA");
                        insertSQL.Append("\" VALUES (");
                        int i = 0;
                        foreach (DataRow row in columns.Rows)
                        {
                            i++;
                            string paramname = string.Format("@param{0}", i);
                            insertSQL.Append(paramname);
                            if (i != columns.Rows.Count)
                            {
                                insertSQL.Append(",");
                            }
                            else
                            {
                                insertSQL.Append(")");
                            }
                            SQLiteParameter param = new SQLiteParameter();
                            //param.Value = val;
                            param.DbType = DbType.String;
                            param.ParameterName = paramname;
                            com.Parameters.Add(param);
                        }
                        com.CommandText = insertSQL.ToString();

                        //CREATE A TEMPLATE COMMAND FOR INSERT A ROW IN THE KEYWORDS SINGLE VALUE TABLE
                        columns = PcAxisConnection.GetSchema("ColumnsKeywordsSingleValue", new string[] { null, null, tablename });
                        SQLiteCommand com2 = new SQLiteCommand(Connection) { Transaction = transaction };
                        insertSQL = new StringBuilder();
                        insertSQL.Append("INSERT INTO \"");
                        insertSQL.Append(tablename + "_KEYWORDS_SINGLE_VALUE");
                        insertSQL.Append("\" VALUES (");
                        i = 0;
                        foreach (DataRow row in columns.Rows) {
                            i++;
                            string paramname = string.Format("@param{0}", i);
                            insertSQL.Append(paramname);
                            if (i != columns.Rows.Count) {
                                insertSQL.Append(",");
                            } else {
                                insertSQL.Append(")");
                            }
                            SQLiteParameter param = new SQLiteParameter();
                            //param.Value = val;
                            param.DbType = DbType.String;
                            param.ParameterName = paramname;
                            com2.Parameters.Add(param);
                        }
                        com2.CommandText = insertSQL.ToString();


                        //CREATE A TEMPLATE COMMAND FOR INSERT A ROW IN THE CONTVARIABLE KEYWORDS TABLE
                        columns = PcAxisConnection.GetSchema("ColumnsContVariableKeywords", new string[] { null, null, tablename });
                        SQLiteCommand com3 = new SQLiteCommand(Connection) { Transaction = transaction };
                        insertSQL = new StringBuilder();
                        insertSQL.Append("INSERT INTO \"");
                        insertSQL.Append(tablename + "_CONTVARIABLE_KEYWORDS");
                        insertSQL.Append("\" VALUES (@contvar,");
                        i = 0;

                        SQLiteParameter paramContVar = new SQLiteParameter();
                        //param.Value = val;
                        paramContVar.DbType = DbType.String;
                        paramContVar.ParameterName = "@contvar";
                        com3.Parameters.Add(paramContVar);


                        foreach (DataRow row in columns.Rows) {
                            i++;
                            string paramname = string.Format("@param{0}", i);
                            insertSQL.Append(paramname);
                            if (i != columns.Rows.Count) {
                                insertSQL.Append(",");
                            } else {
                                insertSQL.Append(")");
                            }
                            SQLiteParameter param = new SQLiteParameter();
                            //param.Value = val;
                            param.DbType = DbType.String;
                            param.ParameterName = paramname;
                            com3.Parameters.Add(param);
                        }
                        com3.CommandText = insertSQL.ToString();

                        pcaxis.DataPreviewRows = this.PcAxisConnection.DataPreviewRows;
                        //RUN THIS COMMAND FOR EACH RECORD
                        pcaxis.InsertDataInMemoryDB(com, com2, com3);
                    }
                }
            }

            transaction.Commit();

        }

        // Properties
        [DefaultValue(""), Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), RefreshProperties(RefreshProperties.All)]
        public override string CommandText {
            get {
                return this.command.CommandText;
            }
            set {
                this.command.CommandText = value;
            }
        }

        [DefaultValue(30)]
        public override int CommandTimeout {
            get {
                return this.command.CommandTimeout;
            }
            set {
                this.command.CommandTimeout = value;
            }
        }

        [RefreshProperties(RefreshProperties.All), DefaultValue(1)]
        public override CommandType CommandType {
            get {
                return CommandType.Text;
            }
            set {
                if (value != CommandType.Text) {
                    throw new NotSupportedException();
                }
            }
        }

        [DefaultValue((string)null), Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new SQLiteConnection Connection {
            get {
                return this.command.Connection;
            }
            set {
                this.command.Connection = value;
            }
        }

        protected override DbConnection DbConnection {
            get {
                return this.command.Connection;
            }
            set {
                this.command.Connection = (SQLiteConnection)value;
            }
        }

        protected override DbParameterCollection DbParameterCollection {
            get {
                return this.Parameters;
            }
        }

        protected override DbTransaction DbTransaction {
            get {
                return this.Transaction;
            }
            set {
                this.Transaction = (SQLiteTransaction)value;
            }
        }

        [DefaultValue(true), EditorBrowsable(EditorBrowsableState.Never), DesignOnly(true), Browsable(false)]
        public override bool DesignTimeVisible {
            get {
                return this.command.DesignTimeVisible;
            }
            set {
                this.command.DesignTimeVisible = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new SQLiteParameterCollection Parameters {
            get {
                return this.command.Parameters;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new SQLiteTransaction Transaction {
            get {
                return this.command.Transaction;
            }
            set {
                this.command.Transaction = value;
            }
        }

        [DefaultValue(0)]
        public override UpdateRowSource UpdatedRowSource {
            get {
                return this.command.UpdatedRowSource;
            }
            set {
                this.command.UpdatedRowSource = value;
            }
        }
    }
}
