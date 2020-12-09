using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace wzhoo.Apps
{
    public class FileUploadController : Controller
    {


        private static IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public FileUploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// only file uploading  （ajax，Form表单都适用） update time: 2020-10-17
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public JsonResult SingleFileUpload()
        {
            var formFile = Request.Form.Files[0];                //获取请求发送过来的文件
            var currentDate = DateTime.Now;
            var webRootPath = _hostingEnvironment.WebRootPath;   //>>>相当于HttpContext.Current.Server.MapPath("") 

            try
            {
                var filePath = $"/uploadfile/{currentDate:yyyyMMdd}/";
                //创建每日存储文件夹
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (formFile != null)
                {
                    //文件后缀
                    var fileExtension = Path.GetExtension(formFile.FileName);  //获取文件格式，拓展名
                    //判断文件大小
                    var fileSize = formFile.Length;
                    if (fileSize > 1024 * 1024 * 10) //10M TODO:(1mb=1024X1024b)
                    {
                        return new JsonResult(new { isSuccess = "false", resultMsg = "上传的文件不能大于10M" });
                    }
                    //保存的文件名称(以名称和保存时间命名)
                    var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;
                    //文件保存
                    using (var fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    //完整的文件路径
                    var completeFilePath = Path.Combine(filePath, saveName);
                    return new JsonResult(new { isSuccess = "true", returnMsg = "Upload success", completeFilePath = completeFilePath });
                }
                else
                {
                    return new JsonResult(new { isSuccess = "false", resultMsg = "Upload error" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { isSuccess = "false", resultMsg = "File save error，information：" + ex.Message });
            }
        }



    }
}