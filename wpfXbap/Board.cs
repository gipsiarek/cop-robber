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
    #region NODE CLASS
    public class Node : Page
    {
        public double x, y;
        public int number;
        public Ellipse elly;
        public bool isDrawn;

        public Node(double x, double y, Ellipse el, int digit)
        {
            this.x = x;
            this.y = y;
            this.elly = el;
            this.number = digit;
            this.isDrawn = false;
        }
        public Node(Point node)
        {
            this.x = node.X;
            this.y = node.Y;
            this.elly = new Ellipse();
            this.number = -1;
            this.isDrawn = false;
        }
        
    }
    #endregion
    public class Board : Page
    {
        #region members of board
        private int width;
        private int height;
        private int count;
        private string type;
        public List<List<int>> neighbor;
        public List<List<int>> neighborForClass;
        public List<Node> vertex;
        public List<int> vertexTest;
        public double distance;
        public int verticies;
        public bool isCopWinGraph;
        public Random random = new Random(Convert.ToInt32(DateTime.Now.Ticks % 0x7FFFFFFF));
        #endregion
        /// <summary>
        /// constructor for game with user
        /// </summary>
        public Board(int vertical, int horizontal, string type, int boardWidth, int boardHeight)
        {
            width = height = 17;
            distance = height*2+10;
            this.type = type;
            generateUserGraph(vertical, horizontal, getStartPositionX(vertical, boardWidth), getStarPositionY(horizontal, boardHeight));
            neighborForClass = new List<List<int>>();
            foreach (List<int> tmp in neighbor)
            {
                neighborForClass.Add(new List<int>(tmp));
            }
            isCopWinGraph = true;

            count = vertical + horizontal;
        }
        
        /// <summary>
        /// constructor for test
        /// get random values of nodes between 'nodeNumberMin' and nodeNumberMax'
        /// </summary>
        /// <param name="maxNodeSt">max degree of node</param>
        public Board(int nodeNumberMin, int nodeNumberMax, int maxNodeSt)
        {
            int nodeNumber = random.Next(nodeNumberMin, nodeNumberMax);
            do
            {
                generateTestGraph(nodeNumber, out verticies, maxNodeSt);
            } while (!isCompact(neighbor, nodeNumber));
            neighborForClass = new List<List<int>>();
            foreach (List<int> tmp in neighbor)
            {
                neighborForClass.Add(new List<int>(tmp));
            }
            isCopWinGraph = true;
        }


        #region MAIN FUNCTIONS
        /// <summary>
        /// create a point on graph with user game
        /// </summary>
        /// <param name="x and y"> positions of point</param>
        /// <param name="i">point number in list</param>
        public Node point(double x, double y, int i)
        {
            Ellipse myEllipse = new Ellipse();
            myEllipse = drawEllipse(myEllipse);
            myEllipse.SetValue(Canvas.LeftProperty, x);       
            myEllipse.SetValue(Canvas.TopProperty, y);
            myEllipse.Name = "node" + i.ToString();
            Node punkt = new Node(x, y, myEllipse, i);
            return punkt;
        }
        public Node pointCop(Point point, Node node)
        {
            if (!node.isDrawn)
            {
                node.elly = drawCop(node.elly);
                node.elly.Name = "cop";
            }
            
            node.elly.SetValue(Canvas.LeftProperty, point.X);
            node.elly.SetValue(Canvas.TopProperty, point.Y);

            return node;
        }
        public Node pointRobber(Point point, Node node)
        {
            node.elly = drawRobber(node.elly);
            node.elly.SetValue(Canvas.LeftProperty, point.X);
            node.elly.SetValue(Canvas.TopProperty, point.Y);
            node.elly.Name = "robber";
            return node;
        }
        #endregion

        #region DRAWING
        /// <summary>
        /// normal node
        /// </summary>
        public Ellipse drawEllipse(Ellipse elly)
        {
            elly.Fill = Brushes.YellowGreen;
            elly.StrokeThickness = 1;
            elly.Stroke = Brushes.Black;
            elly.Width = width;                              
            elly.Height = height;
            return elly;
        }
        public Ellipse drawRobber(Ellipse elly)
        {

            elly.StrokeThickness = 2;
            elly.Stroke = Brushes.Black;
            RadialGradientBrush radialGradient = new RadialGradientBrush();
            radialGradient.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF450D0D"), 1.0));
            radialGradient.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFF04646"), 0.224));
            elly.Fill = radialGradient;
            elly.Width = width;
            elly.Height = height;
            return elly;
        }
        public Ellipse drawCop(Ellipse elly)
        {

            elly.StrokeThickness = 2;
            elly.Stroke = Brushes.Black;
            RadialGradientBrush radialGradient = new RadialGradientBrush();
            radialGradient.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF0D1545"), 1.0));
            radialGradient.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF3FCDCB"), 0.0));
            elly.Fill = radialGradient;
            elly.Width = width;
            elly.Height = height;
            return elly;
        }
        public Line drawLine(int i, int j)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = vertex[i].x + (height/2);
            myLine.Y1 = vertex[i].y + (height / 2);
            myLine.X2 = vertex[j].x + (height / 2);
            myLine.Y2 = vertex[j].y + (height / 2);
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            return myLine;
        }


        #endregion

        #region GENERATE GRAPH
        //generate the neighbor list and add vertex to graph
        private void generateUserGraph(int v, int h, int startPosX, int startPosY)
        {
            neighbor = new List<List<int>>();
           
            vertex = new List<Node>();
            for (int x = 0; x < v * h; x++)
                neighbor.Add(new List<int>());
            if(type=="c3")
            for (int i = 0; i < h*v; i++)
            {
                List<int> tmpList = new List<int>();
                tmpList= generateNeighborC3(i, v, h);
                vertex.Add(point((i % v) * distance + startPosX, i / v * distance + startPosY, i));
                foreach(int x in tmpList)
                    neighbor[i].Add(x);
            }
            else if (type == "c4")
                for (int i = 0; i < h * v; i++)
                {
                    List<int> tmpList = new List<int>();
                    tmpList = generateNeighborC4(i, v, h);
                    vertex.Add(point((i % v) * distance + startPosX, i / v * distance + startPosY, i));
                    foreach (int x in tmpList)
                        neighbor[i].Add(x);
                }
            else if (type == "random")
                for (int i = 0; i < h * v; i++)
                {
                    List<int> tmpList = new List<int>();
                    tmpList = generateRandom(i, v, h);
                    vertex.Add(point((i % v) * distance + startPosX, i / v * distance + startPosY, i));
                    foreach (int x in tmpList)
                    {
                        neighbor[i].Add(x);
                        neighbor[x].Add(i);
                    }
                }

        }
        private void generateTestGraph(int nodeNumber, out int verticies, int maxNodeSt)
        {
            neighbor = new List<List<int>>();
            verticies = 0;
            
            for (int i = 0; i < nodeNumber; i++)
            {
                neighbor.Add(new List<int>());
            }            
            for (int i = 0; i < nodeNumber; i++)
            {
                List<int> tmpList = generateTest(i, nodeNumber, maxNodeSt, neighbor[i].Count);
                foreach (int x in tmpList)
                {
                    if (!neighbor[i].Contains(x))
                    {
                        
                        neighbor[i].Add(x);
                        neighbor[x].Add(i);
                        verticies++;
                       
                    }
                }
            }
        }

        private List<int> generateNeighborC3(int i, int v, int h)
        {
            List<int> neigh = new List<int>();
            if ((i+1)%v != 0 && (i+1)%v < v && i+1<v*h) neigh.Add(i + 1);
            if (i - 1 >= 0 && (i - 1) % v != v - 1) neigh.Add(i - 1);
            if (i + v < v * h) neigh.Add(i + v);
            if (i + v + 1 < v * h && (i + v + 1) % v != 0) neigh.Add(i + v + 1);
            if (i + v - 1 < v * h && (i + v - 1) % v != v - 1) neigh.Add(i + v - 1);
            if (i - v >= 0) neigh.Add(i - v);
            if (i - v + 1 >= 0 && (i - v + 1) % v != 0) neigh.Add(i - v + 1);
            if (i - v - 1 >= 0 && (i - v - 1) % v != v - 1) neigh.Add(i - v - 1);
            return neigh;
        }
        private List<int> generateNeighborC4(int i, int v, int h)
        {
            List<int> neigh = new List<int>();
            if ((i + 1) % v != 0 && (i + 1) % v < v && i + 1 < v * h) neigh.Add(i + 1);
            if (i - 1 >= 0 && (i - 1) % v != v - 1) neigh.Add(i - 1);
            if (i + v < v * h) neigh.Add(i + v);
            if (i - v >= 0) neigh.Add(i - v);
            return neigh;
        }
        private List<int> generateRandom(int i, int v, int h)
        {
            List<int> neigh = new List<int>();
            
            int tmp, prog;
            prog = 400;
            if ((i + 1) % v != 0 && (i + 1) % v < v && i + 1 < v * h)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i + 1);
            }
            if (i - 1 >= 0 && (i - 1) % v != v - 1)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i - 1);
            }
            if (i + v < v * h)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i + v);
            }
            if (i + v + 1 < v * h && (i + v + 1) % v != 0)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i + v + 1);
            }
            if (i + v - 1 < v * h && (i + v - 1) % v != v - 1)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i + v - 1);
            }
            if (i - v >= 0)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i - v);
            }
            if (i - v + 1 >= 0 && (i - v + 1) % v != 0)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i - v + 1);
            }
            if (i - v - 1 >= 0 && (i - v - 1) % v != v - 1)
            {
                tmp = random.Next(999);
                if (tmp < prog) neigh.Add(i - v - 1);
            }
            System.Threading.Thread.Sleep(100);
            return neigh;
        }
        private List<int> generateTest(int i, int v, int maxNode, int connectionCount)
        {            
            List<int> neigh = new List<int>();
            try
            {
                neigh.Add((i + 1) % v);
                connectionCount++;
            }
            catch (Exception e)
            {
                MessageBox.Show("bla bla bla" + i + e);
            }
                for (int x = 0; x < v; x++)
                {
                    if (connectionCount >= maxNode)
                        break;
                    if (x!=i && (random.Next()%900) < 100 && neighbor[x].Count<(maxNode))
                    {
                        if (!neigh.Contains(x))
                        {
                            neigh.Add(x);
                            connectionCount++;
                        }
                    }
                }
            return neigh;           
        }
        #endregion

        #region FUNCTIONS
        /// <summary>
        /// return neighborhood of point
        /// </summary>
        public List<int> findNeighbors(int number)
        {
            List<int> list = new List<int>();
            list = neighbor[number];
            return list;
        }
        public List<int> findNeighbors(int number, bool forTest)
        {
            List<int> list = new List<int>();
            list = neighborForClass[number];
            return list;
        }
        /// <summary>
        /// gets center of the screen to draw graph
        /// </summary>
        private int getStartPositionX(int nodes, int boardWidth)
        {
            return boardWidth / 2 - (nodes / 2 * (int)distance); 
        }
        private int getStarPositionY(int nodes, int boardHeight)
        {
            return boardHeight / 2 - (nodes / 2 * (int)distance); 
        }
        private bool isCompact(List<List<int>> neighbor, int nodenumber)
        {
            int[] tmp = new int[nodenumber];
            int nCount = 0;
            foreach (List<int> item in neighbor)
            {
                foreach (int node in item)
                {
                    if (tmp[node] != 1)
                    {
                        tmp[node] = 1;
                        nCount++;
                    }
                }
            }
            if (nCount!=nodenumber)
            {
                return false;
            }
            return true;
        }
        public static List<List<int>> removePitfalls(Board board)
        {
            int numberOfNodes = board.neighborForClass.Count();
            bool isEqual=true;
            for (int i = 0; i < numberOfNodes; i++)
            {
                List<int> nodeList = board.neighborForClass[i];
                if (nodeList.Count != 0)
                    for (int j = 0; j < nodeList.Count; j++)
                    {
                        isEqual = true;
                        int node = nodeList[j];
                        List<int> robPos = new List<int>(nodeList);
                        List<int> copPos = new List<int>(board.findNeighbors(node, true));
                        copPos.Add(node);
                        robPos.Add(i);
                        copPos.Sort();
                        robPos.Sort();

                        for (int k = 0; k < robPos.Count; k++)
                        {
                            if (!copPos.Contains(robPos[k]))
                            {
                                isEqual = false;
                                break;
                            }
                        }
                        if (isEqual)
                        {
                            // nodeList.Remove(i);
                            foreach (List<int> tmp in board.neighborForClass)
                            {
                                if (tmp.Contains(i))
                                    tmp.Remove(i);
                            }
                            nodeList.Clear();
                            board.neighborForClass = removePitfalls(board);
                            nodeList = board.neighborForClass[0];
                            i = 0; j = 0;
                            break;
                        }

                    }
            }
            return board.neighborForClass;
        }
       
        #endregion

    }
}
        