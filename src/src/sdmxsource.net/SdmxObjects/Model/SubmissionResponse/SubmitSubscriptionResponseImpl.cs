// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitSubscriptionResponseImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit subscription response impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.SubmissionResponse
{
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;

    /// <summary>
    ///   The submit subscription response impl.
    /// </summary>
    public class SubmitSubscriptionResponseImpl : SubmitStructureResponseImpl, ISubmitSubscriptionResponse
    {
        #region Fields

        /// <summary>
        ///   The _subscriber id.
        /// </summary>
        private readonly string _subscriberId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitSubscriptionResponseImpl"/> class.
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference. 
        /// </param>
        /// <param name="errorList">
        /// The error list. 
        /// </param>
        /// <param name="subscriberId">
        /// The subscriber id. 
        /// </param>
        public SubmitSubscriptionResponseImpl(
            IStructureReference structureReference, IErrorList errorList, string subscriberId)
            : base(structureReference, errorList)
        {
            this._subscriberId = subscriberId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the subscriber assigned id.
        /// </summary>
        public virtual string SubscriberAssignedId
        {
            get
            {
                return this._subscriberId;
            }
        }

        #endregion
    }
}