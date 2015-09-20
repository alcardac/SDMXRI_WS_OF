// -----------------------------------------------------------------------
// <copyright file="ISdmxStructureDocumentSplitter.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISdmxStructureDocumentSplitter
    {
        /// <summary>
        /// Splits an SDMX Structure Document into 2 documents, the first containing a structure document with all structures that are not included in the second
        /// (splitTypes) the second document containing all the structures in splitTypes.  Both documents will contain the same header information.  If
        /// there are no structures in the input document that match the split types, then the returned array will be of size 1.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="splitTypes"></param>
        /// <returns></returns>
        IReadableDataLocation[] SplitDocument(Uri uri, params SdmxStructureType[] splitTypes);
    }
}
