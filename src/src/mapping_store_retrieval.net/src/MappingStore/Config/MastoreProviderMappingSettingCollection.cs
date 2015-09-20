// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MastoreProviderMappingSettingCollection.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1.
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Collection of <see cref="MastoreProviderMappingSetting" /> elements.
//   They are used to store database related configuration, not for connecting but provider and some behavior
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Config
{
    using System.Collections.Generic;
    using System.Configuration;

    /// <summary>
    /// Collection of <see cref="MastoreProviderMappingSetting"/> elements.
    /// They are used to store database related configuration, not for connecting but provider and some behavior
    /// </summary>
    public class MastoreProviderMappingSettingCollection : ConfigurationElementCollection, ICollection<MastoreProviderMappingSetting>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MastoreProviderMappingSettingCollection"/> class and specify the defaults. 
        /// </summary>
        public MastoreProviderMappingSettingCollection()
        {
            this.Add(
                new MastoreProviderMappingSetting
                    {
                        Name = MappingStoreDefaultConstants.SqlServerName, 
                        Provider = MappingStoreDefaultConstants.SqlServerProvider 
                    });
            this.Add(
                new MastoreProviderMappingSetting
                    {
                        Name = MappingStoreDefaultConstants.OracleName, 
                        Provider = MappingStoreDefaultConstants.OracleProviderOdp 
                    });
            this.Add(
                new MastoreProviderMappingSetting
                    {
                        Name = MappingStoreDefaultConstants.MySqlName, 
                        Provider = MappingStoreDefaultConstants.MySqlProvider 
                    });
            this.Add(
                new MastoreProviderMappingSetting
                    {
                        Name = MappingStoreDefaultConstants.PCAxisName, 
                        Provider = MappingStoreDefaultConstants.PCAxisProvider
                    });
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public new bool IsReadOnly
        {
            get
            {
                return this.IsReadOnly();
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Gets or sets the <see cref="MastoreProviderMappingSetting"/> at the specified <paramref name="index"/>
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>The <see cref="MastoreProviderMappingSetting"/> at the specified <paramref name="index"/></returns>
        public MastoreProviderMappingSetting this[int index]
        {
            get
            {
                return (MastoreProviderMappingSetting)this.BaseGet(index);
            }

            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="MastoreProviderMappingSetting"/> at the specified <paramref name="name"/>
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>The <see cref="MastoreProviderMappingSetting"/> at the specified <paramref name="name"/></returns>
        public new MastoreProviderMappingSetting this[string name]
        {
            get
            {
                return (MastoreProviderMappingSetting)this.BaseGet(name);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add <see cref="MastoreProviderMappingSetting"/> to this collection
        /// </summary>
        /// <param name="item">
        /// The <see cref="MastoreProviderMappingSetting"/>.
        /// </param>
        public void Add(MastoreProviderMappingSetting item)
        {
            this.BaseAdd(item, false);
        }

        /// <summary>
        /// Clear all <see cref="MastoreProviderMappingSetting"/>
        /// </summary>
        public void Clear()
        {
            this.BaseClear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        public bool Contains(MastoreProviderMappingSetting item)
        {
            return this.BaseIndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        ///                 </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        ///                 </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array"/> is null.
        ///                 </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        ///                 </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="array"/> is multidimensional.
        ///                     -or-
        ///                 <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///                     -or-
        ///                     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(MastoreProviderMappingSetting[] array, int arrayIndex)
        {
            var elements = new ConfigurationElement[array.Length];
            array.CopyTo(elements, 0);
            this.CopyTo(elements, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<MastoreProviderMappingSetting> IEnumerable<MastoreProviderMappingSetting>.GetEnumerator()
        {
            return (IEnumerator<MastoreProviderMappingSetting>)this.GetEnumerator();
        }

        /// <summary>
        /// Remove the specified <paramref name="item"/>
        /// </summary>
        /// <param name="item">
        /// The <see cref="MastoreProviderMappingSetting"/> to remove
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="item"/> was removed. Otherwise <c>false</c>
        /// </returns>
        public bool Remove(MastoreProviderMappingSetting item)
        {
            int index = this.BaseIndexOf(item);
            if (index >= 0)
            {
                this.BaseRemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove the <see cref="MastoreProviderMappingSetting"/> with the specified <paramref name="name"/>
        /// </summary>
        /// <param name="name">
        /// The <see cref="MastoreProviderMappingSetting.Name"/> of the <see cref="MastoreProviderMappingSetting"/> to remove
        /// </param>
        public void Remove(string name)
        {
            this.BaseRemove(name);
        }

        /// <summary>
        /// Remove the <see cref="MastoreProviderMappingSetting"/> at the specified <paramref name="index"/>
        /// </summary>
        /// <param name="index">
        /// The index of the <see cref="MastoreProviderMappingSetting"/> to remove
        /// </param>
        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new <see cref="T:System.Configuration.ConfigurationElement"/> when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="elementName">
        /// The name of the <see cref="T:System.Configuration.ConfigurationElement"/> to create. 
        /// </param>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new MastoreProviderMappingSetting(elementName);
        }

        /// <summary>
        /// Creates a <see cref="MastoreProviderMappingSetting"/> element
        /// </summary>
        /// <returns>
        /// A new <see cref="MastoreProviderMappingSetting"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MastoreProviderMappingSetting();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="element">
        /// The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for. 
        /// </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MastoreProviderMappingSetting)element).Name;
        }

        #endregion
    }
}