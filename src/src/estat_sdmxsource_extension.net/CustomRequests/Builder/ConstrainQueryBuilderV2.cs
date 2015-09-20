// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstrainQueryBuilderV2.cs" company="Eurostat">
//   Date Created : 2013-03-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Singleton factory pattern to build Version 2 reference beans from query types
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Builder
{
    using Estat.Sri.CustomRequests.Constants;
    using Estat.Sri.CustomRequests.Model;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.Query;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     Singleton factory pattern to build Version 2 reference beans from query types
    /// </summary>
    public class ConstrainQueryBuilderV2 : QueryBuilderV2
    {
        #region Methods

        /// <summary>
        /// Build dataflow query. Override to alter the default behavior
        /// </summary>
        /// <param name="dataflowRefType">
        /// The Dataflow reference (SDMX v2.0
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        protected override IStructureReference BuildDataflowQuery(DataflowRefType dataflowRefType)
        {
            var urn = dataflowRefType.URN;
            var immutableInstance = BuildConstraintObject(dataflowRefType);

            StructureReferenceImpl structureReferenceImpl;
            if (ObjectUtil.ValidString(urn))
            {
                structureReferenceImpl = new ConstrainableStructureReference(urn);
            }
            else
            {
                string agencyId16 = dataflowRefType.AgencyID;
                string maintId17 = dataflowRefType.DataflowID;
                string version18 = dataflowRefType.Version;

                structureReferenceImpl = new ConstrainableStructureReference(agencyId16, maintId17, version18, SdmxStructureEnumType.Dataflow, immutableInstance);
            }

            return structureReferenceImpl;
        }

        /// <summary>
        /// Add dummy values for validity.
        /// </summary>
        /// <param name="dataflowRefType">
        /// The dataflow ref type.
        /// </param>
        /// <param name="mutable">
        /// The mutable.
        /// </param>
        private static void AddDummyValuesForValidity(DataflowRefType dataflowRefType, IMaintainableMutableObject mutable)
        {
            mutable.AddName("en", "Content Constraint for dataflow"); // TODO: Name is hardcoded.
            mutable.AgencyId = dataflowRefType.AgencyID;
        }

        /// <summary>
        /// The build constraint object.
        /// </summary>
        /// <param name="dataflowRefType">
        /// The dataflow ref type.
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/>.
        /// </returns>
        private static IContentConstraintObject BuildConstraintObject(DataflowRefType dataflowRefType)
        {
            IContentConstraintObject immutableInstance = null;
            if (dataflowRefType.Constraint != null)
            {
                string id = dataflowRefType.Constraint.ConstraintID;
                IContentConstraintMutableObject mutable = new ContentConstraintMutableCore();
                AddDummyValuesForValidity(dataflowRefType, mutable);
                mutable.Id = id;
                foreach (CubeRegionType cubeRegionType in dataflowRefType.Constraint.CubeRegion)
                {
                    ICubeRegionMutableObject cubeRegion = BuildCubeRegion(cubeRegionType);

                    if (cubeRegionType.isIncluded && mutable.IncludedCubeRegion == null)
                    {
                        mutable.IncludedCubeRegion = cubeRegion;
                    }
                    else if (mutable.ExcludedCubeRegion == null)
                    {
                        mutable.ExcludedCubeRegion = cubeRegion;
                    }
                }

                if (dataflowRefType.Constraint.ReferencePeriod != null)
                {
                    mutable.ReferencePeriod = new ReferencePeriodMutableCore
                                                  {
                                                      StartTime = dataflowRefType.Constraint.ReferencePeriod.startTime,
                                                      EndTime = dataflowRefType.Constraint.ReferencePeriod.endTime
                                                  };
                }

                immutableInstance = mutable.ImmutableInstance;
            }

            return immutableInstance;
        }

        /// <summary>
        /// The build cube region.
        /// </summary>
        /// <param name="cubeRegionType">
        /// The cube region type.
        /// </param>
        /// <returns>
        /// The <see cref="ICubeRegionMutableObject"/>.
        /// </returns>
        private static ICubeRegionMutableObject BuildCubeRegion(CubeRegionType cubeRegionType)
        {
            ICubeRegionMutableObject cubeRegion = new CubeRegionMutableCore();
            foreach (MemberType memberType in cubeRegionType.Member)
            {
                IKeyValuesMutable keyValues = new KeyValuesMutableImpl();
                keyValues.Id = memberType.ComponentRef;
                cubeRegion.AddKeyValue(keyValues);
                if (memberType.MemberValue == null || memberType.MemberValue.Count == 0)
                {
                    keyValues.AddValue(SpecialValues.DummyMemberValue);
                }
                else
                {
                    foreach (MemberValueType memberValueType in memberType.MemberValue)
                    {
                        keyValues.AddValue(memberValueType.Value);
                    }
                }
            }

            return cubeRegion;
        }

        #endregion
    }
}