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

        public FolderNode(FileSystemNode node, FileSystemMap.FileBuildType buildType, FTPConnection connection = null)
        {
            this.Name = node.Name;
            this.Modified = node.Modified;
            this.IsDirectory = true;
            this.RelativeParentDirectory = node.RelativeParentDirectory;
            this.Size = node.Size;
            SubdirectoyMap = new FileSystemMap(Path.Combine(node.RelativeParentDirectory,Name), buildType, connection);
        }

        public FolderNode(string lineFTPListDetail, string ftpListDirectory, FTPDetailParser parser, FTPConnection connection)
        {
            this.Modified = parser.ParseModifiedDate(lineFTPListDetail);
            this.Name = parser.ParseFileName(lineFTPListDetail);
            this.IsDirectory = true;
            this.RelativeParentDirectory = ftpListDirectory;
            this.Size = 0;
            SubdirectoyMap = new FileSystemMap(Path.Combine(ftpListDirectory,Name),FileSystemMap.FileBuildType.FTP, connection);
        }

        public FolderNode(DateTime modified, string name, bool isDirectory, string parentDirectory)
        {
            this.Modified = modified;
            this.Name = name;
            this.IsDirectory = true;
            this.RelativeParentDirectory = parentDirectory;
            this.Size = 0;
            SubdirectoyMap = new FileSystemMap(Path.Combine(parentDirectory, Name), FileSystemMap.FileBuildType.Local);
        }

        public FolderNode(string localDirectory)
        {
            this.Modified = Directory.GetLastWriteTime(localDirectory);
            this.Name = Path.GetFileName(localDirectory);
            this.IsDirectory = true;
            this.RelativeParentDirectory = Path.GetDirectoryName(localDirectory);
            this.Size = 0;
            SubdirectoyMap = new FileSystemMap(Path.Combine(RelativeParentDirectory, Name), FileSystemMap.FileBuildType.Local, null);
        }
    }
}
