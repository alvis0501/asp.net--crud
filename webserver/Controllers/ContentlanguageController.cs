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
using System.Configuration;
using System.IO;


namespace webserver.Controllers
{
    public class ContentlanguageController : AuthController
    {
        public CheckModel validCheck(string idcon, string idlang, string op1, string op2, string op3, string op4, string op5)
        {
            CheckModel result = new CheckModel();
            string errors = "";
            double Num;
            bool isNum = double.TryParse(idcon, out Num);
            bool isNum1 = double.TryParse(idlang, out Num);
            bool isNum2 = double.TryParse(op1, out Num);
            bool isNum3 = double.TryParse(op2, out Num);
            bool isNum4 = double.TryParse(op3, out Num);
            bool isNum5 = double.TryParse(op4, out Num);
            bool isNum6 = double.TryParse(op5, out Num);
            if (!isNum || idcon == "0")
            {
                errors = "idContent should be valid .";
                result.status = true;
            }

            if (!isNum1 || idlang == "0")
            {
                errors += "idLevel should be valid .";
                result.status = true;
            }
            if (!isNum2)
            {
                errors += "Option1  should be numeric .";
                result.status = true;
            }
            if (!isNum3)
            {
                errors += "Option2  should be numeric .";
                result.status = true;
            }
            if (!isNum4)
            {
                errors += "Option3  should be numeric .";
                result.status = true;
            }
            if (!isNum5)
            {
                errors += "Option4  should be numeric .";
                result.status = true;
            }
            if (!isNum6)
            {
                errors += "Option5  should be numeric .";
                result.status = true;
            }

            result.error = errors;
            return result;
        }
        [HttpPost]
        public HttpResponseMessage Get([FromBody]Content_languageModel jsondata)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            int idcon = 0;
            int idlang = 0;
            try
            {
                idcon = jsondata.idContent;
                idlang = jsondata.idLanguage;
            }
            catch
            {
                idcon = 0;
                idlang = 0;
            }
            Content_language db = new Content_language();
            Content_languageModel model = new Content_languageModel();
            List<Content_languageModel> data = new List<Content_languageModel>();
            try
            {
                data = db.get(idcon,idlang);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = 1, data = data });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Insert()
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Content_language db = new Content_language();
            string idcon = HttpContext.Current.Request.Form.Get("idContent");
            string idlang = HttpContext.Current.Request.Form.Get("idLanguage");
            string desp1 = HttpContext.Current.Request.Form.Get("desp1");
            string desp2 = HttpContext.Current.Request.Form.Get("desp2");
            string desp3 = HttpContext.Current.Request.Form.Get("desp3");
            string desp4 = HttpContext.Current.Request.Form.Get("desp4");
            string desp5 = HttpContext.Current.Request.Form.Get("desp5");
            string op1 = HttpContext.Current.Request.Form.Get("op1");
            string op2 = HttpContext.Current.Request.Form.Get("op2");
            string op3 = HttpContext.Current.Request.Form.Get("op3");
            string op4 = HttpContext.Current.Request.Form.Get("op4");
            string op5 = HttpContext.Current.Request.Form.Get("op5");
            CheckModel checkvalue = new CheckModel();
            checkvalue = validCheck(idcon, idlang, op1, op2, op3, op4, op5);
            if (checkvalue.status)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = 0 ,error=checkvalue.error});
            }
            string audio = "";
            string image = "";
            string audioext = "";
            string imageext = "";
            string folderPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["uploadpath"].ToString());
            if (!Directory.Exists(folderPath))
            {
                //If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(folderPath);
            }
            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(folderPath);
            List<string> files = new List<string>();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                  
                    if (file.Headers.ContentDisposition.Name.Contains("imagefile"))
                    {
                        image = Path.GetFileName(file.LocalFileName);
                        imageext = Path.GetExtension(file.LocalFileName);
                    }
                    else if (file.Headers.ContentDisposition.Name.Contains("audiofile"))
                    {
                        audio = Path.GetFileName(file.LocalFileName);
                        audioext = Path.GetExtension(file.LocalFileName);
                    }
                }
               
            }
            catch (System.Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
            if (image == "" || audio == "" || !IsMediaFile(audioext) || !IsImageFile(imageext))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "Image or audio file should be valid ." });
            }
            try
            {
                int result = db.Insert(idcon, idlang, desp1, desp2, desp3, desp4, desp5, op1, op2, op3, op4, op5, image, audio);

                    return Request.CreateResponse(HttpStatusCode.OK, new { result = result });

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }

        }

        public async Task<HttpResponseMessage> Update()
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            Content_language db = new Content_language();
            string idcon_old = HttpContext.Current.Request.Form.Get("idCon_old");
            string idlang_old = HttpContext.Current.Request.Form.Get("idLang_old");
            string idcon = HttpContext.Current.Request.Form.Get("idContent");
            string idlang = HttpContext.Current.Request.Form.Get("idLanguage");
            string desp1 = HttpContext.Current.Request.Form.Get("desp1");
            string desp2 = HttpContext.Current.Request.Form.Get("desp2");
            string desp3 = HttpContext.Current.Request.Form.Get("desp3");
            string desp4 = HttpContext.Current.Request.Form.Get("desp4");
            string desp5 = HttpContext.Current.Request.Form.Get("desp5");
            string op1 = HttpContext.Current.Request.Form.Get("op1");
            string op2 = HttpContext.Current.Request.Form.Get("op2");
            string op3 = HttpContext.Current.Request.Form.Get("op3");
            string op4 = HttpContext.Current.Request.Form.Get("op4");
            string op5 = HttpContext.Current.Request.Form.Get("op5");
            CheckModel checkvalue = new CheckModel();
            checkvalue = validCheck(idcon, idlang, op1, op2, op3, op4, op5);
            string audio = "";
            string image = "";
            string audioext = "";
            string imageext = "";
            string folderPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings["uploadpath"].ToString());
            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(folderPath);
            List<string> files = new List<string>();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    
                    if (file.Headers.ContentDisposition.Name.Contains("imagefile"))
                    {
                        image = Path.GetFileName(file.LocalFileName);
                        imageext = Path.GetExtension(file.LocalFileName);
                    }
                    else if (file.Headers.ContentDisposition.Name.Contains("audiofile"))
                    {
                        audio = Path.GetFileName(file.LocalFileName);
                        audioext = Path.GetExtension(file.LocalFileName);
                    }
                }

            }
            catch (System.Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
            if (checkvalue.status)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = 0, error = checkvalue.error });
            }
            try
            {
                int status = db.Update(idcon_old, idlang_old, idcon, idlang, desp1, desp2, desp3, desp4, desp5, op1, op2, op3, op4, op5, image, audio);

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
        public HttpResponseMessage Delete([FromBody] Content_languageModel data)
        {
            bool checktoken = authCheckToken();
            if (!checktoken)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "no valid User" });
            }
            int idcon = data.idContent;
            int idlang = data.idLanguage;
            if ( idcon < 1 || idlang < 1)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = "idLanguage or idContent should be valid." });
            }
            Content_language db = new Content_language();
            try
            {
                int status = db.Delete(idcon,idlang);
                if (status > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 1 });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = 0,error="Can't find data ." });
                }
               

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { result = 0, error = e.Message });
            }
        }
    }
}
