// -----------------------------------------------------------------------
// <copyright file="MutableMaintainableExtensions.cs" company="Eurostat">
//   Date Created : 2013-09-23
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Collections.Generic;

    using Estat.Sdmxsource.Extension.Constant;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Annotation;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// Various extensions for <see cref="IMaintainableMutableObject"/>
    /// </summary>
    public static class MutableMaintainableExtensions
    {
        /// <summary>
        /// The _default URI
        /// </summary>
        private static readonly Uri _defaultUri = new Uri("http://need/to/changeit");

        /// <summary>
        /// Returns a new stub <see cref="IMaintainableMutableObject"/> object based on the specified <paramref name="maintainable"/>.
        /// The new object is a shallow copy.
        /// </summary>
        /// <typeparam name="TInterface">The maintainable type (interface).</typeparam>
        /// <typeparam name="TImplementation">The maintainable type (concrete implementation).</typeparam>
        /// <param name="maintainable">The maintainable.</param>
        /// <returns>A new stub <see cref="IMaintainableMutableObject"/> object based on the specified <paramref name="maintainable"/>; otherwise if <paramref name="maintainable"/> is null it returns null.</returns>
        public static TInterface CloneAsStub<TInterface, TImplementation>(this TInterface maintainable) where TInterface : IMaintainableMutableObject
            where TImplementation : TInterface, new()
        {
            if (maintainable.IsDefault())
            {
                return default(TInterface);
            }

            var stub = new TImplementation
                           {
                               Id = maintainable.Id,
                               AgencyId = maintainable.AgencyId,
                               Version = maintainable.Version,
                               StructureURL = _defaultUri,
                               StartDate = maintainable.StartDate,
                               EndDate = maintainable.EndDate,
                               FinalStructure = maintainable.FinalStructure,
                               ServiceURL = maintainable.ServiceURL,
                               Uri = maintainable.Uri,
                               ExternalReference = TertiaryBool.ParseBoolean(true),

                               // TODO following line when we remove when sync to CAPI v1.0
                               Stub = true
                           };

            stub.Annotations.AddAll(maintainable.Annotations);
            stub.Names.AddAll(maintainable.Names);

            return stub;
        }

        /// <summary>
        /// Converts the specified <paramref name="crossDsd"/> to stub.
        /// </summary>
        /// <param name="crossDsd">The cross DSD.</param>
        public static void ConvertToStub(this ICrossSectionalDataStructureMutableObject crossDsd)
        {
            crossDsd.Stub = true;
            crossDsd.ExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);
            crossDsd.StructureURL = _defaultUri;
            
            // remove components
            crossDsd.AttributeList = null;
            crossDsd.DimensionList = new DimensionListMutableCore();
            crossDsd.Dimensions.Clear();
            crossDsd.Groups.Clear();
            crossDsd.MeasureList = null;
            crossDsd.CrossSectionalAttachDataSet.Clear();
            crossDsd.CrossSectionalAttachGroup.Clear();
            crossDsd.CrossSectionalAttachSection.Clear();
            crossDsd.CrossSectionalAttachObservation.Clear();
            crossDsd.CrossSectionalMeasures.Clear();
            crossDsd.MeasureDimensionCodelistMapping.Clear();
            crossDsd.AttributeToMeasureMap.Clear();
        }

        /// <summary>
        /// Normalizes the SDMXV20 data structure.
        /// </summary>
        /// <param name="crossDsd">The cross DSD.</param>
        public static void NormalizeSdmxv20DataStructure(this ICrossSectionalDataStructureMutableObject crossDsd)
        {
            crossDsd.ConvertToStub();
            var annotation = CustomAnnotationType.SDMXv20Only.ToAnnotation<AnnotationMutableCore>();
            crossDsd.AddAnnotation(annotation);
        }

        /// <summary>
        /// Normalizes the SDMXV20 data structure.
        /// </summary>
        /// <param name="dataStructure">The data structure.</param>
        public static void NormalizeSdmxv20DataStructure(this IDataStructureMutableObject dataStructure)
        {
            ApplyCodedTimeDimensionNormalization(dataStructure);

            var crossDsd = dataStructure as ICrossSectionalDataStructureMutableObject;
            if (crossDsd != null)
            {
                crossDsd.NormalizeSdmxv20DataStructure();
            }
        }

        /// <summary>
        /// Normalizes the SDMXV20 data structure.
        /// </summary>
        /// <param name="dataStructure">The data structure.</param>
        public static void NormalizeSdmxv20DataStructure(this IMaintainableMutableObject dataStructure)
        {
            var dsd = dataStructure as IDataStructureMutableObject;
            if (dsd != null)
            {
                dsd.NormalizeSdmxv20DataStructure();
            }
        }

        /// <summary>
        /// Normalizes the SDMXV20 data structures.
        /// </summary>
        /// <param name="dataStructures">The data structures.</param>
        public static void NormalizeSdmxv20DataStructures(this IEnumerable<IDataStructureMutableObject> dataStructures)
        {
            foreach (var crossDsd in dataStructures)
            {
                crossDsd.NormalizeSdmxv20DataStructure();
            }
        }

        /// <summary>
        /// Gets the enumerated representation.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns>The representation <see cref="IStructureReference"/> of the <paramref name="component"/>; otherwise null.</returns>
        public static IStructureReference GetEnumeratedRepresentation(this IComponentMutableObject component)
        {
            return component.Representation != null ? component.Representation.Representation : null;
        }

        /// <summary>
        /// Gets the enumerated representation.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="dsd">The DSD.</param>
        /// <returns>The representation <see cref="IStructureReference"/> of the <paramref name="dimension"/>; otherwise null.</returns>
        public static IStructureReference GetEnumeratedRepresentation(this IDimensionMutableObject dimension, IDataStructureMutableObject dsd)
        {
            if (dimension.MeasureDimension)
            {
                var crossDsd = dsd as ICrossSectionalDataStructureMutableObject;
                if (crossDsd != null)
                {
                    IStructureReference reference = crossDsd.MeasureDimensionCodelistMapping[dimension.Id];
                    return reference;
                }
            }

            return dimension.GetEnumeratedRepresentation();
        }

        /// <summary>
        /// Applies the coded time dimension normalization.
        /// </summary>
        /// <param name="dataStructure">The data structure.</param>
        private static void ApplyCodedTimeDimensionNormalization(IDataStructureMutableObject dataStructure)
        {
            var timeDimension = dataStructure.GetDimension(DimensionObject.TimeDimensionFixedId);
            if (timeDimension != null)
            {
                var codelist = timeDimension.GetEnumeratedRepresentation();
                if (codelist != null)
                {
                    timeDimension.Representation.Representation = null;
                    if (timeDimension.Representation.TextFormat == null)
                    {
                        timeDimension.Representation = null;
                    }

                    IAnnotationBuilder<IMaintainableRefObject> codedTimeAnnotationBuilder = new CodedTimeDimensionAnnotationBuilder<AnnotationMutableCore>();
                    var annotation = codedTimeAnnotationBuilder.Build(codelist);
                    timeDimension.AddAnnotation(annotation);
                }
            }
        }
    }
}