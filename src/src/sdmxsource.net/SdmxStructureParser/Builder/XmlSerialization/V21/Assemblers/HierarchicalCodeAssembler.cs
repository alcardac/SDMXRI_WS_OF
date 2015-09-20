// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodeAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical code bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The hierarchical code bean assembler.
    /// </summary>
    public class HierarchicalCodeAssembler : IdentifiableAssembler, IAssembler<HierarchicalCodeType, IHierarchicalCode>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Assemble from <paramref name="assembleFrom"/> to <paramref name="assembleInto"/>.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(HierarchicalCodeType assembleInto, IHierarchicalCode assembleFrom)
        {
            var codes = new Stack<KeyValuePair<HierarchicalCodeType, IHierarchicalCode>>();
            codes.Push(new KeyValuePair<HierarchicalCodeType, IHierarchicalCode>(assembleInto, assembleFrom));
            while (codes.Count > 0)
            {
                KeyValuePair<HierarchicalCodeType, IHierarchicalCode> pair = codes.Pop();
                assembleInto = pair.Key;
                assembleFrom = pair.Value;

                // Populate it from inherited super
                this.AssembleIdentifiable(assembleInto, assembleFrom);
                if (assembleFrom.CodelistAliasRef != null)
                {
                    assembleInto.CodelistAliasRef = assembleFrom.CodelistAliasRef;
                    var localRef = new LocalCodeReferenceType();
                    assembleInto.CodeID = localRef;
                    var xref = new LocalCodeRefType();
                    localRef.SetTypedRef(xref);
                    xref.id = assembleFrom.CodeId;
                }
                else
                {
                    ICrossReference crossReference = assembleFrom.CodeReference;
                    var code = new CodeReferenceType();
                    assembleInto.Code = code;
                    var codeRefType = new CodeRefType();
                    code.SetTypedRef(codeRefType);

                    this.SetReference(codeRefType, crossReference);
                }

                // Children
                foreach (IHierarchicalCode eachCodeRefBean in assembleFrom.CodeRefs)
                {
                    var eachHierarchicalCode = new HierarchicalCodeType();
                    assembleInto.HierarchicalCode.Add(eachHierarchicalCode);
                    codes.Push(
                        new KeyValuePair<HierarchicalCodeType, IHierarchicalCode>(eachHierarchicalCode, eachCodeRefBean));

                    //// this.Assemble(eachHierarchicalCode, eachCodeRefBean);
                }

                // LEVEL
                if (assembleFrom.GetLevel(false) != null)
                {
                    var ref0 = new LocalLevelReferenceType();
                    assembleInto.Level = ref0;
                    assembleInto.Level.SetTypedRef(
                        new LocalLevelRefType { id = assembleFrom.GetLevel(false).GetFullIdPath(false) });
                }
            }
        }

        #endregion
    }
}