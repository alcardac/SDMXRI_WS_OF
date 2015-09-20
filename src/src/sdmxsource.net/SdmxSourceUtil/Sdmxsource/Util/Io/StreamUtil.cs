// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamUtil.cs" company="Eurostat">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The stream util.
    /// </summary>
    public class StreamUtil
    {
        // HACK - THIS IS FOR INPUT STREAMS THAT ARE NOT UTF*, BUT IF THE STREAM IS UTF- THIS METHOD BREAKS THE ENCODING 
        // BY CONVERTING AN INT TO A CHAR
        /*
        private static void CopyNonUTF8InputStream(Stream inputStream, Stream outputStream) {
            TextWriter bos = null;
            BufferedStream bis = null;
            try {
    
                bis = new BufferedStream(inputStream);
                bos = ILOG.J2CsMapping.IO.IOUtility.NewStreamWriter(new BufferedStream(outputStream),System.Text.Encoding.GetEncoding("UTF-8"));
    
                byte[] bytes = new byte[1024];
                int i;
                while ((i = bis.Read(bytes,0,bytes.Length)) > 0) {
                    bos.Write(new String(bytes, 0, i));
                }
                bos.Flush();
    
            } catch (Exception th) {
                throw new Exception(th.Message, th);
            } finally {
                try {
                    inputStream.Close();
                    outputStream.Flush();
                    outputStream.Close();
                    if (bos != null) {
                        bos.Close();
                    }
                    if (bis != null) {
                        bis.Close();
                    }
                } catch (Exception th_0) {
                    throw new Exception(th_0.Message, th_0);
                }
            }
        }
*/
        #region Public Methods and Operators

        /// <summary>
        /// Closes all of the supplied InputStreams.
        /// </summary>
        /// <param name="inputStreams">
        /// The input streams 
        /// </param>
        public static void CloseStream(params Stream[] inputStreams)
        {
            if (inputStreams == null)
            {
                return;
            }

            foreach (Stream currentIn in inputStreams)
            {
                currentIn.Close();
            }
        }

        /// <summary>
        /// Copies the first 'x' number of lines to a list, each list element represents a line.
        /// </summary>
        /// <param name="stream">
        /// The input stream 
        /// </param>
        /// <param name="numLines">
        /// The number of lines 
        /// </param>
        /// <returns>
        /// the first 'x' number of lines to a list, each list element represents a line. 
        /// </returns>
        public static IList<string> CopyFirstXLines(Stream stream, int numLines)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            TextReader reader = new StreamReader(stream);
            string line;
            var firstXRows = new List<string>();
            while (firstXRows.Count < numLines && (line = reader.ReadLine()) != null)
            {
                firstXRows.Add(line);
            }

            return firstXRows;
        }

        /// <summary>
        /// Copies the supplied <paramref name="inputStream"/> to the supplied <paramref name="outputStream"/>.
        /// Converts the OutputStream to UTF-8.
        /// Both streams are closed on completion, uses a buffer of 1Kb
        /// </summary>
        /// <param name="inputStream">
        /// The input stream. 
        /// </param>
        /// <param name="outputStream">
        /// the OutputStream to write to. 
        /// </param>
        public static void CopyStream(Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }

            using (inputStream)
            {
                using (outputStream)
                {
                    //// TODO 1024 is smallish 
                    inputStream.CopyTo(outputStream, 1024);
                    outputStream.Flush();
                }
            }
        }

        /// <summary>
        /// Create a byte[] from the supplied InputStream.
        /// The InputStream is closed after use.
        /// </summary>
        /// <param name="inputStream">
        /// The input stream. 
        /// </param>
        /// <returns>
        /// An array of <see cref="byte"/>
        /// </returns>
        public static byte[] ToByteArray(Stream inputStream)
        {
            var bytes = new byte[1024];
            using (inputStream)
            using (var bos = new MemoryStream())
            {
                int i;
                while ((i = inputStream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    bos.Write(bytes, 0, i);
                }

                bos.Flush();
                return bos.ToArray();
            }
        }

        #endregion
    }
}