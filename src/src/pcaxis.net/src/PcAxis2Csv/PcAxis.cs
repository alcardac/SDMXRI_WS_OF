using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SQLite;
using org.estat.PcAxis.PcAxisProvider;
using System.Data;

namespace org.estat.PcAxis
{
    public class PcAxis
    {
        #region private fields
        private String pcaxisFile;
        private static Dictionary<char, Regex> periodTranscodingCollection;
        private static Dictionary<char, string> periodReplaceStringCollection;
        #endregion
        #region Constructors
        public PcAxis(String pcaxisFile)
        {
            this.pcaxisFile = pcaxisFile;
            if (periodTranscodingCollection == null)
            {
                periodTranscodingCollection = new Dictionary<char, Regex>();
                periodReplaceStringCollection = new Dictionary<char, string>();
                // monthly
                periodTranscodingCollection.Add('M', new Regex("(?<year>[0-9]{4})M?(?<period>[0-9]{2})", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));
                periodReplaceStringCollection.Add('M', "${year}-${period}");
                // weekly
                periodTranscodingCollection.Add('W', new Regex("(?<year>[0-9]{4})W?((0?(?<period>[1-9]))|(?<period>[1-5][0-9]))", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));
                periodReplaceStringCollection.Add('W', "${year}-W${period}");
                // half year
                periodTranscodingCollection.Add('H', new Regex("(?<year>[0-9]{4})H?(?<period>[1-2])", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));
                periodReplaceStringCollection.Add('H', "${year}-B${period}");
                // quarterly
                periodTranscodingCollection.Add('Q', new Regex("(?<year>[0-9]{4})Q?(?<period>[1-4])", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));
                periodReplaceStringCollection.Add('Q', "${year}-Q${period}");
            }
        }
        #endregion
        #region private methods
        //private string fixHeader(string csvHeader,Regex regex)
        //{
        //    List<string> header = new List<string>(regex.Split(csvHeader.Substring(1, csvHeader.Length - 2)));
        //    Regex r = new Regex("[ ,;()=\\\\/#!%$@]",RegexOptions.CultureInvariant);
        //    for (int i = 0; i < header.Count; i++)
        //    {
        //        header[i] = r.Replace(header[i], "");
        //        header[i] = '"' + header[i].Substring(0, Math.Min(header[i].Length, 50)) + '"';
        //    }
        //    string returnValue = String.Join(";", header.ToArray());
        //    header.Clear();
        //    return returnValue;
        //}
        #endregion
        #region Public methods
        //public void convert2csv(TextWriter writer)
        //{
        //    StreamReader reader = new StreamReader(pcaxisFile);

        //    /*
        //    * String to hold PC-AXIS records.
        //    */
        //    String record = "";
        //    /*
        //     * String to hold PC-AXIS keyword of each record.
        //     */
        //    String keyword = "";
        //    /*
        //     * Strings to hold info for CSV header line.
        //     */
        //    String heading = "";
        //    String stub = "";
        //    String contents = null;
        //    /*
        //     * ArrayList that will include all ArrayLists that we
        //     * need to build tuples for.
        //     */
        //    ArrayList listsForTuples = new ArrayList();

        //    Dictionary<String, ArrayList> values = new Dictionary<String, ArrayList>();
        //    Dictionary<String, ArrayList> codes = new Dictionary<String, ArrayList>();
        //    Dictionary<String, String> keys = new Dictionary<String, String>();
        //    List<String> variablesList = new List<String>();
        //    List<String> stubList = new List<String>();
        //    List<String> headingList = new List<String>();

        //    Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    Regex extractValues = new Regex("\"(?:,\")?");
        //    while (!reader.EndOfStream)
        //    {
        //        /*
        //        * The following is required because we are not sure if a delimiter
        //        * has been used at the end of each line for the DATA record.
        //         * Later on, we use \n as a delimiter.
        //        */
        //        string variable = null;
        //        string val = null;
        //        record += '\n' + reader.ReadLine().Trim();
        //        if (record.EndsWith(";", StringComparison.InvariantCulture))
        //        {
        //            /*
        //             * Record complete. Trim unecessary whitespace.
        //             */
        //            record = record.Trim();
        //            Match m = extractRecord.Match(record);
        //            keyword = m.Groups["keyword"].Value;
        //            variable = m.Groups["var"].Value.Trim();
        //            val = m.Groups["value"].Value.Trim();
        //            if (keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                contents = val;
        //            }
        //            else if (keyword.Equals("STUB", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                stub = val;
        //                stubList.AddRange(stub.Split(','));
        //            }
        //            else if (keyword.Equals("HEADING", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                heading = val;
        //                headingList.AddRange(heading.Split(','));
        //            }
        //            else if (!String.IsNullOrEmpty(variable) && !keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                val=val.Replace("\n", "");

        //                ArrayList tmlList = new ArrayList();

        //                if (keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    string tList = m.Groups["tlist"].Value;
        //                    if (!tList.Equals("A1", StringComparison.InvariantCultureIgnoreCase) &&
        //                        !tList.Equals("H1", StringComparison.InvariantCultureIgnoreCase) &&
        //                        !tList.Equals("Q1", StringComparison.InvariantCultureIgnoreCase) &&
        //                        !tList.Equals("M1", StringComparison.InvariantCultureIgnoreCase) &&
        //                        !tList.Equals("W1", StringComparison.InvariantCultureIgnoreCase))
        //                    {
        //                        val = buildTimeVal(tList);
        //                    }
        //                }
        //                tmlList.AddRange(extractValues.Split(val.Substring(1, val.Length - 2)));
        //                //tmlList.AddRange(val.Split(','));
        //                if (keyword.Equals("CODES", StringComparison.InvariantCultureIgnoreCase) ||
        //                    keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    codes.Add(variable, tmlList);
        //                }
        //                else if (keyword.Equals("VALUES", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    values.Add(variable, tmlList);
        //                }

        //            }
        //            else if (keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                /*
        //             * Build list of keys used in the file.
        //             */
        //                keys.Add(variable, val);
        //            }
        //            if (keyword.Equals("DATA", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                record = record.Substring(0, record.Length - 1);
        //            }
        //            else
        //            {
        //                record = "";
        //            }
        //        }
        //    }

        //    char separator = ',';
        //    /*
        // * Prepare CSV header String and write it.
        // */
        //    if (!String.IsNullOrEmpty(stub))
        //    {
        //        stub += separator;
        //    }
        //    if (!String.IsNullOrEmpty(heading))
        //    {
        //        heading += separator;
        //    }
        //    if (contents == null)
        //    {
        //        Console.WriteLine("NO CONTENTS RECORD DEFINED! MANDATORY!");
        //    }
           
        //    String csvHeaderString = stub + heading + contents;
        //    writer.Write(fixHeader(csvHeaderString,extractValues) + '\n');
        //    /*
        //     * Remove contents from CSV header in order to build up a varaibles List.
        //     */
        //    String variablesString = csvHeaderString.Substring(0, csvHeaderString.LastIndexOf("\",\"") + 1);

        //    variablesList.AddRange(variablesString.Split(',')); // TODO: Take into account that STUB or HEADING values may include commas (,). 

        //    /* 
        //     * Go through keys to identify whether values or codes are going to be used
        //     * for the tuples. If no keys defined, codes are prefered, unless only values
        //     * are defined.
        //     */
        //    bool keysUsed = false;
        //    if (keys.Count > 0)
        //    {
        //        /* 
        //         * This means that DATA will contain
        //         * a sparse table, thus including values/codes
        //         * for the STUB in front of the observations.
        //         * So, we need the tuples for the rest of the
        //         * variables, i.e. the HEADING (rows).
        //         */
        //        foreach (String headingVariable in headingList)
        //        {
        //            ArrayList variableItems = null;
        //            if (!codes.TryGetValue(headingVariable, out variableItems))
        //            {
        //                if (values.TryGetValue(headingVariable, out variableItems))
        //                {
        //                    listsForTuples.Add(variableItems);
        //                }
        //                else
        //                {
        //                    Console.WriteLine("NO ITEMS FOUND FOR: " + headingVariable);
        //                }
        //            }
        //        }
        //        keysUsed = true;
        //    }
        //    else
        //    {
        //        /*
        //         * All values MUST exist in DATA record.
        //         * VALUES or CODES should be taken for each variable.
        //         */
        //        foreach (String variable in variablesList)
        //        {
        //            if (codes.Count > 0)
        //            {
        //                listsForTuples.Add(codes[variable]);
        //            }
        //            else if (values.Count > 0)
        //            {
        //                listsForTuples.Add(values[variable]);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Nor CODES or VALUES found for variable: " + variable);
        //            }
        //        }
        //    }

