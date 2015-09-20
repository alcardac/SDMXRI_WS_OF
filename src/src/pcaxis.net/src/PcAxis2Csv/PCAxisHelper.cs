using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using org.estat.PcAxis;
using EnterpriseDT.Net.Ftp;
using System.Collections;

namespace org.estat.PcAxis.helpers
{
    public static class PCAxisHelper {

        //private const string SCHEMA_INI="Schema.ini";
        //public const string extension = "tab";
        //private static void RefreshSchemaIni(string filePath) {
        //    DirectoryInfo di = new DirectoryInfo(filePath);
        //    FileInfo[] files = di.GetFiles("*." + extension);
        //    foreach (FileInfo fi in files) {
        //        FileInfo schemaIni = new FileInfo(string.Format(@"{0}\{1}",filePath,SCHEMA_INI));
        //        if (!schemaIni.Exists) {
        //            FileStream fs = schemaIni.Create();
        //            fs.Close();
        //            fs.Dispose();
        //        }

        //        TextReader tr = new StreamReader(string.Format(@"{0}\{1}",filePath,SCHEMA_INI));
        //        string schemaStr = tr.ReadToEnd();
        //        tr.Close();
        //        tr.Dispose();

        //        if (!schemaStr.Contains(string.Format("[{0}]", fi.Name))) {
        //            TextWriter tw = new StreamWriter(string.Format(@"{0}\{1}",filePath,SCHEMA_INI),true);
        //            tw.WriteLine(string.Format("[{0}]", fi.Name));
        //            tw.WriteLine("Format=Delimited(;)");
        //            tw.WriteLine("ColNameHeader=True");
        //            tw.WriteLine("FileType=text");
        //            tw.WriteLine("MaxScanRows=0");
        //            tw.WriteLine("CharacterSet=Unicode");
        //            tw.Close();
        //            tw.Dispose();
        //        }
        //    }
        //}

        //private static string ConvertPCAxisStrToCSVStr(string pcaxisStr) {
        //    return pcaxisStr; //TODO: Implement the actual conversion
        //}

        //public static void ConvertPCAxisFilesToCSVFiles(string connectionString) {
        //    string filePath = GetFilePathFromConnectionString(connectionString);
        //    string csvFilePath = GetCSVFilePathFromConnectionString(connectionString);
        //    DirectoryInfo csvDi = new DirectoryInfo(csvFilePath);
        //    if (!csvDi.Exists)
        //        csvDi.Create();

        //    if (filePath.Trim().ToLower().StartsWith("ftp://")) {
        //        filePath = SynchFTPFiles(filePath, csvFilePath);
        //    }

        //    DirectoryInfo di = new DirectoryInfo(filePath);
        //    FileInfo[] files = di.GetFiles("*.px");
        //    foreach (FileInfo fi in files) {
        //        string csvFileName = string.Format(@"{0}\{1}", csvFilePath, fi.Name.Replace(fi.Extension, "." + extension));
        //        FileInfo csvFileCheck = new FileInfo(csvFileName);
        //        if (csvFileCheck.Exists && csvFileCheck.CreationTime >= fi.LastWriteTime)
        //            continue; //we dont have to do nothing in that case because the CSV file is up to date

        //        string lockFileName = string.Format(@"{0}\{1}", csvFilePath, fi.Name.Replace(fi.Extension, ".lock"));
        //        FileInfo lockFile = new FileInfo(lockFileName);
        //        int i = 0;
        //        while (lockFile.Exists && i<10) {
        //            Thread.CurrentThread.Join(1000);
        //            lockFile.Refresh();
        //            i++;
        //        }
        //        if (i == 10)
        //            throw new IOException("File is locked.");
        //        TextWriter tw = null;
        //        try
        //        {
        //            FileStream lfs = lockFile.Create();//create lock file
        //            lfs.Close();
        //            lfs.Dispose();

        //            //Create csv file if not exists
        //            FileInfo csvFile = new FileInfo(csvFileName);
                    
        //            if (!csvFile.Exists)
        //            {
        //                FileStream fs = csvFile.Create();
        //                fs.Close();
        //                fs.Dispose();
        //            }

        //            //Read pc-axis file
        //            PcAxis pcAxis = new PcAxis(fi.FullName);
        //            //TextReader tr = new StreamReader(fi.FullName);
        //            //string pcaxisStr = tr.ReadToEnd();
        //            //tr.Close();
        //            //tr.Dispose();
        //            //string csvString = ConvertPCAxisStrToCSVStr(pcaxisStr);

        //            //Write csv file on the disk
        //            tw = new StreamWriter(csvFileName, false, Encoding.Unicode);
        //            pcAxis.convert2csv(tw);
        //            //tw.Write(csvString);
        //            //tw.Close();
        //            tw.Dispose();

