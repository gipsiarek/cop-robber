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
        int clickedElement, nodenumber;
        bool copTurn, robberPlaced, copPlaced;
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
            int gWidth, gHeight;
            string gType;
            gWidth = Convert.ToInt32(Application.Current.Properties["gWidth"]);
            gHeight = Convert.ToInt32(Application.Current.Properties["gHeight"]);
            gType = Convert.ToString(Application.Current.Properties["gType"]);
            nodenumber = gHeight * gWidth;
            board = new Board(gWidth, gHeight, gType);


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
             NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
         }

         public void RobberMove()
         {
             List<int> away = new List<int>();
             int currentPath = 0, bestPath = 0, nodetoGo = 0;
             checkboard.Children.Remove(robber.myNode.elly);
             robber.myNeighbors = board.findNeighbors(robber.myNode.number);

             foreach (int item in robber.myNeighbors)
             {
                 if (item != cop.myNode.number)
                 {
                     away = Dijkstra(robber.myNode.number, item, cop.myNode.number);
                     currentPath = away.Count();
                     if (bestPath < currentPath)
                     {
                         bestPath = currentPath;
                         nodetoGo = away[0];
                     }
                 }
             }
             robber.myNode.number = nodetoGo;
             string name = "node" + nodetoGo.ToString();
             board.pointRobber(findPoint(name), robber.myNode);
             checkboard.Children.Add(robber.myNode.elly);

         }
         public List<int> Dijkstra(int from, int startNode, int finalNode)
         {
             List<int> path = new List<int>();
             List<List<int>> visited = new List<List<int>>(nodenumber);
             for (int i = 0; i < nodenumber; i++)
             {
                 visited.Add(new List<int>());
             }
             List<int> neighbors = board.findNeighbors(startNode);
             Queue<int> q = new Queue<int>(nodenumber);
             int tmpNode;
             visited[startNode].Add(from);
             foreach (int i in neighbors)
             {
                 if (i == finalNode)
                 {
                     path.Add(startNode);
                     path.Add(finalNode);
                     return path;
                 }
                 else
                 {
                     q.Enqueue(i);
                     visited[i].Add(startNode);
                 }
             }
             while (q.Count != 0)
             {
                 tmpNode = q.Dequeue();
                 neighbors = board.findNeighbors(tmpNode);
                 foreach (int node in neighbors)
                 {
                     if (node == finalNode)
                     {
                         q.Enqueue(node);
                         visited[node].Add(tmpNode);
                         createPathFromQueue(visited, out path, node, startNode);
                         return path;
                     }
                     else
                     {
                         if (visited[node].Count == 0)
                         {
                             q.Enqueue(node);
                             visited[node].Add(tmpNode);
                         }
                     }
                 }
             }

             return path;
         }
         private void createPathFromQueue(List<List<int>> visited, out List<int> path, int node, int startNode)
         {
             Queue<int> q = new Queue<int>();
             Queue<int> qtemp = new Queue<int>();
             q.Enqueue(node);
             while (true)
             {
                 node = visited[node][0];
                 q.Enqueue(node);
                 if (node == startNode)
                 {
                     break;
                 }
             }
             path = q.Reverse().ToList<int>();

         }
        
        #endregion

         
              
    }
}
