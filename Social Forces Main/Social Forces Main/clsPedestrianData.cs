using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Social_Forces_Main
{
    public class PedestrianData
    {
        static int ArraySize = 3001;
        uint _id;
        UInt16 _entryNodeId;
        double _desiredSpeed; //v0
        double _relaxTime; //τ
        double _interactionStrength; //A
        double _interactionRange; //B
        double _angularDepend; //λ
        double _radius; //R
        double _sightDistance; //Implemented at crosswalk level?
        double _critGap;
        double _safetyBuffer; //How close can a vehicle get to the pedestrian
        double[] _accelX = new double[ArraySize];
        double[] _accelY = new double[ArraySize];
        double[] _accelZ = new double[ArraySize];
        double[] _velocityX = new double[ArraySize];
        double[] _velocityY = new double[ArraySize];
        double[] _velocityZ = new double[ArraySize];
        double[] _positionX = new double[ArraySize];
        double[] _positionY = new double[ArraySize];
        double[] _positionZ = new double[ArraySize];
        double[,] _desiredDir = new double[ArraySize, 3];
        UInt16[] _destinationNode = new UInt16[ArraySize];
        UInt16[] _currentLink = new UInt16[ArraySize];
        bool[] _isInNetwork = new bool[ArraySize];
        int _systemEntryTime;
        int _systemExitTime;
        List<UInt16> _destinationList = new List<UInt16>();
        int _pushback = 0;

        


        public PedestrianData(double StartPosX, double StartPosY, double StartPosZ, double StartVelX, double StartVelY, double StartVelZ, int TimeIndex, uint PedId, UInt16 EntryNode, InputData Inputs)
        {
            _positionX[TimeIndex] = StartPosX;
            _positionY[TimeIndex] = StartPosY;
            _positionZ[TimeIndex] = StartPosZ;
            _velocityX[TimeIndex] = StartVelX;
            _velocityY[TimeIndex] = StartVelY;
            _velocityZ[TimeIndex] = StartVelZ;
            _id = PedId;
            _entryNodeId = EntryNode;
            _isInNetwork[TimeIndex] = true;
            _systemEntryTime = TimeIndex;
            _systemExitTime = 3001;
            _relaxTime = Inputs.RelaxTime; //τ
            _interactionStrength = Inputs.InteractionStrength; //A = 4.30556417?
            _interactionRange = Inputs.InteractionRange; //B=10.5643?
            _angularDepend =Inputs.AngularDepend; //λ
            _radius = Inputs.Radius; //R
            _desiredSpeed = PedDesiredSpeed(Inputs); //ft/s

        }

        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public UInt16 EntryNodeId
        {
            get { return _entryNodeId; }
            set { _entryNodeId = value; }
        }

        public double DesiredSpeed
        {
            get { return _desiredSpeed; }
            set { _desiredSpeed = value; }
        }

        public double RelaxTime
        {
            get { return _relaxTime; }
            set { _relaxTime = value; }
        }

        public double InteractionStrength
        {
            get { return _interactionStrength; }
            set { _interactionStrength = value; }
        }

        public double InteractionRange
        {
            get { return _interactionRange; }
            set { _interactionRange = value; }
        }

        public double AngularDepend
        {
            get { return _angularDepend; }
            set { _angularDepend = value; }
        }

        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public double SightDistance
        {
            get { return _sightDistance; }
            set { _sightDistance = value; }
        }

        public double CritGap
        {
            get { return _critGap; }
            set { _critGap = value; }
        }

        public double SafetyBuffer
        {
            get { return _safetyBuffer; }
            set { _safetyBuffer = value; }
        }

        public double[] AccelX
        {
            get { return _accelX; }
            set { _accelX = value; }
        }

        public double[] AccelY
        {
            get { return _accelY; }
            set { _accelY = value; }
        }

        public double[] AccelZ
        {
            get { return _accelZ; }
            set { _accelZ = value; }
        }

        public double[] VelocityX
        {
            get { return _velocityX; }
            set { _velocityX = value; }
        }

        public double[] VelocityY
        {
            get { return _velocityY; }
            set { _velocityY = value; }
        }

        public double[] VelocityZ
        {
            get { return _velocityZ; }
            set { _velocityZ = value; }
        }

        public double[] PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        public double[] PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        public double[] PositionZ
        {
            get { return _positionZ; }
            set { _positionZ = value; }
        }

        public UInt16[] DestinationNode
        {
            get { return _destinationNode; }
            set { _destinationNode = value; }
        }

        public UInt16[] CurrentLink
        {
            get { return _currentLink; }
            set { _currentLink = value; }
        }

        public double[,] DesiredDir
        {
            get { return _desiredDir; }
            set { _desiredDir = value; }
        }

        public int SystemExitTime
        {
            get { return _systemExitTime; }
            set { _systemExitTime = value; }
        }

        public bool[] IsInNetwork
        {
            get { return _isInNetwork; }
            set { _isInNetwork = value; }
        }

        public int SystemEntryTime
        {
            get { return _systemEntryTime; }
            set { _systemEntryTime = value; }
        }

        public List<UInt16> DestinationList
        {
            get { return _destinationList; }
            set { _destinationList = value; }
        }

        public int Pushback
        {
            get { return _pushback; }
            set { _pushback = value; }
        }

        public double PedDesiredSpeed(InputData Inputs)
        {
            double speed = 0;

            while (speed > Inputs.PedMaxSpeed || speed < Inputs.PedMinSpeed)
            {
                double u1 = InputData.RandNum.NextDouble(); //these are uniform(0,1) random doubles
                double u2 = InputData.RandNum.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                speed = Inputs.PedDesiredSpeed + Inputs.PedStdDevSpeed * randStdNormal;

                if (speed > Inputs.PedMinSpeed && speed < Inputs.PedMaxSpeed)
                    break;
            }

            return speed;
        }

        public double[] PedDesDir(PedestrianData Ped, PedNodeData DestNode, int TimeIndex)
        {
            //Find unit vector of shortest vector between point and line segment
            double dx = DestNode.Point1[0] - DestNode.Point2[0];
            double dy = DestNode.Point1[1] - DestNode.Point2[1];
            double dz = DestNode.Point1[2] - DestNode.Point2[2];
            double[] vector = { 0, 0, 0 };
            double[] DesDir = { 0, 0, 0 };


            // Calculate the t that minimizes the distance.
            double t = ((Ped.PositionX[TimeIndex] - DestNode.Point2[0]) * dx + (Ped.PositionY[TimeIndex] - DestNode.Point2[1]) * dy + (Ped.PositionZ[TimeIndex] - DestNode.Point2[2]) * dz) / (dx * dx + dy * dy + dz * dz);
            //double t = 0.5;

            // See if this represents one of the segment's
            // end points or a point in the middle.
            double AvoidanceDistance = Math.Max(2,Ped.Radius);//how far from edge of sidewalk will pedestrians aim
            if (t < 0)
            {
                vector[0] = DestNode.Point2[0] - Ped.PositionX[TimeIndex]-AvoidanceDistance;
                vector[1] = DestNode.Point2[1]-AvoidanceDistance - Ped.PositionY[TimeIndex];
                vector[2] = DestNode.Point2[2] - Ped.PositionZ[TimeIndex];
                DesDir = UnitVector(vector);
            }
            else if (t > 1)
            {
                vector[0] = DestNode.Point1[0] - Ped.PositionX[TimeIndex]-AvoidanceDistance;
                vector[1] = DestNode.Point1[1] + AvoidanceDistance - Ped.PositionY[TimeIndex];
                vector[2] = DestNode.Point1[2] - Ped.PositionZ[TimeIndex];
                DesDir = UnitVector(vector);
            }
            else
            {
                
                if (dx != 0)
                {
                    if (t > (dx  - AvoidanceDistance) / dx)
                    {
                        t = (dx -  - AvoidanceDistance) / dx;
                    }
                    else if (t < (  AvoidanceDistance) / dx)
                    {
                        t = ( AvoidanceDistance) / dx;
                    }
                }
                else if (dy != 0)
                {
                    if (dy > 0)
                    {
                        if (t > (dy  - AvoidanceDistance) / dy)
                        {
                            t = (dy  - AvoidanceDistance) / dy;
                        }
                        else if (t < (  AvoidanceDistance) / Math.Abs(dy))
                        {
                            t = ( AvoidanceDistance) / Math.Abs(dy);
                        }
                    }
                    else
                    {
                        if (t > (dy  + AvoidanceDistance) / dy)
                        {
                            t = (dy  + AvoidanceDistance) / dy;
                        }
                        else if (t < ( AvoidanceDistance) / Math.Abs(dy))
                        {
                            t = ( AvoidanceDistance) / Math.Abs(dy);
                        }
                    }
                }

                vector[0] = (DestNode.Point2[0] + t * dx) - Ped.PositionX[TimeIndex];
                vector[1] = (DestNode.Point2[1] + t * dy) - Ped.PositionY[TimeIndex];
                vector[2] = (DestNode.Point2[2] + t * dz) - Ped.PositionZ[TimeIndex];
                DesDir = UnitVector(vector);
            }



            return DesDir;
        }

        public double[] PedDesDir(double[] PedPosition, PedNodeData DestNode)
        {
            //Find unit vector of shortest vector between point and line segment
            double dx = DestNode.Point1[0] - DestNode.Point2[0];
            double dy = DestNode.Point1[1] - DestNode.Point2[1];
            double dz = DestNode.Point1[2] - DestNode.Point2[2];
            double[] vector = { 0, 0, 0 };
            double[] DesDir = { 0, 0, 0 };


            // Calculate the t that minimizes the distance.
            double t = ((PedPosition[0] - DestNode.Point2[0]) * dx + (PedPosition[1] - DestNode.Point2[1]) * dy + (PedPosition[2] - DestNode.Point2[2]) * dz) / (dx * dx + dy * dy + dz * dz);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                vector[0] = DestNode.Point2[0] - PedPosition[0];
                vector[1] = DestNode.Point2[1] - PedPosition[1];
                vector[2] = DestNode.Point2[2] - PedPosition[2];
                DesDir = UnitVector(vector);
            }
            else if (t > 1)
            {
                vector[0] = DestNode.Point1[0] - PedPosition[0];
                vector[1] = DestNode.Point1[1] - PedPosition[1];
                vector[2] = DestNode.Point1[2] - PedPosition[2];
                DesDir = UnitVector(vector);
            }
            else
            {
                vector[0] = (DestNode.Point2[0] + t * dx) - PedPosition[0];
                vector[1] = (DestNode.Point2[1] + t * dy) - PedPosition[1];
                vector[2] = (DestNode.Point2[2] + t * dz) - PedPosition[2];
                DesDir = UnitVector(vector);
            }

            return DesDir;
        }

        public double[] UnitVector(double[] vector)
        {
            double[] uvector = { 0, 0, 0 };
            double sum = Math.Pow(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2) + Math.Pow(vector[2], 2), 0.5);
            uvector[0] = vector[0] / sum;
            uvector[1] = vector[1] / sum;
            uvector[2] = vector[2] / sum;

            return uvector;
        }

        public double PedPedAngle(PedestrianData Ped1, PedestrianData Ped2, int TimeIndex)
        {
            double dx = Ped2.PositionX[TimeIndex] - Ped1.PositionX[TimeIndex];
            double dy = Ped2.PositionY[TimeIndex] - Ped1.PositionY[TimeIndex];
            double dz = Ped2.PositionZ[TimeIndex] - Ped1.PositionZ[TimeIndex];
            double[] DistVector = { dx, dy, dz };
            double[] DistUnitVector = UnitVector(DistVector);

            double dot = DistUnitVector[0] * Ped1.DesiredDir[TimeIndex, 0] + DistUnitVector[1] * Ped1.DesiredDir[TimeIndex, 1] + DistUnitVector[2] * Ped1.DesiredDir[TimeIndex, 2];

            return dot;
        }

        public bool PassedNodeTest(PedestrianData Ped, PedNodeData DestNode, int TimeIndex)
        {
            double x1 = Ped.PositionX[TimeIndex];
            double x2 = Ped.PositionX[TimeIndex + 1];
            double x3 = DestNode.Point1[0];
            double x4 = DestNode.Point2[0];
            double y1 = Ped.PositionY[TimeIndex];
            double y2 = Ped.PositionY[TimeIndex + 1];
            double y3 = DestNode.Point1[1];
            double y4 = DestNode.Point2[1];

            double det123 = ((x2 - x1) * (y3 - y1)) - ((x3 - x1) * (y2 - y1));
            double det124 = (x2 - x1) * (y4 - y1) - (x4 - x1) * (y2 - y1);
            double det341 = (x3 - x1) * (y4 - y1) - (x4 - x1) * (y3 - y1);
            double det342 = det123 - det124 + det341;

            if (det123 == 0 || det124 == 0 || det341 == 0 || det342 == 0)//3 of the points are colinear
            {
                return true;
            }
            else if (det123 * det124 < 0 && det341 * det342 < 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        double Mag(double[] vector)
        {
            return Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2) + Math.Pow(vector[2], 2));
        }

        public bool PassedObsTest(PedestrianData Ped, List<PedLinkData> PedLinks, int TimeIndex)
        {
            bool passed = false;

            foreach (PedLinkData link in PedLinks)
            {
                foreach (PedLinkObstacle obs in link.Obstacles)
                {
                    double dx = obs.Point1[0] - obs.Point2[0];
                    double dy = obs.Point1[1] - obs.Point2[1];
                    double dz = obs.Point1[2] - obs.Point2[2];
                    double[] vector = { 0, 0, 0 };
                    double[] DesDir = { 0, 0, 0 };


                    // Calculate the t that minimizes the distance.
                    double t = ((Ped.PositionX[TimeIndex + 1] - obs.Point2[0]) * dx + (Ped.PositionY[TimeIndex + 1] - obs.Point2[1]) * dy + (Ped.PositionZ[TimeIndex + 1] - obs.Point2[2]) * dz) / (dx * dx + dy * dy + dz * dz);

                    // See if this represents one of the segment's
                    // end points or a point in the middle.
                    if (t < 0)
                    {
                        vector[0] = obs.Point2[0] - Ped.PositionX[TimeIndex + 1];
                        vector[1] = obs.Point2[1] - Ped.PositionY[TimeIndex + 1];
                        vector[2] = obs.Point2[2] - Ped.PositionZ[TimeIndex + 1];
                    }
                    else if (t > 1)
                    {
                        vector[0] = obs.Point1[0] - Ped.PositionX[TimeIndex + 1];
                        vector[1] = obs.Point1[1] - Ped.PositionY[TimeIndex + 1];
                        vector[2] = obs.Point1[2] - Ped.PositionZ[TimeIndex + 1];
                    }
                    else
                    {
                        vector[0] = (obs.Point2[0] + t * dx) - Ped.PositionX[TimeIndex + 1];
                        vector[1] = (obs.Point2[1] + t * dy) - Ped.PositionY[TimeIndex + 1];
                        vector[2] = (obs.Point2[2] + t * dz) - Ped.PositionZ[TimeIndex + 1];
                    }

                    if (Mag(vector) < Ped.Radius)
                    {
                        return true;
                    }

                    double x1 = Ped.PositionX[TimeIndex];
                    double x2 = Ped.PositionX[TimeIndex + 1];
                    double x3 = obs.Point1[0];
                    double x4 = obs.Point2[0];
                    double y1 = Ped.PositionY[TimeIndex];
                    double y2 = Ped.PositionY[TimeIndex + 1];
                    double y3 = obs.Point1[1];
                    double y4 = obs.Point2[1];

                    double det123 = ((x2 - x1) * (y3 - y1)) - ((x3 - x1) * (y2 - y1));
                    double det124 = (x2 - x1) * (y4 - y1) - (x4 - x1) * (y2 - y1);
                    double det341 = (x3 - x1) * (y4 - y1) - (x4 - x1) * (y3 - y1);
                    double det342 = det123 - det124 + det341;

                    if (det123 == 0 || det124 == 0 || det341 == 0 || det342 == 0)//3 of the points are colinear
                    {
                        return true;
                    }
                    else if (det123 * det124 < 0 && det341 * det342 < 0)
                    {
                        return true;
                    }
                }
            }

            return passed;
        }


    }

}

