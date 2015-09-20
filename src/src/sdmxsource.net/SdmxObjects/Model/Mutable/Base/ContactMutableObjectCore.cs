// -----------------------------------------------------------------------
// <copyright file="ContactMutableObjectCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class ContactMutableObjectCore : MutableCore, IContactMutableObject
    {

        private readonly IList<ITextTypeWrapperMutableObject> _names = new List<ITextTypeWrapperMutableObject>();

        private readonly IList<ITextTypeWrapperMutableObject> _roles = new List<ITextTypeWrapperMutableObject>();

        private readonly IList<ITextTypeWrapperMutableObject> _departments = new List<ITextTypeWrapperMutableObject>();

        private readonly IList<string> _email = new List<string>();

        private readonly IList<string> _fax = new List<string>();

        private readonly IList<string> _telephone = new List<string>();

        private readonly IList<string> _uri = new List<string>();

        private readonly IList<string> _x400 = new List<string>();

        private string _id;

        public ContactMutableObjectCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Contact))
        {
        }

        public ContactMutableObjectCore(IContact contact)
            : base(contact)
        {
            this._id = contact.Id;

            CopyTextTypes(contact.Name, this._names);
            CopyTextTypes(contact.Role, this._roles);
            CopyTextTypes(contact.Departments, this._departments);
            this._email = new List<string>(contact.Email);
            this._fax = new List<string>(contact.Fax);
            this._telephone = new List<string>(contact.Telephone);
            this._uri = new List<string>(contact.Uri);
            this._x400 = new List<string>(contact.X400);
        }

        private void CopyTextTypes(IList<ITextTypeWrapper> textType, IList<ITextTypeWrapperMutableObject> copyTo)
        {
            if (textType != null)
            {
                foreach (ITextTypeWrapper currentTextType in textType)
                {
                    copyTo.Add(new TextTypeWrapperMutableCore(currentTextType));
                }
            }
        }

        #region Implementation of IContactMutableObject

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public virtual  IList<ITextTypeWrapperMutableObject> Names
        {
            get
            {
                return this._names;
            }
        }

        public void AddName(ITextTypeWrapperMutableObject name)
        {
            this._names.Add(name);
        }

        public virtual IList<ITextTypeWrapperMutableObject> Roles
        {
            get
            {
                return this._roles;
            }
        }

        public void AddRole(ITextTypeWrapperMutableObject role)
        {
            this._roles.Add(role);
        }

        public IList<ITextTypeWrapperMutableObject> Departments
        {
            get
            {
                return this._departments;
            }
        }

        public void AddDepartment(ITextTypeWrapperMutableObject dept)
        {
            this._departments.Add(dept);
        }

        public IList<string> Email
        {
            get
            {
                return this._email;
            }
        }

        public void AddEmail(string email)
        {
            this._email.Add(email);
        }

        public IList<string> Fax
        {
            get
            {
                return this._fax;
            }
        }

        public void AddFax(string fax)
        {
            this._fax.Add(fax);
        }

        public IList<string> Telephone
        {
            get
            {
                return this._telephone;
            }
        }

        public void AddTelephone(string telephone)
        {
            this._telephone.Add(telephone);
        }

        public IList<string> Uri
        {
            get
            {
                return this._uri;
            }
        }

        public void AddUri(string uri)
        {
            this._uri.Add(uri);
        }

        public IList<string> X400
        {
            get
            {
                return this._x400;
            }
        }

        public void AddX400(string x400)
        {
            this._x400.Add(x400);
        }

        #endregion
    }
}
