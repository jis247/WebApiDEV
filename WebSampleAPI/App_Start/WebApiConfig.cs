using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace WebSampleAPI
{
    public static class WebApiConfig
    {
        //Display json data in browser and xml/json in fiddler
        public class CustomJsonFormatter: JsonMediaTypeFormatter
        {
            public CustomJsonFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            }

            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType=new MediaTypeHeaderValue("application/json");
            }
        }


        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //replaces http with https
            //config.Filters.Add(new RequireHttpsAttribute());

            
            //CustomJsonFormatter class is called
            config.Formatters.Add(new CustomJsonFormatter());


            //Only json format will be displayed both in browser and fiddler.
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
            

            //only xml format will be displayed both in browser and fiddler.
            //config.Formatters.Remove(config.Formatters.JsonFormatter);


            //Change Raw content data to intdented and property name in camelcase.
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
