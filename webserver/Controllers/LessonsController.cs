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
    public class LessonsController : AuthController
    {
        public CheckModel validCheck(int idcon, int idlevel, string active, int order)
        {
            CheckModel result = new CheckModel();
            string errors = "";

            if (idcon < 1)
            {
                errors = "idContent should be valid .";
                result.status = true;
            }

            if (idlevel < 1)
            {
                errors += "idLevel should be valid .";
                result.status = true;
            }

            if (order < 1)
            {
                errors += "order should be valid. ";
                result.status = true;
            }

            if (active != "0" && active != "1")
            {
                errors += "active should be valid ,only 0 or 1 .";
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
            Lessons db = new Lessons();
            LessonsModel model = new LessonsModel();
            List<LessonsModel> data = new List<LessonsModel>();
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


        public HttpResponseMessage Insert([FromBody] LessonsModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Lessons db = new Lessons();
            int idcon =data.idContent;
            int idlevel = data.idLevel;
            string active = data.active;
            int order = data.order;
            CheckModel check = new CheckModel();
            check = validCheck(idcon, idlevel, active, order);
            if (check.status)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = check.error });
            }
            try
            {
                int result = db.Insert(idcon, idlevel, active, order);
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

        public HttpResponseMessage Update([FromBody] LessonsModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Lessons db = new Lessons();
            int id = data.idLesson;
            int idcon = data.idContent;
            int idlevel = data.idLevel;
            string active = data.active;
            int order = data.order;
            CheckModel check = new CheckModel();
            check = validCheck(idcon, idlevel, active, order);
            if (check.status)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = check.error });
            }
            try
            {
                int status = db.Update(id, idcon, idlevel, active, order);

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
            Lessons db = new Lessons();
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
