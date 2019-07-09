using System;
using System.Collections.Generic;
using System.Text;

namespace FTP_Plus
{
    public class FTPDetailParser
    {
        public DateTime ParseModifiedDate(string listDetail)
        {
            
            string strDate = listDetail.Substring(1, 16);
            DateTime dt = DateTime.Parse(strDate);

            return dt;
        }

        public string ParseFileName(string listDetail)
        {
            string rest = listDetail.Substring(17, listDetail.Length - 17).Trim();
            
            string[] parts = rest.Split(' ', 2);
            rest = parts[1].Trim();

            return rest; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listDetail">FTP ListDetail provided by server.</param>
        /// <returns>Returns true if file is marked as a directory.</returns>
        public bool ParseDIRMarker(string listDetail)
        {
            return listDetail.IndexOf("<DIR>") > 0;
        }

        public uint ParseSize(string listDetail)
        {
            string[] parts = listDetail.Substring(17, listDetail.Length - 17).Trim().Split(' ');

            return (uint)Int32.Parse(parts[0]);
        }
    }
}
