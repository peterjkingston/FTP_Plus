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
            FtpWebRequest request = GetFTPRequest(relativePath, WebRequestMethods.Ftp.ListDirectoryDetails);

            using (var response = await request.GetResponseAsync())
            {
                TextReader reader = new StreamReader(response.GetResponseStream());
                string message = reader.ReadToEnd();

                reader.Close();
                response.Close();
                
                return message; 
            }
        }

        private FtpWebRequest GetFTPRequest(string relativePath, string ftpMethod)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{Domain}{relativePath}");
            request.Credentials = _credential;
            request.Method = ftpMethod;

            return request;
        }

        public async Task<string[]> GetDirectoryContentsAsync(string relativePath)
        {
            FtpWebRequest request = GetFTPRequest(relativePath, WebRequestMethods.Ftp.ListDirectoryDetails);

            using (var response = await request.GetResponseAsync())
            {
                Stream responseStream = response.GetResponseStream();
                TextReader reader = new StreamReader(responseStream);
                List<String> lines = new List<String>();
               
                bool escape = false;

                while (!escape)
                {
                    String line = new String(reader.ReadLine());
                    escape = line == null;

                    if (!escape)
                    {
                        lines.Add(line);
                    }
                }

                reader.Close();
                return lines.ToArray();
            }
        }
    }    
}
