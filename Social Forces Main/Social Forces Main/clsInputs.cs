using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social_Forces_Main
{

    public enum ProjectType
    {
        Pedestrian,
        Pedestrian2
    }

    public class InputData
    {

        #region
        //---------------------- Network-Specific Variables ------------------------
        private ProjectType _project;
        public ProjectType Project
        {
            get { return _project; }
            set { _project = value; }
        }

        private double _simDuration;
        public double SimDuration
        {
            get { return _simDuration; }
            set { _simDuration = value; }
        }

        private double _simTimeStep;
        public double SimTimeStep
        {
            get { return _simTimeStep; }
            set { _simTimeStep = value; }
        }

        private int _numTimeSteps;
        public int NumTimeSteps
        {
            get { return _numTimeSteps; }
            set { _numTimeSteps = value; }
        }

        private double[] _simTime;
        public double[] SimTime
        {
            get { return _simTime; }
            set { _simTime = value; }
        }

        private int[] _linkLength;
        public int[] LinkLength
        {
            get { return _linkLength; }
            set { _linkLength = value; }
        }

        private List<PedLinkData> _links;

        public List<PedLinkData> Links
        {
            get { return _links; }
            set { _links = value; }
        }

        private List<PedNodeData> _nodes;

        public List<PedNodeData> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        private PedNetworkData _network;

        public PedNetworkData Network
        {
            get { return _network; }
            set { _network = value; }
        }

        private bool _loaded = false;

        public bool Loaded
        {
            get { return _loaded; }
            set { _loaded = value; }
        }

        //---------------------- Pedestrian-Specific Variables ------------------------

        private PedArrivalDistribution _pedArrivalDist;
        public PedArrivalDistribution PedArrivalDist
        {
            get { return _pedArrivalDist; }
            set { _pedArrivalDist = value; }
        }

        private double[] _linkWidth;
        public double[] LinkWidth
        {
            get { return _linkWidth; }
            set { _linkWidth = value; }
        }

        private double _relaxTime; //τ
        public double RelaxTime
        {
            get { return _relaxTime; }
            set { _relaxTime = value; }
        }
        private double _interactionStrength; //A
        public double InteractionStrength
        {
            get { return _interactionStrength; }
            set { _interactionStrength = value; }
        }
        private double _interactionRange; //B
        public double InteractionRange
        {
            get { return _interactionRange; }
            set { _interactionRange = value; }
        }
        private double _angularDepend; //λ
        public double AngularDepend
        {
            get { return _angularDepend; }
            set { _angularDepend = value; }
        }
        private double _radius; //R
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        private double _pedDesiredSpeed;
        public double PedDesiredSpeed
        {
            get { return _pedDesiredSpeed; }
            set { _pedDesiredSpeed = value; }
        }

        private double _pedStdDevSpeed;
        public double PedStdDevSpeed
        {
            get { return _pedStdDevSpeed; }
            set { _pedStdDevSpeed = value; }
        }

        private double _pedMinSpeed;
        public double PedMinSpeed
        {
            get { return _pedMinSpeed; }
            set { _pedMinSpeed = value; }
        }

        private double _pedMaxSpeed;
        public double PedMaxSpeed
        {
            get { return _pedMaxSpeed; }
            set { _pedMaxSpeed = value; }
        }

        private double _minEntryHeadwayPed;
        public double MinEntryHeadwayPed
        {
            get { return _minEntryHeadwayPed; }
            set { _minEntryHeadwayPed = value; }
        }

        private int[] _enteringFlowRatePed = new int[4];
        public int[] EnteringFlowRatePed
        {
            get { return _enteringFlowRatePed; }
            set { _enteringFlowRatePed = value; }
        }

        #endregion

        public static Random RandNum = new Random();  // //no parameter indicates seed value is based on system timer
        //public static Random RandNum = new Random(12345);

        public static double SecondsPerHour = 3600;
        public static double FeetPerMile = 5280;
        public static int ArraySize = 3001;

        //public static List<VehiclePhysicalProperties> VehTypes = new List<VehiclePhysicalProperties>();

        public InputData(ProjectType project, double flow, double a, double b)
        {

            //Simulation Time Inputs
            _project = project;
            _simDuration = 300;  //units of seconds, 5 minutes
            _simTimeStep = 0.1;  //units of seconds
            _numTimeSteps = Convert.ToInt32(SimDuration / SimTimeStep);
            _simTime = new double[NumTimeSteps + 1];
            
            if (project == ProjectType.Pedestrian)
            {
                //Entry Node Inputs
                _minEntryHeadwayPed = 0.1; //sec
                _enteringFlowRatePed[0] = Convert.ToInt16(flow);  //ped/hr

                //Pedestrian Movement
                _relaxTime = 0.2; //τ
                _interactionStrength = a; //A = 4.30556417?
                _interactionRange = b; //B = 10.5643?
                _angularDepend = 0.06; //λ
                _radius = 0; //R
                _pedDesiredSpeed = 4.5; //ft/s
                _pedStdDevSpeed = 0.37;
                _pedMinSpeed = 3;
                _pedMaxSpeed = 6;
                _linkLength = new int[3] { 5, 50, 10 };
                _linkWidth = new double[2] { (8 * 5), 8 };
            }
        }

        public InputData()
        {
            _relaxTime = 0.2; //τ
            _angularDepend = 0.06; //λ
            _radius = 0; //R
            _pedDesiredSpeed = 4.5; //ft/s
            _pedStdDevSpeed = 0.37;
            _pedMinSpeed = 3;
            _pedMaxSpeed = 6;

        }
    }
}
