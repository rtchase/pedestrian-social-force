using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Forces_Main;
using System.IO;

namespace Social_Forces_Analysis
{
    public class XML_Analysis
    {
        double TravelTime = 0;
        double TravelDistance = 0;
        double TravelTime1 = 0;
        double TravelDistance1 = 0;
        double TravelTime2 = 0;
        double TravelDistance2 = 0;
        double TravelTime3 = 0;
        double TravelDistance3 = 0;
        double TravelTime4 = 0;
        double TravelDistance4 = 0;
        double TravelTime5 = 0;
        double TravelDistance5 = 0;
        double TravelTime6 = 0;
        double TravelDistance6 = 0;
        double TravelTime7 = 0;
        double TravelDistance7 = 0;
        double TravelTime8 = 0;
        double TravelDistance8 = 0;
        List<double> X1 = new List<double>() { 50, 60, 50, 50, 50, 52, 60, 60 };
        List<double> X2 = new List<double>() { 58, 68, 54, 54, 58, 60, 64, 64 };
        List<double> Y1 = new List<double>() { 6, 6, 6, 10, -10, -10, 6, 10 };
        List<double> Y2 = new List<double>() { 14, 14, 10, 14, 30, 30, 10, 14 };
        List<double> TimeStep = new List<double>();
        List<double> ID = new List<double>();
        List<double> PosX = new List<double>();
        List<double> PosY = new List<double>();
        List<double> SysEntry = new List<double>();
        List<double> SysExit = new List<double>();
        int errors = 0;
        int extreme = 0;

