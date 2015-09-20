// -----------------------------------------------------------------------
// <copyright file="CodeListHelper.cs" company="Eurostat">
//   Date Created : 2012-04-06
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Engines
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;

    /// <summary>
    /// This class contains helper method for codelist retrieval engines
    /// </summary>
    internal static class CodeListHelper
    {
        /// <summary>
        /// Gets the first codelist from <paramref name="codeLists"/> if any
        /// </summary>
        /// <param name="codeLists">
        /// The code lists.
        /// </param>
        /// <returns>
        /// The first codelist from <paramref name="codeLists"/>; otherwise null
        /// </returns>
        public static ICodelistMutableObject GetFirstCodeList(ISet<ICodelistMutableObject> codeLists)
        {
            ICodelistMutableObject firstCL = null;
            foreach (ICodelistMutableObject cl in codeLists)
            {
                firstCL = cl;
                break;
            }
            return codeLists != null && codeLists.Count != 0 ? firstCL : null;
        }
    }
}