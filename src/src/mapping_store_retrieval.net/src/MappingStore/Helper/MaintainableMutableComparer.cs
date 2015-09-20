// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableMutableComparer.cs" company="Eurostat">
//   Date Created : 2013-03-04
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <see cref="IMaintainableMutableObject" /> comparer
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The <see cref="IMaintainableMutableObject" /> comparer
    /// </summary>
    public class MaintainableMutableComparer : IEqualityComparer<IMaintainableMutableObject>
    {
        #region Static Fields

        /// <summary>
        ///     The from mutable structure reference builder.
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        /// <summary>
        /// The _instance.
        /// </summary>
        private static readonly MaintainableMutableComparer _instance = new MaintainableMutableComparer();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="MaintainableMutableComparer" /> class from being created.
        /// </summary>
        private MaintainableMutableComparer()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MaintainableMutableComparer Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">
        /// The first object of type <see cref="IMaintainableMutableObject"/> to compare.
        /// </param>
        /// <param name="y">
        /// The second object of type <see cref="IMaintainableMutableObject"/> to compare.
        /// </param>
        public bool Equals(IMaintainableMutableObject x, IMaintainableMutableObject y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            IStructureReference firstReference = _fromMutable.Build(x);
            IStructureReference secondReference = _fromMutable.Build(y);
            return firstReference.Equals(secondReference);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> for which a hash code is to be returned.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        /// </exception>
        public int GetHashCode(IMaintainableMutableObject obj)
        {
            return _fromMutable.Build(obj).GetHashCode();
        }

        #endregion
    }
}