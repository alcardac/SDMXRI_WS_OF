// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableReferenceMap.cs" company="Eurostat">
//   Date Created : 2013-03-04
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The maintainable reference map.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// The maintainable reference map.
    /// </summary>
    public class MaintainableReferenceDictionary : MaintainableDictionary<IStructureReference>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        public MaintainableReferenceDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public MaintainableReferenceDictionary(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        public MaintainableReferenceDictionary(IDictionaryOfSets<IMaintainableMutableObject, IStructureReference> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainableReferenceDictionary"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected MaintainableReferenceDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}