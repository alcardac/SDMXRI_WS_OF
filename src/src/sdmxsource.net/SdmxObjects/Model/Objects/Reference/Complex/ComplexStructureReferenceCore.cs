// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexStructureReferenceBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex structure reference core.
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
    ///   The complex structure reference.
    /// </summary>
    [Serializable]
    public class ComplexStructureReferenceCore : ComplexNameableReferenceCore, IComplexStructureReferenceObject
    {
        #region Fields

        /// <summary>
        ///   The id.
        /// </summary>
        private IComplexTextReference _id;

        /// <summary>
        ///   The agency id.
        /// </summary>
	    private IComplexTextReference _agencyId;

        /// <summary>
        ///   The version ref.
        /// </summary>
	    private IComplexVersionReference _versionRef;

        /// <summary>
        ///   The child ref.
        /// </summary>
	    private IComplexIdentifiableReferenceObject _childRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexStructureReferenceCore"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <param name="versionRef">
        /// The version ref. 
        /// </param>
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
        /// <param name="childRef">
        /// The child ref. 
        /// </param>
        public ComplexStructureReferenceCore(IComplexTextReference agencyId, IComplexTextReference id,
            IComplexVersionReference versionRef, SdmxStructureType structureType, IComplexAnnotationReference annotationRef,
			IComplexTextReference nameRef, IComplexTextReference descriptionRef, IComplexIdentifiableReferenceObject childRef) 
		: base(structureType, annotationRef, nameRef, descriptionRef)
		{
		this._id = id;
		this._agencyId = agencyId;
		this._versionRef = versionRef;
		//TODO childRef defaults && null check...
		this._childRef = childRef;
	    }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public virtual IComplexTextReference Id
        {
		     get
		     {
		       return _id;
		     }
        }

        /// <summary>
        ///   Gets the agency id.
        /// </summary>
        public virtual IComplexTextReference AgencyId
        {
		   get
		   {
		      return _agencyId;
		   }
        }

       /// <summary>
       ///   Gets the version reference.
       /// </summary>
	   public virtual IComplexVersionReference VersionReference
       {
		   get
		   {
		      return _versionRef;
		   }
       }

       /// <summary>
       ///   Gets the child reference.
       /// </summary>
	   public virtual IComplexIdentifiableReferenceObject ChildReference 
       {
		  get
		  {
		      return _childRef;
		  }
       }

      #endregion
    }
}
