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

namespace webserver.Controllers
{
    public class UsersController : AuthController
    {
        public CheckModel validCheck(string email, string pass, string fname, string lname, string gender, string avatar, string active, string birthdate,bool flag)
        {
            CheckModel result = new CheckModel();
            string errors = "";
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                Users users = new Users();
                if (flag)
                {
                    bool unique_mail = users.checkUnique_email(email);
                    if (unique_mail)
                    {
                        errors = "email should be Unique .This email already exists. ";
                        result.status = true;
                    }
                }
               
            }
            catch
            {
                errors = "email should be valid .";
                result.status = true;
            }

            if (pass.Length < 6)
            {
                errors = "Password should be longer than 6 characters .";
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
            if (avatar == ""||!IsImageFile(Path.GetExtension(avatar)))
            {
                errors = "Avatar should be valid .";
                result.status = true;
            }
            result.error = errors;
            return result;
        }

       
        public HttpResponseMessage Get(int id = 0)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Users users = new Users();
            UsersModel model = new UsersModel();
            List<UsersModel> data = new List<UsersModel>();
            try
            {
                data = users.get(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, data = data });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }


        public HttpResponseMessage Insert([FromBody] UsersModel data)
        {
            bool checktoken = basicAuthCheck();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Users users = new Users();
            string email = data.email;
            string face_id = "";
            string fname = data.firstName;
            string lname = data.lastName;
            string birthdate = data.birthdate;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            string avatar = "";
            try
            {
                avatar = SetBase64Image(data.avatar);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "Avatar info should be valid . " });
            }
            string gender = data.gender;
            string active = data.active;
            int idlangf = data.idLanguageFirst;
            int idlangs = data.idLanguageSecond;
            int idusertype = data.idUserType;
            string pass = data.password;
            
            CheckModel check = validCheck(email, pass, fname, lname, gender, avatar, active, birthdate,true);
            if (check.status)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = check.error });
            }
            try
            {
                int result = users.Insert(email, pass, fname, lname, birthdate, avatar, gender, face_id, idlangf, idlangs, active, idusertype);
                if (result > 0)
                {
                    string userToken = GenerateToken(email);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, _usertoken = userToken });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = 0 });
                }
                
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }

        }

        public HttpResponseMessage Update([FromBody] UsersModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Users users = new Users();
            int id = data.idUser;
            string email = data.email;
           
            string fname = data.firstName;
            string lname = data.lastName;
            string birthdate = data.birthdate;
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            string avatar = "error";
            try
            {
                avatar = SetBase64Image(data.avatar);
            }
            catch
            {
                avatar = "error";
            }

            string gender = data.gender;
            string active = data.active;
            int idlangf = data.idLanguageFirst;
            int idlangs = data.idLanguageSecond;
            int idusertype = data.idUserType;
            string pass = data.password;
            string facebook_id = "";
            
            CheckModel check = validCheck(email, pass, fname, lname, gender, avatar, active, birthdate,false);
            if (check.status)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = check.error });
            }
            
            try
            {
                int status = users.Update(id, email, pass, fname, lname, birthdate, avatar, gender, facebook_id, idlangf, idlangs, active, idusertype);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = status });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Users users = new Users();
            try
            {
                int status = users.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = status });

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }
    }
}
