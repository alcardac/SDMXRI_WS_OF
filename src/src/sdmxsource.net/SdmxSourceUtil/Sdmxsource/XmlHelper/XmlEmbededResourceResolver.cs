// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlEmbededResourceResolver.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.XmlHelper
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// The xml embeded resource resolver.
    /// </summary>
    public class XmlEmbededResourceResolver : XmlUrlResolver
    {
        #region Fields

        /// <summary>
        /// The root path
        /// </summary>
        private readonly string _rootDir;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlEmbededResourceResolver"/> class.
        /// </summary>
        /// <param name="rootDir">
        /// The root namespace for "res://" type Uri 
        /// </param>
        public XmlEmbededResourceResolver(string rootDir)
        {
            this._rootDir = rootDir;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// When overridden in a derived class, sets the credentials used to authenticate Web requests.
        /// </summary>
        /// <returns> An <see cref="T:System.Net.ICredentials" /> object. If this property is not set, the value defaults to null; that is, the XmlResolver has no user credentials. </returns>
        public override ICredentials Credentials
        {
            set
            {
                throw new NotImplementedException("Credentials is not implemented");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// When overridden in a derived class, maps a URI to an object containing the actual resource.
        /// </summary>
        /// <returns>
        /// A System.IO.Stream object or null if a type other than stream is specified. 
        /// </returns>
        /// <param name="absoluteUri">
        /// The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)"/> . 
        /// </param>
        /// <param name="role">
        /// The current version1 does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios. 
        /// </param>
        /// <param name="returnObject">
        /// The type of object to return. The current version1 only returns System.IO.Stream objects. 
        /// </param>
        /// <exception cref="T:System.Xml.XmlException">
        /// <paramref name="returnObject"/>
        /// is not a Stream type.
        /// </exception>
        /// <exception cref="T:System.UriFormatException">
        /// The specified URI is not an absolute URI.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="absoluteUri"/>
        /// is null.
        /// </exception>
        /// <exception cref="T:System.Exception">
        /// There is a runtime error (for example, an interrupted server connection).
        /// </exception>
        public override object GetEntity(Uri absoluteUri, string role, Type returnObject)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }

            Stream stream;

            ////Console.WriteLine("Attempting to retrieve: {0}", absoluteUri);

            switch (absoluteUri.Scheme)
            {
                case "res":

                    try
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        string resourceName = absoluteUri.AbsolutePath;
                        if (resourceName.StartsWith("/", StringComparison.Ordinal))
                        {
                            resourceName = resourceName.Substring(1);
                        }

                        resourceName = string.Format("{0}.{1}", this._rootDir, resourceName);
                        stream = assembly.GetManifestResourceStream(resourceName);
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.ToString());
                        throw;
                    }

                    return stream;

                default:

                    // Handle file:// and http:// 
                    // requests from the XmlUrlResolver base class
                    stream = (Stream)base.GetEntity(absoluteUri, role, returnObject);
                    return stream;
            }
        }

        #endregion
    }
}