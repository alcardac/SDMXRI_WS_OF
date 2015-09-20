using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstatExtension.DataWriter.Model
{
    public class SDMXJsonValue
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _descr;

        public string Descr
        {
            get { return _descr; }
            set { _descr = value; }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public bool Exist (SDMXJsonValue Val)
        {
            if (this.Id == Val.Id && this.Descr == Val.Descr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
