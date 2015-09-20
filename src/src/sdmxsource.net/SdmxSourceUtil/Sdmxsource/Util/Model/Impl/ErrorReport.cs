// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorReport.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Model.Impl
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    #endregion


    /// <summary>
    /// The error report.
    /// </summary>
    [Serializable]
    public class ErrorReport
    {
        // TODO private static readonly SerializeUtil<List<String>> su = new SerializeUtil<List<String>>();
        #region Constants

        /// <summary>
        /// The chun k_ size.
        /// </summary>
        private const int ChunkSize = 1024;

        #endregion

        #region Fields

        /// <summary>
        /// The error message.
        /// </summary>
        private readonly List<string> errorMessage;

        /// <summary>
        /// The error messages as byte array.
        /// </summary>
        private IList<byte[]> errorMessagesAsByteArray;

        /// <summary>
        /// The error type.
        /// </summary>
        private string errorType;

        /// <summary>
        /// The id.
        /// </summary>
        private int id;

        /// <summary>
        /// The exception.
        /// </summary>
        private Exception ex;

        #endregion

        // DEFAULT CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        public ErrorReport()
        {
            this.errorMessagesAsByteArray = new List<byte[]>();
            this.errorMessage = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private ErrorReport(Exception e)
        {
            this.errorMessagesAsByteArray = new List<byte[]>();
            this.errorMessage = new List<string>();
            this.AddErrorMessage(e);
            this.ex = e;
            this.errorMessage.Reverse();

            // TODO errorMessagesAsByteArray = su.SerializeAsChunkedByteArray(errorMessage,
            // TODO         CHUNK_SIZE);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        private ErrorReport(string msg)
        {
            this.errorMessagesAsByteArray = new List<byte[]>();
            this.errorMessage = new List<string>();
            this.AddErrorMessage(msg);
            this.errorMessage.Reverse();

            // TODO errorMessagesAsByteArray = su.SerializeAsChunkedByteArray(errorMessage,
            // TODO         CHUNK_SIZE);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private ErrorReport(string msg, Exception e)
        {
            this.errorMessagesAsByteArray = new List<byte[]>();
            this.errorMessage = new List<string>();
            this.AddErrorMessage(msg, e);
            this.ex = e;
            this.errorMessage.Reverse();

            // TODO errorMessagesAsByteArray = su.SerializeAsChunkedByteArray(errorMessage,
            // TODO         CHUNK_SIZE);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public IList<string> ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType
        {
            get
            {
                return this.errorType;
            }

            set
            {
                this.errorType = value;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception
        {
            get
            {
                return this.ex;
            }
        }

        /// <summary>
        /// Gets or sets the serialized error message.
        /// </summary>
        public IList<byte[]> SerializedErrorMessage
        {
            get
            {
                return this.errorMessagesAsByteArray;
            }

            set
            {
                this.errorMessagesAsByteArray = value;

                if (this.errorMessagesAsByteArray.Count > 0)
                {
                    // TODO errorMessage = su.DeSerializeChunkedByteArray(value);
                }
            }
        }

        /// <summary>
        /// Gets a santized error message so that all illegal characters are replaced by valid characters.
        /// NOTE. XmlWriter already does this! DO NOT USE with XmlWriter
        /// </summary>
        public string StringForHtml
        {
            get
            {
                // Sanitizes the error message so that all illegal characters are replaced by 
                // valid characters.
                var sb = new StringBuilder();

                foreach (string currentMessage in this.errorMessage)
                {
                    sb.Append("Caused By " + HttpUtility.HtmlEncode(currentMessage));
                }

                return sb.ToString();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="ErrorReport"/>.
        /// </returns>
        public static ErrorReport Build(Exception e)
        {
            var report = new ErrorReport(e);
            return report;
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <returns>
        /// The <see cref="ErrorReport"/>.
        /// </returns>
        public static ErrorReport Build(string msg)
        {
            var report = new ErrorReport(msg);
            return report;
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="msg">
        /// The msgs.
        /// </param>
        /// <returns>
        /// The <see cref="ErrorReport"/>.
        /// </returns>
        public static ErrorReport Build(IList<string> msg)
        {
            var report = new ErrorReport();

            foreach (string currentMsg in msg)
            {
                report.AddErrorMessage(currentMsg);
            }

            return report;
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="ErrorReport"/>.
        /// </returns>
        public static ErrorReport Build(string msg, Exception e)
        {
            var report = new ErrorReport(msg, e);
            return report;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (string currentMessage in this.errorMessage)
            {
                sb.Append(currentMessage);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add error message.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AddErrorMessage(Exception e)
        {
            if (e != null)
            {
                string errorMessageStr = e.Message;
                if (string.IsNullOrEmpty(errorMessageStr))
                {
                    this.errorMessage.Add("Error message is empty");
                }
                else
                {
                    string[] currentErrors = Regex.Split(errorMessageStr, Environment.NewLine);
                    foreach (string currentError in currentErrors)
                    {
                        this.errorMessage.Add(currentError);
                    }
                }


                this.AddErrorMessage(e.InnerException);
            }
        }

        /// <summary>
        /// The add error message.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        private void AddErrorMessage(string msg)
        {
            this.errorMessage.Add(msg);
        }

        /// <summary>
        /// The add error message.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AddErrorMessage(string msg, Exception e)
        {
            if (e != null)
            {
                this.errorMessage.Add(msg + ": " + e.Message);
                this.AddErrorMessage(e.InnerException);
            }
        }

        #endregion
    }
}