// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFooterMessage.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// Provides extra detail on close, used to inform users of things such as truncated messages, or errors while processing.
    /// </summary>
    public interface IFooterMessage
    {
        /// <summary>
        /// Mandatory Field - use to describe the error/warning
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Optional - describes severity of problem
        /// </summary>
        Severity Severity { get; }

        /// <summary>
        /// Any text associated with the footer, there must be at least one text message
        /// </summary>
        IList<ITextTypeWrapper> FooterText { get; }
    }
}
