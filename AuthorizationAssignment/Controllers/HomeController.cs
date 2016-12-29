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
        private static Dictionary<string, string> _AESIVs = new Dictionary<string, string>();


        public ActionResult Index()
        {
            return View();
        }
        private static string AddEqualSigns(string a)
        {
            int mod4 = a.Length % 4;
            if (mod4 > 0)
            {
                a += new string('=', 4 - mod4);
            }
            return a;
        }
      
        [WebMethod]
        public string GetNewKey(string publicKey, string e)
        {

            var xmlString = $"<RSAKeyValue><Modulus>{AddEqualSigns(publicKey).Replace('-', '+').Replace('_', '/')}</ Modulus >< Exponent >{AddEqualSigns(e).Replace('-', '+').Replace('_', '/')}</ Exponent ></ RSAKeyValue >";
            var key = Encryption.CreateKey(256 / 8);
            var iv = Encryption.CreateKey(16);
            var encryptedKey = Encryption.EncryptStringRSA(key, xmlString);
            var encryptedIV = Encryption.EncryptStringRSA(iv, xmlString);

            _AESKeys[Session.SessionID] = key;
            _AESIVs[Session.SessionID] = iv;
            return encryptedKey + ";" + encryptedIV;
        }

       
        [WebMethod]
        public string Decrypt(string encryptedMessage)
        {
            Thread.Sleep(5000);
            return Encryption.DecryptStringAES(encryptedMessage, _AESKeys[Session.SessionID], _AESIVs[Session.SessionID]);
        }

        [WebMethod]
        public string Encrypt(string message)
        {
            return Encryption.EncryptStringAES(message, _AESKeys[Session.SessionID], _AESIVs[Session.SessionID]);
        }
    }
}