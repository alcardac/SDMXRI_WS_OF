// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType;

    /// <summary>
    ///     The abstract bean assembler.
    /// </summary>
    public class AbstractAssembler
    {
        #region Constants

        // This character MUST be a character that is not a valid character in an ID.
        private const string DELIMITER = "~";

        /// <summary>
        ///     The default lang.
        /// </summary>
        private const string DefaultLang = "en";

        #endregion

        #region Methods

        /// <summary>
        /// Gets annotations type.
        /// </summary>
        /// <param name="annotable">
        /// The annotable.
        /// </param>
        /// <returns>
        /// The <see cref="AnnotationsType"/>.
        /// </returns>
        internal AnnotationsType GetAnnotationsType(IAnnotableObject annotable)
        {
            if (!ObjectUtil.ValidCollection(annotable.Annotations))
            {
                return null;
            }

            var returnType = new AnnotationsType();

            /* foreach */
            foreach (IAnnotation currentAnnotationBean in annotable.Annotations)
            {
                var annotation = new AnnotationType();
                returnType.Annotation.Add(annotation);
                string value2 = currentAnnotationBean.Id;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    annotation.id = currentAnnotationBean.Id;
                }

                if (ObjectUtil.ValidCollection(currentAnnotationBean.Text))
                {
                    annotation.AnnotationText = this.GetTextType(currentAnnotationBean.Text);
                }

                string value1 = currentAnnotationBean.Title;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    annotation.AnnotationTitle = currentAnnotationBean.Title;
                }

                string value = currentAnnotationBean.Type;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    annotation.AnnotationType1 = currentAnnotationBean.Type;
                }

                if (currentAnnotationBean.Uri != null)
                {
                    annotation.AnnotationURL = currentAnnotationBean.Uri;
                }
            }

            return returnType;
        }

        /// <summary>
        /// Transform and Copy the <see cref="ITextTypeWrapper"/> from <paramref name="source"/> to <see cref="Name"/>
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="destination">
        /// The destination.
        /// </param>
        internal void GetTextType(IList<ITextTypeWrapper> source, IList<Name> destination)
        {
            for (int i = 0; i < source.Count; i++)
            {
                destination.Add(new Name(this.GetTextType(source[i])));
            }
        }

        /// <summary>
        /// Transform and Copy the <see cref="ITextTypeWrapper"/> from <paramref name="source"/> to <see cref="Name"/>
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="destination">
        /// The destination.
        /// </param>
        internal void GetTextType(IList<ITextTypeWrapper> source, IList<Description> destination)
        {
            for (int i = 0; i < source.Count; i++)
            {
                destination.Add(new Description(this.GetTextType(source[i])));
            }
        }

        /// <summary>
        /// Gets the <see cref="Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType"/> from the specified
        ///     <paramref name="textTypeWrappers"/>
        /// </summary>
        /// <param name="textTypeWrappers">
        /// The text Type Wrappers
        /// </param>
        /// <returns>
        /// The the <see cref="Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType"/> from the specified
        ///     <paramref name="textTypeWrappers"/>
        /// </returns>
        internal TextType[] GetTextType(IList<ITextTypeWrapper> textTypeWrappers)
        {
            if (!ObjectUtil.ValidCollection(textTypeWrappers))
            {
                return null;
            }

            var textTypes = new TextType[textTypeWrappers.Count];
            for (int i = 0; i < textTypeWrappers.Count; i++)
            {
                TextType tt = this.GetTextType(textTypeWrappers[i]);
                textTypes[i] = tt;
            }

            return textTypes;
        }

        /// <summary>
        /// Gets the <see cref="TextType"/> from the specified <paramref name="textTypeWrapper"/>
        /// </summary>
        /// <param name="textTypeWrapper">
        /// The text Type Wrapper
        /// </param>
        /// <returns>
        /// The the <see cref="TextType"/> from the specified <paramref name="textTypeWrapper"/>
        /// </returns>
        internal TextType GetTextType(ITextTypeWrapper textTypeWrapper)
        {
            var tt = new TextType { lang = textTypeWrapper.Locale, TypedValue = textTypeWrapper.Value };
            return tt;
        }

        /// <summary>
        /// Gets the <see cref="TextType"/> from the specified <paramref name="englishString"/>
        /// </summary>
        /// <param name="englishString">
        /// The text
        /// </param>
        /// <returns>
        /// The the <see cref="TextType"/> from the specified <paramref name="englishString"/>
        /// </returns>
        internal TextType GetTextType(string englishString)
        {
            var tt = new TextType { lang = DefaultLang, TypedValue = englishString };
            return tt;
        }

        /// <summary>
        /// The has annotations.
        /// </summary>
        /// <param name="annotable">
        /// The annotable.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        internal bool HasAnnotations(IAnnotableObject annotable)
        {
            if (ObjectUtil.ValidCollection(annotable.Annotations))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets reference.
        /// </summary>
        /// <param name="refBase">
        /// The refBase.
        /// </param>
        /// <param name="crossReference">
        /// The cross reference.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="crossReference"/> is null
        ///     -or-
        ///     <paramref name="refBase"/> is null
        /// </exception>
        /// <exception cref="ValidationException">
        /// URN class is null
        /// </exception>
        protected internal void SetReference(RefBaseType refBase, IStructureReference crossReference)
        {
            if (refBase == null)
            {
                throw new ArgumentNullException("refBase");
            }

            if (crossReference == null)
            {
                throw new ArgumentNullException("crossReference");
            }

            IMaintainableRefObject maintRef = crossReference.MaintainableReference;
            if (crossReference.HasChildReference())
            {
                string fullId = GetFullIdentifiableId(crossReference);

                int lastIndexOf = fullId.LastIndexOf(DELIMITER);
                if (lastIndexOf > -1)
                {
                    string containerId = fullId.Substring(0, lastIndexOf);
                    string targetId = fullId.Substring(lastIndexOf, fullId.Length - lastIndexOf);
                    refBase.containerID = containerId;
                    refBase.id = targetId;
                }
                else
                {
                    refBase.id = fullId;
                }

                string value = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    refBase.maintainableParentID = maintRef.MaintainableId;
                }

                string value1 = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    refBase.maintainableParentVersion = maintRef.Version;
                }
            }
            else
            {
                string value = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    refBase.id = maintRef.MaintainableId;
                }

                string value1 = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    refBase.version = maintRef.Version;
                }
            }

            string value2 = maintRef.AgencyId;
            if (!string.IsNullOrWhiteSpace(value2))
            {
                refBase.agencyID = maintRef.AgencyId;
            }

            //// TODO because of differences in the Xml and LINQ2XSD generated code setting package and class might not be needed.
            SdmxStructureType sdmxStructureType = crossReference.TargetReference;

            //// TODO Assume that the PackageTypeCodelistType are the same as in SdmxStructureType.UrnPackage
            string urnPackage = sdmxStructureType.UrnPackage;
            refBase.package = urnPackage; //// PackageTypeCodelistType.TypeDefinition.

            string classEnum;
            switch (crossReference.TargetReference.EnumType)
            {
                //Place any exclusions to the rule in this case statement
                case SdmxStructureEnumType.DataAttribute:
                    classEnum = ObjectTypeCodelistTypeConstants.Attribute;
                    break;
                default:
                    string urnClass = crossReference.TargetReference.UrnClass;
                    classEnum = urnClass;//ObjectTypeCodelistType.Enum.forString(urnClass);
                    break;
            }

            if (classEnum == null)
            {
                throw new SdmxSemmanticException("Unknown urnClass : " + crossReference.TargetReference.UrnClass);
            }

            refBase.@class = classEnum;
        }

        /// <summary>
        /// Gets full identifiable id.
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetFullIdentifiableId(IStructureReference crossReference)
        {
            string returnString = string.Empty;
            IIdentifiableRefObject childReference = crossReference.ChildReference;
            string concat = string.Empty;
            SdmxStructureType type = null;
            while (childReference != null)
            {
                if (type != null && childReference.StructureEnumType != type)
                {
                    // Change the concat character into a symbol that is not in an ID.
                    // We do this so we can't hunt for it later
                    concat = DELIMITER;
                }
                returnString += concat + childReference.Id;
                type = childReference.StructureEnumType;  //Store the previous type
                concat = ".";
                childReference = childReference.ChildReference;
            }

            return returnString;
        }

        #endregion
    }
}