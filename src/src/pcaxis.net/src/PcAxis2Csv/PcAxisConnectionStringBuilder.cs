using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;
using org.estat.PcAxis.helpers;
using org.estat.PcAxis;
using System.Threading;
using System.Data;
using System.Collections;
using System.ComponentModel;

namespace org.estat.PcAxis.PcAxisProvider {
    public sealed class PcAxisConnectionStringBuilder : DbConnectionStringBuilder {
        // Fields
        private Hashtable _properties;

        // Methods
        public PcAxisConnectionStringBuilder() {
            this.Initialize(null);
        }

        public PcAxisConnectionStringBuilder(string connectionString) {
            this.Initialize(connectionString);
        }

        private void FallbackGetProperties(Hashtable propertyList) {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this, true)) {
                if ((descriptor.Name != "ConnectionString") && !propertyList.ContainsKey(descriptor.DisplayName)) {
                    propertyList.Add(descriptor.DisplayName, descriptor);
                }
            }
        }

        private void Initialize(string cnnString) {
            this._properties = new Hashtable(StringComparer.OrdinalIgnoreCase);
            try {
                base.GetProperties(this._properties);
            } catch (NotImplementedException) {
                this.FallbackGetProperties(this._properties);
            }
            if (!string.IsNullOrEmpty(cnnString)) {
                base.ConnectionString = cnnString;
            }
        }

        public override bool TryGetValue(string keyword, out object value) {
            bool flag = base.TryGetValue(keyword, out value);
            if (this._properties.ContainsKey(keyword)) {
                PropertyDescriptor descriptor = this._properties[keyword] as PropertyDescriptor;
                if (descriptor == null) {
                    return flag;
                }
                if (flag) {
                    if (descriptor.PropertyType == typeof(bool)) {
                        value = SQLiteConvert.ToBoolean(value);
                        return flag;
                    }
                    value = TypeDescriptor.GetConverter(descriptor.PropertyType).ConvertFrom(value);
                    return flag;
                }
                DefaultValueAttribute attribute = descriptor.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                if (attribute != null) {
                    value = attribute.Value;
                    flag = true;
                }
            }
            return flag;
        }

        // Properties
        [DefaultValue(""), DisplayName("defaultdir"), Browsable(true)]
        public string DefaultDir {
            get {
                object obj2;
                this.TryGetValue("defaultdir", out obj2);
                return obj2.ToString();
            }
            set {
                this["defaultdir"] = value;
            }
        }
    }
}
