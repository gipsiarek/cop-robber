﻿using System;
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
        #region members of test class
        public Board board;
        public Robber robber;
        public List<Cop> cops;
        public List<int> ocupied; 
        private int testNumber, nodenumberMin, nodenumberMax, nodeNumber, maxNodeSt;    //info z kontrolek
        private int txbIloscGoniacych, txbIloscGoniacychMax, txbBeaconRandom, txbAlfaBetaDepthMin, txbAlfaBetaDepthMax, txbGoOnTime;
        private bool isGreedy, isRandomB, isAlphaBeta;
        public bool copMove;
        public List<outputClass> outputGreedy = new List<outputClass>();
        public List<outputClass> outputBeacon = new List<outputClass>();
        public List<outputClass> outputAlfaBeta = new List<outputClass>();

        #endregion
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
            getDataFromProperties();
           
           
            runRobberRun(testNumber);
            GridOutputGreedy.ItemsSource = outputGreedy;
            GridOutputBeacon.ItemsSource = outputBeacon;
            gridOutputAlfBet.ItemsSource = outputAlfaBeta;

        }

        private void getDataFromProperties()
        {
            testNumber = Convert.ToInt32(Application.Current.Properties["tTestNumber"]);
            nodenumberMin = Convert.ToInt32(Application.Current.Properties["tNodeNumberMin"]);
            nodenumberMax = Convert.ToInt32(Application.Current.Properties["tNodeNumberMax"]);
            maxNodeSt = Convert.ToInt32(Application.Current.Properties["tMaxNodeSt"]);
            isGreedy = Convert.ToBoolean(Application.Current.Properties["tChbGreedy"]);
            isRandomB = Convert.ToBoolean(Application.Current.Properties["tChbBeacon"]);
            isAlphaBeta = Convert.ToBoolean(Application.Current.Properties["tChbAlfaBeta"]);
            txbIloscGoniacych = Convert.ToInt32(Application.Current.Properties["tTbCopNr"]);
            txbIloscGoniacychMax = Convert.ToInt32(Application.Current.Properties["tTbCopNrMax"]);
            txbBeaconRandom = Convert.ToInt32(Application.Current.Properties["tTbRandNr"]);
            txbGoOnTime = Convert.ToInt32(Application.Current.Properties["tTbgoOnTime"]);
            txbAlfaBetaDepthMin = Convert.ToInt32(Application.Current.Properties["tTbAlfaDepthMin"]);
            txbAlfaBetaDepthMax = Convert.ToInt32(Application.Current.Properties["tTbAlfaDepthMax"]);

            if (nodenumberMin > nodenumberMax) nodenumberMax = nodenumberMin;
            if (txbIloscGoniacych > txbIloscGoniacychMax) txbIloscGoniacychMax = txbIloscGoniacych;
            if (txbAlfaBetaDepthMin > txbAlfaBetaDepthMax) txbAlfaBetaDepthMax = txbAlfaBetaDepthMin;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
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
            NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            XYPlot plot = new XYPlot();
            plot.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region ALGORITHMS
        ///// <summary>
        ///// 1 greedy robber vs 1 greedy cop
        ///// GREEDY - Find The Shortest path to robber when cop moves and longest path to cop when robber moves
        ///// It uses the Dijkstra algorithm to find a path from all of the neighbors
        ///// </summary>
        //private bool greedy_1vs1()
        //{
        //    Cop cop = cops[0];
        //    copMove = true;
        //    int moves=0;
        //    int maxMoves=board.verticies*4;
        //    List<int> currentPath = new List<int>();
        //    List<int> bestPath = new List<int>();
        //    //List<List<int>> movesList = new List<List<int>>();
        //    bool defeat=false;
        //    while (!isCought(1) && moves != maxMoves)
        //    {
        //        if (copMove)
        //        {
        //            moves++;
        //            currentPath.Clear(); bestPath.Clear();
        //            if (cop.myNeighbors.Contains(robber.ocpupiedNode))
        //                cop.move(robber.ocpupiedNode, board);
        //            else
        //            {
        //                foreach (int item in cop.myNeighbors)
        //                {
        //                    currentPath = Dijkstra(cop.ocupiedNode, item, robber.ocpupiedNode);
        //                    if (currentPath.Count < bestPath.Count || bestPath.Count == 0)
        //                    {
        //                        bestPath = currentPath;
        //                    }
        //                }
        //                cop.move(bestPath[0], board);
        //            }
        //            copMove = false;
        //        }
        //        else
        //        {
        //            defeat = false;
        //            currentPath.Clear(); bestPath.Clear();
        //            if (robber.myNeighbors.Contains(cop.ocupiedNode))
        //                defeat = true;
        //            foreach (int item in robber.myNeighbors)
        //            {
        //                if (item != cop.ocupiedNode)
        //                {
        //                    currentPath = Dijkstra(robber.ocpupiedNode, item, cop.ocupiedNode);
        //                    defeat = false;
        //                    if (bestPath.Count < currentPath.Count)
        //                        bestPath = currentPath;
        //                }
        //            }
        //            if (!defeat)
        //                robber.move(bestPath[0], board);
        //            else robber.move(cop.ocupiedNode, board);
        //            copMove = true;
        //        }
        //    }
        //    outputClass o = new outputClass(whoseWin(isCought(1)), "greedy", 1, board.neighbor.Count, board.verticies, moves, maxMoves);
        //    outputGreedy.Add(o);
        //    return isCought(1); //one cop
        //}
        /// <summary>
        /// 1 greedy robber vs 'copnumber' of greedy cops
        /// </summary>
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
            while (!isCought(copnumber) && moves != maxMoves)
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
                            bestPath.Add(ocupied[cop.copId]);
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
            outputClass o = new outputClass(whoseWin(isCought(copnumber)), "greedy", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputGreedy.Add(o);
            return isCought(copnumber);
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
            while (!isCought(copnumber) && moves != maxMoves)
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
                            bestPath.Add(ocupied[cop.copId]);
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
                                MessageBox.Show("Random bekon ruch gliny" + ex.StackTrace);
                            }
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
            outputClass o = new outputClass(whoseWin(isCought(copnumber)), "Beacon", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputBeacon.Add(o);
            return isCought(copnumber);
        }
        /// <summary>
        /// function of robber running away with alpha beta and cop chaising him with greedy algorithm
        /// </summary>
        /// <param name="searchDepth">depth of alpha beta</param>
        private bool alphaBetavsGreedy(int searchDepth, int copnumber)
        {
            Cop cop = cops[0];
            copMove = true;
            int moves = 0;
            int maxMoves = board.verticies * 2;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            while (!isCought(copnumber) && moves != maxMoves)
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
                    currentPath.Clear(); bestPath.Clear();
                    int tmpMove = alphabeta(robber.ocpupiedNode, searchDepth, -999, 999, true, cop.ocupiedNode, searchDepth);
                    robber.move(tmpMove, board);
                    copMove = true;
                }
            }
            outputClass o = new outputClass(whoseWin(isCought(copnumber)), "ABvsGreedy d=" + searchDepth.ToString(), copnumber, board.neighbor.Count, board.verticies, moves, maxMoves);
            outputAlfaBeta.Add(o);
            return isCought(copnumber);
        }
       
        #endregion

        #region FUNCTIONS
        /// <summary>
        /// check if robber is cought
        /// </summary>
        /// <param name="copNumber">number of cops currently chaising robber</param>
        public bool isCought(int copNumber)
        {
            for (int i = 0; i < copNumber; i++ )
            {
                if (cops[i].ocupiedNode == robber.ocpupiedNode)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// returns a name of winner of the game
        /// </summary>
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
                if (!ocupied.Contains(cops[i].ocupiedNode))
                {
                    ocupied.Add(cops[i].ocupiedNode);
                    cops[i].startNode = cops[i].ocupiedNode;
                }
                else
                {
                    do
                    {
                        cops[i] = new Cop(board.random.Next(nodenumber), board);
                        cops[i].startNode = cops[i].ocupiedNode;
                    } while (ocupied.Contains(cops[i].startNode));
                    ocupied.Add(cops[i].startNode);
                }
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
        /// <summary>
        /// main loop for invoking test
        /// </summary>
        private void runRobberRun(int testNumber)
        {
            for (int i = 0; i < testNumber; i++)
            {
                newDataForTest(txbIloscGoniacychMax);
                #region ROZNE ALGORYTMY
                if (isGreedy)
                {
                    for (int tmp = txbIloscGoniacych; tmp <= txbIloscGoniacychMax; tmp++)
                    {
                            greedy_1vsMany(tmp);
                            resetCops();
                    }
                }
                if (isRandomB)
                {
                    for (int tmp = txbIloscGoniacych; tmp <= txbIloscGoniacychMax; tmp++)
                    {
                        randomBeacon(txbBeaconRandom, txbGoOnTime, tmp);
                        resetCops();
                    }
                    
                }
                if (isAlphaBeta)
                {
                    for (int tmp = txbAlfaBetaDepthMin; tmp <= txbAlfaBetaDepthMax; tmp++)
                    {
                        alphaBetavsGreedy(tmp, 1); 
                        resetCops();
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// reset players and cops to start positions for new test
        /// </summary>
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
        /// <summary>
        /// Convert the path from dijkstra algorithm to fit natural version from start node to finish node
        /// </summary>
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
        /// <summary>
        /// alpha beta algorithm :)
        /// </summary>
        /// <param name="node">start node for player which turn it is</param>
        /// <param name="depth">current depth of search</param>
        /// <returns>the best node to move to</returns>
        private int alphabeta(int node, int depth, int alpha, int beta, bool player, int oponentNode, int startDepth)
        {
            int goNode = node;
            if (depth == 0 || node == oponentNode) //lub koniec gry
            {
                return getNodeValue(node, oponentNode, player);
            }

            if (player == true)
            {
                foreach (int neighbor in board.findNeighbors(node))
                {
                    int alpha2 = Math.Max(alpha, alphabeta(oponentNode, depth - 1, alpha, beta, !player, neighbor, startDepth));
                    if (alpha2 > alpha)
                    {
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
                    if (beta < alpha)
                    {
                        break;
                    }
                }
                return beta;
            }
        }
        /// <summary>
        /// heuristic function to rank a node for alpha beta algoritm
        /// </summary>
        /// <param name="node">node of player</param>
        private int getNodeValue(int node, int oponentNode, bool player)
        {
            List<int> length = new List<int>();
            if (player)
            {
                if (node == oponentNode) return 999;
                length = Dijkstra(node, node, oponentNode);
                return length.Count;
            }
            else
            {
                if (node == oponentNode) return -999;
                length = Dijkstra(node, node, oponentNode);
                return -(length.Count);

            }
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
    #endregion
}
    