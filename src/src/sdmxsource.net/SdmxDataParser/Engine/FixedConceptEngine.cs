// -----------------------------------------------------------------------
// <copyright file="FixedConceptEngine.cs" company="Eurostat">
//   Date Created : 2014-07-16
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;

    /// <summary>
    /// The fixed concept engine.
    /// </summary>
    public class FixedConceptEngine : IFixedConceptEngine
    {
        /// <summary>
        /// Gets the fixed concepts.
        /// </summary>
        /// <param name="dre">The data reader engine</param>
        /// <param name="includeObs">The include observation</param>
        /// <param name="includeAttributes">The include attributes</param>
        /// <returns>
        /// The list of key values
        /// </returns>
        public IList<IKeyValue> GetFixedConcepts(IDataReaderEngine dre, bool includeObs, bool includeAttributes)
        {
            dre.Reset();
            var conceptMap = new Dictionary<string, string>(StringComparer.Ordinal);
            var skipConcepts = new HashSet<string>(StringComparer.Ordinal);

            while (dre.MoveNextKeyable())
            {
                var key = dre.CurrentKey;
                if (includeAttributes)
                {
                    ProcessKeyValues(key.Attributes, conceptMap, skipConcepts);
                }

                ProcessKeyValues(key.Key, conceptMap, skipConcepts);
                if (includeObs)
                {
                    while (dre.MoveNextObservation())
                    {
                        var obs = dre.CurrentObservation;
                        if (includeAttributes)
                        {
                            ProcessKeyValues(obs.Attributes, conceptMap, skipConcepts);
                        }

                        if (obs.CrossSection)
                        {
                            ProcessKeyValue(obs.CrossSectionalValue, conceptMap, skipConcepts);
                        }
                    }
                }
            }

            var fixedKeyValues = new List<IKeyValue>(conceptMap.Select(pair => new KeyValueImpl(pair.Value, pair.Key)));
            return fixedKeyValues;
        }

        /// <summary>
        /// Processes the key value.
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <param name="conceptMap">The concept map.</param>
        /// <param name="skipConcepts">The skip concepts.</param>
        private static void ProcessKeyValue(IKeyValue keyValue, IDictionary<string, string> conceptMap, ISet<string> skipConcepts)
        {
            string currentConcept = keyValue.Concept;
            if (skipConcepts.Contains(currentConcept))
            {
                return;
            }

            string value;
            if (!conceptMap.TryGetValue(currentConcept, out value))
            {
                conceptMap.Add(currentConcept, keyValue.Code);
            } 
            else if (!value.Equals(keyValue.Code))
            {
                conceptMap.Remove(currentConcept);
                skipConcepts.Add(currentConcept);
            }
        }

        /// <summary>
        /// Processes the key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <param name="conceptMap">The concept map.</param>
        /// <param name="skipConcepts">The skip concepts.</param>
        private static void ProcessKeyValues(IEnumerable<IKeyValue> keyValues, IDictionary<string, string> conceptMap, ISet<string> skipConcepts)
        {
            foreach (var keyValue in keyValues)
            {
                ProcessKeyValue(keyValue, conceptMap, skipConcepts);
            }
        }
    }
}