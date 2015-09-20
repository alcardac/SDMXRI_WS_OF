// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxMutablePersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist.Mutable
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     Interface to perform save and delete operations on Mutable Objects.
    /// </summary>
    public interface ISdmxMutablePersistenceManager
    {
        // Save Methods

        // Delete Methods
        #region Public Methods and Operators

        /// <summary>
        /// Deletes the maintainable - returns the deleted maintainable
        ///     <p/>
        ///     The Object is validated and the saved instance is returned as MaintainableMutableObject
        ///     <p/>
        ///     <b>NOTE :</b> Certain attributes may have been automatically generated by the system regardless of what was
        ///     supplied, and example is URN which is system generated and any supplied value is ignored
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// MaintainableMutableObject as a representation of what was deleted - with values such as URN generated by the system
        /// </returns>
        IMaintainableMutableObject DeleteMaintainable(IMaintainableMutableObject maintainable);

        /// <summary>
        /// Delete the specified <paramref name="maintainable"/>.
        /// </summary>
        /// <param name="maintainable">
        /// The set maintainable objects.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> as a representation of what was saved - with values such as URN generated by the system.
        /// </returns>
        ISet<IMaintainableMutableObject> DeleteMaintainables(ISet<IMaintainableMutableObject> maintainable);

        /// <summary>
        /// Saves the maintainable mutable Object
        ///     <p/>
        ///     The Object is validated and the saved instance is returned as a MaintainableMutableObject.
        ///     <p/>
        ///     <b>NOTE :</b>Certain attributes may have been automatically generated by the system regardless of what was
        ///     supplied, and example is URN which is system generated and any supplied value is ignored
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// MaintainableMutableObject as a representation of what was saved - with values such as URN generated by the system
        /// </returns>
        IMaintainableMutableObject SaveMaintainable(IMaintainableMutableObject maintainable);

        /// <summary>
        ///  Save the specified <paramref name="maintainable"/>.
        /// </summary>
        /// <param name="maintainable">
        /// The set maintainable objects.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/> as a representation of what was saved - with values such as URN generated by the system.
        /// </returns>
        ISet<IMaintainableMutableObject> SaveMaintainables(ISet<IMaintainableMutableObject> maintainable);

        #endregion
    }
}