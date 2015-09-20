// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupXmlsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The group xml beans builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The group xml beans builder.
    /// </summary>
    public class GroupXmlsBuilder : AbstractBuilder, IBuilder<GroupType, IGroup>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="GroupType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="GroupType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual GroupType Build(IGroup buildFrom)
        {
            var builtObj = new GroupType();
            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.id = buildFrom.Id;
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            IList<string> dimensionRefs = buildFrom.DimensionRefs;
            if (ObjectUtil.ValidCollection(dimensionRefs))
            {
                /* foreach */
                foreach (string dimensionRef in dimensionRefs)
                {
                    builtObj.DimensionRef.Add(dimensionRef);
                }
            }

            return builtObj;
        }

        #endregion
    }
}