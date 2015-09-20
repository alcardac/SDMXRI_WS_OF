// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureResponseImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit structure response impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.SubmissionResponse
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;

    /// <summary>
    ///   The submit structure response impl.
    /// </summary>
    public class SubmitStructureResponseImpl : ISubmitStructureResponse
    {
        #region Fields

        /// <summary>
        ///   The _error list.
        /// </summary>
        private readonly IErrorList _errorList;

        /// <summary>
        ///   The _structure reference.
        /// </summary>
        private readonly IStructureReference _structureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitStructureResponseImpl"/> class.
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference dataStructureObject. 
        /// </param>
        /// <param name="errorList">
        /// The error list. 
        /// </param>
        /// ///
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException.
        /// </exception>
        public SubmitStructureResponseImpl(IStructureReference structureReference, IErrorList errorList)
        {
            this._structureReference = structureReference;
            this._errorList = errorList;
            if (structureReference != null && structureReference.TargetUrn == null)
            {
                throw new ArgumentException("SubmitStructureResponseImpl expects a complete IStructureReference");
            }

            if (!this.IsError)
            {
                if (structureReference == null)
                {
                    throw new ArgumentException("Sucessful SubmitStructureResponse expects a IStructureReference");
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether error.
        /// </summary>
        public bool IsError
        {
            get
            {
                return this._errorList != null && !this._errorList.Warning;
            }
        }

        /// <summary>
        ///   Gets the error list.
        /// </summary>
        public virtual IErrorList ErrorList
        {
            get
            {
                return this._errorList;
            }
        }

        /// <summary>
        ///   Gets the structure reference.
        /// </summary>
        public virtual IStructureReference StructureReference
        {
            get
            {
                return this._structureReference;
            }
        }

        #endregion
    }
}