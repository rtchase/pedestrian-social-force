using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social_Forces_Main
{
    public class PedNetworkData
    {
        private ushort _numPedNodes;

        public ushort NumPedNodes
        {
            get { return _numPedNodes; }
            set { _numPedNodes = value; }
        }

        private ushort _numPedLinks;

        public ushort NumPedLinks
        {
            get { return _numPedLinks; }
            set { _numPedLinks = value; }
        }

        private uint _totPedEntered;

        public uint TotPedEntered
        {
            get { return _totPedEntered; }
            set { _totPedEntered = value; }
        }

        private List<ushort> _pedNodeIdList = new List<ushort>();

        public List<ushort> PedNodeIdList
        {
            get { return _pedNodeIdList; }
            set { _pedNodeIdList = value; }
        }

        private List<ushort> _pedLinkIdList = new List<ushort>();

        public List<ushort> PedLinkIdList
        {
            get { return _pedLinkIdList; }
            set { _pedLinkIdList = value; }
        }

        public PedNetworkData(ushort s)
        {
            _numPedNodes = 0;
            _numPedLinks = 0;
            _totPedEntered = 0;
            
        }
    }
}
