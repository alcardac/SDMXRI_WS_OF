// -----------------------------------------------------------------------
// <copyright file="OrganisationCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using V21 = Org.Sdmx.Resources.SdmxMl.Schemas.V21;
    using V20 = Org.Sdmx.Resources.SdmxMl.Schemas.V20;
    using V10 = Org.Sdmx.Resources.SdmxMl.Schemas.V10;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OrganisationCore<TItem> : ItemCore, IOrganisation
        where TItem : IItemObject 
    {
        private readonly List<IContact> contacts = new List<IContact>();

        	///////////////////////////////////////////////////////////////////////////////////////////////////
	////////////BUILD FROM MUTABLE BEANS			 //////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////
	public OrganisationCore(IOrganisationMutableObject organisationMutableObject, IItemSchemeObject<TItem> parent) : 
        base(organisationMutableObject, parent)
    {
		foreach(IContactMutableObject currentContact in organisationMutableObject.Contacts) {
			this.contacts.Add(new ContactCore(currentContact, this));
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////
	////////////BUILD FROM V2.1 SCHEMA				 //////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////
	public OrganisationCore(V21.Structure.OrganisationType type, 
			SdmxStructureType structureType, 
			IItemSchemeObject<TItem> parent) : base(type, structureType, parent)  {
		
		foreach(V21.Structure.ContactType contact in type.Contact) {
			this.contacts.Add(new ContactCore(contact, this));
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////
	////////////BUILD FROM V2 SCHEMA				 //////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////
	public OrganisationCore(IXmlSerializable createdFrom, SdmxStructureType structureType,
						V20.structure.ContactType contact,
			            string id, 
						Uri uri, 
						IList<V20.common.TextType> name, 
						IList<V20.common.TextType> description,
						V20.common.AnnotationsType annotationsType,
						IIdentifiableObject parent) :base(createdFrom, structureType, id, uri, name, description, annotationsType, parent)
    {
		
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////
	////////////BUILD FROM V1 SCHEMA				 //////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////
	public OrganisationCore(IXmlSerializable createdFrom, 
						SdmxStructureType structureType,
						V10.structure.ContactType contact,
			            string id, 
						Uri uri, 
						IList<V10.common.TextType> name, 
						IList<V10.common.TextType> description, 
						V10.common.AnnotationsType annotationsType,
						IIdentifiableObject parent) : 
                base(createdFrom, structureType, id, uri, name, description, annotationsType, parent)
    {
		
	}
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////COMPOSITES				 //////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
	   protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
		  ISet<ISdmxObject> composites = base.GetCompositesInternal();
		  base.AddToCompositeSet(contacts, composites);
		  return composites;
	   }

       public IList<IContact> Contacts{get { return new List<IContact>(contacts);}}


       /// <summary>
       /// 
       /// </summary>
       /// <param name="organisation"></param>
       /// <param name="includeFinalProperties"></param>
       /// <returns></returns>
       public bool DeepEqualsInternal(IOrganisation organisation, bool includeFinalProperties)
        {
           if(!base.Equivalent(this.contacts, organisation.Contacts, includeFinalProperties)) return false;

           return this.DeepEqualsNameable(organisation, includeFinalProperties);
        }



    }
}
