// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexStructureQueryMetadataImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex structure query metadata core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion

    /// <summary>
    ///   The complex structure query metadata.
    /// </summary>
    [Serializable]
    public class ComplexStructureQueryMetadataCore : IComplexStructureQueryMetadata
    {

        #region Fields

        /// <summary>
        ///   The reference detail.
        /// </summary>
        private readonly StructureReferenceDetail _referenceDetail;

        /// <summary>
        ///   The query detail.
        /// </summary>
	    private readonly ComplexStructureQueryDetail _queryDetail = ComplexStructureQueryDetail.GetFromEnum(ComplexStructureQueryDetailEnumType.Full);

        /// <summary>
        ///   The return matched artefact.
        /// </summary>
        private readonly bool _returnMatchedArtefact;

        /// <summary>
        ///   The reference specific structures.
        /// </summary>
	    private readonly IList<SdmxStructureType> _referenceSpecificStructures;

        /// <summary>
        ///   The references query detail.
        /// </summary>
        private readonly ComplexMaintainableQueryDetail _referencesQueryDetail = ComplexMaintainableQueryDetail.GetFromEnum(ComplexMaintainableQueryDetailEnumType.Full);

        /// <summary>
        /// The _process constraints
        /// </summary>
        private readonly bool _processConstraints;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexAnnotationReferenceCore"/> class.
        /// </summary>
        /// <param name="returnMatchedArtefact">
        /// The return matched artefatc. 
        /// </param>
        /// <param name="queryDetail">
        /// The query detail. 
        /// </param>
        /// <param name="referencesQueryDetail">
        /// The references query detail. 
        /// </param>
        /// <param name="referenceDetail">
        /// The reference detail. 
        /// </param>
        /// <param name="referenceSpecificStructures">
        /// The reference specific structures. 
        /// </param>
        public ComplexStructureQueryMetadataCore(bool returnMatchedArtefact, ComplexStructureQueryDetail queryDetail, 
            ComplexMaintainableQueryDetail referencesQueryDetail, StructureReferenceDetail referenceDetail,
            IList<SdmxStructureType> referenceSpecificStructures) 
        {
		
	  	   this._returnMatchedArtefact = returnMatchedArtefact;
		   if (queryDetail != null) 
           {
	 		this._queryDetail = queryDetail;
	       }

	  	   if (referencesQueryDetail != null)
		   this._referencesQueryDetail = referencesQueryDetail;
		
		   if (referenceDetail == null) 
           {
		    	throw new SdmxSemmanticException("Reference Detail cannot be null.");
	  	   }
		
		  this._referenceDetail = referenceDetail;
		  this._referenceSpecificStructures = referenceSpecificStructures;
	   }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexAnnotationReferenceCore" /> class.
        /// </summary>
        /// <param name="returnMatchedArtefact">The return matched artefact.</param>
        /// <param name="processConstraints">if set to <c>true</c> [process constraints].</param>
        /// <param name="queryDetail">The query detail.</param>
        /// <param name="referencesQueryDetail">The references query detail.</param>
        /// <param name="referenceDetail">The reference detail.</param>
        /// <param name="referenceSpecificStructures">The reference specific structures.</param>
        public ComplexStructureQueryMetadataCore(bool returnMatchedArtefact, bool processConstraints, ComplexStructureQueryDetail queryDetail,
            ComplexMaintainableQueryDetail referencesQueryDetail, StructureReferenceDetail referenceDetail,
            IList<SdmxStructureType> referenceSpecificStructures)

            : this(returnMatchedArtefact, queryDetail, referencesQueryDetail, referenceDetail, referenceSpecificStructures)
        {
            this._processConstraints = processConstraints;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the structure query detail.
        /// </summary>
        public virtual ComplexStructureQueryDetail StructureQueryDetail
        {
	  	    get
		    {
		      return _queryDetail;
		    }
        }

       /// <summary>
       ///   Gets the structure reference detail.
       /// </summary>
       public virtual StructureReferenceDetail StructureReferenceDetail
       {
		  get
		  {
		    return _referenceDetail;
		  }
       }

      /// <summary>
      ///   Gets the references query detail.
      /// </summary>
	  public virtual ComplexMaintainableQueryDetail ReferencesQueryDetail
      {
		 get
		 {
		    return _referencesQueryDetail;
		 }
      }

      /// <summary>
      ///   Gets the reference specific structures.
      /// </summary>
	  public virtual IList<SdmxStructureType> ReferenceSpecificStructures
      {
        get
        {
            return _referenceSpecificStructures;
        }
      }

      /// <summary>
      /// Gets a value indicating whether the attribute processConstraints is set to true. Triggers potential creation of partial structures.
      /// </summary>
      /// <value><c>true</c> if the attribute processConstraints is set to true. otherwise, <c>false</c>.</value>
      public bool IsProcessConstraints
      {
          get
          {
              return this._processConstraints;
          }
      }

        #endregion

      #region Public Methods and Operators

      /// <summary>
      ///   The is returned matched artefact.
      /// </summary>
      /// <returns> The <see cref="bool" /> . </returns>
      public virtual bool IsReturnedMatchedArtefact()
      {
          return _returnMatchedArtefact;
      }

      #endregion
    }
}
