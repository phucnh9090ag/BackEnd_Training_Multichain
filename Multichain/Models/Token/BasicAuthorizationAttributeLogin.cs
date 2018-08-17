using FinalTestAPI.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FinalTestAPI.Model
{
    public class BasicAuthorizationAttributeLogin : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.
                    CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authorizationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodeAuthorizationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationToken));
                string[] usernamePasswordArr = decodeAuthorizationToken.Split(':');
                string username = usernamePasswordArr[0];
                string password = usernamePasswordArr[1];

                var appAuth = new AuthSecure();

                if (appAuth.AuthAccount(username, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.
                        CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}