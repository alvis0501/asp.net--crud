using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webserver.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace webserver.Controllers
{
    public class LoginController : AuthController
    {
        
        
        public HttpResponseMessage userlogin([FromBody]UserLoginModel data)
        {
            bool checktoken = basicAuthCheck();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }

            string email = data.email;

            string pass = data.password;

          
            Users users = new Users();
            try
            {
                int status = users.authCheck(email, pass);
                
                if (status > 0)
                {
                    //if success..
                    string userToken = GenerateToken(email);
                  
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, _usertoken = userToken });

                }
                else
                {
                    //if not user..
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }

        }
        public HttpResponseMessage facebooklogin([FromBody]UsersModel data)
        {
            bool checktoken = basicAuthCheck();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Users users = new Users();
            string email = data.email;
            string face_id = data.facebook_id;
            string fname = data.firstName;
            string lname = data.lastName;
            string birthdate = data.birthdate;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            string avatar = "avatar";

            string gender = data.gender;
            string active = data.active;
            int idlangf = data.idLanguageFirst;
            int idlangs = data.idLanguageSecond; 
            int idusertype = data.idUserType;
            string pass = "";
            CheckModel check = validCheck(data.email, data.facebook_id, data.firstName, data.lastName, data.gender, avatar, data.active, data.birthdate);
            if (check.status)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = check.error });
            }
            bool check_faceId = users.checkUnique_face(face_id);
            if (!check_faceId) //if no exists in db then save .
            {
                try
                {
                    int result = users.Insert(email, pass, fname, lname, birthdate, avatar, gender, face_id, idlangf, idlangs, active, idusertype);

                    string userToken = GenerateToken(email);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, _usertoken = userToken });
                    

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
                }
            }
            else
            {
                string userToken = GenerateToken(email);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, _usertoken = userToken });
            }



        }
        public CheckModel validCheck(string email, string face_id, string fname, string lname, string gender, string avatar, string active, string birthdate)
        {
            CheckModel result = new CheckModel();
            Users users = new Users();

            string errors = "";
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                bool unique_mail = users.checkUnique_email(email);
                if (unique_mail)
                {
                    errors = "email should be Unique .This email already exists. ";
                    result.status = true;
                }

            }
            catch
            {
                errors = "email should be valid .";
                result.status = true;
            }
            if (face_id == "" || face_id == null)
            {
                errors += "facebook_id should be valid .";
                result.status = true;
            }

            if (fname == "" || lname == "")
            {
                errors += "firstName or lastName should be valid .";
                result.status = true;
            }
            if (gender != "Male" && gender != "Female")
            {
                errors += "Gender should be valid , only 'Male' or 'Female' .";
                result.status = true;
            }
            if (active != "0" && active != "1")
            {
                errors += "active should be valid ,only 0 or 1 .";
                result.status = true;
            }
            try
            {
                DateTime dt = DateTime.Parse(birthdate);
                if ((dt.Month < 1 && dt.Month > 12) || (dt.Day < 1 && dt.Day > 31) || dt.Year < 1800)
                {
                    result.status = true;
                    errors += " birthDate should be valid . please check this format .";
                }

            }
            catch
            {
                result.status = true;
                errors += " birthDate should be valid . please check this format .";
            }
            if (avatar == "error")
            {
                errors = "Avatar should be valid .";
                result.status = true;
            }
            result.error = errors;
            return result;
        }
    }
}
