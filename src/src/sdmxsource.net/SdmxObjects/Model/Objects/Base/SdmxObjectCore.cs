// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Security;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Util.Extensions;
    using Org.Sdmxsource.Util.Reflect;

    /// <summary>
    ///   The sdmx object core.
    /// </summary>
    [Serializable]
    public abstract class SdmxObjectCore : ISdmxObject
    {
        #region Fields

        /// <summary>
        ///   The _parent.
        /// </summary>
        private readonly ISdmxObject _parent;

        /// <summary>
        ///   The composites.
        /// </summary>
        private ISet<ISdmxObject> _composites;

        /// <summary>
        ///   The cross references.
        /// </summary>
        private ISet<ICrossReference> _crossReferences;

        /// <summary>
        ///   The structure type.
        /// </summary>
        private SdmxStructureType _structureType;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectCore"/> class.
        /// </summary>
        /// <param name="structureType0">
        /// The structure type 0. 
        /// </param>
        /// <param name="parent1">
        /// The parent 1. 
        /// </param>
        protected internal SdmxObjectCore(SdmxStructureType structureType0, ISdmxObject parent1)
        {
            this._structureType = structureType0;
            this._parent = parent1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The sdmxObject. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal SdmxObjectCore(ISdmxObject agencyScheme)
        {
            if (agencyScheme == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectNull, "object in constructor");
            }

            this._structureType = agencyScheme.StructureType;
            this._parent = agencyScheme.Parent;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxObjectCore"/> class.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected SdmxObjectCore(IMutableObject mutableObject, ISdmxObject parent)
        {
            if (mutableObject == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectNull, "object in constructor");
            }

            this._structureType = mutableObject.StructureType;
            this._parent = parent;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the composites.
        /// </summary>
        public virtual ISet<ISdmxObject> Composites
        {
            get
            {
                if (this._composites == null)
                {
                    this._composites = GetCompositesInternal();
                }
                return new HashSet<ISdmxObject>(this._composites);
            }

            set
            {
                this._composites = value;
            }
        }

        /// <summary>
        ///   Gets or sets the cross references.
        /// </summary>
        /// <exception cref="Exception">Throws Exception.</exception>
        public virtual ISet<ICrossReference> CrossReferences
        {
            get
            {
                if (this._crossReferences != null)
                {
                    return new HashSet<ICrossReference>(this._crossReferences);
                }

                try
                {
                    var reflectCrossReference = new ReflectUtil<ICrossReference>();
                    ISet<ICrossReference> returnSet = reflectCrossReference.GetCompositeObjects(
                        this, this.GetType().GetProperty("CrossReferences"));
                    returnSet.Remove(null);
                    if (this.StructureType.IsMaintainable)
                    {
                        ISet<ISdmxObject> compositSet = this.Composites;

                        foreach (ISdmxObject currentComposite in compositSet)
                        {
                            if (!ReferenceEquals(this, currentComposite))
                            {
                                returnSet.AddAll(currentComposite.CrossReferences);    
                            }
                            
                            ////currentComposite.CrossReferences.AddAll(returnSet);
                        }
                    }

                    this._crossReferences = returnSet;
                    return returnSet;
                }
                catch (SecurityException e)
                {
                    throw new Exception(e.Message, e);
                }
                catch (AmbiguousMatchException e0)
                {
                    throw new Exception(e0.Message, e0);
                }
            }

            set
            {
                this._crossReferences = value;
            }
        }

        /// <summary>
        ///   Gets the parent.
        /// </summary>
        public virtual ISdmxObject Parent
        {
            get
            {
                return this._parent;
            }
        }

        /// <summary>
        ///   Gets or sets the structure type.
        /// </summary>
        public SdmxStructureType StructureType
        {
            get
            {
                return this._structureType;
            }

            set
            {
                this._structureType = value;
            }
        }

       #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create tertiary.
        /// </summary>
        /// <param name="isSet">
        /// The is set. 
        /// </param>
        /// <param name="valueren">
        /// The valueren. 
        /// </param>
        /// <returns>
        /// The <see cref="TertiaryBool"/> . 
        /// </returns>
        public static TertiaryBool CreateTertiary(bool isSet, bool valueren)
        {
            return SdmxObjectUtil.CreateTertiary(isSet, valueren);
        }

        protected bool DeepEqualsInternal(ISdmxObject bean, bool includeFinalProperties)
        {
            if (bean.StructureType == this.StructureType)
            {
                return true;
            }
            return false;
        }

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
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public virtual bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            throw new SdmxNotImplementedException("Deep Equals on " + this.ToString());
        }

        /// <summary>
        /// The get composites.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <typeparam name="T">Generic type parameter.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISet{T}"/> . 
        /// </returns>
        public virtual ISet<T> GetComposites<T>(Type type)
        {
            ISet<T> returnSet = new HashSet<T>();
            if (this._parent != null)
            {
                foreach (ISdmxObject currentComposite in this.Composites)
                {
                    if (type.IsInstanceOfType(currentComposite))
                    {
                        returnSet.Add((T)currentComposite);
                    }
                }
            }

            return returnSet;
        }

        /// <summary>
        /// The get parent.
        /// </summary>
        /// <param name="includeThisInSearch">
        /// The include this type. 
        /// </param>
        /// <typeparam name="T">Generic type param 
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/> . 
        /// </returns>
        public T GetParent<T>(bool includeThisInSearch) where T : class
        {
            Type type = typeof(T);
            if (this._parent != null)
            {
                if (type.IsInstanceOfType(this._parent))
                {
                    var returnObj = (T)this._parent;
                    return returnObj;
                }

                return this._parent.GetParent<T>(false);
            }

            if (type.IsInstanceOfType(this))
            {
                return this as T;
            }

            return null; // $$$default(T)/* was: null */;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create tertiary.
        /// </summary>
        /// <param name="valueren">
        /// The valueren. 
        /// </param>
        /// <returns>
        /// The <see cref="TertiaryBool"/> . 
        /// </returns>
        protected internal static TertiaryBool CreateTertiary(bool? valueren)
        {
            return SdmxObjectUtil.CreateTertiary(valueren);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The equivalent.
        /// </summary>
        /// <param name="list1">
        /// The list 1. 
        /// </param>
        /// <param name="list2">
        /// The list 2. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <typeparam name="T">Generic type param of type ISdmxObject
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected internal bool Equivalent<T>(IList<T> list1, IList<T> list2, bool includeFinalProperties) where T : ISdmxObject
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            if (list1.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < list2.Count; i++)
            {
                ISdmxObject thisCurrentObj = list2[i];
                ISdmxObject thatCurrentObj = list1[i];

                if (!thisCurrentObj.DeepEquals(thatCurrentObj, includeFinalProperties))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The equivalent.
        /// </summary>
        /// <param name="obj1">
        /// The obj 1. 
        /// </param>
        /// <param name="obj2">
        /// The obj 2. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected internal bool Equivalent(ISdmxObject obj1, ISdmxObject obj2, bool includeFinalProperties)
        {
            if (obj1 == null)
            {
                return obj2 == null;
            }

            if (obj2 == null)
            {
                return false;
            }

            return obj1.DeepEquals(obj2, includeFinalProperties);
        }

        /// <summary>
        /// The equivalent.
        /// </summary>
        /// <param name="crossRef1">
        /// The cross ref 1. 
        /// </param>
        /// <param name="crossRef2">
        /// The cross ref 2. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected internal bool Equivalent(ICrossReference crossRef1, ICrossReference crossRef2)
        {
            if (crossRef1 == null)
            {
                return crossRef2 == null;
            }

            if (crossRef2 == null)
            {
                return false;
            }

            return crossRef2.TargetUrn.Equals(crossRef1.TargetUrn);
        }

        ///// <summary>
        ///// The generate sdmx sdmxObject composites.
        ///// </summary>
        ///// <param name="ignoreMethods">
        ///// The ignore methods. 
        ///// </param>
        //protected internal void GenerateSdmxObjectComposites(params PropertyInfo[] ignoreMethods)
        //{
        //    if (this._composites == null)
        //    {
        //        var reflectUtil = new ReflectUtil<ISdmxObject>();
        //        ISet<ISdmxObject> compositeSet = reflectUtil.GetCompositeObjects(this, ignoreMethods);
        //        ISet<ISdmxObject> mergeSet = new HashSet<ISdmxObject>();
        //        foreach (ISdmxObject currentComposite in compositeSet)
        //        {
        //            if (!ReferenceEquals(this, currentComposite))
        //            {
        //                mergeSet.AddAll(currentComposite.Composites);
        //            }
        //        }

        //        compositeSet.AddAll(mergeSet);
        //        this._composites = compositeSet;
        //    }
        //}

        /// <summary>
        /// The add to composite set.
        /// </summary>
        protected void AddToCompositeSet<T>(ICollection<T> comp, ISet<ISdmxObject> compositesSet) where T : ISdmxObject
        {
            foreach (T composite in comp)
            {
                AddToCompositeSet(composite, compositesSet);
            }
        }

        /// <summary>
        /// The add to composite set.
        /// </summary>
	    protected void AddToCompositeSet(ISdmxObject composite, ISet<ISdmxObject> composites) 
        {
		   if (composite != null)
           {
			  composites.Add(composite);
	       }

		  //Add Bean's Composites
	      if (composite != null) 
          {
			ISet<ISdmxObject> getComposites = composite.Composites;
			if (getComposites != null)
            {
				composites.AddAll(getComposites);
			}
		  }
    	}

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected abstract ISet<ISdmxObject> GetCompositesInternal();

        #endregion
    }
}