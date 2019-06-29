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


namespace webserver.Controllers
{
    public class LanguagesController : AuthController
    {
        public HttpResponseMessage Get(int id = 0)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Languages db = new Languages();
            LanguagesModel model = new LanguagesModel();
            List<LanguagesModel> data = new List<LanguagesModel>();
            try
            {
                data = db.get(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, data = data });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }


        public HttpResponseMessage Insert([FromBody] LanguagesModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Languages db = new Languages();
            string name = data.Name;
            if (name == "")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "Name should be valid ." });
            }
            try
            {
                int result = db.Insert(name);
                if (result > 0)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, data = result });
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

        public HttpResponseMessage Update([FromBody] LanguagesModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Languages db = new Languages();
            int id = data.IdLanguage;
            string name = data.Name;
            if (name == "")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "Name should be valid ." });
            }
            try
            {
                int status = db.Update(id, name);

                if (status > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1 });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 0, error = "Can't find data ." });
                }
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
            Languages db = new Languages();
            try
            {
                int status = db.Delete(id);

                if (status > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1 });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 0, error = "Can't find data ." });
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }
    }
}
