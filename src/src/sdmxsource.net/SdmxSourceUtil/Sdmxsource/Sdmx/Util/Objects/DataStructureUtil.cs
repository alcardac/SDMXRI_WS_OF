// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The data structure util.
    /// </summary>
    public static class DataStructureUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Convert measure representation from SDMX v2.0 to SDMX v2.1 and <see cref="ICrossSectionalDataStructureObject"/>.
        /// </summary>
        /// <param name="crossSectionalDataStructure">
        /// The cross sectional data structure.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="crossSectionalDataStructure"/> is null
        /// </exception>
        /// <remarks>
        /// HACK HORIBLE HACK 
        /// in 2.0 measure dimensions have a codelist based representation. On 2.1 have concept scheme based representation.
        /// </remarks>
        public static void ConvertMeasureRepresentation(ICrossSectionalDataStructureMutableObject crossSectionalDataStructure)
        {
            if (crossSectionalDataStructure == null)
            {
                throw new ArgumentNullException("crossSectionalDataStructure");
            }

            IDimensionMutableObject measureDim = crossSectionalDataStructure.Dimensions.FirstOrDefault(o => o.MeasureDimension);
            if (measureDim != null && crossSectionalDataStructure.CrossSectionalMeasures.Count > 0)
            {
                IStructureReference crossSectionalMeasureConceptRef = crossSectionalDataStructure.CrossSectionalMeasures[0].ConceptRef;
                IDictionary<string, IStructureReference> measureDimensionCodelistMapping = crossSectionalDataStructure.MeasureDimensionCodelistMapping;
                IStructureReference cocneptSchemeRef = new StructureReferenceImpl(
                    crossSectionalMeasureConceptRef.MaintainableReference, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme));
                measureDimensionCodelistMapping.Add(measureDim.Id ?? measureDim.ConceptRef.ChildReference.Id, measureDim.Representation.Representation);
                measureDim.Representation.Representation = cocneptSchemeRef;
            }
        }

        /// <summary>
        /// Returns the concepts of all the dimensions and attributes that are not attached at the observation level.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object
        /// </param>
        /// <returns>
        /// The concepts of all the dimensions and attributes that are not attached at the observation level.
        /// </returns>
        public static IList<string> GetGroupAttribtueConcepts(IDataStructureObject dataStructureObject)
        {
            IList<string> keyConcepts = new List<string>();
            foreach (IAttributeObject currentBean in dataStructureObject.GroupAttributes)
            {
                keyConcepts.Add(currentBean.Id);
            }

            return keyConcepts;
        }

        /// <summary>
        /// Returns the group id, along with a list of concepts that belong to the group key.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The group id, along with a list of concepts that belong to the group key.
        /// </returns>
        public static IDictionary<string, IList<string>> GetGroupConcepts(IDataStructureObject dataStructureObject)
        {
            IDictionary<string, IList<string>> returnMap = new Dictionary<string, IList<string>>();
            if (dataStructureObject.Groups != null)
            {
                foreach (IGroup currentBean in dataStructureObject.Groups)
                {
                    returnMap.Add(currentBean.Id, currentBean.DimensionRefs);
                }
            }

            return returnMap;
        }

        /// <summary>
        /// Gets the measure concept.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The measure concept.
        /// </returns>
        public static string GetMeasureConcept(IDataStructureObject dataStructureObject)
        {
            return dataStructureObject.PrimaryMeasure.Id;
        }

        /// <summary>
        /// Returns the observation concepts including the measure concept,
        ///     and all the attribute concepts that are attached to the observation.
        ///     The measure concept is always the first concept in the list, the others are added in the order they appear
        ///     in the DataStructureObject.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The observation concepts.
        /// </returns>
        public static IList<string> GetObservationConcepts(IDataStructureObject dataStructureObject)
        {
            IList<string> obsConcepts = new List<string>();
            obsConcepts.Add(GetMeasureConcept(dataStructureObject));
            if (dataStructureObject.Attributes != null)
            {
                foreach (IAttributeObject currentAttribute in dataStructureObject.Attributes)
                {
                    string conceptId = currentAttribute.Id;
                    if (currentAttribute.AttachmentLevel == AttributeAttachmentLevel.Observation)
                    {
                        if (!obsConcepts.Contains(conceptId))
                        {
                            obsConcepts.Add(conceptId);
                        }
                    }
                }
            }

            return obsConcepts;
        }

        /// <summary>
        /// Returns the concept id's belonging to each series attributes in the order they appear in the DataStructureObject,
        ///     followed by the concept id's belonging to each group attributes in the order of the groups,
        ///     and attributes within each group.
        ///     A concept id will not be added to the list twice, so if a concept id appears in a series attribute
        ///     and a group attribute, then the first occurrence will be added to the list, which will be the series
        ///     attribute, the second occurrence will not be added.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The concept id's.
        /// </returns>
        public static IList<string> GetSeriesAndGroupAttributeConcepts(IDataStructureObject dataStructureObject)
        {
            IList<string> attributeConcepts = new List<string>();
            if (dataStructureObject.DimensionGroupAttributes != null)
            {
                foreach (IAttributeObject currentAttribute in dataStructureObject.DimensionGroupAttributes)
                {
                    attributeConcepts.Add(currentAttribute.Id);
                }

                foreach (IAttributeObject currentAttribute in dataStructureObject.GroupAttributes)
                {
                    if (!attributeConcepts.Contains(currentAttribute.Id))
                    {
                        attributeConcepts.Add(currentAttribute.Id);
                    }
                }
            }

            return attributeConcepts;
        }

        /// <summary>
        /// Returns the concepts of all the dimensions and attributes that are not attached at the observation level.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The concepts of all the dimensions and attributes that are not attached at the observation level.
        /// </returns>
        public static IList<string> GetSeriesAttribtueConcepts(IDataStructureObject dataStructureObject)
        {
            IList<string> keyConcepts = new List<string>();

            foreach (IAttributeObject currentBean in dataStructureObject.DimensionGroupAttributes)
            {
                keyConcepts.Add(currentBean.Id);
            }

            return keyConcepts;
        }

        /// <summary>
        /// Returns the series key concepts.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The series key concepts.
        /// </returns>
        public static IList<string> GetSeriesKeyConcepts(IDataStructureObject dataStructureObject)
        {
            IList<string> keyConcepts = new List<string>();
            foreach (IDimension currentDimension in dataStructureObject.GetDimensions(SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension))
            {
                keyConcepts.Add(currentDimension.Id);
            }

            return keyConcepts;
        }

        /// <summary>
        /// Gets the time concept.
        /// </summary>
        /// <param name="dataStructureObject">
        /// The data structure object.
        /// </param>
        /// <returns>
        /// The time concept.
        /// </returns>
        public static string GetTimeConcept(IDataStructureObject dataStructureObject)
        {
            if (dataStructureObject.TimeDimension != null)
            {
                return dataStructureObject.TimeDimension.ConceptRef.ChildReference.Id;
            }

            return null;
        }

        #endregion
    }
}