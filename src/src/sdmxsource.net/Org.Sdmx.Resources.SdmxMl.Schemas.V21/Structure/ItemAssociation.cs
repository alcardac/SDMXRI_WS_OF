// -----------------------------------------------------------------------
// <copyright file="ItemAssociation.cs" company="EUROSTAT">
//   Date Created : 2014-10-15
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

    public abstract partial class ItemAssociation
    {
        /// <summary>
        /// Gets the typed source.
        /// </summary>
        /// <typeparam name="T">The <see cref="ItemSchemeReferenceBaseType"/> based type.</typeparam>
        /// <returns>he <see cref="ItemSchemeReferenceBaseType"/> based instance.</returns>
        public T GetTypedSource<T>() where T : LocalItemReferenceType
        {
            return this.ContentField.GetTypedSource<T>();
        }

        /// <summary>
        /// Gets the typed source.
        /// </summary>
        /// <typeparam name="T">The <see cref="ItemSchemeReferenceBaseType"/> based type.</typeparam>
        /// <returns>he <see cref="ItemSchemeReferenceBaseType"/> based instance.</returns>
        public T GetTypedTarget<T>() where T : LocalItemReferenceType
        {
            return this.ContentField.GetTypedTarget<T>();
        }
    }
}