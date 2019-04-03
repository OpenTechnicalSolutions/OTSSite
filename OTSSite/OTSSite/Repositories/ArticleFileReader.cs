using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSite.Repositories
{
    public class ArticleFileReader
    {
        private const int PREVIEW_LINE_LENGTH = 20;
        private const int PAGE_LINE_LENGTH = 100;
        /// <summary>
        /// Returns preview text for the article
        /// </summary>
        /// <param name="path">path to article file</param>
        /// <returns>string data</returns>
        public string GetPreview(string path)
        {
            if (!File.Exists(path))
                return null;

            var stringBuilder = new StringBuilder();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    for (var i = 0; i < PREVIEW_LINE_LENGTH; i++)
                    {
                        var line = sr.ReadLine();
                        if (line == null)
                            break;
                        stringBuilder.Append(line);
                    }
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Return an article
        /// </summary>
        /// <param name="path">path to file</param>
        /// <returns>string array of pages.</returns>
        public string[] GetArticle(string path)
        {
            if (!File.Exists(path))
                return null;

            var pages = new List<string>();
            var stringBuilder = new StringBuilder();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {                   
                    string line;
                    while((line = sr.ReadLine()) != null)
                    {
                        for(int i = 0;i < PAGE_LINE_LENGTH;i++)
                            stringBuilder.Append(line);
                        pages.Add(stringBuilder.ToString());
                        stringBuilder = new StringBuilder();
                    }
                }
            }
            return pages.ToArray();
        }
    }
}
