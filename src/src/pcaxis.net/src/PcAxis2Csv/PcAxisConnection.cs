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

namespace org.estat.PcAxis.PcAxisProvider {
    using System.CodeDom.Compiler;
    using System.Globalization;

    public class PcAxisConnection : DbConnection {
        public static bool UsesDataCaching = false;
        public static bool DataPreviewOnly = false;

        /// <summary>
        /// the directory in which sqlite files will be saved.
        /// </summary>
        private static string tempDir; // = Path.GetTempPath();

        /// <summary>
        /// Gets or sets the directory in which sqlite files will be saved.
        /// </summary>
        public static string TempDir
        {
            get
            {
                return tempDir;
            }

            set
            {
                tempDir = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use in memory sqlite or not
        /// </summary>
        public static bool UseMemory { get; set; }

        /// <summary>
        /// The temporary file collection
        /// </summary>
        private TempFileCollection _tempFileCollection;

        /// <summary>
        /// The number of rows to show when <see cref="DataPreviewOnly"/> is set to true
        /// </summary>
        public int DataPreviewRows {
            get;
            set;
        }
        
        private SQLiteConnection sqliteConn;

        private string connectionstring;

        public PcAxisConnection() {
            this.sqliteConn = new SQLiteConnection();//connectionString);
            DataPreviewRows = 100;
        }

        public PcAxisConnection(String connectionString) {
            this.sqliteConn = new SQLiteConnection();//connectionString);
            DataPreviewRows = 100;
            connectionstring = connectionString;
        }

        #region Overrides of Component

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. 
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (UsesDataCaching)
            {
                return;
            }
            base.Dispose(disposing);

            if (disposing)
            {
                LoadedTables.Clear();
                CreatedTables.Clear();
                if (this.sqliteConn != null)
                {
                    this.sqliteConn.Dispose();
                    this.sqliteConn = null;
                }
            }

            if (this._tempFileCollection != null)
            {
                this._tempFileCollection.Delete();
                ((IDisposable)this._tempFileCollection).Dispose();
                this._tempFileCollection = null;
            }
        }

        #endregion

        #region Get Schema Methods 
        public override DataTable GetSchema(string collectionName, string[] restrictionValues) {
            GetSchemata();
            if (restrictionValues.Length > 2)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("TABLE_NAME"));
                dt.Columns.Add(new DataColumn("COLUMN_NAME"));
                dt.Columns.Add(new DataColumn("DATA_TYPE"));
                DataRow[] rows = SchemaCollection[collectionName].Select("TABLE_NAME='" + restrictionValues[2] + "'");
                foreach (DataRow row in rows)
                {
                    DataRow r = dt.NewRow();
                    r["TABLE_NAME"] = row["TABLE_NAME"];
                    r["COLUMN_NAME"] = row["COLUMN_NAME"];
                    r["DATA_TYPE"] = row["DATA_TYPE"];
                    dt.Rows.Add(r);
                }
                return dt;
            }
            else if ( restrictionValues.Length == 1)// MAT-280
            {
                DataTable dt = SchemaCollection[collectionName].Copy();
                dt.Clear();
                DataRow[] rows = SchemaCollection[collectionName].Select("TABLE_NAME='" + restrictionValues[0] + "'");
                for (int i = 0; i < rows.Length; i++)
                {
                    dt.ImportRow(rows[i]);
                }
                return dt;
            }
            return GetSchema(collectionName);
        }
        public override DataTable GetSchema(string collectionName) {
            GetSchemata();
            return SchemaCollection[collectionName];
        }

        public override DataTable GetSchema() {
            GetSchemata();
            return SchemaCollection["Tables"];
        }

        public Dictionary<string,DataTable> SchemaCollection { get; set; }


