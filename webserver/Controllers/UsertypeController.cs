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
    public class UsertypeController : AuthController
    {
        public HttpResponseMessage Get(int id = 0)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            UserType usertype = new UserType();
            UsertypeModel model = new UsertypeModel();
            List<UsertypeModel> data = new List<UsertypeModel>();
            try
            {
                data = usertype.get(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, data = data });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }


        public HttpResponseMessage Insert([FromBody] UsertypeModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            UserType users = new UserType();
            string description = data.Description;
           
           
            try
            {
                int result = users.Insert(description);
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

        public HttpResponseMessage Update([FromBody] UsertypeModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            UserType users = new UserType();
            int id = data.IdUserType;
            string description = data.Description;
            try
            {
                int status = users.Update(id,description);

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
            UserType users = new UserType();
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
