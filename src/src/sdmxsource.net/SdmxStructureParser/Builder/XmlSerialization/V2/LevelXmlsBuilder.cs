// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelXmlsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The level xml beans builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The level xml beans builder.
    /// </summary>
    public class LevelXmlsBuilder : AbstractBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Populate <see cref="HierarchyType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="hierarchyType">
        /// The hierarchy type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        public void BuildList(HierarchyType hierarchyType, ILevelObject buildFrom)
        {
            int order = 1;
            do
            {
                var builtObj = new LevelType();
                hierarchyType.Level.Add(builtObj);

                string value = buildFrom.Id;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builtObj.id = buildFrom.Id;
                }

                if (ObjectUtil.ValidString(buildFrom.Urn))
                {
                    builtObj.urn = buildFrom.Urn;
                }

                IList<ITextTypeWrapper> names = buildFrom.Names;
                if (ObjectUtil.ValidCollection(names))
                {
                    builtObj.Name = this.GetTextType(names);
                }

                IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
                if (ObjectUtil.ValidCollection(descriptions))
                {
                    builtObj.Description = this.GetTextType(descriptions);
                }

                if (this.HasAnnotations(buildFrom))
                {
                    builtObj.Annotations = this.GetAnnotationsType(buildFrom);
                }

                builtObj.Order = order;
                if (buildFrom.CodingFormat != null)
                {
                    var textFormatType = new TextFormatType();
                    this.PopulateTextFormatType(textFormatType, buildFrom.CodingFormat);
                    builtObj.CodingType = textFormatType;
                }

                buildFrom = buildFrom.ChildLevel;
                order++;
            }
            while (buildFrom != null);
        }

        #endregion
    }
}