// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsInfoImpl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects.Container
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// The sdmx objects info impl.
    /// </summary>
    public class SdmxObjectsInfoImpl : ISdmxObjectsInfo
    {
        #region Fields

        /// <summary>
        /// The _agency metadata.
        /// </summary>
        private IList<IAgencyMetadata> _agencyMetadata;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectsInfoImpl"/> class.
        /// </summary>
        public SdmxObjectsInfoImpl()
        {
            this._agencyMetadata = new List<IAgencyMetadata>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the agency metadata.
        /// </summary>
        public IList<IAgencyMetadata> AgencyMetadata
        {
            get
            {
                return this._agencyMetadata;
            }

            set
            {
                if (value == null)
                {
                    this._agencyMetadata = new List<IAgencyMetadata>();
                }
                else
                {
                    this._agencyMetadata = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number maintainables.
        /// </summary>
        public int NumberMaintainables
        {
            get
            {
                int i = 0;

                /* foreach */
                foreach (IAgencyMetadata currentAgencyMetadata in this.AgencyMetadata)
                {
                    i += currentAgencyMetadata.NumberMaintainables;
                }

                return i;
            }

            set
            {
                // DO NOTHING - this is here for passing to external applications
            }
        }

        #endregion
    }
}