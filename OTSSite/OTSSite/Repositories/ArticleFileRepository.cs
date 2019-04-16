using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OTSSite.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class ArticleFileRepository
    {
        private readonly IOptions<FileWriteOptions> _fileWriteOptions;

        public ArticleFileRepository(IOptions<FileWriteOptions> options)
        {
            _fileWriteOptions = options;
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
        public async Task<string> SaveArticle(IFormFile articleFormFile, IEnumerable<IFormFile> imageFormFiles, string username)
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
            Directory.CreateDirectory(basePath + "images/");                //Create images directory

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

            if (imageFormFiles == null)                                     //If imageFormFiles is null
                return fullPath;                                            //Return path with filename.

            //Save images
            foreach (var formFile in imageFormFiles)
            {
                using (FileStream fs = 
                    new FileStream(basePath + "images/" + formFile.FileName, FileMode.Create, FileAccess.Write))
                {
                    await formFile.CopyToAsync(fs);
                }
            }

            return fullPath;                                                 //Return path with filename.
        }
    }
}
