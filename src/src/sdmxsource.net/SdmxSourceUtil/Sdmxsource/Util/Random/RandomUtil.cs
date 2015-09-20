// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Random
{
    using System;
    using System.Text;
    using System.Threading;

    /// <summary>
    ///     The random util.
    /// </summary>
    public class RandomUtil
    {
        #region Constants

        /// <summary>
        ///     The password set.
        /// </summary>
        private const string PasswordSet = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The <see cref="RandomGenerator" />, one object per thread.
        /// </summary>
        [ThreadStatic]
        private static Random _random;

        /// <summary>
        ///     Seed counter
        /// </summary>
        private static int _seedCounter = new Random().Next();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="RandomGenerator" />, one object per thread.
        /// </summary>
        private static Random RandomGenerator
        {
            get
            {
                if (_random == null)
                {
                    int seed = Interlocked.Increment(ref _seedCounter);
                    _random = new Random(seed);
                }

                return _random;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The generate random password.
        /// </summary>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GenerateRandomPassword(int length)
        {
            var b = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                b.Append(PasswordSet[RandomGenerator.Next(PasswordSet.Length)]);
            }

            return b.ToString();
        }

        /// <summary>
        ///     The generate random string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GenerateRandomString()
        {
            int r1 = RandomGenerator.Next();
            int r2 = RandomGenerator.Next();
            string hash1 = string.Format("{0:x}", r1);
            string hash2 = string.Format("{0:x}", r2);
            return hash1 + hash2;
        }

        #endregion
    }
}