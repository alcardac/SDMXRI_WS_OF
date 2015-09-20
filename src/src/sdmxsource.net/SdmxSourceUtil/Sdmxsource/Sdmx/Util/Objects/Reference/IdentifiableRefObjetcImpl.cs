// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableRefObjetcImpl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    /// The identifiable ref objetc impl.
    /// </summary>
    [Serializable]
    public class IdentifiableRefObjetcImpl : IIdentifiableRefObject
    {
        #region Fields

        /// <summary>
        /// The structure type.
        /// </summary>
        private readonly SdmxStructureType _structureEnumType;

        /// <summary>
        /// The _maintainable parent.
        /// </summary>
        private readonly IStructureReference _maintainableParent;

        /// <summary>
        /// The _parent reference.
        /// </summary>
        private readonly IIdentifiableRefObject _parentReference;

        /// <summary>
        /// The _child reference.
        /// </summary>
        private IIdentifiableRefObject _childReference;

        /// <summary>
        /// The _id.
        /// </summary>
        private string _id;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////CONSTRUCTORS                          //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableRefObjetcImpl"/> class.
        /// </summary>
        public IdentifiableRefObjetcImpl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableRefObjetcImpl"/> class.
        /// </summary>
        /// <param name="maintainableParent">
        /// The maintainable parent.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="targetStructureEnumType">
        /// The target structure enum type.
        /// </param>
        public IdentifiableRefObjetcImpl(
            IStructureReference maintainableParent, string id, SdmxStructureType targetStructureEnumType)
        {
            this._structureEnumType = targetStructureEnumType;
            this._maintainableParent = maintainableParent;
            this._id = id;

            Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableRefObjetcImpl"/> class.
        /// </summary>
        /// <param name="maintainableParent">
        /// The maintainable parent.
        /// </param>
        /// <param name="idArr">
        /// The id arr.
        /// </param>
        /// <param name="targetStructureEnumType">
        /// The target structure enum type.
        /// </param>
        public IdentifiableRefObjetcImpl(
            IStructureReference maintainableParent, IList<string> idArr, SdmxStructureType targetStructureEnumType)
        {
            this._structureEnumType = SetStructureType(targetStructureEnumType, 0);
            this._maintainableParent = maintainableParent;

            if (this._structureEnumType.HasFixedId)
            {
                if (!idArr[0].Equals(this._structureEnumType.FixedId))
                {
                    //Change the id array by inserting a new fixed id
                    string[] tempArray = new string[idArr.Count + 1];
                    tempArray[0] = this._structureEnumType.FixedId;

                    for (int i = 0; i < idArr.Count; i++)
                    {
                        tempArray[i + 1] = idArr[i];
                    }

                    idArr = tempArray;
                }
            }

            this._id = idArr[0];
            if (idArr.Count > 1)
            {
                this._childReference = new IdentifiableRefObjetcImpl(this, idArr, 1, targetStructureEnumType);
            }

            Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableRefObjetcImpl"/> class.
        /// </summary>
        /// <param name="parentReference">
        /// The parent reference.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="currentDepth">
        /// The current depth.
        /// </param>
        /// <param name="targetStructureEnumType">
        /// The target structure enum type.
        /// </param>
        private IdentifiableRefObjetcImpl(
            IIdentifiableRefObject parentReference, 
            IList<string> id, 
            int currentDepth, 
            SdmxStructureType targetStructureEnumType)
        {
            this._parentReference = parentReference;
            this._id = id[currentDepth];
            this._structureEnumType = SetStructureType(targetStructureEnumType, currentDepth);
            currentDepth++;
            if (currentDepth < id.Count)
            {
                this._childReference = new IdentifiableRefObjetcImpl(this, id, currentDepth, targetStructureEnumType);
            }

            Validate();
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="IdentifiableRefObjetcImpl"/> class.
        ///// </summary>
        ///// <param name="maintainableParent">
        ///// The maintainable parent.
        ///// </param>
        ///// <param name="ids">
        ///// The ids.
        ///// </param>
        ///// <param name="targetStructureEnumType">
        ///// The target structure enum type.
        ///// </param>
        //public IdentifiableRefObjetcImpl(IStructureReference maintainableParent,
        //    ICollection<string> ids, SdmxStructureType targetStructureEnumType)
        //{
        //    SetStructureType(targetStructureEnumType, 0);
        //    this._maintainableParent = maintainableParent;
        //    var idArr = new string[ids.Count];
        //    ids.CopyTo(idArr, 0);
        //    this._id = idArr[0];
        //    if (idArr.Length > 1)
        //    {
        //        this._childReference = new IdentifiableRefObjetcImpl(this, idArr, 1,
        //                targetStructureEnumType);
        //    }

        //    Validate();
        //}

        #endregion

        
        #region Public Properties

        /// <summary>
        /// Gets or sets the child reference.
        /// </summary>
        public IIdentifiableRefObject ChildReference
        {
            get
            {
                return this._childReference;
            }

            set
            {
                this._childReference = value;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Gets the parent identifiable reference.
        /// </summary>
        public IIdentifiableRefObject ParentIdentifiableReference
        {
            get
            {
                return this._parentReference;
            }
        }

        /// <summary>
        /// Gets the parent maintainable referece.
        /// </summary>
        public IStructureReference ParentMaintainableReferece
        {
            get
            {
                if (this._maintainableParent != null)
                {
                    return this._maintainableParent;
                }

                return this._childReference.ParentMaintainableReferece;
            }
        }

        /// <summary>
        /// Gets the structure enum type.
        /// </summary>
        public SdmxStructureType StructureEnumType
        {
            get
            {
                return this._structureEnumType;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var o = obj as IIdentifiableRefObject;
            if (o != null)
            {
                var that = o;
                if (this.StructureEnumType == that.StructureEnumType)
                {
                    if (ObjectUtil.Equivalent(this.Id, that.Id))
                    {
                        if (ObjectUtil.Equivalent(this.ChildReference, that.ChildReference))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="IdentifiableRefObjetcImpl"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return (this._structureEnumType.GetType() + this.ToString()).GetHashCode();
        }

        /// <summary>
        /// The get match.
        /// </summary>
        /// <param name="reference">
        /// The reference.
        /// </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/>.
        /// </returns>
        public IIdentifiableObject GetMatch(IIdentifiableObject reference)
        {
            if (this._structureEnumType != reference.StructureType)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                if (this.Id.Equals(reference.Id))
                {
                    return reference;
                }

                return null;
            }

            if (this.ChildReference != null)
            {
                /* foreach */
                foreach (IIdentifiableObject currentComposite in reference.IdentifiableComposites)
                {
                    if (this.ChildReference.GetMatch(currentComposite) != null)
                    {
                        return currentComposite;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            string concatString = string.Empty;
            if (this.ChildReference != null)
            {
                concatString = "." + this.ChildReference;
            }

            return this.Id + concatString;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The set structure type.
        /// </summary>
        /// <param name="targetStructureEnumType">
        /// The target structure enum type.
        /// </param>
        /// <param name="currentDepth">
        /// The current depth.
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/>.
        /// </returns>
        private static SdmxStructureType SetStructureType(SdmxStructureType targetStructureEnumType, int currentDepth)
        {
            SdmxStructureType returnType = null;
            if (currentDepth < targetStructureEnumType.NestedDepth)
            {
                SdmxStructureType parentStructureEnum = targetStructureEnumType.ParentStructureType;

                while (parentStructureEnum != null)
                {
                    if (parentStructureEnum.NestedDepth == currentDepth)
                    {
                        returnType = parentStructureEnum;
                    }

                    parentStructureEnum = parentStructureEnum.ParentStructureType;
                }
            }
            else
            {
                returnType = targetStructureEnumType;
            }

            return returnType;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The validate.
        /// </summary>
        private void Validate()
        {
            if (this._structureEnumType.HasFixedId && ObjectUtil.ValidString(this._id))
            {
                if (!this._id.Equals(this._structureEnumType.FixedId))
                {
                    throw new SdmxSemmanticException(this._structureEnumType.StructureType + " has a fixed id of '" + this._structureEnumType.FixedId + "'.  Identifiable reference can not set this to '" + this._id + "'");
                }
            }
        }

        #endregion
    }
}