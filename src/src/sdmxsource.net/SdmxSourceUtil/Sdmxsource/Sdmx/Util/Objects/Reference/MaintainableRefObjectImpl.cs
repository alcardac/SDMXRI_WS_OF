// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableRefObjectImpl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects.Reference
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The maintainable ref object impl.
    /// </summary>
    [Serializable]
    public class MaintainableRefObjectImpl : IMaintainableRefObject
    {
        #region Fields

        /// <summary>
        /// The _agency id.
        /// </summary>
        private string _agencyId;

        /// <summary>
        /// The _id.
        /// </summary>
        private string _id;

        /// <summary>
        /// The _version.
        /// </summary>
        private string _version;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableRefObjectImpl"/> class.
        /// </summary>
        public MaintainableRefObjectImpl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableRefObjectImpl"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version1.
        /// </param>
        public MaintainableRefObjectImpl(string agencyId, string id, string version)
        {
            // DO NOT SET EMPTY STRINGS
            if (!string.IsNullOrWhiteSpace(agencyId))
            {
                this._agencyId = agencyId;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                this._id = id;
            }

            if (!string.IsNullOrWhiteSpace(version))
            {
                this._version = version;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the agency id.
        /// </summary>
        public virtual string AgencyId
        {
            get
            {
                return this._agencyId;
            }

            set
            {
                this._agencyId = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        /// <summary>
        /// Gets or sets the maintainable id.
        /// </summary>
        public virtual string MaintainableId
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        /// <summary>
        /// Gets or sets the version1.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return this._version;
            }

            set
            {
                this._version = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified <see cref="IMaintainableRefObject"/> is equal to the current <see cref="MaintainableRefObjectImpl"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="IMaintainableRefObject"/> is equal to the current <see cref="MaintainableRefObjectImpl"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="IMaintainableRefObject"/> to compare with the current <see cref="MaintainableRefObjectImpl"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var that = obj as IMaintainableRefObject;
            if (that == null)
            {
                return false;
            }

            return string.Equals(this._agencyId, that.AgencyId)
                   && string.Equals(this._id, that.MaintainableId)
                   && string.Equals(this._version, that.Version);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="MaintainableRefObjectImpl"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// The has agency id.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool HasAgencyId()
        {
            return !string.IsNullOrWhiteSpace(this._agencyId);
        }

        /// <summary>
        /// The has maintainable id.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool HasMaintainableId()
        {
            return !string.IsNullOrWhiteSpace(this._id);
        }

        /// <summary>
        /// Has version.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool HasVersion()
        {
            return !string.IsNullOrWhiteSpace(this._version);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="MaintainableRefObjectImpl"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="MaintainableRefObjectImpl"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            string returnString = "";
            string concat = "";
            if (_agencyId != null)
            {
                returnString += "agency: " + _agencyId;
                concat = ", ";
            }
            if (_id != null)
            {
                returnString += concat + "id: " + _id;
                concat = ", ";
            }
            if (_version != null)
                returnString += concat + "version: " + _version;

            if (returnString.Length > 0)
                return returnString;

            return "Empty Reference (no parameters defined)";
        }

        #endregion
    }
}