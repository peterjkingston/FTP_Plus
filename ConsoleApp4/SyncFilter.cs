using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FTP_Plus
{
    public class SyncFilter
    {
        private SyncInstruction _GET { get; set; }
        private SyncInstruction _POST { get; set; }
        private string _currentKey { get; set; }

        public SyncFilter(SyncInstruction GET, SyncInstruction POST)
        {
            _GET = GET;
            _POST = POST;
        }

        public SyncPlan CompareContents(FolderNode localNode, FolderNode remoteNode)
        {
            SyncPlan plan = new SyncPlan();
            Dictionary<string, FileSystemNode> localMap = new Dictionary<string, FileSystemNode>();
            Dictionary<string, FileSystemNode> remoteMap = new Dictionary<string, FileSystemNode>();
            List<string> keys = new List<string>();

            DigDeeper(localMap, localNode, keys);
            DigDeeper(remoteMap, remoteNode, keys);

            foreach (string key in keys)
            {
                FileSystemNode local = localMap.ContainsKey(key) ? localMap[key] : null;
                FileSystemNode remote = remoteMap.ContainsKey(key) ? remoteMap[key] : null;

                if(local != null && remote != null)
                {
                    //If a both local and remote copies exist.
                    //TODO: Compare times, add newer node and upload/download instruction to plan.
                    if (local.IsNewerThan(remote))
                    {
                        //Upload the local node because it is newer.
                        plan.AddInstruction(_POST, local);
                    }
                    else
                    {
                        //Download the remote node because it is newer.
                        plan.AddInstruction(_GET, remote);
                    }
                }
                else if(local != null)
                {
                    //If a local copy exists and a remote does not.
                    //TODO: Add local node and upload instruction to plan.
                    plan.AddInstruction(_POST, local);
                }
                else if(remote != null)
                {
                    //If a remote copy exists and a local does not.
                    //TODO: Add remote node and download instruction to plan.
                    plan.AddInstruction(_GET, remote);
                }
            }

            return plan;
        }

        private void DigDeeper(Dictionary<string, FileSystemNode> dict, FolderNode folder, List<string> keys)
        {
            foreach (FileSystemNode node in folder.SubdirectoyMap.GetContents())
            {
                string nodePath = Path.Combine(node.RelativeParentDirectory, node.Name);

                dict.Add(Path.Combine(node.RelativeParentDirectory, node.Name), node);
                if (node.IsDirectory)
                {
                    DigDeeper(dict, node as FolderNode, keys);
                }

                _currentKey = nodePath;
                if (!keys.Exists(CheckUnique))
                {
                    keys.Add(nodePath);
                }
            }
        }

        private bool CheckUnique(string key)
        {
            return _currentKey == key;
        }
    }
}
