using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace IJustWatched.Models.CustomConstraits
{
    public class UserIdConstraint: IRouteConstraint 
    {
        private static readonly TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(5);  
  
        public bool Match(HttpContext httpContext,   
            IRouter route,   
            string routeKey,   
            RouteValueDictionary values,   
            RouteDirection routeDirection)  
        {  
            //validate input params  
            if (httpContext == null)  
                throw new ArgumentNullException(nameof(httpContext));  

            if (route == null)  
                throw new ArgumentNullException(nameof(route));  

            if (routeKey == null)  
                throw new ArgumentNullException(nameof(routeKey));  

            if (values == null)  
                throw new ArgumentNullException(nameof(values));

            if(values.TryGetValue(routeKey, out var routeValue))  
            {  
                var parameterValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);  
                return new Regex(@"^[a-z0-9-]+$",   
                    RegexOptions.CultureInvariant, RegexMatchTimeout).IsMatch(parameterValueString);  
            }  

            return false;  
        }  
    }
}