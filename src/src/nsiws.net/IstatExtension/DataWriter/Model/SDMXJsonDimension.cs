using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstatExtension.DataWriter.Model
{
    public class SDMXJsonDimension
    {
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _keyPosition;
        public string KeyPosition
        {
            get { return _keyPosition; }
            set { _keyPosition = value; }
        }

        private string _role;

        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }        

        private List<SDMXJsonValue> _values;
        public List<SDMXJsonValue> Values
        {
            get { return _values; }
            set { _values = value; }
        }


        public SDMXJsonDimension()
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


        public int GetValueIndex(string DimValue)
        {
            int Index = 0;
            foreach (var Value in this.Values)
            {
                if (Value.Id == DimValue)
                {
                    Index = Value.Index;
                }
            }
            return Index;
        }

    }
}
