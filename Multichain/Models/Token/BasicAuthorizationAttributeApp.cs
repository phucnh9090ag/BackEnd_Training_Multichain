using MultiChain.Models.Secure;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Multichain.Models.Token
{
    public class BasicAuthorizationAttributeApp: AuthorizationFilterAttribute
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

                if (appAuth.AuthApp(username, password))
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