using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Plus
{
    public class FTPConnection
    {
        public string Domain { get; private set; }
        private NetworkCredential _credential;
        public FTPConnection(string domain, string username, SecureString password)
        {
            this.Domain = domain;
            _credential = new NetworkCredential(username, password);
        }

        public async Task<string> GetDirectoryAsync(string relativePath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{Domain}{relativePath}");
            request.Credentials = _credential;
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            using(var response = await request.GetResponseAsync())
            {
                TextReader reader = new StreamReader(response.GetResponseStream());
                string message = reader.ReadToEnd();

                reader.Close();
                response.Close();
                
                return message; 
            }
        }

    }    
}
