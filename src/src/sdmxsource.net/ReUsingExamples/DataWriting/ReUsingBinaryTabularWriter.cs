// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingBinaryTabularWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Re-Using example for <see cref="BinaryTabularWriter" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.DataWriting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    using Estat.Sri.TabularWriters.Engine;

    /// <summary>
    ///     Re-Using example for <see cref="BinaryTabularWriter" />
    /// </summary>
    public static class ReUsingBinaryTabularWriter
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
            const int Columns = ColKeyCount + ColAttributeCount + 1;
            const int Records = 1000000;
            const string TestFile = "test.bin";

            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();

                if (File.Exists(TestFile))
                {
                    File.Delete(TestFile);
                }

                var index = new List<long>();
                using (FileStream fileStream = File.OpenWrite(TestFile))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream))
                    {
                        ITabularWriter tabular = new BinaryTabularWriter(binaryWriter, index);

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

                        tabular.Close();
                        stopwatch.Stop();
                        Console.WriteLine("Time for {0} records: {1}", tabular.TotalRecordsWritten, stopwatch.Elapsed);
                    }
                }

                // reading the contents example
                stopwatch.Reset();
                stopwatch.Start();

                // load a random record
                var random = new Random();
                int row = random.Next(Records);

                // get row page
                int page = row >> 10;

                // get page start end offsets
                long startDataOffSet = index[page];
                long endDataOffSet = index[page + 1];

                // load the page into a memory buffer
                FileStream fileReader = File.OpenRead(TestFile);
                byte[] buffer;
                using (var dataFile = new BinaryReader(fileReader))
                {
                    dataFile.BaseStream.Seek(startDataOffSet, SeekOrigin.Begin);
                    buffer = dataFile.ReadBytes((int)(endDataOffSet - startDataOffSet));
                }

                // get row from memory buffer
                int rowOffset = row - (page << 10);
                using (var memoryStream = new MemoryStream(buffer, false))
                {
                    using (var memoryReader = new BinaryReader(memoryStream))
                    {
                        int currentLine = 0;
                        var record = new string[Columns];
                        while (currentLine <= rowOffset)
                        {
                            for (int i = 0; i < Columns; i++)
                            {
                                record[i] = memoryReader.ReadString();
                            }

                            currentLine++;
                        }

                        stopwatch.Stop();
                        Console.WriteLine("Requested row {0} in {2}, got record: {1}", row, string.Join(",", record), stopwatch.Elapsed);
                    }
                }
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