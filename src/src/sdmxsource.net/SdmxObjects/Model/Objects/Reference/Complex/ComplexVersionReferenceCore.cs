// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexVersionReferenceImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex version reference core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex
{
    #region Using directives

    using System;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion

    /// <summary>
    ///   The complex annotation reference.
    /// </summary>
    [Serializable]
    public class ComplexVersionReferenceCore : IComplexVersionReference
    {

        #region Fields

        /// <summary>
        ///   The return latest.
        /// </summary>
        private TertiaryBool _returnLatest = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);

        /// <summary>
        ///   The version.
        /// </summary>
	    private string _version;

        /// <summary>
        ///   The valid from.
        /// </summary>
	    private ITimeRange _validFrom;

        /// <summary>
        ///   The valid to.
        /// </summary>
	    private ITimeRange _validTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexAnnotationReferenceCore"/> class.
        /// </summary>
        /// <param name="returnLatest">
        /// The return latest. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="validFrom">
        /// The valid from. 
        /// </param>
        /// <param name="validTo">
        /// The valid to. 
        /// </param>
        public ComplexVersionReferenceCore(TertiaryBool returnLatest, string version, ITimeRange validFrom, ITimeRange validTo) 
        {
		    if (returnLatest != null)
            {
			   this._returnLatest = returnLatest;
		    }
	    	this._version = version;
	    	this._validFrom = validFrom;
		    this._validTo = validTo;
	    }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the is return latest.
        /// </summary>
        public virtual TertiaryBool IsReturnLatest 
        {
	        get
	        {
	            return _returnLatest;
	        }
        }

        /// <summary>
        ///   Gets the version.
        /// </summary>
	    public virtual string Version
        {
	   	    get
	   	    {
	   	        return _version;
	   	    }
        }

        /// <summary>
        ///   Gets the version valid from.
        /// </summary>
        public virtual ITimeRange VersionValidFrom
        {
	        get
            {
                return _validFrom;
            }
        }

        /// <summary>
        ///   Gets the version valid to.
        /// </summary>
	    public virtual ITimeRange VersionValidTo
        {
            get
            {
                return _validTo;
            }
        }

        #endregion
    }
}
