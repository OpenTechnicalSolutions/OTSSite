using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OTSSiteMVC.Configurations;
using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OTSSiteMVC.Repositories
{
    public class SiteFileRepository
    {
        private readonly IOptions<FileWriteOptions> _fileWriteOptions;
        private readonly IHostingEnvironment _environment;

        public SiteFileRepository(
            IOptions<FileWriteOptions> options,
            IHostingEnvironment environment)
        {
            _fileWriteOptions = options;
            _environment = environment;
        }
        /// <summary>
        /// Return an article
        /// </summary>
        /// <param name="path">path to file</param>
        /// <returns>string array of pages.</returns>
        public async Task<string> GetArticle(string path)
        {
            if (!File.Exists(path))
                return null;

            //var stringBuilder = new StringBuilder();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    var data = await sr.ReadToEndAsync();
                    return data;
                }
            }
        }
        /// <summary>
        /// Writes an article file to the server.
        /// </summary>
        /// <param name="formFile">article IFormFile</param>
        /// <param name="username">article author username</param>
        /// <returns>path to file</returns>
        public async Task<string> SaveArticle(IFormFile articleFormFile, string username)
        {
            string basePath;                                                //Path to save to
            string fullPath;                                                //Path with file name
            var articlePath = _fileWriteOptions.Value.ArticlePath;          //Root path from appsettings

            //Check to see if the path ends in a '/'
            //Sets the path accordingly
            if (articlePath[articlePath.Length - 1] == '/')                 //Check to see if the path ends in a '/'
                basePath = articlePath + $"{username}/{Guid.NewGuid()}/";       //Set path
            else
                basePath = articlePath + $"/{username}/{Guid.NewGuid()}/";      //Set path
            
            Directory.CreateDirectory(basePath);                            //Create article directory

            var extension =                                                 //Get extension
                Path.GetExtension(articleFormFile.FileName).ToLower();  
            if (!(extension == ".txt" || extension == ".md"))               //If extension isn't the one expected
                return null;                                                //Return null
            fullPath = basePath + articleFormFile.FileName;                 //Create the full path

            //Save the article File
            using (FileStream fs = 
                new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await articleFormFile.CopyToAsync(fs);
            }
            return fullPath;                                                 //Return path with filename.
        }

        public bool SaveImage(string userName, IFormFile image)
        {
            if (!ConfirmImage(image.ContentType, image.FileName.ToLower()))           //Confirms content type
                return false;

            var imageRoot = _fileWriteOptions.Value.ImageRoot;              //Gets image root storage location from appsettings.json
            var imageSavePath = Path.Combine(imageRoot, userName);          //Create a full file path
            var imageFullPath = Path.Combine(imageSavePath, image.FileName);
            if (!Directory.Exists(imageSavePath))
                Directory.CreateDirectory(imageSavePath);                                                                            //Write the file to path
            using (FileStream fs = new FileStream(imageFullPath, FileMode.Create, FileAccess.Write))          
                image.CopyToAsync(fs);
            return true;
        }

        public FileStream GetImage(string username, string filename)
        {
            var path = Path.Combine(Path.Combine(_fileWriteOptions.Value.ImageRoot, username), filename);
            return File.OpenRead(path);
        }

        private bool ConfirmImage(string contentType, string fileName)
        {
            //Supported ContentTypes
            var contentTypes = new[]
            {
                "image/jpg",
                "image/jpeg",
                "image/gif",
                "image/png",
                "image/x-png",
                "image/pjpeg"
            };
            //Supported ExtensionTypes
            var extensionTypes = new[]
            {
                ".jpg",
                ".jpeg",
                ".gif",
                ".png"
            };

            return contentTypes.Contains(contentType) && extensionTypes.Contains(Path.GetExtension(fileName));
        }
    }
}
