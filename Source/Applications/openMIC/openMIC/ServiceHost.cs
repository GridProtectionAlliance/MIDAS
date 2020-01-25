﻿//******************************************************************************************************
//  ServiceHost.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/02/2009 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.ComponentModel;
using GSF.Configuration;
using GSF.Diagnostics;
using GSF.IO;
using GSF.Security;
using GSF.Security.Model;
using GSF.ServiceProcess;
using GSF.TimeSeries;
using GSF.TimeSeries.Adapters;
using GSF.Web.Hosting;
using GSF.Web.Model;
using GSF.Web.Model.Handlers;
using GSF.Web.Security;
using GSF.Web.Shared;
using GSF.Web.Shared.Model;
using Microsoft.Owin.Hosting;
using Microsoft.SqlServer.Server;
using openMIC.Model;

namespace openMIC
{
    public class ServiceHost : ServiceHostBase
    {
        #region [ Members ]

        // Constants
        private const int DefaultMaximumDiagnosticLogSize = 10;

        // Events

        /// <summary>
        /// Raised when there is a new status message reported to service.
        /// </summary>
        public event EventHandler<EventArgs<Guid, string, UpdateType>> UpdatedStatus;

        /// <summary>
        /// Raised when there is a new exception logged to service.
        /// </summary>
        public event EventHandler<EventArgs<Exception>> LoggedException;

