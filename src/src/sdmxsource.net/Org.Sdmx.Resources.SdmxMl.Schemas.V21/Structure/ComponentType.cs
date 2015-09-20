// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentType.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The component type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    ///     The component type.
    /// </summary>
    public partial class ComponentType
    {
        #region Static Fields

        /// <summary>
        ///     The reference element name
        /// </summary>
        private static readonly XName _referenceName = XName.Get(
            "LocalRepresentation", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure");

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     <para>
        ///         Gets the LocalRepresentation references item schemes that may be used to create the representation of a component. The type of this must be refined such that a concrete item scheme reference is used.
        ///     </para>
        ///     <para>
        ///         Occurrence: optional
        ///     </para>
        ///     <para>
        ///         Regular expression: (Annotations?, ConceptIdentity?, LocalRepresentation?)
        ///     </para>
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the return value
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public T GetTypedLocalRepresentation<T>() where T : RepresentationType
        {
            XElement x = this.GetElement(_referenceName);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }

        /// <summary>
        /// <para>
        /// Sets the LocalRepresentation references item schemes that may be used to create the representation of a component. The type of this must be refined such that a concrete item scheme reference is used.
        ///     </para>
        /// <para>
        /// Occurrence: optional
        ///     </para>
        /// <para>
        /// Regular expression: (Annotations?, ConceptIdentity?, LocalRepresentation?)
        ///     </para>
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="value"/>
        /// </typeparam>
        /// <param name="value">
        /// The value.
        /// </param>
        public void SetTypedLocalRepresentation<T>(T value) where T : RepresentationType
        {
            this.SetElement(_referenceName, value);
        }

        #endregion
    }
}