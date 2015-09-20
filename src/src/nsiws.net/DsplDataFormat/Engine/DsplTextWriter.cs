using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Security.Principal;
using System.Security.AccessControl;

namespace DsplDataFormat.Engine
{
    public class DsplTextWriter : IDisposable
    
    {

        private readonly TextWriter _writer;

        public string ObsValueType;

        public static FileStream compressedFileStream;

        public DsplTextWriter(TextWriter writer)
        {
            _writer = writer != null ? writer : new StringWriter();
        } 

        public void WriteValue(string p)
        {
            _writer.Write(p);
        }

        public void WriteValue(DateTime p)
        {
            _writer.Write(p);
        }

        public void Dispose()
        {

            _writer.Close();
            //throw new System.NotImplementedException();
        }

        public void Close() {

            _writer.Close();

        }

        public void CompressFile (DirectoryInfo directorySelected) 
        {

            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                Compress(fileToCompress);                
            }
        
        }

        public void CleanDirectory(DirectoryInfo directorySelected)
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                fileToCompress.Delete();                
            }
        }
        public void DeleteDirectory(DirectoryInfo directorySelected, DirectoryInfo directory)
        {
            foreach (DirectoryInfo directoryToCompress in directorySelected.GetDirectories())
            {
                if (directoryToCompress.FullName == directory.FullName)
                {
                    directoryToCompress.Refresh();
                    directoryToCompress.Delete();
                }
            } 
        }
        public void CompressDirectory(DirectoryInfo directorySelected, DirectoryInfo directory)
        {
            foreach (DirectoryInfo directoryToCompress in directorySelected.GetDirectories())
            {
                if (directoryToCompress.FullName == directory.FullName)
                {
                    using (ZipFile zip = new ZipFile())
                    {                        
                        SetAccessDirectory(directorySelected.FullName);                      
                        zip.AddItem(directory.FullName);
                        zip.TempFileFolder = directory.FullName;
                        zip.Save(directory.FullName + ".zip");
                    }  
                }            
            }
        }

        public static void SetAccessDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            // Pega a segurança atual da pasta
            DirectorySecurity oDirSec = Directory.GetAccessControl(dirPath);
            // Define o usuário Everyone (Todos)
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            //SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            NTAccount oAccount = sid.Translate(typeof(NTAccount)) as NTAccount;

            oDirSec.PurgeAccessRules(oAccount);

            FileSystemAccessRule fsAR = new FileSystemAccessRule(oAccount,
                                                                 FileSystemRights.Modify,
                                                                 InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                                                 PropagationFlags.None,
                                                                 AccessControlType.Allow);
            // Atribui a regra de acesso alterada
            oDirSec.SetAccessRule(fsAR);
            Directory.SetAccessControl(dirPath, oDirSec);
        }

        public static void Compress(DirectoryInfo directoryToCompress)
        {
            if ((File.GetAttributes(directoryToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & directoryToCompress.Extension != ".gz")
            {
                compressedFileStream = File.Create(directoryToCompress.FullName + ".zip");              
            }
        }


        public static void Compress(FileInfo fileToCompress)
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (compressedFileStream = File.Create(fileToCompress.FullName + ".zip"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            
                            originalFileStream.CopyTo(compressionStream);
                            
                        }
                    }
                }
            }
        }
        
    }
}
