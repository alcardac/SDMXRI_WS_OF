﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossReferenceTreeMutable.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICrossReferenceTreeMutable
    {
        /// <summary>
        /// 
        /// </summary>
        IMaintainableMutableObject MaintianableObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IList<ICrossReferenceTreeMutable> ReferencingObjects { get; }
    }
}
