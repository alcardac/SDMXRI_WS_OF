using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
using Org.Sdmxsource.Sdmx.Api.Constants;
using Org.Sdmxsource.Sdmx.Api.Exception;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query.Complex
{
    using Org.Sdmxsource.Util;

    public class ComplexComponentValueImpl : IComplexComponentValue
    {
        #region Fields

        private readonly string _value;

	    private readonly TextSearch _textOperator;

	    private readonly OrderedOperator _orderedOperator;

        #endregion

        #region Constructors and Destructors

	    public ComplexComponentValueImpl(string value, TextSearch textOperator, SdmxStructureEnumType componentType)
        {
		    if(componentType.Equals(SdmxStructureEnumType.Dimension) || componentType.Equals(SdmxStructureEnumType.TimeDimension)) 
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionIllegalOperator);
		
		    this._value = value;
		    if (textOperator != null)
			    this._textOperator = textOperator;
		    else
			    this._textOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
	    }

	    public ComplexComponentValueImpl(string value, OrderedOperator orderedOperator,  SdmxStructureEnumType componentType)
        {
		    if (componentType.Equals(SdmxStructureEnumType.TimeDimension) && orderedOperator.Equals(OrderedOperatorEnumType.NotEqual))
			    throw new SdmxSemmanticException(ExceptionCode.QuerySelectionIllegalOperator);

		    this._value =value;
		    if (orderedOperator!=null)
			    this._orderedOperator = orderedOperator;
		    else
			    this._orderedOperator = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal);
	    }

        #endregion

	    #region Public Properties

        public string  Value
        {
	        get 
            { 
                return _value;
            }
        }

        public TextSearch  TextSearchOperator
        {
	        get 
            { 
                return _textOperator; 
            }
        }

        public OrderedOperator  OrderedOperator
        {
	        get 
            { 
                return _orderedOperator;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var value = obj as IComplexComponentValue;
            return value != null
                   && string.Equals(value.Value, this._value)
                   && Equals(value.OrderedOperator, this._orderedOperator)
                   && Equals(value.TextSearchOperator, this._textOperator);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() 
        {
		    return this.ToString().GetHashCode();
	    }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() 
        {
		    StringBuilder sb = new StringBuilder();
		    sb.Append("Operator");
		    sb.Append(" : ");
            if (this._textOperator != null)
            {
                sb.Append(this._textOperator);
            }
            if (this._orderedOperator != null)
            {
                sb.Append(this._orderedOperator);
            }
            sb.Append("applied upon ");
            sb.Append(_value);
		    return sb.ToString();
        }

        #endregion
    }
}