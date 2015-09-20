// -----------------------------------------------------------------------
// <copyright file="MetadataTargetRegionMutableObjectCorel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{

	using System.Collections.Generic;

	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class MetadataTargetRegionMutableObjectCore : MutableCore, IMetadataTargetRegionMutableObject
	{

		private readonly bool include;
		private readonly string report;
		private readonly string metadataTarget;
		private readonly IList<IKeyValuesMutable> attributes = new List<IKeyValuesMutable>();
		private readonly IList<IMetadataTargetKeyValuesMutable> key = new List<IMetadataTargetKeyValuesMutable>();

		public MetadataTargetRegionMutableObjectCore()
			: base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTargetRegion))
		{
		}

		public MetadataTargetRegionMutableObjectCore(IMetadataTargetRegion createdFrom)
			: base(createdFrom)
		{
			this.include  = createdFrom.IsInclude;
		this.report = createdFrom.Report;
		this.metadataTarget = createdFrom.MetadataTarget;
		foreach(IMetadataTargetKeyValues currentKv in createdFrom.Key) 
		{
			key.Add(new MetadataTargetKeyValuesMutableObjectCore(currentKv));
		}
		foreach(IKeyValues currentKv in createdFrom.Attributes) {
			attributes.Add(new KeyValuesMutableImpl(currentKv));
		}
		}

		#region Implementation of IMetadataTargetRegionMutableObject

		public bool IsInclude
		{
			get
			{
				return include;
			}
		}

		public string Report
		{
			get
			{
				return report;
			}
		}

		public string MetadataTarget
		{
			get
			{
				return metadataTarget;
			}
		}

		public IList<IMetadataTargetKeyValuesMutable> Key
		{
			get
			{
				return key;
			}
		}

		public void AddKey(IMetadataTargetKeyValuesMutable key)
		{
			this.key.Add(key);
		}

		public IList<IKeyValuesMutable> Attributes
		{
			get
			{
				return attributes;
			}
		}

		public void AddAttribute(IKeyValuesMutable attribute)
		{
			this.attributes.Add(attribute);
		}

		#endregion
	}
}
