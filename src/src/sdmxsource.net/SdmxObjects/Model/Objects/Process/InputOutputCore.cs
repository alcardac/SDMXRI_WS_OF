// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputOutputBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The input output core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The input output core.
    /// </summary>
    [Serializable]
    public class InputOutputCore : AnnotableCore, IInputOutputObject
    {
        #region Fields

        /// <summary>
        ///   The local id.
        /// </summary>
        private readonly string localId;

        /// <summary>
        ///   The structure reference.
        /// </summary>
        private readonly ICrossReference structureReference;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InputOutputCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="mutableObject">
        /// The mutable object. 
        /// </param>
        public InputOutputCore(IIdentifiableObject parent, IInputOutputMutableObject mutableObject)
            : base(mutableObject, parent)
        {
            this.localId = mutableObject.LocalId;
            if (mutableObject.StructureReference != null)
            {
                this.structureReference = new CrossReferenceImpl(this, mutableObject.StructureReference);
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="InputOutputCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="xmlType">
        /// The xml type. 
        /// </param>
        public InputOutputCore(IIdentifiableObject parent, InputOutputType xmlType)
            : base(xmlType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Computation), parent)
        {
            this.localId = xmlType.localID;
            if (xmlType.ObjectReference != null)
            {
                this.structureReference = RefUtil.CreateReference(this, xmlType.ObjectReference);
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the local id.
        /// </summary>
        public virtual string LocalId
        {
            get
            {
                return this.localId;
            }
        }

        /// <summary>
        ///   Gets the structure reference.
        /// </summary>
        public virtual ICrossReference StructureReference
        {
            get
            {
                return this.structureReference;
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
                var that = (IInputOutputObject)sdmxObject;
                if (!this.Equivalent(this.structureReference, that.StructureReference))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.localId, that.LocalId))
                {
                    return false;
                }

                return this.DeepEqualsInternalAnnotable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALiDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this.structureReference == null)
            {
                throw new SdmxSemmanticException("Input / Output sdmxObject, requires an Object Reference");
            }
        }

        #endregion
    }
}