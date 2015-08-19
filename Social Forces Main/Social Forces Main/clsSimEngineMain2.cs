using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social_Forces_Main
{
    public partial  class SimEngineMain
    {

        //private static void MoveVehiclesForward(InputData inputs, NetworkData network, List<VehicleData> vehs, List<LinkData> links, List<NodeData> nodes, List<VehicleControlPoint> controlPoints, int timeIndex)
        //{

        //    double MaxTruckAccelOnGrade = 99;
        //    double VehDistanceTraveled;
        //    double[] UpstreamNodePosition = new double[3];  //0 - X coord, 1 - Y coord, 2 - Z coord
        //    bool NewLink = false;  //used to indicate when when a vehicle moves from one link to another; is used in the position calcs to not adjust for a lane change
        //    int NewTimeIndex = timeIndex + 1;

        //    //loop through vehicle list for each link lane
        //    for (int LinkIndex = 0; LinkIndex <= network.NumLinks - 1; LinkIndex++)
        //    {
        //        for (byte LaneNum = 1; LaneNum <= links[LinkIndex].TotalLanes; LaneNum++)
        //        {
        //            foreach (int VehIndex in links[LinkIndex].Lane[(int)LaneNum - 1].VehIdList)
        //            {
        //                //int subjectVehLinkIndex = Network.LinkIdList.FindIndex(item => item == Vehs[VehIndex].LinkIdCurrent[TimeIndex]);
        //                int SubjectVehLinkIndex = links.FindIndex(Link => Link.Id.Equals(vehs[VehIndex].LinkIdCurrent[timeIndex]));
        //                int LeadVehLinkIndex = links.FindIndex(Link => Link.Id.Equals(vehs[(int)vehs[VehIndex].LeaderId[timeIndex]].LinkIdCurrent[timeIndex]));

        //                if (vehs[VehIndex].LeaderId[timeIndex] != 0)
        //                {
        //                    vehs[VehIndex].DistanceToLeadVehicle[timeIndex] = vehs[VehIndex].GetDistanceToLeadVehicle(vehs[VehIndex].PositionLink[timeIndex], links[SubjectVehLinkIndex], vehs[(int)vehs[VehIndex].LeaderId[timeIndex]].PositionLink[timeIndex], links[LeadVehLinkIndex], vehs[(int)vehs[VehIndex].LeaderId[timeIndex]].PhysProperties.Length);

        //                }
        //                else
        //                    vehs[VehIndex].DistanceToLeadVehicle[timeIndex] = -1;  //null;

        //                vehs[VehIndex].PlatoonPos[timeIndex] = vehs[VehIndex].SetPlatoonPosition(vehs[VehIndex].DistanceToLeadVehicle[timeIndex]);

        //                //int DownstreamNodeListIndex = Network.NodeIdList.FindIndex(item => item == Links[SubjectVehLinkIndex].NodeIdDown);   //Vehs[VehIndex].DestinationNodeId[TimeIndex] 
        //                vehs[VehIndex].AccelType[timeIndex] = vehs[VehIndex].DetermineAccelerationMode(vehs[VehIndex], vehs[(int)vehs[VehIndex].LeaderId[timeIndex]], timeIndex, links[SubjectVehLinkIndex], controlPoints);

        //                UpstreamNodePosition[0] = links[SubjectVehLinkIndex].XCoordinateStart;
        //                UpstreamNodePosition[1] = links[SubjectVehLinkIndex].YCoordinateStart;

        //                //accel, velocity, and position are set for system entry time in vehicle class constructor

        //                if (vehs[VehIndex].AccelType[timeIndex] == AccelerationType.CarFollow)
        //                {

        //                    vehs[VehIndex].Accel[NewTimeIndex] = CarFollowModPitt.AccelCal(0.75, inputs.SimTimeStep, timeIndex - 1, timeIndex, vehs[VehIndex], vehs[(int)vehs[VehIndex].LeaderId[timeIndex]], vehs[VehIndex].Driver.DesiredSpeed, links[SubjectVehLinkIndex], links[LeadVehLinkIndex]);
        //                }

        //                else if (vehs[VehIndex].AccelType[timeIndex] == AccelerationType.NoCarFollow)
        //                {
        //                    if (vehs[VehIndex].Velocity[timeIndex] < vehs[VehIndex].Driver.DesiredSpeed)
        //                        vehs[VehIndex].Accel[NewTimeIndex] = vehs[VehIndex].Driver.DesiredAccel;
        //                    else
        //                        vehs[VehIndex].Accel[NewTimeIndex] = 0;

        //                }
        //                else if (vehs[VehIndex].AccelType[timeIndex] == AccelerationType.StoppingLeader)
        //                {
        //                    //vehs[VehIndex].Accel[NewTimeIndex] = vehs[VehIndex].Driver.DesiredDecel * 1.25;
        //                    vehs[VehIndex].Accel[NewTimeIndex] = -0.5 * Math.Pow(vehs[VehIndex].Velocity[timeIndex - 1], 2) / (vehs[VehIndex].DistanceToControlPoint);

        //                }
        //                else if (vehs[VehIndex].AccelType[timeIndex] == AccelerationType.StoppingFollower)
        //                {
        //                    //double BackOfQueue = vehs[(int)vehs[VehIndex].LeaderId[timeIndex]].PositionLink[timeIndex] - vehs[(int)vehs[VehIndex].LeaderId[timeIndex]].Length - 10;
        //                    //vehs[VehIndex].Accel[NewTimeIndex] = 0.5 * Math.Pow(vehs[VehIndex].Velocity[timeIndex - 1], 2) / -(Math.Abs(vehs[VehIndex].PositionLink[timeIndex - 1] - BackOfQueue));

        //                    vehs[VehIndex].Accel[NewTimeIndex] = -0.5 * Math.Pow(vehs[VehIndex].Velocity[timeIndex - 1], 2) / (Math.Abs(vehs[VehIndex].DistanceToLeadVehicle[timeIndex] - 10));

        //                }
        //                else if (vehs[VehIndex].AccelType[timeIndex] == AccelerationType.Stopped)
        //                {
        //                    vehs[VehIndex].PositionX[NewTimeIndex] = vehs[VehIndex].PositionX[timeIndex];
        //                    vehs[VehIndex].PositionY[NewTimeIndex] = vehs[VehIndex].PositionY[timeIndex];
        //                    vehs[VehIndex].Angle[NewTimeIndex] = vehs[VehIndex].Angle[timeIndex];
        //                }

        //                //---------- check max acceleration constraint for trucks ---------------------------------------------------
        //                if (vehs[VehIndex].PhysProperties.Fleet == FleetType.truck)
        //                    MaxTruckAccelOnGrade = CarFollowModPitt.MaxAccelOnGrade(links[SubjectVehLinkIndex].GradePct / 100, vehs[VehIndex].PhysProperties.WeightLB, vehs[VehIndex].PhysProperties.PowerHP, vehs[VehIndex].PhysProperties.DragCoeff, vehs[VehIndex].PhysProperties.FrontalAreaFT, vehs[VehIndex].Velocity[timeIndex]);
        //                if (vehs[VehIndex].Accel[NewTimeIndex] > MaxTruckAccelOnGrade)
        //                    vehs[VehIndex].Accel[NewTimeIndex] = MaxTruckAccelOnGrade;


        //                //---------- check whether acceleration value needs to be overridden due to reduced free-flow speed on destination link ---------------
        //                int DestinationLinkIndex = links.FindIndex(Link => Link.Id.Equals(vehs[VehIndex].DestinationLinkId));
        //                if (DestinationLinkIndex > 0)
        //                {
        //                    if (vehs[VehIndex].AccelType[timeIndex] == AccelerationType.CarFollow || vehs[VehIndex].AccelType[timeIndex] == AccelerationType.NoCarFollow)
        //                        vehs[VehIndex].Accel[NewTimeIndex] = AdjacentLinkVelocityDifference(timeIndex, vehs[VehIndex], links[SubjectVehLinkIndex], links[DestinationLinkIndex]);
        //                }
        //                //---------------------------------------------------------------------------------------------------

        //                vehs[VehIndex].Velocity[NewTimeIndex] = VelocityFollower(NewTimeIndex, timeIndex, inputs.SimTimeStep, vehs[VehIndex]);

        //                if (vehs[VehIndex].Velocity[NewTimeIndex] == 0)
        //                    vehs[VehIndex].Accel[NewTimeIndex] = 0;

        //                if (inputs.ObdEnabled == true)
        //                {
        //                    vehs[VehIndex].OBD.GearRatio[NewTimeIndex] = vehs[VehIndex].OBD.DetermineGearRatio(vehs[VehIndex].Velocity[NewTimeIndex]);
        //                    vehs[VehIndex].OBD.Rpm[NewTimeIndex] = vehs[VehIndex].OBD.RevsPerMinute(vehs[VehIndex].OBD.GearRatio[NewTimeIndex], vehs[VehIndex].Velocity[NewTimeIndex]);
        //                }

        //                VehDistanceTraveled = DistanceTraveled(NewTimeIndex, timeIndex, inputs.SimTimeStep, vehs[VehIndex]);


        //                //Because lane numbers may not match between turn links and connecting links
        //                //to-do: remove after implementing connecting lane id's logic
        //                int CurrentTimeStepLinkListIndex = links.FindIndex(Link => Link.Id.Equals(vehs[VehIndex].LinkIdCurrent[timeIndex]));
        //                int PreviousTimeStepLinkListIndex = links.FindIndex(Link => Link.Id.Equals(vehs[VehIndex].LinkIdPrevious[timeIndex]));
        //                if (PreviousTimeStepLinkListIndex != -1)  //do not check for vehicles on entry link
        //                {
        //                    if (links[CurrentTimeStepLinkListIndex].CurveType == links[PreviousTimeStepLinkListIndex].CurveType)
        //                        NewLink = false;
        //                    else
        //                        NewLink = true;
        //                }

        //                CalculateVehiclePositionValues(timeIndex, NewTimeIndex, vehs[VehIndex], links[SubjectVehLinkIndex], VehDistanceTraveled, UpstreamNodePosition[0], UpstreamNodePosition[1], NewLink);

        //                if (vehs[VehIndex].PositionLink[NewTimeIndex] == vehs[VehIndex].PositionLink[timeIndex])
        //                {
        //                    //with current bezier curve calculations, when vehicle reaches end of curve, its link position will be exactly equal to curve length
        //                    //the second time the link position equals the curve length, the vehicle needs to be moved onto its destination link
        //                    /*
        //                    int TargetLinkListIndex = Links.FindIndex(Link => Link.Id.Equals(Vehs[VehIndex].LinkIdTarget[TimeIndex]));
        //                    UpstreamNodePosition[0] = Links[TargetLinkListIndex].XCoordinateStart;
        //                    UpstreamNodePosition[1] = Links[TargetLinkListIndex].YCoordinateStart;
        //                    CalculateVehiclePositionValues(TimeIndex, NewTimeIndex, Vehs[VehIndex], Links[TargetLinkListIndex], DistanceTraveled, UpstreamNodePosition[0], UpstreamNodePosition[1]);
        //                    */

        //                    //Values will get updated at the next check for link position versus link length
        //                    vehs[VehIndex].PositionLink[NewTimeIndex] = vehs[VehIndex].PositionLink[timeIndex] + VehDistanceTraveled;

        //                }




        //            }
        //        }
        //    }
        //}

        //public static double VelocityFollower(int currentTimeIndex, int previousTimeIndex, double timeStep, VehicleData subjectVeh)
        //{
        //    //Velocity Calculation
        //    //possibly revise the following code to allow leader vehicle's speed to vary +/- 2 ft/s (3 mi/h) from desired speed

        //    double SubjectVehVelocity = Math.Round(subjectVeh.Velocity[previousTimeIndex] + subjectVeh.Accel[currentTimeIndex] * timeStep, 2);

        //    //If the calculated velocity is greater than the desired speed, then set it equal to the desired speed
        //    if (SubjectVehVelocity > subjectVeh.Driver.DesiredSpeed)
        //    {
        //        //to-do: change acceleration such that desired speed will not be exceeded
        //        SubjectVehVelocity = subjectVeh.Driver.DesiredSpeed;
        //        if (SubjectVehVelocity == subjectVeh.Velocity[previousTimeIndex])
        //            //replace with calculation to back-solve for acceleration that gets vehicle to desired speed
        //            subjectVeh.Accel[currentTimeIndex] = 0;

        //    }

        //    if (SubjectVehVelocity < 0.05)  //this avoids an overly extended deceleration period for vehicles stopping at a control point
        //        SubjectVehVelocity = 0;

        //    return SubjectVehVelocity;
        //}

        ////public static double DistanceTraveled(int currentTimeIndex, int previousTimeIndex, double timeStep, VehicleData subjectVeh, double upstreamNodePosition)
        //public static double DistanceTraveled(int currentTimeIndex, int previousTimeIndex, double timeStep, VehicleData subjectVeh)
        //{
        //    double DistanceTraveled = subjectVeh.Velocity[previousTimeIndex] * timeStep + 0.5 * subjectVeh.Accel[currentTimeIndex] * Math.Pow(timeStep, 2);

        //    return DistanceTraveled;
        //}

        //private static void CalculateVehiclePositionValues(int timeIndex, int newTimeIndex, VehicleData subjectVehicle, LinkData link, double distanceTraveled, double upstreamNodeStartX, double upstreamNodeStartY, bool newLink)
        //{
        //    double[] Position = new double[5];  //0 - X coord, 1 - Y coord, 2 - Z coord, 3 - bezier curve tangent, 4 - bezier curve cumulative length

        //    if (link.CurveType == LinkCurveType.Linear)
        //    {
        //        Position = VehicleData.CalcPositionCoordinates(subjectVehicle.PositionX[timeIndex], subjectVehicle.PositionY[timeIndex], distanceTraveled, subjectVehicle.LaneIdCurrent[timeIndex - 1], subjectVehicle.LaneIdCurrent[timeIndex], link, newLink);
        //        subjectVehicle.PositionX[newTimeIndex] = Position[0];
        //        subjectVehicle.PositionY[newTimeIndex] = Position[1];
        //        subjectVehicle.Angle[newTimeIndex] = link.OrientationAngleDegrees;
        //        subjectVehicle.PositionLink[newTimeIndex] = VehicleData.CalcLinearLinkPosition(subjectVehicle.PositionX[newTimeIndex], subjectVehicle.PositionY[newTimeIndex], upstreamNodeStartX, upstreamNodeStartY, link.OrientationAngleDegrees);
        //    }
        //    else if (link.CurveType == LinkCurveType.Bezier)
        //    {
        //        Position = VehicleData.CalcBezierCurvePositionValues(subjectVehicle.PositionLink[timeIndex], distanceTraveled, link);
        //        subjectVehicle.PositionX[newTimeIndex] = Position[0];
        //        subjectVehicle.PositionY[newTimeIndex] = Position[1];
        //        subjectVehicle.Angle[newTimeIndex] = Position[3];
        //        subjectVehicle.PositionLink[newTimeIndex] = Position[4];
        //    }
        //}

        //public static double AdjacentLinkVelocityDifference(int timeIndex, VehicleData subjectVeh, LinkData currentLink, LinkData destinationLink)
        //{
        //    //Determine if a vehicle will need to decelerate to a slower speed before entering a new link due to a lower free-flow speed
        //    double VehCurrentVelocity = subjectVeh.Velocity[timeIndex];
        //    double DestLinkDesiredSpeed = destinationLink.FreeFlowSpeed * subjectVeh.Driver.DesiredSpeedMultipliers[subjectVeh.Driver.Type - 1];
        //    double VelocityDifference = VehCurrentVelocity - DestLinkDesiredSpeed;
        //    double DecelDistance = 0;
        //    double acceleration = subjectVeh.Accel[timeIndex + 1];

        //    if (VelocityDifference > 0)
        //    {
        //        DecelDistance = (Math.Pow(VehCurrentVelocity, 2) - Math.Pow(DestLinkDesiredSpeed, 2)) / -(2 * subjectVeh.Driver.DesiredDecel);

        //        if (currentLink.Length - subjectVeh.PositionLink[timeIndex] <= DecelDistance)
        //            acceleration = -0.5 * (Math.Pow(VehCurrentVelocity, 2) - Math.Pow(DestLinkDesiredSpeed, 2)) / (currentLink.Length - subjectVeh.PositionLink[timeIndex]);
        //    }

        //    return acceleration;
        //}

        private static void MovePeds(InputData Inputs, PedNetworkData PedNetwork, List<PedestrianData> Peds, List<PedLinkData> PedLinks, List<PedNodeData> PedNodes, int TimeIndex)
        {
            double[] Accel = { 0, 0, 0 };
            double[] PedPed = { 0, 0, 0 };
            double[] PedDes = { 0, 0, 0 };
            double[] PedObs = { 0, 0, 0 };

            PedSocialForce PedSF = new PedSocialForce();


            for (int Ped1Index = 1; Ped1Index <= PedNetwork.TotPedEntered; Ped1Index++)
            {
                if (Peds[Ped1Index].IsInNetwork[TimeIndex])
                {
                    for (int Ped2Index = 1; Ped2Index <= PedNetwork.TotPedEntered; Ped2Index++)
                    {
                        if (Peds[Ped2Index].IsInNetwork[TimeIndex])
                        {
                            if (Ped1Index != Ped2Index)
                            {
                                PedPed = PedSF.PedForceCalc(Peds[Ped1Index], Peds[Ped2Index], TimeIndex, PedNodes[PedNodes.FindIndex(node => node.Id.Equals((Peds[Ped1Index]).DestinationNode[TimeIndex]))], PedNodes[PedNodes.FindIndex(node => node.Id.Equals((Peds[Ped2Index]).DestinationNode[TimeIndex]))]);//calc ped-ped force

                                Accel[0] = Accel[0] + PedPed[0];
                                Accel[1] = Accel[1] + PedPed[1];
                                Accel[2] = Accel[2] + PedPed[2];

                                if (PedPed[0] == Double.NegativeInfinity)
                                {
                                }
                            }
                        }
                    }

                    PedDes = PedSF.PedForceCalc(Peds[Ped1Index], TimeIndex, ((PedNodeData)PedNodes[PedNodes.FindIndex(node => node.Id.Equals((Peds[Ped1Index]).DestinationNode[TimeIndex]))]));//calc desired force

                    PedObs = PedSF.PedForceCalc(Peds[Ped1Index], TimeIndex, ((PedNodeData)PedNodes[PedNodes.FindIndex(node => node.Id.Equals((Peds[Ped1Index]).DestinationNode[TimeIndex]))]), PedLinks[PedLinks.FindIndex(link => link.Id.Equals((Peds[Ped1Index]).CurrentLink[TimeIndex]))]);//calc obstacle forces


                    Accel[0] = Accel[0] + PedDes[0] + PedObs[0];
                    Accel[1] = Accel[1] + PedDes[1] + PedObs[1];
                    Accel[2] = Accel[2] + PedDes[2] + PedObs[2];

                    /*if (PedSF.Mag(Accel) > 10)
                    {
                        Accel[0] = Accel[0] / PedSF.Mag(Accel);
                        Accel[1] = Accel[1] / PedSF.Mag(Accel);
                        Accel[2] = Accel[2] / PedSF.Mag(Accel);
                    }*/

                    if (Accel[0] == double.NaN)
                    {
                        throw new System.ArgumentException();
                    }

                    Peds[Ped1Index].AccelX[TimeIndex] = Accel[0];
                    Peds[Ped1Index].AccelY[TimeIndex] = Accel[1];
                    Peds[Ped1Index].AccelZ[TimeIndex] = Accel[2];

                    Peds[Ped1Index].VelocityX[TimeIndex + 1] = Peds[Ped1Index].VelocityX[TimeIndex] + (Peds[Ped1Index].AccelX[TimeIndex] * 0.1); //Change Velocity
                    Peds[Ped1Index].VelocityY[TimeIndex + 1] = Peds[Ped1Index].VelocityY[TimeIndex] + (Peds[Ped1Index].AccelY[TimeIndex] * 0.1);
                    Peds[Ped1Index].VelocityZ[TimeIndex + 1] = Peds[Ped1Index].VelocityZ[TimeIndex] + (Peds[Ped1Index].AccelZ[TimeIndex] * 0.1);

                    Peds[Ped1Index].PositionX[TimeIndex + 1] = Peds[Ped1Index].PositionX[TimeIndex] + (Peds[Ped1Index].VelocityX[TimeIndex] * 0.1) + (0.5 * Peds[Ped1Index].AccelX[TimeIndex] * 0.01);//move over time
                    Peds[Ped1Index].PositionY[TimeIndex + 1] = Peds[Ped1Index].PositionY[TimeIndex] + (Peds[Ped1Index].VelocityY[TimeIndex] * 0.1) + (0.5 * Peds[Ped1Index].AccelY[TimeIndex] * 0.01);
                    Peds[Ped1Index].PositionZ[TimeIndex + 1] = Peds[Ped1Index].PositionZ[TimeIndex] + (Peds[Ped1Index].VelocityZ[TimeIndex] * 0.1) + (0.5 * Peds[Ped1Index].AccelZ[TimeIndex] * 0.01);

                    //Check if new position overlaps obstacles- stay in place one time period if it does
                    if (Peds[Ped1Index].PassedObsTest(Peds[Ped1Index], PedLinks, TimeIndex))
                    {
                        Peds[Ped1Index].VelocityX[TimeIndex + 1] = 0; //Change Velocity
                        Peds[Ped1Index].VelocityY[TimeIndex + 1] = 0;
                        Peds[Ped1Index].VelocityZ[TimeIndex + 1] = 0;

                        Peds[Ped1Index].PositionX[TimeIndex + 1] = Peds[Ped1Index].PositionX[TimeIndex];//move over time
                        Peds[Ped1Index].PositionY[TimeIndex + 1] = Peds[Ped1Index].PositionY[TimeIndex];
                        Peds[Ped1Index].PositionZ[TimeIndex + 1] = Peds[Ped1Index].PositionZ[TimeIndex];
                    }

                    if (Ped1Index == 1 && TimeIndex == 1083)
                    {
                    }

                    //calc new destination node- Do lines between new and old position intersect the node gate?
                    if (Peds[Ped1Index].PassedNodeTest(Peds[Ped1Index], ((PedNodeData)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationNode[TimeIndex]))]), TimeIndex))
                    {


                        if (((PedNodeData)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationNode[TimeIndex]))]).GetType() == typeof(PedEntryNode) || ((PedNodeData)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationNode[TimeIndex]))]).GetType() == typeof(PedExitNode))
                        {
                            Peds[Ped1Index].IsInNetwork[TimeIndex + 1] = false;
                            Peds[Ped1Index].SystemExitTime = TimeIndex + 1;
                        }
                        else
                        {
                            UInt16[] NewIds = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationNode[TimeIndex]))]).NewDestNode(Peds[Ped1Index], (PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationNode[TimeIndex]))], TimeIndex);
                            Peds[Ped1Index].DestinationNode[TimeIndex + 1] = NewIds[0];
                            Peds[Ped1Index].DestinationList.Add(NewIds[0]);
                            Peds[Ped1Index].CurrentLink[TimeIndex + 1] = NewIds[1];
                            Peds[Ped1Index].IsInNetwork[TimeIndex + 1] = true;
                        }

                    }
                    //else if (Peds[Ped1Index].DestinationList.Count() > 1 && PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationList[Peds[Ped1Index].DestinationList.Count() - 2]))].GetType() == typeof(PedLinkConnectorNode) && Peds[Ped1Index].DestinationNode[TimeIndex - 1] == Peds[Ped1Index].DestinationNode[TimeIndex])//check if peds are pushed back across links
                    //{

                    //}
                    /*else if (Peds[Ped1Index].DestinationList.Count() > 1 && PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationList[Peds[Ped1Index].DestinationList.Count() - 2]))].GetType() == typeof(PedLinkConnectorNode) && Peds[Ped1Index].DestinationNode[TimeIndex - 1] == Peds[Ped1Index].DestinationNode[TimeIndex])//check if peds are pushed back across links
                    {
                        List<UInt16> DownStreamIds = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationList[Peds[Ped1Index].DestinationList.Count() - 2]))]).DownstreamNodeIds;
                        List<UInt16> UpStreamIds = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationList[Peds[Ped1Index].DestinationList.Count() - 2]))]).UpstreamNodeIds;
                        bool crossed = false;

                        for (int k = 0; k < DownStreamIds.Count()-1; k++)
                        {
                            if (PedNodes[PedNodes.FindIndex(node => node.Id.Equals(DownStreamIds[k]))].GetType() == typeof(PedLinkConnectorNode))
                            {
                                if (Peds[Ped1Index].PassedNodeTest(Peds[Ped1Index], ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(DownStreamIds[k]))]), TimeIndex))
                                {
                                    Peds[Ped1Index].DestinationNode[TimeIndex + 1] = DownStreamIds[k];

                                    if (((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(DownStreamIds[k]))]).DownstreamLinkId == Peds[Ped1Index].CurrentLink[TimeIndex])
                                    {
                                        Peds[Ped1Index].CurrentLink[TimeIndex + 1] = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(DownStreamIds[k]))]).UpstreamLinkId;
                                    }
                                    else
                                    {
                                        Peds[Ped1Index].CurrentLink[TimeIndex + 1] = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(DownStreamIds[k]))]).DownstreamLinkId;
                                    }

                                    Peds[Ped1Index].DestinationList.Remove(Peds[Ped1Index].DestinationNode[TimeIndex]);
                                    Peds[Ped1Index].IsInNetwork[TimeIndex + 1] = true;
                                    crossed = true;
                                }
                            }
                        }

                        for (int k = 0; k < UpStreamIds.Count(); k++)
                        {
                            if (PedNodes[PedNodes.FindIndex(node => node.Id.Equals(UpStreamIds[k]))].GetType() == typeof(PedLinkConnectorNode))
                            {
                                if (Peds[Ped1Index].PassedNodeTest(Peds[Ped1Index], ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(UpStreamIds[k]))]), TimeIndex))
                                {
                                    Peds[Ped1Index].DestinationNode[TimeIndex + 1] = UpStreamIds[k];

                                    if (((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(UpStreamIds[k]))]).DownstreamLinkId == Peds[Ped1Index].CurrentLink[TimeIndex])
                                    {
                                        Peds[Ped1Index].CurrentLink[TimeIndex + 1] = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(UpStreamIds[k]))]).UpstreamLinkId;
                                    }
                                    else
                                    {
                                        Peds[Ped1Index].CurrentLink[TimeIndex + 1] = ((PedLinkConnectorNode)PedNodes[PedNodes.FindIndex(node => node.Id.Equals(UpStreamIds[k]))]).DownstreamLinkId;
                                    }

                                    Peds[Ped1Index].DestinationList.Remove(Peds[Ped1Index].DestinationNode[TimeIndex]);
                                    Peds[Ped1Index].IsInNetwork[TimeIndex + 1] = true;
                                    crossed = true;
                                }
                            }
                        }

                        if (crossed == false)
                        {
                            Peds[Ped1Index].DestinationNode[TimeIndex + 1] = Peds[Ped1Index].DestinationNode[TimeIndex];
                            Peds[Ped1Index].CurrentLink[TimeIndex + 1] = Peds[Ped1Index].CurrentLink[TimeIndex];
                            Peds[Ped1Index].IsInNetwork[TimeIndex + 1] = true;
                        }
                    }*/
                    else
                    {
                        Peds[Ped1Index].DestinationNode[TimeIndex + 1] = Peds[Ped1Index].DestinationNode[TimeIndex];
                        Peds[Ped1Index].CurrentLink[TimeIndex + 1] = Peds[Ped1Index].CurrentLink[TimeIndex];
                        Peds[Ped1Index].IsInNetwork[TimeIndex + 1] = true;
                    }

                    // Calc new desired direction

                    if (Peds[Ped1Index].IsInNetwork[TimeIndex + 1])
                    {
                        double[] DesDir = Peds[Ped1Index].PedDesDir(Peds[Ped1Index], PedNodes[PedNodes.FindIndex(node => node.Id.Equals(Peds[Ped1Index].DestinationNode[TimeIndex + 1]))], TimeIndex + 1);

                        
                        int tau = 4;//time periods to change direction 90 deg

                        if (TimeIndex > tau + 1 && Peds[Ped1Index].DestinationNode[TimeIndex + 1] != Peds[Ped1Index].DestinationNode[TimeIndex - tau + 1])//do not allow instantaneous change in desired direction when getting new destination node
                        {
                            if (Math.Abs(DesDir[0] - Peds[Ped1Index].DesiredDir[TimeIndex, 0]) > (1 / tau) || Math.Abs(DesDir[1] - Peds[Ped1Index].DesiredDir[TimeIndex, 1]) > (1 / tau) || Math.Abs(DesDir[2] - Peds[Ped1Index].DesiredDir[TimeIndex, 2]) > (1 / tau))
                            {
                                Peds[Ped1Index].DesiredDir[TimeIndex + 1, 0] = Peds[Ped1Index].DesiredDir[TimeIndex, 0] + (DesDir[0] - Peds[Ped1Index].DesiredDir[TimeIndex, 0]) * (1 / tau);
                                Peds[Ped1Index].DesiredDir[TimeIndex + 1, 1] = Peds[Ped1Index].DesiredDir[TimeIndex, 1] + (DesDir[1] - Peds[Ped1Index].DesiredDir[TimeIndex, 1]) * (1 / tau);
                                Peds[Ped1Index].DesiredDir[TimeIndex + 1, 2] = Peds[Ped1Index].DesiredDir[TimeIndex, 2] + (DesDir[2] - Peds[Ped1Index].DesiredDir[TimeIndex, 2]) * (1 / tau);
                            }
                            else
                            {
                                Peds[Ped1Index].DesiredDir[TimeIndex + 1, 0] = DesDir[0];
                                Peds[Ped1Index].DesiredDir[TimeIndex + 1, 1] = DesDir[1];
                                Peds[Ped1Index].DesiredDir[TimeIndex + 1, 2] = DesDir[2];
                            }
                        }
                        else
                        {
                            Peds[Ped1Index].DesiredDir[TimeIndex + 1, 0] = DesDir[0];
                            Peds[Ped1Index].DesiredDir[TimeIndex + 1, 1] = DesDir[1];
                            Peds[Ped1Index].DesiredDir[TimeIndex + 1, 2] = DesDir[2];
                        }
                    }

                    Accel[0] = 0;
                    Accel[1] = 0;
                    Accel[2] = 0;


                }

            }

        }

    }
}
