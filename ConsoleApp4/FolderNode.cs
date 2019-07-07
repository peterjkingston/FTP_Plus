using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FTP_Plus
{
    public class FolderNode : FileSystemNode
    {
        public FileSystemMap SubdirectoyMap { get; internal set; }

        public FolderNode()
        {

        }

        /// <summary>
        /// Constructor for the top FTP server directory. Use this one.
        /// </summary>
        /// <param name="connection"></param>
        public FolderNode(FTPConnection connection)
        {
            this.Modified = default;
            this.Name = "httpdocs";
            this.IsDirectory = true;
            this.RelativeParentDirectory = "/";
            SubdirectoyMap = new FileSystemMap("/", FileSystemMap.FileBuildType.FTP, connection);
        }

        public FolderNode(string lineFTPListDetail, string ftpListDirectory, FTPDetailParser parser, FTPConnection connection)
        {
            this.Modified = parser.ParseModifiedDate(lineFTPListDetail);
            this.Name = parser.ParseFileName(lineFTPListDetail);
            this.IsDirectory = parser.ParseDIRMarker(lineFTPListDetail);
            this.RelativeParentDirectory = ftpListDirectory;
            SubdirectoyMap = new FileSystemMap(Path.Combine(ftpListDirectory,Name),FileSystemMap.FileBuildType.FTP, connection);
        }

        public FolderNode(DateTime modified, string name, bool isDirectory, string parentDirectory)
        {
            this.Modified = modified;
            this.Name = name;
            this.IsDirectory = isDirectory;
            this.RelativeParentDirectory = parentDirectory;
            SubdirectoyMap = new FileSystemMap(Path.Combine(parentDirectory, Name), FileSystemMap.FileBuildType.Local);
        }
    }
}
