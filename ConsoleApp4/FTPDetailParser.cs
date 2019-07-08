using System;
using System.Collections.Generic;
using System.Text;

namespace FTP_Plus
{
    public class FTPDetailParser
    {
        public DateTime ParseModifiedDate(string listDetail)
        {
            string strDate = listDetail.Substring(1, 8);
            DateTime dt = DateTime.Parse(strDate);
            return dt;
        }

        public string ParseFileName(string listDetail)
        {
            string rest = listDetail.Substring(9, listDetail.Length - 9);
            rest = rest.IndexOf("<DIR>") > 0 ? rest.Substring(rest.IndexOf("<DIR>") + 5, rest.Length - (rest.IndexOf("<DIR>") + 5)).Trim() :
                                               rest.Trim();
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
    }
}
