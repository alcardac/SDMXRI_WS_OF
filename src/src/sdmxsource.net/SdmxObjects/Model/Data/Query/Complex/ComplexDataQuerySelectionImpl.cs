using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
using Org.Sdmxsource.Util;
using Org.Sdmxsource.Sdmx.Api.Exception;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Util.Extensions;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex
{
    public class ComplexDataQuerySelectionImpl : IComplexDataQuerySelection
    {
        #region Fields

        string _componentId;

	    private ISet<IComplexComponentValue> _values = new HashSet<IComplexComponentValue>();
	
	    #endregion

        #region Constructors and Destructors

	    public ComplexDataQuerySelectionImpl(string componentId, IList<IComplexComponentValue> value)
        {
		    if(!ObjectUtil.ValidString(componentId)) 
            {
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConcept);
		    }
		    this._componentId = componentId;
		
		    if (value == null || value.Count == 0) 
            {
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConceptValue);
		    }
		    //check 
		    foreach (IComplexComponentValue currentValue in value) 
            {
			    _values.Add(currentValue);
		    }
	    }
	
	    public ComplexDataQuerySelectionImpl(string componentId, ISet<IComplexComponentValue> values) 
        {
		    if(!ObjectUtil.ValidString(componentId)) 
            {
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConcept);
		    }

		    this._componentId = componentId;

		    if(!ObjectUtil.ValidCollection(values)) 
            {
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConceptValue);
		    }
		    this._values = new HashSet<IComplexComponentValue>(values);
	    }

        #endregion

        #region Public Properties
    
        public string  ComponentId
        {
	        get { return _componentId; }
        }

        public IComplexComponentValue  Value
        {
	        get 
            { 
                if(_values.Count > 1) 
                {
			        throw new ArgumentException("More than one value exists for this selection");
		        }
		        return (IComplexComponentValue)_values.ToArray()[0]; 
            }
        }

        public ISet<IComplexComponentValue>  Values
        {
	        get { return _values; }
        }

        public bool  HasMultipleValues()
        {
 	        return _values.Count > 1;
        }

        #endregion

        #region Public Methods and Operators

	    public void AddValue(IComplexComponentValue value) 
        {
		    this._values.Add(value);
	    }

	    public override bool Equals(Object obj) 
        {
            var selection = obj as IComplexDataQuerySelection;
            return selection != null && this.ComponentId.Equals(selection.ComponentId) && this.Values.SetEquals(selection.Values);
        }

	    public override int GetHashCode() 
        {
		    return ToString().GetHashCode();
	    }
	
	    public override string ToString() 
        {
		    StringBuilder sb = new StringBuilder();
		    sb.Append("");
		    sb.Append(_componentId);
		    sb.Append("");
		    sb.Append(" : ");
		    string concat = "";
		    foreach(IComplexComponentValue currentValue in _values) 
            {
			    sb.Append(concat);
			    sb.Append(currentValue.Value);
			    concat = ",";
		    }
		    return sb.ToString();
	    }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}