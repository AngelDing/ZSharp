using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ZSharp.Mvc.Demo.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public KeyValuePair<bool, string> UploadFile()
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];

                    if (httpPostedFile != null)
                    {
                        // Validate the uploaded image(optional)

                        // Get the complete file path
                        var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                        // Save the uploaded file to "UploadedFiles" folder
                        httpPostedFile.SaveAs(fileSavePath);

                        return new KeyValuePair<bool, string>(true, "File uploaded successfully.");
                    }

                    return new KeyValuePair<bool, string>(true, "Could not get the uploaded file.");
                }

                return new KeyValuePair<bool, string>(true, "No file found to upload.");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
            }
        }
    }
}