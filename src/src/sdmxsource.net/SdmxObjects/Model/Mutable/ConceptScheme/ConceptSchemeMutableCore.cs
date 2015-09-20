// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;

    /// <summary>
    ///   The concept scheme mutable core.
    /// </summary>
    [Serializable]
    public class ConceptSchemeMutableCore :
        ItemSchemeMutableCore<IConceptMutableObject, IConceptObject, IConceptSchemeObject>, 
        IConceptSchemeMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConceptSchemeMutableCore" /> class.
        /// </summary>
        public ConceptSchemeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeMutableCore"/> class.
        /// </summary>
        /// <param name="conceptSchemeObject">
        /// The conceptSchemeObject. 
        /// </param>
        public ConceptSchemeMutableCore(IConceptSchemeObject conceptSchemeObject)
            : base(conceptSchemeObject)
        {
            if (conceptSchemeObject.Items != null)
            {
                foreach (IConceptObject icurrent in conceptSchemeObject.Items)
                {
                    this.AddItem(new ConceptMutableCore(icurrent));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IConceptSchemeObject ImmutableInstance
        {
            get
            {
                return new ConceptSchemeObjectCore(this);
            }
        }

        #endregion

        #region Overrides of ItemSchemeMutableCore<IConceptMutableObject,IConceptObject,IConceptSchemeObject>

        public override IConceptMutableObject CreateItem(string id, string name)
        {
            IConceptMutableObject concept = new ConceptMutableCore();
            concept.Id = id;
            concept.AddName("en", name);
            AddItem(concept);
            return concept;
        }

        #endregion
    }
}