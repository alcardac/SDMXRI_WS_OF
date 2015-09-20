// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexNameableReferenceImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex nameable reference core.
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
    ///   The complex nameable reference.
    /// </summary>
    [Serializable] 
    public class ComplexNameableReferenceCore : IComplexNameableReference
    {
     
       #region Fields
        
        /// <summary>
       ///   The structure type ref.
       /// </summary>
       private SdmxStructureType _structureType;
   
       ///  /// <summary>
       ///   The annotation ref.
       /// </summary>
	   private IComplexAnnotationReference _annotationRef;

       /// <summary>
       ///   The name ref.
       /// </summary>
       private IComplexTextReference _nameRef;

       /// <summary>
       ///   The description ref.
       /// </summary>
	   private IComplexTextReference _descriptionRef;

        #endregion

       #region Constructors and Destructors

       /// <summary>
       /// Initializes a new instance of the <see cref="ComplexNameableReferenceCore"/> class.
       /// </summary>
       /// <param name="structureType">
       /// The structure type. 
       /// </param>
       /// <param name="annotationRef">
       /// The annotation ref. 
       /// </param>
       /// <param name="nameRef">
       /// The name ref. 
       /// </param>
       /// <param name="descriptionRef">
       /// The description ref. 
       /// </param>
        public ComplexNameableReferenceCore(SdmxStructureType structureType, IComplexAnnotationReference annotationRef,
			IComplexTextReference nameRef, IComplexTextReference descriptionRef)
        {
		
	  	   if ( structureType == null )
           {
		    	throw new SdmxSemmanticException("Null structure type provided for reference in query.");
		   }
		
		   this._structureType = structureType;	
		   if (annotationRef != null) 
           {
		    	this._annotationRef = annotationRef;
		   }
	  	   if (nameRef != null) 
           {
		    	this._nameRef = nameRef;	
		   }
		   if (descriptionRef != null) 
           {
			    this._descriptionRef = descriptionRef;
		   }
	  }

       #endregion

       #region Public Properties

       /// <summary>
       ///   Gets the referenced structure type.
       /// </summary>
       public virtual SdmxStructureType ReferencedStructureType
       {
          get
          {
              return _structureType;
          }
       }

      /// <summary>
      ///   Gets the annotation reference.
      /// </summary>
	  public virtual IComplexAnnotationReference AnnotationReference
      {
		  get
	      {
		      return _annotationRef;
		  }
      }

     /// <summary>
     ///   Gets the name reference.
     /// </summary>
      public virtual IComplexTextReference NameReference
      {
	    	get
		    {
		       return _nameRef;
		    }
       }

      /// <summary>
      ///   Gets the description reference.
      /// </summary>
	   public virtual IComplexTextReference DescriptionReference
       {
          get
          {
            return _descriptionRef;
          }
       }

       #endregion

    }
}
