// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesAttributeGroupReader.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A binary reader for files written by <see cref="GesmesAttributeGroupWriter" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Engine
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Estat.Sri.SdmxEdiDataWriter.Helper;

    /// <summary>
    ///     A binary reader for files written by <see cref="GesmesAttributeGroupWriter" />
    /// </summary>
    internal class GesmesAttributeGroupReader : BinaryReader
    {
        #region Fields

        /// <summary>
        ///     A string builder used as buffer
        /// </summary>
        private readonly StringBuilder _buffer = new StringBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesAttributeGroupReader"/> class.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public GesmesAttributeGroupReader(Stream input)
            : base(input, Encoding.UTF8)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Stream this instance as GESMES attributes to <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="codedAttributes">
        /// The coded attributes set.
        /// </param>
        /// <param name="count">
        /// The number of attribute groups
        /// </param>
        /// <returns>
        /// The number of segments written
        /// </returns>
        public int StreamToGesmes(TextWriter writer, IDictionary<string, object> codedAttributes, int count)
        {
            // TODO convert codedAttributes to Set<string> when we target 3.5 or later
            int segmentCount = 0;
            while (count > 0)
            {
                count--;
                int dimensionCount = this.ReadInt32();
                var dimensionValues = new string[dimensionCount];
                for (int i = 0; i < dimensionValues.Length; i++)
                {
                    dimensionValues[i] = this.ReadString();
                }

                int attributeCount = this.ReadInt32();
                if (attributeCount > 0)
                {
                    this._buffer.Length = 0;
                    writer.Write(GesmesHelper.ArrSegment(this._buffer, dimensionCount, dimensionValues));
                    segmentCount++;

                    for (int i = 0; i < attributeCount; i++)
                    {
                        this._buffer.Length = 0;
                        string name = this.ReadString();
                        string value = this.ReadString();

                        segmentCount += codedAttributes.ContainsKey(name)
                                            ? GesmesHelper.CodedAttribute(this._buffer, name, value)
                                            : GesmesHelper.UncodedAttribute(this._buffer, name, value);

                        writer.Write(this._buffer);
                    }
                }
            }

            return segmentCount;
        }

        #endregion
    }
}