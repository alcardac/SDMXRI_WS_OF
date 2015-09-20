// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoapUtils.cs" company="Eurostat">
//   Date Created : 2013-09-13
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.Util.Xml
{
    using System.IO;
    using System.Xml;
    using Org.Sdmxsource.Sdmx.Util.Sdmx;

    /// <summary>
    /// The soap utils.
    /// </summary>
    public class SoapUtils
    {
        #region Public Methods

        /// <summary>
        /// Extract message from soap
        /// </summary>
        /// <param name="input">
        /// The input
        /// </param>
        /// <param name="writer">
        /// The writer
        /// </param>
        public static void ExtractSdmxMessage(Stream input, XmlWriter writer)
        {
            using (var reader = XmlReader.Create(input))
            {
                SdmxMessageUtil.FindSdmx(reader);
                if (reader.EOF)
                {
                   return;
                }
              writer.WriteStartDocument();
              writer.WriteNode(reader, false);
            }
        }

        #endregion
    }
}
