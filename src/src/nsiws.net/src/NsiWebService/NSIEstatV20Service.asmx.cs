// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NSIEstatV20Service.asmx.cs" company="Eurostat">
//   Date Created : 2011-10-11
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//  Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.0 Schema files with Eurostat extensions 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataDisseminationWS
{
    using System.ComponentModel;
    using System.Web.Services;

    using Estat.Sri.Ws.Controllers.Constants;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.0 Schema files with Eurostat extensions
    /// </summary>
    /// <example>
    /// A simple <see cref="NSIStdV20Service"/> client in C#
    /// <code source="ReUsingExamples\NsiWebService\ReUsingWebService.cs" lang="cs"/>
    /// The <code>WSDLSettings</code> class used by the client
    /// <code source="ReUsingExamples\NsiWebService\WSDLSettings.cs" lang="cs"/>
    /// </example>
    [WebService(Namespace = "http://ec.europa.eu/eurostat/sri/service/2.0/extended", 
        Description =
            "Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.0 Schema files with Eurostat extensions")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class NSIEstatV20Service : NSIStdV20Service
    {
        /// <summary>
        /// Gets the <see cref="SdmxSchema"/> for this service
        /// </summary>
        protected override WebServiceEndpoint Endpoint 
        {
            get
            {
                return WebServiceEndpoint.EstatEndpoint;
            }
        }
    }
}