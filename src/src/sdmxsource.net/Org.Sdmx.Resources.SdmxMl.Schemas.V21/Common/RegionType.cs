// -----------------------------------------------------------------------
// <copyright file="RegionType.cs" company="EUROSTAT">
//   Date Created : 2014-09-25
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    public partial class RegionType
    {
        /// <summary>
        /// <para>
        /// Attributes contains a reference to an attribute component (data or metadata) and provides a collection of values for the referenced attribute. This serves to state that for the key which defines the region, the attributes that are specified here have or do not have (depending to the include attribute of the value set) the values provided. It is possible to provide and attribute reference without specifying values, for the purpose of stating the attribute is absent (include = false) or present with an unbounded set of values. As opposed to key components, which are assumed to be wild carded if absent, no assumptions are made about the absence of an attribute. Only attributes which are explicitly stated to be present or absent from the region will be know. All unstated attributes for the set cannot be assumed to absent or present.
        /// </para>
        /// <para>
        /// Occurrence: optional, repeating
        /// </para>
        /// <para>
        /// Regular expression: (KeyValue*, Attribute*)
        /// </para>
        /// </summary>
        private static readonly XName _attributName = XName.Get("Attribute", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common");

        /// <summary>
        /// <para>
        /// KeyValue contains a reference to a component which disambiguates the data or metadata (i.e. a dimension or target object) and provides a collection of values for the component. The collection of values can be flagged as being inclusive or exclusive to the region being defined. Any key component that is not included is assumed to be wild carded, which is to say that the cube includes all possible values for the un-referenced key components. Further, this assumption applies to the values of the components as well. The values for any given component can only be sub-setted in the region by explicit inclusion or exclusion. For example, a dimension X which has the possible values of 1, 2, 3 is assumed to have all of these values if a key value is not defined. If a key value is defined with an inclusion attribute of true and the values of 1 and 2, the only the values of 1 and 2 for dimension X are included in the definition of the region. If the key value is defined with an inclusion attribute of false and the value of 1, then the values of 2 and 3 for dimension X are included in the definition of the region. Note that any given key component must only be referenced once in the region.
        /// </para>
        /// <para>
        /// Occurrence: optional, repeating
        /// </para>
        /// <para>
        /// Regular expression: (KeyValue*, Attribute*)
        /// </para>
        /// </summary>
        private static readonly XName _keyValue = XName.Get("KeyValue", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common");

        /// <summary>
        /// <para>
        /// KeyValue contains a reference to a component which disambiguates the data or metadata (i.e. a dimension or target object) and provides a collection of values for the component. The collection of values can be flagged as being inclusive or exclusive to the region being defined. Any key component that is not included is assumed to be wild carded, which is to say that the cube includes all possible values for the un-referenced key components. Further, this assumption applies to the values of the components as well. The values for any given component can only be sub-setted in the region by explicit inclusion or exclusion. For example, a dimension X which has the possible values of 1, 2, 3 is assumed to have all of these values if a key value is not defined. If a key value is defined with an inclusion attribute of true and the values of 1 and 2, the only the values of 1 and 2 for dimension X are included in the definition of the region. If the key value is defined with an inclusion attribute of false and the value of 1, then the values of 2 and 3 for dimension X are included in the definition of the region. Note that any given key component must only be referenced once in the region.
        /// </para>
        /// <para>
        /// Occurrence: optional, repeating
        /// </para>
        /// <para>
        /// Regular expression: (KeyValue*, Attribute*)
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="ComponentValueSetType"/> based class.
        /// </typeparam>
        /// <returns>The KeyValues</returns>
        public IList<T> GetTypedKeyValue<T>() where T : ComponentValueSetType
        {
            return new XTypedList<T>(this, LinqToXsdTypeManager.Instance, _keyValue);
        } 

        /// <summary>
        /// <para>
        /// Attributes contains a reference to an attribute component (data or metadata) and provides a collection of values for the referenced attribute. This serves to state that for the key which defines the region, the attributes that are specified here have or do not have (depending to the include attribute of the value set) the values provided. It is possible to provide and attribute reference without specifying values, for the purpose of stating the attribute is absent (include = false) or present with an unbounded set of values. As opposed to key components, which are assumed to be wild carded if absent, no assumptions are made about the absence of an attribute. Only attributes which are explicitly stated to be present or absent from the region will be know. All unstated attributes for the set cannot be assumed to absent or present.
        /// </para>
        /// <para>
        /// Occurrence: optional, repeating
        /// </para>
        /// <para>
        /// Regular expression: (KeyValue*, Attribute*)
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="ComponentValueSetType"/> based class.
        /// </typeparam>
        /// <returns>
        /// The Attributes.
        /// </returns>
        public IList<T> GetTypedAttribute<T>() where T : ComponentValueSetType
        {
            return new XTypedList<T>(this, LinqToXsdTypeManager.Instance, _attributName);
        }
    }
}