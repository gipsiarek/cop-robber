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
    public class Robber : Page
    {
        public Node myNode;
        public List<int> myNeighbors;
        public int ocpupiedNode;
        public int startNode;
        public int movesSoFar;
        public List<int> myPath;
        /// <summary>
        /// creates ellipse on a board in Point and gives it number i
        /// </summary>
        public Robber(Board board, Point node, int i)
        {
            myNode = new Node(node);
            myNeighbors = new List<int>();
            myNode.number = i;
            movesSoFar = 0;
            myPath = new List<int>();
            board.pointRobber(node, myNode);
            myNeighbors = board.findNeighbors(myNode.number);
        }
        /// <summary>
        /// robber for test uses
        /// </summary>
        public Robber(int startNode, Board board)
        {
            myPath = new List<int>();
            ocpupiedNode = startNode;
            movesSoFar = 0;
            myNeighbors = new List<int>();
            myNeighbors = board.findNeighbors(ocpupiedNode);
        }
        /// <summary>
        /// move and find neighbor list
        /// </summary>
        /// <param name="node">node number to move to</param>
        /// <param name="board">board for witch we find neighborhood</param>
        public void move(int node, Board board)
        {
            ocpupiedNode = node;
            movesSoFar++;
            myNeighbors = board.findNeighbors(ocpupiedNode);
        }
    }
}
