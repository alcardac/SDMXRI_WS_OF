// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexAnnotationReferenceImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The complex annotation reference core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex
{
    #region Using directives

    using System;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion

    /// <summary>
    ///   The complex annotation reference.
    /// </summary>
    [Serializable]
    public class ComplexAnnotationReferenceCore : IComplexAnnotationReference
    {
        #region Fields

        /// <summary>
        ///   The type ref.
        /// </summary>
        private IComplexTextReference _typeRef;

        /// <summary>
        ///   The title ref.
        /// </summary>
        private IComplexTextReference _titleRef;

        /// <summary>
        ///   The text ref.
        /// </summary>
        private IComplexTextReference _textRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexAnnotationReferenceCore"/> class.
        /// </summary>
        /// <param name="typeRef">
        /// The type ref. 
        /// </param>
        /// <param name="titleRef">
        /// The title ref. 
        /// </param>
        /// <param name="textRef">
        /// The text ref. 
        /// </param>
        public ComplexAnnotationReferenceCore(IComplexTextReference typeRef, IComplexTextReference titleRef, IComplexTextReference textRef)
        {
            this._typeRef = typeRef;
            this._titleRef = titleRef;
            this._textRef = textRef;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the type reference.
        /// </summary>
        public virtual IComplexTextReference TypeReference
        {
             get
             {
                 return _typeRef;
             }
        }

        /// <summary>
        ///   Gets the title reference.
        /// </summary>
        public virtual IComplexTextReference TitleReference
        {
            get
            {
                return _titleRef;
            }
        }

        /// <summary>
        ///   Gets the text reference.
        /// </summary>
        public virtual IComplexTextReference TextReference
        {
           get
           {
               return _textRef;
           }
        }

        #endregion

    }
}
