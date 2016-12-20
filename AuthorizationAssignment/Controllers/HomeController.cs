using AuthorizationAssignment.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace AuthorizationAssignment.Controllers
{

    [ConnectionAuthorize]
    public class HomeController : Controller
    {
        private static Dictionary<string, string> _AESKeys = new Dictionary<string, string>();

        public ActionResult Index()
        {
            return View();
        }
        [WebMethod]
        public string GetNewKey()
        {
            var key = Encryption.CreateKey(256 / 8);
            _AESKeys[Session.SessionID] = key;
            return key;
        }
        [WebMethod]
        public string Decrypt(string encryptedMessage)
        {
            Thread.Sleep(5000);
            return Encryption.DecryptStringAES(encryptedMessage, _AESKeys[Session.SessionID]);
        }

        [WebMethod]
        public string Encrypt(string message)
        {
            return Encryption.EncryptStringAES(message, _AESKeys[Session.SessionID]);
        }
    }
}