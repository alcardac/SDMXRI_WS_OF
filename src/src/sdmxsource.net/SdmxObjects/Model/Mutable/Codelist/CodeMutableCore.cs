// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The code mutable core.
    /// </summary>
    [Serializable]
    public sealed class CodeMutableCore : ItemMutableCore, ICodeMutableObject
    {
        #region Static Fields

        /// <summary>
        ///   The _code type.
        /// </summary>
        private static readonly SdmxStructureType _codeType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code);

        #endregion

        #region Fields

        /// <summary>
        ///   The parent code.
        /// </summary>
        private string parentCode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CodeMutableCore" /> class.
        /// </summary>
        public CodeMutableCore()
            : base(_codeType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public CodeMutableCore(ICode objTarget)
            : base(objTarget)
        {
            this.parentCode = objTarget.ParentCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeMutableCore"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        public CodeMutableCore(ISdmxReader reader)
            : base(_codeType)
        {
            this.BuildIdentifiableAttributes(reader);

            reader.MoveNextElement();
            while (this.ProcessReader(reader))
            {
                reader.MoveNextElement();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the parent code.
        /// </summary>
        public string ParentCode
        {
            get
            {
                return this.parentCode;
            }

            set
            {
                this.parentCode = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The process reader.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal override bool ProcessReader(ISdmxReader reader)
        {
            if (base.ProcessReader(reader))
            {
                return true;
            }

            if (reader.CurrentElement.Equals("Parent"))
            {
                reader.MoveNextElement();
                if (!reader.CurrentElement.Equals("Ref"))
                {
                    throw new SdmxSemmanticException("Expected 'Ref' as a child node of Parent");
                }

                this.parentCode = reader.GetAttributeValue("id", true);
                return true;
            }

            return false;
        }

        #endregion
    }
}