using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliacSearchAlgo;

namespace AISearchSample
{
    class Fringe2:Fringes
{
        //Stack<Node> s;
        Queue<Node> q;
        public Fringe2() 
        {
            //s = new Stack<Node>();
           q = new Queue<Node>();
        }
        
        public void add(Node n,Node origin)
        {
            n.Origin = origin;
            //s.Push(n);
            q.Enqueue(n);
        }

        public Node remove()
        {
            if (s.Count != 0)
                 return q.Dequeue();
                 //return s.Pop();
            return null;
        }
    }
}
