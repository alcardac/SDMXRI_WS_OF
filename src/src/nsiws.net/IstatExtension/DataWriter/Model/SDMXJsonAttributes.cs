using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstatExtension.DataWriter.Model
{
    public class SDMXJsonAttributes
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _attachmentLevel;

        public string AttachmentLevel
        {
            get { return _attachmentLevel; }
            set { _attachmentLevel = value; }
        }
        

        private List<SDMXJsonValue> _values;

        public List<SDMXJsonValue> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public SDMXJsonAttributes()
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

        public int GetValueIndex(string AttValue)
        {
            int Index = 0;
            foreach (var Value in this.Values)
            {
                if (Value.Id == AttValue)
                {
                    Index = Value.Index;
                }
            }
            return Index;
        }
    }
}
