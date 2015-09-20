using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstatExtension.DataWriter.Model
{
    public class SDMXJsonObservation
    {
        private string _role;

        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        
        private List<SDMXJsonValue> _values;
        public List<SDMXJsonValue> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public SDMXJsonObservation ()
        {
            this.Values = new List<SDMXJsonValue>();
        }


        public bool ExistValue(SDMXJsonValue Val)
        {
            bool Exists = false;
            foreach (var item in this.Values)
            {
                if (item.Id == Val.Id && item.Descr == Val.Descr)
                {
                    Exists = true;
                    break;
                }
            }
            return Exists;
        }

        public int GetLastIndex(SDMXJsonValue Val)
        {
            int Index = 0;
            foreach (var Value in this.Values)
            {
                if (Value.Index > Index && !(Value.Index == null))
                {
                    Index = Value.Index;
                }
            }
            return Index;
        }

        public int GetValueIndex(string ObsValue)
        {
            int Index = 0;
            foreach (var Value in this.Values)
            {
                if (Value.Id == ObsValue)
                {
                    Index = Value.Index;
                }
            }
            return Index;
        }
        
    }
}
