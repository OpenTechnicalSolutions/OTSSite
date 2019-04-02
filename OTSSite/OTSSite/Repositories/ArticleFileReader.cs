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


        public string GetPreview(string path)
        {
            if (!File.Exists(path))
                return null;

            var stringBuilder = new StringBuilder();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    for(var i = 0;i < PREVIEW_LINE_LENGTH; i++)
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
    }
}
