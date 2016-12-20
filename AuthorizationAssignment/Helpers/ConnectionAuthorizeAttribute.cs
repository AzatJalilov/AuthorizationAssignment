using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationAssignment.Helpers
{
    public class ConnectionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                // The user is not authorized => no need to go any further
                return false;
            }

            if (MvcApplication.SessionsToDelete.Contains(httpContext.Session.SessionID))
            {
                httpContext.Session.Abandon();
                MvcApplication.RemoveSessionFromDeleted(httpContext.Session.SessionID);
                return false;
            }

            return true;
        }
    }
}