using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;

namespace wpfXbap
{
    /// <summary>
    /// Interaction logic for XYPlot.xaml
    /// </summary>
    public partial class XYPlot : Page
    {
        Line OX = new Line();
        Line OY = new Line();
        Polygon arrowX = new Polygon();
        Polygon arrowY = new Polygon();
        Series greedeSeries;
        
        int offsetX = 50;   //offset - lewy górny róg grafu
        int offsetY = 50;
        int width = 900;
        int height = 700;
        
        public XYPlot()
        {
            InitializeComponent();
            InitPlot();
        }
        public void InitPlot(){
            int copWin = 0;
            int robWin = 0;
            List<KeyValuePair<double, int>> greedySource = new List<KeyValuePair<double, int>>();
            List<KeyValuePair<int, int>> greedySource2 = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<string, int>> barGraphCop = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> barGraphRob = new List<KeyValuePair<string, int>>();
            if (Tests.outputGreedyDumb != null)
            {
                
                foreach (outputClass item in Tests.outputGreedyDumb)
                {
                    greedySource.Add(new KeyValuePair<double, int>((double)(((double)item.Moves / (double)item.MaxMoves)) * 100, item.Iterations));
                    greedySource2.Add(new KeyValuePair<int, int>(item.Moves , item.Iterations));
                    if(item.Winner.Equals("COP")) copWin++; else robWin++;
                }

                barGraphCop.Add(new KeyValuePair<string, int>("Greedy Dumb Cop", copWin));
                barGraphRob.Add(new KeyValuePair<string, int>("Greedy Dumb Rob", robWin));
            }
            ((ScatterSeries)chart1.Series[0]).ItemsSource = greedySource;
            ((ScatterSeries)chart2.Series[0]).ItemsSource = greedySource2;

            List<KeyValuePair<double, int>> greedyDijkstraSource = new List<KeyValuePair<double, int>>();
            List<KeyValuePair<int, int>> greedyDijkstraSource2 = new List<KeyValuePair<int, int>>();
            copWin = robWin = 0;
            if (Tests.outputGreedyDijkstra != null)
            {
                foreach (outputClass item in Tests.outputGreedyDijkstra)
                {
                    greedyDijkstraSource.Add(new KeyValuePair<double, int>((double)(((double)item.Moves / (double)item.MaxMoves)) * 100, item.Iterations));
                    greedyDijkstraSource2.Add(new KeyValuePair<int, int>(item.Moves, item.Iterations));
                    if (item.Winner.Equals("COP")) copWin++; else robWin++;
                }
                barGraphCop.Add(new KeyValuePair<string, int>("Greedy Dijkstra Cop", copWin));
                barGraphRob.Add(new KeyValuePair<string, int>("Greedy Dijkstra Rob", robWin));
            }
            ((ScatterSeries)chart1.Series[1]).ItemsSource = greedyDijkstraSource;
            ((ScatterSeries)chart2.Series[1]).ItemsSource = greedyDijkstraSource2;

            List<KeyValuePair<double, int>> beaconSource = new List<KeyValuePair<double, int>>();
            List<KeyValuePair<int, int>> beaconSource2 = new List<KeyValuePair<int, int>>();
            if (Tests.outputBeacon != null)
            {
                copWin = robWin = 0;
                foreach (outputClass item in Tests.outputBeacon)
                {
                    beaconSource.Add(new KeyValuePair<double, int>((double)(((double)item.Moves / (double)item.MaxMoves)) * 100, item.Iterations));
                    beaconSource2.Add(new KeyValuePair<int, int>(item.Moves, item.Iterations));
                    if (item.Winner.Equals("COP")) copWin++; else robWin++;
                }
                barGraphCop.Add(new KeyValuePair<string, int>("Beacon Cop", copWin));
                barGraphRob.Add(new KeyValuePair<string, int>("Beacon Rob", robWin));
            }
            ((ScatterSeries)chart1.Series[2]).ItemsSource = beaconSource;
            ((ScatterSeries)chart2.Series[2]).ItemsSource = beaconSource2;

            List<KeyValuePair<double, int>> alfaBetaSource = new List<KeyValuePair<double, int>>();
            List<KeyValuePair<int, int>> alfaBetaSource2 = new List<KeyValuePair<int, int>>();
            if (Tests.outputAlfaBeta != null)
            {
                copWin = robWin = 0;
                foreach (outputClass item in Tests.outputAlfaBeta)
                {
                    alfaBetaSource.Add(new KeyValuePair<double, int>((double)(((double)item.Moves / (double)item.MaxMoves)) * 100, item.Iterations));
                    alfaBetaSource2.Add(new KeyValuePair<int, int>(item.Moves, item.Iterations));
                    if (item.Winner.Equals("COP")) copWin++; else robWin++;
                }
                barGraphCop.Add(new KeyValuePair<string, int>("Alfa Beta Cop", copWin));
                barGraphRob.Add(new KeyValuePair<string, int>("Alfa Beta Rob", robWin));
            }
            ((ScatterSeries)chart1.Series[3]).ItemsSource = alfaBetaSource;
            ((ScatterSeries)chart2.Series[3]).ItemsSource = alfaBetaSource2;

            List<KeyValuePair<double, int>> MCTSSource = new List<KeyValuePair<double, int>>();
            List<KeyValuePair<int, int>> MCTSSource2 = new List<KeyValuePair<int, int>>();
            if (Tests.outputBeacon != null)
            {
                foreach (outputClass item in Tests.outputBeacon)
                {
                    copWin = robWin = 0;
                    MCTSSource.Add(new KeyValuePair<double, int>((double)(((double)item.Moves / (double)item.MaxMoves)) * 100, item.Iterations));
                    MCTSSource2.Add(new KeyValuePair<int, int>(item.Moves, item.Iterations));
                    if (item.Winner.Equals("COP")) copWin++; else robWin++;
                }
                barGraphCop.Add(new KeyValuePair<string, int>("MCTS Cop", copWin));
                barGraphRob.Add(new KeyValuePair<string, int>("MCTS Rob", robWin));
            }
            ((ScatterSeries)chart1.Series[4]).ItemsSource = MCTSSource;
            ((ScatterSeries)chart2.Series[4]).ItemsSource = MCTSSource2;
            ((BarSeries)chart3.Series[0]).ItemsSource = barGraphCop;
            ((BarSeries)chart3.Series[1]).ItemsSource = barGraphRob;
        }

