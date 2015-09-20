// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespacePrefixPair.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class holds a namespace and prefix pair
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Constants
{
    using System;

    /// <summary>
    ///     This class holds a namespace and prefix pair
    /// </summary>
    public class NamespacePrefixPair
    {
        #region Static Fields

        /// <summary>
        ///     The empty
        /// </summary>
        private static readonly NamespacePrefixPair _empty = new NamespacePrefixPair();

        #endregion

        #region Fields

        /// <summary>
        ///     The namespace.
        /// </summary>
        private readonly string _namespace;

        /// <summary>
        ///     The prefix.
        /// </summary>
        private readonly string _prefix;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespacePrefixPair"/> class.
        /// </summary>
        /// <param name="ns">
        /// The namespace
        /// </param>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        public NamespacePrefixPair(Uri ns, string prefix)
        {
            this._namespace = ns != null ? ns.OriginalString : null;
            this._prefix = prefix;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespacePrefixPair"/> class.
        /// </summary>
        /// <param name="ns">
        /// The namespace
        /// </param>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        public NamespacePrefixPair(string ns, string prefix)
            : this(new Uri(ns), prefix)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespacePrefixPair"/> class.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        public NamespacePrefixPair(string prefix)
        {
            this._prefix = prefix;
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="NamespacePrefixPair" /> class from being created.
        /// </summary>
        private NamespacePrefixPair()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets an empty <see cref="NamespacePrefixPair" />
        /// </summary>
        public static NamespacePrefixPair Empty
        {
            get
            {
                return _empty;
            }
        }

        /// <summary>
        ///     Gets the namespace
        /// </summary>
        public string NS
        {
            get
            {
                return this._namespace;
            }
        }

        /// <summary>
        ///     Gets the prefix.
        /// </summary>
        public string Prefix
        {
            get
            {
                return this._prefix;
            }
        }

        #endregion
    }
}