        //            RefreshSchemaIni(csvFilePath);

                 
        //        }
        //        catch (IOException ex)
        //        {
        //            if (tw != null && File.Exists(csvFileName))
        //            {
        //                tw.Close();
        //                tw.Dispose();
        //                File.Delete(csvFileName);
        //            }
        //            throw new IOException(string.Format("Error while reading PC Axis files : {0}", ex.Message));
        //        }
        //        catch (Exception ex)
        //        {
        //            if (tw != null && File.Exists(csvFileName))
        //            {
        //                tw.Close();
        //                tw.Dispose();
        //                File.Delete(csvFileName);
        //            }
        //            throw ex;
        //        }
        //        finally
        //        {

        //            lockFile.Delete();//delete lock file (even if there was an exception
        //        }
        //    }
 
        //}

        
        //public static string SynchFTPFiles(string filePath, string csvFilePath) {
        //    FTPConnection connection = GetFtpConnection(filePath);

        //    if (connection == null) {
        //        throw new Exception("Wrong ftp URL");
        //    }

        //    filePath = string.Format(@"{0}\DownloadedPcAxis", csvFilePath).Replace(@"\\", @"\");
        //    DirectoryInfo dlds = new DirectoryInfo(filePath);
        //    if (!dlds.Exists)
        //        dlds.Create();

        //    connection.Connect();
        //    connection.ConnectMode = FTPConnectMode.PASV;
        //    connection.TransferType = FTPTransferType.BINARY;
        //    connection.DataEncoding = Encoding.ASCII;
        //    string[] ftpFileNames = connection.GetFiles();
        //    FTPFile[] ftpfiles = connection.GetFileInfos();
        //    int i = 0;
        //    foreach (FTPFile ftpfile in ftpfiles) {
        //        string csvFileName="";
        //        string filename = "";
        //        if (ftpfile.Name.ToLower().Trim().EndsWith(".px")) {
        //            filename = ftpfile.Name;
        //            csvFileName = string.Format(@"{0}\{1}", filePath, ftpfile.Name);
        //        } else if (ftpFileNames.Length > i && ftpFileNames[i].ToLower().Trim().EndsWith(".px"))
        //        {
        //            filename = GetFileName(ftpFileNames[i]);
        //            csvFileName = string.Format(@"{0}\{1}", filePath, filename);
        //        }
        //        FileInfo csvFileCheck = new FileInfo(csvFileName);
        //        if (csvFileCheck.Exists && csvFileCheck.CreationTime >= ftpfile.LastModified)
        //            continue; //we dont have to do nothing in that case because the CSV file is up to date
        //        connection.DownloadFile(csvFileName, filename);

        //        i++;
        //    }

        //    connection.Close();
        //    connection.Dispose();
        //    return filePath;
        //}

