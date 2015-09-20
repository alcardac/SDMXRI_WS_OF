// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureType.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    // JAVADOC missing

    /// <summary>
    ///     The structure type.
    /// </summary>
    public enum StructureType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The structure.
        /// </summary>
        Structure, 

        /// <summary>
        ///     The metadata report.
        /// </summary>
        MetadataReport, 

        /// <summary>
        ///     The registry interface.
        /// </summary>
        RegistryInterface, 

        /// <summary>
        ///     The query message.
        /// </summary>
        QueryMessage
    }

    /*
     * The only method is the ToString() and it matches the output of the Enum. So no reason to do this.
    public class StructureType : BaseConstantType<StructureType>
    {
        public static readonly StructureType Structure = new StructureType("Structure");
        public static readonly StructureType MetadataReport = new StructureType("MetadataReport");
        public static readonly StructureType RegistryInterface = new StructureType("RegistryInterface");
        public static readonly StructureType QueryMessage = new StructureType("QueryMessage");

        public static IEnumerable<StructureType> Values
        {
            get
            {
                yield return Structure;
                yield return MetadataReport;
                yield return RegistryInterface;
                yield return QueryMessage;
            }
        }

        private string _nodeName;

        private StructureType(string nodeName)
        {
            this._nodeName = nodeName;
        }

        public override string ToString()
        {
            return this._nodeName;
        }
    }
     */
}