// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalDataStructureMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross sectional data structure mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///   The cross sectional data structure mutable core.
    /// </summary>
    [Serializable]
    public class CrossSectionalDataStructureMutableCore : DataStructureMutableCore, 
                                                          ICrossSectionalDataStructureMutableObject
    {
        #region Fields

        /// <summary>
        ///   The attribute to measure map.
        /// </summary>
        private readonly IDictionaryOfLists<string, string> _attributeToMeasureMap;

        /// <summary>
        ///   The cross sectional attach data set.
        /// </summary>
        private readonly IList<string> _crossSectionalAttachDataSet;

        /// <summary>
        ///   The cross sectional attach group.
        /// </summary>
        private readonly IList<string> _crossSectionalAttachGroup;

        /// <summary>
        ///   The cross sectional attach observation.
        /// </summary>
        private readonly IList<string> _crossSectionalAttachObservation;

        /// <summary>
        ///   The cross sectional attach section.
        /// </summary>
        private readonly IList<string> _crossSectionalAttachSection;

        /// <summary>
        ///   The cross sectional measures.
        /// </summary>
        private readonly IList<ICrossSectionalMeasureMutableObject> _crossSectionalMeasures;

        /// <summary>
        ///   The measure codelist mapping.
        /// </summary>
        private readonly IDictionary<string, IStructureReference> _measureCodelistMapping = new Dictionary<string, IStructureReference>(StringComparer.Ordinal);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CrossSectionalDataStructureMutableCore" /> class.
        /// </summary>
        public CrossSectionalDataStructureMutableCore()
        {
            this._crossSectionalMeasures = new List<ICrossSectionalMeasureMutableObject>();
            this._crossSectionalAttachDataSet = new List<string>();
            this._crossSectionalAttachGroup = new List<string>();
            this._crossSectionalAttachSection = new List<string>();
            this._crossSectionalAttachObservation = new List<string>();
            this._attributeToMeasureMap = new DictionaryOfLists<string, string>(StringComparer.Ordinal); 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalDataStructureMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CrossSectionalDataStructureMutableCore(ICrossSectionalDataStructureObject objTarget)
            : base(objTarget)
        {
            this._crossSectionalMeasures = new List<ICrossSectionalMeasureMutableObject>();
            this._crossSectionalAttachDataSet = new List<string>();
            this._crossSectionalAttachGroup = new List<string>();
            this._crossSectionalAttachSection = new List<string>();
            this._crossSectionalAttachObservation = new List<string>();
            this._attributeToMeasureMap = new DictionaryOfLists<string, string>(StringComparer.Ordinal);
            PopulateList(this._crossSectionalAttachDataSet, objTarget.GetCrossSectionalAttachDataSet(false));
            PopulateList(this._crossSectionalAttachGroup, objTarget.GetCrossSectionalAttachGroup(false));
            PopulateList(this._crossSectionalAttachObservation, objTarget.GetCrossSectionalAttachObservation());
            PopulateList(this._crossSectionalAttachSection, objTarget.GetCrossSectionalAttachSection(false));

            foreach (IAttributeObject attribute in objTarget.Attributes)
            {
                IList<string> measureIds = new List<string>();

                foreach (ICrossSectionalMeasure xsMeasure in objTarget.GetAttachmentMeasures(attribute))
                {
                    measureIds.Add(xsMeasure.Id);
                }

                this._attributeToMeasureMap.Add(attribute.Id, measureIds);
            }

            foreach (ICrossSectionalMeasure measure in objTarget.CrossSectionalMeasures)
            {
                this._crossSectionalMeasures.Add(new CrossSectionalMeasureMutableCore(measure));
            }

            foreach (var dimension in objTarget.GetDimensions(SdmxStructureEnumType.MeasureDimension))
            {
                var codelistForMeasureDimension = objTarget.GetCodelistForMeasureDimension(dimension.Id);
                if (codelistForMeasureDimension != null)
                {
                    this._measureCodelistMapping[dimension.Id] = codelistForMeasureDimension.CreateMutableInstance();
                }
                
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attribute to measure map.
        /// </summary>
        public IDictionaryOfLists<string, string> AttributeToMeasureMap
        {
            get
            {
                return this._attributeToMeasureMap;
            }
        }

        /// <summary>
        ///   Gets the cross sectional attach data set.
        /// </summary>
        public IList<string> CrossSectionalAttachDataSet
        {
            get
            {
                return this._crossSectionalAttachDataSet;
            }
        }

        /// <summary>
        ///   Gets the cross sectional attach group.
        /// </summary>
        public IList<string> CrossSectionalAttachGroup
        {
            get
            {
                return this._crossSectionalAttachGroup;
            }
        }

        /// <summary>
        ///   Gets the cross sectional attach observation.
        /// </summary>
        public IList<string> CrossSectionalAttachObservation
        {
            get
            {
                return this._crossSectionalAttachObservation;
            }
        }

        /// <summary>
        ///   Gets the cross sectional attach section.
        /// </summary>
        public IList<string> CrossSectionalAttachSection
        {
            get
            {
                return this._crossSectionalAttachSection;
            }
        }

        /// <summary>
        ///   Gets the cross sectional measures.
        /// </summary>
        public IList<ICrossSectionalMeasureMutableObject> CrossSectionalMeasures
        {
            get
            {
                return this._crossSectionalMeasures;
            }
        }


        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        ICrossSectionalDataStructureObject ICrossSectionalDataStructureMutableObject.ImmutableInstance
        {
            get
            {
                return new CrossSectionalDataStructureObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IDataStructureObject ImmutableInstance
        {
            get
            {
                return new CrossSectionalDataStructureObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets the measure dimension codelist mapping.
        /// </summary>
        public IDictionary<string, IStructureReference> MeasureDimensionCodelistMapping
        {
            get
            {
                return this._measureCodelistMapping;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add component reference to  cross sectional attach data set.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference. 
        /// </param>
        public void AddCrossSectionalAttachDataSet(string dimensionReference)
        {
            this._crossSectionalAttachDataSet.Add(dimensionReference);
        }

        /// <summary>
        /// Add component reference to  cross sectional attach group.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference. 
        /// </param>
        public void AddCrossSectionalAttachGroup(string dimensionReference)
        {
            this._crossSectionalAttachGroup.Add(dimensionReference);
        }

        /// <summary>
        /// Add component reference to  cross sectional attach observation.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference. 
        /// </param>
        public void AddCrossSectionalAttachObservation(string dimensionReference)
        {
            this._crossSectionalAttachObservation.Add(dimensionReference);
        }

        /// <summary>
        /// Add component reference to  cross sectional attach section.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference. 
        /// </param>
        public void AddCrossSectionalAttachSection(string dimensionReference)
        {
            this._crossSectionalAttachSection.Add(dimensionReference);
        }

        /// <summary>
        /// Add component reference to  cross sectional measures.
        /// </summary>
        /// <param name="crossSectionalMeasure">
        /// The cross sectional measure. 
        /// </param>
        public void AddCrossSectionalMeasures(ICrossSectionalMeasureMutableObject crossSectionalMeasure)
        {
            this._crossSectionalMeasures.Add(crossSectionalMeasure);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The populate list.
        /// </summary>
        /// <param name="toPopulateList">
        /// The to populate list. 
        /// </param>
        /// <param name="components">
        /// The components. 
        /// </param>
        private static void PopulateList(ICollection<string> toPopulateList, IEnumerable<IComponent> components)
        {
            foreach (IComponent currentComponent in components)
            {
                toPopulateList.Add(currentComponent.Id);
            }
        }

        #endregion
    }
}