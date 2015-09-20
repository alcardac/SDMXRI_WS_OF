// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesAttributeGroupWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A binary writer for <see cref="GesmesAttributeGroup" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Engine
{
    using System.IO;
    using System.Text;

    using Estat.Sri.SdmxEdiDataWriter.Model;

    /// <summary>
    ///     A binary writer for <see cref="GesmesAttributeGroup" />
    /// </summary>
    internal class GesmesAttributeGroupWriter : BinaryWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesAttributeGroupWriter"/> class.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        public GesmesAttributeGroupWriter(Stream output)
            : base(output, Encoding.UTF8)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Write the specified <paramref name="gesmesAttributeGroup"/> to output
        /// </summary>
        /// <param name="gesmesAttributeGroup">
        /// The GESMES attribute group that contains a single ARR and all of it's attributes
        /// </param>
        public void Write(GesmesAttributeGroup gesmesAttributeGroup)
        {
            int length = gesmesAttributeGroup.DimensionValues.Length;
            this.Write(length);
            for (int i = 0; i < length; i++)
            {
                this.Write(gesmesAttributeGroup.DimensionValues[i]);
            }

            this.Write(gesmesAttributeGroup.AttributeValues.Count);
            for (int i = 0; i < gesmesAttributeGroup.AttributeValues.Count; i++)
            {
                this.Write(gesmesAttributeGroup.AttributeValues[i].Key);
                this.Write(gesmesAttributeGroup.AttributeValues[i].Value ?? string.Empty);
            }
        }

        #endregion
    }
}