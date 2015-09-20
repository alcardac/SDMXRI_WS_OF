namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    /// <para>
    /// ReferenceType is an abstract base type. It is used as the basis for all references, to all for a top level generic object reference that can be substituted with an explicit reference to any object. Any reference can consist of a Ref (which contains all required reference fields separately) and/or a URN. These must result in the identification of the same object. Note that the Ref and URN elements are local and unqualified in order to allow for refinement of this structure outside of the namespace. This allows any reference to further refined by a different namespace. For example, a metadata structure definition specific metadata set might wish to restrict the URN to only allow for a value from an enumerated list. The general URN structure, for the purpose of mapping the reference fields is as follows: urn:sdmx:org.package-name.class-name=agency-id:(maintainable-parent-object-id[maintainable-parent-object-version].)?(container-object-id.)?object-id([object-version])?.
    /// </para>
    /// <para>
    /// Regular expression: ((@Ref, URN?)|URN)
    /// </para>
    /// </summary>
    public abstract partial class ReferenceType
    {
        /// <summary>
        /// The reference element name
        /// </summary>
        private static readonly XName _referenceName = XName.Get("Ref", string.Empty);

        /// <summary>
        /// <para>
        /// Set the Reference. Ref is used to provide a complete set of reference fields. Derived reference types will restrict the RefType so that the content of the Ref element requires exactly what is needed for a complete reference.
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Setter: Appends
        /// </para>
        /// <para>
        /// Regular expression: ((@Ref, URN?)|URN)
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="RefBaseType"/>
        /// </typeparam>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <paramref name="value"/>
        /// </returns>
        public T SetTypedRef<T>(T value) where T : RefBaseType
        {
            this.SetElement(_referenceName, value);
            return value;
        }

        /// <summary>
        /// <para>
        /// Gets the reference of type <typeparamref name="T"/>. Ref is used to provide a complete set of reference fields. Derived reference types will restrict the RefType so that the content of the Ref element requires exactly what is needed for a complete reference.
        /// </para>
        /// <para>
        /// Occurrence: required
        /// </para>
        /// <para>
        /// Setter: Appends
        /// </para>
        /// <para>
        /// Regular expression: ((@Ref, URN?)|URN)
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="RefBaseType"/> to return
        /// </typeparam>
        /// <returns>
        /// The reference of type <typeparamref name="T"/>
        /// </returns>
        public T GetTypedRef<T>() where T : RefBaseType
        {
            XElement x = this.GetElement(_referenceName);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance);
        }
    }
}