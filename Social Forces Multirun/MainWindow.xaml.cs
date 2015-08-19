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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Social_Forces_Main;
using Social_Forces_Analysis;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Social_Forces_Multirun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MultirunProperties Multirun=new MultirunProperties();
        int runs = 2;
        InputData Inputs = new InputData(ProjectType.Pedestrian,0,0,0);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoadScenarios_Click(object sender, RoutedEventArgs e)
        {
            double[] Flow1 = new double[6] { 5, 10, 20, 40, 70, 450 };
            double[] A = new double[9] { 2, 6, 10, 14, 18, 22, 26, 30, 34 };
            double[] B = new double[9] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5 };

            //double[] A = new double[1] { 24.22 };//2.25 m^2/s^2
            //double[] B = new double[1] { 1.15 };//0.35 m
            //double[] Flow1 = new double[2] { 40, 450 };

            Multirun = new MultirunProperties(Flow1, A, B);
            //lblStatus.Content = "Loaded";
            txtStatus.Text = "Loaded";
        }

        private void btnRunScenarios_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            txtStatus.Text = "Running";
            TimeSpan runtime;
            
            for (int i = 0; i < Multirun.Factor1; i++)
            {
                for (int j = 0; j < Multirun.Factor2; j++)
                {
                    for (int k = 0; k < Multirun.Factor3; k++)
                    {

                        for (int l = 1; l <= runs; l++)
                        {
                            txtStatus.Text = "Processing Run " + (i + 1).ToString() + "_" + ((j * 9) + k + 1).ToString() + "_" + l.ToString();
                            this.UpdateLayout();

                            if (!File.Exists("TSD_Ped_" + (i + 1).ToString() + "_" + ((j * 9) + k + 1).ToString() + "_" + l.ToString() + ".csv"))
                            {
                                try
                                {
                                    SimEngineMain.SimMain(i + 1, (j * 9) + k + 1, l, Multirun.Flow1[i] * 40,0, Multirun.A[j], Multirun.B[k]*2);
                                }
                                catch (System.ArgumentException ex)
                                {
                                    //throw new System.ArgumentException(ex + (i + 1).ToString() + "_" + ((j * 9) + k + 1).ToString() + "_" + l.ToString());
                                    //MessageBox.Show(ex.ToString() + (i + 1).ToString() + "_" + ((j * 9) + k + 1).ToString() + "_" + l.ToString());
                                }
                            }
                        }
                    }
                }
            }
            runtime = DateTime.Now - start;
            txtStatus.Text = "Runs Complete in " + runtime.ToString();
            btnAnalyzeData_Click(btnAnalyzeData, e);
        }

        private void btnAnalyzeData_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            for (int i = 0; i < Multirun.Factor1; i++)
            {
                for (int j = 0; j < Multirun.Factor2; j++)
                {
                    for (int k = 0; k < Multirun.Factor3; k++)
                    {
                        for (int l = 1; l <= runs; l++)
                        {
                            txtStatus.Text = "Analyzing Run " + (i + 1).ToString() + "_" + ((j * 9) + k + 1).ToString() + "_" + l.ToString();
                            this.UpdateLayout();
                            Social_Forces_Analysis.XML_Analysis Analysis = new XML_Analysis(i + 1, (j * 9) + k + 1, l, Multirun.Flow1[i] * 40, Multirun.A[j], Multirun.B[k]*2);
                        }
                    }
                }
            }
            TimeSpan runtime = DateTime.Now - start;
            txtStatus.Text = "Analysis Complete in " + runtime.ToString();
        }

        private void btnAnimation_Click(object sender, RoutedEventArgs e)
        {
            Animation AnimationWindow = new Animation();
            AnimationWindow.Show();
        }

        private void MenuOpenProject(object sender, RoutedEventArgs e)
        {
            //OpenTimingPlansFile("G:\\My Documents\\Projects\\CORSIM\\CORSIM NG\\SignalController\\SignalControl\\SignalController\\SignalControllerUI\\Resources\\Timing Plans-Sig2.xml");
            //OpenDetectorsFile("G:\\My Documents\\Projects\\CORSIM\\CORSIM NG\\SignalController\\SignalControl\\SignalController\\SignalControllerUI\\Resources\\Detectors-Sig2.xml");
            //OpenControlPointsFile("G:\\My Documents\\Projects\\CORSIM\\CORSIM NG\\SignalController\\SignalControl\\SignalController\\SignalControllerUI\\Resources\\VehicleControlPoints-Sig2.xml");


            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                FileStream myFileStream = new FileStream(dlg.FileName, FileMode.Open);

                // Create the XmlSerializer instance.
                XmlSerializer mySerializer = new XmlSerializer(typeof(InputData));

                InputData openInputs = (InputData)mySerializer.Deserialize(myFileStream);

                Inputs = openInputs;
                Inputs.Loaded = true;

                //Activate run buttons
                menuSingleRun.IsEnabled = true;
                menuNewMultirun.IsEnabled = true;
                if (!Multirun.Equals(new MultirunProperties()))
                {

                }
            }
        }

        private void MenuSaveProject(object sender, RoutedEventArgs e)
        {
            if (Inputs.Loaded)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "SocialForcesProject"; // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document

                    // Writing the file requires a TextWriter.
                    TextWriter myStreamWriter = new StreamWriter(dlg.FileName);

                    // Create the XmlSerializer instance.
                    XmlSerializer mySerializer = new XmlSerializer(typeof(InputData));

                    InputData saveProject = Inputs;

                    mySerializer.Serialize(myStreamWriter, saveProject);
                    myStreamWriter.Close();

                }
            }
            else
            {
                MessageBox.Show("No project is currently loaded.");
            }
        }

        

        private void MenuNewProject(object sender, RoutedEventArgs e)
        {
            Inputs = new InputData(ProjectType.Pedestrian, 400, 14, 6);
            List<PedLinkData> links = new List<PedLinkData>();
            List<PedNodeData> nodes = new List<PedNodeData>();
            PedNetworkData network = new PedNetworkData(0);
            NetworkTopologyPed.CreateNetworkTopology(Inputs, network,nodes, links);

            Inputs.Network = network;
            Inputs.Nodes = nodes;
            Inputs.Links = links;
            Inputs.Loaded = true;
        }

        private void MenuNewMultirun(object sender, RoutedEventArgs e)
        {
            double[] Flow1 = new double[6] { 5, 20, 70, 150, 300, 450 };
            double[] A = new double[9] { 2, 6, 10, 14, 18, 22, 26, 30, 34 };
            double[] B = new double[9] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5 };

            //double[] A = new double[1] { 24.22 };//2.25 m^2/s^2
            //double[] B = new double[1] { 1.15 };//0.35 m
            //double[] Flow1 = new double[2] { 40, 450 };

            Multirun = new MultirunProperties(Flow1, A, B);
        }

        private void MenuOpenMultirun(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSaveMultirun(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSingleRun(object sender, RoutedEventArgs e)
        {

        }

        private void MenuMultirun(object sender, RoutedEventArgs e)
        {

        }

        private void MenuAbout(object sender, RoutedEventArgs e)
        {

        }

        private void MenuMultirunAnalysis(object sender, RoutedEventArgs e)
        {

        }
    }
}
