// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValueImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The key value impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    /// <summary>
    ///   The key value impl.
    /// </summary>
    [Serializable]
    public class KeyValueImpl : IKeyValue
    {
        ///// According to the http://msdn.microsoft.com/en-us/library/system.serializableattribute(v=vs.100).aspx
        ///// ALL we need to do is use the SerializableAttribute which we do. TODO test
        ///// IXmlSerializable //$$$ Externalizable 
        #region Fields

        /// <summary>
        ///   The _code.
        /// </summary>
        private readonly string _code;

        /// <summary>
        ///   The _concept.
        /// </summary>
        private readonly string _concept;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="KeyValueImpl" /> class.
        /// </summary>
        public KeyValueImpl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueImpl"/> class.
        /// </summary>
        /// <param name="code0">
        /// The code 0. 
        /// </param>
        /// <param name="concept1">
        /// The concept 1. 
        /// </param>
        public KeyValueImpl(string code0, string concept1)
        {
            this._code = code0;
            this._concept = concept1;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the code.
        /// </summary>
        public virtual string Code
        {
            get
            {
                return this._code;
            }
        }

        /// <summary>
        ///   Gets the concept.
        /// </summary>
        public virtual string Concept
        {
            get
            {
                return this._concept;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The read external.
        /// </summary>
        /// <param name="formatter">
        /// The formatter. 
        /// </param>
        /// <param name="stream">
        /// The stream. 
        /// </param>
        /// <returns>
        /// The <see cref="KeyValueImpl"/> . 
        /// </returns>
        public static KeyValueImpl ReadExternal(IFormatter formatter, Stream stream)
        {
            return (KeyValueImpl)formatter.Deserialize(stream);
        }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// Key value to compare to
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        public virtual int CompareTo(IKeyValue other)
        {
            if (this._concept.Equals(other.Concept))
            {
                return string.CompareOrdinal(this._code, other.Code);
            }

            return string.CompareOrdinal(this._concept, other.Concept);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            var that = obj as IKeyValue;
            if (that != null)
            {
                return this.Concept.Equals(that.Concept) && this.Code.Equals(that.Code);
            }

            return base.Equals(obj);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The <see cref="int" /> . </returns>
        public override int GetHashCode()
        {
            return (this._concept + this._code).GetHashCode();
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            return this._concept + ":" + this._code;
        }

        /// <summary>
        /// The write external.
        /// </summary>
        /// <param name="formatter">
        /// The formatter. 
        /// </param>
        /// <param name="stream">
        /// The stream. 
        /// </param>
        public void WriteExternal(IFormatter formatter, Stream stream)
        {
            formatter.Serialize(stream, this);
        }

        #endregion

        /*    public virtual void WriteExternal(ObjectOutput xout) {
            xout.WriteObject(_concept);
            xout.WriteObject(_code);
        }
    
        public virtual void ReadExternal(ObjectInput ins0) {
            _concept = (string) ins0.ReadObject();
            _code = (string) ins0.ReadObject();
        }*/

        ////public XmlSchema GetSchema()
        ////{
        ////    return null;
        ////}

        ////public void ReadXml(XmlReader reader)
        ////{
        ////    _concept = reader.ReadContentAsString();
        ////    _code = reader.ReadContentAsString();
        ////}

        ////public void WriteXml(XmlWriter writer)
        ////{
        ////    throw new NotImplementedException("Not yet implemented");
        ////    //// TODO $$$ writer.Wqqqq
        ////}
    }
}