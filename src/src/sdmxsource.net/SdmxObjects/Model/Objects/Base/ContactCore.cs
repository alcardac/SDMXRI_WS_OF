// -----------------------------------------------------------------------
// <copyright file="ContactCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    #region Using directives

    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.TextType;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class ContactCore : SdmxObjectCore, IContact
    {
        private readonly string id;

        private readonly IList<ITextTypeWrapper> name = new List<ITextTypeWrapper>();

        private readonly IList<ITextTypeWrapper> role = new List<ITextTypeWrapper>();

        private readonly IList<ITextTypeWrapper> departments = new List<ITextTypeWrapper>();

        private readonly IList<string> email = new List<string>();

        private readonly IList<string> fax = new List<string>();

        private readonly IList<string> telephone = new List<string>();

        private readonly IList<string> uri = new List<string>();

        private readonly IList<string> x400 = new List<string>();

        public string Id
        {
            get
            {
                return id;
            }
        }

        public ContactCore(IContactMutableObject mutableBean)
            : this(mutableBean, null)
        {
        }

        public ContactCore(IContactMutableObject mutableBean, ISdmxObject parent)
            : base(mutableBean, parent)
        {
            this.id = mutableBean.Id;
            CopyTextTypes(name, mutableBean.Names);
            CopyTextTypes(role, mutableBean.Names);
            CopyTextTypes(departments, mutableBean.Departments);

            if (mutableBean.Email != null)
            {
                this.email = new List<string>(mutableBean.Email);
            }
            if (mutableBean.Telephone != null)
            {
                this.telephone = new List<string>(mutableBean.Telephone);
            }
            if (mutableBean.Fax != null)
            {
                this.fax = new List<string>(mutableBean.Fax);
            }
            if (mutableBean.Uri != null)
            {
                this.uri = new List<string>(mutableBean.Uri);
            }
            if (mutableBean.X400 != null)
            {
                this.x400 = new List<string>(mutableBean.X400);
            }
        }

        private void CopyTextTypes(IList<ITextTypeWrapper> copyTo, IList<ITextTypeWrapperMutableObject> mutable)
        {
            if (mutable != null)
            {
                foreach (ITextTypeWrapperMutableObject currentTextType in mutable)
                {
                    copyTo.Add(new TextTypeWrapperImpl(currentTextType, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ATTRIBUTES			   ////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        public ContactCore(
            IList<ITextTypeWrapper> name,
            IList<ITextTypeWrapper> role,
            IList<ITextTypeWrapper> departments,
            IList<string> email,
            IList<string> fax,
            IList<string> telephone,
            IList<string> uri,
            IList<string> x400)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Contact), null)
        {
            if (name != null)
            {
                this.name = new List<ITextTypeWrapper>(name);
            }
            if (role != null)
            {
                this.role = new List<ITextTypeWrapper>(role);
            }
            if (departments != null)
            {
                this.departments = new List<ITextTypeWrapper>(departments);
            }
            if (email != null)
            {
                this.email = new List<string>(email);
            }
            if (fax != null)
            {
                this.fax = new List<string>(fax);
            }
            if (telephone != null)
            {
                this.telephone = new List<string>(telephone);
            }
            if (uri != null)
            {
                this.uri = new List<string>(uri);
            }
            if (x400 != null)
            {
                this.x400 = new List<string>(x400);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA			///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        public ContactCore(ContactType contactType)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Contact), null)
        {
            if (ObjectUtil.ValidCollection(contactType.Department))
            {
                foreach (TextType tt in contactType.Department)
                {
                    departments.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Email))
            {
                email = new List<string>(contactType.Email);
            }
            if (ObjectUtil.ValidCollection(contactType.Fax))
            {
                fax = new List<string>(contactType.Fax);
            }
            if (ObjectUtil.ValidCollection(contactType.Name))
            {
                foreach (Name tt in contactType.Name)
                {
                    name.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Role))
            {
                foreach (TextType tt in contactType.Role)
                {
                    role.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Telephone))
            {
                telephone = new List<string>(contactType.Telephone);
            }
            if (ObjectUtil.ValidCollection(contactType.URI))
            {
                uri = new List<string>(contactType.URI.Select(x => x.ToString()).ToList());
            }
            if (ObjectUtil.ValidCollection(contactType.X400))
            {
                x400 = new List<string>(contactType.X400);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA			///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        public ContactCore(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ContactType contactType, ISdmxObject parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Contact), parent)
        {
            if (ObjectUtil.ValidCollection(contactType.Department))
            {
                foreach (TextType tt in contactType.Department)
                {
                    departments.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Email))
            {
                email = new List<string>(contactType.Email);
            }
            if (ObjectUtil.ValidCollection(contactType.Fax))
            {
                fax = new List<string>(contactType.Fax);
            }
            if (ObjectUtil.ValidCollection(contactType.Name))
            {
                foreach (Name tt in contactType.Name)
                {
                    name.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Role))
            {
                foreach (TextType tt in contactType.Role)
                {
                    role.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Telephone))
            {
                telephone = new List<string>(contactType.Telephone);
            }
            if (ObjectUtil.ValidCollection(contactType.URI))
            {
                uri = new List<string>(contactType.URI.Select(x => x.ToString()).ToList());
            }
            if (ObjectUtil.ValidCollection(contactType.X400))
            {
                x400 = new List<string>(contactType.X400);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.0 SCHEMA			///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        public ContactCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.ContactType contactType)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Contact), null)
        {
            if (ObjectUtil.ValidCollection(contactType.Department))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType tt in contactType.Department)
                {
                   // Only add departments that are non-null
                    if (ObjectUtil.ValidString(tt.TypedValue))
                    {
                        departments.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                    }
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Email))
            {
                email = new List<string>(contactType.Email);
            }
            if (ObjectUtil.ValidCollection(contactType.Fax))
            {
                fax = new List<string>(contactType.Fax);
            }
            if (ObjectUtil.ValidCollection(contactType.Name))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType tt in contactType.Name)
                {
                   // Only add names that are non-null
			  	   if (ObjectUtil.ValidString(tt.TypedValue))
                   {
                      name.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                   }
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Role))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType tt in contactType.Role)
                {
                    // Only add roles that are non-null 
                    if (ObjectUtil.ValidString(tt.TypedValue))
                    {
                      role.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                    }
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Telephone))
            {
                telephone = new List<string>(contactType.Telephone);
            }
            if (ObjectUtil.ValidCollection(contactType.URI))
            {
                uri = new List<string>(contactType.URI.Select(x => x.ToString()).ToList());
            }
            if (ObjectUtil.ValidCollection(contactType.X400))
            {
                x400 = new List<string>(contactType.X400);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1.0 SCHEMA			///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        public ContactCore(Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.ContactType contactType)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Contact), null)
        {
            if (ObjectUtil.ValidCollection(contactType.Department))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType tt in contactType.Department)
                {
                    departments.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Email))
            {
                email = new List<string>(contactType.Email);
            }
            if (ObjectUtil.ValidCollection(contactType.Fax))
            {
                fax = new List<string>(contactType.Fax);
            }
            if (ObjectUtil.ValidCollection(contactType.Name))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType tt in contactType.Name)
                {
                    name.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Role))
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.common.TextType tt in contactType.Role)
                {
                    role.Add(new TextTypeWrapperImpl(tt.lang, tt.TypedValue, null));
                }
            }
            if (ObjectUtil.ValidCollection(contactType.Telephone))
            {
                telephone = new List<string>(contactType.Telephone);
            }
            if (ObjectUtil.ValidCollection(contactType.URI))
            {
                uri = new List<string>(contactType.URI.Select(x => x.ToString()).ToList());
            }
            if (ObjectUtil.ValidCollection(contactType.X400))
            {
                x400 = new List<string>(contactType.X400);
            }
        }

        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            var contact = sdmxObject as IContact;
            if (contact != null)
            {
                if (!base.Equivalent(contact.Departments, this.departments, includeFinalProperties))
                {
                    return false;
                }
                if (!base.Equivalent(contact.Name, this.name, includeFinalProperties))
                {
                    return false;
                }
                if (!base.Equivalent(contact.Role, this.role, includeFinalProperties))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(contact.Email, this.email))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(contact.Fax, this.fax))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(contact.Uri, this.uri))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(contact.X400, this.x400))
                {
                    return false;
                }
                if (!ObjectUtil.Equivalent(contact.Telephone, this.telephone))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public IList<string> Email
        {
            get
            {
                return new List<string>(email);
            }
        }

        public IList<ITextTypeWrapper> Name
        {
            get
            {
                return new List<ITextTypeWrapper>(name);
            }
        }

        public IList<ITextTypeWrapper> Role
        {
            get
            {
                return new List<ITextTypeWrapper>(role);
            }
        }

        public IList<ITextTypeWrapper> Departments
        {
            get { return new List<ITextTypeWrapper>(departments); }
        }

        public IList<string> Fax
        {
            get
            {
                return new List<string>(fax);
            }
        }

        public IList<string> Telephone
        {
            get
            {
                return new List<string>(telephone);
            }
        }

        public IList<string> Uri
        {
            get
            {
                return new List<string>(uri);
            }
        }

        public IList<string> X400
        {
            get
            {
                return new List<string>(x400);
            }
        }
        #region Methods

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				 //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////	

	    protected override ISet<ISdmxObject> GetCompositesInternal()
        {
		  ISet<ISdmxObject> composites = new HashSet<ISdmxObject>();
	      base.AddToCompositeSet(name, composites);
	      base.AddToCompositeSet(role, composites);
	      base.AddToCompositeSet(departments, composites);
	      return composites;
	    }

        #endregion
    }
}
