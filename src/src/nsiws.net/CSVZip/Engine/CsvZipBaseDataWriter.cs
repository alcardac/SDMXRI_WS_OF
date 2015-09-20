using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Model;
using Org.Sdmxsource.Sdmx.Api.Model.Header;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
using System.Threading;
using System.Configuration;


namespace CSVZip.Engine
{
    public class CsvZipBaseDataWriter : ICsvZipDataWriterEngine
    {
        #region Static Fields

        private readonly CsvZipTextWriter _CsvZipDataWriter;

        private static string[] ordine;
        private static string[] ordineser;

        private static string _status = "StartSeriesClose";

        private static string dirPath = "";

        private static string serie = "";

        #endregion

        #region Fields

        public StringBuilder csvdati = new StringBuilder();


        #endregion

        #region Constructor

        public CsvZipBaseDataWriter(CsvZipTextWriter writer)
        {
            this._CsvZipDataWriter = writer;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void StartDataset(string dsdName, IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, params IAnnotation[] annotations)
        {

            string appPath = System.Web.HttpRuntime.AppDomainAppPath;
            string giorno = System.Web.HttpContext.Current.Timestamp.Day.ToString();
            string ora = System.Web.HttpContext.Current.Timestamp.Hour.ToString();
            string min = System.Web.HttpContext.Current.Timestamp.Minute.ToString();
            string secondi = System.Web.HttpContext.Current.Timestamp.Second.ToString();
            string ms = System.Web.HttpContext.Current.Timestamp.Millisecond.ToString();
            string namedir = giorno + ora + min + secondi + ms;
            
            dirPath = appPath + namedir;

            DirectoryInfo MyRoot = new DirectoryInfo(@appPath);
            MyRoot.CreateSubdirectory(namedir);

            IList<IDimension> dimensions = dsd.GetDimensions(SdmxStructureEnumType.Dimension);
            string[] ordinamento1 = new string[dimensions.Count];
            string[] ordinamento2 = new string[dsd.Components.Count];


            ordine = new string[dimensions.Count];
            ordineser = new string[dimensions.Count];
            int indord = 0;

            string valoredati = "";

            IList<IDimension> dimensions1 = dsd.GetDimensions(SdmxStructureEnumType.Dimension);
            foreach (IDimension dim in dimensions1)
            {
                _CsvZipDataWriter.WriteValue(dim.Id + "_code,");
                valoredati += dim.Id + "_code,";
                ordine[indord] = dim.Id;
                indord++;
            }

            valoredati += "VALUE,date";
            string newLinedati = string.Format("{0}{1}", valoredati, Environment.NewLine);

            csvdati.Append(newLinedati);

        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void StartGroup(string groupId, params IAnnotation[] annotations)
        {
            throw new NotImplementedException();
        }

        public void StartSeries(params IAnnotation[] annotations)
        {

            if (CsvZipBaseDataWriter._status.Equals("StartSeriesOpen") || _status.Equals("WriteObservation"))
            {
                serie = "";
            }
            CsvZipBaseDataWriter._status = "StartSeriesOpen";

        }

        public void WriteAttributeValue(string id, string valueRen)
        {
            //throw new NotImplementedException();

        }

        public void WriteGroupKeyValue(string id, string valueRen)
        {
            throw new NotImplementedException();
        }

        public void WriteObservation(DateTime obsTime, string obsValue, TimeFormat sdmxSwTimeFormat, params IAnnotation[] annotations)
        {
            string tt = obsTime + "," + obsValue;
            this._CsvZipDataWriter.WriteValue(tt);
        }

        public void WriteObservation(string observationConceptId, string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            //throw new NotImplementedException();
        }

        public void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {

            if (CsvZipBaseDataWriter._status.Equals("StartSeriesOpen"))
            {
                CsvZipBaseDataWriter._status = "WriteObservation";

                for (int i = 0; i < ordineser.Length; i++)
                {
                    serie += ordineser[i] + ",";
                }

            }

            string totobs = obsValue + "," + obsConceptValue;

            string valoredati = "";
            valoredati += serie + totobs;
            string newLinedati = string.Format("{0}{1}", valoredati, Environment.NewLine);

            csvdati.Append(newLinedati);

        }

        public void WriteSeriesKeyValue(string id, string value_ren)
        {
            for (int i = 0; i < ordine.Length; i++)
            {
                if (ordine[i] == id)
                {
                    ordineser[i] = value_ren;
                }
            }
        }



        public void Close(params IFooterMessage[] footer)
        {

            File.WriteAllText(dirPath + "\\" + "DATI.csv", csvdati.ToString());

            DirectoryInfo directorySelected = new DirectoryInfo(dirPath);

            string[] pasta = dirPath.Split('\\');
            var total = pasta.Length;
           
               


            DirectoryInfo directory = new DirectoryInfo(dirPath.Replace("\\" + pasta[total - 1], ""));

            this._CsvZipDataWriter.CompressDirectory(directory, directorySelected);

            Thread.Sleep(1000);
            
            bool isZipped = Convert.ToBoolean(ConfigurationManager.AppSettings["isZipped"]);
            var bct = System.Web.HttpContext.Current.Response.ContentType;

            if (bct.Substring(0, 8) == "text/csv")
                isZipped = false;
            else
                isZipped = true;

            DownloadFile(dirPath, pasta[total - 1], isZipped);

            
            this._CsvZipDataWriter.CleanDirectory(directorySelected);
            this._CsvZipDataWriter.DeleteDirectory(directory, directorySelected);

            dirPath = "";
            Thread.Sleep(1000);
        }


        public void DownloadFile(string downloadFilePath, string filename, bool isZipped)
        {

            if (isZipped)
            {
                downloadFilePath = downloadFilePath + ".zip";            
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".zip");    
            }
            else
            {
                //downloadFilePath = downloadFilePath + "\\" + filename + ".csv";
                downloadFilePath = downloadFilePath + "\\DATI.csv";
                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".csv");    
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename="  +"\\DATI.csv");
            }
            
            System.Web.HttpContext.Current.Response.WriteFile(downloadFilePath);
            System.Web.HttpContext.Current.Response.End();

        }

        void ICsvZipDataWriterEngine.WriteAttributeValue(string id, string valueRen)
        {
            // throw new NotImplementedException();            
        }

        public void WriteHeader(IHeader header)
        {
            // throw new NotImplementedException();
        }

        void System.IDisposable.Dispose()
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}

