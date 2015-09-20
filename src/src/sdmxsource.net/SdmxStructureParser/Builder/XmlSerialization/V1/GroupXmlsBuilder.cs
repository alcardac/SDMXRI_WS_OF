// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupXmlsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The group xml beans builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The group xml beans builder.
    /// </summary>
    public class GroupXmlsBuilder : AbstractBuilder, IBuilder<GroupType, IGroup>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="GroupXmlsBuilder" /> class.
        /// </summary>
        static GroupXmlsBuilder()
        {
            Log = LogManager.GetLogger(typeof(GroupXmlsBuilder));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="GroupType"/>.
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