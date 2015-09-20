// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross sectional data structure helper class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;

    /// <summary>
    ///     The cross sectional data structure helper class.
    /// </summary>
    public static class CrossSectionalUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns true if the specified <paramref name="currentType"/> has SDMX v2.0 cross sectional information.
        /// </summary>
        /// <param name="currentType">
        /// The KeyFamily instance
        /// </param>
        /// <returns>
        /// true if the specified <paramref name="currentType"/> has at least some SDMX v2.0 cross sectional information.
        /// </returns>
        public static bool IsCrossSectional(KeyFamilyType currentType)
        {
            bool isCrossSectional = currentType.Components.Dimension.All(
                component =>
                component.isFrequencyDimension || component.isMeasureDimension 
                || (component.crossSectionalAttachDataSet.HasValue && component.crossSectionalAttachDataSet.Value)
                || (component.crossSectionalAttachGroup.HasValue && component.crossSectionalAttachGroup.Value)
                || (component.crossSectionalAttachSection.HasValue && component.crossSectionalAttachSection.Value)
                || (component.crossSectionalAttachObservation.HasValue && component.crossSectionalAttachObservation.Value));
            if (currentType.Components.Attribute.Count > 0)
            {
                isCrossSectional = isCrossSectional
                                   && currentType.Components.Attribute.All(
                                       component =>
                                       (component.crossSectionalAttachDataSet.HasValue && component.crossSectionalAttachDataSet.Value)
                                       || (component.crossSectionalAttachGroup.HasValue && component.crossSectionalAttachGroup.Value)
                                       || (component.crossSectionalAttachSection.HasValue && component.crossSectionalAttachSection.Value)
                                       || (component.crossSectionalAttachObservation.HasValue && component.crossSectionalAttachObservation.Value));
            }

            return isCrossSectional;
        }

        #endregion
    }
}