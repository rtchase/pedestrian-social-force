using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social_Forces_Main
{

    public enum PedNodeType
    {
        Entry,
        Exit,
        LinkConnector,
    }

    public enum PedArrivalDistribution
    {
        NegativeExp,
        Uniform,
        Lognormal
    }

    public class PedNodeData
    {

        PedNodeType _type;
        UInt16 _id;
        UInt16 _listIndex;
        double _positionX;
        double _positionY;
        double _positionZ;
        double[] _point1 = new double[3];
        double[] _point2 = new double[3];



        public PedNodeData(UInt16 iD, PedNodeType nodeType, UInt16 listIndex, double positionX, double positionY, double[] point1, double[] point2)
        {
            _type = nodeType;
            _id = iD;
            _positionX = positionX;
            _positionY = positionY;
            _point1 = point1;
            _point2 = point2;

        }

        public PedNodeType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public UInt16 Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public UInt16 ListIndex
        {
            get { return _listIndex; }
            set { _listIndex = value; }
        }
        public double PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }
        public double PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }
        public double PositionZ
        {
            get { return _positionZ; }
            set { _positionZ = value; }
        }

        public double[] Point1
        {
            get { return _point1; }
            set { _point1 = value; }
        }

        public double[] Point2
        {
            get { return _point2; }
            set { _point2 = value; }
        }


    }


    public class PedEntryNode : PedNodeData
    {
        //an entry node can only connect to one other node; thus, there is only 1 link downstream of the entry node
        PedArrivalDistribution _arrivalDist;
        int _enteringFlowRatePedPerHour;
        double _enteringFlowRatePedPerSecond;

        double _avgArrivalHeadway;

        double _minEntryHeadway;
        double _nextPedEntryTime;
        uint _numPedEntered;
        double _averageDesiredSpeed;  //not used

        UInt16 _downstreamNodeId;
        //UInt16 _downstreamNodeListIndex;
        UInt16 _downstreamLinkId;

        int _unservedPedEntries;

        


        List<uint> _pedIdList;
        //List<VehicleData> _vehs;

        public double EntryHeadway(PedArrivalDistribution PedArrDist, double AvgHdwy, double MinHdwy)
        {
            if (PedArrDist == PedArrivalDistribution.NegativeExp)
            {
                double hdwy = 0;

                double rand = Convert.ToDouble(InputData.RandNum.Next(0, 10000)) / 10000;
                hdwy = Math.Round(((-1 * (AvgHdwy - MinHdwy) * Math.Log(1 - rand)) + MinHdwy), 1);

                return hdwy;
            }

            if (PedArrDist == PedArrivalDistribution.Uniform)
            {

                double maxhdwy = (2 * AvgHdwy) - MinHdwy;
                double rand = Convert.ToDouble(InputData.RandNum.Next(0, 10000)) / 10000;
                double hdwy = Math.Round((rand * (maxhdwy - MinHdwy) + MinHdwy), 1);

                return hdwy;

            }
            if (PedArrDist == PedArrivalDistribution.Lognormal)
            {
                return 0;
            }
            return 0;

        }

        public double[] EntryOffset(PedEntryNode Node)
        {
            double rand = Convert.ToDouble(InputData.RandNum.Next(0, 10000)) / 10000;
            //double AvoidanceDistance = 
            double dx = Node.Point2[0] - Node.Point1[0];
            double dy = Node.Point2[1] - Node.Point1[1] - 3.5;//2*radius
            double dz = Node.Point2[2] - Node.Point1[2];
            if (dx > 0)
            {
                dx = dx - 2;
                double[] offset = new double[3];
                offset[0] = Node.Point1[0] + rand * dx + 1.75;//Radius
                offset[1] = Node.Point1[1] + rand * dy;
                offset[2] = Node.Point1[2] + rand * dz;

                return offset;
            }
            else
            {
                //dy = dy - 2;
                double[] offset = new double[3];
                double rand2 = Convert.ToDouble(InputData.RandNum.Next(0, 10000)) / 10000;
                offset[0] = Node.Point1[0] + (rand2-0.5) * 6;
                offset[1] = Node.Point1[1] + rand * dy + 1.75;//Radius
                offset[2] = Node.Point1[2] + rand * dz;

                return offset;
            }

        }


        public PedEntryNode(UInt16 iD, PedNodeType nodeType, UInt16 listIndex, double positionX, double positionY, double[] point1, double[] point2, UInt16 downstreamLinkId, UInt16 downstreamNodeId, double minEntryHeadway, int enteringFlowRatePPH, PedArrivalDistribution ArrivalDist)
            : base(iD, nodeType, listIndex, positionX, positionY, point1, point2)
        {
            _numPedEntered = 0;
            _minEntryHeadway = minEntryHeadway;
            _arrivalDist = ArrivalDist;
            _enteringFlowRatePedPerHour = enteringFlowRatePPH;
            _enteringFlowRatePedPerSecond = (double)_enteringFlowRatePedPerHour / 3600;
            if (_enteringFlowRatePedPerHour > 0)
                _avgArrivalHeadway = 3600 / _enteringFlowRatePedPerHour;
            else
                _avgArrivalHeadway = double.PositiveInfinity;
            _downstreamNodeId = downstreamNodeId;
            //_downstreamLinkListIndex = downstreamLinkListIndex;
            //_downstreamLinkId = LinkData.LinkID(iD, _downstreamNodeId);
            _downstreamLinkId = downstreamLinkId;
            //double EntryWidth = Math.Pow(Math.Pow(Point1[0] - Point2[0], 2) + Math.Pow(Point1[1] - Point2[1], 2) + Math.Pow(Point1[2] - Point2[2], 2), 0.5);//use distance between points

            Id = iD;

            _pedIdList = new List<uint>();
            _unservedPedEntries = 0;
            
        }

        public int UnservedPedEntries
        {
            get { return _unservedPedEntries; }
            set { _unservedPedEntries = value; }
        }

        public PedArrivalDistribution ArrivalDist
        {
            get { return _arrivalDist; }
            set { _arrivalDist = value; }
        }
        public int EnteringFlowRatePedPerHour
        {
            get { return _enteringFlowRatePedPerHour; }
            set { _enteringFlowRatePedPerHour = value; }
        }
        public double EnteringFlowRatePedPerSecond
        {
            get { return _enteringFlowRatePedPerSecond; }
            set { _enteringFlowRatePedPerSecond = value; }
        }

        public double AvgArrivalHeadway
        {
            get { return _avgArrivalHeadway; }
            set { _avgArrivalHeadway = value; }
        }

        public double MinEntryHeadway
        {
            get { return _minEntryHeadway; }
            set { _minEntryHeadway = value; }
        }
        public double NextPedEntryTime
        {
            get { return _nextPedEntryTime; }
            set { _nextPedEntryTime = value; }
        }
        public uint NumPedEntered
        {
            get { return _numPedEntered; }
            set { _numPedEntered = value; }
        }
        public double AverageDesiredSpeed
        {
            get { return _averageDesiredSpeed; }
            set { _averageDesiredSpeed = value; }
        }
        public UInt16 DownstreamNodeId
        {
            get { return _downstreamNodeId; }
            set { _downstreamNodeId = value; }
        }
        /*public UInt16 DownstreamLinkListIndex
        {
            get { return _downstreamLinkListIndex; }
            set { _downstreamLinkListIndex = value; }
        }*/
        public UInt16 DownstreamLinkId
        {
            get { return _downstreamLinkId; }
            set { _downstreamLinkId = value; }
        }
        public List<uint> PedIdList
        {
            get { return _pedIdList; }
            set { _pedIdList = value; }
        }

    }

    public class PedExitNode : PedNodeData
    {

        public PedExitNode(UInt16 iD, PedNodeType nodeType, UInt16 listIndex, double positionX, double positionY, double[] point1, double[] point2)
            : base(iD, nodeType, listIndex, positionX, positionY, point1, point2)
        {

        }

    }


    public class PedLinkConnectorNode : PedNodeData
    {
        //a link connector node can connect to multiple other downstream/upstream nodes- downstream nodes are to the east or north (positive dx or dy), upstream are to west or south (negative dx or dy)

        int _downstreamNodeListIndex;
        int _upstreamNodeListIndex;
        UInt16 _downstreamLinkId;
        UInt16 _upstreamLinkId;

        List<UInt16> _downstreamNodeIds = new List<UInt16>();  //index 0: left turn node; index 1: through node; index 2: right turn node
        List<UInt16> _upstreamNodeIds = new List<UInt16>();  //index 0: left turn node; index 1: through node; index 2: right turn node
        List<double> _downstreamNodePrcnt = new List<double>(); //"Turn" percentages to each destination node
        List<double> _upstreamNodePrcnt = new List<double>(); //"Turn" percentages to each destination node


        //public LinkConnectorNode(UInt16 iD, NodeType nodeType, UInt16 listIndex, int positionX, int positionY, UInt16 downstreamLeftNodeId, UInt16 downstreamThruNodeId, UInt16 downstreamRightNodeId, ControlType control)
        public PedLinkConnectorNode(UInt16 iD, PedNodeType nodeType, UInt16 listIndex, double positionX, double positionY, double[] point1, double[] point2, UInt16[] upstreamNodeIds, UInt16[] downstreamNodeIds, double[] upstreamNodePrcnts, double[] downstreamNodePrcnts, UInt16 upstreamLinkId, UInt16 downstreamLinkId)
            : base(iD, nodeType, listIndex, positionX, positionY, point1, point2)
        {
            _downstreamNodeListIndex = downstreamNodeIds.Count() - 1;
            _upstreamNodeListIndex = upstreamNodeIds.Count() - 1;
            _upstreamLinkId = upstreamLinkId;
            _downstreamLinkId = downstreamLinkId;
            double cumprcnt = 0;

            for (int node = 0; node <= _downstreamNodeListIndex; node++)
            {                
                cumprcnt = cumprcnt + (downstreamNodePrcnts[node] / downstreamNodePrcnts.Sum());
                _downstreamNodeIds.Add(downstreamNodeIds[node]);
                _downstreamNodePrcnt.Add(cumprcnt);
            }

            cumprcnt = 0;

            for (int node = 0; node <= _upstreamNodeListIndex; node++)
            {                
                cumprcnt = cumprcnt + (upstreamNodePrcnts[node] / upstreamNodePrcnts.Sum());
                _upstreamNodeIds.Add(upstreamNodeIds[node]);
                _upstreamNodePrcnt.Add(cumprcnt);
            }



        }


        public int DownstreamNodeListIndex
        {
            get { return _downstreamNodeListIndex; }
            set { _downstreamNodeListIndex = value; }
        }
        public int UpstreamNodeListIndex
        {
            get { return _upstreamNodeListIndex; }
            set { _upstreamNodeListIndex = value; }
        }
        public UInt16 DownstreamLinkId
        {
            get { return _downstreamLinkId; }
            set { _downstreamLinkId = value; }
        }
        public UInt16 UpstreamLinkId
        {
            get { return _upstreamLinkId; }
            set { _upstreamLinkId = value; }
        }

        public List<UInt16> DownstreamNodeIds
        {
            get { return _downstreamNodeIds; }
            set { _downstreamNodeIds = value; }
        }
        public List<UInt16> UpstreamNodeIds
        {
            get { return _upstreamNodeIds; }
            set { _upstreamNodeIds = value; }
        }
        public List<double> DownstreamNodePrcnt
        {
            get { return _downstreamNodePrcnt; }
            set { _downstreamNodePrcnt = value; }
        }
        public List<double> UpstreamNodePrcnt
        {
            get { return _upstreamNodePrcnt; }
            set { _upstreamNodePrcnt = value; }
        }


        public UInt16[] NewDestNode(PedestrianData Ped, PedLinkConnectorNode DestNode, int TimeIndex)
        {
            double rand = Convert.ToDouble(InputData.RandNum.Next(0, 10000)) / 10000;
            bool upstream = false;//came from upstream node
            for (int i = 0; i <= DestNode.UpstreamNodeIds.Count()-1; i++)
            {
                if (Ped.DestinationList[Ped.DestinationList.Count()-2] == DestNode.UpstreamNodeIds[i])
                {
                    upstream = true;
                }
            }

            if (upstream)
            {
                for (int i = 0; i <= DestNode.DownstreamNodeIds.Count() - 1; i++)
                {
                    if (DestNode.DownstreamNodePrcnt[i] >= rand)
                    {
                        UInt16[] Ids = new UInt16[] { DestNode.DownstreamNodeIds[i], DestNode.DownstreamLinkId };
                        return Ids;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= DestNode.UpstreamNodeIds.Count() - 1; i++)
                {
                    if (DestNode.UpstreamNodePrcnt[i] >= rand)
                    {
                        UInt16[] Ids = new UInt16[] { DestNode.UpstreamNodeIds[i], DestNode.UpstreamLinkId };
                        return Ids;
                    }
                }
            }

            return null;
        }
    }
}

