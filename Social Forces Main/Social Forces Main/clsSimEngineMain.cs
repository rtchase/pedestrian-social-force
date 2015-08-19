using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Social_Forces_Main
{

    public partial  class SimEngineMain
    {
        
        static double FirstPedEntryTime = 0;

        public static void SimMain(int scenario, int subscenario, int run, double flow1,double flow2, double a, double b)
        {
            
            InputData Inputs = new InputData(ProjectType.Pedestrian, flow1, a, b);
            //InputData Inputs = new InputData(ProjectType.Pedestrian2, flow1, flow2, a, b);
            

            if (Inputs.Project == ProjectType.Pedestrian)
            {

                List<PedestrianData> Peds;
                List<PedNodeData> PedNodes;
                List<PedLinkData> PedLinks;
                PedNetworkData PedNetwork;
                Peds = new List<PedestrianData>();  //Pedestrian Lists
                PedNodes = new List<PedNodeData>();
                PedLinks = new List<PedLinkData>();
                PedNetwork = new PedNetworkData(0);
                NetworkTopologyPed.CreateNetworkTopology(Inputs, PedNetwork, PedNodes, PedLinks);

                for (int TimeIndex = 1; TimeIndex <= Inputs.NumTimeSteps; TimeIndex++)
                    Inputs.SimTime[TimeIndex] = Math.Round(Inputs.SimTime[TimeIndex - 1] + Inputs.SimTimeStep, 1);
                
                for (int PedNodeIndex = 0; PedNodeIndex <= PedNetwork.NumPedNodes - 1; PedNodeIndex++)
                {

                    if (PedNodes[PedNodeIndex].GetType() == typeof(PedEntryNode))
                    {

                        ((PedEntryNode)PedNodes[PedNodeIndex]).AvgArrivalHeadway = 3600 / Convert.ToDouble(((PedEntryNode)PedNodes[PedNodeIndex]).EnteringFlowRatePedPerHour);
                        ((PedEntryNode)PedNodes[PedNodeIndex]).NextPedEntryTime = ((PedEntryNode)PedNodes[PedNodeIndex]).EntryHeadway(((PedEntryNode)PedNodes[PedNodeIndex]).ArrivalDist, ((PedEntryNode)PedNodes[PedNodeIndex]).AvgArrivalHeadway, ((PedEntryNode)PedNodes[PedNodeIndex]).MinEntryHeadway);
                    }
                }

                PedestrianData DummyPed = new PedestrianData(1, 1, 0, 0, 0, 0, 0, 0, 0, Inputs);
                Peds.Add(DummyPed);

                for (int TimeIndex = 0; TimeIndex < Inputs.NumTimeSteps; TimeIndex++)
                {
                    for (int PedNodeIndex = 0; PedNodeIndex <= PedNetwork.NumPedNodes - 1; PedNodeIndex++)
                    {
                        if (PedNodes[PedNodeIndex].GetType() == typeof(PedEntryNode))
                        {
                            if (Inputs.SimTime[TimeIndex] >= ((PedEntryNode)PedNodes[PedNodeIndex]).NextPedEntryTime || ((PedEntryNode)PedNodes[PedNodeIndex]).UnservedPedEntries > 0)
                            {


                                int PedLinkIndex = ((PedEntryNode)PedNodes[PedNodeIndex]).DownstreamLinkId;

                                //double EntryWidth = Convert.ToDouble(Math.Pow(Math.Pow(((PedEntryNode)PedNodes[PedNodeIndex]).Point1[0] - ((PedEntryNode)PedNodes[PedNodeIndex]).Point2[0], 2) + Math.Pow(((PedEntryNode)PedNodes[PedNodeIndex]).Point1[1] - ((PedEntryNode)PedNodes[PedNodeIndex]).Point2[1], 2) + Math.Pow(((PedEntryNode)PedNodes[PedNodeIndex]).Point1[2] - ((PedEntryNode)PedNodes[PedNodeIndex]).Point2[2], 2),0.5));
                                bool entered = true;
                                double[] entrypos = new double[3];
                                PedSocialForce SocialForce = new PedSocialForce();
                                for (int i = 0; i <= 10; i++)
                                {
                                    entered = true;
                                    entrypos = ((PedEntryNode)PedNodes[PedNodeIndex]).EntryOffset(((PedEntryNode)PedNodes[PedNodeIndex]));
                                    foreach (PedestrianData ped in Peds)
                                    {
                                        if (ped.IsInNetwork[TimeIndex])
                                        {
                                            double dx = entrypos[0] - ped.PositionX[TimeIndex];
                                            double dy = entrypos[1] - ped.PositionY[TimeIndex];
                                            double dz = entrypos[2] - ped.PositionZ[TimeIndex];
                                            double[] DistVector = { dx, dy, dz };
                                            if (SocialForce.Mag(DistVector) < 4) //Check that pedestrian does not overlap existing ped
                                            {
                                                entered = false;
                                            }
                                            
                                        }
                                    }
                                    if (entered)
                                    {
                                        break;
                                    }
                                }

                                if (!entered)
                                {
                                    if (Inputs.SimTime[TimeIndex] >= ((PedEntryNode)PedNodes[PedNodeIndex]).NextPedEntryTime)
                                    {
                                        ((PedEntryNode)PedNodes[PedNodeIndex]).UnservedPedEntries++;
                                        ((PedEntryNode)PedNodes[PedNodeIndex]).NextPedEntryTime = Inputs.SimTime[TimeIndex] + ((PedEntryNode)PedNodes[PedNodeIndex]).EntryHeadway(((PedEntryNode)PedNodes[PedNodeIndex]).ArrivalDist, ((PedEntryNode)PedNodes[PedNodeIndex]).AvgArrivalHeadway, ((PedEntryNode)PedNodes[PedNodeIndex]).MinEntryHeadway);
                                    }
                                }
                                else
                                {
                                    if (((PedEntryNode)PedNodes[PedNodeIndex]).UnservedPedEntries > 0)
                                    {
                                        ((PedEntryNode)PedNodes[PedNodeIndex]).UnservedPedEntries--;
                                    }
                                    PedNetwork.TotPedEntered++;
                                    ((PedEntryNode)PedNodes[PedNodeIndex]).NumPedEntered++;

                                    double[] DesiredDirection = new double[3];
                                    DesiredDirection = Peds[0].PedDesDir(entrypos, ((PedNodeData)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(((PedEntryNode)PedNodes[PedNodeIndex]).DownstreamNodeId))]));
                                    PedestrianData NewPed = new PedestrianData(entrypos[0], entrypos[1], entrypos[2], Inputs.PedDesiredSpeed * DesiredDirection[0], Inputs.PedDesiredSpeed * DesiredDirection[1], Inputs.PedDesiredSpeed * DesiredDirection[2], TimeIndex, (UInt32)PedNetwork.TotPedEntered, ((PedEntryNode)PedNodes[PedNodeIndex]).Id, Inputs);

                                    NewPed.DestinationNode[TimeIndex] = ((PedEntryNode)PedNodes[PedNodeIndex]).DownstreamNodeId;
                                    NewPed.DestinationList.Add(PedNodes[PedNodeIndex].Id);
                                    NewPed.DestinationList.Add(NewPed.DestinationNode[TimeIndex]);
                                    NewPed.CurrentLink[TimeIndex] = ((PedEntryNode)PedNodes[PedNodeIndex]).DownstreamLinkId;
                                    NewPed.IsInNetwork[TimeIndex] = true;

                                    NewPed.DesiredDir[TimeIndex, 0] = DesiredDirection[0];
                                    NewPed.DesiredDir[TimeIndex, 1] = DesiredDirection[1];
                                    NewPed.DesiredDir[TimeIndex, 2] = DesiredDirection[2];
                                    Peds.Add(NewPed);

                                    PedLinks[PedLinkIndex].PedIdList.Add((uint)PedNetwork.TotPedEntered);     //add ped to list of peds for link
                                    FirstPedEntryTime = Inputs.SimTime[Peds[1].SystemEntryTime];

                                    if (Inputs.SimTime[TimeIndex] >= ((PedEntryNode)PedNodes[PedNodeIndex]).NextPedEntryTime)
                                    {
                                        ((PedEntryNode)PedNodes[PedNodeIndex]).NextPedEntryTime = Inputs.SimTime[TimeIndex] + ((PedEntryNode)PedNodes[PedNodeIndex]).EntryHeadway(((PedEntryNode)PedNodes[PedNodeIndex]).ArrivalDist, ((PedEntryNode)PedNodes[PedNodeIndex]).AvgArrivalHeadway, ((PedEntryNode)PedNodes[PedNodeIndex]).MinEntryHeadway);
                                    }
                                }
                            }
                        }
                    }

                    if (Inputs.SimTime[TimeIndex] >= FirstPedEntryTime && FirstPedEntryTime > 0)          //do not move peds until the first ped has entered the system
                    {
                        MovePeds(Inputs, PedNetwork, Peds, PedLinks, PedNodes, TimeIndex);
                    }
                }

                int[] Run = new int[3] { scenario, subscenario, run };

                OutputPedTSD.WriteTSDfile3(PedNetwork, Peds, PedLinks, Inputs.NumTimeSteps, Inputs.SimTime, Run, (PedEntryNode)PedNodes[0]);
                //OutputPedTSD.WritePedXML(Peds, Run,(PedEntryNode)PedNodes[0]);
            }
        }

        
    }
}
