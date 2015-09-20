// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFormatAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The text format assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System;
    using System.Globalization;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The text format assembler.
    /// </summary>
    public class TextFormatAssembler : IAssembler<TextFormatType, ITextFormat>
    {
        #region Fields

        /// <summary>
        ///     The data type builder.
        /// </summary>
        private readonly DataTypeBuilder _dataTypeBuilder = new DataTypeBuilder();

        /// <summary>
        ///     The log.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(TextFormatAssembler));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(TextFormatType assembleInto, ITextFormat assembleFrom)
        {
            // TODO some try-catch are not reachable/pointless
            TextType textType = assembleFrom.TextType;
            if (textType != null)
            {
                try
                {
                    assembleInto.textType = this._dataTypeBuilder.Build(textType);
                }
                catch (Exception)
                {
                    assembleInto.textType = null; //// was unset but why ?? TODO
                    this._log.Warn("SDMX 2.1 - Unable to set TextType on " + assembleFrom.Parent);
                }
            }

            if (assembleFrom.Sequence.IsSet())
            {
                try
                {
                    assembleInto.isSequence = assembleFrom.Sequence.IsTrue;
                }
                catch (Exception e)
                {
                    assembleInto.isSequence = null; //// was unset but why ?? TODO
                    this._log.Warn("SDMX 2.1 - Unable to set Sequence on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.MinLength != null)
            {
                try
                {
                    assembleInto.minLength = assembleFrom.MinLength;
                }
                catch (Exception e)
                {
                    assembleInto.minLength = null; //// was unset but why ?? TODO
                    this._log.Warn("SDMX 2.1 - Unable to set MinLength on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.MaxLength != null)
            {
                try
                {
                    assembleInto.maxLength = assembleFrom.MaxLength;
                }
                catch (Exception e)
                {
                    assembleInto.maxLength = null;
                    this._log.Warn("SDMX 2.1 - Unable to set MaxLength on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.MinValue != null)
            {
                try
                {
                    assembleInto.minValue = assembleFrom.MinValue;
                }
                catch (Exception e)
                {
                    assembleInto.minValue = null;
                    this._log.Warn("SDMX 2.1 - Unable to set MinValue on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.MaxValue != null)
            {
                try
                {
                    assembleInto.maxValue = assembleFrom.MaxValue;
                }
                catch (Exception e)
                {
                    assembleInto.maxValue = null;
                    this._log.Warn("SDMX 2.1 - Unable to set MaxValue on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.StartValue != null)
            {
                try
                {
                    assembleInto.startValue = assembleFrom.StartValue;
                }
                catch (Exception e)
                {
                    assembleInto.startValue = null;
                    this._log.Warn("SDMX 2.1 - Unable to set StartValue on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.EndValue != null)
            {
                try
                {
                    assembleInto.endValue = assembleFrom.EndValue;
                }
                catch (Exception e)
                {
                    assembleInto.endValue = null;
                    this._log.Warn("SDMX 2.1 - Unable to set EndValue on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.Interval != null)
            {
                try
                {
                    assembleInto.interval = assembleFrom.Interval;
                }
                catch (Exception e)
                {
                    assembleInto.interval = null;
                    this._log.Warn("SDMX 2.1 - Unable to set Interval on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.TimeInterval != null)
            {
                TimeSpan result;
                if (TimeSpan.TryParse(assembleFrom.TimeInterval, CultureInfo.InvariantCulture, out result))
                {
                    assembleInto.timeInterval = result;
                }
                else
                {
                    assembleInto.timeInterval = null;
                    this._log.Warn("SDMX 2.1 - Unable to set TimeInterval on " + assembleFrom.Parent);
                }
            }

            if (assembleFrom.Decimals != null)
            {
                try
                {
                    assembleInto.decimals = assembleFrom.Decimals;
                }
                catch (Exception e)
                {
                    assembleInto.decimals = null;
                    this._log.Warn("SDMX 2.1 - Unable to set Decimals on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.Pattern != null)
            {
                ////try
                ////{
                assembleInto.pattern = assembleFrom.Pattern;

                ////}
                ////catch (Exception th10)
                ////{
                ////    assembleInto.pattern = null;
                ////    this._log.Warn("SDMX 2.1 - Unable to set Pattern on " + assembleFrom.Parent);
                ////}
            }

            if (assembleFrom.StartTime != null)
            {
                try
                {
                    assembleInto.startTime = assembleFrom.StartTime.DateInSdmxFormat;
                }
                catch (Exception e)
                {
                    assembleInto.startTime = null;
                    this._log.Warn("SDMX 2.1 - Unable to set StartTime on " + assembleFrom.Parent, e);
                }
            }

            if (assembleFrom.EndTime != null)
            {
                try
                {
                    // FUNC 2.1 end time TODO
                }
                catch (Exception e)
                {
                    assembleInto.endTime = null;
                    this._log.Warn("SDMX 2.1 - Unable to set EndTime on " + assembleFrom.Parent, e);
                }
            }
        }

        #endregion
    }
}