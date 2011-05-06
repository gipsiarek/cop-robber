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
    public class Cop : Page
    {
        public Node myNode;
        public List<int> myNeighbors;
        public int ocupiedNode;
        public int copId;
        public int startNode;
        /// <summary>
        /// creates ellipse on a board in Point and gives it number i
        /// </summary>
        public Cop(Board board, Point node, int i)
        {
            myNode = new Node(node);
            myNeighbors = new List<int>();
            myNode.number = i;
            board.pointCop(node, myNode);
            myNeighbors = board.findNeighbors(myNode.number);
        }
        /// <summary>
        /// cop for test uses
        /// </summary>
         public Cop(int startNode, Board board)
        {
            ocupiedNode = startNode;
            myNeighbors = new List<int>();
            myNeighbors = board.findNeighbors(ocupiedNode);
        }
        /// <summary>
        /// move and find neighbor list
        /// </summary>
        /// <param name="node">node number to move to</param>
        /// <param name="board">board for witch we find neighborhood</param>
        public void move(int node, Board board)
        {
            ocupiedNode = node;
            myNeighbors = board.findNeighbors(ocupiedNode);
        }

    }
}