        //    /* Go through DATA section and gather values.
        //     * In case of full set of values, a simple list of values will be built.
        //     * In case of sparse table of values, a map will be built based on the key.
        //     */
        //    string[] dataList = null;
        //    // Remove \n put before entry by trimming.
        //    record = record.Trim();
        //    if (record.StartsWith("DATA="))
        //    {
        //        /* Also trim the records, in order to eliminate extra new lines
        //         * added in the process of building the entry.
        //         */
        //        if (keysUsed)
        //        {
        //            /* 
        //             * Taken from "PC-AXIS File Format" (i.e. ESYSPX2008.DOC) document:
        //             * "For every variable in the stub is indicated the value for the variable 
        //             * within quotation marks, comma separated, followed by all datacells for 
        //             * that row (no quotation marks, space separated)."
        //             * 
        //             * Each row in the dataList will include the Key and the values, i.e. one line
        //             * of the PC-AXIS file DATA record.
        //             */
        //            dataList = record.Substring(5).Trim().Split('\n');
        //            //				dataList = record.substring(5).trim().split("[,|\n]");
        //        }
        //        else
        //        {
        //            /*
        //             * Taken from "PC-AXIS File Format" (i.e. ESYSPX2008.DOC) document:
        //             * 
        //             * "In most cases the cells are written with one record per line in the table. 
        //             * The number of records will thus be determined by the number of values in the stub. 
        //             * The length of the records depends upon the number of values in the heading, 
        //             * and also upon the size of figures.
        //             * 
        //             * Table cells that contain a dash, one, two,  three, four, five or six dots 
        //             * should be within quotation marks.
        //             * 
        //             * For data without keys it is possible to write all cells in just one record, 
        //             * terminated by a semicolon.
        //             * 
        //             * PC-Axis accepts the delimiters comma, space, semicolon, tabulator. The different 
        //             * delimiters are synonyms and can be mixed in the file. 
        //             * Recommended delimiter is space."
        //             */
        //            dataList = Regex.Split(record.Substring(5).Trim(), "[ |;|,|\n|\t]");
        //        }
        //    }

        //    /*
        //     * Depending on whether KEYS are used or not (sparse or full table included in DATA),
        //     * the respective method is followed in order to write the data table into CSV format.
        //     */
        //    if (keysUsed)
        //    {
        //        /*
        //         * KEYS are used, thus the prefix of each record from the DATA section
        //         * will be copied directly to the CSV.
        //         */
        //        int entriesPerDataLine = stubList.Count + 1;
        //        String line = "";
        //        String linePrefix = "";
        //        for (int i = 0; i < dataList.Length; i++)
        //        {
        //            /*
        //             * Split key from data segment per line, e.g. the line:
        //             * "19","1980","1","31",820 98 144
        //             * will be split into:
        //             * "19","1980","1","31",
        //             * and:
        //             * 820 98 144
        //             */
        //            int split = dataList[i].LastIndexOf(',') + 1;
        //            linePrefix = dataList[i].Substring(0, split);
        //            String[] dataPartialList = dataList[i].Substring(split).Split(' ');
        //            int j = 0;
        //            /*
        //             * TupleIterator will return tuples for HEADING variables, i.e. for the
        //             * second part of the split:
        //             * 820 98 144
        //             */
        //            for (IEnumerator iter = new TupleIterator(listsForTuples); iter.MoveNext(); j++, line = "")
        //            {
        //                /*
        //                 * Re-encapsulate values into double-quotes.
        //                 */
        //                foreach (object obj in (ArrayList)iter.Current)
        //                {
        //                    line += '"' + (String)obj + "\"" + separator;
        //                }
        //                /*
        //                 * For values not in double-quotes, add double-quotes.
        //                 */
        //                if (!dataPartialList[j].StartsWith("\""))
        //                {
        //                    dataPartialList[j] = '"' + dataPartialList[j] + '"';
        //                }
        //                /*
        //                 * Write the line to the CSV.
        //                 */
        //                writer.Write((linePrefix + line + dataPartialList[j]).Replace("\",\"","\";\"") + "\n");
        //            }

        //            //				if ((i + 1) % entriesPerDataLine == 0) {
        //            //					String[] dataPartialList = dataList[i].split(" ");
        //            //					int j = 0;
        //            //					for (Iterator iter = new TupleIterator(listsForTuples); iter.hasNext(); ) {
        //            //						for (Object object : (ArrayList) iter.next()) {
        //            //							line += '"' + (String) object + "\",";
        //            //						}
        //            //						if (!dataPartialList[j].startsWith("\"")) {
        //            //							dataPartialList[j] = '"' + dataPartialList[j] + '"';
        //            //						}
        //            //						writer.write(linePrefix + line + dataPartialList[j] + "\n");
        //            //						j++;
        //            //						line = "";
        //            //					}
        //            //					linePrefix = "";
        //            //				}else {
        //            //					linePrefix += dataList[i] + ',';
        //            //				}
        //        }
        //    }
        //    else
        //    {
        //        /* 
        //         * TupleIterator will process all Lists included into collections,
        //         * in an ordered manner, and return an Iterator of tuples (in ArrayLists).
        //         */
        //        int i = 0;
        //        String line = "";
        //        for (IEnumerator iter = new TupleIterator(listsForTuples); iter.MoveNext(); )
        //        {
        //            line = "";
        //            /* 
        //             * Each item in the TupleIterator is an ArrayList including all variable values.
        //             * Thus, we can iterate through it and get these values.
        //             */
        //            foreach (object obj in (ArrayList)iter.Current)
        //            {
        //                line += '"' + (String)obj + "\"" + separator;
        //            }
        //            /* 
        //             * Check if values are already included in double quotes.
        //             * If not, encapsulate them into double quotes.
        //             */
        //            if (!dataList[i].StartsWith("\""))
        //            {
        //                dataList[i] = '"' + dataList[i] + '"';
        //            }
        //            writer.Write((line + dataList[i]).Replace("\",\"","\";\"") + "\n");
        //            i++;
        //        }
        //    }

        //    /*
        //     * Close streams and writers/readers.
        //     */
        //    writer.Close();
        //    reader.Close();
        //}

        private string handleTimeVal(string val, string tList)
        {
            if (!tList.Equals("A1", StringComparison.InvariantCultureIgnoreCase) &&
                                !tList.Equals("H1", StringComparison.InvariantCultureIgnoreCase) &&
                                !tList.Equals("Q1", StringComparison.InvariantCultureIgnoreCase) &&
                                !tList.Equals("M1", StringComparison.InvariantCultureIgnoreCase) &&
                                !tList.Equals("W1", StringComparison.InvariantCultureIgnoreCase))
            {
                val = buildTimeVal(tList);
            }
            else
            {
                val =transcodeTimeOutsideTlist(val, tList);
            }
            return val;
        }

        private void trancodeValueList(List<string> values, string tlist)
        {
            char freq = tlist[0];
            for(int i=0;i < values.Count; i++)
            {
                values[i] = transcodeTimeOutsideTlist(values[i], tlist);
            }
        }

        private string transcodeTimeOutsideTlist(string record, string tlist)
        {
            char freq = tlist[0];
            Regex periodRegex = null;
            if (periodTranscodingCollection.TryGetValue(freq,out periodRegex))
            {
                return periodTranscodingCollection[freq].Replace(record,periodReplaceStringCollection[freq]);
            }
#if false
            switch (tlist[0])
            {
                case 'A': // no change is needed
                    return record;
                case 'M': // 2000M01 or 200001 to 2000-01
                    return Regex.Replace(record, "(?<year>[0-9]{4})M?(?<period>[0-9]{2})", "${year}-${period}");
                case 'W': // 2000W01 to 2000-W1
                    return Regex.Replace(record, "(?<year>[0-9]{4})W?((0(?<period>[1-9]))|(?<period>[1-5][0-9]))", "${year}-W${period}");
                case 'H': // 2000H1 to 2000-B1
                    return Regex.Replace(record, "(?<year>[0-9]{4})H?(?<period>[1-2])", "${year}-B${period}");
                case 'Q': // 2000Q1 to 2000-Q1
                    return Regex.Replace(record, "(?<year>[0-9]{4})Q?(?<period>[1-4])", "${year}-Q${period}");
            }
#endif
            return record;
        }

