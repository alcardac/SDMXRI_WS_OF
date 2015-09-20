<%@ Page Language="C#" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="Estat.Nsi.DataDisseminationWS" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Estat.Sri.Ws.Controllers.Model" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%
    string applicationPath = Context.Request.ApplicationPath ?? "/";
    if (!applicationPath.EndsWith("/"))
    {
        applicationPath = applicationPath + "/";
    }
    
    Uri requestedUrl = Context.Request.Url;
    Uri applicationUrl = new Uri(requestedUrl, applicationPath);
    
    string dnsSafeHost = requestedUrl.DnsSafeHost;
    IPHostEntry host = Dns.GetHostEntry(dnsSafeHost);
    IPAddress address = Default.TryGetExternal(host);
    bool isLocal = IPAddress.IsLoopback(address) || applicationUrl.IsLoopback;
    bool isPrivate = !isLocal && Default.IsPrivate(address);
    bool isExternal = !(isLocal || isPrivate);

    string resultClass = isExternal ? "external" : isPrivate?"private":"localhost";
    string helpMessage = isExternal
                             ? Messages.help_external
                             : isPrivate ? Messages.help_not_external : Messages.help_localhost;
%>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><%= Messages.index_page_title %></title>
         <meta http-equiv="content-type" content="text/html; charset=utf-8" />
        <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
        <meta http-equiv="PRAGMA" content="NO-CACHE" />
        <!-- common styles -->
        <link href="style/main.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <div id="page-content">
        <div id="page-content-inner">
            <div id="page-header">
                <div id="page-header-left">
                </div>
                <div id="page-header-right">
                </div>
            </div>
            <div id="page-body">
                <div id="pb-workarea-pages">
                    <div id="pb-host">
                        <h1><%= Messages.label_request_info %></h1>
                        <table class="results-table">
                            <thead>
                                <tr>
                                    <th><%= Messages.label_request_property %></th><th><%= Messages.label_request_value %></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <th><%= Messages.label_request_root_url %></th><td><%= applicationUrl.ToString()  %></td>
                                </tr>
                                <tr>
                                    <th><%= Messages.label_request_host %></th><td><%= dnsSafeHost  %></td>
                                    </tr>
                                <tr>
                                    <th><%= Messages.label_request_port %></th><td><%= requestedUrl.Port  %></td>
                                    </tr>
                                <tr>
                                    <th><%= Messages.label_request_is_external %></th><td class="<%= resultClass %>"><%= isExternal.ToString(CultureInfo.CurrentUICulture)  %></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="pb-endpoints">
                        <h1><%= Messages.label_endpoints_info %></h1>
                       <table class="results-table">
                           <thead>
                                  <tr>
                                      <th><%= Messages.label_endpoint_name %></th><th><%= Messages.label_endpoint_path %></th><th><%= Messages.label_endpoint_namespace %></th><th><%= Messages.label_endpoint_wsdl %></th><th><%= Messages.label_endpoint_xsd %></th>
                                  </tr>
                          </thead>
                          <tbody>
                                  <%
                                      foreach (WebServiceInfo webServiceInfo in Default.WebServices)
                                      {
                                          string endpointUrl = string.Format(
                                              CultureInfo.InvariantCulture,
                                              "{0}{1}",
                                              applicationUrl,
                                              webServiceInfo.Name);
                                          %> 
                                                <tr>
                                                    <td><%= webServiceInfo.Description%></td><td><a href="<%= endpointUrl%>"><%= endpointUrl%></a></td><td><%=  webServiceInfo.Namespace%></td><td><a class="page-links" href="<%= string.Format(CultureInfo.InvariantCulture, "{0}{1}?wsdl",applicationPath, webServiceInfo.Name)%>"><%= Messages.lable_endpoint_link_wsdl %></a></td><td><a class="page-links" href="<%= string.Format(CultureInfo.InvariantCulture, "{0}{1}/SDMXMessage.xsd",applicationPath,webServiceInfo.SchemaPath)%>"><%= Messages.lable_endpoint_link_xsd %></a></td>
                                                </tr>        
                                          <%
                                      }
                                   %>
                                 </tbody> 
                       </table>
                    </div>
                    
                    <div id="pb-help" class="help-message">
                        <h1><%= Messages.label_remarks %></h1>
                        <p>
                        <%= helpMessage %>
                        </p>
                    </div>
                </div>
            </div>
            <div id="page-footer">
                <div id="page-footer-copyright">
                    <%= Messages.app_copyright %>
                </div>
                <div id="page-footer-version">
                    <%= Default.Version %>
                </div>
            </div>
        </div>
    </div>
    </body>
</html>
