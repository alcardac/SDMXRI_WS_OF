// -----------------------------------------------------------------------
// <copyright file="SdmxDataFormatCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SdmxDataFormatCore : IDataFormat
    {
        private DataType _dataType;
        #region Implementation of IDataFormat

        public DataType SdmxDataFormat
        {
            get
            {
                return this._dataType;
            }
        }

        #endregion

        public SdmxDataFormatCore(DataType dataType)
        {
            if(dataType == null)
                throw new ArgumentException("Data Type can not be null for SdmxDataFormat");

            this._dataType = dataType;
        }

        public override string ToString()
        {
            return _dataType.ToString();
        }

        public virtual string FormatAsString
        {
            get
            {
                return ToString();
            }
        }
    }
}
