// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The assembler interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    /// <summary>
    /// The assembler interface.
    /// </summary>
    /// <typeparam name="TInto">
    /// The destination type
    /// </typeparam>
    /// <typeparam name="TFrom">
    /// The Source type
    /// </typeparam>
    public interface IAssembler<in TInto, in TFrom>
    {
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
        void Assemble(TInto assembleInto, TFrom assembleFrom);

        #endregion
    }
}