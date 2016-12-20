using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AuthorizationAssignment.Models;
using System.Collections.Generic;
using System.Collections;
using System.Web.SessionState;
using System.Reflection;
using Microsoft.AspNet.SignalR;

namespace AuthorizationAssignment.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ManageController : Controller
    {
        public ManageController()
        {
        }

        //
        // GET: /Manage/Index
        public ActionResult Index()
        {
            ViewBag.Sessions = SessionHub.MyUsers;
            
            return View();
        }

        //
        // POST: /Manage/Disconnect
        [HttpPost]
        public ActionResult Disconnect(string sessionId, string userId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
            hubContext.Clients.Client(sessionId).disconnect(sessionId);
            
            if(MvcApplication.Sessions.Any(x => x.Value == userId))
            {
                var session = MvcApplication.Sessions.First(x => x.Value == userId);
                MvcApplication.AddSessionToDeleted(session.Key);
            }
            return RedirectToAction("Index");
        }

    }
}