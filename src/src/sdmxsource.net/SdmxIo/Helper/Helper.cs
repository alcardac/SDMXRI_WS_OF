// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class contains helper static methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;

    /// <summary>
    ///     This class contains helper static methods
    /// </summary>
    public static class Helper
    {
        #region Public Methods and Operators

        /// <summary>
        /// This internal method is used to generate a dummy header in case no header was already provided
        /// </summary>
        /// <param name="id">
        /// The dataflow identifier used to populate the header identifier property <see cref="IHeader.Id"/>
        /// </param>
        /// <param name="senderId">
        /// The sender ID to use
        /// </param>
        /// <returns>
        /// This method return the newly created <see cref="IHeader"/>
        /// </returns>
        public static IHeader BuildSimpleHeaderObject(string id, string senderId)
        {
            // create header
            IHeader header = new HeaderImpl(id, senderId);

            //// TODO set test to true
            return header;
        }

        /// <summary>
        /// Gets the attachment level v 2.
        /// </summary>
        /// <param name="attachmentLevel">
        /// The attachment level.
        /// </param>
        /// <returns>
        /// The attachment level as string
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Unknown attachment level
        /// </exception>
        public static string GetAttachmentLevelV2(AttributeAttachmentLevel attachmentLevel)
        {
            switch (attachmentLevel)
            {
                case AttributeAttachmentLevel.DataSet:
                    return AttachmentLevelTypeConstants.DataSet;
                case AttributeAttachmentLevel.DimensionGroup:
                    return AttachmentLevelTypeConstants.Series;
                case AttributeAttachmentLevel.Observation:
                    return AttachmentLevelTypeConstants.Observation;
                case AttributeAttachmentLevel.Group:
                    return AttachmentLevelTypeConstants.Group;
                default:
                    throw new ArgumentException("Unknown attachment level: " + attachmentLevel);
            }
        }

        /// <summary>
        /// Merge two arrays of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type
        /// </typeparam>
        /// <param name="arrayOne">
        /// The first array
        /// </param>
        /// <param name="arrayTwo">
        /// The second array
        /// </param>
        /// <returns>
        /// a new array [ <paramref name="arrayOne"/> , <paramref name="arrayTwo"/> ]
        /// </returns>
        public static T[] MergeArray<T>(T[] arrayOne, T[] arrayTwo)
        {
            var allCelss = new T[arrayTwo.Length + arrayOne.Length];
            Array.Copy(arrayTwo, 0, allCelss, 0, arrayTwo.Length);
            Array.Copy(arrayOne, 0, allCelss, arrayTwo.Length, arrayOne.Length);
            return allCelss;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <typeparam name="T">
        /// The Enumeration type
        /// </typeparam>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/>
        ///     is not an Enumeration
        /// </exception>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static T TrySetEnumFromAttribute<T>(
            IDictionary<string, string> attributes, AttributeNameTable attribute, T property) where T : struct
        {
            return TrySetEnumFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <typeparam name="T">
        /// The Enumeration type
        /// </typeparam>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/>
        ///     is not an Enumeration
        /// </exception>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static T TrySetEnumFromAttribute<T>(IDictionary<string, string> attributes, string attribute, T property)
            where T : struct
        {
            Type ttype = typeof(T);
            if (!ttype.IsEnum)
            {
                throw new ArgumentException("T is not an Enum");
            }

            string ret;
            T value;
            if (!attributes.TryGetValue(attribute, out ret) || !Enum.TryParse(ret, out value))
            {
                return property;
            }

            return value;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static string TrySetFromAttribute(
            IDictionary<string, string> attributes, string attribute, string property)
        {
            string ret;
            if (!attributes.TryGetValue(attribute, out ret))
            {
                return property;
            }

            return ret;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static Uri TrySetFromAttribute(IDictionary<string, string> attributes, string attribute, Uri property)
        {
            string ret;
            Uri uri;
            if (!attributes.TryGetValue(attribute, out ret) || !Uri.TryCreate(ret, UriKind.RelativeOrAbsolute, out uri))
            {
                return property;
            }

            return uri;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static TertiaryBool TrySetFromAttribute(
            IDictionary<string, string> attributes, string attribute, TertiaryBool property)
        {
            string xmlBoolVal;
            if (attributes.TryGetValue(attribute, out xmlBoolVal))
            {
                return TertiaryBool.ParseBoolean(IsTrue(xmlBoolVal));
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static bool TrySetFromAttribute(IDictionary<string, string> attributes, string attribute, bool property)
        {
            string xmlBoolVal;
            if (attributes.TryGetValue(attribute, out xmlBoolVal))
            {
                return IsTrue(xmlBoolVal);
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static string TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, string property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static Uri TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, Uri property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static bool TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, bool property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static TertiaryBool TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, TertiaryBool property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static long? TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, long? property)
        {
            string name = NameTableCache.GetAttributeName(attribute);
            string xmlIntVal;
            long value;
            if (attributes.TryGetValue(name, out xmlIntVal) && long.TryParse(xmlIntVal, out value))
            {
                return value;
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static decimal? TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, decimal? property)
        {
            string name = NameTableCache.GetAttributeName(attribute);
            string xmlIntVal;
            decimal value;
            if (attributes.TryGetValue(name, out xmlIntVal) && decimal.TryParse(xmlIntVal, out value))
            {
                return value;
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static int TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, int property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static DateTime TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, DateTime property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static DateTime? TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, DateTime? property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static double TrySetFromAttribute(
            IDictionary<string, string> attributes, AttributeNameTable attribute, double property)
        {
            return TrySetFromAttribute(attributes, NameTableCache.GetAttributeName(attribute), property);
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static int TrySetFromAttribute(IDictionary<string, string> attributes, string attribute, int property)
        {
            string xmlIntVal;
            int n;
            if (attributes.TryGetValue(attribute, out xmlIntVal) && int.TryParse(xmlIntVal, out n))
            {
                return n;
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static DateTime TrySetFromAttribute(
            IDictionary<string, string> attributes, string attribute, DateTime property)
        {
            string xmlIntVal;
            DateTime n;
            if (attributes.TryGetValue(attribute, out xmlIntVal) && DateTime.TryParse(xmlIntVal, out n))
            {
                return n;
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static DateTime? TrySetFromAttribute(
            IDictionary<string, string> attributes, string attribute, DateTime? property)
        {
            string xmlIntVal;
            DateTime n;
            if (attributes.TryGetValue(attribute, out xmlIntVal) && DateTime.TryParse(xmlIntVal, out n))
            {
                return n;
            }

            return property;
        }

        /// <summary>
        /// Try to get the value from the given attribute (name,value) dictionary if any. Else return the existing value of given property.
        /// </summary>
        /// <param name="attributes">
        /// A dictionary with attribute (name, value) pairs
        /// </param>
        /// <param name="attribute">
        /// The attribute name we need to get the value from
        /// </param>
        /// <param name="property">
        /// The default value in case the attribute is not in the dictionary
        /// </param>
        /// <returns>
        /// The value of the attribute if any, else the value of the property
        /// </returns>
        public static double TrySetFromAttribute(
            IDictionary<string, string> attributes, string attribute, double property)
        {
            string xmlDoubleVal;
            double d;

            if (attributes.TryGetValue(attribute, out xmlDoubleVal) && double.TryParse(xmlDoubleVal, out d))
            {
                return d;
            }

            return property;
        }

        /// <summary>
        /// Try set Uri from text, only if the text contains a valid Uri
        /// </summary>
        /// <param name="text">
        /// THe uri
        /// </param>
        /// <returns>
        /// The Uri if <paramref name="text"/> contains a valid <see cref="Uri"/> else <c>null</c>
        /// </returns>
        public static Uri TrySetUri(string text)
        {
            Uri uri;
            if (string.IsNullOrEmpty(text) || !Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out uri))
            {
                return null;
            }

            return uri;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the given <paramref name="xmlBoolValue"/> is "true" or "1". Else if it is null, empty or some other value return false.
        /// </summary>
        /// <param name="xmlBoolValue">
        /// The <c>xs:bool</c> type value
        /// </param>
        /// <returns>
        /// true if <paramref name="xmlBoolValue"/> is "true" or "1". Else false
        /// </returns>
        internal static bool IsTrue(string xmlBoolValue)
        {
            return !string.IsNullOrEmpty(xmlBoolValue) && XmlConvert.ToBoolean(xmlBoolValue);
        }

        #endregion
    }
}