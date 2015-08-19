using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Social_Forces_Main
{
    public class NetworkTopologyPed
    {
        public static void CreateNetworkTopology(InputData inputs, PedNetworkData PedNetwork, List<PedNodeData> node, List<PedLinkData> link)
        {
            PedNetwork.NumPedLinks = 0;
            int XcoordStart = 5;
            int YcoordStart = 10;
            double Width = (8*5);

            if (inputs.Project == ProjectType.Pedestrian)
            {
                //single sidewalk
                // entry node
                PedNetwork.NumPedNodes++;
                double[] p1 = new double[] { XcoordStart, YcoordStart - Width / 2, 0 };
                double[] p2 = new double[] { XcoordStart, YcoordStart + Width / 2, 0 };
                PedEntryNode NewEntryNode = new PedEntryNode(1, PedNodeType.Entry, 0, XcoordStart, YcoordStart, p1, p2, 0, 2, inputs.MinEntryHeadwayPed, inputs.EnteringFlowRatePed[0], inputs.PedArrivalDist);
                node.Add(NewEntryNode);
                PedNetwork.PedNodeIdList.Add(NewEntryNode.Id);

                //connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { XcoordStart + inputs.LinkLength[0], YcoordStart - Width / 2, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0], YcoordStart + Width / 2, 0 };
                UInt16[] U1 = { 1 };
                UInt16[] D1 = { 3 };
                double[] U2 = { 1 };
                double[] D2 = { 1 };
                PedLinkConnectorNode NewConnectorNode = new PedLinkConnectorNode(2, PedNodeType.LinkConnector, 1, XcoordStart + inputs.LinkLength[0], YcoordStart, p1, p2, U1, D1, U2, D2, 0, 1);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart - 4, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart +  4, 0 };
                U1 = new UInt16[] { 2 };
                D1 = new UInt16[] { 4 };
                U2 = new double[] { 1 };
                D2 = new double[] { 1 };
                NewConnectorNode = new PedLinkConnectorNode(3, PedNodeType.LinkConnector, 2, XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart, p1, p2, U1, D1, U2, D2, 1, 2);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // exit node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1] + inputs.LinkLength[2], YcoordStart -  4, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1] + inputs.LinkLength[2], YcoordStart +  4, 0 };
                //PedEntryNode NewEntryNode2 = new PedEntryNode(4, PedNodeType.Entry, 3, XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1] + inputs.LinkLength[2], YcoordStart, p1, p2, 2, 3, inputs.MinEntryHeadwayPed, inputs.EnteringFlowRatePed[0], inputs.PedArrivalDist);
                //node.Add(NewEntryNode2);
                //PedNetwork.PedNodeIdList.Add(NewEntryNode2.Id);
                PedExitNode NewExitNode = new PedExitNode(4, PedNodeType.Exit, 3, XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1] + inputs.LinkLength[2], YcoordStart, p1, p2);
                node.Add(NewExitNode);
                PedNetwork.PedNodeIdList.Add(NewExitNode.Id);


                //link 1 obstacles
                List<PedLinkObstacle> Obs1 = new List<PedLinkObstacle>();
                p1 = new double[] { XcoordStart - 5, YcoordStart - Width / 2, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0], YcoordStart - Width / 2, 0 };
                PedLinkObstacle NewObs = new PedLinkObstacle(p1, p2);
                Obs1.Add(NewObs);
                p1 = new double[] { XcoordStart - 5, YcoordStart + Width / 2, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0], YcoordStart + Width / 2, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs1.Add(NewObs);
                p1 = new double[] { XcoordStart - 5, YcoordStart - Width / 2, 0 };
                p2 = new double[] { XcoordStart - 5, YcoordStart + Width / 2, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs1.Add(NewObs);

                List<PedLinkObstacle> Obs2 = new List<PedLinkObstacle>();
                p1 = new double[] { XcoordStart + inputs.LinkLength[0], YcoordStart - Width / 2, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart - Width / 2, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs2.Add(NewObs);
                p1 = new double[] { XcoordStart + inputs.LinkLength[0], YcoordStart + Width / 2, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart + Width / 2, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs2.Add(NewObs);
                p1 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart - Width / 2, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart - 4, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs2.Add(NewObs);
                p1 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart +  4, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart + Width / 2, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs2.Add(NewObs);

                List<PedLinkObstacle> Obs3 = new List<PedLinkObstacle>();
                p1 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart -  4, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1] + inputs.LinkLength[2], YcoordStart -  4, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs3.Add(NewObs);
                p1 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1], YcoordStart + 4, 0 };
                p2 = new double[] { XcoordStart + inputs.LinkLength[0] + inputs.LinkLength[1] + inputs.LinkLength[2], YcoordStart +  4, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs3.Add(NewObs);

                // link 1
                PedNetwork.NumPedLinks++;
                PedLinkData NewLink = new PedLinkData(0, node[0], node[1], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs1, 0);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 2
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(1, node[1], node[2], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs2, 1);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 3
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(2, node[2], node[3], inputs.LinkWidth[1], 0, 100, 0, 0, 0, 0, Obs3, 2);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);


            }

            else if (inputs.Project == ProjectType.Pedestrian2)
            {
                // Single sidewalk Intersection
                // entry node
                PedNetwork.NumPedNodes++;
                double[] p1 = new double[] { 5, 65, 0 };
                double[] p2 = new double[] { 5, 81, 0 };
                PedEntryNode NewEntryNode = new PedEntryNode(1, PedNodeType.Entry, 0, 5, 73, p1, p2, 0, 2, inputs.MinEntryHeadwayPed, inputs.EnteringFlowRatePed[0], inputs.PedArrivalDist);
                node.Add(NewEntryNode);
                PedNetwork.PedNodeIdList.Add(NewEntryNode.Id);

                //connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 15, 65, 0 };
                p2 = new double[] { 15, 81, 0 };
                UInt16[] U1 = { 1 };
                UInt16[] D1 = { 3 };
                double[] U2 = { 1 };
                double[] D2 = { 1 };
                PedLinkConnectorNode NewConnectorNode = new PedLinkConnectorNode(2, PedNodeType.LinkConnector, 1, 15, 73, p1, p2, U1, D1, U2, D2, 0, 1);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 65, 0 };
                p2 = new double[] { 65, 81, 0 };
                U1 = new UInt16[] { 2 };
                D1 = new UInt16[] { 6, 12, 9 };
                U2 = new double[] { 1 };
                D2 = new double[] { 1, 1, 1 };
                NewConnectorNode = new PedLinkConnectorNode(3, PedNodeType.LinkConnector, 2, 65, 73, p1, p2, U1, D1, U2, D2, 1, 2);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // entry node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 141, 0 };
                p2 = new double[] { 81, 141, 0 };
                NewEntryNode = new PedEntryNode(4, PedNodeType.Entry, 3, 73, 141, p1, p2, 3, 5, inputs.MinEntryHeadwayPed, inputs.EnteringFlowRatePed[0], inputs.PedArrivalDist);
                node.Add(NewEntryNode);
                PedNetwork.PedNodeIdList.Add(NewEntryNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 131, 0 };
                p2 = new double[] { 81, 131, 0 };
                U1 = new UInt16[] { 6 };
                D1 = new UInt16[] { 4 };
                U2 = new double[] { 1 };
                D2 = new double[] { 1 };
                NewConnectorNode = new PedLinkConnectorNode(5, PedNodeType.LinkConnector, 4, 73, 131, p1, p2, U1, D1, U2, D2, 4, 3);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 81, 0 };
                p2 = new double[] { 81, 81, 0 };
                U1 = new UInt16[] { 12, 9, 3 };
                D1 = new UInt16[] { 5 };
                U2 = new double[] { 1, 1, 1 };
                D2 = new double[] { 1 };
                NewConnectorNode = new PedLinkConnectorNode(6, PedNodeType.LinkConnector, 5, 73, 81, p1, p2, U1, D1, U2, D2, 2, 4);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // entry node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 5, 0 };
                p2 = new double[] { 81, 5, 0 };
                NewEntryNode = new PedEntryNode(7, PedNodeType.Entry, 6, 73, 5, p1, p2, 5, 8, inputs.MinEntryHeadwayPed, inputs.EnteringFlowRatePed[0], inputs.PedArrivalDist);
                node.Add(NewEntryNode);
                PedNetwork.PedNodeIdList.Add(NewEntryNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 15, 0 };
                p2 = new double[] { 81, 15, 0 };
                U1 = new UInt16[] { 7 };
                D1 = new UInt16[] { 9 };
                U2 = new double[] { 1 };
                D2 = new double[] { 1 };
                NewConnectorNode = new PedLinkConnectorNode(8, PedNodeType.LinkConnector, 7, 73, 15, p1, p2, U1, D1, U2, D2, 5, 6);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 65, 65, 0 };
                p2 = new double[] { 81, 65, 0 };
                U1 = new UInt16[] { 8 };
                D1 = new UInt16[] { 3, 6, 12 };
                U2 = new double[] { 1 };
                D2 = new double[] { 1, 1, 1 };
                NewConnectorNode = new PedLinkConnectorNode(9, PedNodeType.LinkConnector, 8, 73, 65, p1, p2, U1, D1, U2, D2, 6, 2);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // entry node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 141, 65, 0 };
                p2 = new double[] { 141, 81, 0 };
                NewEntryNode = new PedEntryNode(10, PedNodeType.Entry, 9, 141, 73, p1, p2, 7, 11, inputs.MinEntryHeadwayPed, inputs.EnteringFlowRatePed[0], inputs.PedArrivalDist);
                node.Add(NewEntryNode);
                PedNetwork.PedNodeIdList.Add(NewEntryNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 131, 65, 0 };
                p2 = new double[] { 131, 81, 0 };
                U1 = new UInt16[] { 12 };
                D1 = new UInt16[] { 10 };
                U2 = new double[] { 1 };
                D2 = new double[] { 1 };
                NewConnectorNode = new PedLinkConnectorNode(11, PedNodeType.LinkConnector, 10, 131, 73, p1, p2, U1, D1, U2, D2, 8, 7);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);

                // connector node
                PedNetwork.NumPedNodes++;
                p1 = new double[] { 81, 65, 0 };
                p2 = new double[] { 81, 81, 0 };
                U1 = new UInt16[] { 9, 3, 6 };
                D1 = new UInt16[] { 11 };
                U2 = new double[] { 1, 1, 1 };
                D2 = new double[] { 1 };
                NewConnectorNode = new PedLinkConnectorNode(12, PedNodeType.LinkConnector, 11, 81, 73, p1, p2, U1, D1, U2, D2, 2, 8);
                node.Add(NewConnectorNode);
                PedNetwork.PedNodeIdList.Add(NewConnectorNode.Id);


                //link obstacles
                List<PedLinkObstacle> Obs1 = new List<PedLinkObstacle>();
                p1 = new double[] { 5, 65, 0 };
                p2 = new double[] { 15, 65, 0 };
                PedLinkObstacle NewObs = new PedLinkObstacle(p1, p2);
                Obs1.Add(NewObs);
                p1 = new double[] { 5, 81, 0 };
                p2 = new double[] { 15, 81, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs1.Add(NewObs);

                List<PedLinkObstacle> Obs2 = new List<PedLinkObstacle>();
                p1 = new double[] { 15, 65, 0 };
                p2 = new double[] { 65, 65, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs2.Add(NewObs);
                p1 = new double[] { 15, 81, 0 };
                p2 = new double[] { 65, 81, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs2.Add(NewObs);

                List<PedLinkObstacle> Obs4 = new List<PedLinkObstacle>();
                p1 = new double[] { 65, 141, 0 };
                p2 = new double[] { 65, 131, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs4.Add(NewObs);
                p1 = new double[] { 81, 141, 0 };
                p2 = new double[] { 81, 131, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs4.Add(NewObs);

                List<PedLinkObstacle> Obs5 = new List<PedLinkObstacle>();
                p1 = new double[] { 65, 131, 0 };
                p2 = new double[] { 65, 81, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs5.Add(NewObs);
                p1 = new double[] { 81, 131, 0 };
                p2 = new double[] { 81, 81, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs5.Add(NewObs);

                List<PedLinkObstacle> Obs6 = new List<PedLinkObstacle>();
                p1 = new double[] { 65, 5, 0 };
                p2 = new double[] { 65, 15, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs6.Add(NewObs);
                p1 = new double[] { 81, 5, 0 };
                p2 = new double[] { 81, 15, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs6.Add(NewObs);

                List<PedLinkObstacle> Obs7 = new List<PedLinkObstacle>();
                p1 = new double[] { 65, 15, 0 };
                p2 = new double[] { 65, 65, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs7.Add(NewObs);
                p1 = new double[] { 81, 15, 0 };
                p2 = new double[] { 81, 65, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs7.Add(NewObs);

                List<PedLinkObstacle> Obs8 = new List<PedLinkObstacle>();
                p1 = new double[] { 141, 65, 0 };
                p2 = new double[] { 131, 65, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs8.Add(NewObs);
                p1 = new double[] { 141, 81, 0 };
                p2 = new double[] { 131, 81, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs8.Add(NewObs);

                List<PedLinkObstacle> Obs9 = new List<PedLinkObstacle>();
                p1 = new double[] { 131, 65, 0 };
                p2 = new double[] { 81, 65, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs9.Add(NewObs);
                p1 = new double[] { 131, 81, 0 };
                p2 = new double[] { 81, 81, 0 };
                NewObs = new PedLinkObstacle(p1, p2);
                Obs9.Add(NewObs);

                List<PedLinkObstacle> Obs3 = new List<PedLinkObstacle>(8);
                Obs3.AddRange(Obs2);
                Obs3.AddRange(Obs5);
                Obs3.AddRange(Obs7);
                Obs3.AddRange(Obs9);

                // link 1
                PedNetwork.NumPedLinks++;
                PedLinkData NewLink = new PedLinkData(0, node[0], node[1], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs1, 0);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                //link 2
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(1, node[1], node[2], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs2, 1);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 3
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(2, node[2], node[3], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs3, 2);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 4
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(3, node[0], node[1], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs4, 3);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                //link 5
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(4, node[1], node[2], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs5, 4);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 6
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(5, node[2], node[3], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs6, 5);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 7
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(6, node[0], node[1], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs7, 6);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                //link 8
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(7, node[1], node[2], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs8, 7);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);

                // link 9
                PedNetwork.NumPedLinks++;
                NewLink = new PedLinkData(8, node[2], node[3], inputs.LinkWidth[0], 0, 100, 0, 0, 0, 0, Obs9, 8);
                link.Add(NewLink);
                PedNetwork.PedLinkIdList.Add(NewLink.Id);
            }

        }
    }
}

