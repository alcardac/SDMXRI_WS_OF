using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Sdmxsource.Sdmx.Api.Model;

namespace Org.Sdmxsource.Sdmx.Structureparser.Model
{
    public class SdmxErrorFormat : IErrorFormat
    {
        #region Static Fields

        private readonly SdmxErrorFormat INSTANCE = new SdmxErrorFormat();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SdmxErrorFormat" /> class.
        /// </summary>
        protected SdmxErrorFormat()
        {
        }

        #endregion

        #region Public Properties

        public virtual SdmxErrorFormat Instance
        {
            get
            {
                return this.INSTANCE;
            }
        }

        #endregion
    }
}
