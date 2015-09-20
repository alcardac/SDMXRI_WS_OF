// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingSqliteTabularWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Re-Using example for <see cref="SqliteTabularWriter" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.DataWriting
{
    using System;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    using Estat.Sri.TabularWriters.Engine;

    /// <summary>
    ///     Re-Using example for <see cref="SqliteTabularWriter" />
    /// </summary>
    public static class ReUsingSqliteTabularWriter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            const int ColKeyCount = 10;
            const int ColAttributeCount = 5;
            const int Records = 1000000;
            const string TestSqlite = "test.sqlite";
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();

                if (File.Exists(TestSqlite))
                {
                    File.Delete(TestSqlite);
                }

                DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SQLite");

                using (DbConnection connection = dbProviderFactory.CreateConnection())
                {
                    if (connection != null)
                    {
                        connection.ConnectionString = "Data Source=" + TestSqlite + ";Version=3;";
                        using (var tabular = new SqliteTabularWriter(connection, "test_table"))
                        {
                            // write header i.e. create the table
                            tabular.StartColumns();
                            for (int i = 0; i < ColKeyCount; i++)
                            {
                                tabular.WriteColumnKey(string.Format(CultureInfo.InvariantCulture, "key{0:00}", i));
                            }

                            for (int i = 0; i < ColAttributeCount; i++)
                            {
                                tabular.WriteColumnAttribute(string.Format(CultureInfo.InvariantCulture, "attr{0:00}", i));
                            }

                            tabular.WriteColumnMeasure("measure");

                            // write data
                            for (int r = 0; r < Records; r++)
                            {
                                tabular.StartRecord();
                                for (int i = 0; i < ColKeyCount; i++)
                                {
                                    tabular.WriteCellKeyValue(string.Format(CultureInfo.InvariantCulture, "keycell{0:00}-{1:00}", r, i));
                                }

                                for (int i = 0; i < ColAttributeCount; i++)
                                {
                                    tabular.WriteCellAttributeValue(string.Format(CultureInfo.InvariantCulture, "attrcell{0:00}-{1:00}", r, i));
                                }

                                tabular.WriteCellMeasureValue(r.ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine("Time for {0} records: {1}", Records, stopwatch.Elapsed);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                throw;
            }
        }

        #endregion
    }
}