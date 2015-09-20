// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryMeasureBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The primary measure core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using PrimaryMeasure = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.PrimaryMeasure;

    /// <summary>
    ///   The primary measure core.
    /// </summary>
    [Serializable]
    public class PrimaryMeasureCore : ComponentCore, IPrimaryMeasure
    {
        #region Static Fields

        /// <summary>
        ///   The _primary measure type.
        /// </summary>
        private static readonly SdmxStructureType _primaryMeasureType =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.PrimaryMeasure);

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryMeasureCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public PrimaryMeasureCore(IPrimaryMeasureMutableObject itemMutableObject, IMeasureList parent)
            : base(itemMutableObject, parent)
        {
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryMeasureCore"/> class.
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primary measure. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public PrimaryMeasureCore(PrimaryMeasureType primaryMeasure, IMeasureList parent)
            : base(primaryMeasure, _primaryMeasureType, parent)
        {
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryMeasureCore"/> class.
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primary measure. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public PrimaryMeasureCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.PrimaryMeasureType primaryMeasure, IMeasureList parent)
            : base(
                primaryMeasure, 
                _primaryMeasureType, 
                primaryMeasure.Annotations, 
                primaryMeasure.TextFormat, 
                primaryMeasure.codelistAgency, 
                primaryMeasure.codelist, 
                primaryMeasure.codelistVersion, 
                primaryMeasure.conceptSchemeAgency, 
                primaryMeasure.conceptSchemeRef,
                GetConceptSchemeVersion(primaryMeasure), 
                primaryMeasure.codelistAgency, 
                primaryMeasure.conceptRef, 
                parent)
        {
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryMeasureCore"/> class.
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primary measure. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public PrimaryMeasureCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.PrimaryMeasureType primaryMeasure, IMeasureList parent)
            : base(primaryMeasure, _primaryMeasureType, primaryMeasure.Annotations, null, primaryMeasure.concept, parent)
        {
            this.Validate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return PrimaryMeasure.FixedId;
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
                return this.DeepEqualsInternal((IPrimaryMeasure)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get parent ids.
        /// </summary>
        /// <param name="includeDifferentTypes">
        /// The include different types. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        protected internal override IList<string> GetParentIds(bool includeDifferentTypes)
        {
            IList<string> returnList = new List<string>();
            returnList.Add(this.Id);
            return returnList;
        }

        /// <summary>
        /// Returns concept scheme version. It tries to detect various conventions
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primaryMeasure.
        /// </param>
        /// <returns>
        /// The concept scheme version; otherwise null
        /// </returns>
        private static string GetConceptSchemeVersion(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.PrimaryMeasureType primaryMeasure)
        {
            if (!string.IsNullOrWhiteSpace(primaryMeasure.conceptVersion))
            {
                return primaryMeasure.conceptVersion;
            }

            if (!string.IsNullOrWhiteSpace(primaryMeasure.ConceptSchemeVersionEstat))
            {
                return primaryMeasure.ConceptSchemeVersionEstat;
            }

            var extDimension = primaryMeasure as Org.Sdmx.Resources.SdmxMl.Schemas.V20.extension.structure.PrimaryMeasureType;
            if (extDimension != null && !string.IsNullOrWhiteSpace(extDimension.conceptSchemeVersion))
            {
                return extDimension.conceptSchemeVersion;
            }

            return null;
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        private void Validate()
        {
            this.Id = PrimaryMeasure.FixedId;
        }

        #endregion
    }
}