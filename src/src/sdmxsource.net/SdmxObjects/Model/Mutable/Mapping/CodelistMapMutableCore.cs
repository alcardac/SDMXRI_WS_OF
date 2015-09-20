// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///   The codelist map mutable core.
    /// </summary>
    [Serializable]
    public class CodelistMapMutableCore : ItemSchemeMapMutableCore, ICodelistMapMutableObject
    {
        #region Fields

        /// <summary>
        ///   The src alias.
        /// </summary>
        private string srcAlias;

        /// <summary>
        ///   The target alias.
        /// </summary>
        private string targetAlias;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CodelistMapMutableCore" /> class.
        /// </summary>
        public CodelistMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListMap))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMapMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CodelistMapMutableCore(ICodelistMapObject objTarget)
            : base(objTarget)
        {
            this.SrcAlias = objTarget.SrcAlias;
            this.TargetAlias = objTarget.TargetAlias;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the src alias.
        /// </summary>
        public string SrcAlias
        {
            get
            {
                return this.srcAlias;
            }

            set
            {
                this.srcAlias = value;
            }
        }

        /// <summary>
        ///   Gets or sets the target alias.
        /// </summary>
        public string TargetAlias
        {
            get
            {
                return this.targetAlias;
            }

            set
            {
                this.targetAlias = value;
            }
        }

        #endregion
    }
}