        public XML_Analysis(int scenario, int subscenario, int run, double demand, double a, double b)
        {
            if (File.Exists("TSD_Ped_" + scenario.ToString() + "_" + subscenario.ToString() + "_" + run.ToString() + ".csv"))
            {
                PedSocialForce SF = new PedSocialForce();

                string[] InputData = File.ReadAllLines("TSD_Ped_" + scenario.ToString() + "_" + subscenario.ToString() + "_" + run.ToString() + ".csv");
                foreach (string dataLine in InputData)
                {
                    string[] data = dataLine.Split(',');
                    if (data[0] != "SimTime")
                    {
                        TimeStep.Add(Convert.ToDouble(data[0]));
                        ID.Add(Convert.ToDouble(data[1]));
                        PosX.Add(Convert.ToDouble(data[2]));
                        PosY.Add(Convert.ToDouble(data[3]));
                        SysEntry.Add(Convert.ToDouble(data[13]));
                        SysExit.Add(Convert.ToDouble(data[14]));
                        //if(SF.Mag(Convert.ToDouble(data[2]))>90)
                        //{
                        //    extreme++;
                        //}
                    }
                }

                double[] TT = new double[9];
                double[] TD = new double[9];

                //for (int j = 1; j < PosY.Count(); j++)
                //{
                //    if (TimeStep[j] > 60)
                //    {
                //        if (ID[j] == ID[j - 1])
                //        {
                //            TravelTime += 0.1;
                //            TravelDistance += PosX[j] - PosX[j - 1];
                //        }
                //        else
                //        {
                //            //TravelDistance.Add(0);
                //            //TravelTime.Add(0);
                //        }
                //    }
                //}

                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j < PosY.Count() - 1; j++)
                    {
                        if (ID[j - 1] != ID[j]&&PosX[j-1]<61)
                        {
                            errors++;
                        }

                        if (TimeStep[j] > 120 && ID[j - 1] == ID[j])
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                if (PosX[j] >= X1[k] && PosY[j] >= Y1[k])
                                {
                                    if (PosX[j] < X2[k] && PosY[j] < Y2[k])
                                    {
                                        if (PosX[j - 1] >= X1[k] && PosY[j - 1] >= Y1[k] && PosX[j - 1] <= X2[k] && PosY[j - 1] <= Y2[k])
                                        {
                                            TT[k + 1] += 0.1;
                                            TD[k + 1] += (PosX[j] - PosX[j - 1]);
                                        }
                                        else
                                        {
                                            if (PosX[j - 1] < X1[k])
                                            {
                                                TD[k + 1] += PosX[j] - X1[k];
                                                TT[k + 1] += Math.Abs(0.1 * (PosX[j] - X1[k]) / (PosX[j] - PosX[j - 1]));
                                            }
                                            else
                                            {
                                                if (PosX[j - 1] > X2[k])
                                                {
                                                    TD[k + 1] -= X2[k] - PosX[j];
                                                    TT[k + 1] += Math.Abs(0.1 * (X2[k] - PosX[j]) / (PosX[j] - PosX[j - 1]));
                                                }
                                                else
                                                {
                                                    if (PosY[j - 1] > Y2[k])
                                                    {
                                                        double dy = Math.Abs((Y2[k] - PosY[j]) / (PosY[j - 1] - PosY[j]));
                                                        TD[k + 1] += (PosX[j] - PosX[j - 1]) * dy;
                                                        TT[k + 1] += dy * 0.1;
                                                    }
                                                    if (PosY[j - 1] < Y1[k])
                                                    {
                                                        double dy = Math.Abs((PosY[j] - Y1[k]) / (PosY[j] - PosY[j - 1]));
                                                        TD[k + 1] += (PosX[j] - PosX[j - 1]) * dy;
                                                        TT[k + 1] += dy * 0.1;
                                                    }
                                                }
                                            }
                                        }

                                        if (PosX[j + 1] < X1[k] || PosY[j + 1] < Y1[k] || PosX[j + 1] > X2[k] || PosY[j + 1] > Y2[k])
                                        {
                                            if (PosX[j + 1] < X1[k])
                                            {
                                                TD[k + 1] -= PosX[j] - X1[k];
                                                TT[k + 1] += Math.Abs(0.1 * (PosX[j] - X1[k]) / (PosX[j] - PosX[j + 1]));
                                            }
                                            else
                                            {
                                                if (PosX[j + 1] > X2[k])
                                                {
                                                    TD[k + 1] += X2[k] - PosX[j];
                                                    TT[k + 1] += Math.Abs(0.1 * (X2[k] - PosX[j]) / (PosX[j] - PosX[j + 1]));
                                                }
                                                else
                                                {
                                                    if (PosY[j + 1] > Y2[k])
                                                    {
                                                        double dy = Math.Abs((Y2[k] - PosY[j]) / (PosY[j + 1] - PosY[j]));
                                                        TD[k + 1] += (PosX[j + 1] - PosX[j]) * dy;
                                                        TT[k + 1] += dy * 0.1;
                                                    }
                                                    if (PosY[j + 1] < Y1[k])
                                                    {
                                                        double dy = Math.Abs((PosY[j] - Y1[k]) / (PosY[j] - PosY[j + 1]));
                                                        TD[k + 1] += (PosX[j + 1] - PosX[j]) * dy;
                                                        TT[k + 1] += dy * 0.1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //TravelTime = TT[0];
                //TravelDistance = TD[0];
                //TravelTime1 = TT[1];
                //TravelDistance1 = TD[1];
                //TravelTime2 = TT[2];
                //TravelDistance2 = TD[2];
                //TravelTime3 = TT[3];
                //TravelDistance3 = TD[3];
                //TravelTime4 = TT[4];
                //TravelDistance4 = TD[4];
                //TravelTime5 = TT[5];
                //TravelDistance5 = TD[5];
                //TravelTime6 = TT[6];
                //TravelDistance6 = TD[6];
                //TravelTime7 = TT[7];
                //TravelDistance7 = TD[7];
                //TravelTime8 = TT[8];
                //TravelDistance8 = TD[8];

                if (!File.Exists("PedAnalysis.csv"))
                {
                    File.WriteAllText("PedAnalysis.csv", "Demand, A, B, Run, Ped Entries,Analysis Region, Speed, Density, Flow");
                }
                using (StreamWriter sw2 = File.AppendText("PedAnalysis.csv"))
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        sw2.WriteLine();
                        sw2.Write(demand.ToString() + "," + a.ToString() + "," + b.ToString() + "," + run.ToString() + "," + ID[ID.Count() - 1].ToString() + "," + i.ToString() + "," + (TD[i] / TT[i]).ToString() + "," + (TT[i] / ((8 * 8) * 240)).ToString() + "," + (TD[i] / ((8 * 8) * 240)).ToString());
                    }
                }
            }
        }
    }
}
