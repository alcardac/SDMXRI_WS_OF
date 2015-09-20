// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist
{
    using System;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The codelist mutable core.
    /// </summary>
    [Serializable]
    public class CodelistMutableCore : ItemSchemeMutableCore<ICodeMutableObject, ICode, ICodelistObject>,
                                       ICodelistMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CodelistMutableCore" /> class.
        /// </summary>
        public CodelistMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMutableCore"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        public CodelistMutableCore(ISdmxReader reader)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList))
        {
            ValidateRootElement(reader);
            this.BuildMaintainableAttributes(reader);

            reader.MoveNextElement();
            while (this.ProcessReader(reader))
            {
                reader.MoveNextElement();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        public CodelistMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistMutableCore"/> class.
        /// </summary>
        /// <param name="codelist">
        /// The codelist. 
        /// </param>
        public CodelistMutableCore(ICodelistObject codelist)
            : base(codelist)
        {
            // change Codelist beans in Mutable Codelist beans
            if (codelist.Items != null)
            {
                foreach (ICode code in codelist.Items)
                {
                    this.AddItem(new CodeMutableCore(code));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override ICodelistObject ImmutableInstance
        {
            get
            {
                return new CodelistObjectCore(this);
            }
        }

        public ICodeMutableObject GetCodeById(string id)
        {
            return this.Items.FirstOrDefault(currentCode => currentCode.Id.Equals(id));
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
        protected new bool ProcessReader(ISdmxReader reader)
        {
            if (base.ProcessReader(reader))
            {
                return true;
            }

            if (reader.CurrentElement.Equals("Code"))
            {
                this.ProcessCodes(reader);
            }

            return false;
        }

        /// <summary>
        /// The validate root element.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private static void ValidateRootElement(ISdmxReader reader)
        {
            if (!reader.CurrentElement.Equals("Codelist"))
            {
                throw new SdmxSemmanticException(
                    "Can not construct codelist - expecting 'Codelist' Element in SDMX, actual element:"
                    + reader.CurrentElementValue);
            }
        }

        /// <summary>
        /// The process codes.
        /// </summary>
        /// <param name="reader">
        /// The reader. 
        /// </param>
        private void ProcessCodes(ISdmxReader reader)
        {
            while (reader.CurrentElement.Equals("Code"))
            {
                ICodeMutableObject newCode = new CodeMutableCore(reader);
                ////if (newCode.Name == null || newCode.Name.Count == 0 || string.IsNullOrWhiteSpace(newCode.Name[0].Value))
                ////{
                ////    // Remove console output
                ////    ////Console.Out.WriteLine("HERE");
                ////}

                this.AddItem(newCode);
            }
        }

        #endregion

        #region Overrides of ItemSchemeMutableCore<ICodeMutableObject,ICode,ICodelistObject>

        public override ICodeMutableObject CreateItem(string id, string name)
        {
            ICodeMutableObject code = new CodeMutableCore();
            code.Id = id;
            code.AddName("en", name);
            AddItem(code);
            return code;
        }

        #endregion
    }
}