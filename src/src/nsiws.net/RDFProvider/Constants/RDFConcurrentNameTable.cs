// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RDFConcurrentNameTable.cs" company="ISTAT">
//   TODO
// </copyright>
// <summary>
//   A concurrent version of RDFNameTable. It locks Add methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Constants
{
    using System.Xml;

    /// <summary>
    ///     A concurrent version of NameTable. It locks Add methods
    /// </summary>
    /// <seealso cref="System.Xml.NameTable" />
    public class RDFConcurrentNameTable : NameTable
    {
        #region Fields

        /// <summary>
        ///     This field is used to lock Add methods
        /// </summary>
        private readonly object _lockAdd;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RDFConcurrentNameTable" /> class.
        /// </summary>
        public RDFConcurrentNameTable()
        {
            this._lockAdd = new object();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Atomizes the specified string and adds it to the NameTable.
        /// </summary>
        /// <param name="key">
        /// The string to add.
        /// </param>
        /// <returns>
        /// The atomized string or the existing string if it already exists in the NameTable.
        /// </returns>
        public override string Add(string key)
        {
            lock (this._lockAdd)
            {
                return base.Add(key);
            }
        }

        /// <summary>
        /// Atomizes the specified string and adds it to the NameTable.
        /// </summary>
        /// <param name="key">
        /// The character array containing the string to add.
        /// </param>
        /// <param name="start">
        /// The zero-based index into the array specifying the first character of the string.
        /// </param>
        /// <param name="len">
        /// The number of characters in the string.
        /// </param>
        /// <returns>
        /// The atomized string or the existing string if it already exists in the NameTable.
        /// </returns>
        public override string Add(char[] key, int start, int len)
        {
            lock (this._lockAdd)
            {
                return base.Add(key, start, len);
            }
        }

        #endregion
    }
}