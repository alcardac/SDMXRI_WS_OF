// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The representation mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   The representation mutable core.
    /// </summary>
    [Serializable]
    public class RepresentationMutableCore : MutableCore, IRepresentationMutableObject
    {
        #region Fields

        /// <summary>
        ///   The representation ref.
        /// </summary>
        private IStructureReference representationRef;

        /// <summary>
        ///   The text format.
        /// </summary>
        private ITextFormatMutableObject textFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="RepresentationMutableCore" /> class.
        /// </summary>
        public RepresentationMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.LocalRepresentation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationMutableCore"/> class.
        /// </summary>
        /// <param name="representation">
        /// The representation. 
        /// </param>
        public RepresentationMutableCore(IRepresentation representation)
            : base(representation)
        {
            if (representation.TextFormat != null)
            {
                this.textFormat = new TextFormatMutableCore(representation.TextFormat);
            }

            if (representation.Representation != null)
            {
                this.representationRef = representation.Representation.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the representation.
        /// </summary>
        public virtual IStructureReference Representation
        {
            get
            {
                return this.representationRef;
            }

            set
            {
                this.representationRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the text format.
        /// </summary>
        public virtual ITextFormatMutableObject TextFormat
        {
            get
            {
                return this.textFormat;
            }

            set
            {
                this.textFormat = value;
            }
        }

        #endregion
    }
}