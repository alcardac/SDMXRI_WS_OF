// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxException.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Exception
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///  Abstract class to be sub-classed by all custom Exceptions in the system.
    /// </summary>
    [Serializable]
    public class SdmxException : Exception
    {
        #region Static Fields

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxException));

        /// <summary>
        ///     The default locale.
        /// </summary>
        private static readonly CultureInfo _defaultLocale = CultureInfo.CreateSpecificCulture("en");

        #endregion


        #region Fields

        private static IMessageResolver _messageResolver;
        private readonly ExceptionCode _code;
        private readonly string _codeStr;
        private readonly object[] _args;
        private readonly Exception _exception;
        private readonly SdmxErrorCode _errorCode = SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError);

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// </summary>
        public SdmxException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="code">
        /// The exception code
        /// </param>
        public SdmxException(ExceptionCode code)
            : this(SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError), code, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="errorMessage">
        /// The error message
        /// </param>
        /// <param name="errorCode">
        /// The error code
        /// </param>
        public SdmxException(string errorMessage, SdmxErrorCode errorCode)
            : this(null, errorCode, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// If the <paramref name="innerException"/> is a SdmxException - then the
        /// error code will be used, if it is not, then InternalServerError will be used
        /// </summary>
        /// <param name="errorMessage">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public SdmxException(string errorMessage, Exception innerException)
            : this(innerException, null, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="nestedException">
        /// The exception
        /// </param>
        /// <param name="errorCode">
        /// The error code
        /// </param>
        /// <param name="message">
        /// the message
        /// </param>
        public SdmxException(Exception nestedException, SdmxErrorCode errorCode, string message)
            : base(message, nestedException)
        {
            this._exception = nestedException;
            if (errorCode != null)
            {
                this._errorCode = errorCode;
            }

            if (nestedException != null)
            {
                Console.Error.WriteLine(nestedException.StackTrace);
                if (errorCode == null)
                {

                    var exception = nestedException as SdmxException;
                    if (exception != null)
                    {
                        this._errorCode = exception.SdmxErrorCode;
                    }
                }
            }
            else
            {
                Console.Error.WriteLine(this);
            }

            _log.Error(message, this);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="exception">
        /// The exception
        /// </param>
        /// <param name="code">
        /// The exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxException(Exception exception, ExceptionCode code, params object[] args)
            : this(exception, null, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="errorCode">
        /// The error code
        /// </param>
        /// <param name="code">
        /// The exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxException(SdmxErrorCode errorCode, ExceptionCode code, params object[] args)
            : this(null, errorCode, code, args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Creates Exception from an error String and an Error code
        /// </summary>
        /// <param name="nestedException">
        /// The exception
        /// </param>
        /// <param name="errorCode">
        /// The error code
        /// </param>
        /// <param name="code">
        /// The exception code
        /// </param>
        /// <param name="args">
        /// The arguments
        /// </param>
        public SdmxException(Exception nestedException, SdmxErrorCode errorCode, ExceptionCode code, params object[] args)
            : base(nestedException != null ? nestedException.Message : errorCode != null ? errorCode.ErrorString : null, nestedException)
        {
            this._exception = nestedException;
            this._errorCode = errorCode;
            this._args = args;
            if (nestedException != null)
            {
                Console.Error.WriteLine(nestedException.StackTrace);
                if (errorCode == null)
                {
                    var exception = nestedException as SdmxException;
                    if (exception != null)
                    {
                        this._errorCode = exception.SdmxErrorCode;
                    }
                }
            }
            else
            {
                Console.Error.WriteLine(this);
            }
            if (code != null)
            {
                this._code = code;
                this._codeStr = code.Code;
            }

            _log.Error("Error ", this);
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error. 
        /// </param>
        public SdmxException(string message)
            : this(null, null, message)
        {
        }
     
        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxException"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. 
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. 
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null. 
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). 
        /// </exception>
        protected SdmxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this._errorCode = (SdmxErrorCode)info.GetValue("_errorCode", typeof(SdmxErrorCode));
            this._args = (object[])info.GetValue("_args", typeof(object[]));
            this._codeStr = info.GetString("_codeStr");
            this._code = (ExceptionCode)info.GetValue("_code", typeof(ExceptionCode));
        }

        /// <summary>
        /// Gets the error code
        /// </summary>
        public virtual SdmxErrorCode SdmxErrorCode
        {
            get
            {
                return this._errorCode;
            }
        }

        /// <summary>
        /// Gets the HTTP REST error code
        /// </summary>
        public int HttpRestErrorCode
        {
            get
            {
                return this._errorCode.HttpRestErrorCode;
            }
        }


        /// <summary>
        ///     Gets the message.
        /// </summary>
        public override string Message
        {
            get
            {
                if (this._codeStr == null)
                {
                    return base.Message;
                }

                return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", base.Message, this.GetMessage(this._exception, this._codeStr, this._args));
            }
        }



        /// <summary>
        ///     Gets the full message.
        /// </summary>
        public string FullMessage
        {
            get
            {
                if (this._codeStr == null)
                {
                    var sb = new StringBuilder();
                    sb.Append(base.Message);
                    if (this.InnerException != null)
                    {
                        sb.AppendLine("  - caused by ");
                        var exception = this.InnerException as SdmxException;
                        sb.Append(exception != null ? exception.FullMessage : this.InnerException.Message);
                    }
                    return sb.ToString();
                }

                return this.GetFullMessage(this._exception, this._codeStr, this._args);
            }
        }


        /// <summary>
        ///     Gets the code.
        /// </summary>
        public ExceptionCode Code
        {
            get
            {
                var exception = this._exception as SdmxException;
                if (this._code == null && exception != null)
                {
                    return exception.Code;
                }

                return this._code;
            }
        }


        /// <summary>
        /// Gets the error type.
        /// </summary>
        public virtual string ErrorType
        {
            get
            {
                return "SdmxException";
            }
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// The get full message.
        /// </summary>
        /// <param name="nestedException">
        /// The nestedException.
        /// </param>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public string GetFullMessage(Exception nestedException, string code, object[] args)
        {
            string nestedMessage = string.Empty;
            if (nestedException != null)
            {
                var exception = nestedException as SdmxException;
                if (exception != null)
                {
                    SdmxException ex = exception;
                    nestedMessage = "Nested Message : " + ex.FullMessage;
                }
                else
                {
                    nestedMessage = "Nested Message : " + nestedException.Message;
                }
            }

            if (code == null)
            {
                return nestedMessage;
            }

            return nestedMessage.Length > 0
                        ? string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}\n\n{1}",
                            this.ResolveMessage(code, args, CultureInfo.InvariantCulture),
                            nestedMessage)
                        : this.ResolveMessage(code, args, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the message.
        /// </summary>
        /// <param name="locale">
        /// The locale.
        /// </param>
        /// <returns>
        /// The message.
        /// </returns>
        public virtual string GetMessage(CultureInfo locale)
        {
            return this.GetMessage(this._exception, this._codeStr, this._args, locale);
        }


        /// <summary>
        /// Returns the exception message
        /// </summary>
        /// <param name="nestedException">
        /// The nested Exception.
        /// </param>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public virtual string GetMessage(Exception nestedException, string code, object[] args)
        {
            string nestedMessage = nestedException != null ? nestedException.Message : base.Message;

            // if(th != null) {
            // nestedMessage = "Nested Message : " + th.getMessage();
            // }
            if (code == null)
            {
                return nestedMessage;
            }

            // if(ObjectUtil.validString(nestedMessage)) {
            // return resolveMessage(code, args) + "\n\n" + nestedMessage;
            // }
            return this.ResolveMessage(code, args, _defaultLocale);
        }


        /// <summary>
        /// Returns the message.
        /// </summary>
        /// <param name="nestedException">The nestedException.</param>
        /// <param name="code">The code.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>
        /// The message.
        /// </returns>
        public virtual string GetMessage(Exception nestedException, string code, object[] args, CultureInfo locale)
        {
            string nestedMessage = string.Empty;
            if (nestedException != null)
            {
                var exception = nestedException as SdmxException;
                nestedMessage = exception != null ? exception.GetMessage(locale) : nestedException.Message;
            }

            if (code == null)
            {
                return nestedMessage;
            }

            if (nestedMessage.Length > 0)
            {
                return this.ResolveMessage(code, args, locale) + "\n\n" + nestedMessage;
            }

            return this.ResolveMessage(code, args, locale);
        }


        /// <summary>
        /// Resolves the message.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The message.</returns>
        public virtual string ResolveMessage(string code, object[] args)
        {
            if (_messageResolver == null)
            {
                return code;
            }

            var sb = new StringBuilder();

            // Try preferred locale first
            CultureInfo myLoc = CultureInfo.CreateSpecificCulture("en");

            sb.Append(myLoc);
            sb.Append("\n\n");
            string message = _messageResolver.ResolveMessage(code, myLoc, args);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            return "Exception message could not be resolved for code : " + code
                    + " the following locales were checked: " + sb;
        }


        /// <summary>
        /// Resolves the message.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>The message.</returns>
        public virtual string ResolveMessage(string code, object[] args, CultureInfo locale)
        {
            if (_messageResolver == null)
            {
                return code;
            }

            string message = _messageResolver.ResolveMessage(code, locale, args);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            return "Exception message could not be resolved for code : " + code + " for the following locale " + locale;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageResolver">
        /// The Message resolver
        /// </param>
        public static void SetMessageResolver(IMessageResolver messageResolver)
        {
            SdmxException._messageResolver = messageResolver;
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_errorCode", this._errorCode);
            info.AddValue("_code", this._code);
            info.AddValue("_codeStr", this._codeStr);
            info.AddValue("_args", this._args);
        }

        #endregion
    }
}

