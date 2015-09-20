// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstrainableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     A constrainable @object is one which can have constraints associated with it.
    ///     <p />
    ///     Constraints are not `by composition` but `by reference` therefore a constraint can only reference a constrainable artifact,
    ///     a constrainable artifact does not contain the constraint.
    /// </summary>
    public interface IConstrainableObject : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the cross referenced constrainables.
        /// <li>Registration would return reference to a provision agreement</li>
        /// <li>Provision Agreement would return a reference to a dataflow and a Data Provider</li>
        /// <li>Dataflow would return a reference to a data structure</li>
        /// <li>Data Structure would return null</li>
        /// (example a provision agreement would return a reference to a dataflow) 
        /// </summary>
        IList<ICrossReference> CrossReferencedConstrainables { get; }

        #endregion
    }
}