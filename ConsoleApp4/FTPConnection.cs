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
        public event EventHandler<string> FailedLogon;

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
            string[] result = new string[]{""};
            try
            {
                FtpWebRequest request = GetFTPRequest(relativePath, WebRequestMethods.Ftp.ListDirectoryDetails);
                using (var response = await request.GetResponseAsync())
                {

                    TextReader reader = new StreamReader(response.GetResponseStream());
                    List<string> lines = new List<string>();

                    bool escape = false;
                    string line;

                    while (!escape)
                    {
                        line = reader.ReadLine();
                        escape = line == null;

                        if (!escape)
                        {
                            lines.Add(line);
                        }
                    }
                    result = lines.ToArray();
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                OnFailedLogon(ex.Message);
            }

            return result;
        }

        protected virtual void OnFailedLogon(string message)
        {
            FailedLogon?.Invoke(this, message);
        }
    }    
}
