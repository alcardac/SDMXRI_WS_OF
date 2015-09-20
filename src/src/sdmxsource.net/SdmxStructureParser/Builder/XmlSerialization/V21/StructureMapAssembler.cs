// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure map bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The structure map bean assembler.
    /// </summary>
    public class StructureMapAssembler : NameableAssembler, IAssembler<StructureMapType, IStructureMapObject>
    {
        #region Fields

        /// <summary>
        ///     The text format assembler.
        /// </summary>
        private readonly TextFormatAssembler _textFormatAssembler = new TextFormatAssembler();

        /// <summary>
        ///     The to value type type builder.
        /// </summary>
        private readonly ToValueTypeTypeBuilder _toValueTypeTypeBuilder = new ToValueTypeTypeBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Assemble from <paramref name="assembleFrom"/> to <paramref name="assembleInto"/>
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(StructureMapType assembleInto, IStructureMapObject assembleFrom)
        {
            // Populate it using this class's specifics
            this.AssembleNameable(assembleInto, assembleFrom);
            assembleInto.isExtension = assembleFrom.Extension;

            // Source
            var source = new StructureOrUsageReferenceType();
            var sourceRef = new StructureOrUsageRefType();
            source.SetTypedRef(sourceRef);
            assembleInto.Source = source;
            this.SetReference(sourceRef, assembleFrom.SourceRef);

            var target = new StructureOrUsageReferenceType();
            var targetRef = new StructureOrUsageRefType();
            target.SetTypedRef(targetRef);
            assembleInto.Target = target;
            this.SetReference(targetRef, assembleFrom.TargetRef);

            // Child maps
            foreach (IComponentMapObject eachMapBean in assembleFrom.Components)
            {
                // Defer child creation to subclass
                var newMap = new ComponentMapType();

                //// FIXME TODO ComponetMap should be a list
                assembleInto.ComponentMapTypes.Add(newMap);

                // Common source and target id allocation
                var sourceComponent = new LocalComponentListComponentReferenceType();
                newMap.Source = sourceComponent;
                var targetComponent = new LocalComponentListComponentReferenceType();
                newMap.Target = targetComponent;
                var sourceComponentRef = new LocalComponentListComponentRefType();
                var targetComponentRef = new LocalComponentListComponentRefType();
                sourceComponent.SetTypedRef(sourceComponentRef);
                targetComponent.SetTypedRef(targetComponentRef);

                this.SetComponentReference(sourceComponentRef, eachMapBean.MapConceptRef);
                this.SetComponentReference(targetComponentRef, eachMapBean.MapTargetConceptRef);

                // Representation mapping
                if (eachMapBean.RepMapRef != null)
                {
                    IRepresentationMapRef repMapRef = eachMapBean.RepMapRef;
                    var repMap = new RepresentationMapType();
                    newMap.RepresentationMapping = repMap;
                    if (repMapRef.CodelistMap != null)
                    {
                        var codelistMapReferenceType = new LocalCodelistMapReferenceType();
                        repMap.CodelistMap = codelistMapReferenceType;
                        codelistMapReferenceType.SetTypedRef(
                            new CodelistMapRefType { id = repMapRef.CodelistMap.ChildReference.Id });
                    }

                    if (repMapRef.ToTextFormat != null)
                    {
                        this._textFormatAssembler.Assemble(
                            repMap.ToTextFormat = new TextFormatType(), repMapRef.ToTextFormat);
                    }

                    if (repMapRef.ToValueType != ToValue.Null)
                    {
                        repMap.ToValueType = this._toValueTypeTypeBuilder.Build(repMapRef.ToValueType);
                    }

                    if (repMapRef.ValueMappings.Count > 0)
                    {
                        var vmt = new ValueMapType();
                        repMap.ValueMap = vmt;

                        IDictionary<string, ISet<string>> valueMappings = repMapRef.ValueMappings;

                        /* foreach */
                        foreach (string currentKey in valueMappings.Keys)
                            foreach (string currentValue in valueMappings[currentKey])
                            {
                                var valueMappingType = new ValueMappingType();
                                vmt.ValueMapping.Add(valueMappingType);

                                valueMappingType.source = currentKey;
                                valueMappingType.target = currentValue;
                            }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the component reference.
        /// </summary>
        /// <param name="crossReference">
        /// The crossReference.
        /// </param>
        /// <param name="partialReference">
        /// The partial reference.
        /// </param>
        protected internal void SetComponentReference(RefBaseType crossReference, string partialReference)
        {
            crossReference.id = partialReference;
            crossReference.containerID = "DimensionList";
            crossReference.package = PackageTypeCodelistTypeConstants.Datastructure;
            crossReference.@class = ObjectTypeCodelistTypeConstants.Dimension;
        }

        #endregion
    }
}