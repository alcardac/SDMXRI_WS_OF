// -----------------------------------------------------------------------
// <copyright file="HeaderExtensions.cs" company="Eurostat">
//   Date Created : 2013-04-10
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// The header extensions.
    /// </summary>
    public static class HeaderExtensions
    {
         /// <summary>
         /// Add coordinate type of type <paramref name="coordinateType"/> with <paramref name="coordinateValue"/> to <paramref name="contact"/>
         /// </summary>
         /// <param name="contact">The contact</param>
         /// <param name="coordinateType">The type of communication, e.g. <c>Email</c></param>
         /// <param name="coordinateValue">The value, e.g. <c>foo@bar.gr</c></param>
         public static void AddCoordinateType(this IContactMutableObject contact, string coordinateType, string coordinateValue)
         {
             switch (coordinateType)
             {
                 case "Email":
                     contact.AddEmail(coordinateValue);
                     break;
                 case "Telephone":
                     contact.AddTelephone(coordinateValue);
                     break;
                 case "X400":
                     contact.AddX400(coordinateValue);
                     break;
                 case "Fax":
                     contact.AddFax(coordinateValue);
                     break;
                 case "URI":
                     contact.AddUri(coordinateValue);
                     break;
             }
         }
    }
}