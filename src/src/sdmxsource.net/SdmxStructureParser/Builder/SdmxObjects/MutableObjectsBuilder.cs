// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MutableObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This Builder is realized as being able to build Mutable beans and from a collection of beans.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     This Builder is realized as being able to build Mutable beans and from a collection of beans.
    /// </summary>
    public class MutableObjectsBuilder : IMutableObjectsBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds Mutable beans from a collection of beans
        /// </summary>
        /// <param name="buildFrom">
        /// beans to build from
        /// </param>
        /// <returns>
        /// Mutable bean copies
        /// </returns>
        /// <exception cref="BuilderException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public virtual IMutableObjects Build(ISdmxObjects buildFrom)
        {
            IMutableObjects mutableBeans = new MutableObjectsImpl();

            /* foreach */
            foreach (IMaintainableObject currentMaintainable in buildFrom.GetAllMaintainables())
            {
                mutableBeans.AddIdentifiable(currentMaintainable.MutableInstance);
            }

            return mutableBeans;
        }

        #endregion
    }
}