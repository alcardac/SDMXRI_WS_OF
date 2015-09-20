// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexStructureQueryImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex structure query core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex
{
    #region Using directives

    using System;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion


    /// <summary>
    ///   The complex structure query.
    /// </summary>
    [Serializable]
    public class ComplexStructureQueryCore : IComplexStructureQuery
    {

        #region Fields

        /// <summary>
        ///   The structure ref.
        /// </summary>
        private IComplexStructureReferenceObject _structuRef;

        /// <summary>
        ///   The query metadata.
        /// </summary>
	    private IComplexStructureQueryMetadata _queryMetadata;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexStructureQueryCore"/> class.
        /// </summary>
        /// <param name="structureRef">
        /// The structure ref. 
        /// </param>
        /// <param name="queryMetadata">
        /// The query metadata. 
        /// </param>
        public ComplexStructureQueryCore(IComplexStructureReferenceObject structureRef, IComplexStructureQueryMetadata queryMetadata)
        {
		   if (structureRef == null)
           {
			   throw new SdmxSemmanticException("StructureRefernce cannot be null");
		   }
		   this._structuRef = structureRef;
		
		   if (queryMetadata == null)
           {
		      throw new SdmxSemmanticException("StructureQueryMetadata cannot be null");
		   }
		   this._queryMetadata = queryMetadata;
	    }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the complex structure query metadata.
        /// </summary>
        public virtual IComplexStructureQueryMetadata StructureQueryMetadata
        {
           get
           {
               return _queryMetadata;
           }
        }


        /// <summary>
        ///   Gets the complex structure reference object.
        /// </summary>
	    public virtual IComplexStructureReferenceObject StructureReference
        {
           get
           {
               return _structuRef;
           }
        }

        #endregion
    }
}
