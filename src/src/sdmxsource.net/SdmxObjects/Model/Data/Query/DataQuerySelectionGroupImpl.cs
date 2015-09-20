// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQuerySelectionGroupImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query selection group impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;

    /// <summary>
    ///   The data query selection group impl.
    /// </summary>
    [Serializable]
    public class DataQuerySelectionGroupImpl : IDataQuerySelectionGroup
    {
        #region Fields

        /// <summary>
        ///   The _date from.
        /// </summary>
        private readonly ISdmxDate _dateFrom;

        /// <summary>
        ///   The _date to.
        /// </summary>
        private readonly ISdmxDate _dateTo;

        /// <summary>
        ///   The _selection for concept.
        /// </summary>
        private readonly IDictionary<string, IDataQuerySelection> _selectionForConcept;

        /// <summary>
        ///   The _selections.
        /// </summary>
        private readonly ISet<IDataQuerySelection> _selections;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQuerySelectionGroupImpl"/> class.
        /// </summary>
        /// <param name="selections">
        /// The selections. 
        /// </param>
        /// <param name="dateFrom">
        /// The date from. 
        /// </param>
        /// <param name="dateTo">
        /// The date to. 
        /// </param>
        /// ///
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException.
        /// </exception>
        public DataQuerySelectionGroupImpl(
            ISet<IDataQuerySelection> selections, ISdmxDate dateFrom, ISdmxDate dateTo)
        {
            this._selections = new HashSet<IDataQuerySelection>();
            this._selectionForConcept = new Dictionary<string, IDataQuerySelection>();
            this._dateFrom = dateFrom;
            this._dateTo = dateTo;

            // NPE defence. If the selections are null, then exit this method.
            if (selections == null)
            {
                return;
            }

            this._selections = selections;

            // Add each of the Dimensions Selections to the selection concept map. 
            foreach (IDataQuerySelection dimSel in selections)
            {
                if (this._selectionForConcept.ContainsKey(dimSel.ComponentId))
                {
                    // TODO Does this require a exception, or can the code selections be merged?
                    throw new ArgumentException("Duplicate concept");
                }

                this._selectionForConcept.Add(dimSel.ComponentId, dimSel);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the date from.
        /// </summary>
        public virtual ISdmxDate DateFrom
        {
            get
            {
                return this._dateFrom;
            }
        }

        /// <summary>
        ///   Gets the date to.
        /// </summary>
        public virtual ISdmxDate DateTo
        {
            get
            {
                return this._dateTo;
            }
        }

        /// <summary>
        ///   Gets the selections.
        /// </summary>
        public virtual ISet<IDataQuerySelection> Selections
        {
            get
            {
                return new HashSet<IDataQuerySelection>(this._selections);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get selections for concept.
        /// </summary>
        /// <param name="dimensionId">
        /// The conept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IDataQuerySelection"/> . 
        /// </returns>
        public virtual IDataQuerySelection GetSelectionsForConcept(string dimensionId)
        {
            IDataQuerySelection selection;
            if (dimensionId == null || !this._selectionForConcept.TryGetValue(dimensionId, out selection))
            {
                return null;
            }

            return selection;
        }

        /// <summary>
        /// The has selection for concept.
        /// </summary>
        /// <param name="dimensionId">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public virtual bool HasSelectionForConcept(string dimensionId)
        {
            return dimensionId != null && this._selectionForConcept.ContainsKey(dimensionId);
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            string concat = string.Empty;
            foreach (IDataQuerySelection dqs in this._selections)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1} = (", concat, dqs.ComponentId);
                concat = string.Empty;

                foreach (string val in dqs.Values)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", concat, val);
                    concat = " OR ";
                }

                sb.Append(")");
                concat = " AND ";
            }

            if (this._dateFrom != null)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} Date >= {1}", concat, this._dateFrom.DateInSdmxFormat);
                concat = " AND ";
            }

            if (this._dateTo != null)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} Date <= {1}", concat, this._dateTo.DateInSdmxFormat);
            }

            return sb.ToString();
        }

        #endregion
    }
}