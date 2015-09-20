// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentConstraintXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   attachments the constraint xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///     attachments the constraint xml bean builder.
    /// </summary>
    public class AttachmentConstraintXmlBuilder : ConstraintAssembler,
                                                  IBuilder<AttachmentConstraintType, IAttachmentConstraintObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// builds the from.
        /// </param>
        /// <returns>
        /// The <see cref="AttachmentConstraintType"/>.
        /// </returns>
        public virtual AttachmentConstraintType Build(IAttachmentConstraintObject buildFrom)
        {
            var returnType = new AttachmentConstraintType();

            this.Assemble(returnType, buildFrom);

            return returnType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a <see cref="DataKeySetType"/> to <paramref name="constraintType"/> and return it.
        /// </summary>
        /// <param name="constraintType">
        /// constraints the type.
        /// </param>
        /// <returns>
        /// The <see cref="DataKeySetType"/>.
        /// </returns>
        private static DataKeySetType AddDataKeySetType(ConstraintType constraintType)
        {
            var dataKeySetType = new DataKeySetType();
            constraintType.DataKeySet.Add(dataKeySetType);
            return dataKeySetType;
        }

        /// <summary>
        ///     builds the metadata key set.
        /// </summary>
        private void BuildMetadataKeySet()
        {
            // FUNC 2.1 buildMetadataKeySet
        }

        #endregion
    }
}