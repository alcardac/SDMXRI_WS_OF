// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationInformationImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration information impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model
{
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///   The registration information impl.
    /// </summary>
    public class RegistrationInformationImpl : IRegistrationInformation
    {
        #region Fields

        /// <summary>
        ///   The _action.
        /// </summary>
        private readonly DatasetAction _action;

        /// <summary>
        ///   The _registration.
        /// </summary>
        private readonly IRegistrationObject _registration;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationInformationImpl"/> class.
        /// </summary>
        /// <param name="action">
        /// The action. 
        /// </param>
        /// <param name="registration">
        /// The registration codelistRef. 
        /// </param>
        public RegistrationInformationImpl(DatasetAction action, IRegistrationObject registration)
        {
            this._action = action;
            this._registration = registration;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the registration.
        /// </summary>
        public virtual IRegistrationObject Registration
        {
            get
            {
                return this._registration;
            }
        }

        /// <summary>
        ///   Gets the registration action.
        /// </summary>
        public virtual DatasetAction RegistrationAction
        {
            get
            {
                return this._action;
            }
        }

        #endregion
    }
}