// -----------------------------------------------------------------------
// <copyright file="ItemSchemeAssembler.cs" company="EUROSTAT">
//   Date Created : 2015-01-12
//   Copyright (c) 2015 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The item scheme assembler.
    /// </summary>
    public class ItemSchemeAssembler : MaintainableAssembler
    {
        /// <summary>
        /// Assembles the item scheme.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="builtObj">The built object.</param>
        /// <param name="buildFrom">The build from.</param>
        public void AssembleItemScheme<T>(ItemSchemeType builtObj, IItemSchemeObject<T> buildFrom) where T : IItemObject
        {
            this.AssembleMaintainable(builtObj, buildFrom);
            if (buildFrom.Partial)
            {
                builtObj.isPartial = true;
            }
        }
    }
}