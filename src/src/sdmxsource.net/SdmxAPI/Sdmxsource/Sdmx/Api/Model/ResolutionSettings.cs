// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolutionSettings.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    #region Using directives

    using System;

    #endregion

    /// <summary>
    ///     Contains all the settings for parsing structures.
    ///     <p />
    ///     The setting are given by enumerated flags at construction, making instances immutable.
    ///     <p />
    ///     Functions are available to reinterpret these settings when applied to parsing XML structures into Objects.
    ///     <p />
    ///     The StructureSettings take into account two settings for parsing SDMX-ML documents
    ///     <ol>
    ///         <li>Resolve Cross References</li>
    ///         <li>Resolve External References</li>
    ///     </ol>
    ///     Cross References are declared by structures such as Key Families also known as a Data Structure Definition (DSD) (see <c>DataStructureObject</c>).  A DSD
    ///     contains a number of Dimensions (<c>IDimension</c>) a Dimensions may declare a reference to a Codelist and a Concept through reference parameters.
    ///     <p />
    ///     If a system is to resolve the cross references, then it can do this by either of the following ways:
    ///     <ul>
    ///         <li>Resolve the external reference exists at the given URI</li>
    ///         <li>Resolve the external reference exists, and substitute it for the provided 'stub' artifact</li>
    ///         <li>Either of the above two can be lenient (do not error if it can not be done) or not (throw exception if this could not be done)</li>
    ///     </ul>
    ///     <p />
    ///     External References are declared by MaintainableObject structures by having their isExternal attribute set to true.  In this instance the Maintainable artifact
    ///     is declaring that it is not the full artifact, but just an empty container whose purpose is to declare the existence of the artifact (stub).  The stub also has
    ///     the responsibility of declaring where to get the full artifact, this is defined by the URI attribute.
    ///     <p />
    ///     The StructureSettings provides the following options for resolving cross references:
    ///     <ul>
    ///         <li>Do  not resolve</li>
    ///         <li>Resolve all the references (it is up to the implementation to decide how deep to resolve)</li>
    ///         <li>Resolve all the references except for Agencies</li>
    ///     </ul>
    ///     <p />
    ///     External references are when a structure is declared, like a codelist, as a 'stub'
    ///     containing the minimum possible information for it to be valid against the schema;
    ///     i.e it has a name, id and agency id, but it also has a URI which says:
    ///     if you want the full codelists, with a 100 million codes, it's retrievable from this location.
    ///     This is useful if someone wants to store information in the registry, but maintain the contents of the artifacts externally.
    ///     <p />
    /// </summary>
    /// <seealso cref="null" />
    /// which is built from an XML message.
    /// <seealso cref="null" />
    /// which is built from resolving all the cross referenced artifacts.
    public class ResolutionSettings : ICloneable
    {
        #region Fields

        /// <summary>
        ///     The _resolution depth.
        /// </summary>
        private readonly int _resolutionDepth;

        /// <summary>
        ///     The _resolve cross ref.
        /// </summary>
        private readonly ResolveCrossReferences _resolveCrossRef;

        /// <summary>
        ///     The _resolve external.
        /// </summary>
        private readonly ResolveExternalSetting _resolveExternal;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionSettings"/> class.
        ///     Constructor, all settings provided here, immutable hereafter.
        /// </summary>
        /// <param name="resolveExternal">The Resolve External Setting enum
        /// </param>
        /// <param name="resolveCrossRef">The Resolve Cross References enum
        /// </param>
        /// <param name="resolutionDepth">
        /// - required if resolving references. 0 indicates resolve all, any other positive integer is the level of recursion to resolve
        /// </param>
        public ResolutionSettings(ResolveExternalSetting resolveExternal, ResolveCrossReferences resolveCrossRef, int resolutionDepth)
        {
            this._resolveExternal = resolveExternal;
            this._resolveCrossRef = resolveCrossRef;
            this._resolutionDepth = resolutionDepth;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionSettings"/> class.
        ///     Constructor, all settings provided here, immutable hereafter.
        /// </summary>
        /// <param name="resolveExternal">The Resolve External Setting enum
        /// </param>
        /// <param name="resolveCrossRef">The Resolve Cross References enum
        /// </param>
        public ResolutionSettings(ResolveExternalSetting resolveExternal, ResolveCrossReferences resolveCrossRef)
        {
            this._resolveExternal = resolveExternal;
            this._resolveCrossRef = resolveCrossRef;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the the cross referenced structures need to be resolved to exist
        /// </summary>
        /// <value> </value>
        public bool Lenient
        {
            get
            {
                return this._resolveExternal == ResolveExternalSetting.ResolveLenient
                       || this._resolveExternal == ResolveExternalSetting.ResolveSubstituteLenient;
            }
        }

        /// <summary>
        ///     Gets the resolution depth.
        /// </summary>
        public int ResolutionDepth
        {
            get
            {
                return this._resolutionDepth;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the the cross referenced structures need to be resolved to exist
        /// </summary>
        /// <value> </value>
        public bool ResolveAgencyReferences
        {
            get
            {
                return this._resolveCrossRef == Model.ResolveCrossReferences.ResolveAll;
            }
        }

        /// <summary>
        ///     Gets the resolve cross ref.
        /// </summary>
        public ResolveCrossReferences ResolveCrossRef
        {
            get
            {
                return this._resolveCrossRef;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the the cross referenced structures need to be resolved to exist
        /// </summary>
        /// <value> </value>
        public bool ResolveCrossReferences
        {
            get
            {
                return this._resolveCrossRef != Model.ResolveCrossReferences.DoNotResolve;
            }
        }

        /// <summary>
        ///     Gets the resolve external.
        /// </summary>
        public ResolveExternalSetting ResolveExternal
        {
            get
            {
                return this._resolveExternal;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the external references are to be retrieved and replace the stub
        /// </summary>
        /// <value> </value>
        public bool ResolveExternalReferences
        {
            get
            {
                return this._resolveExternal != ResolveExternalSetting.DoNotResolve;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the the resolved external references should be substituted for the maintainable stub
        /// </summary>
        /// <value> </value>
        public bool SubstituteExternal
        {
            get
            {
                return this._resolveExternal == ResolveExternalSetting.ResolveSubstitute
                       || this._resolveExternal == ResolveExternalSetting.ResolveSubstituteLenient;
            }
        }

        #endregion

        // public virtual ResolutionSettings Clone() {
        #region Public Methods and Operators

        /// <summary>
        ///     The clone.
        /// </summary>
        /// <returns>
        ///     The <see cref="object" /> .
        /// </returns>
        public object Clone()
        {
            return new ResolutionSettings(this._resolveExternal, this._resolveCrossRef, this._resolutionDepth);
        }

        public override string ToString()
        {
            return "Resolve external=" + _resolveExternal + ", resolve cross references=" + _resolveCrossRef + ", resolution depth=" + _resolutionDepth;
        }
        #endregion
    }
}