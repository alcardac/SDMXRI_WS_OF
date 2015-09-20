// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAndMetadataSetMutableReferenceImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data and metadata set mutable reference impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   The data and metadata set mutable reference impl.
    /// </summary>
    public class DataAndMetadataSetMutableReferenceImpl : IDataAndMetadataSetMutableReference
    {
        #region Fields

        /// <summary>
        ///   The _is is data set reference.
        /// </summary>
        private bool _isIsDataSetReference;

        /// <summary>
        ///   The data set reference.
        /// </summary>
        private IStructureReference dataSetReference;

        /// <summary>
        ///   The set id.
        /// </summary>
        private string setId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataAndMetadataSetMutableReferenceImpl" /> class.
        /// </summary>
        public DataAndMetadataSetMutableReferenceImpl()
        {
            this.dataSetReference = null;
            this.setId = null;
            this._isIsDataSetReference = false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAndMetadataSetMutableReferenceImpl"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public DataAndMetadataSetMutableReferenceImpl(IDataAndMetadataSetReference immutable)
        {
            this.dataSetReference = null;
            this.setId = null;
            this._isIsDataSetReference = false;
            if (immutable.DataSetReference != null)
            {
                this.dataSetReference = immutable.DataSetReference.CreateMutableInstance();
            }

            this.setId = immutable.SetId;
            this._isIsDataSetReference = immutable.IsDataSetReference;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the data set reference.
        /// </summary>
        public virtual IStructureReference DataSetReference
        {
            get
            {
                return this.dataSetReference;
            }

            set
            {
                this.dataSetReference = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether is data set reference.
        /// </summary>
        public virtual bool IsDataSetReference
        {
            get
            {
                return this._isIsDataSetReference;
            }

            set
            {
                this._isIsDataSetReference = value;
            }
        }

        /// <summary>
        ///   Gets or sets the set id.
        /// </summary>
        public virtual string SetId
        {
            get
            {
                return this.setId;
            }

            set
            {
                this.setId = value;
            }
        }

        #endregion
    }
}