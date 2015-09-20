// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexIdentifiableReferenceBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex identifiable reference core.
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
    ///   The complex identifiable reference.
    /// </summary>
    [Serializable]
    public class ComplexIdentifiableReferenceCore : ComplexNameableReferenceCore, IComplexIdentifiableReferenceObject
    {

        #region Fields

        /// <summary>
        ///   The id.
        /// </summary>
        private IComplexTextReference _id;

        /// <summary>
        ///   The child reference.
        /// </summary>
        private IComplexIdentifiableReferenceObject _childReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexIdentifiableReferenceCore"/> class.
        /// </summary>
        /// <param name="id">
        /// The id. 
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
        /// <param name="childReference">
        /// The child reference. 
        /// </param> 
        public ComplexIdentifiableReferenceCore(IComplexTextReference id, SdmxStructureType structureType,
			IComplexAnnotationReference annotationRef, IComplexTextReference nameRef, IComplexTextReference descriptionRef,
			IComplexIdentifiableReferenceObject childReference) 
	    	: base(structureType, annotationRef, nameRef, descriptionRef)
	    {	
	        	this._id = id;
	        	this._childReference = childReference;
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
        ///   Gets the child reference.
        /// </summary>
	    public virtual IComplexIdentifiableReferenceObject ChildReference
        {
            get
            {
                return _childReference;
            }
        }

        #endregion

    }
}
