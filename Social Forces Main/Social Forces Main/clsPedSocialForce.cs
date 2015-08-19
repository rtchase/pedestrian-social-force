using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Social_Forces_Main
{
    public class PedSocialForce
    {
        public PedSocialForce() { }

        public double[] PedForceCalc(PedestrianData Ped1, int TimeIndex, PedNodeData Dest1)//Calculate Desired Speed Forces
        {
            double ForceX = 0.0;
            double ForceY = 0.0;
            double ForceZ = 0.0;

            double[] DesDir = Ped1.PedDesDir(Ped1, Dest1, TimeIndex);
            ForceX = (1 / Ped1.RelaxTime) * (Ped1.DesiredSpeed * Ped1.DesiredDir[TimeIndex, 0] - Ped1.VelocityX[TimeIndex]);
            ForceY = (1 / Ped1.RelaxTime) * (Ped1.DesiredSpeed * Ped1.DesiredDir[TimeIndex, 1] - Ped1.VelocityY[TimeIndex]);
            ForceZ = (1 / Ped1.RelaxTime) * (Ped1.DesiredSpeed * Ped1.DesiredDir[TimeIndex, 2] - Ped1.VelocityZ[TimeIndex]);

            double[] Force = { ForceX, ForceY, ForceZ };
            return Force;
        }

        public double[] PedForceCalc(PedestrianData Ped1, PedestrianData Ped2, int TimeIndex, PedNodeData Dest1, PedNodeData Dest2)//Calculate Ped-Ped  Forces
        {
            double w = Ped1.AngularDepend + (1 - Ped1.AngularDepend) * ((1 + Ped1.PedPedAngle(Ped1, Ped2, TimeIndex)) / 2);

            double dx = Ped1.PositionX[TimeIndex] - Ped2.PositionX[TimeIndex];
            double dy = Ped1.PositionY[TimeIndex] - Ped2.PositionY[TimeIndex];
            double dz = Ped1.PositionZ[TimeIndex] - Ped2.PositionZ[TimeIndex];
            double[] DistVector = { dx, dy, dz };
            double[] UDistVector = Ped1.UnitVector(DistVector);
            DistVector[0] = DistVector[0] - (Ped1.Radius + Ped2.Radius) * UDistVector[0];
            DistVector[1] = DistVector[1] - (Ped1.Radius + Ped2.Radius) * UDistVector[1];
            DistVector[2] = DistVector[2] - (Ped1.Radius + Ped2.Radius) * UDistVector[2];
            double dt = 0.1; //Time Step in seconds

            if (DistVector[0].Equals(double.NaN)) //End run if any overlap or error in position calc occurs
            {
                throw new System.ArgumentException();
            }

            double[] y = { (Ped2.VelocityX[TimeIndex] - Ped1.VelocityX[TimeIndex]) * dt, (Ped2.VelocityY[TimeIndex] - Ped1.VelocityY[TimeIndex]) * dt, (Ped2.VelocityZ[TimeIndex] - Ped1.VelocityZ[TimeIndex]) * dt };
            double[] dvy = { DistVector[0] - y[0], DistVector[1] - y[1], DistVector[2] - y[2] };

            double b = Math.Sqrt(Math.Pow((Mag(DistVector)) + Mag(dvy), 2) - Math.Pow(Mag(y), 2)) / 2;
            if (b==0)
            { b = 0.001; }
            double FScalar = Ped1.InteractionStrength * Math.Exp(-b / Ped1.InteractionRange) * (Mag(DistVector) + Mag(dvy)) / (2 * b);
            double[] FVector = { (Ped1.UnitVector(DistVector)[0] + Ped1.UnitVector(dvy)[0]) / 2, (Ped1.UnitVector(DistVector)[1] + Ped1.UnitVector(dvy)[1]) / 2, (Ped1.UnitVector(DistVector)[2] + Ped1.UnitVector(dvy)[2]) / 2 };

            double[] Force = { w * FScalar * FVector[0], w * FScalar * FVector[1], w * FScalar * FVector[2] };

            //if ((TimeIndex == 1083) && Ped1.Id == 1)
            //{
            //    ;
            //}

            return Force;
        }

        public double[] PedForceCalc(PedestrianData Ped1, int TimeIndex, PedNodeData Dest1, PedLinkData Link)//Calculate Obstacle Forces
        {
            double[] Force = new double[3] { 0, 0, 0 };

            //ped signal
            //if (Ped1.CurrentLink[TimeIndex] == 2 || Ped1.CurrentLink[TimeIndex] == 8)
            //{
            //    List<PedLinkObstacle> Signals = new List<PedLinkObstacle>();
            //    //double[] p1 = new double[] { 475, 250, 0 };
            //    //double[] p2 = new double[] { 483, 250, 0 };
            //    double[] p1 = new double[] { 460, 250, 0 };
            //    double[] p2 = new double[] { 503, 250, 0 };
            //    PedLinkObstacle Obs = new PedLinkObstacle(p1, p2);
            //    Signals.Add(Obs);
            //    //p1 = new double[] { 475, 298, 0 };
            //    //p2 = new double[] { 483, 298, 0 };
            //    p1 = new double[] { 460, 298, 0 };
            //    p2 = new double[] { 503, 298, 0 };
            //    Obs = new PedLinkObstacle(p1, p2);
            //    Signals.Add(Obs);
            //    if (Mag(ObsDistVector(Ped1, TimeIndex, Signals[0])) > 1 && Mag(ObsDistVector(Ped1, TimeIndex, Signals[1])) > 1)
            //    {
            //        for (int i = 0; i <= 7; i++)
            //        {
            //            if ((i + 1) * 400 > TimeIndex && i * 400 <= TimeIndex)
            //            {
            //                if ((40 * i + 24) * 10 <= TimeIndex && (40 * i + 29) * 10 > TimeIndex)
            //                {
            //                    //signal is green
            //                }
            //                else //signal is yellow or red
            //                {



            //                    for (int j = 0; j <= 1; j++)
            //                    {
            //                        Obs = Signals[j];
            //                        double[] DistVector = ObsDistVector(Ped1, TimeIndex, Obs);
            //                        double[] UDistVector = Ped1.UnitVector(DistVector);
            //                        DistVector[0] = DistVector[0] - Ped1.Radius * UDistVector[0];
            //                        DistVector[1] = DistVector[1] - Ped1.Radius * UDistVector[1];
            //                        DistVector[2] = DistVector[2] - Ped1.Radius * UDistVector[2];

            //                        //double w = Ped1.AngularDepend + (1 - Ped1.AngularDepend) * ((1 + (Ped1.DesiredDir[TimeIndex, 0] * UDistVector[0] + Ped1.DesiredDir[TimeIndex, 1] * UDistVector[1] + Ped1.DesiredDir[TimeIndex, 2] * UDistVector[2])) / 2);
            //                        double w = 1;//Obstacles considered in all directions equally

            //                        double dt = 0.1; //Time Step in seconds

            //                        double[] y = { (-Ped1.VelocityX[TimeIndex]) * dt, (-Ped1.VelocityY[TimeIndex]) * dt, (-Ped1.VelocityZ[TimeIndex]) * dt };
            //                        double[] dvy = { DistVector[0] - y[0], DistVector[1] - y[1], DistVector[2] - y[2] };

            //                        double b = Math.Sqrt(Math.Pow((Mag(DistVector)) + Mag(dvy), 2) - Math.Pow(Mag(y), 2)) / 2;
            //                        double FScalar = Ped1.InteractionStrength * Math.Exp(-b / Ped1.InteractionRange) * (Mag(DistVector) + Mag(dvy)) / (2 * b);
            //                        //double[] FVector = { (Ped1.UnitVector(DistVector)[0] + Ped1.UnitVector(dvy)[0]) / 2, (Ped1.UnitVector(DistVector)[1] + Ped1.UnitVector(dvy)[1]) / 2, (Ped1.UnitVector(DistVector)[2] + Ped1.UnitVector(dvy)[2]) / 2 };
            //                        double[] FVector = UDistVector;//force perpendicular to obstacle
            //                        Force[0] = Force[0] + w * FScalar * FVector[0];//reversed
            //                        Force[1] = Force[1] + w * FScalar * FVector[1];
            //                        Force[2] = Force[2] + w * FScalar * FVector[2];
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            for (int i = 0; i <= Link.Obstacles.Count - 1; i++)
            {
                PedLinkObstacle Obs = Link.Obstacles[i];
                double[] DistVector = ObsDistVector(Ped1, TimeIndex, Obs);
                double[] UDistVector = Ped1.UnitVector(DistVector);
                DistVector[0] = DistVector[0] - Ped1.Radius * UDistVector[0];
                DistVector[1] = DistVector[1] - Ped1.Radius * UDistVector[1];
                DistVector[2] = DistVector[2] - Ped1.Radius * UDistVector[2];

                double w = Ped1.AngularDepend + (1 - Ped1.AngularDepend) * ((1 + (Ped1.DesiredDir[TimeIndex, 0] * UDistVector[0] + Ped1.DesiredDir[TimeIndex, 1] * UDistVector[1] + Ped1.DesiredDir[TimeIndex, 2] * UDistVector[2])) / 2);
                //double w = 1;//Obstacles considered in all directions equally

                double dt = 0.1; //Time Step in seconds

                double[] y = { (-Ped1.VelocityX[TimeIndex]) * dt, (-Ped1.VelocityY[TimeIndex]) * dt, (-Ped1.VelocityZ[TimeIndex]) * dt };
                double[] dvy = { DistVector[0] - y[0], DistVector[1] - y[1], DistVector[2] - y[2] };

                double b = Math.Sqrt(Math.Pow((Mag(DistVector)) + Mag(dvy), 2) - Math.Pow(Mag(y), 2)) / 2;
                double FScalar = Ped1.InteractionStrength * Math.Exp(-b / Ped1.InteractionRange) * (Mag(DistVector) + Mag(dvy)) / (2 * b);
                //double[] FVector = { (Ped1.UnitVector(DistVector)[0] + Ped1.UnitVector(dvy)[0]) / 2, (Ped1.UnitVector(DistVector)[1] + Ped1.UnitVector(dvy)[1]) / 2, (Ped1.UnitVector(DistVector)[2] + Ped1.UnitVector(dvy)[2]) / 2 };
                double[] FVector = UDistVector;//force perpendicular to obstacle
                Force[0] = Force[0] + w * FScalar * FVector[0];//reversed
                Force[1] = Force[1] + w * FScalar * FVector[1];
                Force[2] = Force[2] + w * FScalar * FVector[2];

                if (Ped1.Id == 1 && Link.Id==2)
                {
                    ;
                }
            }


            return Force;
        }

        public double Mag(double[] vector)
        {
            return Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2) + Math.Pow(vector[2], 2));
        }

        private double[] ObsDistVector(PedestrianData Ped1, int TimeIndex, PedLinkObstacle Obs)
        {
            double dx = Obs.Point1[0] - Obs.Point2[0];
            double dy = Obs.Point1[1] - Obs.Point2[1];
            double dz = Obs.Point1[2] - Obs.Point2[2];
            double[] vector = { 0, 0, 0 };

            if (dx == 0 && dy == 0 && dz == 0)
            {
                vector[0] = Ped1.PositionX[TimeIndex] - Obs.Point2[0];
                vector[1] = Ped1.PositionY[TimeIndex] - Obs.Point2[1];
                vector[2] = Ped1.PositionZ[TimeIndex] - Obs.Point2[2];

                return vector;
            }

            // Calculate the t that minimizes the distance.
            double t = ((Ped1.PositionX[TimeIndex] - Obs.Point2[0]) * dx + (Ped1.PositionY[TimeIndex] - Obs.Point2[1]) * dy + (Ped1.PositionZ[TimeIndex] - Obs.Point2[2]) * dz) / (dx * dx + dy * dy + dz * dz);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                vector[0] = Ped1.PositionX[TimeIndex] - Obs.Point2[0];
                vector[1] = Ped1.PositionY[TimeIndex] - Obs.Point2[1];
                vector[2] = Ped1.PositionZ[TimeIndex] - Obs.Point2[2];
            }
            else if (t > 1)
            {
                vector[0] = Ped1.PositionX[TimeIndex] - Obs.Point1[0];
                vector[1] = Ped1.PositionY[TimeIndex] - Obs.Point1[1];
                vector[2] = Ped1.PositionZ[TimeIndex] - Obs.Point1[2];
            }
            else
            {
                vector[0] = Ped1.PositionX[TimeIndex] - (Obs.Point2[0] + t * dx);
                vector[1] = Ped1.PositionY[TimeIndex] - (Obs.Point2[1] + t * dy);
                vector[2] = Ped1.PositionZ[TimeIndex] - (Obs.Point2[2] + t * dz);
            }

            return vector;
        }
    }
}
