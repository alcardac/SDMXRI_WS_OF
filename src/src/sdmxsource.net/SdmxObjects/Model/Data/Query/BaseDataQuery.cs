using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
using Org.Sdmxsource.Sdmx.Api.Exception;
using Org.Sdmxsource.Sdmx.Api.Constants;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query
{
    public abstract class BaseDataQuery
    {
        #region Fields

        private string _dimensionAtObservation;

        private int? _firstNObs;

        private int? _lastNObs;

        private IDataflowObject _dataflow;

        private IDataStructureObject _dataStructure;

        #endregion

        #region Public Methods and Operators
        
        protected void ValidateQuery() 
        {
		    if (_dataflow == null) 
            {
			    throw new ArgumentException("Can not create DataQuery, Dataflow is required");
		    }

		    if(_dataStructure == null) 
            {
			    throw new ArgumentException("Can not create DataQuery, DataStructure is required");
		    }

		    ValidateQueryComponents();
		    ValidateDimensionAtObservation();
	    }
	
	    /**
	     * Validates that the queried components (e.g. dimension/attributes) exist on the data structure
	     */
	    private void ValidateQueryComponents() 
        {
		    foreach (string currentComponetId in GetQueryComponentIds()) 
            {
			    if (_dataStructure.GetComponent(currentComponetId) == null) 
                {
				    throw new SdmxSemmanticException("Data Structure '" + _dataStructure.Urn + "' does not contain component with id: " + currentComponetId);
			    }
		    }
	    }
	
	    /**
	     * Returns a set of strings which represent the component ids that are being queried on
	     * @return
	     */
	    protected abstract ISet<string> GetQueryComponentIds();
	
	    /**
	     * If no dimension at observation is set, then the following rules apply in the order specified:
	     * <ol>
	     *  <li>Set to Time Dimension (if it exists)</li>
	     *  <li>Set to the first Measure Dimension (if one exists)</li>
	     *  <li>Set to AllDimensions</li>
	     * </ol>  
	     * 
	     * If the dimension at observation is set, then it is validated to exist
	     */
	    private void ValidateDimensionAtObservation() 
        {
		    if (_dimensionAtObservation == null) 
            {
			    if (_dataStructure.TimeDimension != null) 
                {
				    _dimensionAtObservation = _dataStructure.TimeDimension.Id;
			    } 
                else if (_dataStructure.GetDimensions(SdmxStructureEnumType.MeasureDimension).Count > 0)
                {
				    _dimensionAtObservation = _dataStructure.GetDimensions(SdmxStructureEnumType.MeasureDimension)[0].Id;
			    } 
                else 
                {
				    _dimensionAtObservation = "AllDimensions";
			    }
		    } 
            else if (!_dimensionAtObservation.Equals("AllDimensions"))
            {
			    IDimension dimension = _dataStructure.GetDimension(_dimensionAtObservation);
			    if (dimension==null) 
                {
				    StringBuilder sb = new StringBuilder();
				    foreach (IDimension dim in _dataStructure.GetDimensions(SdmxStructureEnumType.Dimension, SdmxStructureEnumType.MeasureDimension, SdmxStructureEnumType.TimeDimension))
                    {
                        sb.Append(dim.Id + "\r\n");
				    }

                    // Changed from ArgumentException in order for Web Service to produce the correct error (semantic).
                    // This is different than in SdmxSource Java where it throws the equivalent of ArgumentException. 
                    throw new SdmxSemmanticException("Can not create DataQuery, The dimension at observation '" + _dimensionAtObservation + "' is not included in the Dimension list of the DSD.  Allowed values are " + sb.ToString());		
			    }
		    }
	    }

        #endregion

        #region Public Properties


        /// <summary>
        /// Gets or sets the dimension at observation.
        /// </summary>
        /// <value>
        /// The dimension at observation.
        /// </value>
        public string DimensionAtObservation
        {
            get
            {
                return _dimensionAtObservation;
            }
            protected set
            {
                this._dimensionAtObservation = value;
            }
        }

        public int? FirstNObservations
        {
            get
            {
                return this._firstNObs;
            }
            set
            {
                this._firstNObs = value;
            }
        }

        public int? LastNObservations
        {
            get
            {
                return this._lastNObs;
            }
            set
            {
                this._lastNObs = value;
            }
        }

        public IDataflowObject Dataflow
        {
            get
            {
                return this._dataflow;
            }
            set
            {
                this._dataflow = value;
            }
        }

        public IDataStructureObject DataStructure
        {
            get
            {
                return this._dataStructure;
            }
            set
            {
                this._dataStructure = value;
            }
        }

        #endregion
    }
}