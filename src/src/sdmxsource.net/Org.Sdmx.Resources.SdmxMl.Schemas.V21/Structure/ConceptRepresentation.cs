// -----------------------------------------------------------------------
// <copyright file="ConceptRepresentation.cs" company="Eurostat">
//   Date Created : 2013-01-15
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Xml.Linq;

    /// <summary>
    /// <para>
    /// ConceptRepresentation defines the core representation that are allowed for a concept. The text format allowed for a concept is that which is allowed for any non-target object component.
    /// </para>
    /// </summary>
    public partial class ConceptRepresentation
    {

        /// <summary>
        /// <para>
        /// Enumeration references an item scheme that enumerates the allowable values for this representation.
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Setter: Appends
        /// </para>
        /// <para>
        /// Regular expression: (TextFormat | (Enumeration, EnumerationFormat?))
        /// </para>
        /// </summary>
        public new Common.CodelistReferenceType Enumeration
        {
            get
            {
                XElement x = this.GetElement(XName.Get("Enumeration", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure"));
                return (Common.CodelistReferenceType)x;
            }
            set
            {
                this.SetElement(XName.Get("Enumeration", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure"), value);
            }
        }
        
    }
}