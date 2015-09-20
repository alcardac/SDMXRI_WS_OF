// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToValueTypeTypeBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The to value type type builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The to value type type builder.
    /// </summary>
    public class ToValueTypeTypeBuilder : IBuilder<string, ToValue>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds and return
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="Enum"/>.
        /// </returns>
        public virtual string Build(ToValue buildFrom)
        {
            switch (buildFrom)
            {
                case ToValue.Value:
                    return ToValueTypeTypeConstants.Value;
                case ToValue.Name:
                    return ToValueTypeTypeConstants.Name;
                case ToValue.Description:
                    return ToValueTypeTypeConstants.Description;
            }

            return null;
        }

        #endregion
    }
}