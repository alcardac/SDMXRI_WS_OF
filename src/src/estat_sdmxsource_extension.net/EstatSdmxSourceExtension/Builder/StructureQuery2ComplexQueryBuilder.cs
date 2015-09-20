// -----------------------------------------------------------------------
// <copyright file="StructureQuery2ComplexQueryBuilder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sdmxsource.Extension.Builder
{
    using System;

    using Estat.Sdmxsource.Extension.Extension;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Util.Extension;

    /// <summary>
    ///  Builder class of IComplexStructureQuery object from IRestStructureQuery object.
    ///  Used for transforming messages in SDMX 2.0 version to SDMX 2.1 Messages. 
    /// </summary>
    public class StructureQuery2ComplexQueryBuilder : IBuilder<IComplexStructureQuery, IRestStructureQuery>
    {

      /// <summary>
      /// The build
      /// </summary>
      /// <param name="structureQuery">
      /// The structure query
      /// </param>
      public virtual IComplexStructureQuery Build(IRestStructureQuery structureQuery) 
      {
		 IStructureReference queryReferenceBean = structureQuery.StructureReference.ChangeStarsToNull();
		 IComplexStructureReferenceObject complexQueryReferenceBean = GetComplexReference(queryReferenceBean, structureQuery.StructureQueryMetadata.IsReturnLatest);
		 IComplexStructureQueryMetadata complexStructureQueryMetadata = GetComplexStructureQueryMetadata(structureQuery.StructureQueryMetadata);
		
		 IComplexStructureQuery complexStructureQuery = new ComplexStructureQueryCore(complexQueryReferenceBean, complexStructureQueryMetadata);
		 return complexStructureQuery;
   	  }

        /// <summary>
        /// The get complex reference
        /// </summary>
        /// <param name="sRef">
        ///     The sref
        /// </param>
        private IComplexStructureReferenceObject GetComplexReference(IStructureReference sRef, bool isReturnLatest)
      {
		  /*create an instance of ComplexStructureReferenceBean for the sRef provided */
		  IMaintainableRefObject mRef = sRef.MaintainableReference;
		  IComplexTextReference agencyId = null;
          if (mRef.AgencyId != null)
          {
            agencyId = new ComplexTextReferenceCore("en", TextSearch.GetFromEnum(TextSearchEnumType.Equal), mRef.AgencyId);
          }
          IComplexTextReference id = null;
          if (mRef.MaintainableId != null)
          {
              id = new ComplexTextReferenceCore("en", TextSearch.GetFromEnum(TextSearchEnumType.Equal), mRef.MaintainableId);
          }
          IComplexVersionReference versionRef = null;
     
          versionRef = !isReturnLatest || mRef.Version != null ? new ComplexVersionReferenceCore(TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), mRef.Version, null, null) : new ComplexVersionReferenceCore(TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True), null, null, null);
          IComplexStructureReferenceObject complexStructureRef = new ComplexStructureReferenceCore(agencyId, id, versionRef, sRef.TargetReference, null, null, null, null);
		  return complexStructureRef;
	   }

       /// <summary>
       /// The get complex structure query metadata
       /// </summary>
       /// <param name="queryMetadata">
       /// The structure type. 
       /// </param>
	   private IComplexStructureQueryMetadata GetComplexStructureQueryMetadata(IStructureQueryMetadata queryMetadata)
       {
           ComplexStructureQueryDetail queryDetail = queryMetadata.StructureQueryDetail.ToComplex();
           ComplexMaintainableQueryDetail complexMaintainableQuery = queryMetadata.StructureQueryDetail.ToComplexReference();

           /*create an instance of ComplexStructureQueryMetadata  */
           var referenceSpecificStructures = queryMetadata.SpecificStructureReference != null ? new[] { queryMetadata.SpecificStructureReference } : null;
           IComplexStructureQueryMetadata complexStructureQueryMetadata = new ComplexStructureQueryMetadataCore(
               false,queryDetail,
                complexMaintainableQuery,
                queryMetadata.StructureReferenceDetail, referenceSpecificStructures);

		return complexStructureQueryMetadata;
	    }



    }
}
