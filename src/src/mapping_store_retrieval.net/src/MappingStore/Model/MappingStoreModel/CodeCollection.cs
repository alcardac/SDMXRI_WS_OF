// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeCollection.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A string based <see cref="Collection{T}" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A string based <see cref="Collection{T}"/>
    /// </summary>
    public class CodeCollection : Collection<string>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCollection"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.Collection`1"/> class that is empty.
        /// </summary>
        public CodeCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCollection"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.Collection`1"/> class as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">
        /// The list that is wrapped by the new collection.
        ///                 </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="list"/> is null.
        /// </exception>
        public CodeCollection(IList<string> list)
            : base(list)
        {
        }

        #endregion
    }
}