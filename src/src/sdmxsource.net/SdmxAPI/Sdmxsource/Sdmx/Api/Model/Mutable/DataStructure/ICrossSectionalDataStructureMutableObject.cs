// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossSectionalDataStructureMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     The CrossSectionalDataStructureMutableObject interface.
    /// </summary>
    public interface ICrossSectionalDataStructureMutableObject : IDataStructureMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a map of attribute id, mapping to the cross sectional measure ids it is attached to
        /// </summary>
        /// <value> </value>
        IDictionaryOfLists<string, string> AttributeToMeasureMap { get; }

        /// <summary>
        ///     Gets the cross sectional attach data set.
        /// </summary>
        IList<string> CrossSectionalAttachDataSet { get; }

        /// <summary>
        ///     Gets the cross sectional attach group.
        /// </summary>
        IList<string> CrossSectionalAttachGroup { get; }

        /// <summary>
        ///     Gets the cross sectional attach observation.
        /// </summary>
        IList<string> CrossSectionalAttachObservation { get; }

        /// <summary>
        ///     Gets the cross sectional attach section.
        /// </summary>
        IList<string> CrossSectionalAttachSection { get; }

        /// <summary>
        ///     Gets the cross sectional measures.
        /// </summary>
        IList<ICrossSectionalMeasureMutableObject> CrossSectionalMeasures { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICrossSectionalDataStructureObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets the measure dimension codelist mapping.
        /// </summary>
        IDictionary<string, IStructureReference> MeasureDimensionCodelistMapping { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add cross sectional attach data set.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference.
        /// </param>
        void AddCrossSectionalAttachDataSet(string dimensionReference);

        /// <summary>
        /// The add cross sectional attach group.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference.
        /// </param>
        void AddCrossSectionalAttachGroup(string dimensionReference);

        /// <summary>
        /// The add cross sectional attach observation.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference.
        /// </param>
        void AddCrossSectionalAttachObservation(string dimensionReference);

        /// <summary>
        /// The add cross sectional attach section.
        /// </summary>
        /// <param name="dimensionReference">
        /// The dimension reference.
        /// </param>
        void AddCrossSectionalAttachSection(string dimensionReference);

        /// <summary>
        /// The add cross sectional measures.
        /// </summary>
        /// <param name="crossSectionalMeasure">
        /// The cross sectional measure.
        /// </param>
        void AddCrossSectionalMeasures(ICrossSectionalMeasureMutableObject crossSectionalMeasure);

        #endregion
    }
}