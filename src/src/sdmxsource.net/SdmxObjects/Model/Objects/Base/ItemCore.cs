// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The item core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The item core.
    /// </summary>
    [Serializable]
    public abstract class ItemCore : NameableCore
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The itemMutableObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemCore(IItemMutableObject itemMutableObject, IIdentifiableObject parent)
            : base(itemMutableObject, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public ItemCore(SdmxStructureType structure, SdmxReader reader, IIdentifiableObject parent) {
        // super(structure, reader, parent);
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemCore(ItemType createdFrom, SdmxStructureType structureType, IIdentifiableObject parent)
            : base(createdFrom, structureType, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="description">
        /// The description. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string id, 
            Uri uri, 
            IList<TextType> name, 
            IList<TextType> description, 
            AnnotationsType annotationsType, 
            IIdentifiableObject parent)
            : base(createdFrom, structureType, id, uri, name, description, annotationsType, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        /// <param name="description">
        /// The description. 
        /// </param>
        /// <param name="annotationsType">
        /// The annotations type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ItemCore(
            IXmlSerializable createdFrom, 
            SdmxStructureType structureType, 
            string id, 
            Uri uri, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> name, 
            IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType> description, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.AnnotationsType annotationsType, 
            IIdentifiableObject parent)
            : base(createdFrom, structureType, id, uri, name, description, annotationsType, parent)
        {
        }

        #endregion
    }
}