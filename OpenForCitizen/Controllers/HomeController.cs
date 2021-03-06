﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mail;
using System.Web.Mvc;
namespace OpenForCitizen.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            ViewBag.Message = "Menu";
            return View();
        }
        public ActionResult Sick()
        {
            ViewBag.Message = "Where does it hurt?";
            return View();
        }
        public ActionResult LevelOfPain()
        {
            ViewBag.Message = "How painfull is it ?";
            return View();
        }
        public ActionResult Questions()
        {
            ViewBag.Message = "Questions ?";
            return View();
        }


        public ActionResult InformationOmVarden()
        {
            return View();
        }

        public ActionResult illnessInfo(string illness)
        {
            // illness_search = illness;
            Session["illness"] = illness;
            Debug.Write("\n\n Illness: " + illness);
            return View();
        }
        [HttpPost]
        public string searchIllness(string illness)
        {
            try
            {
                // Create a request for the URL. 
                WebRequest request =
                    WebRequest.Create(
                        "http://www.1177.se/api/v2/artiklar/?antal=10&key=cc7f8361f7eb4e51b46c95d376c7010a");
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.

                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Clean up the streams and the response.
                reader.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                Debug.Write("\nSearchIllness exception:" + e);
                return null;
            }
        }

        private string mouthCareOpeningHours(string dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Friday.ToString())
                return "07:30 - 16:00";
            if (dayOfWeek == DayOfWeek.Saturday.ToString() || dayOfWeek == DayOfWeek.Sunday.ToString())
                return "Stängt";
            return "07:30 - 17:00";
        } 

        private string vcOpeninghours(string dayOfWeek)
        {
            if (dayOfWeek != DayOfWeek.Sunday.ToString() || dayOfWeek != DayOfWeek.Saturday.ToString())
                return "08:00 - 17:00";
            return "Stängt";
        }

        public string openingHours(string healthplace)
        {
            var dayOfWeek = DateTime.Today.DayOfWeek;
            return healthplace == "vardcentralen" ? vcOpeninghours(dayOfWeek.ToString()) : mouthCareOpeningHours(dayOfWeek.ToString());
        }

        private string vcPhonehours(string dayOfWeek)
        {
            if (dayOfWeek != DayOfWeek.Sunday.ToString() || dayOfWeek != DayOfWeek.Saturday.ToString())
                return "08:15 - 09:30 och 13-14";
            return "Stängt";
        }
        public string phoneHours(string healthplace)
        {
            var dayOfWeek = DateTime.Today.DayOfWeek;
            return healthplace == "vardcentralen" ? vcPhonehours(dayOfWeek.ToString()) : mouthCareOpeningHours(dayOfWeek.ToString());
        }


     
        // user:         VCkronoparken@gmail.com (not using this now )
        // password: krpkrpkrp
        public ActionResult sendMail(string name, string emailFrom, string messageFromUser, string emailTo )
        {
            Debug.Write("\nName:"+ name + "\nemail:" + emailFrom + "\nMessage:"+ messageFromUser + "\nTo:" + emailTo);

            return RedirectToAction("Questions", "Home");
        }

    }
}