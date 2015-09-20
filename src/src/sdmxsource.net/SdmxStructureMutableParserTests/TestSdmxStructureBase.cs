// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSdmxStructureBase.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The test sdmx structure base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureMutableParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Schema;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The test sdmx structure base.
    /// </summary>
    public abstract class TestSdmxStructureBase
    {
        #region Methods

        /// <summary>
        /// Compare artefacts of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="newArtefacts">
        /// The new artefacts of type <typeparamref name="T"/>.
        /// </param>
        /// <param name="getOriginal">
        /// The function to get  the original of type <typeparamref name="T"/>.
        /// </param>
        /// <typeparam name="T">
        /// The artefact type
        /// </typeparam>
        protected static void CompareArtefacts<T>(IEnumerable<T> newArtefacts, Func<IMaintainableRefObject, ISet<T>> getOriginal) where T : IMaintainableObject
        {
            foreach (T artefact in newArtefacts)
            {
                ISet<T> originalArtefact = getOriginal(artefact.AsReference.MaintainableReference);
                if (originalArtefact.Count == 1)
                {
                    foreach (T schemeObject in originalArtefact)
                    {
                        Assert.IsTrue(artefact.DeepEquals(schemeObject, true), artefact.AsReference.MaintainableReference.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// The on validation event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected static void OnValidationEventHandler(object sender, ValidationEventArgs args)
        {
            Trace.WriteLine(args.Message);
            if (args.Exception == null)
            {
                return;
            }

            Trace.WriteLine(args.Exception);
            Trace.WriteLine(string.Format("{3}:{0} Column: {1}. Error:\n {2}", args.Exception.SourceUri, args.Exception.LineNumber, args.Exception.LinePosition, args.Exception.Message));
            throw args.Exception;
        }

        #endregion
    }
}