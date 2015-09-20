// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RESTDataQueryCore.cs" company="EUROSTAT">
//  //   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file
// </copyright>
// <summary>
//   The rest data query.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    /// The rest data query.
    /// </summary>
    public class RESTDataQueryCore : IRestDataQuery
    {
        /// <summary>
        /// All
        /// </summary>
        public static readonly string ALL = "all";

        /// <summary>
        /// The flow ref
        /// </summary>
        private IStructureReference flowRef;

        /// <summary>
        /// The provider ref
        /// </summary>
        private IStructureReference providerRef;

        /// <summary>
        /// The start period
        /// </summary>
        private ISdmxDate startPeriod;

        /// <summary>
        /// The end period
        /// </summary>
        private ISdmxDate endPeriod;

        /// <summary>
        /// The updated after
        /// </summary>
        private ISdmxDate updatedAfter;

        /// <summary>
        /// The first n observation
        /// </summary>
        private int? firstNObs;

        /// <summary>
        /// The last n observation
        /// </summary>
        private int? lastNObs;

        /// <summary>
        /// The query detail
        /// </summary>
        private DataQueryDetail queryDetail = DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full);

        /// <summary>
        /// The dimension at observation
        /// </summary>
        private string dimensionAtObservation;

        /// <summary>
        /// The query list
        /// </summary>
        private readonly IList<ISet<string>> queryList;

        /// <summary>
        /// Gets a String representation of this query, in SDMX REST format starting from Data/.
        /// <example>
        /// Example Data/ACY,FLOW,1.0/M.Q+P....L/ALL?detail=seriesKeysOnly
        /// </example>
        /// </summary>
        public virtual string RestQuery
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTDataQueryCore" /> class.
        /// Constructs a data query off a full or partial REST URL - the URL must start before the Data segment and be complete, example input:
        /// /data/IMF,PGI,1.0/156.BCA.BOP.L_B.Q?detail=full
        /// </summary>
        /// <param name="restString">The rest string.</param>
        public RESTDataQueryCore(string restString)
            : this(restString, null)
        {
            
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTDataQueryCore"/> class. 
        /// Constructs a data query off a full or partial REST URL - the URL must start before the Data segment and be complete, example input:
        /// /data/IMF,PGI,1.0/156.BCA.BOP.L_B.Q?detail=full
        /// </summary>
        /// <param name="reststring">
        /// The rest query string.
        /// </param>
        /// <param name="queryParameters">
        /// The query parameters.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Data query expected to start with 'data/'
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Incomplete assignment in data query!
        /// </exception>
	     public RESTDataQueryCore(String reststring, IDictionary<string, string> queryParameters) {
            this.queryList = new List<ISet<string>>();
            int dataIndex = reststring.IndexOf("data/", StringComparison.Ordinal);
            if (dataIndex < 0)
            {
                throw new ArgumentException("Data query expected to start with 'data/'");
            }
            string querystring = reststring.Substring(dataIndex);
            if(queryParameters == null)
            {
                 queryParameters = new Dictionary<string, string>();
            }

	        //Parse any additional parameters
	        int indexOfQuestionMark = querystring.IndexOf('?');
	        if (indexOfQuestionMark > 0)
            {
                string paras = querystring.Substring(indexOfQuestionMark + 1);
                querystring = querystring.Substring(0, indexOfQuestionMark);

                /* foreach */
                string[] currentParams = paras.Split('&');
                foreach (string currentParam  in currentParams)
                {
                    string[] param = currentParam.Split('=');
                  	if (param.Length < 2) 
                    {
					   throw new SdmxSemmanticException("Incomplete assignment in data query! Parameter at fault: " + param);
				    }
                    queryParameters.Add(param[0], param[1]);
                }
            }

            ParserQuerystring(querystring.Split('/'));
            ParserQueryParameters(queryParameters);
        }

        public RESTDataQueryCore(string[] querystring, IDictionary<string, string> queryParameters)
        {
            this.queryList = new List<ISet<string>>();
            ParserQuerystring(querystring);
            ParserQueryParameters(queryParameters);
        }

        /// <summary>
        /// Parsers the query string.
        /// </summary>
        /// <param name="querystring">The query string.</param>
        /// <exception cref="SdmxSemmanticException">
        /// Data query expected to contain Flow as the second argument
        /// or
        /// Data query expected unexpected 4th argument
        /// </exception>
        private void ParserQuerystring(IList<string> querystring)
        {
            if (querystring.Count < 2)
            {
                throw new SdmxSemmanticException("Data query expected to contain Flow as the second argument");
            }
            this.SetFlowRef(querystring[1]);

            if (querystring.Count < 3)
            {
                SetKey(ALL);
	    	}
            else
            {
                SetKey(querystring[2]);
		    }
         
            if (querystring.Count > 3)
            {
                SetProvider(querystring[3]);
            }
            if (querystring.Count > 4)
            {
                throw new SdmxSemmanticException("Data query expected unexpected 4th argument");
            }
        }

        /// <summary>
        /// Gets the dataflow reference
        /// </summary>
        public virtual IStructureReference FlowRef
        {
            get
            {
                return flowRef;
            }
        }

        /// <summary>
        /// Sets the flow ref.
        /// </summary>
        /// <param name="flowRef">The flow ref.</param>
        public void SetFlowRef(string flowRef)
        {
            if (!flowRef.Equals(ALL, StringComparison.OrdinalIgnoreCase))
            {
                this.flowRef = ParseFlowRef(flowRef);
            }
        }

        /// <summary>
        /// Gets the data provider reference, or null if ALL
        /// </summary>
        public virtual IStructureReference ProviderRef
        {
            get
            {
                return providerRef;
            }
        }

        /// <summary>
        /// Gets the start date to  the data from, or null if undefined
        /// </summary>
        public virtual ISdmxDate StartPeriod
        {
            get
            {
                return startPeriod;
            }
        }

        /// <summary>
        /// Sets the detail.
        /// </summary>
        /// <param name="detail">The detail.</param>
        private void SetDetail(string detail)
        {
            if (detail != null)
            {
                queryDetail = DataQueryDetail.ParseString(detail);
            }
        }

        /// <summary>
        /// Sets the start period.
        /// </summary>
        /// <param name="startPeriod">The start period.</param>
        /// <exception cref="SdmxSemmanticException">Could not format 'startPeriod' value  + startPeriod +  as a date</exception>
        public void SetStartPeriod(string startPeriod)
        {
            try
            {
                this.startPeriod = new SdmxDateCore(startPeriod);
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Could not format 'startPeriod' value " + startPeriod + " as a date", e);
            }
        }

        /// <summary>
        /// Sets the end period.
        /// </summary>
        /// <param name="endPeriod">The end period.</param>
        /// <exception cref="SdmxSemmanticException">Could not format 'endPeriod' value  + endPeriod +  as a date</exception>
        public void SetEndPeriod(string endPeriod)
        {
            try
            {
                this.endPeriod = new SdmxDateCore(endPeriod);
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Could not format 'endPeriod' value " + endPeriod + " as a date", e);
            }
        }

        /// <summary>
        /// Sets the updated after.
        /// </summary>
        /// <param name="updatedAfter">The updated after.</param>
        /// <exception cref="SdmxSemmanticException">Could not format 'updatedAfter' value  + updatedAfter +  as a date</exception>
        public void SetUpdatedAfter(string updatedAfter)
        {
            try
            {
                this.updatedAfter = new SdmxDateCore(updatedAfter);
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Could not format 'updatedAfter' value " + updatedAfter + " as a date", e);
            }
        }

        /// <summary>
        /// Gets the end date to  the data from, or null if undefined
        /// </summary>
        public virtual ISdmxDate EndPeriod
        {
            get
            {
                return endPeriod;
            }
        }

        /// <summary>
        /// Gets the updated after date to  the data from, or null if undefined
        /// </summary>
        public virtual ISdmxDate UpdatedAfter
        {
            get
            {
                return updatedAfter;
            }
        }

        /// <summary>
        /// Gets the first 'n' observations, for each series key, to return as a result of a data query.
        /// </summary>
        public virtual int? LastNObsertations
        {
            get
            {
                return lastNObs;
            }
        }

        /// <summary>
        /// Gets the last 'n' observations, for each series key,  to return as a result of a data query
        /// </summary>
        public virtual int? FirstNObservations
        {
            get
            {
                return firstNObs;
            }
        }

        /// <summary>
        /// Gets the level of detail for the returned data, or null if undefined
        /// </summary>
        public virtual DataQueryDetail QueryDetail
        {
            get
            {
                return queryDetail;
            }
        }

        /// <summary>
        /// Gets the dimension to , or null if undefined
        /// </summary>
        public virtual string DimensionAtObservation
        {
            get
            {
                return dimensionAtObservation;
            }
        }

        /// <summary>
        /// Gets the list of dimension code id filters, in the same order as the dimensions are defined by the DataStructure
        /// </summary>
        public virtual IList<ISet<string>> QueryList
        {
            get
            {
                return queryList;
            }
        }

        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetProvider(string value)
        {
            if (!value.Equals(ALL, StringComparison.OrdinalIgnoreCase))
            {
                this.providerRef = ParseProviderRef(value);
            }
        }

        /// <summary>
        /// Parses the provider ref.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// The <see cref="IStructureReference"/>
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Unexpected number of data provider reference arguments (, separated) for reference: 
        ///  expecting a maximum of two arguments (Agency Id, Id)</exception>
        private static IStructureReference ParseProviderRef(string str)
        {
            string[] split = str.Split(',');
            if (split.Length > 2)
            {
                throw new SdmxSemmanticException(
                    "Unexpected number of data provider reference arguments (, separated) for reference: " + str
                    + " - expecting a maximum of two arguments (Agency Id, Id)");
            }
            if (split.Length == 2)
            {
                return new StructureReferenceImpl(
                    split[0], DataProviderScheme.FixedId, DataProviderScheme.FixedVersion,
                    SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), split[1]);
            }
            return new StructureReferenceImpl(
                null, DataProviderScheme.FixedId, DataProviderScheme.FixedVersion,
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), split[0]);
        }

        /// <summary>
        /// Parses the flow ref.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// The <see cref="IStructureReference"/>
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Unexpected number of reference arguments (, separated) for reference: 
        ///  expecting a maximum of three arguments (Agency Id, Id, and Version)</exception>
        private static IStructureReference ParseFlowRef(string str)
        {
            string[] split = str.Split(',');
            if (split.Length > 3)
            {
                throw new SdmxSemmanticException(
                    "Unexpected number of reference arguments (, separated) for reference: " + str
                    + " - expecting a maximum of three arguments (Agency Id, Id, and Version)");
            }
            if(split.Length > 2 && split[2].Equals(ALL, StringComparison.OrdinalIgnoreCase))
            {
			   split[2] = null;
		    }
		    if(split.Length > 1 && split[1].Equals(ALL, StringComparison.OrdinalIgnoreCase))
            {
			   split[1] = null;
		    }
		    if(split.Length > 0 && split[0].Equals(ALL, StringComparison.OrdinalIgnoreCase))
            {
			   split[0] = null;
 		    }
            if (split.Length == 3)
            {
                return new StructureReferenceImpl(
                    split[0], split[1], split[2], SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            }
            if (split.Length == 2)
            {
                return new StructureReferenceImpl(
                    split[0], split[1], null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            }
            return new StructureReferenceImpl(
                null, split[0], null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
        }

        /// <summary>
        /// Sets the key.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetKey(string value)
        {
            if (!value.Equals(ALL, StringComparison.OrdinalIgnoreCase))
            {
                // NOTE in .NET Split does not accept regex like in Java.
                string[] split = value.Split('.');

                foreach (string currentKey  in  split)
                {
                    ISet<string> selectionsList = new HashSet<string>();
                    this.queryList.Add(selectionsList);

                    // NOTE in .NET Split does not accept regex like in Java.
                    foreach (string currentSelection in currentKey.Split('+'))
                    {
                        if (ObjectUtil.ValidString(currentSelection))
                        {
                            selectionsList.Add(currentSelection);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the last x obs.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="SdmxSemmanticException">Query parameter 'lastNObservations' expects an Integer, was given:  + value</exception>
        private void SetLastXObs(string value)
        {
            try
            {
                this.lastNObs = int.Parse(value);
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Query parameter 'lastNObservations' expects an Integer, was given: " + value, e);
            }
        }

        /// <summary>
        /// Sets the first x obs.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="SdmxSemmanticException">Query parameter 'firstNObservations' expects an Integer, was given:  + value</exception>
        private void SetFirstXObs(string value)
        {
            try
            {
                this.firstNObs = int.Parse(value);
            }
            catch (FormatException e)
            {
                throw new SdmxSemmanticException("Query parameter 'firstNObservations' expects an Integer, was given: " + value, e);
            }
        }

        /// <summary>
        /// Parsers the query parameters.
        /// </summary>
        /// <param name="paramsDict">The query parameters dictionary.</param>
        /// <exception cref="SdmxSemmanticException">Unknown query parameter allowed parameters [startPeriod, endPeriod, updatedAfter, firstNObservations, lastNObservations, dimensionAtObservation, detail]</exception>
        private void ParserQueryParameters(IEnumerable<KeyValuePair<string, string>> paramsDict)
        {
            if (paramsDict != null)
            {
                foreach (var keyValue in paramsDict)
                {
                    var key = keyValue.Key;
                    var value = keyValue.Value;
                    if (key.Equals("startPeriod", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetStartPeriod(value);
                    }
                    else if (key.Equals("endPeriod", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetEndPeriod(value);
                    }
                    else if (key.Equals("updatedAfter", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetUpdatedAfter(value);
                    }
                    else if (key.Equals("firstNObservations", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetFirstXObs(value);
                    }
                    else if (key.Equals("lastNObservations", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetLastXObs(value);
                    }
                    else if (key.Equals("dimensionAtObservation", StringComparison.OrdinalIgnoreCase))
                    {
                        this.dimensionAtObservation = value;
                    }
                    else if (key.Equals("detail", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetDetail(value);
                    }
                    else
                    {
                        throw new SdmxSemmanticException(
                            "Unknown query parameter  '" + key
                            +
                            "' allowed parameters [startPeriod, endPeriod, updatedAfter, firstNObservations, lastNObservations, dimensionAtObservation, detail]");
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("data/");
            if (flowRef == null)
            {
                sb.Append("ALL");
            }
            else
            {
                IMaintainableRefObject flowRefMaint = flowRef.MaintainableReference;
                string flowAgency = flowRefMaint.AgencyId ?? "ALL";
                string flowId = flowRefMaint.MaintainableId ?? "ALL";
                string flowVersion = flowRefMaint.Version ?? "ALL";
                sb.Append(flowAgency + "," + flowId + "," + flowVersion);
            }
            sb.Append("/");
            string concat = string.Empty;
            if (!ObjectUtil.ValidCollection(queryList))
            {
                sb.Append("ALL");
            }
            else
            {
                foreach (ISet<string> currentQuery  in  queryList)
                {
                    sb.Append(concat);
                    concat = string.Empty;

                    foreach (string code  in  currentQuery)
                    {
                        sb.Append(concat);
                        sb.Append(code);
                        concat = "+";
                    }
                    concat = ".";
                }
            }
            sb.Append("/");
            if (providerRef == null)
            {
                sb.Append("ALL");
            }
            else
            {
                IMaintainableRefObject provRefMaint = providerRef.MaintainableReference;

                string provAgency = provRefMaint.AgencyId ?? "ALL";
                string provId = (providerRef.ChildReference == null) ? "ALL" : providerRef.ChildReference.Id;
                sb.Append(provAgency + "," + provId);
            }
            concat = "?";
            if (startPeriod != null)
            {
                sb.Append(concat + "startPeriod=" + startPeriod.DateInSdmxFormat);
                concat = "&";
            }
            if (endPeriod != null)
            {
                sb.Append(concat + "endPeriod=" + endPeriod.DateInSdmxFormat);
                concat = "&";
            }
            if (updatedAfter != null)
            {
                sb.Append(concat + "updatedAfter=" + updatedAfter.DateInSdmxFormat);
                concat = "&";
            }
            if (firstNObs != null)
            {
                sb.Append(concat + "firstNObservations=" + firstNObs);
                concat = "&";
            }
            if (lastNObs != null)
            {
                sb.Append(concat + "lastNObservations=" + lastNObs);
                concat = "&";
            }
            if (queryDetail != null)
            {
                sb.Append(concat + "detail=" + queryDetail.RestParam);
                concat = "&";
            }
            if (ObjectUtil.ValidString(dimensionAtObservation))
            {
                sb.Append(concat + "dimensionAtObservation=" + dimensionAtObservation);
                concat = "&";
            }
            return sb.ToString();
        }
    }
}