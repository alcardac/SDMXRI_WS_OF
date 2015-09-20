// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureRequestRestController.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The structure request rest controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System.Linq;

    using Estat.Nsi.AuthModule;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    /// <summary>
    /// The structure request rest controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the output writer
    /// </typeparam>
    public class StructureRequestRestController<TWriter> : QueryStructureController<TWriter>, IController<IRestStructureQuery, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The AUTH structure search manager
        /// </summary>
        private readonly IAuthMutableStructureSearchManager _authStructureSearchManager;

        /// <summary>
        ///     The structure search manager
        /// </summary>
        private readonly IMutableStructureSearchManager _structureSearchManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureRequestRestController{TWriter}"/> class.
        /// </summary>
        /// <param name="responseGenerator">
        /// The response generator.
        /// </param>
        /// <param name="structureSearchManager">
        /// The structure Search Manager.
        /// </param>
        /// <param name="authStructureSearchManager">
        /// The AUTH Structure Search Manager.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        public StructureRequestRestController(
            IResponseGenerator<TWriter, ISdmxObjects> responseGenerator, 
            IMutableStructureSearchManager structureSearchManager, 
            IAuthMutableStructureSearchManager authStructureSearchManager, 
            DataflowPrincipal dataflowPrincipal)
            : base(responseGenerator, dataflowPrincipal)
        {
            this._structureSearchManager = structureSearchManager;
            this._authStructureSearchManager = authStructureSearchManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(IRestStructureQuery input)
        {
            return this.ParseRequestPrivate(principal => this.GetMutableObjectsRest(input, principal));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the mutable objects from rest.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="dataflowPrincipal">
        /// The dataflow principal.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        private IMutableObjects GetMutableObjectsRest(IRestStructureQuery input, DataflowPrincipal dataflowPrincipal)
        {
            IMutableObjects mutableObjects = dataflowPrincipal != null
                                                 ? this._authStructureSearchManager.GetMaintainables(input, dataflowPrincipal.AllowedDataflows.ToList())
                                                 : this._structureSearchManager.GetMaintainables(input);

            return mutableObjects;
        }

        #endregion
    }
}