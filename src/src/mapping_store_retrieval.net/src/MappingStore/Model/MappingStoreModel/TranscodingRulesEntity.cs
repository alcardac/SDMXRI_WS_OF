// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranscodingRulesEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class contains various dictionaries to speed up transcoding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    /// <summary>
    /// This class contains various dictionaries to speed up transcoding.
    /// </summary>
    public class TranscodingRulesEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The separator used in the <see cref="ConvertToString"/> method
        /// </summary>
        private const string Separator = ",";

        /// <summary>
        /// A dictionary that holds the column as key position
        /// </summary>
        private readonly Dictionary<long, int> _columnAsKeyPosition;

        /// <summary>
        /// A dictionary that holds the components to key positions
        /// </summary>
        private readonly Dictionary<long, int> _componentAsKeyPosition;

        /// <summary>
        /// A dictionary that holds the dsd codes to local codes transcoding.
        /// The key is the combination of dsd codes from each dsd component used in the mapping
        /// and the value is a list of the corresponding local code sets.
        /// </summary>
        private readonly Dictionary<CodeCollection, CodeSetCollection> _dsdCodesAsKey;

        /// <summary>
        /// A dictionary that holds the local codes to multiple local codes transcoding.
        /// </summary>
        private readonly Dictionary<CodeCollection, CodeCollection> _localCodesAsKey;

        /// <summary>
        /// A dictionary that holds the transcoding of many components to one column
        /// </summary>
        private readonly List<Dictionary<string, List<string>>> _manyComponentToOneColumn;

        #endregion

        /*
        /// <summary>
        /// A dictionary that holds the key position as column
        /// </summary>
        private readonly Dictionary<int, string> _positionAsKeyColumn;

        /// <summary>
        /// A dictionary that holds the key positions as components
        /// </summary>
        private readonly Dictionary<int, string> _positionAsKeyComponent;
*/
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranscodingRulesEntity"/> class. 
        /// Default construtor used to initialize the private dictionaries of the class
        /// </summary>
        public TranscodingRulesEntity()
            : base(0)
        {
            this._localCodesAsKey = new Dictionary<CodeCollection, CodeCollection>(StringCollectionComparer.Instance);
            this._dsdCodesAsKey = new Dictionary<CodeCollection, CodeSetCollection>(StringCollectionComparer.Instance);
            this._componentAsKeyPosition = new Dictionary<long, int>();

            ////            this._positionAsKeyComponent = new Dictionary<int, string>();
            this._columnAsKeyPosition = new Dictionary<long, int>();

            //// this._positionAsKeyColumn = new Dictionary<int, string>();
            this._manyComponentToOneColumn = new List<Dictionary<string, List<string>>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the transcoding dictionary for columns as positions
        /// </summary>
        public Dictionary<long, int> ColumnAsKeyPosition
        {
            get
            {
                return this._columnAsKeyPosition;
            }
        }

        /// <summary>
        /// Gets the transcoding dictionary for component as key position
        /// </summary>
        public Dictionary<long, int> ComponentAsKeyPosition
        {
            get
            {
                return this._componentAsKeyPosition;
            }
        }

        /// <summary>
        /// Gets or sets the period of the codelist
        /// </summary>
        public string PeriodCodelist { get; set; }

        #endregion

        /*
 * Not used anywhere
        /// <summary>
        /// Gets the transcoding dictionary for positions as columns
        /// </summary>
        public Dictionary<int, string> PositionAsKeyColumn
        {
            get
            {
                return this._positionAsKeyColumn;
            }
        }

        /// <summary>
        /// Gets the transcoding dictionary for positions as key component
        /// </summary>
        public Dictionary<int, string> PositionAsKeyComponent
        {
            get
            {
                return this._positionAsKeyComponent;
            }
        }
*/
        #region Public Methods

        /// <summary>
        /// The method is used to add a new component transcoding into 
        /// the coresponding dictionaries
        /// </summary>
        /// <param name="localCodes">
        /// The list local codes
        /// </param>
        /// <param name="dsdCodes">
        /// The list of data structure definition codes
        /// </param>
        public void Add(CodeCollection localCodes, CodeCollection dsdCodes)
        {
            if (this._localCodesAsKey.ContainsKey(localCodes))
            {
                string localCodesKey = ConvertToString(localCodes);
                string dsdCodesKey = ConvertToString(dsdCodes);
                throw new ArgumentException(
                    "Duplicate code mapping found.\nLocal code(s) : " + localCodesKey
                    + " mapped to two or more sets of SDMX Codes.\n Set 1 :"
                    + ConvertToString(this._localCodesAsKey[localCodes]) + "\n Set 2: " + dsdCodesKey);
            }

            CodeSetCollection localCodesSet;
            if (!this._dsdCodesAsKey.TryGetValue(dsdCodes, out localCodesSet))
            {
                localCodesSet = new CodeSetCollection();
                this._dsdCodesAsKey.Add(dsdCodes, localCodesSet);
            }

            localCodesSet.Add(localCodes);

            // NON MAT200
            // _dsdCodesAsKey.Add(dsdCodesKey, localCodes);
            this._localCodesAsKey.Add(localCodes, dsdCodes);

            // for N comp to 1 col
            for (int i = 0; i < dsdCodes.Count; i++)
            {
                List<string> list;
                if (!this._manyComponentToOneColumn[i].TryGetValue(dsdCodes[i], out list))
                {
                    list = new List<string>();
                    this._manyComponentToOneColumn[i].Add(dsdCodes[i], list);
                }

                list.Add(localCodes[0]);
            }

            // }
        }

        /// <summary>
        /// The method is used to add a new column transcoding into 
        /// <see cref="TranscodingRulesEntity._columnAsKeyPosition"/>
        /// </summary>
        /// <param name="name">
        /// The name of the column
        /// </param>
        /// <param name="pos">
        /// The position of the column
        /// </param>
        public void AddColumn(long name, int pos)
        {
            this._columnAsKeyPosition.Add(name, pos);
        }

        /// <summary>
        /// The method is used to add a new component transcoding into 
        /// <see cref="TranscodingRulesEntity._componentAsKeyPosition"/>
        /// and <see cref="TranscodingRulesEntity._manyComponentToOneColumn"/>
        /// </summary>
        /// <param name="name">
        /// The name of the component
        /// </param>
        /// <param name="pos">
        /// The position of the component
        /// </param>
        public void AddComponent(long name, int pos)
        {
            this._componentAsKeyPosition.Add(name, pos);
            this._manyComponentToOneColumn.Add(new Dictionary<string, List<string>>(StringComparer.Ordinal));
        }

        /// <summary>
        /// The metod is used to retrieve the list of data structure definition codes 
        /// for a list of local codes
        /// </summary>
        /// <param name="localCodes">
        /// The list of local codes
        /// </param>
        /// <returns>
        /// The list of data structure definition codes
        /// </returns>
        public CodeCollection GetDsdCodes(CodeCollection localCodes)
        {
            CodeCollection codes;

            if (!this._localCodesAsKey.TryGetValue(localCodes, out codes))
            {
                codes = null;
            }

            return codes;
        }

        /// <summary>
        /// The method is used to retrieve the local codes for a list 
        /// of data structure definition list codes.
        /// </summary>
        /// <param name="dsdCodes">
        /// The data structure definition list
        /// </param>
        /// <returns>
        /// The list of local codes
        /// </returns>
        public CodeSetCollection GetLocalCodes(CodeCollection dsdCodes)
        {
            CodeSetCollection codes;
            if (!this._dsdCodesAsKey.TryGetValue(dsdCodes, out codes))
            {
                codes = new CodeSetCollection { dsdCodes };
            }

            return codes;
        }

        // NON MAT200
        // public List<string> GetLocalCodes(List<string> dsdCodes)
        // {
        // string key = String.Join(",",dsdCodes.ToArray());
        // List <string> codes;
        // if (!_dsdCodesAsKey.TryGetValue(key,out codes))
        // {
        // codes = dsdCodes;
        // }
        // return codes;
        // }

        /// <summary>
        /// The method is used to get the local codes of a component
        /// </summary>
        /// <param name="component">
        /// The searched component
        /// </param>
        /// <param name="dsdCode">
        /// The searched data structure definition component
        /// </param>
        /// <returns>
        /// The list of local codes
        /// </returns>
        public IEnumerable<string> GetLocalCodes(long component, string dsdCode)
        {
            List<string> localCodes;
            if (
                !this._manyComponentToOneColumn[this._componentAsKeyPosition[component]].TryGetValue(
                    dsdCode, out localCodes))
            {
                localCodes = new List<string> { dsdCode };
            }

            return localCodes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert a collection to string similar to <see cref="string.Join(string,string[])"/>
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// A string with all collection items separated by <see cref="Separator"/>
        /// </returns>
        private static string ConvertToString(IEnumerable<string> collection)
        {
            var sb = new StringBuilder();
            foreach (string s in collection)
            {
                sb.Append(s).Append(Separator);
            }

            sb.Length--;
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// The string collection comparer.
        /// </summary>
        private class StringCollectionComparer : IEqualityComparer<Collection<string>>, 
                                                 IEqualityComparer<CodeCollection>
        {
            #region Constants and Fields

            /// <summary>
            /// The _instance.
            /// </summary>
            private static readonly StringCollectionComparer _instance = new StringCollectionComparer();

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Prevents a default instance of the <see cref="StringCollectionComparer"/> class from being created.
            /// </summary>
            private StringCollectionComparer()
            {
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets Instance.
            /// </summary>
            public static StringCollectionComparer Instance
            {
                get
                {
                    return _instance;
                }
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Checks if two collection, <paramref name="x"/> and <paramref name="y"/> are equal regarding their contents.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// <c>true</c> if the collections are equal. Else <c>false</c>.
            /// </returns>
            public bool Equals(Collection<string> x, Collection<string> y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null)
                {
                    return false;
                }

                if (x.Count != y.Count)
                {
                    return false;
                }

                for (int i = 0; i < x.Count; i++)
                {
                    if (!string.Equals(x[i], y[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Checks if two collection, <paramref name="x"/> and <paramref name="y"/> are equal regarding their contents
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// <c>true</c> if the collections are equal. Else <c>false</c>.
            /// </returns>
            public bool Equals(CodeCollection x, CodeCollection y)
            {
                return this.Equals((Collection<string>)x, y);
            }

            /// <summary>
            /// Gets the hash code from the collections contents
            /// </summary>
            /// <param name="obj">
            /// The collection to get the hash code from
            /// </param>
            /// <returns>
            /// The hash code
            /// </returns>
            public int GetHashCode(Collection<string> obj)
            {
                if (obj.Count == 1)
                {
                    return obj[0].GetHashCode();
                }

                int hashCode = 10;
                foreach (string s in obj)
                {
                    hashCode = hashCode * 33 ^ s.GetHashCode();
                }

                return hashCode;
            }

            /// <summary>
            /// Gets the hash code from the collections contents
            /// </summary>
            /// <param name="obj">
            /// The collection to get the hash code from
            /// </param>
            /// <returns>
            /// The hash code
            /// </returns>
            public int GetHashCode(CodeCollection obj)
            {
                return this.GetHashCode((Collection<string>)obj);
            }

            #endregion
        }
    }
}