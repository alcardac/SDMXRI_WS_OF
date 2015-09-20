// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxReader.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    ///     The SdmxReader is a technology independent definition of a reader for SDMX-ML files
    /// </summary>
    public interface ISdmxReader
    {
        /**
         * Reads the next element enforcing it is one of the following elements
         * @param expectedElement this can not be null
         * @return will return a value that matches one of the expected values, response can never be null
         * @throws SdmxSemmanticException if the next element does not match the expected element
         */

        // string readNextElement(string...expectedElement) throws SdmxSemmanticException;

        /**
         * Takes a peek at the next element without moving the parser forward
         * @return
         */

        // string peek();
        #region Public Properties

        /// <summary>
        ///     Gets a map of all the attributes present on the current node
        /// </summary>
        /// <value> </value>
        IDictionary<string, string> Attributes { get; }

        /// <summary>
        ///     Gets the name of the currentElement
        /// </summary>
        /// <value> </value>
        string CurrentElement { get; }

        /// <summary>
        ///     Gets the value in the current element - returns null if there is no value
        /// </summary>
        /// <value> </value>
        string CurrentElementValue { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes any underlying resource
        /// </summary>
        void Close();

        /// <summary>
        /// Gets an attribute value with the given name, if mandatory is true then will enforce the response is not null
        /// </summary>
        /// <param name="attributeName">The attribute name. </param>
        /// <param name="mandatory">Mandatory parameter. </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// if mandatory is true and the attribute is not present
        /// </exception>
        string GetAttributeValue(string attributeName, bool mandatory);

        /// <summary>
        ///     Reads the next element returning the value.  The response is null if there is no more elements to read
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool MoveNextElement();

        /// <summary>
        /// Moves to the position in the file with the element name, returns true if the move was successful, false if no element was found
        /// </summary>
        /// <param name="element">Element parameter. </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool MoveToElement(string element);

        /// <summary>
        /// The move to element.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="doNoMovePast">
        /// The do no move past.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool MoveToElement(string element, string doNoMovePast);

        #endregion
    }
}