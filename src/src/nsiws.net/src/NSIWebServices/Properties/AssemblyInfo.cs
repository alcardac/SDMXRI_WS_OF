﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

using log4net.Config;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NSIWebServices")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("NSIWebServices")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b142accd-00d9-4a6f-9c1d-bee7210eb61c")]

// Version information for an assembly consists of the following four values:
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("")]