        // Fields
        private IDisposable m_webAppHost;
        private long m_currentPoolTarget;
        private bool m_serviceStopping;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="ServiceHost"/> instance.
        /// </summary>
        public ServiceHost()
        {
            ServiceName = "openMIC";
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the configured default web page for the application.
        /// </summary>
        public string DefaultWebPage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the model used for the application.
        /// </summary>
        public AppModel Model
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of downloaders that were loaded into the input adapter collection.
        /// </summary>
        public List<Downloader> Downloaders
        {
            get
            {
                lock (InputAdapters)
                {
                    return InputAdapters.OfType<Downloader>().ToList();
                }
            }
        }

        /// <summary>
        /// Gets current performance statistics.
        /// </summary>
        public string PerformanceStatistics => ServiceHelper?.PerformanceMonitor?.Status;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ServiceHost"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        m_webAppHost?.Dispose();
                    }
                }
                finally
                {
                    m_disposed = true;          // Prevent duplicate dispose.
                    base.Dispose(disposing);    // Call base class Dispose().
                }
            }
        }

        /// <summary>
        /// Event handler for service starting operations.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments containing command line arguments passed into service at startup.</param>
        protected override void ServiceStartingHandler(object sender, EventArgs<string[]> e)
        {
            // Handle base class service starting procedures
            base.ServiceStartingHandler(sender, e);

            // Make sure openMIC specific default service settings exist
            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
            CategorizedSettingsElementCollection securityProvider = ConfigurationFile.Current.Settings["securityProvider"];

            // Define set of default anonymous web resources for this site
            const string DefaultAnonymousResourceExpression = "^/@|^/Scripts/|^/Content/|^/Images/|^/fonts/|^/favicon.ico$";

            systemSettings.Add("CompanyName", "Grid Protection Alliance", "The name of the company who owns this instance of the openMIC.");
            systemSettings.Add("CompanyAcronym", "GPA", "The acronym representing the company who owns this instance of the openMIC.");
            systemSettings.Add("DiagnosticLogPath", FilePath.GetAbsolutePath(""), "Path for diagnostic logs.");
            systemSettings.Add("MaximumDiagnosticLogSize", DefaultMaximumDiagnosticLogSize, "The combined maximum size for the diagnostic logs in whole Megabytes; curtailment happens hourly. Set to zero for no limit.");
            systemSettings.Add("WebHostURL", "http://+:8089", "The web hosting URL for remote system management.");
            systemSettings.Add("WebRootPath", "wwwroot", "The root path for the hosted web server files. Location will be relative to install folder if full path is not specified.");
            systemSettings.Add("DefaultWebPage", "Index.cshtml", "The default web page for the hosted web server.");
            systemSettings.Add("DateFormat", "MM/dd/yyyy", "The default date format to use when rendering timestamps.");
            systemSettings.Add("TimeFormat", "HH:mm:ss.fff", "The default time format to use when rendering timestamps.");
            systemSettings.Add("BootstrapTheme", "Content/bootstrap.min.css", "Path to Bootstrap CSS to use for rendering styles.");
            systemSettings.Add("SubscriptionConnectionString", "server=localhost:6195; interface=0.0.0.0", "Connection string for data subscriptions to openMIC server.");
            systemSettings.Add("AuthenticationSchemes", AuthenticationOptions.DefaultAuthenticationSchemes, "Comma separated list of authentication schemes to use for clients accessing the hosted web server, e.g., Basic or NTLM.");
            systemSettings.Add("AuthFailureRedirectResourceExpression", AuthenticationOptions.DefaultAuthFailureRedirectResourceExpression, "Expression that will match paths for the resources on the web server that should redirect to the LoginPage when authentication fails.");
            systemSettings.Add("AnonymousResourceExpression", DefaultAnonymousResourceExpression, "Expression that will match paths for the resources on the web server that can be provided without checking credentials.");
            systemSettings.Add("AuthenticationToken", SessionHandler.DefaultAuthenticationToken, "Defines the token used for identifying the authentication token in cookie headers.");
            systemSettings.Add("SessionToken", SessionHandler.DefaultSessionToken, "Defines the token used for identifying the session ID in cookie headers.");
            systemSettings.Add("RequestVerificationToken", AuthenticationOptions.DefaultRequestVerificationToken, "Defines the token used for anti-forgery verification in HTTP request headers.");
            systemSettings.Add("LoginPage", AuthenticationOptions.DefaultLoginPage, "Defines the login page used for redirects on authentication failure. Expects forward slash prefix.");
            systemSettings.Add("AuthTestPage", AuthenticationOptions.DefaultAuthTestPage, "Defines the page name for the web server to test if a user is authenticated. Expects forward slash prefix.");
            systemSettings.Add("Realm", "", "Case-sensitive identifier that defines the protection space for the web based authentication and is used to indicate a scope of protection.");
            systemSettings.Add("DefaultCorsOrigins", "", "Comma-separated list of allowed origins (including http:// prefix) that define the default CORS policy. Use '*' to allow all or empty string to disable CORS.");
            systemSettings.Add("DefaultCorsHeaders", "*", "Comma-separated list of supported headers that define the default CORS policy. Use '*' to allow all or empty string to allow none.");
            systemSettings.Add("DefaultCorsMethods", "*", "Comma-separated list of supported methods that define the default CORS policy. Use '*' to allow all or empty string to allow none.");
            systemSettings.Add("DefaultCorsSupportsCredentials", true, "Boolean flag for the default CORS policy indicating whether the resource supports user credentials in the request.");

            systemSettings.Add("DefaultDialUpRetries", 3, "Default dial-up connection retries.");
            systemSettings.Add("DefaultDialUpTimeout", 90, "Default dial-up connection timeout.");
            systemSettings.Add("DefaultFTPUserName", "anonymous", "Default FTP user name to use for device connections.");
            systemSettings.Add("DefaultFTPPassword", "anonymous", "Default FTP password to use for device connections.");
            systemSettings.Add("DefaultRemotePath", "/", "Default remote FTP path to use for device connections.");
            systemSettings.Add("DefaultLocalPath", "", "Default local path to use for file downloads.");
            systemSettings.Add("MaxRemoteFileAge", "30", "Maximum remote file age, in days, to apply for downloads when limit is enabled.");
            systemSettings.Add("MaxLocalFileAge", "365", "Maximum local file age, in days, to apply for downloads when limit is enabled.");
            systemSettings.Add("SmtpServer", "localhost", "The SMTP relay server from which to send e-mails.");
            systemSettings.Add("FromAddress", "openmic@gridprotectionalliance.org", "The from address for e-mails.");
            systemSettings.Add("SmtpUserName", "", "Username to authenticate to the SMTP server, if any.");
            systemSettings.Add("SmtpPassword", "", "Password to authenticate to the SMTP server, if any.");
            systemSettings.Add("PoolMachines", "", "Comma separated list of openMIC pooled load-balancing machines");

            DefaultWebPage = systemSettings["DefaultWebPage"].Value;

            Model = new AppModel();
            Model.Global.CompanyName = systemSettings["CompanyName"].Value;
            Model.Global.CompanyAcronym = systemSettings["CompanyAcronym"].Value;
            Model.Global.NodeID = Guid.Parse(systemSettings["NodeID"].Value);
            Model.Global.SubscriptionConnectionString = systemSettings["SubscriptionConnectionString"].Value;
            Model.Global.ApplicationName = "openMIC";
            Model.Global.ApplicationDescription = "open Meter Information Collection System";
            Model.Global.ApplicationKeywords = "open source, utility, software, meter, interrogation";
            Model.Global.DateFormat = systemSettings["DateFormat"].Value;
            Model.Global.TimeFormat = systemSettings["TimeFormat"].Value;
            Model.Global.DateTimeFormat = $"{Model.Global.DateFormat} {Model.Global.TimeFormat}";
            Model.Global.PasswordRequirementsRegex = securityProvider["PasswordRequirementsRegex"].Value;
            Model.Global.PasswordRequirementsError = securityProvider["PasswordRequirementsError"].Value;
            Model.Global.BootstrapTheme = systemSettings["BootstrapTheme"].Value;
            Model.Global.PasswordRequirementsRegex = securityProvider["PasswordRequirementsRegex"].Value;
            Model.Global.PasswordRequirementsError = securityProvider["PasswordRequirementsError"].Value;
            Model.Global.DefaultDialUpRetries = int.Parse(systemSettings["DefaultDialUpRetries"].Value);
            Model.Global.DefaultDialUpTimeout = int.Parse(systemSettings["DefaultDialUpTimeout"].Value);
            Model.Global.DefaultFTPUserName = systemSettings["DefaultFTPUserName"].Value;
            Model.Global.DefaultFTPPassword = systemSettings["DefaultFTPPassword"].Value;
            Model.Global.DefaultRemotePath = systemSettings["DefaultRemotePath"].Value;
            Model.Global.DefaultLocalPath = FilePath.GetAbsolutePath(systemSettings["DefaultLocalPath"].Value);
            Model.Global.MaxRemoteFileAge = int.Parse(systemSettings["MaxRemoteFileAge"].Value);
            Model.Global.MaxLocalFileAge = int.Parse(systemSettings["MaxLocalFileAge"].Value);
            Model.Global.DefaultAppPath = FilePath.GetAbsolutePath("");
            Model.Global.SmtpServer = systemSettings["SmtpServer"].Value;
            Model.Global.FromAddress = systemSettings["FromAddress"].Value;
            Model.Global.SmtpUserName = systemSettings["SmtpUserName"].Value;
            Model.Global.SmtpPassword = systemSettings["SmtpPassword"].Value;
            Model.Global.WebRootPath = FilePath.GetAbsolutePath(systemSettings["WebRootPath"].Value);
            Model.Global.DefaultCorsOrigins = systemSettings["DefaultCorsOrigins"].Value;
            Model.Global.DefaultCorsHeaders = systemSettings["DefaultCorsHeaders"].Value;
            Model.Global.DefaultCorsMethods = systemSettings["DefaultCorsMethods"].Value;
            Model.Global.DefaultCorsSupportsCredentials = systemSettings["DefaultCorsSupportsCredentials"].ValueAsBoolean(true);

            // Setup pooled machine configuration
            string poolMachines = systemSettings["PoolMachines"].Value;

            if (!string.IsNullOrWhiteSpace(poolMachines))
            {
                // Add configured pool machines making sure to also add local machine
                HashSet<string> poolMachineList = new HashSet<string>(poolMachines.Split(',')) { "localhost" };
                Model.Global.PoolMachines = poolMachineList.ToArray();
            }

            // Determine web port
            string webHostURL = systemSettings["WebHostURL"].Value;
            string[] parts = webHostURL.Split(':');

            if (parts.Length > 1 && ushort.TryParse(parts[parts.Length - 1], out ushort port))
                Model.Global.WebPort = port;
            else
                Model.Global.WebPort = 8089;

            // Parse configured authentication schemes
            if (!Enum.TryParse(systemSettings["AuthenticationSchemes"].ValueAs(AuthenticationOptions.DefaultAuthenticationSchemes.ToString()), true, out AuthenticationSchemes authenticationSchemes))
                authenticationSchemes = AuthenticationOptions.DefaultAuthenticationSchemes;

            // Initialize web startup configuration
            Startup.AuthenticationOptions.AuthenticationSchemes = authenticationSchemes;
            Startup.AuthenticationOptions.AuthFailureRedirectResourceExpression = systemSettings["AuthFailureRedirectResourceExpression"].ValueAs(AuthenticationOptions.DefaultAuthFailureRedirectResourceExpression);
            Startup.AuthenticationOptions.AnonymousResourceExpression = systemSettings["AnonymousResourceExpression"].ValueAs(DefaultAnonymousResourceExpression);
            Startup.AuthenticationOptions.AuthenticationToken = systemSettings["AuthenticationToken"].ValueAs(SessionHandler.DefaultAuthenticationToken);
            Startup.AuthenticationOptions.SessionToken = systemSettings["SessionToken"].ValueAs(SessionHandler.DefaultSessionToken);
            Startup.AuthenticationOptions.RequestVerificationToken = systemSettings["RequestVerificationToken"].ValueAs(AuthenticationOptions.DefaultRequestVerificationToken);
            Startup.AuthenticationOptions.LoginPage = systemSettings["LoginPage"].ValueAs(AuthenticationOptions.DefaultLoginPage);
            Startup.AuthenticationOptions.AuthTestPage = systemSettings["AuthTestPage"].ValueAs(AuthenticationOptions.DefaultAuthTestPage);
            Startup.AuthenticationOptions.Realm = systemSettings["Realm"].ValueAs("");
            Startup.AuthenticationOptions.LoginHeader = $"<h3><img src=\"/Images/{Model.Global.ApplicationName}.png\"/> {Model.Global.ApplicationName}</h3>";

            // Validate that configured authentication test page does not evaluate as an anonymous resource nor a authentication failure redirection resource
            string authTestPage = Startup.AuthenticationOptions.AuthTestPage;

            if (Startup.AuthenticationOptions.IsAnonymousResource(authTestPage))
                throw new SecurityException($"The configured authentication test page \"{authTestPage}\" evaluates as an anonymous resource. Modify \"AnonymousResourceExpression\" setting so that authorization test page is not a match.");

            if (Startup.AuthenticationOptions.IsAuthFailureRedirectResource(authTestPage))
                throw new SecurityException($"The configured authentication test page \"{authTestPage}\" evaluates as an authentication failure redirection resource. Modify \"AuthFailureRedirectResourceExpression\" setting so that authorization test page is not a match.");

            if (Startup.AuthenticationOptions.AuthenticationToken == Startup.AuthenticationOptions.SessionToken)
                throw new InvalidOperationException("Authentication token must be different from session token in order to differentiate the cookie values in the HTTP headers.");

            // Register a symbolic reference to global settings for use by default value expressions
            ValueExpressionParser.DefaultTypeRegistry.RegisterSymbol("Global", Program.Host.Model.Global);

            ServiceHelper.UpdatedStatus += UpdatedStatusHandler;
            ServiceHelper.LoggedException += LoggedExceptionHandler;

            // Attach to default web server events
            WebServer webServer = WebServer.Default;
            webServer.StatusMessage += WebServer_StatusMessage;
            webServer.ExecutionException += LoggedExceptionHandler;

            // Define types for Razor pages - self-hosted web service does not use view controllers so
            // we must define configuration types for all paged view model based Razor views here:
            webServer.PagedViewModelTypes.TryAdd("Devices.cshtml", new Tuple<Type, Type>(typeof(Device), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Companies.cshtml", new Tuple<Type, Type>(typeof(Company), typeof(SharedHub)));
            webServer.PagedViewModelTypes.TryAdd("Vendors.cshtml", new Tuple<Type, Type>(typeof(Vendor), typeof(SharedHub)));
            webServer.PagedViewModelTypes.TryAdd("VendorDevices.cshtml", new Tuple<Type, Type>(typeof(VendorDevice), typeof(SharedHub)));
            webServer.PagedViewModelTypes.TryAdd("Users.cshtml", new Tuple<Type, Type>(typeof(UserAccount), typeof(SecurityHub)));
            webServer.PagedViewModelTypes.TryAdd("Groups.cshtml", new Tuple<Type, Type>(typeof(SecurityGroup), typeof(SecurityHub)));
            webServer.PagedViewModelTypes.TryAdd("ConnectionProfiles.cshtml", new Tuple<Type, Type>(typeof(ConnectionProfile), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("ConnectionProfileTasks.cshtml", new Tuple<Type, Type>(typeof(ConnectionProfileTask), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Settings.cshtml", new Tuple<Type, Type>(typeof(Setting), typeof(DataHub)));

            // Define exception logger for CSV downloader
            CsvDownloadHandler.LogExceptionHandler = LogException;

            new Thread(() =>
            {
                const int RetryDelay = 1000;
                const int SleepTime = 200;
                const int LoopCount = RetryDelay / SleepTime;

                while (!m_serviceStopping)
                {
                    if (TryStartWebHosting(webHostURL))
                    {
                        try
                        {
                            // Initiate pre-compile of base templates
                            RazorEngine<CSharpEmbeddedResource>.Default.PreCompile(LogException);
                            RazorEngine<CSharpEmbeddedResource>.Default.PreCompile(LogException, "GSF.Web.Security.Views.");
                            RazorEngine<CSharpEmbeddedResource>.Default.PreCompile(LogException, "GSF.Web.Shared.Views.");
                            RazorEngine<CSharp>.Default.PreCompile(LogException);
                        }
                        catch (Exception ex)
                        {
                            LogException(new InvalidOperationException($"Failed to initiate pre-compile of razor templates: {ex.Message}", ex));
                        }

                        break;
                    }

                    for (int i = 0; i < LoopCount && !m_serviceStopping; i++)
                        Thread.Sleep(SleepTime);
                }
            })
            {
                IsBackground = true
            }
            .Start();
        }

        private bool TryStartWebHosting(string webHostURL)
        {
            try
            {
                // Create new web application hosting environment
                m_webAppHost = WebApp.Start<Startup>(webHostURL);
                return true;
            }
            catch (TargetInvocationException ex)
            {
                LogException(new InvalidOperationException($"Failed to initialize web hosting: {ex.InnerException?.Message ?? ex.Message}", ex.InnerException ?? ex));
                return false;
            }
            catch (Exception ex)
            {
                LogException(new InvalidOperationException($"Failed to initialize web hosting: {ex.Message}", ex));
                return false;
            }
        }

        /// <summary>Event handler for service stopping operation.</summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// Time-series framework uses this handler to un-wire events and dispose of system objects.
        /// </remarks>
        protected override void ServiceStoppingHandler(object sender, EventArgs e)
        {
            m_serviceStopping = true;

            ServiceHelper helper = ServiceHelper;

            try
            {
                base.ServiceStoppingHandler(sender, e);
            }
            catch (Exception ex)
            {
                LogException(new InvalidOperationException($"Service stopping handler exception: {ex.Message}", ex));
            }

            if ((object)helper != null)
            {
                helper.UpdatedStatus -= UpdatedStatusHandler;
                helper.LoggedException -= LoggedExceptionHandler;
            }
        }

        private void WebServer_StatusMessage(object sender, EventArgs<string> e)
        {
            LogWebHostStatusMessage(e.Argument);
        }

        public void LogWebHostStatusMessage(string message, UpdateType type = UpdateType.Information)
        {
            LogStatusMessage($"[WEBHOST] {message}", type);
        }

        /// <summary>
        /// Logs a status message to connected clients.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of message to log.</param>
        public void LogStatusMessage(string message, UpdateType type = UpdateType.Information)
        {
            DisplayStatusMessage(message, type);
        }

        /// <summary>
        /// Logs an exception to the service.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        public new void LogException(Exception ex)
        {
            base.LogException(ex);
            DisplayStatusMessage($"{ex.Message}", UpdateType.Alarm);
        }

        /// <summary>
        /// Queues task for operation at specified <paramref name="priority"/>.
        /// </summary>
        /// <param name="acronym">Target device.</param>
        /// <param name="priority">Priority of task to use when queuing.</param>
        public void QueueTasksWithPriority(string acronym, QueuePriority priority)
        {
            string[] pooledMachines = Model.Global.PoolMachines;

            if (pooledMachines == null || pooledMachines.Length == 0)
            {
                QueueTasksLocally(acronym, priority);
                return;
            }

            string targetMachine = pooledMachines[m_currentPoolTarget++ % pooledMachines.Length];

            if (targetMachine.Equals("localhost"))
            {
                QueueTasksLocally(acronym, priority);
                return;
            }

            string actionURI = $"http://{targetMachine}:{Model.Global.WebPort}/api/Operations/QueueTasksWithPriority?priority={priority}&target={acronym}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, actionURI);

            try
            {
                Task<HttpResponseMessage> task = s_http.SendAsync(request);
                task.Wait();

                HttpResponseMessage response = task.Result;

                if (response.StatusCode == HttpStatusCode.OK)
                    LogStatusMessage($"REMOTE QUEUE: Successfully executed remote queue action \"{actionURI}\" for \"{acronym}\" task load at {priority} priority");
                else
                    throw new Exception($"REMOTE QUEUE: Failed to execute remote queue action \"{actionURI}\" for \"{acronym}\", HTTP response = {response.StatusCode}: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                if (ex is AggregateException aggEx)
                    LogException(new Exception($"REMOTE QUEUE: Failed to execute remote queue action \"{actionURI}\" for \"{acronym}\": {string.Join(", ", aggEx.InnerExceptions.Select(innerEx => innerEx.Message))}"));
                else
                    LogException(ex);

                // Move on to next pool machine when queue fails
                QueueTasksWithPriority(acronym, priority);
            }
        }

        private void QueueTasksLocally(string acronym, QueuePriority priority)
        {
            IAdapter adapter = GetRequestedAdapter(new ClientRequestInfo(null, ClientRequest.Parse($"Invoke {acronym}")));

            if (adapter is Downloader downloader)
                downloader.QueueTasksWithPriority(priority);
        }

        /// <summary>
        /// Sends a command request to the service.
        /// </summary>
        /// <param name="userInput">Request string.</param>
        /// <param name="clientID">Client ID of sender.</param>
        /// <param name="principal">The principal used for role-based security.</param>
        public void SendRequest(string userInput, Guid clientID = default, IPrincipal principal = null)
        {
            ClientRequest request = ClientRequest.Parse(userInput);

            if (request == null)
                return;

            if (principal != null && SecurityProviderUtility.IsResourceSecurable(request.Command) && !SecurityProviderUtility.IsResourceAccessible(request.Command, principal))
            {
                ServiceHelper.UpdateStatus(clientID, UpdateType.Alarm, $"Access to \"{request.Command}\" is denied.\r\n\r\n");
                return;
            }

            ClientRequestHandler requestHandler = ServiceHelper.FindClientRequestHandler(request.Command);

            if (requestHandler == null)
            {
                ServiceHelper.UpdateStatus(clientID, UpdateType.Alarm, $"Command \"{request.Command}\" is not supported.\r\n\r\n");
                return;
            }

            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ClientID = clientID;
            clientInfo.SetClientUser(principal ?? Thread.CurrentPrincipal);

            ClientRequestInfo requestInfo = new ClientRequestInfo(clientInfo, request);
            requestHandler.HandlerMethod(requestInfo);
        }

        public void DisconnectClient(Guid clientID)
        {
            ServiceHelper.DisconnectClient(clientID);
        }

        private void UpdatedStatusHandler(object sender, EventArgs<Guid, string, UpdateType> e)
        {
            if ((object)UpdatedStatus != null)
                UpdatedStatus(sender, new EventArgs<Guid, string, UpdateType>(e.Argument1, e.Argument2, e.Argument3));
        }

        private void LoggedExceptionHandler(object sender, EventArgs<Exception> e)
        {
            if ((object)LoggedException != null)
                LoggedException(sender, new EventArgs<Exception>(e.Argument));
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HttpClient s_http = new HttpClient(new HttpClientHandler { UseCookies = false });

        #endregion
    }
}