        public void GetSchemata() {
            if (SchemaCollection==null) {
                SchemaCollection = new Dictionary<string, DataTable>();

                DataTable dtTables = new DataTable();
                DataTable dtColumns = new DataTable();
                DataTable dtColumnsKeywordsSingleValue = new DataTable();
                DataTable dtColumnsContVariableKeywords = new DataTable();
                DataTable dtColumnsMain = new DataTable();
                DataTable dtColumnsContVariable = new DataTable();

                dtTables.Columns.Add(new DataColumn("TABLE_NAME"));
                dtTables.Columns.Add(new DataColumn("FILEPATH"));

                dtColumns.Columns.Add(new DataColumn("TABLE_NAME"));
                dtColumns.Columns.Add(new DataColumn("COLUMN_NAME"));
                dtColumns.Columns.Add(new DataColumn("DATA_TYPE"));

                dtColumnsKeywordsSingleValue.Columns.Add(new DataColumn("TABLE_NAME"));
                dtColumnsKeywordsSingleValue.Columns.Add(new DataColumn("COLUMN_NAME"));
                dtColumnsKeywordsSingleValue.Columns.Add(new DataColumn("DATA_TYPE"));

                dtColumnsContVariableKeywords.Columns.Add(new DataColumn("TABLE_NAME"));
                dtColumnsContVariableKeywords.Columns.Add(new DataColumn("COLUMN_NAME"));
                dtColumnsContVariableKeywords.Columns.Add(new DataColumn("DATA_TYPE"));

                dtColumnsMain.Columns.Add(new DataColumn("TABLE_NAME"));
                dtColumnsMain.Columns.Add(new DataColumn("COLUMN_NAME"));
                dtColumnsMain.Columns.Add(new DataColumn("DATA_TYPE"));

                dtColumnsContVariable.Columns.Add(new DataColumn("TABLE_NAME"));
                dtColumnsContVariable.Columns.Add(new DataColumn("COLUMN_NAME"));
                dtColumnsContVariable.Columns.Add(new DataColumn("DATA_TYPE"));

                string filePath = PCAxisHelper.GetFilePathFromConnectionString(connectionstring);
                if (filePath.Trim().ToLower().StartsWith("ftp://")) {
                    filePath = PCAxisHelper.SynchFTPFiles(filePath);
                }

                DirectoryInfo di = new DirectoryInfo(filePath);
                FileInfo[] files = di.GetFiles("*.px");
                foreach (FileInfo fi in files) {
                    string tablename = PcAxis.FormatTableName(fi.Name);//.Substring(0, fi.Name.LastIndexOf('.')));//.Replace(" ", "_").Replace("-", "_");
                    DataRow dr = dtTables.NewRow();
                    dr["TABLE_NAME"] = tablename;
                    dr["FILEPATH"] = fi.FullName;
                    dtTables.Rows.Add(dr);

                    PcAxis pcAxis = new PcAxis(fi.FullName);
                    List<string> columns = pcAxis.GetColumns(ColumnFilter.All);
                    foreach (string column in columns) {
                        DataRow drc = dtColumns.NewRow();
                        drc["TABLE_NAME"] = tablename;
                        drc["COLUMN_NAME"] = column;
                        drc["DATA_TYPE"] = "VARCHAR";
                        dtColumns.Rows.Add(drc);
                    }
                    columns = pcAxis.GetColumns(ColumnFilter.KeywordsSingleValue);
                    foreach (string column in columns) {
                        DataRow drc = dtColumnsKeywordsSingleValue.NewRow();
                        drc["TABLE_NAME"] = tablename;
                        drc["COLUMN_NAME"] = column;
                        drc["DATA_TYPE"] = "VARCHAR";
                        dtColumnsKeywordsSingleValue.Rows.Add(drc);
                    }
                    columns = pcAxis.GetColumns(ColumnFilter.ContVariableKeywords);
                    foreach (string column in columns) {
                        DataRow drc = dtColumnsContVariableKeywords.NewRow();
                        drc["TABLE_NAME"] = tablename;
                        drc["COLUMN_NAME"] = column;
                        drc["DATA_TYPE"] = "VARCHAR";
                        dtColumnsContVariableKeywords.Rows.Add(drc);
                    }
                    columns = pcAxis.GetColumns(ColumnFilter.Main);
                    foreach (string column in columns) {
                        DataRow drc = dtColumnsMain.NewRow();
                        drc["TABLE_NAME"] = tablename;
                        drc["COLUMN_NAME"] = column;
                        drc["DATA_TYPE"] = "VARCHAR";
                        dtColumnsMain.Rows.Add(drc);
                    }
                    columns = pcAxis.GetColumns(ColumnFilter.ContVariable);
                    foreach (string column in columns) {
                        DataRow drc = dtColumnsContVariable.NewRow();
                        drc["TABLE_NAME"] = tablename;
                        drc["COLUMN_NAME"] = column;
                        drc["DATA_TYPE"] = "VARCHAR";
                        dtColumnsContVariable.Rows.Add(drc);
                    }
                }

                SchemaCollection.Add("Tables", dtTables);
                SchemaCollection.Add("Columns", dtColumns);
                SchemaCollection.Add("ColumnsKeywordsSingleValue", dtColumnsKeywordsSingleValue);
                SchemaCollection.Add("ColumnsContVariableKeywords", dtColumnsContVariableKeywords);
                SchemaCollection.Add("ColumnsMain", dtColumnsMain);
                SchemaCollection.Add("ColumnsContVariable", dtColumnsContVariable);
            }
        }
        #endregion

