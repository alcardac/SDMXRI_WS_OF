// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossReferenceResolverEngine.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The purpose of the ICrossReferenceResolverEngine is to resolve cross references for beans, either MaintainableBeans,
//   beans within a <see cref="ISdmxObjects" /> container, ProvisionBeans, and RegistrationBeans.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Engine
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///     The purpose of the ICrossReferenceResolverEngine is to resolve cross references for beans, either MaintainableBeans,
    ///     beans within a <see cref="ISdmxObjects" /> container, ProvisionBeans, and RegistrationBeans.
    /// </summary>
    public interface ICrossReferenceResolverEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// For the included <paramref name="sdmxObjects"/>, returns a map of agency URN to maintainable Bean that references the agency
        /// </summary>
        /// <param name="sdmxObjects">
        /// The included <c>SDMX</c> objects
        /// </param>
        /// <param name="identifiableRetrievalManager">
        /// The <see cref="IIdentifiableRetrievalManager"/>
        /// </param>
        /// <returns>
        /// The included <paramref name="sdmxObjects"/>, returns a map of agency URN to maintainable Bean that references the agency
        /// </returns>
        IDictionary<string, ISet<IMaintainableObject>> GetMissingAgencies(
            ISdmxObjects sdmxObjects, IIdentifiableRetrievalManager identifiableRetrievalManager);

        /// <summary>
        /// Gets a Dictionary of <see cref="IIdentifiableObject"/> alongside any cross references they declare that could not be found in the set of
        ///     <paramref name="beans"/>
        ///     provided, and the <paramref name="retrievalManager"/> (if given).
        ///     <p/>
        ///     <b>NOTE :</b>An empty Map is returned if all cross references are present.
        /// </summary>
        /// <param name="beans">
        /// - the objects to return the Map of missing references for
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing  until it has found every directly and indirectly referenced
        /// </param>
        /// <param name="identifiableRetrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <returns>
        /// Map of IIdentifiableObject with a Set of CrossReferences that could not be resolved for the IIdentifiableObject - an empty Map is returned if all cross references are present
        /// </returns>
        IDictionary<IIdentifiableObject, ISet<ICrossReference>> GetMissingCrossReferences(
            ISdmxObjects beans, int numberLevelsDeep, IIdentifiableRetrievalManager identifiableRetrievalManager);

        /// <summary>
        /// Resolves a reference from <paramref name="crossReference"/>
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference instance
        /// </param>
        /// <param name="identifiableRetrievalManager">
        /// The identifiable Retrieval Manager.
        /// </param>
        /// <returns>
        /// a reference from <paramref name="crossReference"/>
        /// </returns>
        IIdentifiableObject ResolveCrossReference(
            ICrossReference crossReference, IIdentifiableRetrievalManager identifiableRetrievalManager);

        /// <summary>
        /// Resolves all references and returns a Map containing all the input beans and the objects that are cross referenced,
        ///     the Map's key set contains the Identifiable that is the referencing object and the Map's value collection contains the referenced artifacts.
        /// </summary>
        /// <param name="beans">
        /// - the <see cref="ISdmxObjects"/> container, containing all the beans to check references for
        /// </param>
        /// <param name="resolveAgencies">
        /// - if true the resolver engine will also attempt to resolve referenced agencies
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this.
        /// </param>
        /// <param name="identifiableRetrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <returns>
        /// Map of referencing versus  references
        /// </returns>
        /// <exception cref="CrossReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> ResolveReferences(
            ISdmxObjects beans, bool resolveAgencies, int numberLevelsDeep, IIdentifiableRetrievalManager identifiableRetrievalManager);

        /// <summary>
        /// Returns a set of IdentifiableBeans that the IMaintainableObject cross references
        /// </summary>
        /// <param name="bean">
        /// The bean.
        /// </param>
        /// <param name="resolveAgencies">
        /// - if true will also resolve the agencies
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this.
        /// </param>
        /// <param name="identifiableRetrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <exception cref="CrossReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        /// <returns>
        /// a set of IdentifiableBeans that the IMaintainableObject cross references
        /// </returns>
        ISet<IIdentifiableObject> ResolveReferences(
            IMaintainableObject bean,
            bool resolveAgencies,
            int numberLevelsDeep,
            IIdentifiableRetrievalManager identifiableRetrievalManager);

        /// <summary>
        /// Returns a set of IdentifiableBeans that are directly referenced from this registration
        /// </summary>
        /// <param name="registation">
        /// - the registration to resolve the references for
        /// </param>
        /// <param name="provRetrievalManager">
        /// - Used to resolve the provision references. Can be null if registration is not linked to a provision
        /// </param>
        /// <returns>
        /// a set of IdentifiableBeans that are directly referenced from this registration
        /// </returns>
        ISet<IIdentifiableObject> ResolveReferences(
            IRegistrationObject registation,
            IProvisionRetrievalManager provRetrievalManager);

        /// <summary>
        /// Returns a set of structures that are directly referenced from this provision
        /// </summary>
        /// <param name="provision">
        /// - the provision to resolve the references for
        /// </param>
        /// <param name="identifiableRetrievalManager">
        /// - must not be null as this will be used to resolve the references
        /// </param>
        /// <returns>
        /// a set of structures that are directly referenced from this provision
        /// </returns>
        ISet<IIdentifiableObject> ResolveReferences(
            IProvisionAgreementObject provision, IIdentifiableRetrievalManager identifiableRetrievalManager);

        #endregion
    }
}