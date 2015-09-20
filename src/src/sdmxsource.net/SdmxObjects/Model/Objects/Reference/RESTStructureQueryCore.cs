// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RESTStructureQueryImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The rest structure query core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///   The rest structure query core.
    /// </summary>
    [Serializable]
    public class RESTStructureQueryCore : IRestStructureQuery
    {

        #region Fields

        /// <summary>
        ///   The structure reference.
        /// </summary>
        private IStructureReference _structureReference;

        /// <summary>
        ///   The structure query metadata.
        /// </summary>
        private IStructureQueryMetadata _structureQueryMetadata = new StructureQueryMetadataCore(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Null), null, null, true);

        #endregion

        #region Constructors and Destructors


        /// <summary>
        /// Initializes a new instance of the <see cref="RESTStructureQueryCore"/> class. 
        /// Creation of a Structure Query for structures that match the given reference
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference. 
        /// </param>
	    public RESTStructureQueryCore(IStructureReference structureReference)
        {
		    this._structureReference = structureReference;
            if(structureReference.Version != null)
            {
		  	  _structureQueryMetadata = new StructureQueryMetadataCore(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Null), null, null, false);
		    }
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTStructureQueryCore"/> class. 
        /// </summary>
        /// <param name="structureQueryDetail">
        /// The structure query detail. 
        /// </param>
        /// <param name="structureReferenceDetail">
        /// The structure reference detail. 
        /// </param>
        /// <param name="specificStructureReference">
        /// The specific structure reference. 
        /// </param>
        /// <param name="structureReference">
        /// The structure reference. 
        /// </param>
        /// <param name="returnLatest">
        /// The return last. 
        /// </param>
	    public RESTStructureQueryCore(StructureQueryDetail structureQueryDetail, StructureReferenceDetail structureReferenceDetail,
			SdmxStructureType specificStructureReference, IStructureReference structureReference, bool returnLatest)
         {
		       this._structureQueryMetadata = new StructureQueryMetadataCore(structureQueryDetail, structureReferenceDetail, specificStructureReference, returnLatest);
		       this._structureReference = structureReference;
	     }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTStructureQueryCore"/> class. 
        /// </summary>
        /// <param name="restString">
        /// The rest string. 
        /// </param>
        public RESTStructureQueryCore(string restString) : 	this(restString, null)
        {
	
     	}

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTStructureQueryCore"/> class. 
        ///  Constructs a REST query from the rest query string, example:
        /// /dataflow/ALL/ALL/ALL
        /// </summary>
        /// <param name="queryString">
        /// The rest query string. 
        /// </param>
        /// <param name="queryParameters">
        /// The parameters. 
        /// </param>
        public RESTStructureQueryCore(string queryString, IDictionary<string, string> queryParameters)
        {
            if (queryString.StartsWith("/"))
            {
                queryString = queryString.Substring(1);
            }

            if (queryParameters == null)
            {
                queryParameters = new Dictionary<string, string>();
            }

            //Parse any additional parameters
            if (queryString.IndexOf("?", StringComparison.Ordinal) > 0)
            {
                string paramters = queryString.Substring(queryString.IndexOf("?", StringComparison.Ordinal) + 1);
                queryString = queryString.Substring(0, queryString.IndexOf("?", StringComparison.Ordinal));

                foreach (string currentParam in paramters.Split('&'))
                {
                    string[] param = currentParam.Split('=');
                    queryParameters.Add(param[0], param[1]);
                }
            }

            string[] args = queryString.Split('/');

            ParserQueryString(args);
            _structureQueryMetadata = new StructureQueryMetadataCore(args, queryParameters);
        }

        /// <summary>
         /// Initializes a new instance of the <see cref="RESTStructureQueryCore"/> class. 
         /// </summary>
         /// <param name="queryString">
         /// The query string. 
         /// </param>
         /// <param name="queryParameters">
         /// The query parameters. 
         /// </param>
	     public RESTStructureQueryCore(string[] queryString, IDictionary<string, string> queryParameters) 
         {
		       ParserQueryString(queryString);
		       _structureQueryMetadata = new StructureQueryMetadataCore(queryString, queryParameters);
	     }

        #endregion

         #region Public Properties

         /// <summary>
         ///   Gets the structure query metadata.
         /// </summary>
         public virtual IStructureQueryMetadata StructureQueryMetadata
         {
               get
               {
                   return _structureQueryMetadata;
               }
         }

         /// <summary>
         ///   Gets the structure reference.
         /// </summary>
	     public virtual IStructureReference StructureReference
         {
              get
              {
                  return _structureReference;
              }
          }

         #endregion

         #region Public Methods and Operators

         /// <summary>
         ///   The to string.
         /// </summary>
         public override string ToString()
         {
             return "ref: " + _structureReference + 
                 "-detail:" + _structureQueryMetadata.StructureQueryDetail
                 + "-references:" + _structureReference +
                 "-specific" + _structureQueryMetadata.SpecificStructureReference + 
                 "latest" + _structureQueryMetadata.IsReturnLatest;
         }

         #endregion

         #region Methods

         /// <summary>
         ///   The parser query string.
         /// </summary>
         /// <param name="queryString">
         /// The query string. 
         /// </param>
         private void ParserQueryString(string[] queryString)
         {
		   if(queryString.Length < 1) 
           {
		    	throw new SdmxSemmanticException("Structure Query Expecting at least 1 parameter (structure type)");
		   }
		   SdmxStructureType structureType = GetStructureType(queryString[0]);
		   string agencyId = null;
		   string id = null;
		   string version = null;  
		   if(queryString.Length >= 2)
           {
			   agencyId = ParseQueryString(queryString[1]);
		   }
		   if(queryString.Length >= 3) 
           {
			   id = ParseQueryString(queryString[2]);
		   }
		   if(queryString.Length >= 4) 
           {
		       version = ParseQueryString(queryString[3]); 
		   } 
		   this._structureReference = new StructureReferenceImpl(agencyId, id, version, structureType);
	   }

       /// <summary>
       ///  The parse query string.
       /// </summary>
       /// <param name="query">
       /// The query. 
       /// </param>
	   private string ParseQueryString(string query) 
       {
		   if(!ObjectUtil.ValidString(query)) 
           {
			   return null;
		   }
		   if(query.Equals("all", StringComparison.InvariantCultureIgnoreCase)) 
           {
			   return "*";
		   }
		   if(query.Equals("latest", StringComparison.InvariantCultureIgnoreCase))
           {
			   return null;
		   }
		   return query;
	  }

       /// <summary>
       ///  The get structure type.
       /// </summary>
       /// <param name="str">
       /// The str. 
       /// </param>
	  private static SdmxStructureType GetStructureType(string str)
      {
		  if(str.Equals("structure", StringComparison.InvariantCultureIgnoreCase))
          {
			  return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any);
		  }
		  if(str.Equals("organisationscheme", StringComparison.InvariantCultureIgnoreCase))
          {
			  return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationScheme);
		  }
		  return SdmxStructureType.ParseClass(str);
       }

       #endregion


    }
}
