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
    public class Level_languageController : AuthController
    {
        [HttpPost]
        public HttpResponseMessage Get([FromBody]Level_LanguageModel jsondata)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            int idlevel = 0;
            int idlang = 0;
            try
            {
                idlevel = jsondata.idLevel;
                idlang = jsondata.idLanguage;
            }
            catch
            {
                idlevel = 0;
                idlang = 0;
            }
            Level_Language db = new Level_Language();
            Level_LanguageModel model = new Level_LanguageModel();
            List<Level_LanguageModel> data = new List<Level_LanguageModel>();
            try
            {
                data = db.get(idlevel, idlang);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, data = data });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }


        public HttpResponseMessage Insert([FromBody] Level_LanguageModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Level_Language db = new Level_Language();
            int idlevel = data.idLevel;
            int idlang = data.idLanguage;
            string name = data.Name;
            try
            {
                int result = db.Insert(idlevel, idlang,name);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }

        }

        public HttpResponseMessage Update([FromBody] Level_LanguageModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Level_Language db = new Level_Language();
            int idlevel_old = data.idlevel_old;
            int idlang_old = data.idlang_old;
            int idlevel = data.idLevel;
            int idlang = data.idLanguage;
            string name = data.Name;
            if (idlevel_old < 1 || idlang_old < 1)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "idLanguage or idLevel should be valid." });
            }
            try
            {
                int status = db.Update(idlevel_old, idlang_old, idlevel, idlang, name);

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
        [HttpPost]
        public HttpResponseMessage Delete([FromBody] Level_LanguageModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            int idlevel = data.idLevel;
            int idlang = data.idLanguage;
            if (idlevel < 1 || idlang < 1)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "idLanguage or idContent should be valid." });
            }
            Level_Language db = new Level_Language();
            try
            {
                int status = db.Delete(idlevel, idlang);
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
