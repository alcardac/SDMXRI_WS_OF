// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAndMetadataSetReferenceCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data and metadata set reference impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference
{
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The data and metadata set reference impl.
    /// </summary>
    public class DataAndMetadataSetReferenceCore : IDataAndMetadataSetReference
    {
        #region Fields

        /// <summary>
        ///   The _icross reference.
        /// </summary>
        private readonly ICrossReference _icrossReference;

        /// <summary>
        ///   The _id.
        /// </summary>
        private readonly string _id;

        /// <summary>
        ///   The _is is data set reference.
        /// </summary>
        private readonly bool _isIsDataSetReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAndMetadataSetReferenceCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DataAndMetadataSetReferenceCore(IDataAndMetadataSetMutableReference mutable)
        {
            if (mutable.DataSetReference != null)
            {
                // this.IcrossReference = new CrossReferenceCore(this, mutable.getDataSetReference());
            }

            this._id = mutable.SetId;
            this._isIsDataSetReference = mutable.IsDataSetReference;
            if (this._icrossReference == null)
            {
                throw new SdmxSemmanticException(
                    "DataAndMetadataSetReferenceCore expects IcrossReference (null was provided)");
            }

            if (this._id == null)
            {
                throw new SdmxSemmanticException("DataAndMetadataSetReferenceCore expects id (null was provided)");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAndMetadataSetReferenceCore"/> class.
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference. 
        /// </param>
        /// <param name="id1">
        /// The id 1. 
        /// </param>
        /// <param name="isIsDataSetReference2">
        /// The is is data set reference 2. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DataAndMetadataSetReferenceCore(
            ICrossReference crossReference, string id1, bool isIsDataSetReference2)
        {
            this._icrossReference = crossReference;
            this._id = id1;
            this._isIsDataSetReference = isIsDataSetReference2;
            if (crossReference == null)
            {
                throw new SdmxSemmanticException(
                    "DataAndMetadataSetReferenceCore expects IcrossReference (null was provided)");
            }

            if (id1 == null)
            {
                throw new SdmxSemmanticException("DataAndMetadataSetReferenceCore expects id (null was provided)");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the data set reference.
        /// </summary>
        public virtual ICrossReference DataSetReference
        {
            get
            {
                return this._icrossReference;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether is data set reference.
        /// </summary>
        public virtual bool IsDataSetReference
        {
            get
            {
                return this._isIsDataSetReference;
            }
        }

        /// <summary>
        ///   Gets the set id.
        /// </summary>
        public virtual string SetId
        {
            get
            {
                return this._id;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The create mutable instance.
        /// </summary>
        /// <returns> The <see cref="IDataAndMetadataSetMutableReference" /> . </returns>
        public virtual IDataAndMetadataSetMutableReference CreateMutableInstance()
        {
            return new DataAndMetadataSetMutableReferenceImpl(this);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            var that = obj as IDataAndMetadataSetReference;
            if (that != null)
            {
                if (!ObjectUtil.Equivalent(that.DataSetReference, this.DataSetReference))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(that.SetId, this.SetId))
                {
                    return false;
                }

                if (this._isIsDataSetReference != that.IsDataSetReference)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The <see cref="int" /> . </returns>
        public override int GetHashCode()
        {
            return (this._icrossReference.TargetUrn + this._id + this._isIsDataSetReference).GetHashCode();
        }

        #endregion
    }
}