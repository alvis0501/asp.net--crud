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
    public class ContentsController : AuthController
    {
        public HttpResponseMessage Get(int id = 0)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Contents db = new Contents();
            ContentsModel model = new ContentsModel();
            List<ContentsModel> data = new List<ContentsModel>();
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


        public HttpResponseMessage Insert([FromBody] ContentsModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Contents db = new Contents();
            int idContype = data.IdContentType;
            try
            {
                int result = db.Insert(idContype);
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

        public HttpResponseMessage Update([FromBody] ContentsModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Contents db = new Contents();
            int id = data.IdContent;
            int idContype = data.IdContentType;
          
            try
            {
                int status = db.Update(id, idContype);

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
            Contents db = new Contents();
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
