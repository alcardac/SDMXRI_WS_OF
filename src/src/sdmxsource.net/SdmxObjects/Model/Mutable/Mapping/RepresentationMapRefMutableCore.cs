// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationMapRefMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The representation map ref mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///   The representation map ref mutable core.
    /// </summary>
    [Serializable]
    public class RepresentationMapRefMutableCore : MutableCore, IRepresentationMapRefMutableObject
    {
        #region Fields

        /// <summary>
        ///   The codelist map.
        /// </summary>
        private IStructureReference codelistMap;

        /// <summary>
        ///   The to text format.
        /// </summary>
        private ITextFormatMutableObject toTextFormat;

        /// <summary>
        ///   The to value type.
        /// </summary>
        private ToValue toValueType;

        /// <summary>
        ///   The value mappings.
        /// </summary>
        private IDictionaryOfSets<string, string> valueMappings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="RepresentationMapRefMutableCore" /> class.
        /// </summary>
        public RepresentationMapRefMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RepresentationMap))
        {
            this.valueMappings = new DictionaryOfSets<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationMapRefMutableCore"/> class.
        /// </summary>
        /// <param name="representationMapRef">
        /// The iref. 
        /// </param>
        public RepresentationMapRefMutableCore(IRepresentationMapRef representationMapRef)
            : base(representationMapRef)
        {
            this.valueMappings = new DictionaryOfSets<string, string>();
            if (representationMapRef.CodelistMap != null)
            {
                this.codelistMap = representationMapRef.CodelistMap.CreateMutableInstance();
            }

            if (representationMapRef.ToTextFormat != null)
            {
                this.toTextFormat = new TextFormatMutableCore(representationMapRef.ToTextFormat);
            }

            this.toValueType = representationMapRef.ToValueType;
            this.valueMappings = representationMapRef.ValueMappings;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the codelist map.
        /// </summary>
        public virtual IStructureReference CodelistMap
        {
            get
            {
                return this.codelistMap;
            }

            set
            {
                this.codelistMap = value;
            }
        }

        /// <summary>
        ///   Gets or sets the to text format.
        /// </summary>
        public virtual ITextFormatMutableObject ToTextFormat
        {
            get
            {
                return this.toTextFormat;
            }

            set
            {
                this.toTextFormat = value;
            }
        }

        /// <summary>
        ///   Gets or sets the to value type.
        /// </summary>
        public virtual ToValue ToValueType
        {
            get
            {
                return this.toValueType;
            }

            set
            {
                this.toValueType = value;
            }
        }

        /// <summary>
        ///   Gets the value mappings.
        /// </summary>
        public virtual IDictionaryOfSets<string, string> ValueMappings
        {
            get
            {
                return this.valueMappings;
            }
        }

        #endregion

	    public virtual void AddMapping(string componentId, string componentValue)
        {
		   
            ISet<string> mappings = valueMappings[componentId];
		    if(mappings == null)
            {
			   mappings = new HashSet<string>();
			   valueMappings.Add(componentId, mappings);
		    } 

		    mappings.Add(componentValue);
	    }
    }
}