        public static string TempDirectory {
            get {
                //return org.estat.ma.Properties.Settings.Default.csvLocalFilePath;
                return string.Format("{0}\\mapping assistant", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            }
        }


        public static string SynchFTPFiles(string filePath) {
            FTPConnection connection = GetFtpConnection(filePath);

            if (connection == null) {
                throw new Exception("Wrong ftp URL");
            }
            string csvFilePath = GetLocalFilePathFromRemote(filePath, TempDirectory);
            DirectoryInfo csvDi = new DirectoryInfo(csvFilePath);
            if (!csvDi.Exists)
                csvDi.Create();

            filePath = string.Format(@"{0}\DownloadedPcAxis", csvFilePath).Replace(@"\\", @"\");
            DirectoryInfo dlds = new DirectoryInfo(filePath);
            if (!dlds.Exists)
                dlds.Create();

            connection.Connect();
            connection.ConnectMode = FTPConnectMode.PASV;
            connection.TransferType = FTPTransferType.BINARY;
            connection.DataEncoding = Encoding.ASCII;
            string[] ftpFileNames = connection.GetFiles();
            FTPFile[] ftpfiles = connection.GetFileInfos();
            int i = 0;
            foreach (FTPFile ftpfile in ftpfiles) {
                string localFileName = "";
                string filename = "";
                if (ftpfile.Name.ToLower().Trim().EndsWith(".px")) {
                    filename = ftpfile.Name;
                    localFileName = string.Format(@"{0}\{1}", filePath, ftpfile.Name);
                } else if (ftpFileNames.Length > i && ftpFileNames[i].ToLower().Trim().EndsWith(".px")) {
                    filename = GetFileName(ftpFileNames[i]);
                    localFileName = string.Format(@"{0}\{1}", filePath, filename);
                }
                if (!localFileName.Equals(""))
                {
                    FileInfo csvFileCheck = new FileInfo(localFileName);
                    if (csvFileCheck.Exists && (csvFileCheck.CreationTime >= ftpfile.LastModified && ftpfile.LastModified != DateTime.MinValue))
                        continue; //we dont have to do nothing in that case because the CSV file is up to date
                    connection.DownloadFile(localFileName, filename);
                }

                i++;
            }

            connection.Close();
            connection.Dispose();
            return filePath;
        }


        private static string GetFileName(string p) {
            int pos = p.LastIndexOf('/');
            if (pos >= 0 && p.Length > pos)
                return p.Substring(pos + 1, p.Length - pos - 1);
            return p;
        }

        private static string GetExtension(string fi) {
            if (fi.LastIndexOf('.') > 0)
                return fi.Substring(fi.LastIndexOf('.'), fi.Length - fi.LastIndexOf('.'));
            return "";
        }

        public static void TestFTPConnection(string ftpURL) {
            Exception ex = null;
            FTPConnection connection=null;
            try {
                connection = GetFtpConnection(ftpURL);
                connection.Connect();
            } catch (Exception e) {
                ex = e;
            } finally {
                if (connection != null) {
                    connection.Close();
                    connection.Dispose();
                }
            }
            if (ex != null)
                throw (ex);
        }

        private static FTPConnection GetFtpConnection(string filePath) {
            try {
                //ftp://user:password@host:port/path 
                string urlWithoutProtocol = filePath.Trim().Substring(6, filePath.Trim().Length - 6);
                string[] parts = urlWithoutProtocol.Split('@');

                FTPConnection connection = new FTPConnection();
                string hostPortPath = "";
                if (parts.Length == 2 && parts[0].Split(':').Length == 2) {
                    string userCredentials = parts[0];
                    string[] userPass = userCredentials.Split(':');
                    string userName = userPass[0];
                    string password = userPass[1];
                    connection.UserName = userName;
                    connection.Password = password;
                    hostPortPath = parts[1];
                } else
                    hostPortPath = parts[0];

                string host = "";
                int pos = hostPortPath.IndexOf('/');
                if (pos < 0)
                    host = hostPortPath;
                else {
                    string hostPort = hostPortPath.Substring(0, pos);
                    if (hostPort.Split(':').Length == 2) {
                        host = hostPort.Split(':')[0];
                        string port = hostPort.Split(':')[1];
                        connection.ServerPort = int.Parse(port);
                    } else
                        host = hostPort;

                    string hostDir = hostPortPath.Substring(pos + 1, hostPortPath.Length - pos - 1);
                    connection.ServerDirectory = hostDir;
                }
                connection.ServerAddress = host;

                return connection;
            } catch {
                return null;
            }
        }

        public static string GetFilePathFromConnectionString(string connectionString) {
            string retval = "";
            string[] parts = connectionString.Split(';');
            foreach (string part in parts) {
                if (part.Trim().ToLower().StartsWith("defaultdir=")) {
                    retval = part.Split('=')[1];
                    break;
                }
            }
            if (retval.EndsWith("\""))
                retval = retval.Substring(0, retval.Length - 1);
            if (retval.StartsWith("\""))
                retval = retval.Substring(1, retval.Length - 1);
            return retval;
        }

        //public static string GetCSVFilePathFromConnectionString(string connectionString) {
        //    string retval = "";
        //    string[] parts = connectionString.Split(';');
        //    foreach (string part in parts) {
        //        if (part.Trim().ToLower().StartsWith("dbq=")) {
        //            retval = part.Split('=')[1];
        //            break;
        //        }
        //    }
        //    return retval;
        //}


        //public static string CreateConnectionStringFromFilePath(string filePath, string localFilePath) {
        //    System.Data.Odbc.OdbcConnectionStringBuilder connectionStringBuilder = new System.Data.Odbc.OdbcConnectionStringBuilder();
        //    connectionStringBuilder.Driver = "Microsoft Text Driver (*.txt; *.csv)";
        //    connectionStringBuilder["EXTENSIONS"] = extension;
        //    //connectionStringBuilder["FILETYPE"] = "Text";
        //    //connectionStringBuilder["FORMAT"] = "DELIMITED(;)";
        //    connectionStringBuilder["DefaultDir"] = filePath;
        //    connectionStringBuilder["Dbq"] = localFilePath;
        //    return connectionStringBuilder.ConnectionString;
        //    //return string.Format(@"Driver={{}}; Extensions=csv; Dbq={0};DefaultDir={0};", filePath);
        //}

        public static string GetLocalFilePathFromRemote(string filePath, string defaultPath) {
            string retval = "";
            if (filePath.ToLower().Trim().StartsWith("ftp://")){
                string serverpath=filePath.Trim().Substring(6, filePath.Trim().Length - 6);
                if (serverpath.Length - serverpath.IndexOf('/') - 1 > 0)
                    serverpath = serverpath.Substring(serverpath.IndexOf('/') + 1, serverpath.Length - serverpath.IndexOf('/') - 1);
                else
                    serverpath = "";

                retval = string.Format(@"{0}\{1}", defaultPath, serverpath.Replace("/", @"\")).Replace(@"\\", @"\");
            } else {
                string[] parts = filePath.Split(':');
                retval = string.Format(@"{0}\{1}", defaultPath, parts[parts.Length - 1].Trim()).Replace(@"\\", @"\"); ;
            }
            if (retval.EndsWith(@"\")) {
                retval = retval.Substring(0, retval.Length - 1);
            }
            return retval;
        }
    }
}
