// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexTextReferenceImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex text reference core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex
{
    #region Using directives

    using System;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion

    /// <summary>
    ///   The complex annotation reference.
    /// </summary>
    [Serializable]
    public class ComplexTextReferenceCore : IComplexTextReference
    {
        #region Fields

        /// <summary>
        ///   The lang.
        /// </summary>
        private string _lang;

        /// <summary>
        ///   The operator.
        /// </summary>
	    private TextSearch _operator;

        /// <summary>
        ///   The search param.
        /// </summary>
	    private string _searchParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexTextReferenceCore"/> class.
        /// </summary>
        /// <param name="lang">
        /// The lang. 
        /// </param>
        /// <param name="_operator">
        /// The operator. 
        /// </param>
        /// <param name="searchParam">
        /// The search param. 
        /// </param>
        public ComplexTextReferenceCore(string lang, TextSearch _operator, string searchParam)
        {
	    	this._lang = lang;
		
		    if (_operator == null) 
            {
			   this._operator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
		    } 
            else 
            {
			   this._operator = _operator;	
		    }
		
		    if (string.IsNullOrEmpty(searchParam)) 
           {
			  throw new SdmxSemmanticException("Not provided text to search for. It should not be null or empty.");
		   }
		
		   this._searchParam = searchParam;
	   }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the language.
        /// </summary>
        public virtual string Language
        {
		  get
		  {
		    return _lang;
		  }
        }

       /// <summary>
       ///   Gets the operator.
       /// </summary>
  	   public virtual TextSearch Operator
       {
		  get
          {
           return _operator;
          }
	   }

       /// <summary>
       ///   Gets the search parameter.
       /// </summary>
	   public virtual string SearchParameter
       {
		 get
		 {
		    return _searchParam;
		 }
       }

        #endregion

    }
}
