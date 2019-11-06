using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Extension_Methods
{
    public static class UrlMethods
    {
        public static bool Execute(this Uri uri, out string response)
        {
            try
            {
                var client = new WebClient();
                response = client.DownloadString(uri);
                return true;
            }
            catch (Exception e)
            {
                response = e.ToString();
                return false;
            }
        }

        private static string RegexUrl(string url)
        {
            return "\\\"([^\"]*)\\\"";
        }

        public static List<string> GetWebDirectory(this Uri uri)
        {
            var files = new List<string>();
            var request = (HttpWebRequest)WebRequest.Create(uri);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var html = reader.ReadToEnd();

                    var regex = new Regex(RegexUrl(uri.AbsoluteUri));
                    var matches = regex.Matches(html);
                    if (matches.Count > 0)
                    {
                        files.AddRange(matches.Cast<Match>().Where(match => match.Success).Select(match => match.ToString()));
                    }
                }
            }
            return files;
        }

        public static string GetPageTitle(this Uri uri)
        {
            var title = "";
            try
            {
                if (!(WebRequest.Create(uri) is HttpWebRequest request)) return title;

                if (!(request.GetResponse() is HttpWebResponse response)) return title;
                using (var stream = response.GetResponseStream())
                {
                    // compiled regex to check for <title></title> block
                    var titleCheck = new Regex(@"<title>\s*(.+?)\s*</title>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    const int bytesToRead = 8092;
                    var buffer = new byte[bytesToRead];
                    var contents = "";
                    int length;
                    while (stream != null && (length = stream.Read(buffer, 0, bytesToRead)) > 0)
                    {
                        // convert the byte-array to a string and add it to the rest of the
                        // contents that have been downloaded so far
                        contents += Encoding.UTF8.GetString(buffer, 0, length);

                        var m = titleCheck.Match(contents);
                        if (m.Success)
                        {
                            // we found a <title></title> match =]
                            title = m.Groups[1].Value;
                            break;
                        }

                        if (contents.Contains("</head>"))
                        {
                            // reached end of head-block; no title found =[
                            break;
                        }
                    }
                }
                return title;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error";
            }
        }

        public static List<string> GetUrlSourceAsList(this Uri uri)
        {
            const string temp = "check_file.txt";
            var c = File.CreateText(temp);

            c.Close();
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(uri, temp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            return File.ReadAllLines(temp).ToList();
        }

        public static string GetUrlSource(this Uri uri)
        {
            using (var client = new WebClient())
            {
                var f = client.DownloadString(uri);
                return f;
            }
        }
    }
}
