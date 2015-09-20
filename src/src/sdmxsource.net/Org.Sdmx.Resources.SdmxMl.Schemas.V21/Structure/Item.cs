// -----------------------------------------------------------------------
// <copyright file="Item.cs" company="Eurostat">
//   Date Created : 2014-04-22
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    public partial class Item
    {
        /// <summary>
        /// Gets the typed parent.
        /// </summary>
        /// <typeparam name="T">The type of the parent.</typeparam>
        /// <returns>The parent item of this item; otherwise null.</returns>
        public T GetTypedParent<T>() where T : Common.LocalItemReferenceType
        {
            return this.ContentField.GetTypedParent<T>();
        }

        /// <summary>
        /// Sets the typed parent.
        /// </summary>
        /// <typeparam name="T">The type of the parent.</typeparam>
        /// <param name="value">The value.</param>
        public void SetTypedParent<T>(T value) where T : Common.LocalItemReferenceType
        {
            this.ContentField.SetTypedParent(value);
        } 
    }
}