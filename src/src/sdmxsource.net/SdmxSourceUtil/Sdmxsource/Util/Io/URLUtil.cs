// --------------------------------------------------------------------------------------------------------------------
// <copyright file="URLUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Io
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Sockets;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    class URLUtil
    {
        #region Static Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(URLUtil));

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens an input stream to the URL, accepts GZIP encoding
        /// Gets a new file from <paramref name="sUrl"/>.
        /// </summary>
        /// <param name="sUrl">
        /// The input url.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static Stream GetInputStream(string sUrl)
        {
            _log.Debug("Get Input Stream from URL: " + sUrl);
            WebResponse connection = GetConnection(sUrl);
            return GetInputStream(connection, null);
        }

        /// <summary>
        /// Gets the input stream from <paramref name="sUrl"/>.
        /// </summary>
        /// <param name="sUrl">
        /// The input url.
        /// </param>
        /// <returns>
        /// The web response <see cref="WebResponse"/>.
        /// </returns>
        private static WebResponse GetConnection(string sUrl)
        {
            _log.Debug("Get URLConnection: " + sUrl);
            // Make connection, use post mode, and send query   

            WebRequest wReq = WebRequest.Create(sUrl);
            HttpWebRequest httpReq = (HttpWebRequest)wReq;
            httpReq.Method = "POST";
            httpReq.Headers["Accept-Encoding"] = "gzip";

            WebResponse wResp = null;
            try
            {
                wResp = wReq.GetResponse();

            }
            catch (IOException e)
            {
                throw new SdmxServiceUnavailableException(e, ExceptionCode.WebServiceBadConnection, sUrl);
            }
            return wResp;
        }

        /// <summary>
        /// Gets the input stream from <paramref name="sUrl"/>.
        /// </summary>
        /// <param name="sUrl">
        /// The input url path.
        /// </param>
        /// <param name="payload">
        /// The payload object.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetInputStream(WebResponse wResp, Object payload)
        {
            Stream stream;
            try
            {
                if (payload != null)
                {
                    //Send Payload
                    StreamWriter sw = new StreamWriter(wResp.GetResponseStream());
                    sw.Write(payload);
                    sw.Close();
                }
                stream = GetInputStream(wResp);
            }
            catch (IOException e)
            {
                throw new SdmxServiceUnavailableException(e, ExceptionCode.WebServiceBadConnection, e.Message);
            }

            string sEncoding = ((HttpWebResponse)wResp).ContentEncoding;
            if (sEncoding != null && sEncoding.Equals("gzip"))
            {
                _log.Debug("Response received as GZIP");
                try
                {
                    stream = new GZipStream(stream, CompressionMode.Compress);
                }
                catch (IOException e)
                {
                    throw new SdmxException("I/O Ecception while trying to unzip stream retrieved from service:" + wResp.ResponseUri, e);
                }
            }
            return stream;
        }

        /// <summary>
        /// Gets the input stream from WebResponse <paramref name="wResp"/>.
        /// </summary>
        /// <param name="wResp">
        /// The web response.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetInputStream(WebResponse wResp)
        {
            try
            {
                return wResp.GetResponseStream();
            }
            catch (WebException c)
            {
                throw new SdmxServiceUnavailableException(c, ExceptionCode.WebServiceBadConnection, ((HttpWebResponse)c.Response).StatusDescription);
            }
            catch (SocketException c)
            {
                if (c.SocketErrorCode == SocketError.TimedOut)
                {
                    throw new SdmxServiceUnavailableException(c, ExceptionCode.WebServiceSocketTimeout, c.Message);
                }
                else
                {
                    throw new SdmxServiceUnavailableException(c, ExceptionCode.WebServiceBadConnection, c.Message);
                }
            }
            catch (IOException e)
            {
                if (wResp is HttpWebResponse)
                {
                    try
                    {
                        if (((HttpWebResponse)wResp).StatusCode == HttpStatusCode.Unauthorized /*401*/)
                        {
                            throw new SdmxUnauthorisedException(e.Message);
                        }
                    }
                    catch (IOException e1)
                    {
                        Console.WriteLine(e1.StackTrace);
                    }
                    HttpWebResponse httpConnection = (HttpWebResponse)wResp;
                    Stream inputStream = httpConnection.GetResponseStream();
                    if (inputStream != null)
                    {
                        return inputStream;
                    }
                }
                string message = null;
                if (e.Message.Contains("Server returned HTTP response code:"))
                {
                    string split = e.Message.Split(':')[1];
                    split = split.Trim();
                    split = split.Substring(0, split.IndexOf(' '));

                    try
                    {
                        int responseCode = Int32.Parse(split);

                        switch (responseCode)
                        {
                            case 400:
                                message = "Response Code 400 = The request could not be understood by the server due to malformed syntax";
                                break;
                            case 401:
                                message = "Response Code 401 = Authentication failure";
                                break;
                            case 403:
                                message = "Response Code 403 = The server understood the request, but is refusing to fulfill it";
                                break;
                            case 404:
                                message = "Response Code 404 = Page not found";
                                break;
                            //TODO Do others
                        }
                        Console.Out.Write(responseCode);
                    }
                    catch (Exception)
                    {
                        //DO NOTHING
                    }
                }
                if (message != null)
                {
                    throw new SdmxServiceUnavailableException(e, ExceptionCode.WebServiceBadConnection, message);
                }
                else
                {
                    throw new SdmxServiceUnavailableException(e, ExceptionCode.WebServiceBadConnection, wResp.ResponseUri);
                }
            }
        }

        /// <summary>
        /// Returns a WebRequest from a String, throws a SdmxException if the URL String is not a valid URL <paramref name="sUrl"/>.
        /// </summary>
        /// <param name="sUrl">
        /// The url string.
        /// </param>
        /// <returns>
        /// The <see cref="WebRequest"/>.
        /// </returns>
        public static WebRequest GetURL(string sUrl)
        {
            try
            {
                return WebRequest.Create(sUrl);
            }
            catch (WebException)
            {
                throw new SdmxException("Malformed URL: " + sUrl);
            }
        }

        /// <summary>
        /// Returns true if the HTTP Status is 200 (ok) from the given URL <paramref name="sUrl"/>.
        /// </summary>
        /// <param name="sUrl">
        /// The url string.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool UrlExists(string sUrl)
        {
            try
            {
                HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(sUrl);
                wReq.AllowAutoRedirect = false;
                wReq.Method = "HEAD";

                HttpWebResponse wResp = (HttpWebResponse)wReq.GetResponse();

                //Does the HTTP Status start with 2, if so, it is okay
                if (wResp.StatusCode.ToString().StartsWith("2"))
                    return true;

                _log.Warn("URL " + sUrl + " returns status code: " + wResp.StatusCode.ToString());
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion
    }
}
