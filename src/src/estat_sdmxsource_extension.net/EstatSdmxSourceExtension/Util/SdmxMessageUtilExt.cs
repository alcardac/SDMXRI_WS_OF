// -----------------------------------------------------------------------
// <copyright file="SdmxMessageUtilExt.cs" company="EUROSTAT">
//   Date Created : 2015-02-02
//   Copyright (c) 2015 by the European   Commission, represented by Eurostat.   All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Util
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;
    using Org.Sdmxsource.Sdmx.Util.Xml;

    /// <summary>
    ///     Additional methods to the <see cref="SdmxMessageUtil" />
    /// </summary>
    public static class SdmxMessageUtilExt
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parses the SDMX footer message.
        /// </summary>
        /// <param name="dataLocation">
        /// The data location.
        /// </param>
        /// <returns>
        /// The <see cref="IFooterMessage"/> if there is one; otherwise null.
        /// </returns>
        public static IFooterMessage ParseSdmxFooterMessage(IReadableDataLocation dataLocation)
        {
            using (var stream = dataLocation.InputStream)
            using (XmlReader reader = XmlReader.Create(stream))
            {
                if (!StaxUtil.SkipToNode(reader, ElementNameTable.Footer.ToString()))
                {
                    return null;
                }

                var severity = Severity.Error;
                string text = null;
                string lang = null;
                string code = null;
                IList<ITextTypeWrapper> textType = new List<ITextTypeWrapper>();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                string nodeName = reader.LocalName;
                                if (nodeName.Equals("Message"))
                                {
                                    code = reader.GetAttribute("code");

                                    var attribute = reader.GetAttribute("severity");
                                    if (attribute != null)
                                    {
                                        Enum.TryParse(attribute, out severity);
                                    }
                                }

                                if (nodeName.Equals(ElementNameTable.AnnotationText.ToString()))
                                {
                                    lang = reader.XmlLang;
                                }

                                break;
                            }

                        case XmlNodeType.Text:
                            text = reader.Value;
                            break;

                        case XmlNodeType.EndElement:
                            {
                                string nodeName = reader.LocalName;
                                if (nodeName.Equals(ElementNameTable.AnnotationText.ToString()))
                                {
                                    textType.Add(new TextTypeWrapperImpl(lang, text, null));
                                }

                                break;
                            }
                    }
                }

                return new FooterMessageCore(code, severity, textType);
            }
        }

        #endregion
    }
}