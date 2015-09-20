// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsManager.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This singleton class is used to read the configuration settings from the web.config file
//   and load them in corresponding objects
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Web;
    using System.Web.Configuration;

    using Estat.Sri.SdmxXmlConstants;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Properties;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///     This singleton class is used to read the configuration settings from the web.config file
    ///     and load them in corresponding objects
    /// </summary>
    public class SettingsManager
    {
        #region Static Fields

        /// <summary>
        ///     The singleton instance
        /// </summary>
        private static readonly SettingsManager _instance = new SettingsManager();

        #endregion

        #region Fields

        /// <summary>
        ///     Private field that stores the Log object
        /// </summary>
        private readonly ILog _log;

        /// <summary>
        ///     Holds the set of assemblies
        /// </summary>
        private readonly Dictionary<string, Assembly> _platformAssemblies = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     The _default prefix.
        /// </summary>
        private string _defaultPrefix;

        /// <summary>
        ///     Internal field used to store the header attributes of the generated sdmx result
        /// </summary>
        private IHeader _header;

        /// <summary>
        ///     Internal field used to store connection string settings
        /// </summary>
        private ConnectionStringSettings _mappingStoreConnectionSettings;

        /// <summary>
        ///     Holds the configured DDB oracle provider or null
        /// </summary>
        private string _oracleProvider;

        /// <summary>
        ///     Holds the path to 32bit (win32 platform) assemblies
        /// </summary>
        private string _path32 = SettingsConstants.DefaultBin32;

        /// <summary>
        ///     Holds the path to 64bit (x64 platform) assemblies
        /// </summary>
        private string _path64 = SettingsConstants.DefaultBin64;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="SettingsManager" /> class from being created.
        /// </summary>
        private SettingsManager()
        {
            // initialize the log
            this._log = LogManager.GetLogger(typeof(SettingsManager));

            // Initialize any DLL settings
            this.InitializeDll();

            // initialise MappingStoreConnectionSettings
            this.InitialiseMappingStoreConnectionSettings();

            // initialise header
            this.InitialiseHeader();

            // initialize the oracle data provider if any
            this.InitializeDDBProviders();

            this.InitializePrefix();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the default prefix.
        /// </summary>
        /// <value>
        ///     The default prefix.
        /// </value>
        public static string DefaultPrefix
        {
            get
            {
                return _instance._defaultPrefix;
            }
        }

        /// <summary>
        ///     Gets the list of header attributes used by sdmx output file
        ///     In order to help the reading process the <see cref="HeaderSettings" />
        ///     The value for this property is configured in the web.config file in the "applicationSettings" section
        ///     under the <see cref="HeaderSettings" /> node
        /// </summary>
        public static IHeader Header
        {
            get
            {
                return _instance._header;
            }
        }

        /// <summary>
        ///     Gets the connection string settings.
        /// </summary>
        public static ConnectionStringSettings MappingStoreConnectionSettings
        {
            get
            {
                return _instance._mappingStoreConnectionSettings;
            }
        }

        /// <summary>
        ///     Gets the configured DDB Oracle Provider
        /// </summary>
        public static string OracleProvider
        {
            get
            {
                return _instance._oracleProvider;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle resolving of the <c>System.Data.SQLite.dll</c> depending on the current platform
        /// </summary>
        /// <param name="sender">
        /// The source
        /// </param>
        /// <param name="args">
        /// The <see cref="ResolveEventArgs"/>
        /// </param>
        /// <returns>
        /// An <see cref="Assembly"/>; otherwise null
        /// </returns>
        private Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = new AssemblyName(args.Name).Name;
            Assembly dll;
            if (this._platformAssemblies.TryGetValue(name, out dll))
            {
                if (dll == null)
                {
                    dll = this.LoadAssembly(name);
                    this._platformAssemblies[name] = dll;
                }
            }
            else
            {
                _log.Warn(string.Format(CultureInfo.InvariantCulture, "Assembly '{0}' was requested but is not configured.", args.Name));
            }

            return dll;
        }

        /// <summary>
        ///     Get the current platform specific binary path
        /// </summary>
        /// <returns>
        ///     The platform specific path
        /// </returns>
        private string GetPlatformPath()
        {
            return IntPtr.Size == 8 ? this._path64 : this._path32;
        }

        /// <summary>
        ///     This method initializes the log file name.
        ///     The value for this property is configured in the web.config file in the "appSettings" section
        ///     under the "logFileName" key.
        /// </summary>
        private void InitialiseHeader()
        {
            IList<ITextTypeWrapper> name = new List<ITextTypeWrapper>();
            name.Add(new TextTypeWrapperImpl(HeaderSettings.Default.lang, HeaderSettings.Default.name, null));

            IList<ITextTypeWrapper> textTypeWrapperSender = new List<ITextTypeWrapper>();
            textTypeWrapperSender.Add(new TextTypeWrapperImpl(HeaderSettings.Default.lang, HeaderSettings.Default.sendername, null));

            IContactMutableObject senderContact = new ContactMutableObjectCore();
            senderContact.AddName(new TextTypeWrapperMutableCore(HeaderSettings.Default.lang, HeaderSettings.Default.sendercontactname));
            senderContact.AddDepartment(new TextTypeWrapperMutableCore(HeaderSettings.Default.lang, HeaderSettings.Default.sendercontactdepartment));
            senderContact.AddRole(new TextTypeWrapperMutableCore(HeaderSettings.Default.lang, HeaderSettings.Default.sendercontactrole));

            if (!string.IsNullOrEmpty(HeaderSettings.Default.sendercontacttelephone))
            {
                senderContact.AddTelephone(HeaderSettings.Default.sendercontacttelephone);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.sendercontactfax))
            {
                senderContact.AddFax(HeaderSettings.Default.sendercontactfax);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.sendercontactx400))
            {
                senderContact.AddX400(HeaderSettings.Default.sendercontactx400);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.sendercontacturi))
            {
                senderContact.AddUri(HeaderSettings.Default.sendercontacturi);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.sendercontactemail))
            {
                senderContact.AddEmail(HeaderSettings.Default.sendercontactemail);
            }

            // SENDER
            IContact contactImmutableSender = new ContactCore(senderContact);
            IList<IContact> contactsSender = new List<IContact>();
            contactsSender.Add(contactImmutableSender);
            IParty sender = new PartyCore(textTypeWrapperSender, HeaderSettings.Default.senderid, contactsSender, null);

            IList<ITextTypeWrapper> textTypeWrapperReceiver = new List<ITextTypeWrapper>();
            textTypeWrapperReceiver.Add(new TextTypeWrapperImpl(HeaderSettings.Default.lang, HeaderSettings.Default.receivername, null));

            IContactMutableObject receiverContact = new ContactMutableObjectCore();

            receiverContact.AddName(new TextTypeWrapperMutableCore(HeaderSettings.Default.lang, HeaderSettings.Default.receivercontactname));
            receiverContact.AddDepartment(new TextTypeWrapperMutableCore(HeaderSettings.Default.lang, HeaderSettings.Default.receivercontactdepartment));
            receiverContact.AddRole(new TextTypeWrapperMutableCore(HeaderSettings.Default.lang, HeaderSettings.Default.receivercontactrole));

            if (!string.IsNullOrEmpty(HeaderSettings.Default.receivercontacttelephone))
            {
                receiverContact.AddTelephone(HeaderSettings.Default.receivercontacttelephone);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.receivercontactfax))
            {
                receiverContact.AddFax(HeaderSettings.Default.receivercontactfax);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.receivercontactx400))
            {
                receiverContact.AddX400(HeaderSettings.Default.receivercontactx400);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.receivercontacturi))
            {
                receiverContact.AddUri(HeaderSettings.Default.receivercontacturi);
            }

            if (!string.IsNullOrEmpty(HeaderSettings.Default.receivercontactemail))
            {
                receiverContact.AddEmail(HeaderSettings.Default.receivercontactemail);
            }

            // RECEIVER
            IContact contactImmutableReceiver = new ContactCore(receiverContact);
            IList<IContact> contactsReceiver = new List<IContact>();
            contactsReceiver.Add(contactImmutableReceiver);
            IParty receiver = new PartyCore(textTypeWrapperReceiver, HeaderSettings.Default.receiverid, contactsReceiver, null);
            IList<IParty> receiverList = new List<IParty>();
            receiverList.Add(receiver);

            IDictionary<string, string> additionalAttributes = new Dictionary<string, string>();
            additionalAttributes.Add(NameTableCache.GetElementName(ElementNameTable.KeyFamilyRef), HeaderSettings.Default.keyfamilyref);
            additionalAttributes.Add(NameTableCache.GetElementName(ElementNameTable.KeyFamilyAgency), HeaderSettings.Default.keyfamilyagency);
            additionalAttributes.Add(NameTableCache.GetElementName(ElementNameTable.DataSetAgency), HeaderSettings.Default.datasetagency);

            DateTime extracted, prepared, reportingBegin, reportingEnd;
            bool isValid = DateTime.TryParse(HeaderSettings.Default.extracted, out extracted);
            if (!isValid)
            {
                extracted = DateTime.Now;
            }

            isValid = DateTime.TryParse(HeaderSettings.Default.reportingbegin, out reportingBegin);
            if (!isValid)
            {
                reportingBegin = DateTime.Now;
            }

            isValid = DateTime.TryParse(HeaderSettings.Default.reportingend, out reportingEnd);
            if (!isValid)
            {
                reportingEnd = DateTime.Now;
            }

            isValid = DateTime.TryParse(HeaderSettings.Default.prepared, out prepared);
            if (!isValid)
            {
                prepared = DateTime.Now;
            }

            IList<ITextTypeWrapper> source = new List<ITextTypeWrapper>();
            if (!string.IsNullOrEmpty(HeaderSettings.Default.source))
            {
                source.Add(new TextTypeWrapperImpl(HeaderSettings.Default.lang, HeaderSettings.Default.source, null));
            }

            this._header = new HeaderImpl(
                additionalAttributes, 
                null, 
                null, 
                DatasetAction.GetAction(HeaderSettings.Default.datasetaction), 
                HeaderSettings.Default.id, 
                HeaderSettings.Default.datasetid, 
                null, 
                extracted, 
                prepared, 
                reportingBegin, 
                reportingEnd, 
                name, 
                source, 
                receiverList, 
                sender, 
                bool.Parse(HeaderSettings.Default.test));
        }

        /// <summary>
        ///     This method initializes the connection string settings. It reads them from the web.config file.
        /// </summary>
        private void InitialiseMappingStoreConnectionSettings()
        {
            this._mappingStoreConnectionSettings = WebConfigurationManager.ConnectionStrings[SettingsConstants.MappingStoreConnectionName];
            if (this._mappingStoreConnectionSettings == null)
            {
                _log.ErrorFormat(
                    "No connection string with name {0} could not be found. Please add at the Web.config at <connectionStrings> section a connection string with name {0}.", 
                    SettingsConstants.MappingStoreConnectionName);
            }
        }

        /// <summary>
        ///     This method initializes the DDB default providers. Currently only for Oracle
        /// </summary>
        private void InitializeDDBProviders()
        {
            this._oracleProvider = WebConfigurationManager.AppSettings[SettingsConstants.DefaultDdbOracleProvider];
        }

        /// <summary>
        ///     Append any path and platform specific settings
        /// </summary>
        private void InitializeDll()
        {
            string p32 = WebConfigurationManager.AppSettings[SettingsConstants.Bin32];
            string p64 = WebConfigurationManager.AppSettings[SettingsConstants.Bin64];
            if (!string.IsNullOrEmpty(p32))
            {
                this._path32 = p32;
            }

            if (!string.IsNullOrEmpty(p64))
            {
                this._path64 = p64;
            }

            string path = Path.Combine(HttpContext.Current.Server.MapPath(SettingsConstants.VirtualBin), this.GetPlatformPath());
            string configuredPath = WebConfigurationManager.AppSettings[SettingsConstants.PathSetting];

            if (!string.IsNullOrEmpty(configuredPath))
            {
                _log.Info("Configured path:" + configuredPath);
                path = string.Format(CultureInfo.InvariantCulture, "{0};{1}", configuredPath, path);
            }

            string env = Environment.GetEnvironmentVariable(SettingsConstants.PathEnvironment);
            Environment.SetEnvironmentVariable(SettingsConstants.PathEnvironment, string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", path, Path.PathSeparator, env));
            this._platformAssemblies.Add(SettingsConstants.SqlLiteDataProvider, null);
            string assemlies = WebConfigurationManager.AppSettings[SettingsConstants.PlatformSpecificAssemblies];
            if (!string.IsNullOrEmpty(assemlies))
            {
                string[] list = assemlies.Split(',');
                foreach (string assembly in list)
                {
                    this._platformAssemblies[assembly.Trim()] = null;
                }
            }

            AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomainAssemblyResolve;
        }

        /// <summary>
        ///     Initializes the prefix.
        /// </summary>
        private void InitializePrefix()
        {
            this._defaultPrefix = WebConfigurationManager.AppSettings[SettingsConstants.DefaultPrefix];
        }

        /// <summary>
        /// Try load the assembly with the specified simple name from the <see cref="GetPlatformPath()"/> path
        /// </summary>
        /// <param name="name">
        /// The simple name of the assembly
        /// </param>
        /// <returns>
        /// The assembly or null
        /// </returns>
        private Assembly LoadAssembly(string name)
        {
            string platformPath = this.GetPlatformPath();

            string dllPath = Path.Combine(platformPath, name + SettingsConstants.DllExtension);
            Assembly ret = null;

            string info = string.Format(CultureInfo.InvariantCulture, Resources.LoadingAssemblyFormat1, name);
            _log.Info(info);

            string root = HttpContext.Current.Server.MapPath(SettingsConstants.VirtualBin);
            dllPath = Path.Combine(root, dllPath);
            dllPath = Path.GetFullPath(dllPath);
            if (!File.Exists(dllPath))
            {
                string message = string.Format(CultureInfo.InvariantCulture, Resources.ErrorAssemblyDoesntExistFormat2, name, dllPath);
                _log.Error(message);
                return null;
            }

            try
            {
                AssemblyName aname = AssemblyName.GetAssemblyName(dllPath);
                if (aname != null && aname.Name.Contains(name))
                {
                    ret = Assembly.Load(aname);
                    _log.Info(string.Format(CultureInfo.InvariantCulture, Resources.SettingsManager_LoadAssembly_Assembly__0__was_loaded_successfully, name));
                }
            }
            catch (FileLoadException e)
            {
                _log.Error(string.Format(CultureInfo.InvariantCulture, Resources.SettingsManager_LoadAssembly_An_error_occured_while_trying_to_pre_load_DLL_____0__, e));
            }
            catch (BadImageFormatException e)
            {
                _log.Error(string.Format(CultureInfo.InvariantCulture, Resources.SettingsManager_LoadAssembly_The_DLL___0___seems_to_be_for_a_different_architecture, dllPath));
                _log.Error(e.ToString());
            }
            catch (SecurityException e)
            {
                _log.Error(string.Format(CultureInfo.InvariantCulture, Resources.SettingsManager_LoadAssembly_Could_not_access_the__0___Please_check_the_permissions, dllPath));
                _log.Error(e.ToString());
            }
            catch (ArgumentException e)
            {
                _log.Error(string.Format(CultureInfo.InvariantCulture, Resources.SettingsManager_LoadAssembly_An_error_occured_while_trying_to_pre_load_DLL_____0__, e));
            }
            catch (FileNotFoundException e)
            {
                string message = string.Format(CultureInfo.InvariantCulture, Resources.ErrorAssemblyDoesntExistFormat2, dllPath, dllPath);
                _log.Error(message);
                _log.Error(e.ToString());
            }

            return ret;
        }

        #endregion
    }
}