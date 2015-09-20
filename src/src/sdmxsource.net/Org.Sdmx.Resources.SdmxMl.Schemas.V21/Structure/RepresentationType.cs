// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationType.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   <para>
//   RepresentationType is an abstract type that defines a representation. Because the type of item schemes that are allowed as the an enumeration vary based on the object in which this is defined, this type is abstract to force that the enumeration reference be restricted to the proper type of item scheme reference.
//   </para>
//   <para>
//   Regular expression: (TextFormat | (Enumeration, EnumerationFormat?))
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    ///     <para>
    ///         RepresentationType is an abstract type that defines a representation. Because the type of item schemes that are allowed as the an enumeration vary based on the object in which this is defined, this type is abstract to force that the enumeration reference be restricted to the proper type of item scheme reference.
    ///     </para>
    ///     <para>
    ///         Regular expression: (TextFormat | (Enumeration, EnumerationFormat?))
    ///     </para>
    /// </summary>
    public abstract partial class RepresentationType
    {
        #region Static Fields

        /// <summary>
        ///     The reference element name
        /// </summary>
        private static readonly XName _referenceName = XName.Get(
            "Enumeration", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure");

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the codelist enumeration.
        /// </summary>
        public CodelistReferenceType CodelistEnumeration
        {
            get
            {
                return this.GetTypedEnumeration<CodelistReferenceType>();
            }
        }

        /// <summary>
        /// Gets the concept scheme enumeration.
        /// </summary>
        public ConceptSchemeReferenceType ConceptSchemeEnumeration
        {
            get
            {
                return this.GetTypedEnumeration<ConceptSchemeReferenceType>();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Add new <see cref="TextFormatType" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="TextFormatType" />.
        /// </returns>
        public virtual TextFormatType AddNewTextFormatType()
        {
            var basicComponentTextFormatType = new BasicComponentTextFormatType();
            this.TextFormat = basicComponentTextFormatType;
            return basicComponentTextFormatType;
        }

        /// <summary>
        ///     <para>
        ///         Enumeration references an item scheme that enumerates the allowable values for this representation.
        ///     </para>
        ///     <para>
        ///         Occurrence: required
        ///     </para>
        ///     <para>
        ///         Setter: Appends
        ///     </para>
        ///     <para>
        ///         Regular expression: (TextFormat | (Enumeration, EnumerationFormat?))
        ///     </para>
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the return value
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public T GetTypedEnumeration<T>() where T : ItemSchemeReferenceBaseType
        {
            XElement x = this.GetElement(_referenceName);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }

        /// <summary>
        /// <para>
        /// Enumeration references an item scheme that enumerates the allowable values for this representation.
        ///     </para>
        /// <para>
        /// Occurrence: required
        ///     </para>
        /// <para>
        /// Setter: Appends
        ///     </para>
        /// <para>
        /// Regular expression: (TextFormat | (Enumeration, EnumerationFormat?))
        ///     </para>
        /// </summary>
        /// <typeparam name="T">
        /// The type of value
        /// </typeparam>
        /// <param name="value">
        /// The value.
        /// </param>
        public void SetTypedEnumeration<T>(T value) where T : ItemSchemeReferenceBaseType
        {
            this.SetElement(_referenceName, value);
        }

        #endregion
    }
}