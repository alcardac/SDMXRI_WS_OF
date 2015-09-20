// -----------------------------------------------------------------------
// <copyright file="ItemSchemeMapType.cs" company="EUROSTAT">
//   Date Created : 2014-10-14
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    /// <para>
    /// ItemSchemeMapType is an abstract base type which forms the basis for mapping items between item schemes of the same type.
    /// </para>
    /// <para>
    /// Regular expression: (Annotations?, Name+, Description*, Source, Target, (ItemAssociation)+)
    /// </para>
    /// </summary>
    public abstract partial class ItemSchemeMapType
    {
        /// <summary>
        /// The _source
        /// </summary>
        private static readonly XName _source = XName.Get("Source", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure");

        /// <summary>
        /// The _target
        /// </summary>
        private static readonly XName _target = XName.Get("Target", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure");

        /// <summary>
        /// Gets the typed source.
        /// </summary>
        /// <typeparam name="T">The <see cref="ItemSchemeReferenceBaseType"/> based type.</typeparam>
        /// <returns>he <see cref="ItemSchemeReferenceBaseType"/> based instance.</returns>
        public T GetTypedSource<T>() where T : ItemSchemeReferenceBaseType
        {
            XElement x = this.GetElement(_source);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }

        /// <summary>
        /// Gets the typed source.
        /// </summary>
        /// <typeparam name="T">The <see cref="ItemSchemeReferenceBaseType"/> based type.</typeparam>
        /// <returns>he <see cref="ItemSchemeReferenceBaseType"/> based instance.</returns>
        public T GetTypedTarget<T>() where T : ItemSchemeReferenceBaseType
        {
            XElement x = this.GetElement(_target);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }
    }
}