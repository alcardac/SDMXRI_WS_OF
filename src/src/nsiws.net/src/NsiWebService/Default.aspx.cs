// -----------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="Eurostat">
//   Date Created : 2012-04-10
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Nsi.DataDisseminationWS
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Web.UI;

    using Estat.Nsi.DataDisseminationWS.Builder;
    using Estat.Sri.Ws.Controllers.Model;

    using Resources;

    /// <summary>
    /// The class that interfaces between the aspx page and the internal classes. 
    /// It also contains some utility classes
    /// </summary>
    public class Default : Page
    {
        /// <summary>
        ///   Gets the collection of web service info
        /// </summary>
        public static IList<WebServiceInfo> WebServices 
        {
            get 
            {
                return WebServiceInfoBuilder.WebServices;
            }
        }

        /// <summary>
        ///   Gets the NSI Web Service version
        /// </summary>
        public static string Version 
        {
            get 
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    Messages.app_version,
                    Assembly.GetExecutingAssembly().GetName().Version);
            }
        }

        /// <summary>
        /// Checks if the specified <paramref name="address"/> is a private (RFC 1918, RFC 4193) or link local (RFC 5735, RFC 3927, RFC 4291)
        /// </summary>
        /// <param name="address">The IP Address</param>
        /// <returns>True if the specified <paramref name="address"/> is a private or link local; otherwise false</returns>
        /// <remarks>See <a href="http://en.wikipedia.org/wiki/Private_network">http://en.wikipedia.org/wiki/Private_network</a></remarks>
        public static bool IsPrivate(IPAddress address) 
        {
            if (address.IsIPv6SiteLocal || address.IsIPv6LinkLocal)
            {
                return true;
            }

            if (!address.AddressFamily.Equals(AddressFamily.InterNetwork))
            {
                return false;
            }

            bool isPrivate = false;
            byte[] addressBytes = address.GetAddressBytes();
            switch (addressBytes[0])
            {
                case 10:
                    isPrivate = true;
                    break;
                case 192:
                    isPrivate = addressBytes[1] == 168;
                    break;
                case 172:
                    isPrivate = ((addressBytes[1] & 240) ^ 16) == 0;
                    break;
                case 169: // link-local address RFC 5735, RFC 3927
                    isPrivate = addressBytes[1] == 254;
                    break;
            }

            return isPrivate;
        }

        /// <summary>
        /// Try to get an external IP; otherwise the first local or private IP. It will use IPv6 only if there is no IPv4 IPs.
        /// </summary>
        /// <param name="host">The host entry</param>
        /// <returns>an external IP; otherwise the first local or private IP</returns>
        public static IPAddress TryGetExternal(IPHostEntry host)
        {
            IPAddress ret = null;
            IPAddress ipv6 = null;
            for (int i = 0; i < host.AddressList.Length; i++)
            {
                IPAddress address = host.AddressList[i];
                bool isIpv4 = address.AddressFamily.Equals(AddressFamily.InterNetwork);

                if (isIpv4)
                {
                    if (ret == null)
                    {
                        ret = address;
                    }
                    else
                    {
                        if (!IPAddress.IsLoopback(address) && !IsPrivate(address))
                        {
                            return address;
                        }

                        if (IPAddress.IsLoopback(ret) && IsPrivate(address))
                        {
                            ret = address;
                        }
                    }
                }
                else
                {
                    if (ipv6 == null || (!IPAddress.IsLoopback(address) && !IsPrivate(address)))
                    {
                        ipv6 = address;
                    }
                    else if (IPAddress.IsLoopback(ipv6))
                    {
                        ipv6 = address;
                    }
                }
            }

            return ret ?? ipv6;
        }

/*
        /// <summary>
        /// Gets the types from the <see cref="Assembly.GetExecutingAssembly()"/> which have the <typeparamref name="T"/> attribute
        /// </summary>
        /// <typeparam name="T">
        /// The custom <see cref="Attribute"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>
        /// </returns>
        private static IEnumerable<Type> GetTypesWithCustom<T>() where T : Attribute 
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                {
                    yield return type;
                }
            }
        }
*/
    }
}