using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DsplDataFormat.Model;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Model;
using Org.Sdmxsource.Sdmx.Api.Model.Header;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
using System.Web.Configuration;


namespace DsplDataFormat.Engine
{
    public class DsplBaseDataWriter : IDsplDataWriterEngine , IDisposable
    {
        #region Static Fields

            private readonly DsplTextWriter _dsplDataWriter;
            
            private static bool trovato;
            private static string[] ordine;
            private static string[] ordineser;
                        
            private string dateFormat;

            private static string _status = "StartSeriesClose";

            private static string dirPath = "";

            private static string serie = "";

            private static DsplStructure myDsplStructure = new DsplStructure();

        #endregion

        #region Fields

                public StringBuilder csvdati = new StringBuilder();
                public string ValueType = "";
                private string freqfieldcode = "";
                

        #endregion

        #region Constructor

            public DsplBaseDataWriter(DsplTextWriter writer)
        {
            this._dsplDataWriter = writer;            
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public string GetWebConfigInfo(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        public bool VerifyParentCode(IList<ICode> codes)
        {
               foreach (ICode codeBean in codes)
                {
                    if (codeBean.ParentCode != null)
                    {
                        return true; 
                    }
                }
               return false;
        }

        public List<DsplStructure.Languages> VerifyNames(IDataflowObject dataflow)
        {
            var Languages = new List<DsplStructure.Languages>();
            var Lang = new DsplStructure.Languages();
            bool exists = new bool();

                foreach (var item in dataflow.Names)
                {
                    foreach (var lang in Languages)
                    {
                        if (lang.lang == item.Locale)
                        {
                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        Lang.lang = item.Locale;
                        Languages.Add(Lang);    
                    }                                 
                }
            return Languages;
            
        }

        public void AdjustNames(DsplStructure.Codelists codelisNames, string Name)
        {
            bool exists = new bool();            
            foreach (var Lang in myDsplStructure._info.Languages)
            {
                foreach (var lang in codelisNames.Languages)
                {                    
                    if (lang.lang == Lang.lang)
                    {
                        exists = true;                        
                    }
                }
                if (!exists)
                {
                    var names = new DsplStructure.Name();
                    names.lang = Lang.lang;
                    names.name = Name;
                    codelisNames.Names.Add(names);
                }
                exists = false;
            }
        }

        public void AdjustNames(DsplStructure.Info InfoNames, string Name)
        {
            bool exists = new bool();
            foreach (var Lang in myDsplStructure._info.Languages)
            {
                foreach (var lang in InfoNames.Names)
                {
                    if (lang.lang == Lang.lang)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    var names = new DsplStructure.Name();
                    names.lang = Lang.lang;
                    names.name = Name;
                    InfoNames.Names.Add(names);
                }
                exists = false;
            }
        }

        public void AdjustDescriptions(DsplStructure.Info InfoNames, string Description)
        {
            bool exists = new bool();
            foreach (var Lang in myDsplStructure._info.Languages)
            {
                foreach (var lang in InfoNames.Languages)
                {
                    if (lang.lang == Lang.lang)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    var names = new DsplStructure.Name();
                    names.lang = Lang.lang;
                    names.name = Description;
                    InfoNames.Descriptions.Add(names);
                }
                exists = false;
            }
        }

        public void AdjustLanguages(DsplStructure.Info InfoNames)
        {
            bool exists = new bool();
            foreach (var DsdlLang in myDsplStructure._info.Languages)
            {
                foreach (var lang in InfoNames.Languages)
                {
                    if (lang.lang == DsdlLang.lang)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    var Langs = new DsplStructure.Languages();
                    Langs.lang = DsdlLang.lang;
                    InfoNames.Languages.Add(Langs);
                }
                exists = false;
            }
        }

        public void SetDsdOrder(IDataStructureObject dsd)        
        {
            IList<IDimension> dimensions = dsd.GetDimensions(SdmxStructureEnumType.Dimension);
            ordine = new string[dimensions.Count];
            ordineser = new string[dimensions.Count];
        }

        public void StartDataset(string dsdName, IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, params IAnnotation[] annotations)
        {
            myDsplStructure._info.Names = new List<DsplStructure.Name>();
            myDsplStructure._info.Descriptions = new List<DsplStructure.Name>();
            myDsplStructure._info.Languages = new List<DsplStructure.Languages>();

            myDsplStructure._provider.Names = new List<DsplStructure.Name>();            
            foreach (var item in dataflow.Names)
            {
                var names = new DsplStructure.Name();
                names.name = item.Value;
                names.lang = item.Locale;
                
                //info
                myDsplStructure._info.Names.Add(names);
                myDsplStructure._info.url = GetWebConfigInfo("urlAddress");                

                //provider            
                myDsplStructure._provider.Names.Add(names);
                myDsplStructure._provider.url = GetWebConfigInfo("urlAddress");
            }
            myDsplStructure._info.Languages = VerifyNames(dataflow);
                
            myDsplStructure._info.description= dsdName;
            

            var topic = new DsplStructure.Topic();         
            topic.id = dataflow.Name.PadRight(64);
            topic.Names = new List<DsplStructure.Name>();

            foreach (var item in dataflow.Names)
            {
                var names = new DsplStructure.Name();
                names.name = item.Value;
                names.lang = item.Locale;                
                topic.Names.Add(names);
            }

            myDsplStructure._topics.Add(topic);

            var table = new DsplStructure.Table();
            var colonne = new DsplStructure.Column(); 

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

            string codelist = "";
            string agenzia = "";
            string versione = "";

            var concept = new DsplStructure.Concept();
				
			// metrics
         	concept.id = "VALUE";                      
            concept.type = "float";
                   
            //Pietro
            concept.infoconcept.Names = new List<DsplStructure.Name>();
            concept.infoconcept.Descriptions = new List<DsplStructure.Name>();

                   
            foreach (var item in dataflow.Names)
            {
                var names = new DsplStructure.Name();
                names.lang = item.Locale;
                names.name = item.Value;
                concept.infoconcept.Names.Add(names);
            }
            myDsplStructure._concepts.Add(concept);

            var slices = new DsplStructure.Slice();
            slices.id = "DATI";

            slices.dimension = new List<String>();

            int indord = 0;

            foreach (IDimension dim in dimensions)
            {
                ISet<ICrossReference> isr = dim.Representation.CrossReferences;
                foreach (var x in isr)
                {
                    codelist = x.MaintainableReference.MaintainableId;
                    agenzia = x.MaintainableReference.AgencyId;
                    versione = x.MaintainableReference.Version;
                }

                string IDDimension = dim.ConceptRef.FullId.ToString();

                if (dim.FrequencyDimension == true)
                    freqfieldcode = codelist;

                slices.dimension.Add(dim.Id + "_code");
                colonne.id = dim.Id;
                colonne.type = "string";
                table.columntable = new List<DsplStructure.Column>();                       

                string namefiledim = "";
                StringBuilder csv = new StringBuilder();
                trovato = false;

                using (DsplMetadataEngine GetCodelists = new DsplMetadataEngine(agenzia, codelist, versione))
                {
                        ISet<ICodelistObject> codelists = GetCodelists.GetCodelistStruc();
                        if (codelists.Count > 0)
                        {
                            namefiledim = dirPath + "\\" + dim.Id.ToLower() + ".csv";                            
                            string headLine = "";
                            bool BheadLine = true;
                            foreach (ICodelistObject codelistBean in codelists)
                            {                              
                                concept = new DsplStructure.Concept();
                                concept.id = dim.Id + "_code";
                                concept.type = "string";                              

                                concept.infoconcept.Names = new List<DsplStructure.Name>();
                                concept.infoconcept.Descriptions = new List<DsplStructure.Name>();
                                concept.infoconcept.Languages = new List<DsplStructure.Languages>();
                               
                                foreach (var item in codelistBean.Names)
                                {
                                    var names = new DsplStructure.Name();
                                    var langs = new DsplStructure.Languages();

                                    langs.lang = item.Locale;
                                    names.lang = item.Locale;
                                    names.name = item.Value;
                                    concept.infoconcept.Languages.Add(langs);
                                    concept.infoconcept.Names.Add(names);
                                 }
                        
                                foreach (var item in codelistBean.Descriptions)
                                {
                                    var names = new DsplStructure.Name();
                                    names.lang = item.Locale;
                                    names.name = item.Value;
                                    concept.infoconcept.Descriptions.Add(names);
                                }

                                if (concept.infoconcept.Languages.Count != myDsplStructure._info.Languages.Count)
                                {
                                    AdjustLanguages(concept.infoconcept);
                                    AdjustNames(concept.infoconcept, concept.infoconcept.Names[0].name);                           
                                }

                                if (BheadLine)
                                {
                                    foreach (var item in concept.infoconcept.Languages)
                                    {
                                        headLine = headLine + "name_" + item.lang + ",";
                                        BheadLine = false;
                                    }
                                } 

                                concept.table = dim.Id;

                                myDsplStructure._concepts.Add(concept);
                                string valore = "";
                                string newLine = "";
                                IList<ICode> codes = codelistBean.Items;

                                if (codes.Count > 0)
                                {
                                    foreach (ICode codeBean in codes)
                                    {
                                        var hasParent = VerifyParentCode(codes);
                                        var codelisNames = new DsplStructure.Codelists();
                                        codelisNames.Names = new List<DsplStructure.Name>();
                                        codelisNames.Languages = new List<DsplStructure.Languages>();

                                        foreach (var item in concept.infoconcept.Names)
                                        {
                                            foreach (var codeName in codeBean.Names)
                                            {
                                                if (item.lang == codeName.Locale)
                                                {
                                                    var names = new DsplStructure.Name();
                                                    var Lang = new DsplStructure.Languages();
                                                    Lang.lang = codeName.Locale;
                                                    names.lang = codeName.Locale;
                                                    names.name = codeName.Value.Replace('"', ' ');
                                                    codelisNames.id = codeBean.Id;
                                                    codelisNames.Names.Add(names);
                                                    codelisNames.Languages.Add(Lang);    
                                                }                                        
                                            }
                                        }

                                        if (codelisNames.Languages.Count != myDsplStructure._info.Languages.Count)
                                        {
                                            AdjustNames(codelisNames, codelisNames.Names[0].name);
                                        }
                    
                                        if (hasParent)
                                        {
                                            trovato = true;
                                            var pc = codeBean.ParentCode;


                                            foreach (var value in codelisNames.Names)
                                            {
                                                valore = valore + value.name.Replace(',','.') + ",";
                                            }
                                            valore = valore + codelisNames.id + "," + pc;

                                        }
                                        else {
                                            foreach (var value in codelisNames.Names)
                                            {
                                                valore = valore + value.name.Replace(',', '.') + ",";
                                            }
                                            valore = valore + codelisNames.id;

                                        }
                           
                                        newLine = string.Format("{0}{1}", valore, Environment.NewLine);
                                        valore = "";
                                        csv.Append(newLine);                                                                              
                                    }
                                }

                                if (trovato == true)                               
                                {
                                    valore = headLine + dim.Id + "_code,parent";
                                }
                                else {
                                    valore = headLine + dim.Id + "_code";
                                }

                                table.id = dim.Id;
                                foreach (var item in headLine.Split(','))
                                {
                                    if (item != "")
                                    {
                                        colonne.id = item;
                                        colonne.type = "string";
                                        colonne.value = "";
                                        table.columntable.Add(colonne);
                                    }
                                }                                                           


                                colonne.id = dim.Id + "_code";
                                colonne.type = "string";
                                colonne.value = "";


                                table.columntable.Add(colonne);
                                if (trovato == true) {
                                    colonne.id = "parent";
                                    colonne.type = "string";
                                    colonne.value = "";
                                   
                                    table.columntable.Add(colonne);     
                                }

                                table.datafile_filename = dim.Id.ToLower() + ".csv";

                                myDsplStructure._tables.Add(table);

                                newLine = string.Format("{0}{1}", valore, Environment.NewLine);
                                csv.Insert(0, newLine);
                              
                            }                         
                        }
                    }
                File.WriteAllText(namefiledim, csv.ToString());                                          
            }


                   table.id = "DATI_TBL";               
                   
                   colonne.value = "";
                   table.columntable = new List<DsplStructure.Column>();
                   
                
                string valoredati = "";

                IList<IDimension> dimensions1 = dsd.GetDimensions(SdmxStructureEnumType.Dimension);
                foreach (IDimension dim in dimensions1)
                {
                    if (!dim.Id.Equals("FREQ"))
                    {
                        _dsplDataWriter.WriteValue(dim.Id + "_code,");
                        valoredati += dim.Id + "_code,";
                        colonne.id = dim.Id + "_code";
                        colonne.type = "string";
                        table.columntable.Add(colonne);
                        ordine[indord] = dim.Id;
                        indord++;
                    }
                }
                _dsplDataWriter.WriteValue("FREQ_code,");
                colonne.id = "FREQ_code";
                colonne.type = "string";
                table.columntable.Add(colonne);
                ordine[indord] = "FREQ";

                _dsplDataWriter.WriteValue("date,");

                _dsplDataWriter.WriteValue("VALUE");
                slices.metric = "VALUE";
                colonne.id = "VALUE";

                colonne.type = "float";
                table.columntable.Add(colonne);

                table.datafile_filename = "DATI.csv";
                myDsplStructure._tables.Add(table);


                _dsplDataWriter.WriteValue("\n");
                slices.table = "DATI_TBL";
                
                myDsplStructure._slices.Add(slices);

                valoredati += "FREQ_code,VALUE,date";
                string newLinedati = string.Format("{0}{1}", valoredati, Environment.NewLine);

                csvdati.Append(newLinedati);   
        }

        public void WriteTag(XmlTextWriter writer, string FirstElement, string SecondElement, string Lang, string Value)
        {
            writer.WriteStartElement(FirstElement);
            writer.WriteStartElement(SecondElement);
            writer.WriteAttributeString("xml", "lang", null, Lang);
            writer.WriteValue(Value);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void WriteTagValue(XmlTextWriter writer, string SecondElement, string Lang, string Value)
        {
            writer.WriteStartElement(SecondElement);
            writer.WriteAttributeString("xml", "lang", null, Lang);
            writer.WriteValue(Value);
            writer.WriteEndElement();
        }

        public void WriteImports(XmlTextWriter writer, string key)
        {
            writer.WriteStartElement("import");
            writer.WriteAttributeString("namespace", GetWebConfigInfo(key));
            writer.WriteEndElement();
        }
        
        public void WriteNamespaces(XmlTextWriter writer, string domain ,string key)
        {
            writer.WriteAttributeString("xmlns", domain, null, GetWebConfigInfo(key));
        }

        
        public void WriteDSPLXml(string filenamexml)
        {
            filenamexml += "\\Dspl.xml";
            
             XmlTextWriter writer = new XmlTextWriter(filenamexml,null);
            writer.Formatting = Formatting.Indented;
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            writer.WriteStartElement("dspl");
            writer.WriteAttributeString("xmlns", GetWebConfigInfo("DsplSchema"));
  
            //Namespaces
            WriteNamespaces(writer, "entity", "UriEntity");
            WriteNamespaces(writer, "metrics", "UriMetrics");
            WriteNamespaces(writer, "quantity", "UriQuantity");
            WriteNamespaces(writer, "geo", "UriGeo");
            WriteNamespaces(writer, "time", "UriTime");
            //-----------------------

            //Imports
            WriteImports(writer, "UriTime");
            WriteImports(writer, "UriQuantity");
            WriteImports(writer, "UriEntity");
            WriteImports(writer, "UriGeo");
            WriteImports(writer, "UriMetrics");
            //-----------------------

            // info
            writer.WriteStartElement("info");
            writer.WriteStartElement("name");
            foreach (var item in myDsplStructure._info.Names)
            {
                WriteTagValue(writer, "value", item.lang, item.name);
            }
            writer.WriteEndElement();

            //Controllare 
            writer.WriteStartElement("description");
            foreach (var item in myDsplStructure._info.Languages)
            {
                WriteTagValue(writer, "value", item.lang, myDsplStructure._info.description);
            } 
            writer.WriteEndElement();


            writer.WriteStartElement("url");
            foreach (var item in myDsplStructure._info.Languages)
            {
                WriteTagValue(writer, "value", item.lang, myDsplStructure._info.url);
            } 
            writer.WriteEndElement();

            writer.WriteEndElement();


            //----------------------

            // provider
            writer.WriteStartElement("provider");

            writer.WriteStartElement("name");
            foreach (var item in myDsplStructure._provider.Names)
            {
                WriteTagValue(writer, "value", item.lang, item.name);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("url");
            foreach (var item in myDsplStructure._info.Languages)
            {
                WriteTagValue(writer, "value", item.lang, myDsplStructure._provider.url);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();

            //-----------------------

            //<topics>
            writer.WriteStartElement("topics");
            for (int i = 0; i < myDsplStructure._topics.Count ; i++){
                writer.WriteStartElement("topic");
                    string x = myDsplStructure._topics[i].id;
                    // x.Replace(' ', '_');
                    x = Regex.Replace(x, @"[^\w\._]", " ", RegexOptions.None);
                    string pattern = "\\s+";
                    Regex rgx = new Regex(pattern);
                    string y = rgx.Replace(x, " ");

                    if (y.Length >=63)
                    {
                        writer.WriteAttributeString("", "id", null, y.Replace(' ', '_').Substring(0, 63));
                    }
                    else
                    {
                        writer.WriteAttributeString("", "id", null, y.Replace(' ', '_'));
                    }
                    

                    writer.WriteStartElement("info");

                    //Pietro
                    writer.WriteStartElement("name");
                        foreach (var item in myDsplStructure._topics[i].Names)
                        {                        
                            WriteTagValue(writer, "value", item.lang, item.name);                        
                        }
                    writer.WriteEndElement();

                    writer.WriteEndElement();                    
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); 
            //<topics/>

            // <concepts>
            writer.WriteStartElement("concepts");

            for (int i = 0; i < myDsplStructure._concepts.Count; i++)
            {
                writer.WriteStartElement("concept");
                writer.WriteAttributeString("", "id", null, myDsplStructure._concepts[i].id);

                if (!myDsplStructure._concepts[i].id.Contains("VALUE")&& (!myDsplStructure._concepts[i].table.Contains("AREA")))
                {
                    writer.WriteAttributeString("extends", "entity:entity");
                }
                else if ((myDsplStructure._concepts[i].table != null) && (myDsplStructure._concepts[i].id.Contains("AREA")))
                {
                    writer.WriteAttributeString("extends", "geo:location");
                }

                writer.WriteStartElement("info");

                writer.WriteStartElement("name");
                    foreach (var item in myDsplStructure._concepts[i].infoconcept.Names)
                    {                    
                        WriteTagValue(writer, "value", item.lang, item.name);                    
                    }
                writer.WriteEndElement();

                if (myDsplStructure._concepts[i].infoconcept.Descriptions.Count > 0)
	            {
                    writer.WriteStartElement("description");
                    foreach (var item in myDsplStructure._concepts[i].infoconcept.Descriptions)
                    {
                        WriteTagValue(writer, "value", item.lang, item.name);
                    }
                    writer.WriteEndElement();
                }
                else
                {
                    WriteTag(writer, "description","value", "", "");
                }

                writer.WriteStartElement("url");
                    foreach (var item in myDsplStructure._concepts[i].infoconcept.Names)
                    {                    
                        WriteTagValue(writer, "value", item.lang , myDsplStructure._concepts[i].infoconcept.url);                    
                    }
                writer.WriteEndElement();

                writer.WriteEndElement();

                if (myDsplStructure._concepts[i].topic != null)
                {
                    writer.WriteStartElement("topic");
                    writer.WriteAttributeString("ref", myDsplStructure._concepts[i].topic);
                    writer.WriteEndElement();
                }

                writer.WriteStartElement("type");
                writer.WriteAttributeString("ref", myDsplStructure._concepts[i].type);
                writer.WriteEndElement();

                if (myDsplStructure._concepts[i].id.Contains("REF_AREA") || (myDsplStructure._concepts[i].table != null && myDsplStructure._concepts[i].table.Contains("REFAREA")))
                {
                    writer.WriteStartElement("property");
                    writer.WriteAttributeString("id", "parent");
                    writer.WriteAttributeString("concept", myDsplStructure._concepts[i].id);
                    writer.WriteAttributeString("isParent", "true");
                    writer.WriteEndElement();
                }

                if (myDsplStructure._concepts[i].table != null)
                {
                    writer.WriteStartElement("table");
                    writer.WriteAttributeString("ref", myDsplStructure._concepts[i].table);
                    //Pietro
                    foreach (var item in myDsplStructure._concepts[i].infoconcept.Names)
                    {
                        writer.WriteStartElement("mapProperty");
                        writer.WriteAttributeString("ref", "name");
                        writer.WriteAttributeString("xml:lang", item.lang);
                        writer.WriteAttributeString("toColumn", "name_" + item.lang);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }


                writer.WriteEndElement();
            }

            writer.WriteEndElement(); 
            // <concepts/>


            // <slices>
            writer.WriteStartElement("slices");

            for (int i = 0; i < myDsplStructure._slices.Count; i++)
            {
                writer.WriteStartElement("slice");
                writer.WriteAttributeString("", "id", null, myDsplStructure._slices[i].id);

                for (int ii = 0; ii < myDsplStructure._slices[i].dimension.Count; ii++)
                {
                    writer.WriteStartElement("dimension");
                    writer.WriteAttributeString("concept", myDsplStructure._slices[i].dimension[ii]);
                    writer.WriteEndElement(); // dimension
                
                }

                writer.WriteStartElement("dimension");
                if (this.dateFormat == "A")
                {
                    writer.WriteAttributeString("concept", "time:year");
                }
                else if (this.dateFormat == "M")
                {
                    writer.WriteAttributeString("concept", "time:month");
                }
                else if (this.dateFormat == "Q")
                {
                    writer.WriteAttributeString("concept", "time:month");
                }
                writer.WriteEndElement(); // date dimension

                writer.WriteStartElement("metric");
                writer.WriteAttributeString("concept", myDsplStructure._slices[i].metric);
                writer.WriteEndElement();
                writer.WriteStartElement("table");
                writer.WriteAttributeString("ref", myDsplStructure._slices[i].table);
                writer.WriteStartElement("mapDimension");


                if (this.dateFormat == "A")
                {
                    writer.WriteAttributeString("concept", "time:year");
                }
                else if (this.dateFormat == "M")
                {
                    writer.WriteAttributeString("concept", "time:month");                
                }
                else if (this.dateFormat == "Q")
                {
                    writer.WriteAttributeString("concept", "time:month");
                } 

                writer.WriteAttributeString("toColumn", "date");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement(); //slice                
            }

            writer.WriteEndElement(); 
            //<slices/>


            SetDateSlice(this.dateFormat);


            // <tables>
            writer.WriteStartElement("tables");

            for (int i = 0; i < myDsplStructure._tables.Count; i++)
            {
                writer.WriteStartElement("table");
                writer.WriteAttributeString("", "id", null, myDsplStructure._tables[i].id);

                for (int ii = 0; ii < myDsplStructure._tables[i].columntable.Count; ii++)
                {
                    writer.WriteStartElement("column");
                    writer.WriteAttributeString("id", myDsplStructure._tables[i].columntable[ii].id);
                    writer.WriteAttributeString("type", myDsplStructure._tables[i].columntable[ii].type);
                    if (myDsplStructure._tables[i].columntable[ii].format != null)
                         writer.WriteAttributeString("format", myDsplStructure._tables[i].columntable[ii].format);
                    writer.WriteEndElement(); // column

                }

                writer.WriteStartElement("data");

                writer.WriteStartElement("file");
                writer.WriteAttributeString("encoding", "utf-8");
                writer.WriteAttributeString("format", "csv");
                writer.WriteValue(myDsplStructure._tables[i].datafile_filename);
                writer.WriteEndElement();

                writer.WriteEndElement();
                
                writer.WriteEndElement(); //table
            }

            writer.WriteEndElement(); 
            // <tables/>


            writer.WriteEndElement(); // root
            writer.Flush();
            writer.Close();          

        }

        public void SetDateSlice(string FreqType)
        {
            var table = new DsplStructure.Table();
            var colonne = new DsplStructure.Column();
            var slices = new DsplStructure.Slice();

            table.columntable = new List<DsplStructure.Column>();

            foreach (var myTable in myDsplStructure._tables)
            {
                if (myTable.id == "DATI_TBL")
                {
                    table = myTable;
                }
            }

            slices.dimension = new List<String>();

            if (FreqType == "A")
            {
                slices.dimension.Add("time:year");
                colonne.id = "date";
                colonne.type = "date";
                colonne.format = "yyyy";
                table.columntable.Add(colonne);

            }
            else if (FreqType == "M")
            {
                slices.dimension.Add("time:month");
                colonne.id = "date";
                colonne.type = "date";
                colonne.format = "yyyy.MM";
                table.columntable.Add(colonne);
            }
            else if (true)
            {
                slices.dimension.Add("time:month");
                colonne.id = "date";
                colonne.type = "date";
                colonne.format = "yyyy.MM";
                table.columntable.Add(colonne); 
            }
           
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

            if (DsplBaseDataWriter._status.Equals("StartSeriesOpen") || _status.Equals("WriteObservation"))
            {             
                serie = "";
            }       
            DsplBaseDataWriter._status = "StartSeriesOpen";

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
            this._dsplDataWriter.WriteValue(tt);
        }

        public void WriteObservation(string observationConceptId, string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            //throw new NotImplementedException();
        }

        public void WriteObservation(string obsConceptValue, string obsValue, params IAnnotation[] annotations)
        {
            
            if (DsplBaseDataWriter._status.Equals("StartSeriesOpen"))
            {
               // this._dsplDataWriter.WriteValue("\n");
                DsplBaseDataWriter._status = "WriteObservation";

                for (int i = 0; i < ordineser.Length; i++)
                {
                    serie += ordineser[i] + ",";
                }
                
            }

            if (!obsValue.Contains("."))
            {
                obsValue = obsValue + ".0";
            }

            string totobs = obsValue + "," + obsConceptValue.Replace('-', '.').Replace("Q1", "01").Replace("Q2", "04").Replace("Q3", "07").Replace("Q4", "10");

            string valoredati = "";
            valoredati += serie + totobs;
            string newLinedati = string.Format("{0}{1}", valoredati, Environment.NewLine);

            csvdati.Append(newLinedati);
           
        }

        public void WriteSeriesKeyValue(string id, string value_ren)
        {
            if (id == "FREQ")
            {
                this.dateFormat = value_ren;
            }
            for (int i = 0; i < ordine.Length; i++) {
                if (ordine[i] == id)
                {
                    ordineser[i] = value_ren;

                }
            
            }     
        }

        public void WriteHeader(IHeader header)
        {
           myDsplStructure._info.description = header.Name[0].Value;    
        }

        public void Close(params IFooterMessage[] footer)
        {
            File.WriteAllText(dirPath + "\\" + "DATI.csv", csvdati.ToString());

            DirectoryInfo directorySelected = new DirectoryInfo(dirPath);

            this.WriteDSPLXml(dirPath);

            string[] pasta = dirPath.Split('\\');
            var total = pasta.Length;

            DirectoryInfo directory = new DirectoryInfo(dirPath.Replace("\\" + pasta[total - 1], ""));

            this._dsplDataWriter.CompressDirectory(directory, directorySelected);
            this._dsplDataWriter.CleanDirectory(directorySelected);            
            this._dsplDataWriter.DeleteDirectory(directory, directorySelected);       
          
            DownloadFile(dirPath, pasta[total - 1]);

            dirPath = "";            
        }


        public void DownloadFile(string downloadFilePath, string filename)
        {
            downloadFilePath = downloadFilePath + ".zip";

            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".zip");
            System.Web.HttpContext.Current.Response.WriteFile(downloadFilePath);            

        }
        

       void IDsplDataWriterEngine.WriteAttributeValue(string id, string valueRen)
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

