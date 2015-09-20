// -----------------------------------------------------------------------
// <copyright file="MetadataStructureType.cs" company="Eurostat">
//   Date Created : 2012-11-16
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    /// <summary>
    /// <para>
    /// MetadataStructureType is used to describe a metadata structure definition, which is defined as a collection of metadata concepts, their structure and usage when used to collect or disseminate reference metadata.
    /// </para>
    /// </summary>
    public partial class MetadataStructureType
    {
        /// <summary>
        /// Gets or sets the MetadataStructureComponents. MetadataStructureComponents defines the grouping of the sets of the components that make up the metadata structure definition. All components and component list (target identifiers, identifier components, report structures, and metadata attributes) in the structure definition must have a unique identification.
        /// </summary>
        public MetadataStructureComponents MetadataStructureComponents
        {
            get
            {
                return (MetadataStructureComponents)this.Grouping;
            }

            set
            {
                this.Grouping = value;
            }
        }
    }
}