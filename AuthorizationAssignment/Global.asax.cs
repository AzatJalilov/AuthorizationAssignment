using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AuthorizationAssignment
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly List<string> _sessionsToDelete = new List<string>();
        public static List<string> SessionsToDelete
        {
            get
            {
                return _sessionsToDelete;
            }
        }
        private static readonly Dictionary<string, string> _sessions = new Dictionary<string, string>();
        private static readonly object padlock = new object();
        public static Dictionary<string, string> Sessions
        {
            get
            {
                return _sessions;
            }
        }

        public static void AddSessionToDeleted(string id)
        {
            _sessionsToDelete.Add(id);
        }
        public static void RemoveSessionFromDeleted(string id)
        {
            _sessionsToDelete.Remove(id);
            _sessions.Remove(id);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            lock (padlock)
            {
                _sessions[Session.SessionID] = User.Identity.Name;
            }
        }
        protected void Session_End(object sender, EventArgs e)
        {
            lock (padlock)
            {
                _sessions.Remove(Session.SessionID);
            }
        }
        
    }
}
