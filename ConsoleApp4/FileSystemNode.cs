using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FTP_Plus
{
    public class FileSystemNode
    {
        public DateTime Modified { get; internal set; }
        public string Name { get; internal set; }
        public bool IsDirectory { get; internal set; }
        public string RelativeParentDirectory { get; internal set; }
        public uint Size { get; internal set; }

        public FileSystemNode(){}

        public FileSystemNode(string lineFTPListDetail, string ftpListDirectory, FTPDetailParser parser)
        {
            Modified = parser.ParseModifiedDate(lineFTPListDetail);
            Name = parser.ParseFileName(lineFTPListDetail);
            IsDirectory = parser.ParseDIRMarker(lineFTPListDetail);
            RelativeParentDirectory = ftpListDirectory;
            if (!IsDirectory)
            {
                Size = parser.ParseSize(lineFTPListDetail);
            }
            else
            {
                Size = 0;
            }
            
        }

        public FileSystemNode(DateTime modified, string name, bool isDirectory, string parentDirectory, uint size)
        {
            this.Modified = modified;
            this.Name = name;
            this.IsDirectory = isDirectory;
            this.RelativeParentDirectory = parentDirectory;
            this.Size = size;
        }

        public string GetInternalDirectory()
        {
            return Path.Combine(new string[]{RelativeParentDirectory, Name});
        }
        
    }
}
