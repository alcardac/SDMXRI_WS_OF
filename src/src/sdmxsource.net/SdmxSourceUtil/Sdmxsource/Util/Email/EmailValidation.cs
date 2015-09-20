// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailValidation.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Email
{
    using System;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The email validation.
    /// </summary>
    public class EmailValidation
    {
        #region Static Fields

        /// <summary>
        /// The e-mail validator
        /// </summary>
        private static readonly Regex _regex = new Regex("^.+@.+\\.[a-z]+$", RegexOptions.IgnoreCase);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The validate email.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            try
            {
                var mailAddress = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}