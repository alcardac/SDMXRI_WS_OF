// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomAnnotationTypeExtensions.cs" company="Eurostat">
//   Date Created : 2013-07-18
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The custom annotation type extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Constant
{
    using System.Linq;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///     The custom annotation type extensions.
    /// </summary>
    public static class CustomAnnotationTypeExtensions
    {
        #region Constants

        /// <summary>
        ///     The category scheme node order.
        /// </summary>
        private const string CategoryschemeNodeOrder = "CategoryScheme_node_order";

        /// <summary>
        /// The non production dataflow
        /// </summary>
        private const string NonProductionDataflow = "NonProductionDataflow";

        /// <summary>
        /// a constant holding the annotation title for crossX DSDs retrieved in 2.1 queries
        /// </summary>
        private const string Crossx21AnnotationTitle = "Only for SDMX v2.0";

        /// <summary>
        /// a constant holding the annotation text for crossX DSDs retrieved in 2.1 queries
        /// </summary>
        private const string Crossx21AnnotationText = "This is something that can be queried using only SDMX v2.0 endpoints";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the <see cref="CustomAnnotationType"/> from annotation.
        /// </summary>
        /// <param name="annotation">
        /// The annotation.
        /// </param>
        /// <returns>
        /// The <see cref="CustomAnnotationType"/>.
        /// </returns>
        public static CustomAnnotationType FromAnnotation(this IAnnotationMutableObject annotation)
        {
            if (annotation != null)
            {
                if (!string.IsNullOrWhiteSpace(annotation.Type))
                {
                    switch (annotation.Type)
                    {
                        case CategoryschemeNodeOrder:
                            return CustomAnnotationType.CategorySchemeNodeOrder;
                        case NonProductionDataflow:
                            return CustomAnnotationType.NonProductionDataflow;
                    }
                }
                else
                {
                    switch (annotation.Title)
                    {
                        case Crossx21AnnotationTitle:
                            return CustomAnnotationType.SDMXv20Only;
                    }
                }
            }

            return CustomAnnotationType.None;
        }

        /// <summary>
        /// Returns the <see cref="CustomAnnotationType"/> from annotation.
        /// </summary>
        /// <param name="annotation">
        /// The annotation.
        /// </param>
        /// <returns>
        /// The <see cref="CustomAnnotationType"/>.
        /// </returns>
        public static CustomAnnotationType FromAnnotation(this IAnnotation annotation)
        {
            if (annotation != null)
            {
                if (!string.IsNullOrWhiteSpace(annotation.Type))
                {
                    switch (annotation.Type)
                    {
                        case CategoryschemeNodeOrder:
                            return CustomAnnotationType.CategorySchemeNodeOrder;
                        case NonProductionDataflow:
                            return CustomAnnotationType.CategorySchemeNodeOrder;
                    }
                }
                else
                {
                    switch (annotation.Title)
                    {
                        case Crossx21AnnotationTitle:
                            return CustomAnnotationType.SDMXv20Only;
                    }
                }
            }

            return CustomAnnotationType.None;
        }

        /// <summary>
        /// Create and return an annotation of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="customAnnotation">
        /// The custom annotation.
        /// </param>
        /// <typeparam name="T">
        /// The concrete type of <see cref="IAnnotationMutableObject"/>
        /// </typeparam>
        /// <returns>
        /// The <typeparamref name="T"/>.
        /// </returns>
        public static T ToAnnotation<T>(this CustomAnnotationType customAnnotation) where T : IAnnotationMutableObject, new()
        {
            var annotation = new T();
            switch (customAnnotation)
            {
                case CustomAnnotationType.CategorySchemeNodeOrder:
                    annotation.Type = customAnnotation.ToStringValue();
                    break;
                case CustomAnnotationType.NonProductionDataflow:
                    annotation.Type = customAnnotation.ToStringValue();
                    break;
                case CustomAnnotationType.SDMXv20Only:
                    annotation = new T { Title = customAnnotation.ToStringValue() };
                    annotation.AddText("en", Crossx21AnnotationText);
                    break;
                case CustomAnnotationType.SDMXv21Only:
                    break;
            }

            return annotation;
        }

        /// <summary>
        /// Create and return an annotation of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="customAnnotation">
        /// The custom annotation.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// The concrete type of <see cref="IAnnotationMutableObject"/>
        /// </typeparam>
        /// <returns>
        /// The <typeparamref name="T"/>.
        /// </returns>
        public static T ToAnnotation<T>(this CustomAnnotationType customAnnotation, string value) where T : IAnnotationMutableObject, new()
        {
            var annotation = customAnnotation.ToAnnotation<T>();
            annotation.AddText("en", value);

            return annotation;
        }

        /// <summary>
        /// Returns the string value associated with <paramref name="customAnnotation"/>
        /// </summary>
        /// <param name="customAnnotation">
        /// The custom annotation.
        /// </param>
        /// <returns>
        /// The string value associated with <paramref name="customAnnotation"/>
        /// </returns>
        public static string ToStringValue(this CustomAnnotationType customAnnotation)
        {
            switch (customAnnotation)
            {
                case CustomAnnotationType.CategorySchemeNodeOrder:
                    return CategoryschemeNodeOrder;
                case CustomAnnotationType.NonProductionDataflow:
                    return NonProductionDataflow;
                case CustomAnnotationType.SDMXv20Only:
                    return Crossx21AnnotationTitle;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns  value from annotation.
        /// </summary>
        /// <param name="annotation">
        /// The annotation.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ValueFromAnnotation(this IAnnotationMutableObject annotation)
        {
            if (annotation != null && !string.IsNullOrWhiteSpace(annotation.Type) && annotation.Text.Count > 0)
            {
                return annotation.Text[0].Value;
            }

            return null;
        }

        /// <summary>
        /// Returns  value from annotation.
        /// </summary>
        /// <param name="annotation">
        /// The annotation.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ValueFromAnnotation(this IAnnotation annotation)
        {
            if (annotation != null && !string.IsNullOrWhiteSpace(annotation.Type) && annotation.Text.Count > 0)
            {
                return annotation.Text[0].Value;
            }

            return null;
        }

        /// <summary>
        /// Sets the annotation non production to a dataflow.
        /// </summary>
        /// <param name="dataflowMutableObject">The dataflow mutable object.</param>
        public static void SetNonProduction(this IDataflowMutableObject dataflowMutableObject)
        {
            var annotation = CustomAnnotationType.NonProductionDataflow.ToAnnotation<AnnotationMutableCore>(XmlConvert.ToString(true));
            dataflowMutableObject.AddAnnotation(annotation);
        }

        /// <summary>
        /// Determines whether the specified dataflow is not in production
        /// </summary>
        /// <param name="dataflow">The dataflow.</param>
        /// <returns>true if the dataflow is not in production; otherwise true.</returns>
        public static bool IsNonProduction(this IDataflowMutableObject dataflow)
        {
            return dataflow.Annotations.Any(o => o.FromAnnotation() == CustomAnnotationType.NonProductionDataflow && o.ValueFromAnnotation().Equals(XmlConvert.ToString(true)));
        }

        /// <summary>
        /// Determines whether the specified dataflow is not in production
        /// </summary>
        /// <param name="dataflow">The dataflow.</param>
        /// <returns>true if the dataflow is not in production; otherwise true.</returns>
        public static bool IsNonProduction(this IDataflowObject dataflow)
        {
            return dataflow.Annotations.Any(o => o.FromAnnotation() == CustomAnnotationType.NonProductionDataflow && o.ValueFromAnnotation().Equals(XmlConvert.ToString(true)));
        }


        #endregion
    }
}