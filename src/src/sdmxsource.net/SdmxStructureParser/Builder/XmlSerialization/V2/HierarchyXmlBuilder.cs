// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchy xml hierarchicalCodelist builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The hierarchy xml hierarchicalCodelist builder.
    /// </summary>
    public class HierarchyXmlBuilder : AbstractBuilder, IBuilder<HierarchyType, IHierarchy>
    {
        #region Fields

        /// <summary>
        ///     The code ref xml hierarchicalCodelist builder.
        /// </summary>
        private readonly CodeRefXmlBuilder _codeRefXmlBuilder = new CodeRefXmlBuilder();

        /// <summary>
        ///     The level xml hierarchicalCodelist builder.
        /// </summary>
        private readonly LevelXmlsBuilder _levelXmlBuilder = new LevelXmlsBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="HierarchyType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="HierarchyType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual HierarchyType Build(IHierarchy buildFrom)
        {
            var builtObj = new HierarchyType();
            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.id = buildFrom.Id;
            }

            builtObj.urn = buildFrom.Urn;

            ICollection<ITextTypeWrapper> collection = buildFrom.Names;
            if (ObjectUtil.ValidCollection(collection))
            {
                builtObj.Name = this.GetTextType(buildFrom.Names);
            }

            ICollection<ITextTypeWrapper> collection1 = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(collection1))
            {
                builtObj.Description = this.GetTextType(buildFrom.Descriptions);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            IList<IHierarchicalCode> hierarchicalCodeBeans = buildFrom.HierarchicalCodeObjects;
            if (ObjectUtil.ValidCollection(hierarchicalCodeBeans))
            {
                /* foreach */
                foreach (IHierarchicalCode currentCodeRef in hierarchicalCodeBeans)
                {
                    builtObj.CodeRef.Add(this._codeRefXmlBuilder.Build(currentCodeRef));
                }
            }

            if (buildFrom.Level != null)
            {
                this._levelXmlBuilder.BuildList(builtObj, buildFrom.Level);
            }

            return builtObj;
        }

        #endregion
    }
}