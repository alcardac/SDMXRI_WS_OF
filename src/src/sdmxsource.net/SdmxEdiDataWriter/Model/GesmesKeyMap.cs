// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesKeyMap.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Holds the position of a dimension in an ARR based on the order they appear in the DSD.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Remoting.Messaging;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///     Holds the position of a dimension in an ARR based on the order they appear in the DSD.
    /// </summary>
    internal class GesmesKeyMap : Dictionary<string, int>
    {
        #region Fields

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(GesmesKeyMap));

        /// <summary>
        ///     The DSD
        /// </summary>
        private readonly IDataStructureObject _keyFamily;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesKeyMap"/> class.
        /// </summary>
        /// <param name="keyFamily">
        /// The key family.
        /// </param>
        public GesmesKeyMap(IDataStructureObject keyFamily)
            : base(StringComparer.Ordinal)
        {
            this._keyFamily = keyFamily;
            var dimensions = new List<IDimension>(this._keyFamily.DimensionList.Dimensions);
            dimensions.Sort((x, y) =>
                {
                    if (Equals(x, y))
                    {
                        return 0;
                    }

                    if (x.TimeDimension == y.TimeDimension && x.FrequencyDimension == y.FrequencyDimension)
                    {
                        return x.CompareTo(y);
                    }

                    // time dimension is always last
                    if (x.TimeDimension)
                    {
                        if (y.TimeDimension)
                        {
                            var errorMessage = string.Format(CultureInfo.InvariantCulture, "Two TimeDimensions {0} {1}", x, y);
                            _log.Error(errorMessage);
                            throw new SdmxSemmanticException(errorMessage);
                        }

                        return 1;
                    }

                    if (y.TimeDimension)
                    {
                        return -1;
                    }

                    // freq dimension is always first
                    if (x.FrequencyDimension)
                    {
                        if (y.FrequencyDimension)
                        {
                            var errorMessage = string.Format(CultureInfo.InvariantCulture, "Two Frequencies {0} {1}", x, y);
                            _log.Error(errorMessage);
                            throw new SdmxSemmanticException(errorMessage);
                        }

                        return -1;
                    }

                    if (y.FrequencyDimension)
                    {
                        return 1;
                    }

                    return x.CompareTo(y);
                });
            for (int i = 0; i < dimensions.Count; i++)
            {
                IDimension dimension = dimensions[i];
                string conceptId = ConceptRefUtil.GetConceptId(dimension.ConceptRef);
                if (!dimension.TimeDimension)
                {
                    this.Add(conceptId, i);
                }
            }
        }

        #endregion
    }
}