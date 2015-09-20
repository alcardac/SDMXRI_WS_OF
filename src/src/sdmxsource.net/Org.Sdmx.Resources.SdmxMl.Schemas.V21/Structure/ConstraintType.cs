// -----------------------------------------------------------------------
// <copyright file="ConstraintType.cs" company="EUROSTAT">
//   Date Created : 2014-10-16
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;

    using Xml.Schema.Linq;

    /// <summary>
    /// <para>
    /// ConstraintType is an abstract base type that specific types of constraints (content and attachment) restrict and extend to describe their details. The inclusion of a key or region in a constraint is determined by first processing the included key sets, and then removing those keys defined in the excluded key sets. If no included key sets are defined, then it is assumed the all possible keys or regions are included, and any excluded key or regions are removed from this complete set.
    /// </para>
    /// <para>
    /// Regular expression: (Annotations?, Name+, Description*, ConstraintAttachment?, (DataKeySet | MetadataKeySet | CubeRegion | MetadataTargetRegion)*)
    /// </para>
    /// </summary>
    public abstract partial class ConstraintType
    {
        /// <summary>
        /// The _constraint attachment.
        /// </summary>
        private static readonly XName _constraintAttachment = XName.Get("ConstraintAttachment", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure");

        /// <summary>
        /// <para>
        /// ConstraintAttachment describes the collection of constrainable artefacts that the constraint is attached to.
        /// </para>
        /// <para>
        /// Occurrence: optional
        /// </para>
        /// <para>
        /// Regular expression: (Annotations?, Name+, Description*, ConstraintAttachment?, (DataKeySet | MetadataKeySet | CubeRegion | MetadataTargetRegion)*)
        /// </para>
        /// </summary>
        /// <typeparam name="T">The <see cref="ConstraintAttachmentType"/> based type</typeparam>
        /// <returns>The <see cref="ConstraintAttachmentType"/> based instance.</returns>
        public T GetConstraintAttachmentType<T>() where T : ConstraintAttachmentType
        {
            XElement x = this.GetElement(_constraintAttachment);
            return XTypedServices.ToXTypedElement<T>(x, LinqToXsdTypeManager.Instance); 
        }
    }
}