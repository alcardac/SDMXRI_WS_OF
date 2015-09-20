// -----------------------------------------------------------------------
// <copyright file="AttachmentConstraintMutableObjectCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttachmentConstraintMutableObjectCore : ConstraintMutableCore<IAttachmentConstraintObject>, IAttachmentConstraintMutableObject
    {
        public AttachmentConstraintMutableObjectCore(IAttachmentConstraintObject objTarget)
            : base(objTarget)
        {
        }

        public AttachmentConstraintMutableObjectCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint))
        {
        }

        #region Overrides of MaintainableMutableCore<IContentConstraintObject>

        public override IAttachmentConstraintObject ImmutableInstance
        {
            get
            {
                return new AttachmentConstraintObjectCore(this);
            }
        }

        #endregion
    }
}
