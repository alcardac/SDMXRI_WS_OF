using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
using Org.Sdmxsource.Sdmx.Api.Model.Base;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Exception;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex
{
    public class ComplexDataQuerySelectionGroupImpl : IComplexDataQuerySelectionGroup
    {
        #region Fields

        private ISet<IComplexDataQuerySelection> _complexSelections = new HashSet<IComplexDataQuerySelection>();

	    private ISet<IComplexComponentValue> _primaryMeasureValues = new HashSet<IComplexComponentValue>();
	    
        private IDictionary<string, IComplexDataQuerySelection> _complexSelectionForConcept = new Dictionary<string, IComplexDataQuerySelection>();
	    
        private ISdmxDate _dateFrom;
	    
        private OrderedOperator _dateToOperator;
	    
        private OrderedOperator _dateFromOperator;
	    
        private ISdmxDate _dateTo;

        #endregion

        #region Constructors and Destructors
        
        public ComplexDataQuerySelectionGroupImpl(ISet<IComplexDataQuerySelection> complexSelections, 
                                                    ISdmxDate dateFrom, OrderedOperator dateFromOperator, 
                                                    ISdmxDate dateTo, OrderedOperator dateToOperator, 
                                                    ISet<IComplexComponentValue> primaryMeasureValues)
        {
		    //check if the operator to be applied on the time has not the 'NOT_EQUAL' value
		    if (dateFromOperator.Equals(OrderedOperatorEnumType.NotEqual) || dateToOperator.Equals(OrderedOperatorEnumType.NotEqual))
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionIllegalOperator);
		
		    if (complexSelections == null) 
            {
			    return;
		    }
		
		    this._dateFrom = dateFrom;
		    this._dateFromOperator = dateFromOperator;
		    this._dateTo = dateTo;
		    this._dateToOperator = dateToOperator;	
		    this._complexSelections = complexSelections;
		    this._primaryMeasureValues = primaryMeasureValues;
		
		    // Add each of the Component Selections to the selection concept map. 
		    foreach (IComplexDataQuerySelection compSel in _complexSelections) 
            {
			    if (_complexSelectionForConcept.ContainsKey(compSel.ComponentId)) 
                {
				    //TODO Does this require a exception, or can the code selections be merged?
				    throw new ArgumentException("Duplicate concept");
			    }
			    _complexSelectionForConcept.Add(compSel.ComponentId, compSel);
		    }		
	    }

        #endregion

        #region Public Properties

        public IComplexDataQuerySelection  GetSelectionsForConcept(string componentId)
        {
 	        return _complexSelectionForConcept[componentId];
        }

        public bool  HasSelectionForConcept(string componentId)
        {
 	        return _complexSelectionForConcept.ContainsKey(componentId);
        }

        public ISet<IComplexDataQuerySelection>  Selections
        {
            get { return _complexSelections; }
        }

        public ISdmxDate  DateFrom
        {
            get { return _dateFrom; }
        }

        public OrderedOperator  DateFromOperator
        {
            get { return _dateFromOperator; }
        }

        public ISdmxDate  DateTo
        {
            get { return _dateTo; }
        }

        public OrderedOperator  DateToOperator
        {
            get { return _dateToOperator; }
        }

        public ISet<IComplexComponentValue>  PrimaryMeasureValue
        {
            get { return _primaryMeasureValues; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}