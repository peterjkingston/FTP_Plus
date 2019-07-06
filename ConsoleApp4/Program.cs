using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching peterjkingston.com\n");

            Console.WriteLine(GetFTPDir("peterjkingston.com", "username", ""));

            Console.ReadKey(true);
        }

        private static string GetFTPDir(string domain, string username, string password)
        {
            WebRequest ftpRequest = WebRequest.Create($"ftp://{domain}/");
            ftpRequest.Credentials = new NetworkCredential(username, password);

            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            using(var response = (FtpWebResponse)ftpRequest.GetResponse())
            {
                string message = default;
                TextReader reader = new StreamReader(response.GetResponseStream());
                message = reader.ReadToEnd();
                response.Close();
                return message;
            }
        }
    }
}
