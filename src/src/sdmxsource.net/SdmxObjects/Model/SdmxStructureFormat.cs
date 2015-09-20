// -----------------------------------------------------------------------
// <copyright file="SdmxStructureFormat.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class SdmxStructureFormat : IStructureFormat
    {
        private readonly StructureOutputFormat _structureFormat;

        public StructureOutputFormat SdmxOutputFormat
        {
            get
            {
                return this._structureFormat;
            }
        }

        public SdmxStructureFormat(StructureOutputFormat structureFormat)
        {
            if (structureFormat == null)
            {
                throw new ArgumentException("STRUCTURE_OUTPUT_FORMAT can not be null");
            }
            this._structureFormat = structureFormat;
        }

        public override string ToString()
        {
            return this._structureFormat.EnumType.ToString();
        }

        public string FormatAsString 
        {
            get
            {
                return ToString();
            }
        }
    }
}
