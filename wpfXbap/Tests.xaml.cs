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
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : Page
    {

        #region INITIALIZATION 
        public Board board;
        public Robber robber;
        public List<Cop> cops;
        public List<int> ocupied; //zajety
        private int testNumber, nodenumberMin, nodenumberMax, nodeNumber, maxNodeSt;
        public bool copMove;
        public List<outputClass> outputGreedy = new List<outputClass>();
        public List<outputClass> outputBeacon = new List<outputClass>();
        public List<outputClass> outputAlfaBeta = new List<outputClass>();
        
        public Tests()
        {
            try
            {
                InitializeComponent();
                test_OnLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            }
        }
        private void test_OnLoad()
        {
            testNumber = Convert.ToInt32(Application.Current.Properties["tTestNumber"]);
            nodenumberMin = Convert.ToInt32(Application.Current.Properties["tNodeNumberMin"]);
            nodenumberMax = Convert.ToInt32(Application.Current.Properties["tNodeNumberMax"]);
            maxNodeSt = Convert.ToInt32(Application.Current.Properties["tMaxNodeSt"]);
            //testNumber = 10;
            //nodenumberMin = 50;
            //nodenumberMax = 150;
            //maxNodeSt = 5;
            newDataForTest(1);
           
            runRobberRun(testNumber);
            GridOutputGreedy.ItemsSource = outputGreedy;
            GridOutputBeacon.ItemsSource = outputBeacon;
            gridOutputAlfBet.ItemsSource = outputAlfaBeta;

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Remove("tTestNumber");
            Application.Current.Properties.Remove("tNodeNumberMax");
            Application.Current.Properties.Remove("tNodeNumberMin");
            Application.Current.Properties.Remove("tMaxNodeSt");
            NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            XYPlot plot = new XYPlot();
            plot.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region ALGORITHMS
        /// <summary>
        /// Find The Shortest path to robber when cop moves and longest path to cop when robber moves
        /// It uses the Dijkstra algorithm to find a path from all of the neighbors
        /// </summary>
        private bool greedy_1vs1()
        {
            Cop cop = cops[0];
            copMove = true;
            int moves=0;
            int maxMoves=board.verticies*4;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            //List<List<int>> movesList = new List<List<int>>();
            bool defeat=false;
            while (!isCought() && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    currentPath.Clear(); bestPath.Clear();
                    if (cop.myNeighbors.Contains(robber.ocpupiedNode))
                        cop.move(robber.ocpupiedNode, board);
                    else
                    {
                        foreach (int item in cop.myNeighbors)
                        {
                            currentPath = Dijkstra(cop.ocupiedNode, item, robber.ocpupiedNode);
                            if (currentPath.Count < bestPath.Count || bestPath.Count == 0)
                            {
                                bestPath = currentPath;
                            }
                        }
                        cop.move(bestPath[0], board);
                    }
                    copMove = false;
                }
                else
                {
                    defeat = false;
                    currentPath.Clear(); bestPath.Clear();
                    if (robber.myNeighbors.Contains(cop.ocupiedNode))
                        defeat = true;
                    foreach (int item in robber.myNeighbors)
                    {
                        if (item != cop.ocupiedNode)
                        {
                            currentPath = Dijkstra(robber.ocpupiedNode, item, cop.ocupiedNode);
                            defeat = false;
                            if (bestPath.Count < currentPath.Count)
                                bestPath = currentPath;
                        }
                    }
                    if (!defeat)
                        robber.move(bestPath[0], board);
                    else robber.move(cop.ocupiedNode, board);
                    copMove = true;
                }
            }
            outputClass o = new outputClass(whoseWin(isCought()), "greedy", 1, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputGreedy.Add(o);
            return isCought();
        }
        private bool greedy_1vsMany(int copnumber)
        {
            copMove = true;
            int moves = 0;
            int maxMoves = board.verticies *4 ;
            List<int> currentPath = new List<int>();
            List<List<int>> currentPathToCops = new List<List<int>>();
            List<int> bestPath = new List<int>();
            int pathLength = 0;
            bool defeat = false, freeway=true;
            while (!isCought() && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    for (int i=0; i< copnumber; i++)
                    {
                        Cop cop = cops[i];
                        currentPath.Clear(); bestPath.Clear();
                        if (cop.myNeighbors.Contains(robber.ocpupiedNode))
                            cop.move(robber.ocpupiedNode, board);
                        else
                        {
                            foreach (int item in cop.myNeighbors)
                            {
                                freeway = true;
                                currentPath = Dijkstra(cop.ocupiedNode, item, robber.ocpupiedNode);
                                foreach (int t in ocupied)
                                {
                                    if(currentPath.Contains(t))
                                        freeway= false;
                                }
                                if ((currentPath.Count < bestPath.Count || bestPath.Count == 0) && freeway )
                                {
                                    bestPath = currentPath;
                                }
                            }
                            
                            try
                            {
                                if (bestPath != null || bestPath.Count!=0)
                                {
                                    cop.move(bestPath[0], board);
                                    ocupied[cop.copId] = bestPath[0];
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("dupa przy dodawaniu do zajętych wierzchołków" + ex.StackTrace);
                            }
                        }
                    }
                    copMove = false;
                }
                else
                {
                    defeat = false;
                    currentPath.Clear(); bestPath.Clear();
                    foreach (Cop cop in cops)
                    {
                        if (robber.myNeighbors.Contains(cop.ocupiedNode))
                            defeat = true;
                    }
                    foreach (int item in robber.myNeighbors)
                    {
                        if (!ocupied.Contains(item))
                        {
                            for(int i=0;i<copnumber;i++)
                            {
                                Cop cop = cops[i];
                                currentPath = Dijkstra(robber.ocpupiedNode, item, cop.ocupiedNode);
                                currentPathToCops.Add(currentPath);
                            }

                            foreach (List<int> tmp in currentPathToCops)
                            {
                                if (pathLength == 0 || pathLength < tmp.Count)
                                {
                                    pathLength = tmp.Count;
                                    currentPath = tmp;
                                }
                            }
                            defeat = false;
                            if (bestPath.Count < currentPath.Count)
                                bestPath = currentPath;
                            currentPathToCops.Clear();
                            
                        }
                    }

                    if (!defeat)
                    {
                        try
                        {
                            robber.move(bestPath[0], board);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ruch złodzieja");
                        }
                    }
                    copMove = true;
                    pathLength = 0;
                }
            }
            outputClass o = new outputClass(whoseWin(isCought()), "greedy", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputGreedy.Add(o);
            return isCought();
        }
        /// <summary>
        /// Random beacon algorithm
        /// </summary>
        /// <param name="r">number of nodes to randomly choose</param>
        /// <param name="goOnTime">number of steps to follow one choosed path</param>        
        /// <returns></returns>
        private bool randomBeacon(int r, int goOnTime, int copnumber)
        {
            copMove = true;
            int moves = 0;
            int maxMoves = board.verticies * 4;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            List<int> robberPath = new List<int>();
            bool freeway = true;
            int tmpTime = 0;
            while (!isCought() && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    for (int i = 0; i < copnumber; i++)
                    {
                        Cop cop = cops[i];
                        currentPath.Clear(); bestPath.Clear();
                        if (cop.myNeighbors.Contains(robber.ocpupiedNode))
                            cop.move(robber.ocpupiedNode, board);
                        else
                        {
                            foreach (int item in cop.myNeighbors)
                            {
                                freeway = true;
                                currentPath = Dijkstra(cop.ocupiedNode, item, robber.ocpupiedNode);
                                foreach (int t in ocupied)
                                {
                                    if (currentPath.Contains(t))
                                        freeway = false;
                                }
                                if ((currentPath.Count < bestPath.Count || bestPath.Count == 0) && freeway)
                                {
                                    bestPath = currentPath;
                                }
                            }
                            cop.move(bestPath[0], board);
                            ocupied[cop.copId] = bestPath[0];
                        }
                    }
                    copMove = false;
                }
                else
                {
                    if ((tmpTime == goOnTime || tmpTime == 0 ) && tmpTime!=currentPath.Count)
                    {
                        robberPath = getBeaconNodes(r, copnumber);
                        robber.move(robberPath[1], board);
                        tmpTime++;
                    }
                    else
                    {
                        if (robberPath.Count < tmpTime)
                        {
                            robber.move(robberPath[tmpTime], board);
                            tmpTime++;
                        }
                        else
                        {
                            tmpTime = 0;
                            robberPath = getBeaconNodes(r, copnumber);
                            robber.move(robberPath[1], board);
                            tmpTime++;
                        }
                    }

                    copMove = true;
                }
            }
            outputClass o = new outputClass(whoseWin(isCought()), "Beacon", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputBeacon.Add(o);
            return isCought();
        }
        private bool alphaBetavsGreedy(int searchDepth, int copnumber)
        {
            Cop cop = cops[0];
            copMove = true;
            int moves = 0;
            int maxMoves = board.verticies * 2;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            Boolean defeat = false;
            while (!isCought() && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    currentPath.Clear(); bestPath.Clear();
                    if (cop.myNeighbors.Contains(robber.ocpupiedNode))
                        cop.move(robber.ocpupiedNode, board);
                    else
                    {
                        foreach (int item in cop.myNeighbors)
                        {
                            currentPath = Dijkstra(cop.ocupiedNode, item, robber.ocpupiedNode);
                            if (currentPath.Count < bestPath.Count || bestPath.Count == 0)
                            {
                                bestPath = currentPath;
                            }
                        }
                        cop.move(bestPath[0], board);
                    }
                    copMove = false;
                }
                else
                {
                    defeat = false;
                    currentPath.Clear(); bestPath.Clear();
                    if (robber.myNeighbors.Contains(cop.ocupiedNode))
                        defeat = true;
                    int tmpMove = alphabeta(robber.ocpupiedNode, 3, -999, 999, true, cop.ocupiedNode, 3);
                    if (tmpMove!=null)
                        robber.move(tmpMove, board);
                    else robber.move(cop.ocupiedNode, board);
                    copMove = true;
                }
            }
            outputClass o = new outputClass(whoseWin(isCought()), "ABvsGreedy d=" + searchDepth.ToString(), copnumber, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputAlfaBeta.Add(o);
            return isCought();
        }


        /// <summary>
        /// find a shortest path from one of neighbors 'startnode' to 'finalnode', the root is 'from'
        /// </summary>
        public List<int> Dijkstra(int from, int startNode, int finalNode)
        {
            nodeNumber = board.neighbor.Count;
            List<int> path = new List<int>();
            List<List<int>> visited = new List<List<int>>(nodeNumber);
            for (int i = 0; i < nodeNumber; i++)
            {
                visited.Add(new List<int>());
            }
            List<int> neighbors = board.findNeighbors(startNode);
            Queue<int> q = new Queue<int>(nodeNumber);
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

        /// <summary>
        /// alpha beta algorithm :)
        /// </summary>
        /// <param name="node">start node for player which turn it is</param>
        /// <param name="depth">current depth of search</param>
        /// <returns>the best node to move to</returns>
        private int alphabeta(int node, int depth, int alpha, int beta, bool player, int oponentNode, int startDepth)
        {
            int goNode=node;
            if (depth==0 || node == oponentNode) //lub koniec gry
            {
                return getNodeValue(node, oponentNode, player);
            }
            
            if (player == true)
            {
                foreach (int neighbor in board.findNeighbors(node))
                {
                    int alpha2 = Math.Max(alpha, alphabeta(oponentNode, depth - 1, alpha, beta, !player, neighbor, startDepth));
                    if (alpha2 > alpha){
                        goNode = neighbor;
                        alpha = alpha2;
                    }
                    
                    if (beta < alpha)
                    {
                        break;
                    }
                }
                if (depth == startDepth)
                    return goNode;
                return alpha;
            }
            else
            {
                foreach (int neighbor in board.findNeighbors(oponentNode))
                {
                    beta = Math.Min(beta, alphabeta(oponentNode, depth - 1, alpha, beta, !player, neighbor, startDepth));
                    if (beta<alpha)
                    {
                        break;
                    }
                }
                return beta;                
            }
        }

        private int getNodeValue(int node, int oponentNode, bool player)
        {
            List<int> length = new List<int>();
            if (player)
            {
                if (node == oponentNode) return 400; 
                length = Dijkstra(node, node, oponentNode);
                return length.Count + board.findNeighbors(length[0]).Count;
            }
            else
            {
                if (node == oponentNode) return -400;
                length = Dijkstra(node, node, oponentNode);
                return -(length.Count + board.findNeighbors(length[0]).Count);
                
            }
        }
       
        #endregion

        #region FUNCTIONS

        public bool isCought()
        {
            foreach(Cop cop in cops){
            if (cop.ocupiedNode == robber.ocpupiedNode)
                return true;
            }
            return false;
        }
        private string whoseWin(bool p)
        {
            if (p)
                return "COP";
            else return "ROBBER";
        }
        /// <summary>
        /// creating new board robber and cops for new series of runaways
        /// </summary>        
        private void newDataForTest(int copnumber)
        {
            List<int> notSafe = new List<int>();
            cops = new List<Cop>();
            ocupied = new List<int>();
            board = new Board(nodenumberMin, nodenumberMax, maxNodeSt);
            int nodenumber = board.neighbor.Count;
            int i;
            for (i = 0; i < copnumber; i++)
			{
			    cops.Add(new Cop(board.random.Next(nodenumber), board));
                cops[i].copId = i;
                ocupied.Add(0);
                if (!ocupied.Contains(cops[i].ocupiedNode))
                {                    
                    cops[i].startNode = cops[i].ocupiedNode;
                }
                else do
                {
                    cops[i] = new Cop(board.random.Next(nodenumber), board);
                    cops[i].startNode = cops[i].ocupiedNode;
                } while (ocupied.Contains(cops[i].startNode));
                 ocupied[i] = cops[i].startNode;

			}
            foreach (Cop cop in cops)
            {
                foreach (int a in cop.myNeighbors)
                {
                    notSafe.Add(a);
                }
            }
            i = 0;
            do
            {
                i++;
                robber = new Robber(board.random.Next(nodenumber), board);
                robber.startNode = robber.ocpupiedNode;
                if (i > 100) newDataForTest(copnumber);
            } while (ocupied.Contains(robber.ocpupiedNode) && notSafe.Contains(robber.ocpupiedNode));
        }
        private void runRobberRun(int testNumber)
        {
            for (int i = 0; i < testNumber; i++)
            {
                newDataForTest(3);
                #region ROZNE ALGORYTMY
                greedy_1vs1();
                resetCops();
                randomBeacon(10, 3, 1);
                resetCops();
                alphaBetavsGreedy(2, 1);
                resetCops();
                greedy_1vsMany(2);
                resetCops();
                randomBeacon(10, 3, 2);
                resetCops();
                alphaBetavsGreedy(3, 1);
                resetCops();
                greedy_1vsMany(3);
                resetCops();
                randomBeacon(10, 3, 3);
                #endregion
                
            }
        }
        private void resetCops()
        {
            foreach (Cop cop in cops)
            {
                cop.ocupiedNode = cop.startNode;
                ocupied[cop.copId] = cop.startNode;
                cop.myNeighbors = board.findNeighbors(cop.startNode);
            }
            robber.ocpupiedNode = robber.startNode;
            robber.myNeighbors = board.findNeighbors(robber.startNode);
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
        /// <summary>
        /// randomly r nodes and check which of these are farest away from cop
        /// then returns the path from robber to that node
        /// </summary>
        /// <param name="r">number of random nodes</param>
        private List<int> getBeaconNodes(int r, int copnumber)
        {
            List<int> path = new List<int>();
            List<int> bestPath = new List<int>();
            List<List<int>> currentPathToCops = new List<List<int>>();
            int bestNode=0, pathLength=0;
            int node;
            for (int randNumber = 0; randNumber < r; randNumber++)
            {
                node = board.random.Next(board.neighbor.Count);
                for (int i = 0; i < copnumber; i++)
                {
                    Cop cop = cops[i];
                    if (cop.ocupiedNode != node)
                    {
                        path = Dijkstra(node, node, cop.ocupiedNode);
                        currentPathToCops.Add(path);
                        
                    }
                    foreach (List<int> tmp in currentPathToCops)
                    {
                        if (pathLength == 0 || pathLength > tmp.Count)
                        {
                            pathLength = tmp.Count;
                            path = tmp;
                        }
                    }
                    if (path.Count > bestPath.Count || bestPath.Count==0)
                    {
                        bestPath = path;
                        bestNode = node;
                    }
                    currentPathToCops.Clear();
                }
            }
            return Dijkstra(robber.ocpupiedNode, robber.ocpupiedNode, bestNode);
        }
       
        #endregion



    }


    #region KLASA DO OBSŁUGI DATAGRID
    /// <summary>
    /// Class created for presenting output of tests in datagrid
    /// </summary>
    public class outputClass
    {
        private string m_Winner;
        private string m_algorithm;
        private int m_copNumber;
        private int m_nodes;
        private int m_verticies;
        private int m_moves;
        private int m_maxMoves;


        public string Winner
        {
            get { return m_Winner; }
        }
        public string Algorithm
        {
            get { return m_algorithm; }
        }
        public int copNumber
        {
            get { return m_copNumber; }
        }
        public int Nodes
        {
            get { return m_nodes; }
        }
        public int Vericies
        {
            get { return m_verticies; }
        }
        public int Moves
        {
            get { return m_moves; }
        }
        public int MaxMoves
        {
            get { return m_maxMoves; }
        }


        public outputClass(string Winner, string Algorithm,int copNumber, int Nodes, int Vericies, int Moves, int MaxMoves )
        {
            this.m_Winner = Winner;
            this.m_algorithm = Algorithm;
            this.m_nodes = Nodes;
            this.m_verticies = Vericies;
            this.m_moves = Moves;
            this.m_maxMoves = MaxMoves;
            this.m_copNumber = copNumber;
        }
    }
}
    #endregion