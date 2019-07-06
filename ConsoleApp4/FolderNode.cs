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

        public FolderNode(string lineFTPListDetail, string ftpListDirectory, FTPDetailParser parser)
        {
            this.Modified = parser.ParseModifiedDate(lineFTPListDetail);
            this.Name = parser.ParseFileName(lineFTPListDetail);
            this.IsDirectory = parser.ParseDIRMarker(lineFTPListDetail);
            this.RelativeParentDirectory = ftpListDirectory;
            SubdirectoyMap = new FileSystemMap(Path.Combine(ftpListDirectory,Name),FileSystemMap.FileBuildType.FTP);
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
