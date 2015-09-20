// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingStructureWritingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The re using structure writing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.Structure
{
    using System.Globalization;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;

    /// <summary>
    /// The re using structure writing manager.
    /// </summary>
    public class ReUsingStructureWritingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            // 1. Create the StructureWritingManager
            IStructureWriterManager writingManager = new StructureWriterManager();

            // 2. Create a test mutable codelist with Count codes.
            const int Count = 1000;
            string countStr = Count.ToString(CultureInfo.InvariantCulture);
            ICodelistMutableObject codelist = new CodelistMutableCore();
            codelist.Id = "CL_K" + countStr;
            codelist.AgencyId = "TEST";
            codelist.AddName("en", "Test CL with " + countStr);
            for (int i = 0; i < Count; i++)
            {
                ICodeMutableObject code = new CodeMutableCore();
                code.Id = i.ToString(CultureInfo.InvariantCulture);
                code.AddName("en", "Code " + code.Id);
                codelist.AddItem(code);
            }

            // 3. Select the out filename
            string output = string.Format(CultureInfo.InvariantCulture, "{0}.xml", codelist.Id);

            // 4. Create the immutable instance of the codelist
            ICodelistObject immutableInstance = codelist.ImmutableInstance;

            using (FileStream writer = File.OpenWrite(output))
            {
                // 5. Write the codelist as a SDMX-ML v2.1 Structure document. Other options include SDMX-ML 2.0 Structure or Registry Interface documents.
                writingManager.WriteStructure(immutableInstance, new HeaderImpl("ZZ9", "ZZ9"), new SdmxStructureFormat(StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument)), writer);
            }
        }

        #endregion
    }
}