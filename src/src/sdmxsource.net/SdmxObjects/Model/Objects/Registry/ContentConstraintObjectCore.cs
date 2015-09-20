// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentConstraintBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The content constraint object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
	using System;
	using System.Collections.Generic;

	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
	using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
	using Org.Sdmxsource.Sdmx.Api.Model.Data;
	using Org.Sdmxsource.Util.Extensions;

	using log4net;

	using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
	using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Exception;
	using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
	using Org.Sdmxsource.Util;

	/// <summary>
	///   The content constraint object core.
	/// </summary>
	[Serializable]
	public class ContentConstraintObjectCore :
		ConstraintObjectCore<IContentConstraintObject, IContentConstraintMutableObject>, 
		IContentConstraintObject
	{
		#region Static Fields

		/// <summary>
		///   The log.
		/// </summary>
		private static readonly ILog LOG = LogManager.GetLogger(typeof(ContentConstraintObjectCore));

		#endregion

		#region Fields

		/// <summary>
		///   The ireference period.
		/// </summary>
		private readonly IReferencePeriod referencePeriod;

		/// <summary>
		///   The irelease calendar.
		/// </summary>
		private readonly IReleaseCalendar releaseCalendar;

		/// <summary>
		///   The is defining actual data present.
		/// </summary>
		private readonly bool isIsDefiningActualDataPresent; // Default Value

		/// <summary>
		///   The excluded cube region.
		/// </summary>
		private ICubeRegion excludedCubeRegion;

		/// <summary>
		///   The included cube region.
		/// </summary>
		private ICubeRegion includedCubeRegion;

		private IMetadataTargetRegion _metadataTargetRegionBean;

		#endregion

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////    
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentConstraintObjectCore"/> class.
		/// </summary>
		/// <param name="agencyScheme">
		/// The sdmxObject. 
		/// </param>
		/// <param name="actualLocation">
		/// The actual location. 
		/// </param>
		/// <param name="isServiceUrl">
		/// The is service url. 
		/// </param>
		public ContentConstraintObjectCore(IContentConstraintObject agencyScheme, Uri actualLocation, bool isServiceUrl)
			: base(agencyScheme, actualLocation, isServiceUrl)
		{
			this.isIsDefiningActualDataPresent = true;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentConstraintObjectCore"/> class.
		/// </summary>
		/// <param name="mutable">
		/// The mutable. 
		/// </param>
        /// <exception cref="SdmxSemmanticException">
		/// Throws Validate exception.
		/// </exception>
		public ContentConstraintObjectCore(IContentConstraintMutableObject mutable)
			: base(mutable)
		{
			this.isIsDefiningActualDataPresent = true;
			if (mutable.IncludedCubeRegion != null && 
				(ObjectUtil.ValidCollection(mutable.IncludedCubeRegion.KeyValues) ||
				ObjectUtil.ValidCollection(mutable.IncludedCubeRegion.AttributeValues)))
			{
				this.includedCubeRegion = new CubeRegionCore(mutable.IncludedCubeRegion, this);
			}

			if (mutable.ExcludedCubeRegion != null &&
				(ObjectUtil.ValidCollection(mutable.ExcludedCubeRegion.KeyValues) ||
				ObjectUtil.ValidCollection(mutable.ExcludedCubeRegion.AttributeValues)))
			{
				this.excludedCubeRegion = new CubeRegionCore(mutable.ExcludedCubeRegion, this);
			}

			if (mutable.ReferencePeriod != null)
			{
				this.referencePeriod = new ReferencePeriodCore(mutable.ReferencePeriod, this);
			}

			if (mutable.ReleaseCalendar != null)
			{
				this.releaseCalendar = new ReleaseCalendarCore(mutable.ReleaseCalendar, this);
			}
            if (mutable.MetadataTargetRegion != null) this._metadataTargetRegionBean = new MetadataTargetRegionCore(mutable.MetadataTargetRegion, this);

			this.isIsDefiningActualDataPresent = mutable.IsDefiningActualDataPresent;

			try
			{
				this.Validate();
			}
            catch (SdmxSemmanticException e)
			{
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
			}
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentConstraintObjectCore"/> class.
		/// </summary>
		/// <param name="type">
		/// The type. 
		/// </param>
		/// ///
        /// <exception cref="SdmxNotImplementedException">
		/// Throws Unsupported Exception.
		/// </exception>
        /// <exception cref="SdmxSemmanticException">
		/// Throws Validate exception.
		/// </exception>
		public ContentConstraintObjectCore(ContentConstraintType type)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint), type.GetConstraintAttachmentType<ContentConstraintAttachmentType>())
		{
			this.isIsDefiningActualDataPresent = true;
			if (type.type != null)
			{
				this.isIsDefiningActualDataPresent = type.type == ContentConstraintTypeCodeTypeConstants.Actual;
			}

			if (ObjectUtil.ValidCollection(type.MetadataKeySet))
			{
				// FUNC 2.1 MetadataKeySet
                throw new SdmxNotImplementedException(
					ExceptionCode.Unsupported, "ContentConstraintObjectCore - MetadataKeySet");
			}

			if (ObjectUtil.ValidCollection(type.CubeRegion))
			{
				this.BuildCubeRegions(type.CubeRegion);
			}

			if (ObjectUtil.ValidCollection(type.MetadataTargetRegion))
			{
				// FUNC 2.1 MetadataTarget Region
                throw new SdmxNotImplementedException(
					ExceptionCode.Unsupported, "ContentConstraintObjectCore - MetadataTargetRegionList");
			}

			if (type.ReferencePeriod != null)
			{
				this.referencePeriod = new ReferencePeriodCore(type.ReferencePeriod, this);
			}

			if (type.ReleaseCalendar != null)
			{
				this.releaseCalendar = new ReleaseCalendarCore(type.ReleaseCalendar, this);
			}

			try
			{
				this.Validate();
			}
            catch (SdmxSemmanticException e)
			{
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
			}
		}

		#endregion

		#region Public Properties

		public virtual IMetadataTargetRegion MetadataTargetRegion
		{
			get
			{
				return _metadataTargetRegionBean;
			}
		}

		/// <summary>
		///   Gets a value indicating whether defining actual data present.
		/// </summary>
		public virtual bool IsDefiningActualDataPresent
		{
			get
			{
				return this.isIsDefiningActualDataPresent;
			}
		}

		/// <summary>
		///   Gets the excluded cube region.
		/// </summary>
		public virtual ICubeRegion ExcludedCubeRegion
		{
			get
			{
				return this.excludedCubeRegion;
			}
		}

		/// <summary>
		///   Gets the included cube region.
		/// </summary>
		public virtual ICubeRegion IncludedCubeRegion
		{
			get
			{
				return this.includedCubeRegion;
			}
		}

		/// <summary>
		///   Gets the mutable instance.
		/// </summary>
		public override IContentConstraintMutableObject MutableInstance
		{
			get
			{
				return new ContentConstraintMutableCore(this);
			}
		}

		/// <summary>
		///   Gets the reference period.
		/// </summary>
		public virtual IReferencePeriod ReferencePeriod
		{
			get
			{
				return this.referencePeriod;
			}
		}

		/// <summary>
		///   Gets the release calendar.
		/// </summary>
		public virtual IReleaseCalendar ReleaseCalendar
		{
			get
			{
				return this.releaseCalendar;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// The deep equals.
		/// </summary>
		/// <param name="sdmxObject">
		/// The sdmxObject. 
		/// </param>
		/// <param name="includeFinalProperties"> </param>
		/// <returns>
		/// The <see cref="bool"/> . 
		/// </returns>
		public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
		{
			if (sdmxObject == null) return false;

			if (sdmxObject.StructureType == this.StructureType)
			{
				var that = (IContentConstraintObject)sdmxObject;
				if (!this.Equivalent(this.IncludedCubeRegion, that.IncludedCubeRegion, includeFinalProperties))
				{
					return false;
				}

				if (!this.Equivalent(this.ExcludedCubeRegion, that.ExcludedCubeRegion, includeFinalProperties))
				{
					return false;
				}

				if (!this.Equivalent(this.referencePeriod, that.ReferencePeriod, includeFinalProperties))
				{
					return false;
				}

				if (!this.Equivalent(this.releaseCalendar, that.ReleaseCalendar, includeFinalProperties))
				{
					return false;
				}

				if (this.isIsDefiningActualDataPresent != that.IsDefiningActualDataPresent)
				{
					return false;
				}

				return this.DeepEqualsInternal(that, includeFinalProperties);
			}

			return false;
		}

		/// <summary>
		/// The get stub.
		/// </summary>
		/// <param name="actualLocation">
		/// The actual location. 
		/// </param>
		/// <param name="isServiceUrl">
		/// The is service url. 
		/// </param>
		/// <returns>
		/// The <see cref="IContentConstraintObject"/> . 
		/// </returns>
		public override IContentConstraintObject GetStub(Uri actualLocation, bool isServiceUrl)
		{
			return new ContentConstraintObjectCore(this, actualLocation, isServiceUrl);
		}

		#endregion

		#region Methods

		/// <summary>
		/// The build cube regions.
		/// </summary>
		/// <param name="cubeRegionsTypes">
		/// The cube regions types. 
		/// </param>
		private void BuildCubeRegions(IList<CubeRegionType> cubeRegionsTypes)
		{
			foreach (CubeRegionType cubeRegionType in cubeRegionsTypes)
			{
			    bool isExcluded = false;
			    bool negate = false;
			    if (cubeRegionType.include)
			    {
			        isExcluded = !cubeRegionType.include;
			    }
			    var attributeValueSetTypes = cubeRegionType.GetTypedAttribute<AttributeValueSetType>();
			    if (attributeValueSetTypes != null)
			    {
			        foreach (var currentKey in attributeValueSetTypes)
			        {
			            if (currentKey.include == false)
			            {
			                negate = true;
			                break;
			            }
			        }
			    }

			    if (!isExcluded)
			    {
			        var cubeRegionKeyTypes = cubeRegionType.GetTypedKeyValue<CubeRegionKeyType>();
			        if (cubeRegionKeyTypes != null)
			        {
			            foreach (var currentKey0 in cubeRegionKeyTypes)
			            {
			                if (currentKey0.include == false)
			                {
			                    negate = true;
			                    break;
			                }
			            }
			        }
			    }

			    this.StoreCubeRegion(new CubeRegionCore(cubeRegionType, negate, this), isExcluded);
			}
		}

		/// <summary>
		/// The get key values.
		/// </summary>
		/// <param name="id">
		/// The id. 
		/// </param>
		/// <param name="kvs">
		/// The kvs. 
		/// </param>
		/// <returns>
		/// The <see cref="IKeyValuesMutable"/> . 
		/// </returns>
		private IKeyValuesMutable GetKeyValues(string id, IList<IKeyValuesMutable> kvs)
		{
			foreach (IKeyValuesMutable keyValuesMutable in kvs)
			{
				if (keyValuesMutable.Id.Equals(id))
				{
					return keyValuesMutable;
				}
			}

			return null;
		}

		/// <summary>
		/// The merge cube region.
		/// </summary>
		/// <param name="mergeIncluded">
		/// The merge included. 
		/// </param>
		/// <param name="toMerge">
		/// The to merge. 
		/// </param>
		private void MergeCubeRegion(bool mergeIncluded, ICubeRegion toMerge)
		{
			ICubeRegionMutableObject mutable = null;
			if (mergeIncluded)
			{
				mutable = new CubeRegionMutableCore(this.includedCubeRegion);
			}
			else
			{
				mutable = new CubeRegionMutableCore(this.excludedCubeRegion);
			}

			this.MergeKeyValues(mutable.KeyValues, toMerge.KeyValues);
			this.MergeKeyValues(mutable.AttributeValues, toMerge.AttributeValues);

			if (mergeIncluded)
			{
				this.includedCubeRegion = new CubeRegionCore(mutable, this);
			}
			else
			{
				this.excludedCubeRegion = new CubeRegionCore(mutable, this);
			}
		}

		/// <summary>
		/// The merge key values.
		/// </summary>
		/// <param name="existingKeyValues">
		/// The existing key values. 
		/// </param>
		/// <param name="toMerge">
		/// The to merge. 
		/// </param>
        /// <exception cref="SdmxSemmanticException">
		/// Throws Validate exception.
		/// </exception>
		private void MergeKeyValues(IList<IKeyValuesMutable> existingKeyValues, IList<IKeyValues> toMerge)
		{
			foreach (IKeyValues currentKeyValues in toMerge)
			{
				IKeyValuesMutable existingMutable = this.GetKeyValues(currentKeyValues.Id, existingKeyValues);

				// Nothing exists yet, add this key values container in
				if (existingMutable == null)
				{
					existingKeyValues.Add(new KeyValuesMutableImpl(currentKeyValues));
					continue;
				}

				// We need to merge what we have with what we've got
				if (currentKeyValues.TimeRange != null)
				{
					if (ObjectUtil.ValidCollection(existingMutable.KeyValues))
					{
                        throw new SdmxSemmanticException(
							"Can not create CubeRegion as it is defining both a TimeRange and a Set of allowed values for a Key Value with the same Id");
					}

                    throw new SdmxSemmanticException(
						"Can not create CubeRegion as it is a TimeRange twice for a Key Value with the same Id");
				}

				foreach (string currentValue in currentKeyValues.Values)
				{
					if (existingMutable.KeyValues.Contains(currentValue))
					{
						if (existingMutable.IsCascadeValue(currentValue)
							!= currentKeyValues.IsCascadeValue(currentValue))
						{
                            throw new SdmxSemmanticException(
								"Can not create CubeRegion as it defines a Key/Value '" + currentKeyValues.Id
								+ "'/'+currentValue)+' twice, once with cascade values set to true, and once false");
						}

						LOG.Warn("Duplicate definition of KeyValue in 2 different Cube Regions");
					}
					else
					{
						existingMutable.AddValue(currentValue);
					}

					if (currentKeyValues.IsCascadeValue(currentValue))
					{
						existingMutable.AddCascade(currentValue);
					}
				}
			}
		}

		/// <summary>
		/// Takes a cube region, which is either a inclusive or exclusive, and if there is already the inclusive or exclusive cube region
		///   stored, then this one will merge into the stored one.  If there are any duplicate values, then an error will be thrown.
		/// </summary>
		/// <param name="currentCubeRegion">
		/// The current Cube Region. 
		/// </param>
		/// <param name="isExcluded">
		/// The is Excluded. 
		/// </param>
		private void StoreCubeRegion(ICubeRegion currentCubeRegion, bool isExcluded)
		{
			if (!isExcluded)
			{
				if (this.includedCubeRegion == null)
				{
					this.includedCubeRegion = currentCubeRegion;
				}
				else
				{
					this.MergeCubeRegion(true, currentCubeRegion);
				}
			}
			else
			{
				if (this.excludedCubeRegion == null)
				{
					this.excludedCubeRegion = currentCubeRegion;
				}
				else
				{
					this.MergeCubeRegion(false, currentCubeRegion);
				}
			}
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////VALIDATE                                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		///   The validate.
		/// </summary>
		private void Validate()
		{
			base.ValidateMaintainableAttributes();

			IDictionary<String, ISet<String>> includedCodesForKey = new Dictionary<string, ISet<string>>();
		ISet<String> wildcardedConcepts = new HashSet<String>();

		
		if(IncludedSeriesKeys != null) {
			foreach(IConstrainedDataKey cdkb in IncludedSeriesKeys.ConstrainedDataKeys) 
			{
				foreach(IKeyValue kv in cdkb.KeyValues) {
					if(ContainsKey(excludedCubeRegion, kv)) {
                        throw new SdmxSemmanticException("Constraint is in conflict with itself.  Included series key contains component '" + kv.Concept + "' with value '" + kv.Code + "'.  This code has also been specified as excluded by the constraint's CubeRegion");
					}
					if(kv.Code.Equals(ContentConstraintObject.WildcardCode)) 
					{
						wildcardedConcepts.Add(kv.Concept);
						includedCodesForKey.Remove(kv.Concept);
					} else if(!wildcardedConcepts.Contains(kv.Concept)){
						ISet<String> includedCodes = includedCodesForKey[kv.Concept];
						if(includedCodes == null) {
							includedCodes = new HashSet<String>();
							includedCodesForKey.Add(kv.Concept, includedCodes);
						}
						includedCodes.Add(kv.Code);
					}
				}
			}
		}
		if(IncludedCubeRegion != null) {
			//Check we are not including any more / less then the series includes
			foreach(IKeyValues kvs in IncludedCubeRegion.KeyValues) {
				ISet<String> allIncludes;
                if (includedCodesForKey.TryGetValue(kvs.Id, out allIncludes))
                {
					if (!allIncludes.ContainsAll(kvs.Values) || !kvs.Values.ContainsAll(allIncludes))
					{
                        throw new SdmxSemmanticException("Constraint is in conflict with itself. The constraint defines valid series, this can not be further restricted by the cube region.  The " +
								"Cube Region has further restricted dimension '"+kvs.Id+"' by not including all the codes defined by the keyset.");
					}
				}
				if(ExcludedCubeRegion != null) {
					ValidateNoKeyValuesDuplicates(kvs, ExcludedCubeRegion.KeyValues);
				}
			}
		}
		if(ExcludedCubeRegion != null) {
			foreach(IKeyValues kvs in ExcludedCubeRegion.KeyValues) {
				ISet<String> allIncludes;
                if (includedCodesForKey.TryGetValue(kvs.Id, out allIncludes))
                {
                    throw new SdmxSemmanticException("Constraint is in conflict with itself. The constraint defines valid series, the dimension  '" + kvs.Id + "' can not be further restriced by the cube region to " +
							"exclude codes which are already marked for inclusion by the keyset"); 
				}
				if(IncludedCubeRegion != null) {
					ValidateNoKeyValuesDuplicates(kvs, IncludedCubeRegion.KeyValues);
				}
			}
		}
		}

		private bool ContainsKey(ICubeRegion cubeRegion, IKeyValue kv)
		{
			if (cubeRegion != null) return cubeRegion.GetValues(kv.Concept).Contains(kv.Code);
			return false;
		}

		/// <summary>
		/// The validate no key values duplicates.
		/// </summary>
		/// <param name="kvs">
		/// The kvs. 
		/// </param>
		/// <param name="kvsList">
		/// The kvs list. 
		/// </param>
        /// <exception cref="SdmxSemmanticException">
		/// Throws Validate exception.
		/// </exception>
		private void ValidateNoKeyValuesDuplicates(IKeyValues kvs, IList<IKeyValues> kvsList)
		{
			foreach (IKeyValues currentKvs in kvsList)
			{
				if (currentKvs.Id.Equals(kvs.Id))
				{
					foreach (string currentValue in currentKvs.Values)
					{
						if (kvs.Values.Contains(currentValue))
						{
                            throw new SdmxSemmanticException(
								"CubeRegion contains a Key Value that is both included and excluded Id='" + kvs.Id
								+ "' Value='" + currentValue + "'");
						}
					}
				}
			}
		}

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES		                     //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       /// The get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
    	   ISet<ISdmxObject> composites = base.GetCompositesInternal();
           base.AddToCompositeSet(this.includedCubeRegion, composites);
           base.AddToCompositeSet(this.excludedCubeRegion, composites);
           base.AddToCompositeSet(this.referencePeriod, composites);
           base.AddToCompositeSet(this.releaseCalendar, composites);
           base.AddToCompositeSet(this._metadataTargetRegionBean, composites);
           return composites;
       }

		#endregion

		///////////////////////////////////////////////////////////////////////////////////////////////////
		////////////GETTERS                                 //////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////
	}
}