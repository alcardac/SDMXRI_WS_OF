// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureResponseGenerator.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The structure response generator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Builder;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using RDFProvider.Structure.Manager.Output;
    using RDFProvider.Structure.Controllers.Builder;





    public class StructureResponseGenerator : IResponseGenerator<XmlWriter, ISdmxObjects>
    {
        #region Fields        

        private readonly IWriterBuilder<IStructureWriterManager, XmlWriter> _structureManagerBuilder;

        private readonly IRDFWriterBuilder<IStructureRDFWriterManager, XmlWriter> _structureRDFManagerBuilder;

        #endregion

        #region Constructors and Destructors

        public StructureResponseGenerator(IRDFWriterBuilder<IStructureRDFWriterManager, XmlWriter> structureRDFManagerBuilder)
        {
            this._structureRDFManagerBuilder = structureRDFManagerBuilder;            
        }
        #endregion

        #region Public Methods and Operators

        public Action<XmlWriter, Queue<Action>> GenerateResponseFunction(ISdmxObjects query)
        {
            return (writer, actions) => this.StreamTo(query, writer, actions);
        }

        #endregion

        #region Methods

        private void StreamTo(ISdmxObjects query, XmlWriter writer, Queue<Action> actions)
        {
                IStructureRDFWriterManager structureWritingManager = this._structureRDFManagerBuilder.BuildRDF(writer, actions);
                structureWritingManager.RDFWriteStructures(query, null);
        }

        #endregion
    }
}
