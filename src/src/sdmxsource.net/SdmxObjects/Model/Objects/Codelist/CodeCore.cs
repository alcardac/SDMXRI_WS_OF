// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The code core.
    /// </summary>
    [Serializable]
    public class CodeCore : ItemCore, ICode
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
        private readonly string parentCode;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public CodeCore(ICodelistObject parent, ICodeMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            this.parentCode = string.IsNullOrWhiteSpace(itemMutableObject.ParentCode) ? null : itemMutableObject.ParentCode;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public CodeCore(ICodelistObject parent, SdmxReader reader) {
        // super(validateRootElement(reader), reader, parent);
        // if(reader.moveToElement("Parent", "Code")) {
        // reader.moveNextElement();  //Move to the Ref Node
        // if(!reader.getCurrentElement().equals("Ref")) {
        // throw new SdmxSemmanticException("Expecting 'Ref' element after 'Parent' element for Code");
        // }
        // this.parentCode = reader.getAttributeValue("id", true);
        // } 
        // }

        // private static SdmxStructureType validateRootElement(SdmxReader reader) {
        // if(!reader.getCurrentElement().equals("Code")) {
        // throw new SdmxSemmanticException("Can not construct code - expecting 'Code' Element in SDMX, actual element:" + reader.getCurrentElementValue());
        // }
        // return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CODE);
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="code">
        /// The sdmxObject. 
        /// </param>
        public CodeCore(ICodelistObject parent, CodeType code)
            : base(code, _codeType, parent)
        {
            var parentItem = code.GetTypedParent<LocalCodeReferenceType>();
            this.parentCode = (parentItem != null) ? parentItem.GetTypedRef<LocalCodeRefType>().id : null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="code">
        /// The sdmxObject. 
        /// </param>
        public CodeCore(ICodelistObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CodeType code)
            : base(code, _codeType, code.value, null, code.Description, null, code.Annotations, parent)
        {
            // In SDMX 2.0 it is perfectly valid for the XML to state a code has a parentCode which is blank. e.g.:
            //    <str:Code value="aCode" parentCode="">
            // This can cause issues when manipulating the beans, so police the input by not setting the 
            // parentCode if it is an empty string.
            this.parentCode = string.IsNullOrWhiteSpace(code.parentCode) ? null : code.parentCode;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="code">
        /// The sdmxObject. 
        /// </param>
        public CodeCore(ICodelistObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.CodeType code)
            : base(code, _codeType, code.value, null, code.Description, null, code.Annotations, parent)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the parent code.
        /// </summary>
        public virtual string ParentCode
        {
            get
            {
                return this.parentCode;
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
                var that = (ICode)sdmxObject;
                if (!ObjectUtil.Equivalent(this.parentCode, that.ParentCode))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion
    }
}