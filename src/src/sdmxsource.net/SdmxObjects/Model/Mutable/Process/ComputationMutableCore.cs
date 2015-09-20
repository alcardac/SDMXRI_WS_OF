// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComputationMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The computation mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Process
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The computation mutable core.
    /// </summary>
    [Serializable]
    public class ComputationMutableCore : AnnotableMutableCore, IComputationMutableObject
    {
        #region Fields

        /// <summary>
        ///   The description.
        /// </summary>
        private readonly IList<ITextTypeWrapperMutableObject> _descriptions;

        /// <summary>
        ///   The local id.
        /// </summary>
        private string _localId;

        /// <summary>
        ///   The software language.
        /// </summary>
        private string _softwareLanguage;

        /// <summary>
        ///   The software package.
        /// </summary>
        private string _softwarePackage;

        /// <summary>
        ///   The software version.
        /// </summary>
        private string _softwareVersion;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ComputationMutableCore" /> class.
        /// </summary>
        public ComputationMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Computation))
        {
            this._descriptions = new List<ITextTypeWrapperMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputationMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ComputationMutableCore(IComputationObject objTarget)
            : base(objTarget)
        {
            this._descriptions = new List<ITextTypeWrapperMutableObject>();
            this._localId = objTarget.LocalId;
            this._softwareLanguage = objTarget.SoftwareLanguage;
            this._softwarePackage = objTarget.SoftwarePackage;
            this._softwareVersion = objTarget.SoftwareVersion;
            if (objTarget.Description != null)
            {
                this._descriptions = new List<ITextTypeWrapperMutableObject>();

                foreach (ITextTypeWrapper currentTextType in objTarget.Description)
                {
                    this._descriptions.Add(new TextTypeWrapperMutableCore(currentTextType));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public virtual IList<ITextTypeWrapperMutableObject> Descriptions
        {
            get
            {
                return this._descriptions;
            }
        }

        /// <summary>
        ///   Gets or sets the local id.
        /// </summary>
        public virtual string LocalId
        {
            get
            {
                return this._localId;
            }

            set
            {
                this._localId = value;
            }
        }

        /// <summary>
        ///   Gets or sets the software language.
        /// </summary>
        public virtual string SoftwareLanguage
        {
            get
            {
                return this._softwareLanguage;
            }

            set
            {
                this._softwareLanguage = value;
            }
        }

        /// <summary>
        ///   Gets or sets the software package.
        /// </summary>
        public virtual string SoftwarePackage
        {
            get
            {
                return this._softwarePackage;
            }

            set
            {
                this._softwarePackage = value;
            }
        }

        /// <summary>
        ///   Gets or sets the software version.
        /// </summary>
        public virtual string SoftwareVersion
        {
            get
            {
                return this._softwareVersion;
            }

            set
            {
                this._softwareVersion = value;
            }
        }

        #endregion
    }
}