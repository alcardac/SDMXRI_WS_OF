// -----------------------------------------------------------------------
// <copyright file="StructureUsageType.cs" company="Eurostat">
//   Date Created : 2013-01-24
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    /// The structure usage type.
    /// </summary>
    public partial class StructureUsageType
    {
        /// <summary>
        /// Gets the Structure
        /// <para>
        /// Structure references the structure (data structure or metadata structure definition) which the structure usage is based upon. Implementations will have to refine the type to use a concrete structure reference (i.e. either a data structure or metadata structure definition reference).
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the return value
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T GetStructure<T>() where T : Common.StructureReferenceBaseType
        {
            XElement x = this.GetElement(XName.Get("Structure", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure"));
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }

        /// <summary>
        /// Sets the Structure
        /// <para>
        /// Structure references the structure (data structure or metadata structure definition) which the structure usage is based upon. Implementations will have to refine the type to use a concrete structure reference (i.e. either a data structure or metadata structure definition reference).
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="value"/>
        /// </typeparam>
        /// <param name="value">
        /// The value.
        /// </param>
        public void SetStructure<T>(T value) where T : Common.StructureReferenceBaseType
        {
            this.SetElement(XName.Get("Structure", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure"), value);
        }
    }
}