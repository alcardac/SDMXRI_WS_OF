// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestStructureQueryManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The REST structure query manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Rest
{
    using System.IO;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Rest;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    public class RestStructureQueryManager : IRestStructureQueryManager
    {
        #region Static Fields

        /// <summary>
        ///     The structure writing manager.
        /// </summary>
        private readonly IStructureWriterManager _structureWritingManager;// = new StructureWritingManager();

        /// <summary>
        ///     The sdmx objects retrieval manager.
        /// </summary>
        private ISdmxObjectRetrievalManager _beanRetrievalManager;

        #endregion

        #region Public Properties

        //<summary>
        //    sets the sdmxobjects retrieval manager
        //</summary>
        public ISdmxObjectRetrievalManager BeanRetrievalManager
        {
            set
            {
                _beanRetrievalManager = value;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RestStructureQueryManager"/> class.
        /// </summary>
        /// <param name="structureWritingManager">
        /// The structure writer manager.
        /// </param>
        /// /// <param name="structureSearchManager">
        /// The structure search manager.
        /// </param>
        public RestStructureQueryManager(IStructureWriterManager structureWritingManager, ISdmxObjectRetrievalManager beanRetrievalManager)
        {
            _structureWritingManager = structureWritingManager;
            _beanRetrievalManager = beanRetrievalManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the structures into an output stream
        /// </summary>
        /// <param name="query">
        /// The rest structures query
        /// </param>
        /// <param name="outputStream">
        /// The output stream
        /// </param>
        /// <param name="outputFormat">
        /// The output fromat
        /// </param>
        public void GetStructures(IRestStructureQuery query, Stream outputStream, IStructureFormat outputFormat)
        {
            ISdmxObjects beans = _beanRetrievalManager.GetMaintainables(query);
            _structureWritingManager.WriteStructures(beans, outputFormat, outputStream);
        }

        #endregion
    }
}
