// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputOutputMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The input output mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Process
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The input output mutable core.
    /// </summary>
    [Serializable]
    public class InputOutputMutableCore : AnnotableMutableCore, IInputOutputMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _local id.
        /// </summary>
        private string _localId;

        /// <summary>
        ///   The _structure reference.
        /// </summary>
        private IStructureReference _structureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="InputOutputMutableCore" /> class.
        /// </summary>
        public InputOutputMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.InputOutput))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputOutputMutableCore"/> class.
        /// </summary>
        /// <param name="inputOutputObject">
        /// The inputOutputObject. 
        /// </param>
        public InputOutputMutableCore(IInputOutputObject inputOutputObject)
            : base(inputOutputObject)
        {
            this._localId = inputOutputObject.LocalId;
            if (inputOutputObject.StructureReference != null)
            {
                this._structureReference = inputOutputObject.StructureReference.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the local id.
        /// </summary>
        public virtual string LocalId
        {
            get
            {
                return this._localId;
            }

            set
            {
                this._localId = value;
            }
        }

        /// <summary>
        ///   Gets or sets the structure reference.
        /// </summary>
        public virtual IStructureReference StructureReference
        {
            get
            {
                return this._structureReference;
            }

            set
            {
                this._structureReference = value;
            }
        }

        #endregion
    }
}