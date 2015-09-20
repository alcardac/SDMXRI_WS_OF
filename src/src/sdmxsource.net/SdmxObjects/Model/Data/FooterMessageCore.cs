// -----------------------------------------------------------------------
// <copyright file="FooterMessageCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FooterMessageCore :IFooterMessage
    {
        #region Implementation of IFooterMessage

        private readonly string _code;

        private readonly Severity _severity;

        private readonly IList<ITextTypeWrapper> _footerText;

        public string Code
        {
            get
            {
                return this._code;
            }
        }

        public Severity Severity
        {
            get
            {
                return this._severity;
            }
        }

        public IList<ITextTypeWrapper> FooterText
        {
            get
            {
                return this._footerText;
            }
        }

        #endregion

        public FooterMessageCore(string code, Severity severity, ITextTypeWrapper textType)
        {
            this._code = code;
            this._severity = severity;
            if (code == null)
            {
                throw new ArgumentException("FooterMessage - Code is mandatory");
            }
            if (textType == null)
            {
                throw new ArgumentException("FooterMessage - At least on e text is required");
            }
            this._footerText = new List<ITextTypeWrapper>();
            this._footerText.Add(textType);
        }

        public FooterMessageCore(string code, Severity severity, IList<ITextTypeWrapper> textType)
        {
            this._code = code;
            this._severity = severity;
            this._footerText = textType;
            if (code == null)
            {
                throw new ArgumentException("FooterMessage - Code is mandatory");
            }
            if (textType == null || textType.Count == 0)
            {
                throw new ArgumentException("FooterMessage - At least on e text is required");
            }
        }
    }
}
