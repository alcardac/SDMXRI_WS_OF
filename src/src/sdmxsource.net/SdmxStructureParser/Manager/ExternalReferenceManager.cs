// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalReferenceManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The external reference manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager
{
    using System;
    using System.Collections.Generic;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Log;

    /// <summary>
    ///     The external reference manager implementation.
    /// </summary>
    public class ExternalReferenceManager : IExternalReferenceManager
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ExternalReferenceManager));

        /// <summary>
        ///     The retrieval settings.
        /// </summary>
        private static readonly ResolutionSettings _retriervalSettings =
            new ResolutionSettings(ResolveExternalSetting.DoNotResolve, ResolveCrossReferences.DoNotResolve, 0);

        #endregion

        #region Fields

        /// <summary>
        ///     The structure parsing manager.
        /// </summary>
        private IStructureParsingManager _structureParsingManager;

        private IReadableDataLocationFactory _readableDataLocationFactory;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Sets the structure parsing manager.
        /// </summary>
        public IStructureParsingManager StructureParsingManager
        {
            set
            {
                this._structureParsingManager = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadableDataLocationFactory ReadableDataLocationFactory
        {
            set
            {
                this._readableDataLocationFactory = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Resolves external references, where the 'externalReference' attribute is set to 'true'.
        ///     The external reference locations are expected to be given by a URI, and the URI is expected to point to the
        ///     location of a valid SDMX document containing the referenced structure.
        ///     <p/>
        ///     External references can be of a different version to those that created the input StructureBeans.
        /// </summary>
        /// <param name="structures">
        /// containing structures which may have the external reference attribute set to `true`
        /// </param>
        /// <param name="isSubstitute">
        /// if set to true, this will substitute the external reference sdmxObjects for the real sdmxObjects
        /// </param>
        /// <param name="isLienient">
        /// The is Lenient.
        /// </param>
        /// <returns>
        /// a StructureBeans containing only the externally referenced sdmxObjects
        /// </returns>
        public virtual ISdmxObjects ResolveExternalReferences(
            ISdmxObjects structures, bool isSubstitute, bool isLienient)
        {
            _log.Debug("Check for External References, Substititue=" + isSubstitute + " Lienient=" + isLienient);
            ISdmxObjects returnBeans = new SdmxObjectsImpl();
            if (structures == null)
            {
                return returnBeans;
            }

            this.ResolveBeans(structures.GetAllMaintainables(), returnBeans, isLienient);
            _log.Info("Number of External References=" + returnBeans.GetAllMaintainables().Count);
            if (isSubstitute)
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug("substititing " + returnBeans.GetAllMaintainables().Count + " externally resolved sdmxObjects");
                }

                structures.Merge(returnBeans);
            }

            return returnBeans;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add exception.
        /// </summary>
        /// <param name="maintBean">
        /// The maintainable sdmxObject
        /// </param>
        /// <param name="isLenient">
        /// Lenient.behavior against exception
        /// </param>
        /// <exception cref="ReferenceException">
        /// External Structure Not Found A tUri
        /// </exception>
        private static void AddException(IMaintainableObject maintBean, bool isLenient)
        {
            if (isLenient)
            {
                if (maintBean.StructureUrl == null)
                {
                    AddException("External location not set");
                }
                else
                {
                    AddException(
                        "External location `" + maintBean.StructureUrl + "` does not contain structure : "
                        + maintBean.Urn);
                }
            }
            else
            {
                if (maintBean.StructureUrl == null)
                {
                    throw new SdmxSemmanticException(ExceptionCode.ExternalStructureNotFoundAtUri, maintBean.Urn, "NOT SET");
                }

                throw new SdmxSemmanticException(
                    ExceptionCode.ExternalStructureNotFoundAtUri, maintBean.Urn, maintBean.StructureUrl);
            }
        }

        /// <summary>
        /// Add exception message
        /// </summary>
        /// <param name="message">
        /// The exception message.
        /// </param>
        private static void AddException(string message)
        {
            LoggingUtil.Error(_log, message);
        }

        /// <summary>
        /// Resolve SDMX objects.
        /// </summary>
        /// <param name="maintBean">
        /// The maintainable SDMX objects.
        /// </param>
        /// <param name="isLenient">
        /// Set to true for lenient behavior towards exceptions
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        private ISdmxObjects Resolve(IMaintainableObject maintBean, bool isLenient)
        {
            try
            {
                Uri urlLocation = maintBean.StructureUrl;
                if (urlLocation == null)
                {
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug("can not resolve reference, StructureURL not set ");
                    }

                    AddException(maintBean, isLenient);
                }
                else
                {
                    IReadableDataLocation dataLocation = _readableDataLocationFactory.GetReadableDataLocation(urlLocation);
                    IStructureWorkspace sw = this._structureParsingManager.ParseStructures(
                        dataLocation, _retriervalSettings, null);
                    return sw.GetStructureObjects(false);
                }
            }
            catch (CrossReferenceException refEx)
            {
                if (isLenient)
                {
                    AddException(refEx.Message);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception th)
            {
                if (isLenient)
                {
                    AddException(th.Message);
                }
                else
                {
                    throw;
                }
            }

            return null;
        }

        /// <summary>
        /// Resolve SDMX objects.
        /// </summary>
        /// <param name="maintainableObjects">
        /// The maintainable SDMX objects.
        /// </param>
        /// <param name="returnBeans">
        /// The return sdmxObjects.
        /// </param>
        /// <param name="isLenient">
        /// Set to true for lenient behavior towards exceptions
        /// </param>
        /// <typeparam name="T0">
        /// The type of <paramref name="maintainableObjects"/>
        /// </typeparam>
        private void ResolveBeans<T0>(IEnumerable<T0> maintainableObjects, ISdmxObjects returnBeans, bool isLenient)
            where T0 : IMaintainableObject
        {
            /* foreach */
            foreach (T0 currentBean in maintainableObjects)
            {
                if (currentBean.IsExternalReference.IsTrue)
                {
                    ISdmxObjects retrievedStructures = this.Resolve(currentBean, isLenient);

                    // CHECK THERE WAS SOMETHING RETURNED
                    if (retrievedStructures != null)
                    {
                        IMaintainableObject resolvedReference =
                            MaintainableUtil<T0>.ResolveReference(retrievedStructures.Codelists, currentBean.AsReference.MaintainableReference);
                        if (resolvedReference == null)
                        {
                            if (_log.IsDebugEnabled)
                            {
                                _log.Debug("can not resolve reference, not found in returned sdmxObjects ");
                            }

                            AddException(currentBean, isLenient);
                        }
                        else
                        {
                            // GET THE CODELIST AND ADD IT
                            returnBeans.AddIdentifiable(resolvedReference);
                        }
                    }
                    else
                    {
                        if (_log.IsDebugEnabled)
                        {
                            _log.Debug("can not resolve reference, not sdmxObjects returnd from URL ");
                        }
                    }
                }
            }
        }

        #endregion
    }
}