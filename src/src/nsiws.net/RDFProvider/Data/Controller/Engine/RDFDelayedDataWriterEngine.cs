// -----------------------------------------------------------------------
// <copyright file="DelayedDataWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-11-15
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace RDFProvider.Controller.Engine
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.Ws.Controllers.Extension;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using RDFProvider.Writer.Engine;
    using RDFProvider.Retriever.Model;


    public class RDFDelayedDataWriterEngine : IRDFDataWriterEngine
    {
        private readonly Queue<Action> _actions;

        private readonly IRDFDataWriterEngine _dataWriterEngine;

        public RDFDelayedDataWriterEngine(IRDFDataWriterEngine dataWriterEngine, Queue<Action> actions)
        {
            if (dataWriterEngine == null)
            {
                throw new ArgumentNullException("dataWriterEngine");
            }

            this._dataWriterEngine = dataWriterEngine;
            this._actions = actions ?? new Queue<Action>();
        }

        public void Close(params IFooterMessage[] footer)
        {
            this.RunQueue();
            this._dataWriterEngine.Close();
        }

        public void StartDataset(IDataflowObject dataflow, IDataStructureObject dsd, IDatasetHeader header, DataRetrievalInfoSeries info)
        {
            this._actions.Enqueue(() => this._dataWriterEngine.StartDataset(dataflow, dsd, header, info));
        }

        public void StartSeries(string values)
        {
            this.RunQueue();
            this._dataWriterEngine.StartSeries(values);
        }

        public void RDFWriterStrucInfo(string dataset, string struc)
        {
            this.RunQueue();
            this._actions.Enqueue(() => this._dataWriterEngine.RDFWriterStrucInfo(dataset, struc));
        }

        public void RDFWriteObservation(string obsConceptValue, string obsValue)
        {
            this.RunQueue();
            this._dataWriterEngine.RDFWriteObservation(obsConceptValue, obsValue);
        }

        public void WriteSeriesKeyValue(string id, string value_ren, string value_version, string value_Id)
        {
            this.RunQueue();
            this._dataWriterEngine.WriteSeriesKeyValue(id, value_ren, value_version, value_Id);
        }

        private void RunQueue()
        {
            this._actions.RunAll();
        }
    }
}