        private string buildTimeVal(string record)
        {
            /*
          * Get the TLIST format, i.e. A1, H1, Q1, M1, W1
          */
            String timeFormat = record.Trim().Substring(0, 2).ToUpper();

            /*
             * Extract starting and ending dates.
             */
            int dash = record.IndexOf('-');
            int comma = record.IndexOf(',');
            String startDate = record.Substring(comma + 1, dash - comma -1 ).Trim();
            String endDate = record.Substring(dash + 1).Trim();
            StringBuilder timeVal = new StringBuilder();

            int fromYear = Convert.ToInt32(startDate.Substring(1, 4));
            int toYear = Convert.ToInt32(endDate.Substring(1, 4));
            int fromSuffix = 0;
            int toSuffix = 0;
            int suffixLength = 0;
            string periodFormat = "";
            switch (timeFormat[0])
            {
                case 'A':
                    fromSuffix = 1;
                    toSuffix = 1;
                    suffixLength = 1;
                    periodFormat = "{0}";
                    break;
                case 'H':
                    fromSuffix = Convert.ToInt32(startDate.Substring(5, 1));
                    toSuffix = Convert.ToInt32(endDate.Substring(5, 1));
                    suffixLength = 2;
                    periodFormat = "{0}-B{1}";
                    break;
                case 'Q':
                    fromSuffix = Convert.ToInt32(startDate.Substring(5, 1));
                    toSuffix = Convert.ToInt32(endDate.Substring(5, 1));
                    suffixLength = 4;
                    periodFormat = "{0}-Q{1}";
                    break;
                case 'M':
                    fromSuffix = Convert.ToInt32(startDate.Substring(5, 2));
                    toSuffix = Convert.ToInt32(endDate.Substring(5, 2));
                    suffixLength = 12;
                    periodFormat = "{0}-{1:00}";
                    break;
                case 'W':
                    fromSuffix = Convert.ToInt32(startDate.Substring(5, 2));
                    toSuffix = Convert.ToInt32(endDate.Substring(5, 2));
                    suffixLength = 54;
                    periodFormat = "{0}-W{1}";
                    break;
                default:
                    // unsupported time format
                    // Do nothing
                    return record;

            }
            // same year
            if (fromYear == toYear)
            {
                suffixLength = toSuffix;
            }
            for (int suffixIndex = fromSuffix /*+ 1 */; suffixIndex <= suffixLength; suffixIndex++)
            {
                timeVal.Append('"');
                timeVal.AppendFormat(periodFormat, fromYear, suffixIndex);
                timeVal.Append("\",");
            }
            /*
             * Fill in between years.
             */
            for (int i = fromYear + 1; i < toYear; i++)
            {
                for (int suffixIndex = 1; suffixIndex <= suffixLength; suffixIndex++)
                {
                    timeVal.Append('"');
                    timeVal.AppendFormat(periodFormat, i, suffixIndex);
                    timeVal.Append("\",");
                }
            }
            /*
             * Complete ending year.
             */
            if (fromYear < toYear)
            {
                for (int suffixIndex = 1; suffixIndex < toSuffix; suffixIndex++)
                {
                    timeVal.Append('"');
                    timeVal.AppendFormat(periodFormat, toYear, suffixIndex);
                    timeVal.Append("\",");
                }
                timeVal.Append('"');
                timeVal.AppendFormat(periodFormat, toYear,toSuffix);
                timeVal.Append('"');
                
            }
            else
            {
                // remove the coma
                timeVal.Length = timeVal.Length - 1;
            }
            return timeVal.ToString();
        }

#if false // not used anymore to be deleted
        private String padSuffix(int index, int length)
        {
            return index.ToString(new string('0', length), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            //String suffix = index.ToString( );
            ///*
            // * Padding for Months and Weeks that are one digit.
            // */
            //if (length > 10 && suffix.Length < 2)
            //{
            //    suffix = '0' + suffix;
            //}
            // MAT-268
            //return "-" + suffix;
        }
#endif
        #endregion


        #region New PcAxis Provider

        Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //@aris: maybe the following regex is more correct as there might be a ';' character in the values list within double quotes (")
        //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>.*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        /// <summary>
        /// The number of rows to show when <see cref="PcAxisConnection.DataPreviewOnly"/> is set to true
        /// </summary>
        public int DataPreviewRows {
            get;
            set;
        }
        public DataTable GetCodes(string[] columns) {
            StreamReader reader = new StreamReader(pcaxisFile);

            /*
            * String to hold PC-AXIS records.
            */
            String record = "";
            /*
             * String to hold PC-AXIS keyword of each record.
             */
            String keyword = "";
            /*
             * Strings to hold info for CSV header line.
             */
            String heading = "";
            String stub = "";
            String contents = null;

            // hold the TIMEVAL(field) field.
            string timeVal = null;

            // hold the TLIST frequency
            char freq = '\0';

            // hold the position of time val in key
            int timeValKeyIdx = -1;


            string contVariable = "";

            //holds all the keywords to be used as columns
            Dictionary<string, ColumnInformation> keywordsUsed = new Dictionary<string, ColumnInformation>();

            /*
             * ArrayList that will include all ArrayLists that we
             * need to build tuples for.
             */

            Dictionary<String, List<string>> values = new Dictionary<String, List<string>>();
            Dictionary<String, List<string>> codes = new Dictionary<String, List<string>>();
            Dictionary<String, String> keys = new Dictionary<String, String>();

            //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            bool stopreading = false;
            while (!reader.EndOfStream && !stopreading) {
                /*
			    * The following is required because we are not sure if a delimiter
			    * has been used at the end of each line for the DATA record.
    			 * Later on, we use \n as a delimiter.
			    */
                string variable = null;
                string val = null;
                record += '\n';
                string firstSection = GetFirstLineSection(reader);
                if (firstSection.ToLower() == "data=") {
                    stopreading = true;
                    break;
                }

                if (!reader.EndOfStream)
                    record += firstSection + reader.ReadLine().Trim();
                else
                    record += firstSection;

                if (record.EndsWith(";", StringComparison.InvariantCulture)) {
                    /*
                     * Record complete. Trim unecessary whitespace.
                     */
                    record = record.Trim();
                    Match m = extractRecord.Match(record);
                    keyword = m.Groups["keyword"].Value;
                    variable = m.Groups["var"].Value.Trim().Replace("\"","");
                    val = m.Groups["value"].Value.Trim();
                    if (keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)) {
                        contents = val;
                    } else if (keyword.Equals("STUB", StringComparison.InvariantCultureIgnoreCase)) {
                        stub = val;
                    } else if (keyword.Equals("HEADING", StringComparison.InvariantCultureIgnoreCase)) {
                        heading = val;
                    } else if (keyword.Equals("CONTVARIABLE", StringComparison.InvariantCultureIgnoreCase)) {
                        contVariable = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                    } else if (record.StartsWith(keyword + "=") && (
                               keyword.Equals("MATRIX", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-CODE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-AREA", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DECIMALS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TITLE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UPDATE-FREQUENCY", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TABLE_ID", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SOURCE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DATABASE", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                        )
                        ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Value = val;
                            keywordsUsed[keyword] = ci;
                        }

                    } else if (!string.IsNullOrEmpty(contVariable) && record.StartsWith(keyword + "(") && (
                                keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                             || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                             || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase)
                         )
                         ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                            keywordsUsed[keyword] = ci;
                        } else {
                            ColumnInformation ci = keywordsUsed[keyword];
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                        }
                    } else if (!String.IsNullOrEmpty(variable) && !keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        val = val.Replace("\n", "");

                        List<string> tmlList = new List<string>();

                        if (keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            string tList = m.Groups["tlist"].Value;
                            val = handleTimeVal(val, tList);
                            timeVal = variable;
                            freq = tList[0];
                            //if (!tList.Equals("A1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("H1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("Q1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("M1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("W1", StringComparison.InvariantCultureIgnoreCase)) {
                            //    val = buildTimeVal(tList);
                            //}

                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = "FREQ";
                            ci.Value = freq.ToString();
                            keywordsUsed["FREQ"] = ci;

                            ci = new ColumnInformation();
                            ci.ColumnName = "TIME_FORMAT";
                            ci.Value = TransformFREQToSDMXFormat(freq.ToString());
                            keywordsUsed["TIME_FORMAT"] = ci;

                        }
                        tmlList.AddRange(GetCommaSeperatedValues(val));
                        //if (val[0] == '"') {
                        //    tmlList.AddRange(extractValues.Split(val.Substring(1, val.Length - 2)));
                        //} else {
                        //    tmlList.AddRange(extractValues.Split(val));
                        //}
                        //tmlList.AddRange(val.Split(','));
                        if (keyword.Equals("CODES", StringComparison.InvariantCultureIgnoreCase) ||
                            keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            codes.Add(variable, tmlList);
                        } else if (keyword.Equals("VALUES", StringComparison.InvariantCultureIgnoreCase)) {
                            values.Add(variable, tmlList);
                        }

                    } else if (keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        /*
					 * Build list of keys used in the file.
					 */
                        keys.Add(variable, val);
                        if (variable.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase))
                        {
                            timeValKeyIdx = keys.Count - 1;
                        }
                    }
                    if (keyword.Equals("DATA", StringComparison.InvariantCultureIgnoreCase)) {
                        record = record.Substring(0, record.Length - 1);
                    } else {
                        record = "";
                    }
                }
            }


            ArrayList listsForTuples = new ArrayList();

            foreach (string column in columns) {

                string codesOrValues = "";
                foreach (string col in keys.Keys) {
                    if (FormatColumnHeader(col).Replace("\"", "") == column) {
                        if (col.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase) && keys[col].Equals("VALUES"))
                        {
                            if (values.ContainsKey(col))
                            {
                                trancodeValueList(values[col], freq.ToString());
                            }
                            else
                            {
                                codesOrValues = "CODES";
                                break;
                            }
                        }
                        codesOrValues = keys[col];
                        break;
                    }
                }

                string key = "";
                bool found = false;
                if (codesOrValues != "VALUES") {
                    foreach (string col in codes.Keys) {
                        if (FormatColumnHeader(col).Replace("\"", "") == column) {
                            key = col;
                            break;
                        }
                    }
                    if (key != "") {
                        listsForTuples.Add(codes[key]);
                        found = true;
                    }
                }

                if (key == "") {
                    foreach (string col in values.Keys) {
                        if (FormatColumnHeader(col).Replace("\"", "") == column) {
                            key = col;
                            break;
                        }
                    }

                    if (key != "") {
                        listsForTuples.Add(values[key]);
                        found = true;
                    }
                }

                foreach (string k in keywordsUsed.Keys) {
                    if (keywordsUsed[k].ColumnName == column) {
                        List<string> returnValues = new List<string>();
                        if (!string.IsNullOrEmpty(contVariable) && keywordsUsed[k].IsContVar) {
                            foreach (string cval in keywordsUsed[k].Values.Values) {
                                string contvarvalue = cval;
                                List<string> codevalues = null;
                                List<string> valuevalues = null;
                                if (keys.Count <= 0 || keys.Count > 0 && keys[contVariable] == "CODES") {
                                    if (codes.TryGetValue(contVariable, out codevalues) && values.TryGetValue(contVariable, out valuevalues)) {
                                        for (int l = 0; l < codes.Count; l++) {
                                            if (valuevalues.Count > l && valuevalues[l].ToString() == cval) {
                                                contvarvalue = codevalues[l].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                                if(!returnValues.Contains(contvarvalue))
                                    returnValues.Add(contvarvalue);
                            }
                        } else {
                            returnValues.Add(keywordsUsed[k].Value);
                        }
                        listsForTuples.Add(returnValues);
                        found = true;
                        break;
                    }
                }

                if (!found && column.ToLower() == "obs_status") {

                    List<string> retval = getAllDotsValuesInDataSection(reader);
                    listsForTuples.Add(retval);
                }

            }

            DataTable dt = new DataTable();
            foreach (string column in columns) {
                DataColumn dc = new DataColumn(column);
                dt.Columns.Add(dc);
            }
            int j = 0;
            for (IEnumerator iter = new TupleIterator(listsForTuples); iter.MoveNext(); j++) {
                DataRow dr = dt.NewRow();
                int i=0;
                foreach (object obj in (ArrayList)iter.Current) {
                    dr[i] = obj.ToString();
                    i++;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static List<string> getAllDotsValuesInDataSection(StreamReader reader) {
            List<string> thevalues = new List<string>();

            string value = "";
            char[] chars = new char[1];
            char[] oldchars = new char[1];
            while (!reader.EndOfStream) {
                reader.Read(chars, 0, 1);
                if (chars[0] == ',' || chars[0] == ' ' || chars[0] == ';' || chars[0] == '\n' || chars[0] == '\t') {
                    if(value!="")
                        thevalues.Add(value.Replace("\"", ""));
                    value = "";
                } else if (chars[0] != '\r') {
                    value += chars[0];
                }
                oldchars[0] = chars[0];
            }

            List<string> retval = new List<string>();
            foreach (string v in thevalues) {
                string valtoadd = "";
                if (v == "\".\"" ||
                    v == ".") {
                    valtoadd = ".";
                }
                if (v == "\"..\"" ||
                    v == "..") {
                    valtoadd = "..";
                }
                if (v == "\"...\"" ||
                    v == "...") {
                    valtoadd = "...";
                }
                if (v == "\"....\"" ||
                    v == "....") {
                    valtoadd = "....";
                }
                if (v == "\".....\"" ||
                    v == ".....") {
                    valtoadd = ".....";
                }
                if (v == "\"......\"" ||
                    v == "......") {
                    valtoadd = "......";
                }
                if (v == "\".......\"" ||
                    v == ".......") {
                    valtoadd = ".......";
                }
                if (v == "\"-\"" ||
                    v == "-") {
                    valtoadd = "-";
                }
                if (valtoadd != "" && !retval.Contains(valtoadd))
                    retval.Add(valtoadd);
            }
            retval.Add("A");
            //List<string> retval = new List<string>();
            //retval.Add(".");
            //retval.Add("..");
            //retval.Add("...");
            //retval.Add("....");
            //retval.Add(".....");
            //retval.Add("......");
            //retval.Add(".......");
            //retval.Add("-");
            return retval;
        }

        public Dictionary<String,Dictionary<String,String>> GetCodeValueMap(string[] columns) {
            StreamReader reader = new StreamReader(pcaxisFile);

            /*
            * String to hold PC-AXIS records.
            */
            String record = "";
            /*
             * String to hold PC-AXIS keyword of each record.
             */
            String keyword = "";
            /*
             * Strings to hold info for CSV header line.
             */
            String heading = "";
            String stub = "";
            String contents = null;

            // hold the TIMEVAL(field) field.
            string timeVal = null;

            // hold the TLIST frequency
            char freq = '\0';

            // hold the position of time val in key
            int timeValKeyIdx = -1;


            string contVariable = "";

            //holds all the keywords to be used as columns
            Dictionary<string, ColumnInformation> keywordsUsed = new Dictionary<string, ColumnInformation>();

            /*
             * ArrayList that will include all ArrayLists that we
             * need to build tuples for.
             */

            Dictionary<String, List<string>> values = new Dictionary<String, List<string>>();
            Dictionary<String, List<string>> codes = new Dictionary<String, List<string>>();
            Dictionary<String, String> keys = new Dictionary<String, String>();

            Dictionary<string, string> datasymbolDescriptions = new Dictionary<string, string>();

            //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            bool stopreading = false;
            while (!reader.EndOfStream && !stopreading) {
                /*
			    * The following is required because we are not sure if a delimiter
			    * has been used at the end of each line for the DATA record.
    			 * Later on, we use \n as a delimiter.
			    */
                string variable = null;
                string val = null;
                record += '\n';
                string firstSection = GetFirstLineSection(reader);
                if (firstSection.ToLower() == "data=") {
                    stopreading = true;
                    break;
                }

                if (!reader.EndOfStream)
                    record += firstSection + reader.ReadLine();
                else
                    record += firstSection;

                if (record.EndsWith(";", StringComparison.InvariantCulture)) {
                    /*
                     * Record complete. Trim unecessary whitespace.
                     */
                    record = record.Trim();
                    Match m = extractRecord.Match(record);
                    keyword = m.Groups["keyword"].Value;
                    variable = m.Groups["var"].Value.Trim().Replace("\"", "");
                    val = m.Groups["value"].Value.Trim();
                    if (keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)) {
                        contents = val;
                    }
                    else if (keyword.Equals("STUB", StringComparison.InvariantCultureIgnoreCase)) {
                        stub = val;
                    }
                    else if (keyword.Equals("HEADING", StringComparison.InvariantCultureIgnoreCase)) {
                        heading = val;
                    }
                    else if (keyword.Equals("CONTVARIABLE", StringComparison.InvariantCultureIgnoreCase)) {
                        contVariable = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                    }
                    else if (record.StartsWith(keyword + "=") && (
                             keyword.Equals("MATRIX", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-CODE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-AREA", StringComparison.InvariantCultureIgnoreCase)
                          || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DECIMALS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TITLE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase)
                          || keyword.Equals("UPDATE-FREQUENCY", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TABLE_ID", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SOURCE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DATABASE", StringComparison.InvariantCultureIgnoreCase)
                          || keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)
                          || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                          || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                      )
                      ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Value = val;
                            keywordsUsed[keyword] = ci;
                        }

                    // for the dots ("...") case
                    } else if (record.StartsWith(keyword + "=") && (
                           keyword.StartsWith("DATASYMBOL", StringComparison.InvariantCultureIgnoreCase)
                      )
                      ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {

                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            datasymbolDescriptions.Add(keyword, val);
                        }

                    } else if (!string.IsNullOrEmpty(contVariable) && record.StartsWith(keyword + "(") && (
                              keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                           || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                           || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase)
                       )
                       ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                            keywordsUsed[keyword] = ci;
                        }
                        else {
                            ColumnInformation ci = keywordsUsed[keyword];
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                        }
                    }
                    else if (!String.IsNullOrEmpty(variable) && !keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        val = val.Replace("\n", "");

                        List<string> tmlList = new List<string>();

                        if (keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            string tList = m.Groups["tlist"].Value;
                            val = handleTimeVal(val, tList);
                            timeVal = variable;
                            freq = tList[0];
                            //if (!tList.Equals("A1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("H1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("Q1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("M1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("W1", StringComparison.InvariantCultureIgnoreCase)) {
                            //    val = buildTimeVal(tList);
                            //}

                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = "FREQ";
                            ci.Value = freq.ToString();
                            keywordsUsed["FREQ"] = ci;

                            ci = new ColumnInformation();
                            ci.ColumnName = "TIME_FORMAT";
                            ci.Value = TransformFREQToSDMXFormat(freq.ToString());
                            keywordsUsed["TIME_FORMAT"] = ci;

                        }
                        tmlList.AddRange(GetCommaSeperatedValues(val));
                        //if (val[0] == '"') {
                        //    tmlList.AddRange(extractValues.Split(val.Substring(1, val.Length - 2)));
                        //} else {
                        //    tmlList.AddRange(extractValues.Split(val));
                        //}
                        //tmlList.AddRange(val.Split(','));
                        if (keyword.Equals("CODES", StringComparison.InvariantCultureIgnoreCase) ||
                            keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            codes.Add(variable, tmlList);
                        }
                        else if (keyword.Equals("VALUES", StringComparison.InvariantCultureIgnoreCase)) {
                            values.Add(variable, tmlList);
                        }

                    }
                    else if (keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        /*
					 * Build list of keys used in the file.
					 */
                        keys.Add(variable, val);
                        if (variable.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase)) {
                            timeValKeyIdx = keys.Count - 1;
                        }
                    }
                    if (keyword.Equals("DATA", StringComparison.InvariantCultureIgnoreCase)) {
                        record = record.Substring(0, record.Length - 1);
                    }
                    else {
                        record = "";
                    }
                }
            }


            //ArrayList listsForTuples = new ArrayList();
            Dictionary<String, Dictionary<String, String>> columnCodeValueMap = new Dictionary<string, Dictionary<string, string>>();
            foreach (string column in columns) {
                Dictionary<string, string> codesValuesMap = new Dictionary<string, string>();
                columnCodeValueMap.Add(column, codesValuesMap);
                // we don't care about KEYS or TIME 
                //string codesOrValues = "";
                //foreach (string col in keys.Keys) {
                //    if (FormatColumnHeader(col).Replace("\"", "") == column) {
                //        if (col.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase) && keys[col].Equals("VALUES")) {
                //            if (values.ContainsKey(col)) {
                //                trancodeValueList(values[col], freq.ToString());
                //            }
                //            else {
                //                codesOrValues = "CODES";
                //                break;
                //            }
                //        }
                //        codesOrValues = keys[col];
                //        break;
                //    }
                //}

                string key = "";

                foreach (string col in values.Keys) {
                    if (FormatColumnHeader(col).Replace("\"", "") == column) {
                        key = col;
                        createCodesValuesPairs(key, values, codes, codesValuesMap);
                        break;
                    }
                }
                
                // NOR Keywords
                //foreach (string k in keywordsUsed.Keys) {
                //    if (keywordsUsed[k].ColumnName == column) {
                //        List<string> returnValues = new List<string>();
                //        if (!string.IsNullOrEmpty(contVariable) && keywordsUsed[k].IsContVar) {
                //            foreach (string cval in keywordsUsed[k].Values.Values) {
                //                string contvarvalue = cval;
                //                List<string> codevalues = null;
                //                List<string> valuevalues = null;
                //                if (keys.Count <= 0 || keys.Count > 0 && keys[contVariable] == "CODES") {
                //                    if (codes.TryGetValue(contVariable, out codevalues) && values.TryGetValue(contVariable, out valuevalues)) {
                //                        for (int l = 0; l < codes.Count; l++) {
                //                            if (valuevalues.Count > l && valuevalues[l].ToString() == cval) {
                //                                contvarvalue = codevalues[l].ToString();
                //                                break;
                //                            }
                //                        }
                //                    }
                //                }
                //                if (!returnValues.Contains(contvarvalue))
                //                    returnValues.Add(contvarvalue);
                //            }
                //        }
                //        else {
                //            returnValues.Add(keywordsUsed[k].Value);
                //        }
                //        listsForTuples.Add(returnValues);
                //        break;
                //    }
                //}

                if (column.ToLower()=="obs_status") {
                    foreach (string datasymbol in datasymbolDescriptions.Keys) {
                        string value = "";
                        switch (datasymbol.ToUpper()) {
                            case "DATASYMBOL1":
                                value = ".";
                                break;
                            case "DATASYMBOL2":
                                value = "..";
                                break;
                            case "DATASYMBOL3":
                                value = "...";
                                break;
                            case "DATASYMBOL4":
                                value = "....";
                                break;
                            case "DATASYMBOL5":
                                value = ".....";
                                break;
                            case "DATASYMBOL6":
                                value = "......";
                                break;
                            case "DATASYMBOLSUM":
                                value = ".......";
                                break;
                            case "DATASYMBOLNIL":
                                value = "-";
                                break;
                            default:
                                value = "";
                                break;
                        }
                        if(value!="")
                            columnCodeValueMap[column][value] = datasymbolDescriptions[datasymbol];
                    }
                    columnCodeValueMap[column]["A"] = "Normal value";
                }
            }


            return columnCodeValueMap;
        }



        public List<string> GetCodes(string column) {
            StreamReader reader = new StreamReader(pcaxisFile);

            /*
            * String to hold PC-AXIS records.
            */
            String record = "";
            /*
             * String to hold PC-AXIS keyword of each record.
             */
            String keyword = "";

            // hold the TIMEVAL(field) field.
            string timeVal = null;

            // hold the TLIST frequency
            char freq = '\0';

            // hold the position of time val in key
            int timeValKeyIdx = -1;

            /*
             * Strings to hold info for CSV header line.
             */
            String heading = "";
            String stub = "";
            String contents = null;

            string contVariable = "";

            //holds all the keywords to be used as columns
            Dictionary<string, ColumnInformation> keywordsUsed = new Dictionary<string, ColumnInformation>();

            /*
             * ArrayList that will include all ArrayLists that we
             * need to build tuples for.
             */

            Dictionary<String, List<string>> values = new Dictionary<String, List<string>>();
            Dictionary<String, List<string>> codes = new Dictionary<String, List<string>>();
            Dictionary<String, String> keys = new Dictionary<String, String>();

            //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            bool stopreading = false;
            while (!reader.EndOfStream && !stopreading) {
                /*
			    * The following is required because we are not sure if a delimiter
			    * has been used at the end of each line for the DATA record.
    			 * Later on, we use \n as a delimiter.
			    */
                string variable = null;
                string val = null;
                record += '\n';
                string firstSection = GetFirstLineSection(reader);
                if (firstSection.ToLower() == "data=") {
                    stopreading = true;
                    break;
                }

                if (!reader.EndOfStream)
                    record += firstSection + reader.ReadLine().Trim();
                else
                    record += firstSection;

                if (record.EndsWith(";", StringComparison.InvariantCulture)) {
                    /*
                     * Record complete. Trim unecessary whitespace.
                     */
                    record = record.Trim();
                    Match m = extractRecord.Match(record);
                    keyword = m.Groups["keyword"].Value;
                    variable = m.Groups["var"].Value.Trim().Replace("\"","");
                    val = m.Groups["value"].Value.Trim();
                    if (keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)) {
                        contents = val;
                    } else if (keyword.Equals("STUB", StringComparison.InvariantCultureIgnoreCase)) {
                        stub = val;
                    } else if (keyword.Equals("HEADING", StringComparison.InvariantCultureIgnoreCase)) {
                        heading = val;

                    } else if (keyword.Equals("CONTVARIABLE", StringComparison.InvariantCultureIgnoreCase)) {
                        contVariable = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                    } else if (record.StartsWith(keyword + "=") && (
                               keyword.Equals("MATRIX", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-CODE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-AREA", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DECIMALS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TITLE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UPDATE-FREQUENCY", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TABLE_ID", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SOURCE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DATABASE", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                        )
                        ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Value = val;
                            keywordsUsed[keyword] = ci;
                        }

                    } else if (!string.IsNullOrEmpty(contVariable) && record.StartsWith(keyword + "(") && (
                                keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                             || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                             || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase)
                         )
                         ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                            keywordsUsed[keyword] = ci;
                        } else {
                            ColumnInformation ci = keywordsUsed[keyword];
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                        }
                    
                    } else if (!String.IsNullOrEmpty(variable) && !keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        val = val.Replace("\n", "");

                        List<string> tmlList = new List<string>();

                        if (keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            string tList = m.Groups["tlist"].Value;
                            val = handleTimeVal(val, tList);
                            timeVal = variable;
                            freq = tList[0];
                            //if (!tList.Equals("A1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("H1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("Q1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("M1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("W1", StringComparison.InvariantCultureIgnoreCase)) {
                            //    val = buildTimeVal(tList);
                            //}

                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = "FREQ";
                            ci.Value = freq.ToString();
                            keywordsUsed["FREQ"] = ci;

                            ci = new ColumnInformation();
                            ci.ColumnName = "TIME_FORMAT";
                            ci.Value = TransformFREQToSDMXFormat(freq.ToString());
                            keywordsUsed["TIME_FORMAT"] = ci;

                        }
                        tmlList.AddRange(GetCommaSeperatedValues(val));
                        //if (val[0] == '"') {
                        //    tmlList.AddRange(extractValues.Split(val.Substring(1, val.Length - 2)));
                        //} else {
                        //    tmlList.AddRange(extractValues.Split(val));
                        //}
                        //tmlList.AddRange(val.Split(','));
                        if (keyword.Equals("CODES", StringComparison.InvariantCultureIgnoreCase) ||
                            keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            codes.Add(variable, tmlList);
                        } else if (keyword.Equals("VALUES", StringComparison.InvariantCultureIgnoreCase)) {
                            values.Add(variable, tmlList);
                        }

                    } else if (keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        /*
					 * Build list of keys used in the file.
					 */
                        keys.Add(variable, val);
                        if (variable.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase))
                        {
                            timeValKeyIdx = keys.Count - 1;
                        }
                    }
                    if (keyword.Equals("DATA", StringComparison.InvariantCultureIgnoreCase)) {
                        record = record.Substring(0, record.Length - 1);
                    } else {
                        record = "";
                    }
                }
            }


            foreach (string k in keywordsUsed.Keys) {
                if (keywordsUsed[k].ColumnName == column) {
                    List<string> retval = new List<string>();
                    if (!string.IsNullOrEmpty(contVariable) && keywordsUsed[k].IsContVar) {
                        foreach (string cval in keywordsUsed[k].Values.Values) {
                            string contvarvalue = cval;
                            List<string> codevalues = null;
                            List<string> valuevalues = null;
                            if (keys.Count <= 0 || keys.Count > 0 && keys[contVariable] == "CODES") {
                                if (codes.TryGetValue(contVariable, out codevalues) && values.TryGetValue(contVariable, out valuevalues)) {
                                    for (int l = 0; l < codes.Count; l++) {
                                        if (valuevalues.Count > l && valuevalues[l].ToString() == cval) {
                                            contvarvalue = codevalues[l].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                            if(!retval.Contains(contvarvalue))
                                retval.Add(contvarvalue);
                        }
                    } else {
                        retval.Add(keywordsUsed[k].Value);
                    }
                    return retval;
                }
            }

            string codesOrValues = "";
            foreach (string col in keys.Keys) {
                if (FormatColumnHeader(col).Replace("\"", "") == column) {
                    if (col.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase) && keys[col].Equals("VALUES"))
                    {
                        if (values.ContainsKey(col))
                        {
                            trancodeValueList(values[col], freq.ToString());
                        }
                        else
                        {
                            codesOrValues = "CODES";
                            break;
                        }
                    }
                    codesOrValues = keys[col];
                    break;
                }
            }

            
            
            string key = "";

            if (codesOrValues != "VALUES") {
                foreach (string col in codes.Keys) {
                    if (FormatColumnHeader(col).Replace("\"", "") == column) {
                        key = col;
                        break;
                    }
                }
                if (key != "")
                    return codes[key];
            }

            foreach (string col in values.Keys) {
                if (FormatColumnHeader(col).Replace("\"", "") == column) {
                    key = col;
                    break;
                }
            }

            if (key != "")
                return values[key];
            else if(column.ToLower()=="obs_status"){
                List<string> retval = getAllDotsValuesInDataSection(reader);
                return retval;
            }

            return new List<string>();

        }

        public List<string> GetColumns(ColumnFilter filter) {
            StreamReader reader = new StreamReader(pcaxisFile);

            /*
            * String to hold PC-AXIS records.
            */
            String record = "";
            /*
             * String to hold PC-AXIS keyword of each record.
             */
            String keyword = "";
            /*
             * Strings to hold info for CSV header line.
             */
            String heading = "";
            String stub = "";
            String contents = null;
            /*
             * Keeps all the Keywords found in the file
             */

            string contVariable = "";

            //holds all the keywords to be used as columns
            Dictionary<string, ColumnInformation> keywordsUsed = new Dictionary<string, ColumnInformation>();


            //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            bool stopreading = false;
            while (!reader.EndOfStream && !stopreading) {
                /*
			    * The following is required because we are not sure if a delimiter
			    * has been used at the end of each line for the DATA record.
    			 * Later on, we use \n as a delimiter.
			    */
                string val = null;
                record += '\n';
                string firstSection = GetFirstLineSection(reader);
                if (firstSection.ToLower() == "data=") {
                    stopreading = true;
                    break;
                }
                if (!reader.EndOfStream)
                    record += firstSection + reader.ReadLine().Trim();
                else
                    record += firstSection;

                if (record.EndsWith(";", StringComparison.InvariantCulture)) {
                    /*
                     * Record complete. Trim unecessary whitespace.
                     */
                    record = record.Trim();
                    Match m = extractRecord.Match(record);
                    keyword = m.Groups["keyword"].Value;
                    val = m.Groups["value"].Value.Trim();
                    if (keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)) {
                        contents = val;
                    } else if (keyword.Equals("STUB", StringComparison.InvariantCultureIgnoreCase)) {
                        stub = val;
                    } else if (keyword.Equals("HEADING", StringComparison.InvariantCultureIgnoreCase)) {
                        heading = val;
                    //} else if (
                    //           keyword.Equals("MATRIX", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-CODE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-AREA", StringComparison.InvariantCultureIgnoreCase)
                    //        || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DECIMALS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TITLE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase)
                    //        || keyword.Equals("UPDATE-FREQUENCY", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TABLE_ID", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SOURCE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DATABASE", StringComparison.InvariantCultureIgnoreCase)
                    //        || keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)
                    //        || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                    //        || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)    
                    //    ) {
                    //    if (!keywordsUsed.Contains(keyword))
                    //        keywordsUsed.Add(keyword);
                    } else if (keyword.Equals("CONTVARIABLE", StringComparison.InvariantCultureIgnoreCase)) {
                        contVariable = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");

                    } else if (record.StartsWith(keyword + "=") && (
                               keyword.Equals("MATRIX", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-CODE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-AREA", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DECIMALS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TITLE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UPDATE-FREQUENCY", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TABLE_ID", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SOURCE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DATABASE", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                        )
                        ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Value = val;
                            keywordsUsed[keyword] = ci;
                        }

                    } else if (!string.IsNullOrEmpty(contVariable) && record.StartsWith(keyword + "(") && (
                               keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase)
                        )
                        ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            //ci.Values.Add(variable, val);
                            keywordsUsed[keyword] = ci;
                        } else {
                            ColumnInformation ci = keywordsUsed[keyword];
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            //ci.Values.Add(variable, val);
                        }

                    } else if (keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                        string tList = m.Groups["tlist"].Value;
                        char freq = tList[0];

                        ColumnInformation ci = new ColumnInformation();
                        ci.ColumnName = "FREQ";
                        ci.Value = freq.ToString();
                        keywordsUsed["FREQ"] = ci;

                        ci = new ColumnInformation();
                        ci.ColumnName = "TIME_FORMAT";
                        ci.Value = TransformFREQToSDMXFormat(freq.ToString());
                        keywordsUsed["TIME_FORMAT"] = ci;

                    }
 

                    record = "";
                }
            }

            char separator = ',';
            /*
		     * Prepare CSV header String and write it.
		     */
            if (!String.IsNullOrEmpty(stub)) {
                stub += separator;
            }
            if (!String.IsNullOrEmpty(heading)) {
                heading += separator;
            }
            if (contents == null) {
                Console.WriteLine("NO CONTENTS RECORD DEFINED! MANDATORY!");
            }

            String csvHeaderString = stub + heading + contents + ",\"OBS_STATUS\"";
            List<string> headers = GetColumnHeaders(csvHeaderString);
            List<string> retval = new List<string>();
            if (filter == ColumnFilter.All || filter == ColumnFilter.Main)
                foreach (string header in headers) {
                    retval.Add(header.Replace("\"", ""));
                }
            if(filter!= ColumnFilter.Main && filter!= ColumnFilter.ContVariable)
                foreach (ColumnInformation col in keywordsUsed.Values) {
                    if(filter== ColumnFilter.KeywordsSingleValue && !col.IsContVar
                        || filter== ColumnFilter.ContVariableKeywords && col.IsContVar
                        || filter == ColumnFilter.All)
                        retval.Add(col.ColumnName);
                }

            if (filter == ColumnFilter.ContVariable)
                retval.Add(FormatColumnHeader(contVariable));

            return retval;
        }

        private string TransformFREQToSDMXFormat(string p) {
            switch (p) {
                case "A":
                    return "P1Y";
                case "D":
                    return "P1D";
                case "H":
                    return "P6M";
                case "M":
                    return "P1M";
                case "Q":
                    return "P3M";
                case "W":
                    return "P7D";
                default:
                    return "";
            }
        }


        public string GetFirstLineSection(StreamReader reader) {
            //char[] readChars = new char[5];
            //reader.Read(readChars, 0, 5);
            //return new string(readChars);
            string retval = "";
            char[] readChars = new char[1];
            int i = 0;
            while (!reader.EndOfStream && i<5 && readChars[0]!=';') {
                reader.Read(readChars, 0, 1);
                retval += readChars[0];
                i++;
            }
            return retval;
        }

        public void InsertDataInMemoryDB(SQLiteCommand commandData, SQLiteCommand commandKeywordsSingleValue,SQLiteCommand commandContVariableKeywords) {
            StreamReader reader = new StreamReader(pcaxisFile);

            /*
            * String to hold PC-AXIS records.
            */
            StringBuilder record = new StringBuilder();

            /// check if we have enough space
            //if (reader.BaseStream.Length >= record.MaxCapacity) {
            //    throw new Exception("PC-Axis file is too big to fit in memory");
            //}
            //record.Capacity = (int)reader.BaseStream.Length;

            /*
             * String to hold PC-AXIS keyword of each record.
             */
            String keyword = "";
            /*
             * Strings to hold info for CSV header line.
             */
            String heading = "";
            String stub = "";
            String contents = null;

            // hold the TIMEVAL(field) field.
            string timeVal = null;

            // hold the TLIST frequency
            char freq = '\0';

            // hold the position of time val in key
            int timeValKeyIdx = -1;

            string contVariable = "";

            //holds all the keywords to be used as columns
            Dictionary<string,ColumnInformation> keywordsUsed = new Dictionary<string,ColumnInformation>();

            /*
             * ArrayList that will include all ArrayLists that we
             * need to build tuples for.
             */
            ArrayList listsForTuples = new ArrayList();

            Dictionary<String, ArrayList> values = new Dictionary<String, ArrayList>();
            Dictionary<String, ArrayList> codes = new Dictionary<String, ArrayList>();
            Dictionary<String, String> keys = new Dictionary<String, String>();
            List<String> variablesList = new List<String>();
            List<String> stubList = new List<String>();
            List<String> headingList = new List<String>();

            //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>[^;]*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //@aris: maybe the following regex is more correct as there might be a ';' character in the values list within double quotes (")
            //Regex extractRecord = new Regex("^(?<keyword>[^\\(=]+)(\\((?<var>\"[^\"]+\")\\))?=(TLIST\\((?<tlist>.+)\\),?)?(?<value>.*);$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            while (!reader.EndOfStream) {
                /*
			    * The following is required because we are not sure if a delimiter
			    * has been used at the end of each line for the DATA record.
    			 * Later on, we use \n as a delimiter.
			    */
                string variable = null;
                string val = null;
                record.Append('\n');
                string firstSection = GetFirstLineSection(reader);
                if (firstSection.ToLower() == "data=") {
                    break;
                }
                record.Append(firstSection);
                if(!reader.EndOfStream)
                    record.Append(reader.ReadLine().Trim());

                if (record[record.Length - 1] == ';') {
                    /*
                     * Record complete. Trim unecessary whitespace.
                     */
                    string recordStr = record.ToString().Trim();
                    record = new StringBuilder(recordStr);//record.ToString().Trim());
                    Match m = extractRecord.Match(recordStr);//record.ToString());
                    keyword = m.Groups["keyword"].Value;
                    variable = m.Groups["var"].Value.Trim().Replace("\"","");
                    val = m.Groups["value"].Value.Trim();
                    if (keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)) {
                        contents = val;
                    } else if (keyword.Equals("STUB", StringComparison.InvariantCultureIgnoreCase)) {
                        stub = val;
                        stubList.AddRange(GetCommaSeperatedValues(stub));
                    } else if (keyword.Equals("HEADING", StringComparison.InvariantCultureIgnoreCase)) {
                        heading = val;
                        headingList.AddRange(GetCommaSeperatedValues(heading));
                    } else if (keyword.Equals("CONTVARIABLE", StringComparison.InvariantCultureIgnoreCase)) {
                        contVariable = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                    } else if (recordStr.StartsWith(keyword + "=") && (
                               keyword.Equals("MATRIX", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-CODE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SUBJECT-AREA", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DECIMALS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TITLE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DESCRIPTION", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("UPDATE-FREQUENCY", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("TABLE_ID", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SOURCE", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("DATABASE", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTENTS", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                            || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                        )
                        ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            val = val.Replace("\"", "").Replace("#"," ").Replace("\n"," ");
                            ci.Value = val;
                            keywordsUsed[keyword] = ci;
                        }

                    } else if (!string.IsNullOrEmpty(contVariable) && recordStr.StartsWith(keyword + "(") && (
                                keyword.Equals("REFPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("BASEPERIOD", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CONTACT", StringComparison.InvariantCultureIgnoreCase)
                             || keyword.Equals("LAST-UPDATED", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("STOCKFA", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("CFPRICES", StringComparison.InvariantCultureIgnoreCase)
                             || keyword.Equals("DAYADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("SEASADJ", StringComparison.InvariantCultureIgnoreCase) || keyword.Equals("UNITS", StringComparison.InvariantCultureIgnoreCase)
                         )
                         ) {
                        if (!keywordsUsed.ContainsKey(keyword)) {
                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                            keywordsUsed[keyword] = ci;
                        } else {
                            ColumnInformation ci = keywordsUsed[keyword];
                            ci.ColumnName = FormatColumnHeader(keyword);
                            ci.IsContVar = true;
                            val = val.Replace("\"", "").Replace("#", " ").Replace("\n", " ");
                            ci.Values.Add(variable, val);
                            //keywordsUsed[keyword] = ci;
                        }

                    } else if (!String.IsNullOrEmpty(variable) && !keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        val = val.Replace("\n", "");

                        ArrayList tmlList = new ArrayList();

                        if (keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            string tList = m.Groups["tlist"].Value;
                            val = handleTimeVal(val, tList);
                            timeVal = variable;
                            freq = tList[0];
                            //if (!tList.Equals("A1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("H1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("Q1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("M1", StringComparison.InvariantCultureIgnoreCase) &&
                            //    !tList.Equals("W1", StringComparison.InvariantCultureIgnoreCase))
                            //{
                            //    val = buildTimeVal(tList);
                            //}
                            //else
                            //{
                            //    transcodeTimeOutsideTlist(val, tList);
                            //}


                            ColumnInformation ci = new ColumnInformation();
                            ci.ColumnName = "FREQ";
                            ci.Value = freq.ToString();
                            keywordsUsed["FREQ"] = ci;

                            ci = new ColumnInformation();
                            ci.ColumnName = "TIME_FORMAT";
                            ci.Value = TransformFREQToSDMXFormat(freq.ToString());
                            keywordsUsed["TIME_FORMAT"] = ci;



                        }
                        tmlList.AddRange(GetCommaSeperatedValues(val));
                        //if (val[0] == '"') {
                        //    tmlList.AddRange(extractValues.Split(val.Substring(1, val.Length - 2)));
                        //} else {
                        //    tmlList.AddRange(extractValues.Split(val));
                        //}

                        if (keyword.Equals("CODES", StringComparison.InvariantCultureIgnoreCase) ||
                            keyword.Equals("TIMEVAL", StringComparison.InvariantCultureIgnoreCase)) {
                            codes.Add(variable, tmlList);
                        } else if (keyword.Equals("VALUES", StringComparison.InvariantCultureIgnoreCase)) {
                            values.Add(variable, tmlList);
                        }

                    } else if (keyword.Equals("KEYS", StringComparison.InvariantCultureIgnoreCase)) {
                        /*
					     * Build list of keys used in the file.
					     */
                        keys.Add(variable, val);
                        if (variable.Equals(timeVal, StringComparison.InvariantCultureIgnoreCase)) {
                            timeValKeyIdx = keys.Count - 1;
                        }
                    }



                    if (keyword.Equals("DATA", StringComparison.InvariantCultureIgnoreCase)) {
                        record.Remove(record.Length - 1, 1);
                    } else {
                        record = new StringBuilder();
                    }
                }
            }


            char separator = ',';
            /*
		     * Prepare CSV header String and write it.
		     */
            if (!String.IsNullOrEmpty(stub)) {
                stub += separator;
            }
            if (!String.IsNullOrEmpty(heading)) {
                heading += separator;
            }
            if (contents == null) {
                Console.WriteLine("NO CONTENTS RECORD DEFINED! MANDATORY!");
            }

            String csvHeaderString = stub + heading + contents;
            /*
             * Remove contents from CSV header in order to build up a varaibles List.
             */
            String variablesString = csvHeaderString.Substring(0, csvHeaderString.LastIndexOf("\",\"") + 1);

            variablesList.AddRange(GetCommaSeperatedValues(variablesString)); // TODO: Take into account that STUB or HEADING values may include commas (,). 

            /* 
             * Go through keys to identify whether values or codes are going to be used
             * for the tuples. If no keys defined, codes are prefered, unless only values
             * are defined.
             */
            bool keysUsed = false;
            if (keys.Count > 0) {
                /* 
                 * This means that DATA will contain
                 * a sparse table, thus including values/codes
                 * for the STUB in front of the observations.
                 * So, we need the tuples for the rest of the
                 * variables, i.e. the HEADING (rows).
                 */
                foreach (String headingVariable in headingList) {
                    ArrayList variableItems = null;
                    if (!codes.TryGetValue(headingVariable, out variableItems)) {
                        if (values.TryGetValue(headingVariable, out variableItems)) {
                        } else {
                            Console.WriteLine("NO ITEMS FOUND FOR: " + headingVariable);
                        }
                    }
                    listsForTuples.Add(variableItems);
                }
                
                keysUsed = true;
            } else {
                /*
                 * All values MUST exist in DATA record.
                 * VALUES or CODES should be taken for each variable.
                 */
                foreach (String variable in variablesList) {
                    ArrayList variableItems = null;
                    if (!codes.TryGetValue(variable, out variableItems)) {
                        if (values.TryGetValue(variable, out variableItems)) {
                        } else {
                            Console.WriteLine("Nor CODES or VALUES found for variable: " + variable);
                        }
                    }
                    listsForTuples.Add(variableItems);

                    //if (codes.Count > 0) {
                    //    listsForTuples.Add(codes[variable]);
                    //} else if (values.Count > 0) {
                    //    listsForTuples.Add(values[variable]);
                    //} else {
                    //    Console.WriteLine("Nor CODES or VALUES found for variable: " + variable);
                    //}
                }
            }


            /*
             * KEYS are used, thus the prefix of each record from the DATA section
             * will be copied directly to the CSV.
             */

            List<string> thevalues = new List<string>();
            int valuesRead = 0;
            //NEEDS REVISION
            int valueColumnsNum = 1;
            if(keysUsed)
                foreach (string head in headingList) {
                    valueColumnsNum *= codes[head].Count;
                }

            int j = 0;
            while (!reader.EndOfStream && (!PcAxisConnection.DataPreviewOnly || DataPreviewRows == 0 || j<DataPreviewRows))
                for (IEnumerator iter = new TupleIterator(listsForTuples); iter.MoveNext() && (!PcAxisConnection.DataPreviewOnly || DataPreviewRows == 0|| j < DataPreviewRows); j++) {

                    if (reader.EndOfStream)
                        break;

                    /*
                     * Re-encapsulate values into double-quotes.
                     */
                    int parameterIndex = 0;

                    string value = "";
                    char[] chars = new char[1];
                    char[] oldchars = new char[1];
                    while (!reader.EndOfStream) {
                        reader.Read(chars, 0, 1);
                        if ((chars[0] == ',' || chars[0] == ' ' || chars[0] == ';' || chars[0] == '\n' || chars[0] == '\t') && 
                            (
                                keysUsed && thevalues.Count < keys.Count && oldchars[0] == '"'
                            || (keysUsed && thevalues.Count >= keys.Count || !keysUsed) 
                            )
                            ) {
                            if (keysUsed && thevalues.Count < keys.Count) {
                                thevalues.Add(value.Replace("\"", ""));
                                value = "";
                            } else if (value.Trim() != "")
                                break;
                        } else if (chars[0] != '\r') {
                            value += chars[0];
                        }
                        oldchars[0] = chars[0];
                    }
                    value = value.Trim();
                    if (reader.EndOfStream && value == "")
                        break;

                    foreach (string str in thevalues) {

                        if (parameterIndex == timeValKeyIdx)
                        {
                            commandData.Parameters[parameterIndex].Value = transcodeTimeOutsideTlist(str, freq.ToString());
                        }
                        else
                        {
                            commandData.Parameters[parameterIndex].Value = str;
                        }
                        parameterIndex++;
                    }

                    foreach (object obj in (ArrayList)iter.Current) {

                        commandData.Parameters[parameterIndex].Value = obj.ToString();
                        parameterIndex++;
                    }

                    string obs_status = "A";
                    string valuetoshow = value;
                    
                    if (valuetoshow == "\".\"" ||
                        valuetoshow == ".") {
                        valuetoshow = "";
                        obs_status = ".";
                    }
                    if (valuetoshow == "\"..\"" ||
                        valuetoshow == "..") {
                        valuetoshow = "";
                        obs_status = "..";
                    }
                    if (valuetoshow == "\"...\"" ||
                        valuetoshow == "...") {
                        valuetoshow = "";
                        obs_status = "...";
                    }
                    if (valuetoshow == "\"....\"" ||
                        valuetoshow == "....") {
                        valuetoshow = "";
                        obs_status = "....";
                    }
                    if (valuetoshow == "\".....\"" ||
                        valuetoshow == ".....") {
                        valuetoshow = "";
                        obs_status = ".....";
                    }
                    if (valuetoshow == "\"......\"" ||
                        valuetoshow == "......") {
                        valuetoshow = "";
                        obs_status = "......";
                    }
                    if (valuetoshow == "\".......\"" ||
                        valuetoshow == ".......") {
                        valuetoshow = "";
                        obs_status = ".......";
                    }
                    if (valuetoshow == "\"-\"" ||
                        valuetoshow == "-") {
                        valuetoshow = "";
                        obs_status = "-";
                    }

                    commandData.Parameters[parameterIndex].Value = valuetoshow;
                    parameterIndex++;

                    commandData.Parameters[parameterIndex].Value = obs_status;
                    parameterIndex++;

                    /*
                     * Execute Command
                     */
                    commandData.ExecuteNonQuery();

                    valuesRead++;
                    if (keysUsed && valuesRead == valueColumnsNum) {
                        valuesRead = 0;
                        thevalues.Clear();
                    }

                }

            
            int paramIndex = 0;
            //holds all the possible values that a ContVariable can have
            List<string> contVariableValues = new List<string>();
            foreach (ColumnInformation cinfo in keywordsUsed.Values) {
                if (!cinfo.IsContVar) {
                    commandKeywordsSingleValue.Parameters[paramIndex].Value = cinfo.Value;
                    paramIndex++;
                } else {
                    foreach (string key  in cinfo.Values.Keys) {
                        if (!contVariableValues.Contains(key)) {
                            contVariableValues.Add(key);
                        }
                    }
                }
            }

            /*
             * Execute Command
             */
            commandKeywordsSingleValue.ExecuteNonQuery();

            foreach (string v in contVariableValues) {
                string contvarvalue = v;
                ArrayList codevalues = null;
                ArrayList valuevalues = null;
                if (!keysUsed || keysUsed && keys[contVariable] == "CODES") {
                    if (codes.TryGetValue(contVariable, out codevalues) && values.TryGetValue(contVariable, out valuevalues)) {
                        for (int k = 0; k < codes.Count; k++) {
                            if (valuevalues.Count > k && valuevalues[k].ToString() == v) {
                                contvarvalue = codevalues[k].ToString();
                                break;
                            }
                        }
                    }
                }
                commandContVariableKeywords.Parameters[0].Value = contvarvalue;
                int pIndex = 1;
                //holds all the possible values that a ContVariable can have
                foreach (ColumnInformation cinfo in keywordsUsed.Values) {
                    if (cinfo.IsContVar) {
                        if (cinfo.Values.ContainsKey(v))
                            commandContVariableKeywords.Parameters[pIndex].Value = cinfo.Values[v];
                        else {
                            commandContVariableKeywords.Parameters[pIndex].Value = null;
                            //uncomment for getting the default value of the contVariable in case no value is provided
                            //commandContVariableKeywords.Parameters[pIndex].Value = cinfo.Value;
                        }

                        pIndex++;
                    } 
                }
                commandContVariableKeywords.ExecuteNonQuery();

            }



            /*
             * Close streams and writers/readers.
             */
            reader.Close();
        }
        /// <summary>
        /// Populate a map between the specified codes and values of the specified column
        /// </summary>
        /// <param name="column">The key column. It must be the key used for values and codes maps.</param>
        /// <param name="values">The key columns to list of values map</param>
        /// <param name="codes">The key columns to list of codes map</param>
        /// <param name="codeValuePairs">The map to populate</param>
        private void createCodesValuesPairs(string column, Dictionary<String, List<String>> values, Dictionary<String, List<String>> codes, Dictionary<String,String> codeValuePairs ) {
            List<String> codeList = null;
            List<String> valueList = null;
            
            values.TryGetValue(column, out valueList);
            codes.TryGetValue(column, out codeList);
            
            if (codeList != null && valueList!=null) {
                for (int i = 0, j = valueList.Count, z = codeList.Count; i < j; i++) {
                    if (i < z) {
                        codeValuePairs.Add(codeList[i], valueList[i]);
                    }
                }
            }
        }
       

        private string[] GetCommaSeperatedValues(string strValues) {
            Regex extractValues = new Regex("\"(?:,\")?");
            if (strValues[0] == '"') {
                return extractValues.Split(strValues.Substring(1, strValues.Length - 2));
            } else {
                return extractValues.Split(strValues);
            }
            //return strValues.Split(',');
        }

        private string FormatColumnHeader(string header) {
            //Regex r = new Regex("[ \\.\\-,;()=\\\\/#!%$@]", RegexOptions.CultureInvariant);
            Regex r = new Regex(@"[^a-zA-Z0-9]", RegexOptions.CultureInvariant);
            return r.Replace(header, "_");
        }

        public static string FormatTableName(string pcaxisFileNameWithExtension) {
            //Regex r = new Regex("[ \\.\\-,;()=\\\\/#!%$@]", RegexOptions.CultureInvariant);
            Regex r = new Regex(@"[^a-zA-Z0-9]", RegexOptions.CultureInvariant);
            return r.Replace(pcaxisFileNameWithExtension.Substring(0, pcaxisFileNameWithExtension.LastIndexOf('.')), "_");
        }

        private List<string> GetColumnHeaders(string columnHeaders) {
            //Regex regex = new Regex("\"(?:,\")?");
            List<string> header = new List<string>(GetCommaSeperatedValues(columnHeaders));// new List<string>(regex.Split(columnHeaders.Substring(1, columnHeaders.Length - 2)));
            for (int i = 0; i < header.Count; i++) {
                header[i] = FormatColumnHeader(header[i]);
                header[i] = '"' + header[i].Substring(0, Math.Min(header[i].Length, 50)) + '"';
            }
            return header;
        }

        #endregion
    }

    public class ColumnInformation {
        public string ColumnName { get; set; }
        public bool IsContVar { get; set; }

        public Dictionary<string, string> Values { get; set; }
        public string Value { get; set; }

        public ColumnInformation() {
            Values = new Dictionary<string, string>();
        }
    }

    public enum ColumnFilter {
        All,
        Main,
        KeywordsSingleValue,
        ContVariableKeywords,
        ContVariable
    }

}
