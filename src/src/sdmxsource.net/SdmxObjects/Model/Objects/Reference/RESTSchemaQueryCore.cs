// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RESTSchemaQueryImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The rest schema query core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///   The rest schema query core.
    /// </summary>
    [Serializable]
    public class RESTSchemaQueryCore : IRestSchemaQuery
    {
          #region Fields

          /// <summary>
          ///   The reference.
          /// </summary>
  	      private IStructureReference _reference;

          /// <summary>
          ///   The dim at obs.
          /// </summary>
	      private string _dimAtObs =  DimensionObject.TimeDimensionFixedId;

          /// <summary>
          ///   The explicit measure.
          /// </summary>
	      private bool _explicitMeasure;

          /// <summary>
          ///   The context.
          /// </summary>
	      private string _context;

          /// <summary>
          ///   The agency id.
          /// </summary>
	      private string _agencyId;

          /// <summary>
          ///   The id.
          /// </summary>
	      private string _id;

          /// <summary>
          ///   The version.
          /// </summary>
	      private string _version;

          #endregion

          #region Constructors and Destructors

          public RESTSchemaQueryCore(IStructureReference reference, string dimAtObs): this(reference, dimAtObs, true)
          {
        	    
          }
	
          public RESTSchemaQueryCore(IStructureReference reference, string dimAtObs, bool explicitMeasure) 
          {
		     this._reference = reference;
		     if(ObjectUtil.ValidString(dimAtObs)) 
             {
			      this._dimAtObs = dimAtObs;
		     }
		     this._explicitMeasure = explicitMeasure;
	      }

          /// <summary>
          /// Initializes a new instance of the <see cref="RESTSchemaQueryCore"/> class. 
          ///  Constructs a schema query from a full or partial REST URL.
          ///  The URL must start before the Schema segment and be complete, example input:
          ///  /schema/provision/IMF/PGI/1.0?dimensionAtObservation=freq
          /// </summary>
          /// <param name="restString">REst Uri
          /// </param>
	      public RESTSchemaQueryCore(string restString): this(restString, null)
          {
		
	      }
	
         /// <summary>
         /// Initializes a new instance of the <see cref="RESTSchemaQueryCore"/> class. 
         ///  Constructs a schema query from a full or partial REST URL.
         ///  The URL must start before the Schema segment and be complete, example input:
         ///  /schema/provision/IMF/PGI/1.0?dimensionAtObservation=freq
         /// </summary>
         /// <param name="restString">REst Uri
         /// </param>
         /// <param name="queryParameters">
         /// </param>
	     public RESTSchemaQueryCore(string restString, IDictionary<string, string> queryParameters)
         {
		  
            // Construct a String[] for the queryString
		    string queryString = restString.Substring(restString.IndexOf("schema/", StringComparison.OrdinalIgnoreCase));
		    string[] queryStringArr = queryString.Split('/');
		
		    if(queryParameters == null)
		    {
		        queryParameters = new Dictionary<string, string>();
		    }
		    if(queryString.IndexOf('?') > 0) 
            {
			    string parameters = queryString.Substring(queryString.IndexOf('?')+1);
			    queryString = queryString.Substring(0,queryString.IndexOf('?'));
					
			   foreach(string currentParam in parameters.Split('&'))
               {
				  string[] param = currentParam.Split('=');
				  queryParameters.Add(param[0],param[1]);
			   }
		    }
		
		    Evaluate(queryStringArr, queryParameters);
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTSchemaQueryCore"/> class.
        /// </summary>
        /// <param name="queryString">
        /// The query string. 
        /// </param>
        /// <param name="queryParameters">
        /// The query parameters. 
        /// </param>
	    public RESTSchemaQueryCore(string[] queryString, IDictionary<string, string> queryParameters) 
        {
		    Evaluate(queryString, queryParameters);
	    }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the reference.
        /// </summary>
        public IStructureReference Reference
        {
           get
           {
               return _reference;
           }
        }

        /// <summary>
        ///   Gets the dim at obs.
        /// </summary>
        public string DimAtObs
        {
           get
           {
               return _dimAtObs;
           }
        }

       #endregion

       #region Public Methods and Operators

        /// <summary>
        /// The is explicit measure.
        /// </summary>
        public bool IsExplicitMeasure()
        {
           return _explicitMeasure;
        }

        public void Dispose()
        {

        }

       #endregion

       #region Methods

       /// <summary>
       /// The evaluate.
       /// </summary>
       /// <param name="queryString">
       /// The query string. 
       /// </param>
       /// <param name="queryParameters">
       /// The query parameters. 
       /// </param>
       private void Evaluate(string[] queryString, IDictionary<string, string> queryParameters)
       {
		    ParseQueryString(queryString);
		    ParseQueryParameters(queryParameters);
		
		    SdmxStructureType referencedStructure = SdmxStructureType.ParseClass(_context);
		
		    if (referencedStructure != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd) &&
			referencedStructure != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow) && 
			referencedStructure != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement) && 
			referencedStructure != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow) &&
			referencedStructure != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd))
            {
		    	throw new SdmxSemmanticException("The referenced structure is not a legitimate type!");
		    }
			
	   	    IMaintainableRefObject maintainableRef = new MaintainableRefObjectImpl(_agencyId, _id, _version);
		    _reference = new StructureReferenceImpl(maintainableRef, referencedStructure);
	   }

       /// <summary>
       /// The parse query string.
       /// </summary>
       /// <param name="queryString">
       /// The query string. 
       /// </param>
	    private void ParseQueryString(string[] queryString)
        {
		    if(queryString.Length < 2)
            {
		    	throw new SdmxSemmanticException("Schema query expected to contain context as the second argument");
		    }
		   _context = queryString[1];
		
		    if(queryString.Length < 3)
            {
			    throw new SdmxSemmanticException("Schema query expected to contain Agency ID as the third argument");
		    }
		    _agencyId = queryString[2];
		
		     if(queryString.Length < 4)
             {
			    throw new SdmxSemmanticException("Schema query expected to contain Resource ID as the fourth argument");
		     }
		     _id = queryString[3];
		
		    if(queryString.Length > 4) 
            {
			    _version = queryString[4];
			    if(_version.Equals("latest", StringComparison.InvariantCultureIgnoreCase)) 
                {
				    _version = null;
			    }
		    }
		
		   if(queryString.Length > 5) 
           {
		    	throw new SdmxSemmanticException("Schema query has unexpected sixth argument");
		   }
	  }

      /// <summary>
      /// The parse query parameters.
      /// </summary>
      /// <param name="parameters">
      /// The parameters. 
      /// </param>
	  private void ParseQueryParameters(IDictionary<string, string> parameters)
      {
		  if(parameters != null)
          {
			  foreach(string key in parameters.Keys)
              {
				  if(key.Equals("dimensionAtObservation", StringComparison.InvariantCultureIgnoreCase)) 
                  {
					_dimAtObs = parameters[key];
				  } 
                  else if(key.Equals("explicitMeasure", StringComparison.InvariantCultureIgnoreCase))
                  {
					string val = parameters[key];
					_explicitMeasure = bool.Parse(val);
				  } 
                  else
                  {
					throw new SdmxSemmanticException("Unknown query parameter : " + key +
							" allowed parameters [dimensionAtObservation, explicitMeasure]");
				  }
			  }
		  }
	   }

        #endregion
    }
}
