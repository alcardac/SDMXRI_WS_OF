// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCheckedException.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Exception
{
    #region Using directives

    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     Abstract class to be sub-classed by all custom Exceptions in the system.
    ///     <p />
    ///     If the exceptions are multi-lingual, this class provides the means
    ///     for retrieving exception messages in different languages.
    ///     <p />
    ///     The Exceptions that sub-class this are all Checked Exceptions
    /// </summary>
    ///// TODO REMOVE this class ?? It is not used and is not appear to be useful and needs fixing
    [Serializable]
    public abstract class BaseCheckedException : Exception
    {
        #region Static Fields

        /// <summary>
        ///     The default locale.
        /// </summary>
        private static readonly CultureInfo _defaultLocale = CultureInfo.CreateSpecificCulture("en");

        /// <summary>
        ///     The message resolver.
        /// </summary>
        private static IMessageResolver _messageResolver;

        #endregion

        #region Fields

        /// <summary>
        ///     The _args.
        /// </summary>
        private readonly object[] _args;

        /// <summary>
        ///     The _code.
        /// </summary>
        private readonly ExceptionCode _code;

        /// <summary>
        ///     The _code message.
        /// </summary>
        private readonly string _codeStr;

        /// <summary>
        ///     The _exception.
        /// </summary>
        private readonly Exception _exception;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCheckedException"/> class.
        /// </summary>
        /// <param name="str">
        /// The message.
        /// </param>
        protected BaseCheckedException(string str)
            : base(str)
        {
            Console.Error.WriteLine(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCheckedException"/> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="exception">
        ///     The exception.
        /// </param>
        protected BaseCheckedException(string message, Exception exception)
            : base(message, exception)
        {
            if (exception != null)
            {
                Console.Error.WriteLine(exception.ToString());
            }

            // $$$ base.printStackTrace();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCheckedException" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="code">The code.</param>
        /// <param name="args">The arguments.</param>
        protected BaseCheckedException(Exception exception, ExceptionCode code, object[] args)
            : base(exception != null ? exception.Message : null, exception)
        {
            this._exception = exception;
            if (exception != null)
            {
                Console.Error.WriteLine(exception.StackTrace);
            }

            // $$$ base.printStackTrace();
            if (code != null)
            {
                this._code = code;
                this._codeStr = code.Code;
            }

            this._args = args;
            Console.Out.WriteLine();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCheckedException"/> class. 
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
        protected BaseCheckedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCheckedException"/> class. 
        /// </summary>
        protected BaseCheckedException()
        {
        }

        /// <summary>
        ///     Gets or sets the message resolver.
        /// </summary>
        public static IMessageResolver MessageResolver
        {
            get
            {
                return _messageResolver;
            }

            set
            {
                _messageResolver = value;
            }
        }

        /// <summary>
        ///     Gets the code.
        /// </summary>
        public ExceptionCode Code
        {
            get
            {
                var exception = this._exception as BaseCheckedException;
                if (this._code == null && exception != null)
                {
                    return exception.Code;
                }

                return this._code;
            }
        }

        /// <summary>
        ///     Gets the error type.
        /// </summary>
        public abstract string ErrorType { get; }

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
                        sb.Append("  - caused by " + Environment.GetEnvironmentVariable("line.separator"));
                        var exception = this.InnerException as BaseCheckedException;
                        sb.Append(exception != null ? exception.FullMessage : this.InnerException.Message);
                    }

                    return sb.ToString();
                }

                return this.GetFullMessage(this._exception, this._codeStr, this._args);
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

                return this.GetMessage(this._exception, this._codeStr, this._args, CultureInfo.InvariantCulture);
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
                var exception = nestedException as BaseCheckedException;
                if (exception != null)
                {
                    BaseCheckedException ex = exception;
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
        /// <param name="loc">
        /// The locale.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public string GetMessage(CultureInfo loc)
        {
            return this.GetMessage(this._exception, this._codeStr, this._args, loc);
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
        public string GetMessage(Exception nestedException, string code, object[] args)
        {
            string nestedMessage = nestedException != null ? nestedException.Message : string.Empty;

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
        /// <param name="nestedException">
        /// The nestedException.
        /// </param>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <param name="loc">
        /// The locale.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public string GetMessage(Exception nestedException, string code, object[] args, CultureInfo loc)
        {
            string nestedMessage = string.Empty;
            if (nestedException != null)
            {
                var exception = nestedException as BaseCheckedException;
                nestedMessage = exception != null ? exception.GetMessage(loc) : nestedException.Message;
            }

            if (code == null)
            {
                return nestedMessage;
            }

            if (nestedMessage.Length > 0)
            {
                return this.ResolveMessage(code, args, loc) + "\n\n" + nestedMessage;
            }

            return this.ResolveMessage(code, args, loc);
        }

        /// <summary>
        /// The resolve message.
        /// </summary>
        /// <param name="code0">
        /// The code 0.
        /// </param>
        /// <param name="args1">
        /// The args 1.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public virtual string ResolveMessage(string code0, object[] args1)
        {
            if (_messageResolver == null)
            {
                return code0;
            }

            var sb = new StringBuilder();

            // Try preferred locale first
            CultureInfo myLoc = CultureInfo.CreateSpecificCulture("en");

            sb.Append(myLoc);
            sb.Append("\n\n");
            string message = _messageResolver.ResolveMessage(code0, myLoc, args1);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            return "Exception message could not be resolved for code : " + code0
                   + " the following locales were checked: " + sb;
        }

        /// <summary>
        /// The resolve message.
        /// </summary>
        /// <param name="code0">
        /// The code 0.
        /// </param>
        /// <param name="args1">
        /// The args 1.
        /// </param>
        /// <param name="loc">
        /// The locale.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public virtual string ResolveMessage(string code0, object[] args1, CultureInfo loc)
        {
            if (_messageResolver == null)
            {
                return code0;
            }

            string message = _messageResolver.ResolveMessage(code0, loc, args1);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            return "Exception message could not be resolved for code : " + code0 + " for the following locale " + loc;
        }

        #endregion
    }
}