// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentConstraintBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attachment constraint object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///   The attachment constraint object core.
    /// </summary>
    [Serializable]
    public class AttachmentConstraintObjectCore :
        ConstraintObjectCore<IAttachmentConstraintObject, IMaintainableMutableObject>, 
        IAttachmentConstraintObject
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        public AttachmentConstraintObjectCore(IAttachmentConstraintMutableObject attachment) : base(attachment)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentConstraintObjectCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        public AttachmentConstraintObjectCore(AttachmentConstraintType type)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint), type.GetConstraintAttachmentType<AttachmentConstraintAttachmentType>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentConstraintObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The sdmxObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private AttachmentConstraintObjectCore(IAttachmentConstraintObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        /// ///
        /// <exception cref="SdmxNotImplementedException">Throws SdmxNotImplementedException.</exception>
        public override IMaintainableMutableObject MutableInstance
        {
            get
            {
                // FUNC 2.1 AttachmentConstraintObjectCore.getMutableInstance
                throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "AttachmentConstraint.getMutableInstance()");
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
                return this.DeepEqualsInternal((IAttachmentConstraintObject)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IAttachmentConstraintObject"/> . 
        /// </returns>
        public override IAttachmentConstraintObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new AttachmentConstraintObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion
    }
}