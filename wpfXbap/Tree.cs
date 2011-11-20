using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wpfXbap
{
    public class Tree<T> 
    {
        public Node<Data> root;
        public Node<Data> node;
        public Tree()
        {
            root = null;
        }

        public Tree(Node<Data> node)
        {
            this.node = node;
        }
        public void Clear(){
            this.node.Clear();
            this.root = null;
        }
        
    }
    public class Data
    {
        public Data()
        {
            copsPos = new int[5];
            RobberCopDistance = -1;
            counter = 0;
            visited = 0;
        }
        private int robberCopDistance;
        public int RobberCopDistance
        {
            get { return robberCopDistance; }
            set { robberCopDistance = value; }
        }
        private int robberPos;
        public int RobberPos
        {
            get { return robberPos; }
            set { robberPos = value; }
        }
        private int[] copsPos;
        public int[] CopPos
        {
            get { return copsPos; }
            set { copsPos = value; }
        }
        private double probability;
        public double Probability
        {
            get { return probability; }
            set { probability = value; }
        }
        private int counter;//jak daleko zeszlismy w drzewie
        public int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
        private int visited; //for backpropagation
        public int Visited
        {
            get { return visited; }
            set { visited = value; }
        }

    }
    public class Node<T>
    {
        private Data data;
        private Node<T> parent;
        private LinkedList<Node<T>> children;
        public void Clear()
        {
            children.Clear();
            parent = null;
            data = null;
        }
        public Node()
        {
            parent = null;
            children = new LinkedList<Node<T>>();
        }
        public Node(Node<T> parent)
        {            
            this.parent = parent;
            this.children = new LinkedList<Node<T>>();
        }
        public Node(Node<T> parent, Data data)
        {
            this.parent = parent;
            this.children = new LinkedList<Node<T>>();
            this.data = data;
        }
        public Node<T> GetParent()
        {
            return parent;
        }
        public void SetParent(Node<T> parent)
        {
            this.parent = parent;
        }
        public Data GetData()
        {
            return data;
        }
        public void SetData(Data data)
        {
            this.data = data;
        }
        public Node<T> AddChild(Node<T> child)
        {
            child.SetParent(this.parent);
            children.AddLast(child);
            return child;
        }
        public Node<T> AddChild(Data data)
        {
            Node<T> child = new Node<T>(this, data);
            children.AddLast(child);
            return child;
        }
        public Node<T> GetChild(int i)
        {
            return children.ElementAt(i);
        }
        public LinkedList<Node<T>> GetChildren()
        {
            return children;
        }
        public void RemoveChildren()
        {
            children.Clear();
        }
        public bool IsLeaf()
        {
            if (children.Count == 0) return true;
            else {
                foreach (Node<T> item in children)
                {
                    if (item.GetData().Visited == 0) return false;
                }
                return true;
            }
            
        }

        internal bool areAllChildrenLeaf()
        {
            foreach (Node<T> child in children)
            {
                if (child.IsLeaf()||child.GetData().Visited==1) continue;
                else return false;
            }
            return true;
        }
    }
}
