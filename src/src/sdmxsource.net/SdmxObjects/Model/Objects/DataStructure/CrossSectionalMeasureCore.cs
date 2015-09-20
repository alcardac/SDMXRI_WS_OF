// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalMeasureBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross sectional measure core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The cross sectional measure core.
    /// </summary>
    [Serializable]
    public class CrossSectionalMeasureCore : ComponentCore, ICrossSectionalMeasure
    {
        #region Fields

        /// <summary>
        ///   The code.
        /// </summary>
        private readonly string code;

        /// <summary>
        ///   The measure dimension.
        /// </summary>
        private readonly string measureDimension;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalMeasureCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The itemMutableObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CrossSectionalMeasureCore(
            ICrossSectionalMeasureMutableObject itemMutableObject, ICrossSectionalDataStructureObject parent)
            : base(itemMutableObject, parent)
        {
            this.measureDimension = itemMutableObject.MeasureDimension;
            this.code = itemMutableObject.Code;
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalMeasureCore"/> class.
        /// </summary>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CrossSectionalMeasureCore(CrossSectionalMeasureType createdFrom, IIdentifiableObject parent)
            : base(
                createdFrom, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CrossSectionalMeasure), 
                createdFrom.Annotations, 
                createdFrom.TextFormat, 
                createdFrom.codelistAgency, 
                createdFrom.codelist, 
                createdFrom.codelistVersion, 
                createdFrom.conceptSchemeAgency, 
                createdFrom.conceptSchemeRef,
                GetConceptSchemeVersion(createdFrom), 
                createdFrom.conceptAgency, 
                createdFrom.conceptRef, 
                parent)
        {
            this.measureDimension = createdFrom.measureDimension;
            this.code = createdFrom.code;
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Public Properties

        /// <summary>
        ///   Gets the code.
        /// </summary>
        public virtual string Code
        {
            get
            {
                return this.code;
            }
        }

        /// <summary>
        ///   Gets the measure dimension.
        /// </summary>
        public virtual string MeasureDimension
        {
            get
            {
                return this.measureDimension;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns concept scheme version. It tries to detect various conventions
        /// </summary>
        /// <param name="primaryMeasure">
        /// The primaryMeasure.
        /// </param>
        /// <returns>
        /// The concept scheme version; otherwise null
        /// </returns>
        private static string GetConceptSchemeVersion(CrossSectionalMeasureType primaryMeasure)
        {
            if (!string.IsNullOrWhiteSpace(primaryMeasure.conceptVersion))
            {
                return primaryMeasure.conceptVersion;
            }

            if (!string.IsNullOrWhiteSpace(primaryMeasure.ConceptSchemeVersionEstat))
            {
                return primaryMeasure.ConceptSchemeVersionEstat;
            }

            var extDimension = primaryMeasure as Org.Sdmx.Resources.SdmxMl.Schemas.V20.extension.structure.CrossSectionalMeasureType;
            if (extDimension != null && !string.IsNullOrWhiteSpace(extDimension.conceptSchemeVersion))
            {
                return extDimension.conceptSchemeVersion;
            }

            return null;
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this.measureDimension == null)
            {
                throw new SdmxSemmanticException(
                    "Cross Sectional Measure Dimensions missing mandatory Measure Dimensions reference");
            }

            if (this.code == null)
            {
                throw new SdmxSemmanticException("Cross Sectional Measure Dimensions missing mandatory Code reference");
            }

            if (((ICrossSectionalDataStructureObject)this.MaintainableParent).GetDimension(this.measureDimension)
                == null)
            {
                throw new SdmxSemmanticException(
                    "Cross Sectional Measure Dimensions references non-existent dimension " + this.measureDimension);
            }
        }

        #endregion
    }
}