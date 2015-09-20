using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstatExtension.DataWriter.Model
{
    public class SDMXJsonStructure
    {

       private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private List<SDMXJsonDimension> _series;
        public List<SDMXJsonDimension> Series
        {
            get { return _series; }
            set { _series = value; }
        }

        private SDMXJsonObservation _observation;
        public SDMXJsonObservation Observation
        {
            get { return _observation; }
            set { _observation = value; }
        }

        private List<SDMXJsonAttributes> _attributes;
        public List<SDMXJsonAttributes> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
        

        public SDMXJsonStructure()
        {
            this.Observation = new SDMXJsonObservation();           
            this.Series = new List<SDMXJsonDimension>();            
            this.Attributes = new List<SDMXJsonAttributes>();
        }

    

    }
}
