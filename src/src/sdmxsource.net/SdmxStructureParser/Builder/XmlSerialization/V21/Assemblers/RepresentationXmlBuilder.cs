// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The representation xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The representation xml bean builder.
    /// </summary>
    /// <typeparam name="TRepresenation">
    /// The concrete representation type
    /// </typeparam>
    public class RepresentationXmlBuilder<TRepresenation> : AbstractAssembler, 
                                                            IAssembler<TRepresenation, IRepresentation>
        where TRepresenation : RepresentationType, new()
    {
        #region Fields

        /// <summary>
        ///     The text format assembler.
        /// </summary>
        private readonly TextFormatAssembler _textFormatAssembler = new TextFormatAssembler();

        #endregion

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
        public virtual void Assemble(TRepresenation assembleInto, IRepresentation assembleFrom)
        {
            if (assembleFrom.Representation != null)
            {
                var itemSchemeRefType = new ItemSchemeReferenceType();
                assembleInto.Enumeration = itemSchemeRefType;
                var schemeRefType = new ItemSchemeRefType();
                itemSchemeRefType.SetTypedRef(schemeRefType);
                this.SetReference(schemeRefType, assembleFrom.Representation);
                if (assembleFrom.TextFormat != null)
                {
                    var codededTextFormatType = new CodededTextFormatType();
                    this._textFormatAssembler.Assemble(codededTextFormatType, assembleFrom.TextFormat);
                    assembleInto.EnumerationFormat = codededTextFormatType;
                }
            }
            else if (assembleFrom.TextFormat != null)
            {
                this._textFormatAssembler.Assemble(assembleInto.AddNewTextFormatType(), assembleFrom.TextFormat);
            }
        }

        #endregion
    }
}