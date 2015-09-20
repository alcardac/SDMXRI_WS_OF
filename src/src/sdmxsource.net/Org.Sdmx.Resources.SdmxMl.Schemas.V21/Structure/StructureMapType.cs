// -----------------------------------------------------------------------
// <copyright file="StructureMapType.cs" company="Eurostat">
//   Date Created : 2012-11-29
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    /// <para>
    /// StructureMapType defines the structure for mapping components of one structure to components of another structure. A structure may be referenced directly meaning the map applies wherever the structure is used, or it may be a reference via a structure usage meaning the map only applies within the context of that usage. Using the related structures, one can make extrapolations between maps. For example, if key families, A, B, and C, are all grouped in a related structures container, then a map from key family A to C and a map from key family B to C could be used to infer a relation between key family A to C.
    /// </para>
    /// <para>
    /// Regular expression: (Annotations?, Name+, Description*, Source, Target, ComponentMap)
    /// </para>
    /// </summary>
    public partial class StructureMapType
    {
        private XTypedList<ComponentMapType> _componentMaps;

        public IList<ComponentMapType> ComponentMapTypes
        {
            get {
                if ((this._componentMaps == null)) {
                    this._componentMaps = new XTypedList<ComponentMapType>(this, LinqToXsdTypeManager.Instance, XName.Get("ComponentMap", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure"));
                }
                return this._componentMaps;
            }
            set {
                if ((value == null)) {
                    this._componentMaps = null;
                }
                else {
                    if ((this._componentMaps == null)) {
                        this._componentMaps = XTypedList<ComponentMapType>.Initialize(this, LinqToXsdTypeManager.Instance, value, XName.Get("ComponentMap", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure"));
                    }
                    else {
                        XTypedServices.SetList(this._componentMaps, value);
                    }
                }
            }
        }
    }
}