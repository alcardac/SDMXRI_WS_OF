// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The level assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;

    /// <summary>
    ///     The level assembler.
    /// </summary>
    public class LevelAssembler : NameableAssembler, IAssembler<LevelType, ILevelObject>
    {
        #region Fields

        /// <summary>
        ///     The text format assembler.
        /// </summary>
        private readonly TextFormatAssembler _textFormatAssembler = new TextFormatAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The assemble method. It assembles from <paramref name="assembleFrom"/> into <paramref name="assembleInto"/>
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into XSD object.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from SDMX Object.
        /// </param>
        public virtual void Assemble(LevelType assembleInto, ILevelObject assembleFrom)
        {
            var levels = new Stack<KeyValuePair<LevelType, ILevelObject>>();
            levels.Push(new KeyValuePair<LevelType, ILevelObject>(assembleInto, assembleFrom));

            while (levels.Count > 0)
            {
                KeyValuePair<LevelType, ILevelObject> pair = levels.Pop();
                assembleInto = pair.Key;
                assembleFrom = pair.Value;

                this.AssembleNameable(assembleInto, assembleFrom);
                if (assembleFrom.CodingFormat != null)
                {
                    var codingTextFormatType = new CodingTextFormatType();
                    assembleInto.CodingFormat = codingTextFormatType;
                    this._textFormatAssembler.Assemble(codingTextFormatType, assembleFrom.CodingFormat);
                }

                if (assembleFrom.ChildLevel != null)
                {
                    var levelType = new LevelType();
                    assembleInto.Level = levelType;
                    levels.Push(new KeyValuePair<LevelType, ILevelObject>(levelType, assembleFrom.ChildLevel));

                    ////this.Assemble(levelType, assembleFrom.ChildLevel);
                }
            }
        }

        #endregion
    }
}