// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeTempFile.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class holds <see cref="FileStream" /> and <see cref="GesmesAttributeGroupWriter" /> instances for attribute value on-disk buffering
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Estat.Sri.SdmxEdiDataWriter.Constants;
    using Estat.Sri.SdmxEdiDataWriter.Engine;
    using Estat.Sri.SdmxEdiDataWriter.Helper;

    using Org.Sdmxsource.Sdmx.EdiParser.Constants;

    /// <summary>
    ///     This class holds <see cref="FileStream" /> and <see cref="GesmesAttributeGroupWriter" /> instances for attribute value on-disk buffering
    /// </summary>
    internal class AttributeTempFile : IDisposable
    {
        #region Constants

        /// <summary>
        ///     The buffer size
        /// </summary>
        private const int BufferSize = 32768;

        #endregion

        #region Fields

        /// <summary>
        ///     The current <see cref="GesmesAttributeGroup" />
        /// </summary>
        private readonly GesmesAttributeGroup _currentGroup;

        /// <summary>
        ///     The file stream to the temp file
        /// </summary>
        private readonly FileStream _fileStream;

        /// <summary>
        ///     The binary writer for <see cref="GesmesAttributeGroup" />
        /// </summary>
        private readonly GesmesAttributeGroupWriter _writer;

        /// <summary>
        ///     A value indicating whether this instance was disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     THe total number of <see cref="GesmesAttributeGroup" /> written using <see cref="_writer" />
        /// </summary>
        private int _totalAttributeWritten;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeTempFile"/> class.
        /// </summary>
        /// <param name="tempPath">
        /// The temp path.
        /// </param>
        /// <param name="map">
        /// The dimension position in the ARR map
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        public AttributeTempFile(string tempPath, GesmesKeyMap map, RelStatus level)
        {
            string filePath = Path.Combine(tempPath, Path.GetRandomFileName());
            this._fileStream = File.Create(filePath, BufferSize, FileOptions.DeleteOnClose | FileOptions.SequentialScan);
            this._writer = new GesmesAttributeGroupWriter(this._fileStream);
            this._currentGroup = new GesmesAttributeGroup(map, level);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current <see cref="GesmesAttributeGroup" />
        /// </summary>
        public GesmesAttributeGroup CurrentGroup
        {
            get
            {
                return this._currentGroup;
            }
        }

        /// <summary>
        ///     Gets the total number of <see cref="GesmesAttributeGroup" /> written using <see cref="_writer" />
        /// </summary>
        public int TotalAttributeWritten
        {
            get
            {
                return this._totalAttributeWritten;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Stream file contents as GESMES attributes to <paramref name="gesmesWriter"/>.
        /// </summary>
        /// <param name="gesmesWriter">
        /// The output writer
        /// </param>
        /// <param name="codedAttributes">
        /// The set of coded attributes
        /// </param>
        /// <returns>
        /// The number of segments added
        /// </returns>
        public int StreamToGesmes(TextWriter gesmesWriter, IDictionary<string, object> codedAttributes)
        {
            this._writer.Flush();
            this._fileStream.Position = 0;

            var builder = new StringBuilder();
            GesmesHelper.StartSegment(builder, EdiConstants.RelZ01Tag).AppendFormat("{0:D}", this.CurrentGroup.Level);
            GesmesHelper.EndSegment(builder);
            gesmesWriter.Write(builder);

            using (var reader = new GesmesAttributeGroupReader(this._fileStream))
            {
                return reader.StreamToGesmes(gesmesWriter, codedAttributes, this._totalAttributeWritten) + 1;
            }
        }

        /// <summary>
        ///     Write the current group
        /// </summary>
        public void WriteCurrentGroup()
        {
            this._writer.Write(this._currentGroup);
            this._currentGroup.Clear();
            this._totalAttributeWritten++;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// Set to true to dispose managed disposable resources 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this._writer != null)
                {
                    this._writer.Close();
                }

                if (this._fileStream != null)
                {
                    this._fileStream.Dispose();
                }
            }

            this._disposed = true;
        }

        #endregion
    }
}