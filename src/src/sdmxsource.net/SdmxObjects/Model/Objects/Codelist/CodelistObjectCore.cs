// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using CodeType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.CodeType;

    /// <summary>
    ///   The codelist object core.
    /// </summary>
    [Serializable]
    public class CodelistObjectCore :
        ItemSchemeObjectCore<ICode, ICodelistObject, ICodelistMutableObject, ICodeMutableObject>, 
        ICodelistObject
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistObjectCore"/> class.
        /// </summary>
        /// <param name="codelist">
        /// The codelist. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistObjectCore(ICodelistMutableObject codelist)
            : base(codelist)
        {
            try
            {
                if (codelist.Items != null)
                {
                    foreach (ICodeMutableObject code in codelist.Items)
                    {
                        this.AddInternalItem(new CodeCore(this, code));
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM READER                    //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        // public CodelistObjectCore(SdmxReader reader) {
        // super(validateRootElement(reader), reader);
        // if(reader.moveToElement("Code", "Codelist")) {
        // items.add(new CodeCore(this, reader));
        // if(reader.peek().equals("Code")) {
        // reader.moveNextElement();
        // while(reader.getCurrentElement().equals("Code")) {
        // items.add(new CodeCore(this, reader));
        // }
        // }
        // }
        // }

        // private static SdmxStructureType validateRootElement(SdmxReader reader) {
        // if(!reader.getCurrentElement().equals("Codelist")) {
        // throw new SdmxSemmanticException("Can not construct codelist - expecting 'Codelist' Element in SDMX, actual element:" + reader.getCurrentElementValue());
        // }
        // return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistObjectCore"/> class.
        /// </summary>
        /// <param name="codeList">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistObjectCore(CodeListType codeList)
            : base(
                codeList, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList), 
                codeList.version, 
                codeList.agency, 
                codeList.id, 
                codeList.uri, 
                codeList.Name, 
                CreateTertiary(codeList.isExternalReference), 
                codeList.Annotations)
        {
            try
            {
                foreach (CodeType currentCode in codeList.Code)
                {
                    this.AddInternalItem(new CodeCore(this, currentCode));
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistObjectCore"/> class.
        /// </summary>
        /// <param name="codeList">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CodeListType codeList)
            : base(
                codeList, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList), 
                codeList.validTo, 
                codeList.validFrom, 
                codeList.version, 
                CreateTertiary(codeList.isFinal), 
                codeList.agencyID, 
                codeList.id, 
                codeList.uri, 
                codeList.Name, 
                codeList.Description, 
                CreateTertiary(codeList.isExternalReference), 
                codeList.Annotations)
        {
            try
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CodeType currentCode in codeList.Code)
                {
                    this.AddInternalItem(new CodeCore(this, currentCode));
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistObjectCore"/> class.
        /// </summary>
        /// <param name="codelist">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CodelistObjectCore(CodelistType codelist)
            : base(codelist, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList))
        {
            try
            {
                foreach (Code currentCode in codelist.Item)
                {
                    this.AddInternalItem(new CodeCore(this, currentCode.Content));
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodelistObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private CodelistObjectCore(ICodelistObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override ICodelistMutableObject MutableInstance
        {
            get
            {
                return new CodelistMutableCore(this);
            }
        }

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
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
                return this.DeepEqualsInternal((ICodelistObject)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get code by id.
        /// </summary>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <returns>
        /// The <see cref="ICode"/> . 
        /// </returns>
        public virtual ICode GetCodeById(string id)
        {
            foreach (ICode currentCode in this.Items)
            {
                if (currentCode.Id.Equals(id))
                {
                    return currentCode;
                }
            }

            return null;
        }

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistObject"/> . 
        /// </returns>
        public override ICodelistObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new CodelistObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            // Not allowed to start with an integer
            base.ValidateId(false);
        }

        /// <summary>
        /// The get code.
        /// </summary>
        /// <param name="codes">
        /// The codes. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <returns>
        /// The <see cref="ICode"/> . 
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private static ICode GetCode(IDictionary<string, ICode> codes, string id)
        {
            // NOT in Java. Added for performance reasons in .NET do not replace blindly in the next sync
            ICode code;
            if (codes.TryGetValue(id, out code))
            {
                return code;
            }

            throw new SdmxSemmanticException(ExceptionCode.CannotResolveParent, id);
        }

        /// <summary>
        /// Iterates the map checking the children of each child, if one of the children is the parent code, then an exception is thrown
        /// </summary>
        /// <param name="children">
        /// The children. 
        /// </param>
        /// <param name="parentCode">
        /// The parent Code. 
        /// </param>
        /// <param name="parentChildMap">
        /// The parent Child Map. 
        /// </param>
        private static void IterateParentMap(
            ISet<ICode> children, ICode parentCode, IDictionary<ICode, HashSet<ICode>> parentChildMap)
        {
            if (children == null)
            {
                return;
            }

            var stack = new Stack<ISet<ICode>>();
            stack.Push(children);
            while (stack.Count > 0)
            {
                children = stack.Pop();

                // If the child is also a parent
                if (children.Contains(parentCode))
                {
                    throw new SdmxSemmanticException(ExceptionCode.ParentRecursiveLoop, parentCode.Id);
                }

                foreach (ICode currentChild in children)
                {
                    HashSet<ICode> codeChildren;
                    if (parentChildMap.TryGetValue(currentChild, out codeChildren) && codeChildren != null)
                    {
                        stack.Push(codeChildren);
                    }
                }
            }
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            var urns = new HashSet<Uri>();
            if (this.Items != null)
            {
                // CHECK FOR DUPLICATION OF URN & ILLEGAL PARENTING
                IDictionary<ICode, HashSet<ICode>> parentChildMap = new Dictionary<ICode, HashSet<ICode>>();

                // NOT in Java. Added for performance reasons in .NET do not replace blindly in the next sync
                IDictionary<string, ICode> codeSet = this.Items.ToDictionary(code => code.Id, StringComparer.Ordinal);
                foreach (ICode code in this.Items)
                {
                    if (urns.Contains(code.Urn))
                    {
                        throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, code.Urn);
                    }

                    urns.Add(code.Urn);

                    if (!string.IsNullOrWhiteSpace(code.ParentCode))
                    {
                        ICode parentCode = GetCode(codeSet, code.ParentCode);
                        HashSet<ICode> children;
                        
                        if (!parentChildMap.TryGetValue(parentCode, out children))
                        {
                            children = new HashSet<ICode>();
                            parentChildMap.Add(parentCode, children);
                        }

                        children.Add(code);

                        // Check that the parent code is not directly or indirectly a child of the code it is parenting
                        HashSet<ICode> codeChildren;
                        if (parentChildMap.TryGetValue(code, out codeChildren))
                        {
                            IterateParentMap(codeChildren, parentCode, parentChildMap);
                        }
                    }
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                               //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
    }
}