// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureVersionIncrementManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Parse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// Increments the version of any maintainable beans if the bean already exists.  The rules are:
    /// <ul>
    /// <li>If the beans contains a structure that does not yet exist in no action is taken </li>
    /// <li> If the structure already exists then the version number of the SdmxBeans structure is incremented, meaning the existing structure remains unchanged</li>
    /// <li>A minor version increment is performed if the structure does differ in the identifiable composite structures. For example, a codelist with exactly the same codes (a code is identifiable as it has a URN) </li>
    /// <li> A major increment is performed if the submitted structure has an additional identifiable or has had an identifiable removed. For example a codelist may have had a code removed</li>
    /// <li> A minor version increment is a .1 increase. For example 1.2 becomes 1.3 (or 1.3.2 becomes 1.4)</li>
    /// <li> A major version increment is a full integer increase. For example version 1.0 becomes 2.0 (or 1.4 becomes 2.0)</li>
    /// <li> Any structures which cross reference the existing structure will also have a minor version increment and the references will be updated to reference the latest structure.
    /// This rule is recursive, so any structures that reference the references are also updated. This rule will also apply if the referencing document is the same structure submission, as long as the referencing structure in the submission has had the reference modified
    /// </li>
    /// </ul>
    /// </summary>
    public interface IStructureVersionIncrementManager
    {
        
        /// <summary>
        /// Processes the SdmxBeans to determine if any already exist. If they do then
        /// it will determine if there are any changes to the structure, if there are then the submitted structure will have it's
        /// version number automatically incremented.
        /// </summary>
        /// <param name="sdmxObjects"></param>
        void IncrementVersions(ISdmxObjects sdmxObjects);
    }
}
