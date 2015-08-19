using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Threading;

namespace Social_Forces_Multirun
{
    /// <summary>
    /// Interaction logic for Animation.xaml
    /// </summary>
    public partial class Animation : Window
    {

        private List<SimplePedData> Peds = new List<SimplePedData>();
        private DispatcherTimer timer = new DispatcherTimer();
        int TimeStep = 0;
        bool forward = true;
        bool recording = false;
        
        public Animation()
        {
            InitializeComponent();

            LoadPedData();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += timer_Tick;
            //myPanel.Margin = new Thickness(10);
            //myCanvas.Margin=new Thickness(10);

            //Rectangle myRectangle = new Rectangle();
            //myRectangle.Name = "myRectangle";
            //this.RegisterName(myRectangle.Name, myRectangle);
            //myRectangle.Width = 100;
            //myRectangle.Height = 100;
            //myRectangle.Fill = Brushes.Blue;
            //myRectangle.VerticalAlignment = VerticalAlignment.Center;


            //myCanvas.Children.Add(myRectangle);
            //Canvas.SetLeft(myRectangle, 200);

            btnPause.Click += btnPause_Click;
            btnPlay.Click += btnPlay_Click;
            btnReverse.Click += btnReverse_Click;
            sldSpeed.ValueChanged += sldSpeed_ValueChanged;


            // Use the Loaded event to start the Storyboard.

            //this.Content = myCanvas;
        }

        void sldSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //timer.Stop();
            double x = Math.Pow(2, (sldSpeed.Value - 4));
            timer.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(100 / x));
            lblSlider.Content = "Speed: " + x.ToString("0.00") + "X";
            //timer.Start();
        }



        void timer_Tick(object sender, EventArgs e)
        {

            if (forward)
            {
                if (TimeStep >= 3000)
                {
                    timer.Stop();

                }
                else
                {
                    TimeStep++;
                    foreach (SimplePedData ped in Peds)
                    {
                        if (ped.Entry > TimeStep || ped.Exit < TimeStep)
                            continue;
                        else
                        {
                            if (ped.Entry == TimeStep)
                            {

                                Canvas.SetLeft(ped.Circle, 10 + ped.X[TimeStep] * 10-ped.Circle.Height/2);
                                Canvas.SetTop(ped.Circle, 310 - ped.Y[TimeStep] * 10 - ped.Circle.Height / 2);
                                myCanvas.Children.Add(ped.Circle);
                                continue;
                            }
                            else
                            {
                                if (ped.Exit == TimeStep)
                                {
                                    myCanvas.Children.Remove(ped.Circle);
                                    continue;
                                }
                                else
                                {
                                    Canvas.SetLeft(ped.Circle, 10 + ped.X[TimeStep] * 10 - ped.Circle.Height / 2);
                                    Canvas.SetTop(ped.Circle, 310 - ped.Y[TimeStep] * 10 - ped.Circle.Height / 2);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (TimeStep <= 0)
                {
                    timer.Stop();

                }
                else
                {
                    TimeStep--;
                    foreach (SimplePedData ped in Peds)
                    {
                        if (ped.Entry > TimeStep || ped.Exit < TimeStep)
                            continue;
                        else
                        {
                            if (ped.Exit == TimeStep)
                            {
                                myCanvas.Children.Add(ped.Circle);
                                Canvas.SetLeft(ped.Circle, 10 + ped.X[TimeStep] * 10 - ped.Circle.Height / 2);
                                Canvas.SetTop(ped.Circle, 310 - ped.Y[TimeStep] * 10 - ped.Circle.Height / 2);
                                //ped.Circle.Visibility = System.Windows.Visibility.Visible;
                                continue;
                            }
                            else
                            {
                                if (ped.Entry == TimeStep)
                                {
                                    myCanvas.Children.Remove(ped.Circle);
                                    continue;
                                }
                                else
                                {
                                    Canvas.SetLeft(ped.Circle, 10 + ped.X[TimeStep] * 10 - ped.Circle.Height / 2);
                                    Canvas.SetTop(ped.Circle, 310 - ped.Y[TimeStep] * 10 - ped.Circle.Height / 2);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            lblTime.Content = "SimTime: " + Convert.ToString(Convert.ToInt16(TimeStep / 10)) + "." + Convert.ToString(TimeStep - Convert.ToInt16(TimeStep / 10) * 10);
        }

        void btnReverse_Click(object sender, RoutedEventArgs e)
        {
            if (forward)
                btnReverse.Content = "Forward";
            else btnReverse.Content = "Reverse";
            forward = !forward;
        }

        void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        void btnPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }



        private void LoadPedData()
        {
            bool loaded = false;

            while (!loaded)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".csv"; // Default file extension
                dlg.Filter = "CSV documents (.csv)|*.csv"; // Filter files by extension
                dlg.Multiselect = false;

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    Peds.Add(new SimplePedData(0));
                    Peds[0].Entry = 3002;
                    string[] InputData = File.ReadAllLines(dlg.FileName);
                    foreach (string dataLine in InputData)
                    {
                        string[] data = dataLine.Split(',');
                        if (data[0] != "SimTime")
                        {
                            int timestep = Convert.ToInt16(Convert.ToDouble(data[0]) * 10);
                            if (Convert.ToInt16(data[1]) == Peds[Peds.Count() - 1].Id)
                            {
                                Peds[Peds.Count() - 1].X[timestep] = Convert.ToDouble(data[2]);
                                Peds[Peds.Count() - 1].Y[timestep] = Convert.ToDouble(data[3]);
                            }
                            else
                            {
                                SimplePedData newPed = new SimplePedData(Convert.ToInt16(data[1]));
                                newPed.Entry = Convert.ToInt16(data[13]);
                                newPed.Exit = Convert.ToInt16(data[14]);
                                Canvas.SetLeft(newPed.Circle, 10 + Convert.ToDouble(data[2]) * 10 - newPed.Circle.Height / 2);
                                Canvas.SetTop(newPed.Circle, 310 - Convert.ToDouble(data[3]) * 10 - newPed.Circle.Height / 2);
                                Peds.Add(newPed);
                            }
                        }
                    }

                    loaded = true;
                }
                else
                {
                    MessageBox.Show("File Missing");
                }
            }
        }


    }
}