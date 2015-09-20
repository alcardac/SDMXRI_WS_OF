// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowPrincipal.cs" company="Eurostat">
//   Date Created : 2011-05-20
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   An implementation of the <see cref="IPrincipal" /> interface that stores that allowed dataflows
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Text.RegularExpressions;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// An implementation of the <see cref="IPrincipal"/> interface that stores that allowed dataflows
    /// </summary>
    public class DataflowPrincipal : IPrincipal
    {
        #region Constants and Fields

        /// <summary>
        /// Regular expression for matching the convention AGENCY+ID+VERSION used in dataflow in SDMX Queries
        /// </summary>
        private static readonly Regex _dataflowRegex =
            new Regex("(?<agency>[^\\+]+)\\+(?<id>[^\\+]+)\\+(?<version>[^\\+]+)");

        /// <summary>
        /// This field holds the <see cref="IAuthorizationProvider"/> of this principal
        /// </summary>
        private readonly IAuthorizationProvider _authorizationProvider;

        /// <summary>
        /// This field holds the <see cref="IIdentity"/> of this principal
        /// </summary>
        private readonly IIdentity _identity;

        /// <summary>
        /// The <see cref="IUser"/> of this principal
        /// </summary>
        private readonly IUser _user;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowPrincipal"/> class. 
        /// Initialize a new instance of the <see cref="DataflowPrincipal"/> class with the specified <paramref name="identity"/> and <paramref name="authorizationProvider"/>
        /// </summary>
        /// <param name="identity">
        /// The <see cref="IIdentity"/> of this principal
        /// </param>
        /// <param name="authorizationProvider">
        /// The <see cref="IAuthorizationProvider"/> of this principal
        /// </param>
        /// <param name="user">
        /// The <see cref="IUser"/> for this principal
        /// </param>
        public DataflowPrincipal(IIdentity identity, IAuthorizationProvider authorizationProvider, IUser user)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            if (authorizationProvider == null)
            {
                throw new ArgumentNullException("authorizationProvider");
            }

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this._user = user;
            this._identity = identity;
            this._authorizationProvider = authorizationProvider;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the collection of allowed dataflows
        /// </summary>
        /// <returns>The list of allowed dataflows</returns>
        public ICollection<IMaintainableRefObject> AllowedDataflows
        {
            get
            {
                return this._authorizationProvider.GetDataflows(this._user);
            }
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        public IIdentity Identity
        {
            get
            {
                return this._identity;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the latest allowed dataflow with version and agency (if are either defined)
        /// </summary>
        /// <param name="dataflowId">
        /// The dataflow id
        /// </param>
        /// <returns>
        /// The allowed dataflow with biggest version
        /// </returns>
        public IMaintainableRefObject GetAllowedDataflow(string dataflowId)
        {
            IMaintainableRefObject actualDataflow = null;

            Match conventionUsed = _dataflowRegex.Match(dataflowId);
            if (conventionUsed.Success)
            {
                actualDataflow = new MaintainableRefObjectImpl
                    {
                        AgencyId = conventionUsed.Groups["agency"].Value, 
                        MaintainableId = conventionUsed.Groups["id"].Value, 
                        Version = conventionUsed.Groups["version"].Value
                    };
                if (!this._authorizationProvider.AccessControl(this._user, actualDataflow))
                {
                    actualDataflow = null;
                }
            }
            else
            {
                IEnumerable<IMaintainableRefObject> dataflowRefBeans = this._authorizationProvider.GetDataflows(
                    this._user, dataflowId);
                foreach (IMaintainableRefObject dataflowRefBean in dataflowRefBeans)
                {
                    if (actualDataflow == null
                        || string.CompareOrdinal(actualDataflow.Version, dataflowRefBean.Version) < 0)
                    {
                        actualDataflow = dataflowRefBean;
                    }
                }
            }

            return actualDataflow;
        }

        /// <summary>
        /// Get the latest allowed dataflow with version and agency (if are either defined)
        /// </summary>
        /// <param name="dataflow">
        /// The dataflow object
        /// </param>
        /// <returns>
        /// The allowed dataflow with biggest version
        /// </returns>
        public IMaintainableRefObject GetAllowedDataflow(IDataflowObject dataflow)
        {
            if (dataflow == null)
            {
                throw new ArgumentNullException("dataflow");
            }

            IMaintainableRefObject actualDataflow = null;

            var dataflowRef = dataflow.AsReference.MaintainableReference;
            if (this._authorizationProvider.AccessControl(this._user, dataflowRef))
            {
                actualDataflow = dataflowRef;
            }

            return actualDataflow;
        }

        /// <summary>
        /// Get the latest allowed dataflow with version and agency (if are either defined)
        /// </summary>
        /// <param name="dataflowRef">
        /// The dataflow reference
        /// </param>
        /// <returns>
        /// The allowed dataflow with biggest version
        /// </returns>
        public IMaintainableRefObject GetAllowedDataflow(IMaintainableRefObject dataflowRef)
        {
            if (dataflowRef == null)
            {
                throw new ArgumentNullException("dataflowRef");
            }

            IMaintainableRefObject actualDataflow = null;

            if (dataflowRef.HasAgencyId() && dataflowRef.HasVersion())
            {
                if (this._authorizationProvider.AccessControl(this._user, dataflowRef))
                {
                    actualDataflow = dataflowRef;
                }
            }
            else
            {
                IEnumerable<IMaintainableRefObject> dataflowRefBeans = this._authorizationProvider.GetDataflows(
                    this._user, dataflowRef.MaintainableId);
                foreach (IMaintainableRefObject dataflowRefBean in dataflowRefBeans)
                {
                    // TODO fix version comparison
                    if (actualDataflow == null
                        || string.CompareOrdinal(actualDataflow.Version, dataflowRefBean.Version) < 0)
                    {
                        actualDataflow = dataflowRefBean;
                    }
                }
            }

            return actualDataflow;
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        /// <param name="role">
        /// The name of the role for which to check membership. 
        /// </param>
        public bool IsInRole(string role)
        {
            Match conventionUsed = _dataflowRegex.Match(role);
            if (conventionUsed.Success)
            {
                return this._authorizationProvider.AccessControl(
                    this._user,
                    new MaintainableRefObjectImpl
                    {
                        AgencyId = conventionUsed.Groups ["agency"].Value,
                        MaintainableId = conventionUsed.Groups ["id"].Value,
                        Version = conventionUsed.Groups ["version"].Value
                    });
            }

            return this._authorizationProvider.AccessControl(this._user, role);
        }

        #endregion
    }
}