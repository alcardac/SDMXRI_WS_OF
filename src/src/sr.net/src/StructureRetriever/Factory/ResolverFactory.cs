namespace Estat.Nsi.StructureRetriever.Factory
{
    using System;

    using Estat.Nsi.StructureRetriever.Engines.Resolver;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The resolver factory.
    /// </summary>
    public class ResolverFactory : IResolverFactory
    {
        /// <summary>
        /// Returns the <see cref="IResolver" /> for the specified <paramref name="referenceDetailType"/>.
        /// </summary>
        /// <param name="referenceDetailType">Type of the reference detail.</param>
        /// <param name="crossReferenceManager">The cross reference manager.</param>
        /// <param name="specificStructureTypes">The specific object structure types.</param>
        /// <returns>The <see cref="IResolver" /> for the specified <paramref name="referenceDetailType"/>; otherwise null</returns>
        public IResolver GetResolver(StructureReferenceDetail referenceDetailType, IAuthCrossReferenceMutableRetrievalManager crossReferenceManager, params SdmxStructureType[] specificStructureTypes)
        {
            switch (referenceDetailType.EnumType)
            {
                case StructureReferenceDetailEnumType.Null:
                case StructureReferenceDetailEnumType.None:
                    return new NoneResolver();
                case StructureReferenceDetailEnumType.Parents:
                    return new ParentsResolver(crossReferenceManager);
                case StructureReferenceDetailEnumType.ParentsSiblings:
                    return new ParentsAndSiblingsResolver(crossReferenceManager);
                case StructureReferenceDetailEnumType.Children:
                    return new ChildrenResolver(crossReferenceManager);
                case StructureReferenceDetailEnumType.Descendants:
                    return new DescendantsResolver(crossReferenceManager);
                case StructureReferenceDetailEnumType.All:
                    return new AllResolver(crossReferenceManager);
                case StructureReferenceDetailEnumType.Specific:
                    return new SpecificResolver(crossReferenceManager, specificStructureTypes);
                default:
                    throw new ArgumentOutOfRangeException(referenceDetailType.EnumType.ToString());
            }
        }
    }
}