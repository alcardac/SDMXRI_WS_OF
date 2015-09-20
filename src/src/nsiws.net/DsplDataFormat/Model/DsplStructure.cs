using System.Collections.Generic;
namespace DsplDataFormat.Model
{

	 public class DsplStructure {

         public Info _info;
         public Provider _provider;
         public IList<Topic> _topics;
         public IList<Concept> _concepts;
         public IList<Slice> _slices;
         public IList<Table> _tables;


         public DsplStructure() {

             _topics = new List<Topic>();
             _concepts = new List<Concept>();
             _slices = new List<Slice>();
             _tables = new List<Table>();  
             //Pietro
  
         }

         //Pietro
         public struct Name {
             public string name;
             public string lang;
         }

         public struct Languages
         {
             public string lang;             
         }

         public struct Info {
             public string name;
             public string description;             
             public string url;

             //Pietro
             public IList<Name> Names;
             public IList<Name> Descriptions;
             public IList<Languages> Languages;
            }

           
          public struct Provider {
              public string name;
              public string url;
              //Pietro
              public IList<Name> Names;
              public IList<Languages> Languages;
            }

          public struct Topic
          {
              public string id;
              //Pietro
              public IList<Name> Names;
              public IList<Languages> Languages;
            }

          public struct Concept
          {
              public string id;
              public Info infoconcept;
              public string extends;
              public string topic;
              public string type;
              public Propertyconcept propertyconc;
              public string table;
              public Attribute attributeconcept;
            }

          public struct Propertyconcept
          {
              public string id;
              public Info infopropertyconcept;
              public string type;
              public string concept;
              public bool isparent;
            }

          public struct Attribute
          {
              public string type;
              public string value;
            }

          public struct Slice
          {
              public string id;
              public IList<string> dimension;
              public string metric;
              public string table;

            }

          public struct Table
          {
              public string id;
              public IList<Column> columntable;
              public string datafile_format;
              public string datafile_filename;   
            }

          public struct Column
          {
              public string id;
              public string type;
              public string value;
              public string format;
          }

          public struct Codelists
          {
              public string id;
              public IList<Name> Names;
              public IList<Languages> Languages;
          }
	}

}



