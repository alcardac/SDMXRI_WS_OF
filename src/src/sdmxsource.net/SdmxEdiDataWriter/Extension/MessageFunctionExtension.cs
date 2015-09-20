// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageFunctionExtension.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The message function extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Extension
{
    using System;
    using System.Text;

    using Org.Sdmxsource.Sdmx.EdiParser.Constants;

    /// <summary>
    /// The message function extension.
    /// </summary>
    public static class MessageFunctionExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the EDI string.
        /// </summary>
        /// <param name="messageFunction">
        /// The message function.
        /// </param>
        /// <returns>
        /// The EDI string
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// messageFunction;Not supported value.
        /// </exception>
        public static string GetEdiString(this MessageFunction messageFunction)
        {
            switch (messageFunction)
            {
                case MessageFunction.Null:
                    return null;
                case MessageFunction.StatisticalDefinitions:
                case MessageFunction.StatisticalData:
                    return messageFunction.ToString("D");
                case MessageFunction.DataSetList:
                    return "DSL";
                default:
                    throw new ArgumentOutOfRangeException("messageFunction", messageFunction, "Not supported value.");
            }
        }

        /// <summary>
        /// Gets from EDI string.
        /// </summary>
        /// <param name="ediStr">
        /// The EDI string.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Unknown Message Function
        /// </exception>
        /// <returns>
        /// The <see cref="MessageFunction"/>.
        /// </returns>
        public static MessageFunction GetFromEdiStr(string ediStr)
        {
            Array currentMfs = Enum.GetValues(typeof(MessageFunction));
            foreach (MessageFunction currentMf in currentMfs)
            {
                if (currentMf != MessageFunction.Null)
                {
                    if (currentMf.GetEdiString().Equals(ediStr))
                    {
                        return currentMf;
                    }
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;
            foreach (MessageFunction currentMf in currentMfs)
            {
                sb.Append(concat);
                sb.Append(currentMf.GetEdiString());
                concat = ", ";
            }

            throw new ArgumentException("Unknown Message Function : " + ediStr + " (valid types are - " + sb + ")");
        }

        /// <summary>
        /// Determines whether the specified message function defines an Edi message that can contain data
        /// </summary>
        /// <param name="messageFunction">
        /// The message function.
        /// </param>
        /// <returns>
        /// Returns true if the message function defines an Edi message that can contain data.
        /// </returns>
        public static bool IsData(this MessageFunction messageFunction)
        {
            return messageFunction == MessageFunction.StatisticalData || messageFunction == MessageFunction.DataSetList;
        }

        /// <summary>
        /// Determines whether the specified message function is structure.
        /// </summary>
        /// <param name="messageFunction">
        /// The message function.
        /// </param>
        /// <returns>
        /// Returns true if the message function defines an Edi message that can contain structures.
        /// </returns>
        public static bool IsStructure(this MessageFunction messageFunction)
        {
            return messageFunction == MessageFunction.StatisticalDefinitions || messageFunction == MessageFunction.DataSetList;
        }

        #endregion
    }
}