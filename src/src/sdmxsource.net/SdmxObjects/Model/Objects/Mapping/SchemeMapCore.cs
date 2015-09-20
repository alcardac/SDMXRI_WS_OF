// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemeMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The scheme map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The scheme map core.
    /// </summary>
    [Serializable]
    public abstract class SchemeMapCore : NameableCore, ISchemeMapObject
    {
        #region Fields

        /// <summary>
        ///   The source ref.
        /// </summary>
        private ICrossReference sourceRef;

        /// <summary>
        ///   The target ref.
        /// </summary>
        private ICrossReference targetRef;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemeMapCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        internal SchemeMapCore(ISchemeMapMutableObject itemMutableObject, IIdentifiableObject parent)
            : base(itemMutableObject, parent)
        {
            if (itemMutableObject.SourceRef != null)
            {
                this.sourceRef = new CrossReferenceImpl(this, itemMutableObject.SourceRef);
            }

            if (itemMutableObject.TargetRef != null)
            {
                this.targetRef = new CrossReferenceImpl(this, itemMutableObject.TargetRef);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemeMapCore"/> class.
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
        internal SchemeMapCore(NameableType createdFrom, SdmxStructureType structureType, IIdentifiableObject parent)
            : base(createdFrom, structureType, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemeMapCore"/> class.
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
        internal SchemeMapCore(
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

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets or sets the source ref.
        /// </summary>
        public ICrossReference SourceRef
        {
            get
            {
                return this.sourceRef;
            }

            set
            {
                this.sourceRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the target ref.
        /// </summary>
        public ICrossReference TargetRef
        {
            get
            {
                return this.targetRef;
            }

            set
            {
                this.targetRef = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (ISchemeMapObject)sdmxObject;
                if (ObjectUtil.Equivalent(this.sourceRef, that.SourceRef))
                {
                    return false;
                }

                if (ObjectUtil.Equivalent(this.targetRef, that.TargetRef))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion
    }
}