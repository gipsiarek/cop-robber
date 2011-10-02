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
using System.IO;


namespace wpfXbap
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : Page
    {

        #region INITIALIZATION
        #region members of test class
        public static Board board;
        public static Robber robber;
        public static List<Cop> cops;
        public static List<int> ocupied;
        public int testNumber, currentTest;
        public string classOfGraph;
        private static int nodenumberMin, nodenumberMax, nodeNumber, maxNodeSt;    //info z kontrolek
        private int txbIloscGoniacych, txbIloscGoniacychMax, txbBeaconRandom, txbAlfaBetaDepthMin, txbAlfaBetaDepthMax, txbGoOnTime;
        private int txbTreeWidth, txbTreeDepth;
        public int maxMoves;
        private bool isGreedy, isRandomB, isAlphaBeta, isMCTS, isAutoTest;
        private Tree<Data> MCTSTree;
        public bool copMove;
        public static List<outputClass> outputGreedyDijkstra;
        public static List<outputClass> outputGreedyDumb;
        public static List<outputClass> outputBeacon;
        public static List<outputClass> outputAlfaBeta;
        public static List<outputClass> outputMCTS ;
        public int iterGreedyDumb, iterGreedyDijkstra, iterRandomBeacon, iterAlfaBeta, iterMCTS;

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
            if (!Convert.ToBoolean(Application.Current.Properties["FromXYPlot"]))
            {
                MCTSTree = new Tree<Data>();
                outputGreedyDijkstra = new List<outputClass>();
                outputGreedyDumb = new List<outputClass>();
                outputBeacon = new List<outputClass>();
                outputAlfaBeta = new List<outputClass>();
                outputMCTS = new List<outputClass>();
                if (isAutoTest)
                {
                    //runRobberAutoRun();
                }
                else
                {
                    runRobberRun(testNumber);
                }
            }
            GridOutputGreedy.ItemsSource = outputGreedyDijkstra;
            GridOutputBeacon.ItemsSource = outputBeacon;
            gridOutputAlfBet.ItemsSource = outputAlfaBeta;
            gridGreedyDumb.ItemsSource = outputGreedyDumb;
            gridOutputMCTS.ItemsSource = outputMCTS;

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
            Application.Current.Properties.Remove("tTreeDepth");
            Application.Current.Properties.Remove("tTreeWidth");
            Application.Current.Properties.Remove("tChbMCTS");
            Application.Current.Properties.Remove("FromXYPlot");
            Application.Current.Properties.Remove("tAutoTest");
            NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            XYPlot plot = new XYPlot();
            NavigationService.GetNavigationService(this).Navigate(new Uri("XYPlot.xaml", UriKind.RelativeOrAbsolute));
            //plot.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region ALGORITHMS

        ///// <summary>
        ///// rober find a naighbor without a cop and go there
        ///// </summary>
        private outputClass greedy_dumb_1vsMany(int copnumber)
        {

            copMove = true;
            int moves = 0;
            
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            iterGreedyDumb = 0;
            while (!isCought(copnumber) && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    cops_moves_greedy_dijkstra(copnumber, currentPath, bestPath);
                    copMove = false;
                }
                else
                {
                    robber.move(robber_moves_greedy_dumb(copnumber), board);
                    copMove = true;
                }
            }
            outputClass o = new outputClass(currentTest, classOfGraph, whoseWin(isCought(copnumber)), "greedy-dumb", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves, iterGreedyDumb);
            if(!isAutoTest)
                outputGreedyDumb.Add(o);
            return o;
        }
        /// <summary>
        /// 1 greedy robber vs 'copnumber' of greedy cops
        /// </summary>
        private outputClass greedy_dijkstra_1vs_many(int copnumber)
        {
            copMove = true;
            int moves = 0;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            iterGreedyDijkstra = 0;
            while (!isCought(copnumber) && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    cops_moves_greedy_dijkstra(copnumber, currentPath, bestPath);
                    copMove = false;
                }
                else
                {
                    currentPath.Clear(); bestPath.Clear();
                    robber.move(robber_moves_greedy_dijkstra(copnumber, currentPath, bestPath), board);
                    copMove = true;
                }
            }
            outputClass o = new outputClass(currentTest, classOfGraph, whoseWin(isCought(copnumber)), "greedy", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves, iterGreedyDijkstra);
            if (!isAutoTest)
            {
                outputGreedyDijkstra.Add(o);
            }
            return o;
        }
        /// <summary>
        /// Random beacon algorithm
        /// </summary>
        /// <param name="r">number of nodes to randomly choose</param>
        /// <param name="goOnTime">number of steps to follow one choosed path</param>        
        private outputClass randomBeacon(int r, int goOnTime, int copnumber)
        {
            copMove = true;
            int moves = 0;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            List<int> robberPath = new List<int>();
            int tmpTime = 1;
            iterRandomBeacon = 0;
            while (!isCought(copnumber) && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    cops_moves_greedy_dijkstra(copnumber, currentPath, bestPath);
                    copMove = false;
                }
                else
                {
                    if ((tmpTime == goOnTime || tmpTime == 1 || robberPath.Count <= tmpTime))
                    {
                        tmpTime = 1;
                        robberPath = getBeaconNodes(r, copnumber);
                        robber.move(robberPath[tmpTime], board);
                        tmpTime++;
                    }
                    else
                    {
                        iterRandomBeacon++;
                        robber.move(robberPath[tmpTime], board);
                        tmpTime++;
                    }

                    copMove = true;
                }
            }
            outputClass o = new outputClass(currentTest, classOfGraph, whoseWin(isCought(copnumber)), "Beacon", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves,iterRandomBeacon);
            if (!isAutoTest)
            {
                outputBeacon.Add(o);    
            }
            
            return o;
        }
        /// <summary>
        /// function of robber running away with alpha beta and cop chaising him with greedy algorithm
        /// </summary>
        /// <param name="searchDepth">depth of alpha beta</param>
        private outputClass alphaBetavsGreedy(int searchDepth, int copnumber)
        {
            Cop cop = cops[0];
            copMove = true;
            int moves = 0;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            iterAlfaBeta = 0; ;
            while (!isCought(copnumber) && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    cops_moves_greedy_dijkstra(1, currentPath, bestPath);
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
            outputClass o = new outputClass(currentTest, classOfGraph, whoseWin(isCought(copnumber)), "ABvsGreedy d=" + searchDepth.ToString(), copnumber, board.neighbor.Count, board.verticies, moves, maxMoves, iterAlfaBeta);
            if (!isAutoTest)
            {
                outputAlfaBeta.Add(o);
            }
            return o;
        }
        /// <summary>
        /// cop with greedy algorithm and monte carlo tree search with propagation for robber
        /// </summary>
        private outputClass MCTS(int treeWidth, int searchDepth, int copnumber)
        {
            copMove = true;
            int moves = 0;
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            Tree<Node<Data>> MCTSTree = null;
            int counter = 0;
            iterMCTS = 0;
            while (!isCought(copnumber) && moves != maxMoves)
            {
                if (copMove)
                {
                    moves++;
                    cops_moves_greedy_dijkstra(copnumber, currentPath, bestPath);
                    copMove = false;
                }
                else
                {
  //                  if (MCTSTree == null || counter == 0)
    //                {
                        if(MCTSTree!=null)
                            MCTSTree.Clear();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        counter = txbTreeDepth - 1;
                        Data data = new Data();
                        for (int i = 0; i < copnumber; i++)
                        {
                            data.CopPos[i] = cops[i].ocupiedNode;
                        }
                        data.RobberPos = robber.ocpupiedNode;
                        MCTSTree = new Tree<Node<Data>>(simulateGame(treeWidth, searchDepth, copnumber, new Node<Data>(null, data)));
  //                      MCTSTree.node = PropagateChildrenProbability(MCTSTree.node);
                        robber.move(getBestTreeNode(MCTSTree, out MCTSTree), board);
                        copMove = true;
                //    }
                //    else
                //    {
                //        iterMCTS++;
                //        robber.move(getBestTreeNode(MCTSTree, out MCTSTree), board);
                //        copMove = true;
                //        counter--;
                //    }
                }
            }

            outputClass o = new outputClass(currentTest, classOfGraph, whoseWin(isCought(copnumber)), "MCTS", copnumber, board.neighbor.Count, board.verticies, moves, maxMoves,iterMCTS);
            if (!isAutoTest)
            {
                outputMCTS.Add(o);
            }
            return o;
        }

    
        #endregion

        #region FUNCTIONS

        #region MOVING FUNCTIONS
        /// <summary>
        /// Move cops in test to reach the robber, uses dijkstra for finding shortest path
        /// </summary>
        private void cops_moves_greedy_dijkstra(int copnumber, List<int> currentPath, List<int> bestPath)
        {
            bool freeway = true;
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
                        if (bestPath != null || bestPath.Count != 0)
                        {
                            cop.move(bestPath[0], board);
                            ocupied[cop.copId] = bestPath[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("EXception przy dodawaniu do zajętych wierzchołków" + ex.StackTrace);
                    }
                }
            }
        }
        public int robber_moves_greedy_dumb(int copnumber)
        {
            foreach (int item in robber.myNeighbors)
            {
                iterGreedyDumb++;
                if (!ocupied.Contains(item))
                {
                    return item;
                }
            }
            return -1;
        }
        public static int robber_moves_greedy_dumb(Cop cop, Robber robber)
        {
            foreach (int item in robber.myNeighbors)
            {
                if (item != cop.myNode.number)
                {
                    return item;
                }
            }
            return -1;
        }
        
        /// <summary>
        /// move robber with greedy algorith with dijkstra heuristic
        /// </summary>
        private int robber_moves_greedy_dijkstra(int copnumber, List<int> currentPath, List<int> bestPath)
        {
            List<List<int>> currentPathToCops = new List<List<int>>();
            bool defeat = false;
            int pathLength = 0;
            foreach (Cop cop in cops)
            {
                if (robber.myNeighbors.Contains(cop.ocupiedNode))
                {
                    defeat = true;
                    iterGreedyDijkstra++;
                }
            }
            foreach (int item in robber.myNeighbors)
            {
                if (!ocupied.Contains(item))
                {
                    for (int i = 0; i < copnumber; i++)
                    {
                        Cop cop = cops[i];
                        currentPath = Dijkstra(robber.ocpupiedNode, item, cop.ocupiedNode);
                        currentPathToCops.Add(currentPath);
                    }

                    foreach (List<int> tmp in currentPathToCops)
                    {
                        iterGreedyDijkstra++;
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
                    return bestPath[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ruch złodzieja" + ex);
                }
            }
            return -1;
        }
        public static int robber_moves_greedy_dijkstra(int copnumber, Board board, Cop cop, Robber robber)
        {
            List<int> currentPath = new List<int>();
            List<int> bestPath = new List<int>();
            List<List<int>> currentPathToCops = new List<List<int>>();
            bool defeat = false;
            int pathLength = 0;
            if (robber.myNeighbors.Contains(cop.ocupiedNode))
                defeat = true;

            foreach (int item in robber.myNeighbors)
            {
                if (item != cop.myNode.number)
                {
                    currentPath = Dijkstra(robber.ocpupiedNode, item, cop.ocupiedNode, board);
                    currentPathToCops.Add(currentPath);

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
                    return bestPath[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ruch złodzieja" + ex);
                }
            }
            return -1;
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
            int bestNode = 0, pathLength = 0;
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
                        iterRandomBeacon++;
                        if (pathLength == 0 || pathLength > tmp.Count)
                        {
                            pathLength = tmp.Count;
                            path = tmp;
                        }
                    }
                    if (path.Count > bestPath.Count || bestPath.Count == 0)
                    {
                        bestPath = path;
                        bestNode = node;
                    }
                    currentPathToCops.Clear();
                }
            }
            return Dijkstra(robber.ocpupiedNode, robber.ocpupiedNode, bestNode);
        }
        public static int robber_moves_randomBeacon(Board board, int r, int goOnTime, Robber robber, Cop cop)
        {
            int node = 0;
            if ((robber.movesSoFar == goOnTime || robber.movesSoFar == 1 || robber.myPath.Count <= robber.movesSoFar))
            {
                robber.movesSoFar = 1;
                robber.myPath = getBeaconNodes(r, 1, board, cop, robber);
                //robber.move(robber.myPath[robber.movesSoFar], board);
                node = robber.myPath[robber.movesSoFar];
                robber.movesSoFar++;
            }
            else
            {
                //robber.move(robber.myPath[robber.movesSoFar], board);
                node = robber.myPath[robber.movesSoFar];
                robber.movesSoFar++;
            }
            return node;
        }
        public static List<int> getBeaconNodes(int r, int copnumber, Board board, Cop cop, Robber robber)
        {
            List<int> path = new List<int>();
            List<int> bestPath = new List<int>();
            List<List<int>> currentPathToCops = new List<List<int>>();
            int bestNode = 0, pathLength = 0;
            int node;
            for (int randNumber = 0; randNumber < r; randNumber++)
            {
                node = board.random.Next(board.neighbor.Count);
                if (cop.ocupiedNode != node)
                {
                    path = Dijkstra(node, node, cop.ocupiedNode, board);
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
                if (path.Count > bestPath.Count || bestPath.Count == 0)
                {
                    bestPath = path;
                    bestNode = node;
                }
                currentPathToCops.Clear();
            }
            return Dijkstra(robber.ocpupiedNode, robber.ocpupiedNode, bestNode, board);
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
                iterAlfaBeta++;
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
        public static int alphabeta(int node, int depth, int alpha, int beta, bool player, int oponentNode, int startDepth, Board board)
        {
            int goNode = node;
            if (depth == 0 || node == oponentNode) //lub koniec gry
            {
                return getNodeValue(node, oponentNode, player, board);
            }

            if (player == true)
            {
                foreach (int neighbor in board.findNeighbors(node))
                {
                    int alpha2 = Math.Max(alpha, alphabeta(oponentNode, depth - 1, alpha, beta, !player, neighbor, startDepth, board));
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
                    beta = Math.Min(beta, alphabeta(oponentNode, depth - 1, alpha, beta, !player, neighbor, startDepth, board));
                    if (beta < alpha)
                    {
                        break;
                    }
                }
                if (depth == startDepth)
                    return goNode;
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
                if (node == oponentNode) return -999;
                length = Dijkstra(node, node, oponentNode);
                return -(length.Count);
            }
            else
            {
                if (node == oponentNode) return 999;
                length = Dijkstra(node, node, oponentNode);
                return length.Count;

            }
        }
        private static int getNodeValue(int node, int oponentNode, bool player, Board board)
        {
            List<int> length = new List<int>();
            if (player)
            {
                if (node == oponentNode) return 999;
                length = Dijkstra(node, node, oponentNode, board);
                return length.Count;
            }
            else
            {
                if (node == oponentNode) return -999;
                length = Dijkstra(node, node, oponentNode, board);
                return -(length.Count);

            }
        }

        private Node<Data> simulateGame(int searchWidth, int searchDepth, int copnumber, Node<Data> node)
        {
            if (searchDepth != 0)
            {
                Data nodeInfo = node.GetData();
                List<int> neighborList = new List<int>(board.findNeighbors(nodeInfo.RobberPos));
                List<int> path = new List<int>();
                int robberNodePosition;
                for (int i = 0; i < searchWidth; i++)
                {
                    node.AddChild(FindChildOnBoard(neighborList, nodeInfo, copnumber, out robberNodePosition));
                    if (neighborList.Contains(robberNodePosition))
                        neighborList.Remove(robberNodePosition);
                }
                //obliczanie prawdopodobieństw do ruchu na zaraz
                int sum = 0, iter = 0, maxNode = 0;
                double maxProbNode = 0;
                foreach (Node<Data> child in node.GetChildren())
                {
                    sum += child.GetData().RobberCopDistance;
                }
                foreach (Node<Data> child in node.GetChildren())
                {
                    iterMCTS++;
                    child.GetData().Probability = (double)child.GetData().RobberCopDistance / sum;
                    if (child.GetData().Probability > maxProbNode)
                    {
                        maxProbNode = child.GetData().Probability;
                        maxNode = iter;
                    }
                    iter++;
                }
                foreach(Node<Data> child in node.GetChildren())
                    simulateGame(searchWidth, searchDepth - 1, copnumber, child);
            }

            return node;
        }

        internal static Node<Data> simulateGame(int searchWidth, int searchDepth, int copnumber, Node<Data> node, Board board)
        {
            if (searchDepth != 0)
            {
                Data nodeInfo = node.GetData();
                List<int> neighborList = new List<int>(board.findNeighbors(nodeInfo.RobberPos));
                List<int> path = new List<int>();
                int robberNodePosition;
                for (int i = 0; i < searchWidth; i++)
                {
                    node.AddChild(FindChildOnBoard(neighborList, nodeInfo, copnumber, out robberNodePosition, board));
                    if (neighborList.Contains(robberNodePosition))
                        neighborList.Remove(robberNodePosition);
                }
                //obliczanie prawdopodobieństw do ruchu na zaraz
                int sum = 0, iter = 0, maxNode = 0;
                double maxProbNode = 0;
                foreach (Node<Data> child in node.GetChildren())
                {
                    sum += child.GetData().RobberCopDistance;
                }
                foreach (Node<Data> child in node.GetChildren())
                {
                    child.GetData().Probability = (double)child.GetData().RobberCopDistance / sum;
                    if (child.GetData().Probability > maxProbNode)
                    {
                        maxProbNode = child.GetData().Probability;
                        maxNode = iter;
                    }
                    iter++;
                }
                simulateGame(searchWidth, searchDepth - 1, copnumber, node.GetChild(maxNode), board);
            }

            return node;
        }

        private Data FindChildOnBoard(List<int> neighborList, Data nodeInfo, int copnumber, out int robberNodePosition)
        {
            List<int> path = new List<int>();
            Data newNode = new Data();
            if (neighborList.Count != 0)
            {
                newNode.RobberPos = neighborList[board.random.Next(neighborList.Count)];    //neighbors of robber
            }
            for (int j = 0; j < copnumber; j++)
            {
                path = Dijkstra(nodeInfo.CopPos[j], nodeInfo.CopPos[j], newNode.RobberPos);     //path from cop to robber in the same variable
                if (neighborList.Count > 0)
                {
                    newNode.CopPos[j] = path[1];
                    if (newNode.RobberCopDistance > path.Count - 1 || newNode.RobberCopDistance == -1) newNode.RobberCopDistance = path.Count - 1;
                }
            }
            robberNodePosition = newNode.RobberPos;
            return newNode;
        }
        internal static Data FindChildOnBoard(List<int> neighborList, Data nodeInfo, int copnumber, out int robberNodePosition, Board board)
        {
            List<int> path = new List<int>();
            Data newNode = new Data();
            if (neighborList.Count != 0)
            {
                newNode.RobberPos = neighborList[board.random.Next(neighborList.Count)];    //neighbors of robber
            }
            for (int j = 0; j < copnumber; j++)
            {
                path = Dijkstra(nodeInfo.CopPos[j], nodeInfo.CopPos[j], newNode.RobberPos, board);     //path from cop to robber in the same variable
                if (neighborList.Count > 0)
                {
                    newNode.CopPos[j] = path[1];
                    if (newNode.RobberCopDistance > path.Count - 1 || newNode.RobberCopDistance == -1) newNode.RobberCopDistance = path.Count - 1;
                }
            }
            robberNodePosition = newNode.RobberPos;
            return newNode;
        }
        #endregion

        /// <summary>
        /// check if robber is cought
        /// </summary>
        /// <param name="copNumber">number of cops currently chaising robber</param>
        public bool isCought(int copNumber)
        {
            for (int i = 0; i < copNumber; i++)
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
            } while (ocupied.Contains(robber.ocpupiedNode) || notSafe.Contains(robber.ocpupiedNode));
        }
        
        private void newDataAutoTest(int copnumber, int nodenumberMin, int nodenumberMax, int maxNodeSt)
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
            } while (ocupied.Contains(robber.ocpupiedNode) || notSafe.Contains(robber.ocpupiedNode));
        }
        /// <summary>
        /// main loop for invoking test
        /// </summary>
        private void runRobberRun(int testNumber)
        {
            FileStream fs = new FileStream(@"d:\test.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End); 
            outputClass output = null;
            for (int i = 0; i < testNumber; i++)
            {
                currentTest = i;
                newDataForTest(txbIloscGoniacychMax);
                checkClassOfGraph();
                maxMoves = board.verticies * 10;

                #region ROZNE ALGORYTMY
                if (isGreedy)
                {
                    for (int tmp = txbIloscGoniacych; tmp <= txbIloscGoniacychMax; tmp++)
                    {
                        output = greedy_dumb_1vsMany(tmp);
                        sw.WriteLine(output.ToString());
                        resetCops();
                    }
                    for (int tmp = txbIloscGoniacych; tmp <= txbIloscGoniacychMax; tmp++)
                    {
                        output = greedy_dijkstra_1vs_many(tmp);
                        sw.WriteLine(output.ToString());
                        resetCops();
                    }
                }
                if (isRandomB)
                {
                    for (int tmp = txbIloscGoniacych; tmp <= txbIloscGoniacychMax; tmp++)
                    {
                        output = randomBeacon(txbBeaconRandom, txbGoOnTime, tmp);
                        sw.WriteLine(output.ToString());
                        resetCops();
                    }

                }
                if (isAlphaBeta)
                {
                    for (int tmp = txbAlfaBetaDepthMin; tmp <= txbAlfaBetaDepthMax; tmp++)
                    {
                        output = alphaBetavsGreedy(tmp, 1);
                        sw.WriteLine(output.ToString());
                        resetCops();
                    }
                }
                if (isMCTS)
                {
                    for (int tmp = txbIloscGoniacych; tmp <= txbIloscGoniacychMax; tmp++)
                    {
                        output = MCTS(txbTreeWidth, txbTreeDepth, tmp);
                        sw.WriteLine(output.ToString());
                        resetCops();
                    }
                }
                #endregion
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// automatic test for all algorithm! Takes long time to finish
        /// only for magisterka
        /// </summary>
  

        private void checkClassOfGraph()
        {
            Board.removePitfalls(board);
            foreach (List<int> tmp in board.neighborForClass)
            {
                if (tmp.Count != 0)
                {
                    board.isCopWinGraph = false;
                }
            }

            if (board.isCopWinGraph)
                classOfGraph = "Cop-Win";
            else
                classOfGraph = "Robber-Win";
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
        private static void createPathFromQueue(List<List<int>> visited, out List<int> path, int node, int startNode)
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
      
        internal static Node<Data> PropagateChildrenProbability(Node<Data> nodeTMP)
        {
            while (true)
            {
                if (nodeTMP.areAllChildrenLeaf() || nodeTMP.GetData().Visited == 1)
                {
                    nodeTMP.GetData().Visited = 1;
                    break;
                }
                foreach (Node<Data> child in nodeTMP.GetChildren())
                {
                    if (child.IsLeaf())
                    {
                        if (child.GetData().Probability > child.GetParent().GetData().Probability)
                        {
                            child.GetParent().GetData().Probability = child.GetData().Probability;
                        }
                        child.GetData().Visited = 1;
                    }
                    else
                    {
                        PropagateChildrenProbability(child);
                        child.GetData().Visited = 1;
                    }
                }
            }
            return nodeTMP;
        }

        internal static int robber_moves_MCTS(Tree<Node<Data>> MCTSTree, int robberNode, int copNode, int graphWidth, int graphDepth, Board board, out Tree<Node<Data>> MCTSTreeOut)
        {
            if (MCTSTree == null || MCTSTree.node.GetData().Counter == 0)
            {
                Data data = new Data();
                data.CopPos[0] = copNode;
                data.RobberPos = robberNode;
                data.Counter = graphDepth;
                MCTSTree = new Tree<Node<Data>>(simulateGame(graphWidth, graphDepth, 1, new Node<Data>(null, data), board));
                MCTSTree.node = PropagateChildrenProbability(MCTSTree.node);
                return getBestTreeNode(MCTSTree, out MCTSTreeOut, board);
            }
            else
            {
                return getBestTreeNode(MCTSTree, out MCTSTreeOut, board);
            }
        }

        private int getBestTreeNode(Tree<Node<Data>> Tree, out Tree<Node<Data>> newTree)
        {
            LinkedList<Node<Data>> linkedList = new LinkedList<Node<Data>>(Tree.node.GetChildren());
            int node = 0, iter = 0, nodeToMove = 0;
            double maxProbability = 0;
            foreach (Node<Data> treeNode in linkedList)
            {
                Data data = treeNode.GetData();
                if (data.Probability > maxProbability)
                {
                    nodeToMove = data.RobberPos;
                    node = iter;
                    maxProbability = data.Probability;
                }
                iter++;
            }
            newTree = new Tree<Node<Data>>(Tree.node.GetChild(node));
            return nodeToMove;
        }
        internal static int getBestTreeNode(Tree<Node<Data>> Tree, out Tree<Node<Data>> newTree, Board board)
        {
            LinkedList<Node<Data>> linkedList = new LinkedList<Node<Data>>(Tree.node.GetChildren());
            int node = 0, iter = 0, nodeToMove = 0;
            double maxProbability = 0;
            foreach (Node<Data> treeNode in linkedList)
            {
                Data data = treeNode.GetData();
                if (data.Probability > maxProbability)
                {
                    nodeToMove = data.RobberPos;
                    node = iter;
                    maxProbability = data.Probability;
                }
                iter++;
            }
            newTree = new Tree<Node<Data>>(Tree.node.GetChild(node));
            return nodeToMove;
        }

        /// <summary>
        /// find a shortest path from one of neighbors 'startnode' to 'finalnode', the root is 'from'
        /// </summary>
        private List<int> Dijkstra(int from, int startNode, int finalNode)
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
        public static List<int> Dijkstra(int from, int startNode, int finalNode, Board board)
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

        private void getDataFromProperties()
        {
            testNumber = Convert.ToInt32(Application.Current.Properties["tTestNumber"]);
            nodenumberMin = Convert.ToInt32(Application.Current.Properties["tNodeNumberMin"]);
            nodenumberMax = Convert.ToInt32(Application.Current.Properties["tNodeNumberMax"]);
            maxNodeSt = Convert.ToInt32(Application.Current.Properties["tMaxNodeSt"]);
            isGreedy = Convert.ToBoolean(Application.Current.Properties["tChbGreedy"]);
            isRandomB = Convert.ToBoolean(Application.Current.Properties["tChbBeacon"]);
            isAlphaBeta = Convert.ToBoolean(Application.Current.Properties["tChbAlfaBeta"]);
            isMCTS = Convert.ToBoolean(Application.Current.Properties["tChbMCTS"]);
            txbIloscGoniacych = Convert.ToInt32(Application.Current.Properties["tTbCopNr"]);
            txbIloscGoniacychMax = Convert.ToInt32(Application.Current.Properties["tTbCopNrMax"]);
            txbBeaconRandom = Convert.ToInt32(Application.Current.Properties["tTbRandNr"]);
            txbGoOnTime = Convert.ToInt32(Application.Current.Properties["tTbgoOnTime"]);
            txbAlfaBetaDepthMin = Convert.ToInt32(Application.Current.Properties["tTbAlfaDepthMin"]);
            txbAlfaBetaDepthMax = Convert.ToInt32(Application.Current.Properties["tTbAlfaDepthMax"]);
            txbTreeDepth = Convert.ToInt32(Application.Current.Properties["tTreeDepth"]);
            txbTreeWidth = Convert.ToInt32(Application.Current.Properties["tTreeWidth"]);
            isAutoTest = Convert.ToBoolean(Application.Current.Properties["tAutoTest"]);

            if (nodenumberMin > nodenumberMax) nodenumberMax = nodenumberMin;
            if (txbIloscGoniacych > txbIloscGoniacychMax) txbIloscGoniacychMax = txbIloscGoniacych;
            if (txbAlfaBetaDepthMin > txbAlfaBetaDepthMax) txbAlfaBetaDepthMax = txbAlfaBetaDepthMin;
        }
        #endregion


    }
    #region KLASA DO OBSŁUGI DATAGRID
    /// <summary>
    /// Class created for presenting output of tests in datagrid
    /// </summary>
    public class outputClass
    {
        private int m_testNumber;
        private string m_graphClass;
        private string m_Winner;
        private string m_algorithm;
        private int m_copNumber;
        private int m_nodes;
        private int m_verticies;
        private int m_moves;
        private int m_maxMoves;
        private int m_iterations;
        public int TestNumber
        {
            get { return m_testNumber; }
        }
        public string GraphClass
        {
            get { return m_graphClass; }
        }
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
        public int Iterations
        {
            get { return m_iterations; }
        }

        public outputClass(int TestNumber, string GraphClass, string Winner, string Algorithm, int copNumber, int Nodes, int Vericies, int Moves, int MaxMoves, int Iterations)
        {
            this.m_testNumber = TestNumber;
            this.m_graphClass = GraphClass;
            this.m_Winner = Winner;
            this.m_algorithm = Algorithm;
            this.m_nodes = Nodes;
            this.m_verticies = Vericies;
            this.m_moves = Moves;
            this.m_maxMoves = MaxMoves;
            this.m_copNumber = copNumber;
            this.m_iterations = Iterations;
        }

        public override string ToString()
        {
            return string.Format(@"{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9} ",
                this.m_testNumber,
                this.m_graphClass,
                this.m_Winner,
                this.m_algorithm,
                this.m_nodes,
                this.m_verticies,
                this.m_moves,
                this.m_maxMoves,
                this.m_copNumber,
                this.m_iterations
                ); 
        }
    }
    #endregion
}
