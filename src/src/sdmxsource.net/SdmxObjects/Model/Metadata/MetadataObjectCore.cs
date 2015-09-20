// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Metadata
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   The metadata object core.
    /// </summary>
    [Serializable]
    public class MetadataObjectCore : SdmxObjectCore, IMetadata
    {
        #region Fields

        /// <summary>
        ///   The iheader.
        /// </summary>
        private readonly IHeader header;

        /// <summary>
        ///   The metadata sets.
        /// </summary>
        private readonly IList<IMetadataSet> metadataSets;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataObjectCore"/> class.
        /// </summary>
        /// <param name="metadata">
        /// The metadata. 
        /// </param>
        public MetadataObjectCore(GenericMetadata metadata)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataDocument), null)
        {
            this.metadataSets = new List<IMetadataSet>();

            this.header = new HeaderImpl(metadata.Content.Header);

            foreach (MetadataSetType metadataset in metadata.Content.DataSet)
            {
                this.metadataSets.Add(new MetadataSetObjectCore(this, metadataset));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataObjectCore"/> class.
        /// </summary>
        /// <param name="metadataSets">
        /// The metadata. 
        /// </param>
        public MetadataObjectCore(ICollection<IMetadataSet> metadataSets)
            :base (SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataDocument), null)
        {
		
		   if(metadataSets != null)
           {
			   this.metadataSets.AddAll(metadataSets);
		   }
	    }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the header.
        /// </summary>
        public virtual IHeader Header
        {
            get
            {
                return this.header;
            }
        }

        /// <summary>
        ///   Gets the metadata set.
        /// </summary>
        public virtual IList<IMetadataSet> MetadataSet
        {
            get
            {
                return this.metadataSets;
            }
        }

        #endregion


       ///////////////////////////////////////////////////////////////////////////////////////////////////
	   ////////////DEEP EQUALS							 //////////////////////////////////////////////////
	   ///////////////////////////////////////////////////////////////////////////////////////////////////
	
	   public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
       {
           if (sdmxObject == null)
           {
			  return false;
		   }

           if (sdmxObject.StructureType == this.StructureType)
           {
			  IMetadata that = (IMetadata) sdmxObject;
			  if(!base.Equivalent(this.metadataSets, that.MetadataSet, includeFinalProperties))
              {
				 return false;
			  }

			return base.DeepEqualsInternal(that, includeFinalProperties);
		}

		return false;
	}

  	    ///////////////////////////////////////////////////////////////////////////////////////////////////
 	    ////////////COMPOSITES		                     //////////////////////////////////////////////////
 	    ///////////////////////////////////////////////////////////////////////////////////////////////////
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = new HashSet<ISdmxObject>();
            base.AddToCompositeSet(metadataSets, composites);
            return composites;
        }
    }
}