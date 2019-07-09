using System;
using System.Collections.Generic;
using System.Text;

namespace FTP_Plus
{
    public class SyncFilter
    {
        private SyncInstruction _GET { get; set; }
        private SyncInstruction _POST { get; set; }

        public SyncFilter(SyncInstruction GET, SyncInstruction POST)
        {
            _GET = GET;
            _POST = POST;
        }

        public SyncPlan CompareContents(FolderNode localNode, FolderNode remoteNode)
        {
            SyncPlan plan = new SyncPlan();

            localNode.SubdirectoyMap.
        }
    }
}
