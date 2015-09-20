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
    public class PcAxisFactory : DbProviderFactory {
        // Fields
        public static readonly PcAxisFactory Instance = new PcAxisFactory();

        // Methods
        public override DbCommand CreateCommand() {
            //return new PcAxisCommand(); //new SQLiteCommand();
            throw new NotImplementedException("Use DbConnection.CreateCommand() instead.");
        }

        public override DbCommandBuilder CreateCommandBuilder() {
            return new SQLiteCommandBuilder();
        }

        public override DbConnection CreateConnection() {
            return new PcAxisConnection();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder() {
            return new PcAxisConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter() {
            return new SQLiteDataAdapter();
        }

        public override DbParameter CreateParameter() {
            return new SQLiteParameter();
        }

    }
}
