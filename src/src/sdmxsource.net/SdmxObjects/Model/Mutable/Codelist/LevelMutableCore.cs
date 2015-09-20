// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The level mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The level mutable core.
    /// </summary>
    [Serializable]
    public class LevelMutableCore : NameableMutableCore, ILevelMutableObject
    {
        #region Fields

        /// <summary>
        ///   The child level.
        /// </summary>
        private ILevelMutableObject childLevel;

        /// <summary>
        ///   The coding format.
        /// </summary>
        private ITextFormatMutableObject codingFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="LevelMutableCore" /> class.
        /// </summary>
        public LevelMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Level))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelMutableCore"/> class.
        /// </summary>
        /// <param name="level">
        /// The level. 
        /// </param>
        public LevelMutableCore(ILevelObject level)
            : base(level)
        {
            if (level.HasChild())
            {
                this.childLevel = new LevelMutableCore(level.ChildLevel);
            }

            if (level.CodingFormat != null)
            {
                this.codingFormat = new TextFormatMutableCore(level.CodingFormat);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the child level.
        /// </summary>
        public virtual ILevelMutableObject ChildLevel
        {
            get
            {
                return this.childLevel;
            }

            set
            {
                this.childLevel = value;
            }
        }

        /// <summary>
        ///   Gets or sets the coding format.
        /// </summary>
        public virtual ITextFormatMutableObject CodingFormat
        {
            get
            {
                return this.codingFormat;
            }

            set
            {
                this.codingFormat = value;
            }
        }

        #endregion
    }
}