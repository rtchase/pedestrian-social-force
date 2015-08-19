using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social_Forces_Main
{

    public enum PedLinkType
    {
        Sidewalk,
        Crosswalk
    }

    public class PedLinkObstacle
    {
        double[] _point1;

        public double[] Point1
        {
            get { return _point1; }
            set { _point1 = value; }
        }
        double[] _point2;

        public double[] Point2
        {
            get { return _point2; }
            set { _point2 = value; }
        }

        public PedLinkObstacle(double[] point1, double[] point2)
        {
            _point1 = point1;
            _point2 = point2;
        }
    }

    public class PedLinkData
    {

        //PedLinkType _type;
        UInt16 _id;
        UInt16 _listIndex;
        UInt16 _nodeIdUp;
        UInt16 _nodeIdDown;
        double _length;
        double _width;
        double[] _pctTurnsAtLinkEnd = new double[3];   //index 0 - left turns, index 1 - through movements, index 2 - right turns
        double _xCoordinateStart;
        double _xCoordinateEnd;
        double _yCoordinateStart;
        double _yCoordinateEnd;
        double _orientationAngle;

        UInt16 _downstreamLeftLinkId;
        UInt16 _downstreamThruLinkId;
        UInt16 _downstreamRightLinkId;

        List<PedLinkObstacle> _Obstacles;
        
        

        List<uint> _pedIdList;       //list of id's for vehicles contained in link

        public PedLinkData(UInt16 listIndex, PedNodeData upstreamNode, PedNodeData downstreamNode, double width, double pctLeft, double pctThrough, double pctRight, UInt16 downstreamLeftLinkId, UInt16 downstreamThruLinkId, UInt16 downstreamRightLinkId, List<PedLinkObstacle> obstacles, ushort id)
        {

            _nodeIdUp = upstreamNode.Id;
            _nodeIdDown = downstreamNode.Id;
            //_id = LinkID(_nodeIdUp, _nodeIdDown);
            _id = id;

            _pedIdList = new List<uint>();
            _Obstacles = obstacles;
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
        public UInt16 NodeIdUp
        {
            get { return _nodeIdUp; }
            set { _nodeIdUp = value; }
        }
        public UInt16 NodeIdDown
        {
            get { return _nodeIdDown; }
            set { _nodeIdDown = value; }
        }
        public double XCoordinateStart
        {
            get { return _xCoordinateStart; }
            set { _xCoordinateStart = value; }
        }
        public double XCoordinateEnd
        {
            get { return _xCoordinateEnd; }
            set { _xCoordinateEnd = value; }
        }
        public double YCoordinateStart
        {
            get { return _yCoordinateStart; }
            set { _yCoordinateStart = value; }
        }
        public double YCoordinateEnd
        {
            get { return _yCoordinateEnd; }
            set { _yCoordinateEnd = value; }
        }
        public double OrientationAngle
        {
            get { return _orientationAngle; }
            set { _orientationAngle = value; }
        }
        
        public double Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }
        
        public double[] PctTurnsAtLinkEnd
        {
            get { return _pctTurnsAtLinkEnd; }
            set { _pctTurnsAtLinkEnd = value; }
        }
        
        public UInt16 DownstreamLeftLinkId
        {
            get { return _downstreamLeftLinkId; }
            set { _downstreamLeftLinkId = value; }
        }
        public UInt16 DownstreamThruLinkId
        {
            get { return _downstreamThruLinkId; }
            set { _downstreamThruLinkId = value; }
        }
        public UInt16 DownstreamRightLinkId
        {
            get { return _downstreamRightLinkId; }
            set { _downstreamRightLinkId = value; }
        }
        
        public List<uint> PedIdList
        {
            get { return _pedIdList; }
            set { _pedIdList = value; }
        }

        public List<PedLinkObstacle> Obstacles
        {
            get { return _Obstacles; }
            set { _Obstacles = value; }
        }


    }
}
