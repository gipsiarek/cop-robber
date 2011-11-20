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



namespace wpfXbap
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserGame : Page 
    {
        Board board;
        Robber robber;
        Cop cop;
        Tree<Node<Data>> MCTSTree = null;
        int clickedElement, nodenumber;
        bool copTurn, robberPlaced, copPlaced;
        int gWidth, gHeight;
        string gType, gAlgorithm;

        public UserGame()
        {
            try
            {
                InitializeComponent();
                copTurn = true; robberPlaced = copPlaced = false;
                UserGame_OnLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #region FUNKCJE PODSTAWOWE
        private void UserGame_OnLoad()
        {
            checkboard.Children.Clear();
            gType = Convert.ToString(Application.Current.Properties["gType"]);

            gWidth = (gType == "koperta" || gType == "4-regularny c3" || gType == "dwunastoscian" || gType=="petersen") ? 0 : Convert.ToInt32(Application.Current.Properties["gWidth"]);

            gHeight = (gType == "koperta" || gType == "4-regularny c3" || gType == "dwunastoscian" || gType == "petersen") ? 0 : Convert.ToInt32(Application.Current.Properties["gHeight"]);
           
            gAlgorithm = Convert.ToString(Application.Current.Properties["gAlgorithm"]);
            nodenumber = gHeight * gWidth;
            board = new Board(gWidth, gHeight, gType, (int)checkboard.Width, (int)checkboard.Height);
            Board.removePitfalls(board);            
            foreach (List<int> tmp in board.neighborForClass)
            {
                if (tmp.Count != 0)
                {
                    board.isCopWinGraph = false;
                }
            }
            if (board.isCopWinGraph)
                lblKlasa.Text = "Cop-Win";
            else lblKlasa.Text = "Robber-Win";

            foreach (Node node in board.vertex)
            {
                foreach (int tmp in board.neighbor[node.number])
                {
                    Line linia = board.drawLine(node.number, tmp);
                    checkboard.Children.Add(linia);
                }
            }
            foreach (Node node in board.vertex)
                checkboard.Children.Add(node.elly);
        }
        #endregion

        #region EVENTS
        
        private void plansza_MouseDown(object sender, MouseButtonEventArgs e)
        {
         
            string elem = findElement(sender, e);
            if (elem == null || elem == "")
            { 
            }
            else if (elem != "" && elem.Substring(0, 3) == "nod")
            {
                clickedElement = Convert.ToInt32(elem.Substring(4, elem.Length - 4));
                if (copTurn)
                {
                    if (copPlaced)
                    {
                        if (cop.myNeighbors.Contains(clickedElement))
                        {
                            checkboard.Children.Remove(cop.myNode.elly);
                            cop.myNode.number = clickedElement;
                            cop.myNeighbors = board.findNeighbors(cop.myNode.number);
                            board.pointCop(findPoint(elem), cop.myNode);
                            checkboard.Children.Add(cop.myNode.elly);
                            RobberMove();

                        }
                        else
                        {
                            cop.myNeighbors = board.findNeighbors(cop.myNode.number);
                            MessageBox.Show("tak daleko nie dobiegnę...");
                        }
                    }
                    else
                    {
                        int tmp;
                        cop = new Cop(board, findPoint(elem, out tmp), tmp);
                        checkboard.Children.Add(cop.myNode.elly);
                        copPlaced = true;
                        copTurn = false;
                        lblTura.Text = "Złodziej";
                    }
                }
                else
                {
                    if (!robberPlaced)
                    {

                        int tmp;
                        robber = new Robber(board, findPoint(elem, out tmp), tmp);
                        checkboard.Children.Add(robber.myNode.elly);
                        robberPlaced = true;
                        copTurn = true;
                        lblTura.Text = "Gliniarz";
                    }
                }
            }
            else if (elem.Substring(0, 3) == "rob" || elem.Substring(0, 3) == "cop")
            {
                if (copTurn)
                {
                    if (cop.myNeighbors.Contains(robber.myNode.number))
                    {
                        gameEnd("cop");
                    }
                    else
                        MessageBox.Show("jeszcze za daleko");
                }
                else
                {
                    if (robber.myNeighbors.Contains(cop.myNode.number))
                    {
                        gameEnd("robber");
                    }
                    else
                        MessageBox.Show("no widze go i co?!");
                }
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Remove("gWidth");
            Application.Current.Properties.Remove("gHeight");
            Application.Current.Properties.Remove("gType");
            Application.Current.Properties.Remove("gAlgorithm");
            NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
        } 
        #endregion

        #region FUNKCJE DODATKOWE
        /// <summary>
        /// finds an element that was clicked
        /// </summary>
        /// <returns>name of clicked node</returns>
        private string findElement(object sender, MouseButtonEventArgs e)
        {
            Point pkt = e.GetPosition(checkboard);
            txbPozycjaX.Text = pkt.X.ToString();
            txbPozycjaY.Text = pkt.Y.ToString();
            FrameworkElement elem = (FrameworkElement)e.OriginalSource;
            txbNrElementu.Text = elem.Name;
            return elem.Name;
        }
   
        /// <summary>
        /// finds point to color from name of clicked node
        /// </summary>
         public Point findPoint(string nodeName, out int x)
        {
            Point chords = new Point();
            x = -1;
            string a = nodeName.Substring(3, nodeName.Length-4 );
            int nodeNumber = Convert.ToInt32(nodeName.Substring(4, nodeName.Length - 4));
            foreach (Node tmp in board.vertex)
            {
                if (tmp.number == nodeNumber)
                {
                    x = nodeNumber;
                    chords.X = tmp.x;
                    chords.Y = tmp.y;
                    break;
                }
            }
            return chords;
        }
         public Point findPoint(string nodeName)
         {
             Point chords = new Point();
             string a = nodeName.Substring(3, nodeName.Length - 4);
             int nodeNumber = Convert.ToInt32(nodeName.Substring(4, nodeName.Length - 4));
             foreach (Node tmp in board.vertex)
             {
                 if (tmp.number == nodeNumber)
                 {
                     chords.X = tmp.x;
                     chords.Y = tmp.y;
                     break;
                 }
             }
             return chords;
         }

         public void gameEnd(string who)
         {
             if (who == "cop")
             {
                 MessageBox.Show("Cop is the winner! robber is going to JAIL!!");
             }
             else
             {
                 MessageBox.Show("Robber cachs cop.... WTF?");
             }
             Application.Current.Properties.Remove("gWidth");
             Application.Current.Properties.Remove("gHeight");
             Application.Current.Properties.Remove("gType");
             Application.Current.Properties.Remove("gAlgorithm");
             NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
         }
        /// <summary>
        /// loop for manage rober
        /// </summary>
         public void RobberMove()
         {
             int nodetoGo = 0;

             robber.ocpupiedNode = robber.myNode.number;
             cop.ocupiedNode = cop.myNode.number;
             robber.myNeighbors = board.findNeighbors(robber.myNode.number);
             if(gAlgorithm.Equals("zachłanny")){
                 nodetoGo = Tests.robber_moves_greedy_dumb(cop, robber);
             } else if(gAlgorithm.Equals("alfa-beta")){
                 int value=-200, maxValue=200;
                 foreach (int item in robber.myNeighbors)
                 {
                     value = Tests.alphabeta(item, 3, -999, 999, true, cop.ocupiedNode, 3, board);
                     if (value > maxValue || value == 0)
                     {
                         nodetoGo = item;
                         maxValue = value;
                     }
                 }
             } else if(gAlgorithm.Equals("latarnie morskie")){
                 nodetoGo = Tests.robber_moves_randomBeacon(board, 10, 5, robber, cop);
             }
             else if (gAlgorithm.Equals("MCTS")) 
             {
                 nodetoGo = Tests.robber_moves_MCTS(MCTSTree, robber.ocpupiedNode, cop.ocupiedNode, 4, 3, board, out MCTSTree); //szerokośc drzewa 4 wysokość 3
             }
             else {       //zachłanny z Dijkstrą
                 nodetoGo = Tests.robber_moves_greedy_dijkstra(1, board, cop, robber);
             }


             List<int> away = new List<int>();
             
             checkboard.Children.Remove(robber.myNode.elly);
            
             robber.myNode.number = nodetoGo;
             string name = "node" + nodetoGo.ToString();
             board.pointRobber(findPoint(name), robber.myNode);
             checkboard.Children.Add(robber.myNode.elly);

         }

        
        #endregion

         
              
    }
}
