// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureResource.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IStructureResource
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get structure.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="resourceId">
        /// The resource id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/{agencyId}/{resourceId}/{version}/")]
        Message GetStructure(string structure, string agencyId, string resourceId, string version);

        /// <summary>
        /// The get structure all.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/")]
        Message GetStructureAll(string structure);

        /// <summary>
        /// The get structure all ids latest.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/{agencyId}/")]
        Message GetStructureAllIdsLatest(string structure, string agencyId);

        /// <summary>
        /// The get structure latest.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="resourceId">
        /// The resource id.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{structure}/{agencyId}/{resourceId}/")]
        Message GetStructureLatest(string structure, string agencyId, string resourceId);


        
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "json/{structure}/{agencyId}/{resourceId}/{version}/")]
        Message GetJsonStructure(string structure, string agencyId, string resourceId, string version);


        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rdf/{structure}/{agencyId}/{resourceId}/{version}/")]
        Message GetRDFStructure(string structure, string agencyId, string resourceId, string version);
        
        #endregion
    }
}