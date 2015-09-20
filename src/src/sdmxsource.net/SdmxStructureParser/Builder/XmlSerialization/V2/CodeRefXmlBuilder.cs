// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeRefXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code ref xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The code ref xml bean builder.
    /// </summary>
    public class CodeRefXmlBuilder : AbstractBuilder, IBuilder<CodeRefType, IHierarchicalCode>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CodeRefType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CodeRefType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual CodeRefType Build(IHierarchicalCode buildFrom)
        {
            var pairs = new Stack<KeyValuePair<CodeRefType, IHierarchicalCode>>();
            var root = new CodeRefType();
            pairs.Push(new KeyValuePair<CodeRefType, IHierarchicalCode>(root, buildFrom));

            while (pairs.Count > 0)
            {
                KeyValuePair<CodeRefType, IHierarchicalCode> kv = pairs.Pop();
                buildFrom = kv.Value;
                CodeRefType builtObj = kv.Key;

                string value = buildFrom.CodeId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    builtObj.CodeID = buildFrom.CodeId;
                }

                string value1 = buildFrom.Id;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    builtObj.NodeAliasID = buildFrom.Id;
                }

                string value2 = buildFrom.CodelistAliasRef;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    builtObj.CodelistAliasRef = buildFrom.CodelistAliasRef;
                }
                else
                {
                    ICrossReference crossRefernce = buildFrom.CodeReference;
                    if (crossRefernce != null)
                    {
                        builtObj.URN = crossRefernce.TargetUrn;
                    }
                }

                if (buildFrom.ValidFrom != null)
                {
                    builtObj.ValidFrom = buildFrom.ValidFrom.Date;
                }

                if (buildFrom.ValidTo != null)
                {
                    builtObj.ValidTo = buildFrom.ValidTo.Date;
                }

                if (buildFrom.GetLevel(false) != null)
                {
                    builtObj.LevelRef = buildFrom.GetLevel(false).Id;
                }

                if (ObjectUtil.ValidCollection(buildFrom.CodeRefs))
                {
                    foreach (IHierarchicalCode currentCodeRef in buildFrom.CodeRefs)
                    {
                        var item = new CodeRefType();
                        builtObj.CodeRef.Add(item);
                        pairs.Push(new KeyValuePair<CodeRefType, IHierarchicalCode>(item, currentCodeRef));
                    }
                }
            }

            return root;
        }

        #endregion
    }
}