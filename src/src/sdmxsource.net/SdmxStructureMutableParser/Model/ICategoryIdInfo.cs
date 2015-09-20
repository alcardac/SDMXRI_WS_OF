// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryIdInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The CategoryIdInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    /// <summary>
    ///     The CategoryIdInfo interface.
    /// </summary>
    public interface ICategoryIdInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the category id.
        /// </summary>
        ICategoryIdInfo CategoryId { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        string ID { get; set; }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        ICategoryIdInfo Parent { get; set; }

        /// <summary>
        ///     Gets or sets the CategoryRef that reference this Category ID.
        /// </summary>
        IReferenceInfo ReferenceFrom { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        string Version { get; set; }

        #endregion
    }
}