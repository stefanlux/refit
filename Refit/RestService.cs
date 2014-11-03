using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Refit
{
    interface IRestService
    {
        T For<T>(HttpClient client);
    }

    public static class RestService
    {
        public static T For<T>(HttpClient client)
        {
            var className = "AutoGenerated" + typeof(T).Name;
            var requestBuilder = RequestBuilder.ForType<T>();
            var typeName = typeof(T).AssemblyQualifiedName.Replace(typeof(T).Name, className);
            var generatedType = Type.GetType(typeName);

            if(generatedType == null) { 
                var message = typeof(T).Name + " doesn't look like a Refit interface. Make sure it has at least one " + 
                    "method with a Refit HTTP method attribute and Refit is installed in the project.";

                throw new InvalidOperationException(message);
            }

            return (T)Activator.CreateInstance(generatedType, client, requestBuilder);
        }

        public static T For<T>(string hostUrl)
        {
            var client = new HttpClient() { BaseAddress = new Uri(hostUrl) };
            return RestService.For<T>(client);
        }
    }
}
