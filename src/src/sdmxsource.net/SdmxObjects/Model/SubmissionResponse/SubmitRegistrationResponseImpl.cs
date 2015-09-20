// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationResponseImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit registration response impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.SubmissionResponse
{
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;

    /// <summary>
    ///   The submit registration response impl.
    /// </summary>
    public class SubmitRegistrationResponseImpl : ISubmitRegistrationResponse
    {
        #region Fields

        /// <summary>
        ///   The _errors.
        /// </summary>
        private readonly IErrorList _errors;

        /// <summary>
        ///   The _registration.
        /// </summary>
        private readonly IRegistrationObject _registration;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitRegistrationResponseImpl"/> class.
        /// </summary>
        /// <param name="registration">
        /// The registration. 
        /// </param>
        /// <param name="errors">
        /// The errors. 
        /// </param>
        public SubmitRegistrationResponseImpl(IRegistrationObject registration, IErrorList errors)
        {
            this._registration = registration;
            this._errors = errors;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether it is an error.
        /// </summary>
        public bool IsError
        {
            get
            {
                return this._errors != null;
            }
        }

        /// <summary>
        ///   Gets the error list.
        /// </summary>
        public IErrorList ErrorList
        {
            get
            {
                return this._errors;
            }
        }

        /// <summary>
        ///   Gets the registration.
        /// </summary>
        public IRegistrationObject Registration
        {
            get
            {
                return this._registration;
            }
        }

        #endregion
    }
}