        private List<string> CreatedTables = new List<string>();
        internal List<string> LoadedTables = new List<string>();

        public override void Open() {
            
            if (!UseMemory && this._tempFileCollection == null)
            {
                this._tempFileCollection = tempDir != null ? new TempFileCollection(tempDir) : new TempFileCollection();
            }

            // if not already initialized, put it in memory
            if (this.sqliteConn == null) {
                this.sqliteConn = new SQLiteConnection(); 
            }
            if (sqliteConn.State != ConnectionState.Open) {
                if (UseMemory)
                {
                    this.sqliteConn.ConnectionString = "Data Source=:memory:;Version=3;";
                }
                else
                {
                    this.sqliteConn.ConnectionString = string.Format(
                        CultureInfo.InvariantCulture, "Data Source={0};Version=3;New=true;Synchronous=Off;", this._tempFileCollection.AddExtension("pcaxis.sqlite"));
                }

                sqliteConn.Open();
            }

            GetSchemata();

            DataTable dtTables = GetSchema("Tables");
            foreach (DataRow row in dtTables.Rows) {
                if (!CreatedTables.Contains(row["TABLE_NAME"].ToString())) {
                    CreatedTables.Add(row["TABLE_NAME"].ToString());
                    DbCommand command = sqliteConn.CreateCommand();
                    command.CommandText = GetCreateStatement(row["TABLE_NAME"].ToString());
                    command.Connection = sqliteConn;
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GetCreateStatement(string tableName) {
            string contVariable="";
            bool hassinglevalues = false;
            bool hascontvarvalues = false;

            StringBuilder retval = new StringBuilder();

            DataTable columns = GetSchema("ColumnsMain", new string[] { null, null, tableName });
            retval.Append("CREATE TABLE \"");
            retval.Append(tableName + "_DATA");
            retval.Append("\"(\"");
            int i = 0;
            foreach (DataRow row in columns.Rows) {
                i++;
                retval.Append(row["COLUMN_NAME"]);
                if (i != columns.Rows.Count) {
                    retval.Append("\" ");
                    retval.Append(row["DATA_TYPE"]);
                    retval.Append(",\"");
                } else
                    retval.Append("\")");
            }

            retval.Append(";");


            columns = GetSchema("ColumnsKeywordsSingleValue", new string[] { null, null, tableName });
            if (columns.Rows.Count > 0) {
                hassinglevalues=true;
                retval.Append("CREATE TABLE \"");
                retval.Append(tableName + "_KEYWORDS_SINGLE_VALUE");
                retval.Append("\"(\"");
                i = 0;
                foreach (DataRow row in columns.Rows) {
                    i++;
                    retval.Append(row["COLUMN_NAME"]);
                    if (i != columns.Rows.Count) {
                        retval.Append("\" ");
                        retval.Append(row["DATA_TYPE"]);
                        retval.Append(",\"");
                    } else
                        retval.Append("\")");
                }

                retval.Append(";");
            }

            columns = GetSchema("ColumnsContVariable", new string[] { null, null, tableName });
            if (columns.Rows.Count == 1) {
                contVariable = columns.Rows[0]["COLUMN_NAME"].ToString();
                columns = GetSchema("ColumnsContVariableKeywords", new string[] { null, null, tableName });
                if (columns.Rows.Count > 0) {
                    hascontvarvalues =true;
                    retval.Append("CREATE TABLE \"");
                    retval.Append(tableName + "_CONTVARIABLE_KEYWORDS");
                    retval.Append("\"(\"" + contVariable + "\" VARCHAR,\"");
                    i = 0;
                    foreach (DataRow row in columns.Rows) {
                        i++;
                        retval.Append(row["COLUMN_NAME"]);
                        retval.Append("\" ");
                        retval.Append(row["DATA_TYPE"]);
                        if (i != columns.Rows.Count) {
                            retval.Append(",\"");
                        } else
                            retval.Append(")");
                    }
                }
            }

            retval.Append(";");

            columns = GetSchema("Columns", new string[] { null, null, tableName });
            retval.Append("CREATE VIEW \"");
            retval.Append(tableName);
            retval.Append("\" AS SELECT ");
            i = 0;
            foreach (DataRow row in columns.Rows) {
                i++;
                if (row["COLUMN_NAME"].ToString() == contVariable) {
                    retval.Append("a.");
                    retval.Append("\"");
                    retval.Append(row["COLUMN_NAME"]);
                    retval.Append("\" ");
                } else {
                    retval.Append("\"");
                    retval.Append(row["COLUMN_NAME"]);
                    retval.Append("\" ");
                }
                retval.Append(" ");
                if (row["COLUMN_NAME"].ToString() == contVariable) {
                    retval.Append(" as \"");
                    retval.Append(row["COLUMN_NAME"]);
                    retval.Append("\" ");
                }
                if (i != columns.Rows.Count) {
                    retval.Append(",");
                }
            }
            retval.Append(" FROM ");
            retval.Append(tableName + "_DATA a");
            if (hassinglevalues) {
                retval.Append(",");
                retval.Append(tableName + "_KEYWORDS_SINGLE_VALUE b");
            }
            if (hascontvarvalues) {
                retval.Append(",");
                retval.Append(tableName + "_CONTVARIABLE_KEYWORDS c");
                retval.Append(" WHERE  a.\"" + contVariable + "\"=c.\"" + contVariable + "\"");
            }
            retval.Append(";");


            return retval.ToString();
        }

        public SQLiteConnection SqliteConn {
            get { return sqliteConn; }
        }

        public new DbTransaction BeginTransaction() {
            return sqliteConn.BeginTransaction();// (PxTransaction)BeginDbTransaction(IsolationLevel.Unspecified);
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) {
            return sqliteConn.BeginTransaction(isolationLevel); //new PxTransaction(sqliteConn.BeginTransaction(isolationLevel));
        }

        public override void ChangeDatabase(string databaseName) {
            throw new NotImplementedException();
        }

        public override void Close() {
            this.Dispose();
            
        }

        public override string ConnectionString {
            //TODO: define connection string
            get {
                return connectionstring;
            }
            set {
                connectionstring = value;
            }
        }

        protected override DbCommand CreateDbCommand() {
            PcAxisCommand comm = new PcAxisCommand(sqliteConn);//this.sqliteConn.CreateCommand();
            comm.PcAxisConnection = this;
            return comm;
        }

        public override string DataSource {
            get { throw new NotImplementedException(); }
        }

        public override string Database {
            get { throw new NotImplementedException(); }
        }

        public override string ServerVersion {
            get { throw new NotImplementedException(); }
        }

        public override ConnectionState State {
            get { return this.sqliteConn.State; }
        }


    }

}
