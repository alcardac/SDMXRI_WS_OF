// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The subscription mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    ///   The subscription mutable core.
    /// </summary>
    [Serializable]
    public class SubscriptionMutableCore : MaintainableMutableCore<ISubscriptionObject>, ISubscriptionMutableObject
    {
        #region Fields

        /// <summary>
        ///   The http post to.
        /// </summary>
        private IList<string> httpPostTo;

        /// <summary>
        ///   The mail to.
        /// </summary>
        private IList<string> mailTo;

        /// <summary>
        ///   The owner.
        /// </summary>
        private IStructureReference owner;

        /// <summary>
        ///   The references.
        /// </summary>
        private IList<IStructureReference> references;

        /// <summary>
        ///   The subscription type.
        /// </summary>
        private SubscriptionEnumType subscriptionType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="SubscriptionMutableCore" /> class.
        /// </summary>
        public SubscriptionMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Subscription))
        {
            this.mailTo = new List<string>();
            this.httpPostTo = new List<string>();
            this.references = new List<IStructureReference>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionMutableCore"/> class.
        /// </summary>
        /// <param name="subscription">
        /// The isubscription. 
        /// </param>
        public SubscriptionMutableCore(ISubscriptionObject subscription)
            : base(subscription)
        {
            this.mailTo = new List<string>();
            this.httpPostTo = new List<string>();
            this.references = new List<IStructureReference>();
            if (subscription.HTTPPostTo != null)
            {
                this.httpPostTo = new List<string>(subscription.HTTPPostTo);
            }

            if (subscription.MailTo != null)
            {
                this.mailTo = new List<string>(subscription.MailTo);
            }

            if (subscription.References != null)
            {
                foreach (IStructureReference structureReference in subscription.References)
                {
                    this.references.Add(structureReference.CreateCopy());
                }
            }

            base.StructureType = subscription.StructureType;
            this.owner = subscription.Owner.CreateMutableInstance();
            this.subscriptionType = subscription.SubscriptionType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the http post to.
        /// </summary>
        public virtual IList<string> HttpPostTo
        {
            get
            {
                return this.httpPostTo;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override ISubscriptionObject ImmutableInstance
        {
            get
            {
                return new SubscriptionObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets the mail to.
        /// </summary>
        public virtual IList<string> MailTo
        {
            get
            {
                return this.mailTo;
            }
        }

        /// <summary>
        ///   Gets or sets the owner.
        /// </summary>
        public virtual IStructureReference Owner
        {
            get
            {
                return this.owner;
            }

            set
            {
                this.owner = value;
            }
        }

        /// <summary>
        ///   Gets the references.
        /// </summary>
        public virtual IList<IStructureReference> References
        {
            get
            {
                return new ReadOnlyCollection<IStructureReference>(this.references);
            }
        }

        /// <summary>
        ///   Gets or sets the subscription type.
        /// </summary>
        public virtual SubscriptionEnumType SubscriptionType
        {
            get
            {
                return this.subscriptionType;
            }

            set
            {
                this.subscriptionType = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add reference.
        /// </summary>
        /// <param name="reference">
        /// The reference. 
        /// </param>
        public virtual void AddReference(IStructureReference reference)
        {
            if (this.references == null)
            {
                this.references = new List<IStructureReference>();
            }

            this.references.Add(reference);
        }

        #endregion
    }
}