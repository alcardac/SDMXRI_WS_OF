// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProvisionAgreementMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The provision agreement mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    ///   The provision agreement mutable core.
    /// </summary>
    [Serializable]
    public class ProvisionAgreementMutableCore : MaintainableMutableCore<IProvisionAgreementObject>, 
                                                 IProvisionAgreementMutableObject
    {
        #region Fields

        /// <summary>
        ///   The dataprovider ref.
        /// </summary>
        private IStructureReference dataproviderRef;

        /// <summary>
        ///   The structure useage.
        /// </summary>
        private IStructureReference structureUseage;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ProvisionAgreementMutableCore" /> class.
        /// </summary>
        public ProvisionAgreementMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisionAgreementMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ProvisionAgreementMutableCore(IProvisionAgreementObject objTarget)
            : base(objTarget)
        {
            if (objTarget.StructureUseage != null)
            {
                this.structureUseage = objTarget.StructureUseage.CreateMutableInstance();
            }

            if (objTarget.DataproviderRef != null)
            {
                this.dataproviderRef = objTarget.DataproviderRef.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the dataprovider ref.
        /// </summary>
        public virtual IStructureReference DataproviderRef
        {
            get
            {
                return this.dataproviderRef;
            }

            set
            {
                this.dataproviderRef = value;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IProvisionAgreementObject ImmutableInstance
        {
            get
            {
                return new ProvisionAgreementObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets or sets the structure usage.
        /// </summary>
        public virtual IStructureReference StructureUsage
        {
            get
            {
                return this.structureUseage;
            }

            set
            {
                this.structureUseage = value;
            }
        }

        #endregion
    }
}