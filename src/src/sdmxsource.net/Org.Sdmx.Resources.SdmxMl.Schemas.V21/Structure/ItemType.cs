// -----------------------------------------------------------------------
// <copyright file="ItemType.cs" company="Eurostat">
//   Date Created : 2014-04-22
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
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
    /// The item type.
    /// </summary>
    public partial class ItemType
    {
        /// <summary>
        ///     The Parent element name
        /// </summary>
        private static readonly XName _parentName = XName.Get(
            "Parent", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure");

        /// <summary>
        /// Gets the typed parent.
        /// </summary>
        /// <typeparam name="T">The type of the parent.</typeparam>
        /// <returns>The parent item of this item; otherwise null.</returns>
        public T GetTypedParent<T>() where T : Common.LocalItemReferenceType
        {
            XElement x = this.GetElement(_parentName);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }

        /// <summary>
        /// Sets the typed parent.
        /// </summary>
        /// <typeparam name="T">The type of the parent.</typeparam>
        /// <param name="value">The value.</param>
        public void SetTypedParent<T>(T value) where T : Common.LocalItemReferenceType
        {
            this.SetElement(_parentName, value);
        }
    }
}