using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Social_Forces_Main
{
    class OutputPedTSD
    {

        public static void WriteTSDfile3(PedNetworkData network, List<PedestrianData> Peds, List<PedLinkData> PedLinks, int numTimeSteps, double[] simTime, int[] run, PedEntryNode entryNode)
        {
            string filename;


            filename = "TSD_ped_" + run[0].ToString() + "_" + run[1].ToString() + "_" + run[2].ToString() + ".csv";

            StreamWriter sw = new StreamWriter(filename);   //StreamWriter(Sim.BaseDirectory & ProjTitle & "_TSD_" & cycle & "_Dir_" & Vehdir & ".txt")

            sw.Write("SimTime, Ped Index, PositionX, PositionY, PositionZ, VelocityX, VelocityY, VelocityZ, AccelX, AccelY, AccelZ, Current Link, Destination Node, Sys Entry Time, Sys Exit Time");
            sw.WriteLine();

            for (int PedIndex = 1; PedIndex < Peds.Count; PedIndex++)
            {
                for (int TimeIndex = Peds[PedIndex].SystemEntryTime; TimeIndex < Peds[PedIndex].SystemExitTime; TimeIndex++)
                {
                    //if (simTime[TimeIndex] >= vehs[VehIndex].SysEntryTime && simTime[TimeIndex] <= vehs[VehIndex].SysExitTime)
                    if (Peds[PedIndex].IsInNetwork[TimeIndex] == true)
                    {
                        sw.Write(simTime[TimeIndex]);
                        sw.Write(",");
                        sw.Write(PedIndex);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].PositionX[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].PositionY[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].PositionZ[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].VelocityX[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].VelocityY[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].VelocityZ[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].AccelX[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].AccelY[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].AccelZ[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].CurrentLink[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].DestinationNode[TimeIndex]);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].SystemEntryTime);
                        sw.Write(",");
                        sw.Write(Peds[PedIndex].SystemExitTime);
                        sw.WriteLine();
                    }

                }

            }
            sw.Close();

            if (!File.Exists("PedMetadata.csv"))
            {
                File.WriteAllText("PedMetadata.csv", "Scenario,Subscenario,Run,Unserved Queue");
            }
            using (StreamWriter sw2 = File.AppendText("PedMetadata.csv"))
            {
                sw2.WriteLine(run[0].ToString() + "," + run[1].ToString() + "," + run[2].ToString() + "," + entryNode.UnservedPedEntries.ToString());

            }
            
            //File.AppendAllText("PedMetadata.csv", run[0].ToString() + "," + run[1].ToString() + "," + run[2].ToString() + "," + entryNode.UnservedPedEntries);
        }

        public static void WritePedXML(List<PedestrianData> Peds, int[] run, PedEntryNode entryNode)
        {
            TextWriter myStreamWriter = new StreamWriter("TSD_ped_" + run[0].ToString() + "_" + run[1].ToString() + "_" + run[2].ToString() + ".xml");
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<PedestrianData>));
            mySerializer.Serialize(myStreamWriter, Peds);
            myStreamWriter.Close();

            if (!File.Exists("PedMetadata.csv"))
            {
                File.WriteAllText("PedMetadata.csv", "Scenario,Subscenario,Run,Unserved Queue");
            }

            File.AppendAllText("PedMetadata.csv", run[0].ToString() + "," + run[1].ToString() + "," + run[2].ToString() + "," + entryNode.UnservedPedEntries);
        }
    }
}
