using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Web;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace webserver.Controllers
{
    public class AuthController : ApiController
    {
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static string GenerateToken(string useremail, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, useremail)
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public static bool DecodeToken(string tokenString)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(tokenString);
                string checkToken = Convert.ToString(token.Payload.First().Value);
                if (checkToken != null && checkToken != "")
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }


        }
        public bool authCheckToken()
        {
            // string userToken = Request.Form["_usertoken"];
            if (basicAuthCheck()) //first check basicAuthtoken check..
            {
                var re = Request;
                var headers = re.Headers;

                if (headers.Contains("_usertoken"))
                {
                    string userToken = headers.GetValues("_usertoken").First();
                    if (userToken != null && userToken != "")
                    {
                        return DecodeToken(userToken);
                    }
                    else
                    {
                        return false; //invalid user..
                    }

                }
                else
                    return false;
            }
            else
            {
                return false;
            }
            

        }
        public bool basicAuthCheck()
        {
            string auth = ConfigurationManager.AppSettings["_basicAuth"];
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("_basicAuth"))
            {
                string userToken = headers.GetValues("_basicAuth").First();
                if (userToken != null && userToken != "")
                {
                    if (userToken == auth)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false; //invalid user..
                }

            }
            else
                return false;
        }
        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

            public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
            {
                string filename = "";
                string extension = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));
                Random random = new Random();
                int num = random.Next(100000, 999999);
                filename = num + extension;
               return filename;
              //  return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }
        public  static string[] mediaExtensions = {
            ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
            ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
            ".AVI", ".MP4", ".DIVX", ".WMV", //etc
        };
        public static string[] imageExtension = {
            ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF" //etc
                         };
        public static string[] audioExtentsion = { ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", };
        public static bool IsMediaFile(string ext)
        {
            return -1 != Array.IndexOf(audioExtentsion, ext.ToUpper());
        }
        public static bool IsImageFile(string ext)
        {
            return -1 != Array.IndexOf(imageExtension, ext.ToUpper());
        }

        public static string SetBase64Image(string base64, int width=100, int height=100, string route = "uploadpath")
        {
            string extension = "";
            System.Drawing.Imaging.ImageFormat format;
            if (base64.Contains("image/png"))
            {
                extension = ".png";
                format = System.Drawing.Imaging.ImageFormat.Png;
            }
            else
            {
                extension = ".jpg";
                format = System.Drawing.Imaging.ImageFormat.Png;
            }

            string imageName = Guid.NewGuid().ToString() + extension;

            Char delimiter = ',';
            byte[] imageBytes = System.Convert.FromBase64String(base64.Split(delimiter)[1]);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length);

           // byte[] photo = C_generalFunctions.ScaleByPercent(ms, width, height, format);
            string folderPath = HttpContext.Current.Server.MapPath("~/" + ConfigurationManager.AppSettings[route].ToString());
            if (!Directory.Exists(folderPath))
            {
                //If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(folderPath);
            }
            var fileSavePath = Path.Combine(folderPath, imageName);

           // MemoryStream msSave = new MemoryStream(photo);
            System.Drawing.Image i = System.Drawing.Image.FromStream(ms);
            i.Save(fileSavePath);

            return imageName;
        }
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public byte[] ResizeToBytes(string filename)
        {

            Bitmap img = new Bitmap(filename);
            double oldwidth = img.Width;
            double oldheight = img.Height;
            double newheight;
            double newwidth;
            if (oldwidth > oldheight)
            {
                newwidth = 765;
                newheight = 765 * (oldheight / oldwidth);
            }
            else
            {
                newheight = 765;
                newwidth = 765 * (oldwidth / oldheight);
            }
            Bitmap imgout = new Bitmap(img, (int)newwidth, (int)newheight);
            img.Dispose();
            imgout.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
            imgout.Dispose();

            FileStream f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            int size = (int)f.Length;
            byte[] MyData = new byte[f.Length + 1];
            f.Read(MyData, 0, size);
            f.Close();
            return MyData;
        }


    }
}
