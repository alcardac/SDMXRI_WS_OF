// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This Builder is realised as being able to build Mutable beans and from a collection of beans.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    ///     This Builder is realized as being able to build Mutable beans and from a collection of beans.
    /// </summary>
    public interface IMutableObjectsBuilder : IBuilder<IMutableObjects, ISdmxObjects>
    {
    }
}