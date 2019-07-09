using System;
using System.Collections.Generic;
using System.Text;

namespace FTP_Plus
{
    public class SyncPlan
    {
        private List<SyncInstruction> _instructions {get; set;}
        private List<FileSystemNode> _syncedNodes { get; set; }

        public SyncPlan()
        {
            _instructions = new List<SyncInstruction>();
        }

        public void AddInstruction(SyncInstruction instruction, FileSystemNode node)
        {
            _instructions.Add(instruction);
            _syncedNodes.Add(node);
        }

        public void Execute()
        {
            for(int i = 0; i < _instructions.Count; i++)
            {
                _instructions[i](_syncedNodes[i]);
            }
        }
    }
}
