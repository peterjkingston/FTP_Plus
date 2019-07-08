using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FTP_Plus
{
    public class FileSystemMap
    {
        public enum FileBuildType{Local, FTP, HTTP}
        private Dictionary<string, FileSystemNode> _internalDict;

        public FileSystemMap(string directory, FileBuildType buildType, FTPConnection connection = null)
        {
            _internalDict = new Dictionary<string, FileSystemNode>();

            switch (buildType)
            {
                case FileBuildType.Local:
                    BuildFromLocal(directory);
                    break;
                case FileBuildType.FTP:
                    BuildFromFTP(directory, connection);
                    break;
                case FileBuildType.HTTP:
                    throw new NotImplementedException();
                    break;
                default:
                    break;
            }
        }

        private void BuildFromLocal(string directory)
        {
            string[] folders = Directory.GetDirectories(directory);
            string[] files = Directory.GetFiles(directory);

            foreach (string folder in folders)
            {
                FolderNode folderNode = new FolderNode(Directory.GetLastWriteTime(folder),
                                                       Path.GetFileName(folder),
                                                       true,
                                                       Path.GetDirectoryName(folder));
                _internalDict.Add(folderNode.Name, folderNode);
            }

            foreach (string file in files)
            {
                FileSystemNode fileNode = new FileSystemNode(Directory.GetLastWriteTime(file),
                                                               Path.GetFileName(file),
                                                               false,
                                                               Path.GetDirectoryName(file));
                _internalDict.Add(fileNode.Name, fileNode);
            }
        }

        public async void BuildFromFTP(string directory, FTPConnection connection)
        {
            try
            {
                string[] response = await connection.GetDirectoryContentsAsync(directory);

                for (int i = 0; i < response.Length; i++)
                {
                    FileSystemNode node = new FileSystemNode(response[i], directory, new FTPDetailParser());
                    if (node.IsDirectory)
                    {
                        this._internalDict.Add(node.Name, new FolderNode(node, FileBuildType.FTP, connection));
                    }
                    else
                    {
                        this._internalDict.Add(node.Name, node);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }
    }
}
