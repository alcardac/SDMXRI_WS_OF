namespace TestRest
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Net;
    using System.Reflection;
    using System.Text;

    using log4net;

    using NUnit.Framework;

    /// <summary>
    /// The resource service test base.
    /// </summary>
    public class ResourceServiceTestBase
    {
        /// <summary>
        /// Initializes static members of the <see cref="ResourceServiceTestBase"/> class.
        /// </summary>
        static ResourceServiceTestBase()
        {
            // HACK bug in .NET 4.0 URI. It removes any trailing dots in any part of the URL
            // Workaround from http://stackoverflow.com/questions/856885/httpwebrequest-to-url-with-dot-at-the-end
            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {
                foreach (string scheme in new[] { "http", "https" })
                {
                    var parser = (UriParser)getSyntax.Invoke(null, new object [] { scheme });
                    if (parser != null)
                    {
                        var flagsValue = (int)flagsField.GetValue(parser);

                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                        {
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ResourceServiceTestBase));

        /// <summary>
        /// The rest client bad request.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        protected static void RestClientBadRequest(string query, string contentType)
        {
            var output = new StringBuilder();
            var status = RestClientNoException(query, contentType);
            _log.Info("RESPONSE=\n" + output);
            _log.Info("\n\n");
            Assert.IsTrue(status == HttpStatusCode.BadRequest, "No status Bad Request 400");
        }

        /// <summary>
        /// The rest client no exception.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <param name="contentType">
        ///     The content type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static HttpStatusCode RestClientNoException(string query, string contentType)
        {
            HttpStatusCode statusCode = 0;
            var settings = Properties.Settings.Default;
            var url = new Uri(new Uri(settings.baseURL), query);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(settings.userName, settings.userName);
            request.Method = "GET";
            request.Accept = contentType;
            request.Timeout = 1000 * 60 * 5;
            request.Headers.Add("Accept-Encoding", "gzip");

            _log.InfoFormat("Getting: {0} ", url);

            _log.InfoFormat("Accept: {0}", contentType);
            try
            {
                using (var response = request.GetResponse())
                {
                    var contentTypeHeader = response.ContentType;
                    _log.InfoFormat(" ContentType - {0}", contentTypeHeader);
                    _log.InfoFormat("Content Length : {0} ", response.ContentLength);
                    _log.InfoFormat("Response URI {0}", response.ResponseUri);
                    var httpResponse = (HttpWebResponse)response;
                    statusCode = httpResponse.StatusCode;
                    
                    var stream = response.GetResponseStream();
                    WriteResponse(stream);
                }
            }
            catch (WebException e)
            {
                _log.Error(e.Status, e);
                _log.ErrorFormat("Query '{0}' for content type '{1}' failed with error code {2}", query, contentType, e.Status);
                var httpWebResponse = e.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    _log.ErrorFormat("Response URI {0}", httpWebResponse.ResponseUri);
                    statusCode = httpWebResponse.StatusCode;
                    var responseStream = httpWebResponse.GetResponseStream();
                    if (responseStream != null)
                    {
                       WriteResponse(responseStream);
                    }
                }
            }

            return statusCode;
        }

        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private static void WriteResponse(Stream stream)
        {
            var outputFile = Path.GetTempFileName();
            using (Stream responseStream = stream)
            using (Stream output = new FileStream(outputFile, FileMode.Create))
            {
                _log.InfoFormat("Writing response to : {0}", outputFile);
                Assert.IsNotNull(responseStream);
                var buffer = new byte[32768];
                var count = 0L;
                int len;
                while ((len = responseStream.Read(buffer, 0, 32768)) > 0)
                {
                    output.Write(buffer, 0, len);
                    count += len;
                }

                output.Flush();
                _log.InfoFormat("Wrote {0} bytes to {1}", count, outputFile);
            }
        }

        /// <summary>
        /// The rest client not acceptable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        protected static void RestClientNotAcceptable(string query, string contentType)
        {
            var output = new StringBuilder();
            var status = RestClientNoException(query, contentType);

            _log.Info("RESPONSE=\n" + output);
            _log.Info("\n\n");
            Assert.IsTrue(status == HttpStatusCode.NotAcceptable, "No status Not Acceptable 406");
        }

        /// <summary>
        /// The rest client not implemented.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        protected static void RestClientNotImplemented(string query, string contentType)
        {
            var output = new StringBuilder();
            var status = RestClientNoException(query, contentType);

            _log.Info("RESPONSE=\n" + output);
            _log.Info("\n\n");
            Assert.IsTrue(status == HttpStatusCode.NotImplemented, "No status Not Implemented 501");
        }

        /// <summary>
        /// The rest client ok.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        protected static void RestClientOk(string query, string contentType)
        {
            var output = new StringBuilder();
            var status = RestClientNoException(query, contentType);

            _log.Info("RESPONSE=\n" + output);
            _log.Info("\n\n");
            Assert.IsTrue(status == HttpStatusCode.OK, "No status Ok 200");
        }
    }
}