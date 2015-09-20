// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingCategoryAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting category bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    using ReportingCategory = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ReportingCategory;

    /// <summary>
    ///     The reporting category bean assembler.
    /// </summary>
    public class ReportingCategoryAssembler : MaintainableAssembler, 
                                              IAssembler<ReportingCategoryType, IReportingCategoryObject>
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
        public virtual void Assemble(ReportingCategoryType assembleInto, IReportingCategoryObject assembleFrom)
        {
            var stack = new Stack<KeyValuePair<ReportingCategoryType, IReportingCategoryObject>>();
            stack.Push(new KeyValuePair<ReportingCategoryType, IReportingCategoryObject>(assembleInto, assembleFrom));

            while (stack.Count > 0)
            {
                KeyValuePair<ReportingCategoryType, IReportingCategoryObject> pair = stack.Pop();
                assembleInto = pair.Key;
                assembleFrom = pair.Value;

                // Populate it from inherited super - NOTE downgraded to identifiable from nameable
                this.AssembleNameable(assembleInto, assembleFrom);
                if (ObjectUtil.ValidCollection(assembleFrom.StructuralMetadata))
                {
                    /* foreach */
                    foreach (ICrossReference crossReference in assembleFrom.StructuralMetadata)
                    {
                        var structureReferenceType = new StructureReferenceType();
                        assembleInto.StructuralMetadata.Add(structureReferenceType);
                        var structureRefType = new StructureRefType();
                        structureReferenceType.SetTypedRef(structureRefType);
                        this.SetReference(structureRefType, crossReference);
                    }
                }

                if (ObjectUtil.ValidCollection(assembleFrom.ProvisioningMetadata))
                {
                    /* foreach */
                    foreach (ICrossReference crossReference in assembleFrom.ProvisioningMetadata)
                    {
                        var structureUsageReferenceType = new StructureUsageReferenceType();
                        assembleInto.ProvisioningMetadata.Add(structureUsageReferenceType);
                        var structureUsageRefType = new StructureUsageRefType();
                        structureUsageReferenceType.SetTypedRef(structureUsageRefType);
                        this.SetReference(structureUsageRefType, crossReference);
                    }
                }

                /* foreach */
                foreach (IReportingCategoryObject eachReportingCategoryBean in assembleFrom.Items)
                {
                    var eachReportingCategory = new ReportingCategory();
                    assembleInto.Item.Add(eachReportingCategory);
                    stack.Push(
                        new KeyValuePair<ReportingCategoryType, IReportingCategoryObject>(
                            eachReportingCategory.Content, eachReportingCategoryBean));

                    ////this.Assemble(eachReportingCategory.Content, eachReportingCategoryBean);
                }
            }
        }

        #endregion
    }
}