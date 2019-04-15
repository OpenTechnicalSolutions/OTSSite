﻿using Microsoft.AspNetCore.Http;
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
            var path = string.Format($"./{username}/{new Guid()}");
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await articleFormFile.CopyToAsync(fs);
            }
            if (imageFormFiles == null)
                return path;
            using (FileStream fs = new FileStream(path + "/images", FileMode.Create, FileAccess.Write))
            {
                foreach (var formFile in imageFormFiles)
                    await formFile.CopyToAsync(fs);
            }
            path += articleFormFile.FileName;
            return path;
        }
    }
}
