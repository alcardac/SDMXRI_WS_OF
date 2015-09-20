// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedStructureRetriever.cs" company="Eurostat">
//   Date Created : 2013-09-23
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The advanced mutable structure search manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System.Configuration;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// The advanced mutable structure search manager.
    /// </summary>
    public class AdvancedStructureRetriever : IAdvancedMutableStructureSearchManager
    {
        #region Fields

        /// <summary>
        /// The AUTH advanced mutable structure search manager.
        /// </summary>
        private readonly IAuthAdvancedMutableStructureSearchManager _authAdvancedMutableStructureSearchManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedStructureRetriever"/> class. 
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        public AdvancedStructureRetriever(ConnectionStringSettings connectionStringSettings)
            : this(null, connectionStringSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedStructureRetriever"/> class. 
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        public AdvancedStructureRetriever(IStructureSearchManagerFactory<IAuthAdvancedMutableStructureSearchManager> factory, ConnectionStringSettings connectionStringSettings)
        {
            factory = factory ?? new AuthAdvancedMutableStructureSearchManagerFactory();
            this._authAdvancedMutableStructureSearchManager = factory.GetStructureSearchManager(connectionStringSettings, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get maintainables.
        /// </summary>
        /// <param name="structureQuery">
        /// The structure query.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        public IMutableObjects GetMaintainables(IComplexStructureQuery structureQuery)
        {
            return this._authAdvancedMutableStructureSearchManager.GetMaintainables(structureQuery, null);
        }

        #endregion
    }
}