        //private void DrawChart()
        //{
        //    #region axes
        //    OX.X1 = offsetX;
        //    OX.Y1 = offsetY+height;
        //    OX.X2 = offsetX + width;
        //    OX.Y2 = offsetY + height;

        //    OY.X1 = offsetX;
        //    OY.Y1 = offsetY;
        //    OY.X2 = offsetX;
        //    OY.Y2 = offsetY + height;

        //    OY.Stroke = OX.Stroke = System.Windows.Media.Brushes.Black;
        //    OY.StrokeThickness = OX.StrokeThickness = 3;

        //    canvas.Children.Add(OX);
        //    canvas.Children.Add(OY);
        //    #endregion

        //    #region arrows
        //    PointCollection c1 = new PointCollection();
        //    c1.Add(new Point(offsetX + width, offsetY + height));
        //    c1.Add(new Point(offsetX + width-5, offsetY + height-6));
        //    c1.Add(new Point(offsetX + width - 5, offsetY + height + 6));
        //    arrowX.Points = c1;

        //    PointCollection c2 = new PointCollection();
        //    c2.Add(new Point(offsetX, offsetY));
        //    c2.Add(new Point(offsetX - 6, offsetY + 5));
        //    c2.Add(new Point(offsetX + 6, offsetY + 5));
        //    arrowY.Points = c2;

        //    arrowX.Stroke = arrowY.Stroke = System.Windows.Media.Brushes.Black;
        //    arrowX.Fill = arrowY.Fill = System.Windows.Media.Brushes.Black ;
        //    arrowX.StrokeThickness = arrowY.StrokeThickness = 3;

        //    canvas.Children.Add(arrowX);
        //    canvas.Children.Add(arrowY);
        //    #endregion

        //    #region scale
        //    for (int i = 1; i != 11; i++)
        //    {
        //        Line tmp = new Line();
        //        tmp.X1 = offsetX - 4;
        //        tmp.X2 = offsetX + 4;
        //        tmp.Y1 = offsetY + (height / 10 * i);
        //        tmp.Y2 = offsetY + (height / 10 * i);

        //        Line tmp2 = new Line();
        //        tmp2.Y1 = offsetY + height + 4;
        //        tmp2.Y2 = offsetY + height - 4;
        //        tmp2.X1 = offsetX + (width / 10 * i);
        //        tmp2.X2 = offsetX + (width / 10 * i);

        //        tmp.Stroke = tmp2.Stroke = System.Windows.Media.Brushes.Black;
        //        tmp.StrokeThickness = tmp2.StrokeThickness = 3;

        //        canvas.Children.Add(tmp);
        //        canvas.Children.Add(tmp2);
        //    }
        //    #endregion          


        //}

        private void btnWyniki_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new Uri("Tests.xaml", UriKind.RelativeOrAbsolute));
            if (!Convert.ToBoolean(Application.Current.Properties["FromXYPlot"]))
                Application.Current.Properties.Add("FromXYPlot", true);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Remove("tTestNumber");
            Application.Current.Properties.Remove("tNodeNumberMax");
            Application.Current.Properties.Remove("tMaxNodeSt");
            Application.Current.Properties.Remove("tNodeNumberMin");
            Application.Current.Properties.Remove("tChbGreedy");
            Application.Current.Properties.Remove("tChbBeacon");
            Application.Current.Properties.Remove("tChbAlfaBeta");
            Application.Current.Properties.Remove("tTbCopNr");
            Application.Current.Properties.Remove("tTbCopNrMax");
            Application.Current.Properties.Remove("tTbRandNr");
            Application.Current.Properties.Remove("tTbgoOnTime");
            Application.Current.Properties.Remove("tTbAlfaDepthMin");
            Application.Current.Properties.Remove("tTbAlfaDepthMax");
            Application.Current.Properties.Remove("tTreeDepth");
            Application.Current.Properties.Remove("tTreeWidth");
            Application.Current.Properties.Remove("tChbMCTS");
            Application.Current.Properties.Remove("FromXYPlot");
            Application.Current.Properties.Remove("tAutoTest");
            NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
