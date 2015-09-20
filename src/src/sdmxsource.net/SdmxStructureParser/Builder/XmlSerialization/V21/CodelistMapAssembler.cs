// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist map bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    using CodeMap = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.CodeMap;

    /// <summary>
    ///     The codelist map bean assembler.
    /// </summary>
    public class CodelistMapAssembler : AbstractSchemeMapAssembler, IAssembler<CodelistMapType, ICodelistMapObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(CodelistMapType assembleInto, ICodelistMapObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleMap(assembleInto, assembleFrom);

            // Child maps
            foreach (IItemMap eachMapBean in assembleFrom.Items)
            {
                // Defer child creation to subclass
                var newMap = new CodeMap();
                assembleInto.ItemAssociation.Add(newMap);

                // Common source and target id allocation
                var sourceCodeRef = new LocalCodeReferenceType();
                sourceCodeRef.SetTypedRef(new LocalCodeRefType { id = eachMapBean.SourceId });
                newMap.Source = sourceCodeRef;
                var targetCodeRef = new LocalCodeReferenceType();
                targetCodeRef.SetTypedRef(new LocalCodeRefType { id = eachMapBean.TargetId });
                newMap.Target = targetCodeRef;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create the ref. base type
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeRefBaseType"/>.
        /// </returns>
        protected override ItemSchemeRefBaseType CreateRef(ItemSchemeReferenceBaseType assembleInto)
        {
            var refType = new CodelistRefType();
            assembleInto.SetTypedRef(refType);
            return refType;
        }

        /// <summary>
        /// Create the source reference.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeReferenceBaseType"/>.
        /// </returns>
        protected override ItemSchemeReferenceBaseType CreateSourceReference(ItemSchemeMapType assembleInto)
        {
            var source = new CodelistReferenceType();
            assembleInto.Source = source;

            return source;
        }

        /// <summary>
        /// Create the target reference.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeReferenceBaseType"/>.
        /// </returns>
        protected override ItemSchemeReferenceBaseType CreateTargetReference(ItemSchemeMapType assembleInto)
        {
            var targetReference = new CodelistReferenceType();
            assembleInto.Target = targetReference;

            return targetReference;
        }

        #endregion
    }
}