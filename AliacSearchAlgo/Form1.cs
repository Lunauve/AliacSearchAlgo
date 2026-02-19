using AISearchSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AliacSearchAlgo
{
    public partial class Form1 : Form
    {
        ArrayList nodes; // set of nodes for display of points
        int count;
        int state = 0; // 0=normal, 1=move , 2 remove, 3 connect nodes;
        bool open;
        bool movetoggle;
        bool connectoggle;
        int from;
        int to;
        int start;
        int goal;
        bool showHillDistances = false;
        Node explored;
        Search search;
        HillSearch hillsearch;

        public Form1()
        {
            InitializeComponent();
            nodes = new ArrayList();
            state = 0;
            open = true;
            count = 0x41;
            movetoggle = false;
            connectoggle = false;
            from = -1;
            to = -1;
            start = -1;
            goal = -1;
            explored = null;
            search = null;
           
                        
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string searchopen(int ex,int ey)
        {
            String str="";
            open = true;
            for (int x = 0; x < nodes.Count; x++)
            {
                Node temp = (Node)nodes[x];
                if (ex >= temp.X && ex <= ((temp.X) + 10))
                {
                    if (ey >= temp.Y && ey <= ((temp.Y) + 10))
                    {
                        open = false;
                        str = temp.Name;
                    }
                }
               
               
            }
            return str;
        }


        public int searchMove(int ex, int ey) // gets the index number of the node found to be movable
        {
            int p = -1;
            for (int x = 0; x < nodes.Count; x++)
            {
                Node temp = (Node)nodes[x];
                if (ex >= temp.X && ex <= ((temp.X) + 10))
                {
                    if (ey >= temp.Y && ey <= ((temp.Y) + 10))
                    {
                        p = x;
                    }
                }
            }
            return p;
        }


        public int searchMovedNode(int ex, int ey) // looks for the one object in the nodes that can be moved
        {
            int p = -1;
            for (int x = 0; x < nodes.Count; x++)
            {
                Node temp = (Node)nodes[x];
                if (temp.Moved == true)
                    p = x;
            }
            return p;
        }
       

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            label1.Text = searchopen(e.X,e.Y)+"(" + e.X + ", " + e.Y + ")";
           
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
          // this creates a node with the current coordinates
            if (state == 0) // normal mode
            {   
                  if (open)
                    {
                        Node n = new Node();
                        n.Name=""+(char)count;
                        count++;
                        n.X = e.X;
                        n.Y = e.Y;
                        nodes.Add(n);
                        pictureBox1.Refresh();
                    }                
                
            }
            if (state == 1) // move mode
            {
                int m = searchMove(e.X, e.Y);
                if (movetoggle == false)
                {
                    if (m != -1) // checks is a movable node is found
                    {
                        ((Node)nodes[m]).Moved = true;
                        movetoggle = true;
                        pictureBox1.Refresh();
                    }
                }
                else
                {
                   
                    if (m == -1) // checks for open locations
                    {
                        int y= searchMovedNode(e.X, e.Y);
                        //MessageBox.Show("" + ((Node)nodes[y]).Name + "=" + ((Node)nodes[y]).X + "  " + ((Node)nodes[y]).Y);
                        ((Node)nodes[y]).Moved = false;
                        ((Node)nodes[y]).X = e.X;
                        ((Node)nodes[y]).Y = e.Y;
                         movetoggle=false;
                         pictureBox1.Refresh();
                    }
                }
                
            }
            if (state == 2) // remove mode
            {
                int m = searchMove(e.X, e.Y);
                if (m != -1)
                {
                    nodes.RemoveAt(m);
                    pictureBox1.Refresh();
                }
            }
            if (state == 3) // connect nodes
            {
                int m = searchMove(e.X, e.Y);
                if (connectoggle == false)
                {
                    if (m != -1)
                    { from = m;
                    connectoggle = true;
                    pictureBox1.Refresh();
                    }
                }
                else {
                    if (m != -1 && m!=from)
                    {
                        to = m;
                        ((Node)(nodes[from])).addNeighbor(((Node)(nodes[to])));
                        ((Node)(nodes[to])).addNeighbor(((Node)(nodes[from])));
                        connectoggle = false;
                        from = -1;
                        to = -1;
                        pictureBox1.Refresh();
                    }
                }
            
            }
            if(state==4)  // set start node
            {
                int m = searchMove(e.X, e.Y);
                if (m != -1)
                {
                  //  search.setStart(((Node)nodes[m]));
                  //  explored = ((Node)nodes[m]);
                    start = m;
                    pictureBox1.Refresh();
                }          
            }
            if (state == 5) // set goal node
            {
                int m = searchMove(e.X, e.Y);
                if (m != -1)
                {
                   // search.setGoal(((Node)nodes[m]));
                    goal = m;
                    pictureBox1.Refresh();
                }
            }
       }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 0; // normal mode settings for the nodes
        }

        private void moveNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 1; // move seeting for the nodes
        }

        private void removeNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 2; //remove settings for the nodes
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(nodes!=null)
            for (int x = 0; x < nodes.Count; x++)
            {
               Node temp = (Node)nodes[x];

               if (temp.Moved==false)
                g.DrawArc(Pens.Black, temp.X, temp.Y, 10, 10, 0, 360);
               else
                g.DrawArc(Pens.Yellow, temp.X, temp.Y, 10, 10, 0, 360);
               if(x==from)
                g.DrawArc(Pens.White, temp.X, temp.Y, 10, 10, 0, 360);


                g.DrawString(
                     temp.Name,
                     new Font("Segoe UI", 10),
                     Brushes.Black,
                     temp.X + 12,
                     temp.Y + 2
                );

               ArrayList connects = temp.getNeighbor();
               for (int y = 0; y < connects.Count; y++)
               {
                   Node neighbor = (Node)connects[y];
                   g.DrawLine(Pens.Black, temp.X + 5, temp.Y + 5, neighbor.X + 5, neighbor.Y + 5);
               }

               // highlight neighbor node that is being checked for distance.
               if (showHillDistances && temp.Checking)
               {
                   int glowSize = 18;
                   int offset = (glowSize - 10) / 2;
                   g.FillEllipse(Brushes.DeepSkyBlue, temp.X - offset, temp.Y - offset, glowSize, glowSize);
               }

               if (temp.Expanded)
                   g.FillEllipse(Brushes.Red, temp.X-5, temp.Y-5, 16, 16);

               if (x == start) {
                   int cx = temp.X + 5;
                   int cy = temp.Y + 5;

                   int circleRadius = 6;
                   int totalHeight = 20;

                   int circleTopY = cy - totalHeight;

                   g.FillEllipse(
                       Brushes.SeaGreen,
                       cx - circleRadius,
                       circleTopY,
                       circleRadius * 2,
                       circleRadius * 2
                   );

                   Point[] tip = new Point[] {
                       new Point(cx - 4, circleTopY + circleRadius * 2 - 2),
                       new Point(cx + 4, circleTopY + circleRadius * 2 - 2),
                       new Point(cx, cy - 2)
                   };

                   g.FillPolygon(Brushes.SeaGreen, tip);

                   g.FillEllipse(
                       Brushes.White,
                       cx - 2,
                       circleTopY + circleRadius - 2,
                       4,
                       4
                   );
               }

               if (x == goal) {
                   int cx = temp.X + 5;
                   int cy = temp.Y + 5;

                   int poleHeight = 18;

                   g.DrawLine(
                        new Pen(Color.Black, 2),
                        cx,
                        cy - poleHeight,
                        cx,
                        cy - 2
                   );

                   Point[] flag = new Point[] {
                        new Point(cx, cy - poleHeight),
                        new Point(cx + 12, cy - poleHeight + 4),
                        new Point(cx, cy - poleHeight + 8)
                   };

                   g.FillPolygon(Brushes.Red, flag);
               }

               // show distance for neighbor node being checked.
               if (showHillDistances && (temp.TempDistance > 0 || (x == goal && (temp.Checking || temp == explored)))) {
                        double displayDistance = (x == goal && (temp.Checking || temp == explored)) ? 0 : temp.TempDistance;

                   g.DrawString(
                        $"   ({temp.TempDistance:F1})",
                        new Font("Segoe UI", 9),
                        Brushes.DarkBlue,
                        temp.X + 12,
                        temp.Y + 2
                   );
               }
            }

            Node path = explored;
            while (path != null && path.Origin != null)
            {
                g.DrawLine(new Pen(Brushes.Green, 5), path.X + 5, path.Y + 5, path.Origin.X + 5, path.Origin.Y + 5);
                path = path.Origin;
            }
           
        }

        private void saveNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            
        }

       
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                var serializer = new BinaryFormatter();
                using (var stream = File.OpenWrite(saveFileDialog1.FileName))
                {
                    serializer.Serialize(stream, nodes);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected Error in Saving");
            }

         
        }

        private void loadNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var serializer = new BinaryFormatter();
            using (var stream = File.OpenRead(openFileDialog1.FileName))
            {
                nodes = (ArrayList)serializer.Deserialize(stream);
                count += nodes.Count;
                explored = null;
                search = null;
            }
            start = -1;
            goal = -1;
            pictureBox1.Refresh();
        }

        private void connectNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 3; // connect nodes
        }

        private void setStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 4; // setting start node
        }

        private void setGoalNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 5; // setting goal node
        }

        private void depthFirstSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void fullSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                ArrayList tnodes = new ArrayList(nodes);
                search = new Search(tnodes, 1);
                search.setStart(((Node)tnodes[start]));
                explored = ((Node)tnodes[start]);
                search.setGoal(((Node)tnodes[goal]));
                explored = search.search();
                pictureBox1.Refresh();
            }
            else {
                MessageBox.Show("Start and Goal Nodes not set.");
            }
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                if (search == null)
                {
                    ArrayList tnodes = new ArrayList(nodes);
                    search = new Search(tnodes, 1);
                    search.setStart(((Node)tnodes[start]));
                    explored = ((Node)tnodes[start]);
                    search.setGoal(((Node)tnodes[goal]));
                }
                explored = search.searchone();
                if (explored.Goal)
                    MessageBox.Show(explored.Name + " found");
                pictureBox1.Refresh();
            }
            else {
                MessageBox.Show("Start or Goal nodes not set.");
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = 0;
            open = true;
            movetoggle = false;
            connectoggle = false;
            from = -1;
            to = -1;
            start = -1;
            goal = -1;
            explored = null;
            search = null;
            showHillDistances = false;
            for (int y = 0; y < nodes.Count; y++)
            {
                ((Node)nodes[y]).Moved = false;
                ((Node)nodes[y]).Expanded = false;
                ((Node)nodes[y]).Goal = false;
                ((Node)nodes[y]).Origin = null;
                ((Node)nodes[y]).Start = false;
                ((Node)nodes[y]).TempDistance = 0;
            }
                pictureBox1.Refresh();
        }

        private void clearAllNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            explored = null;
            search = null;
            nodes = new ArrayList();
            state = 0;
            open = true;
            count = 0x41;
            movetoggle = false;
            connectoggle = false;
            from = -1;
            to = -1;
            start = -1;
            goal = -1;
            pictureBox1.Refresh();
        }

        private void stepToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                if (search == null)
                {
                    ArrayList tnodes = new ArrayList(nodes);
                    search = new Search(tnodes, 2);
                    search.setStart(((Node)tnodes[start]));
                    explored = ((Node)tnodes[start]);
                    search.setGoal(((Node)tnodes[goal]));
                }
                explored = search.searchone();
                if (explored.Goal)
                    MessageBox.Show(explored.Name + " found");
                pictureBox1.Refresh();
            }
            else { MessageBox.Show("Start or Goal nodes not Set."); }
        }

        private void fullSearchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                ArrayList tnodes = new ArrayList(nodes);
                search = new Search(tnodes, 2);
                search.setStart(((Node)tnodes[start]));
                explored = ((Node)tnodes[start]);
                search.setGoal(((Node)tnodes[goal]));
                explored = search.search();
                pictureBox1.Refresh();
            }
            else { MessageBox.Show("Start or Goal nodes not Set."); }
        }

        private void hillClimbingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fullSearchToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                try
                {
                    ArrayList tnodes = new ArrayList(nodes);
                    HillSearch hillsearch = new HillSearch(tnodes, start, goal);

                    Node resultNode = hillsearch.search();

                    explored = resultNode;

                    if (explored != null)
                    {
                        Node pathNode = explored;
                        while (pathNode != null)
                        {
                            pathNode.Expanded = true;
                            pathNode = pathNode.Origin;
                        }
                    }

                    pictureBox1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Start or Goal nodes not set.");
            }
        }


        private async void runWithDelayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                ArrayList tnodes = new ArrayList(nodes);
                search = new Search(tnodes, 1); // DFS
                search.setStart((Node)tnodes[start]);
                search.setGoal((Node)tnodes[goal]);

                explored = search.searchone();
                int delay = 500;

                while ((explored = search.searchone()) != null)
                {
                    pictureBox1.Refresh();
                    Application.DoEvents();
                    await Task.Delay(delay);

                    if (explored.Goal)
                    {
                        MessageBox.Show(explored.Name + " found");
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Start and Goal Nodes not set.");
            }
        }

        private async void runWithDelayToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                ArrayList tnodes = new ArrayList(nodes);
                search = new Search(tnodes, 2); // BFS
                search.setStart((Node)tnodes[start]);
                search.setGoal((Node)tnodes[goal]);

                explored = search.searchone();
                int delay = 500;

                while ((explored = search.searchone()) != null)
                {
                    pictureBox1.Refresh();
                    Application.DoEvents();
                    await Task.Delay(delay);

                    if (explored.Goal)
                    {
                        MessageBox.Show(explored.Name + " found");
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Start and Goal Nodes not set.");
            }
        }

        private void stepToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                try
                {
                    if (hillsearch == null)
                    {
                        ArrayList tnodes = new ArrayList(nodes);
                        hillsearch = new HillSearch(tnodes, start, goal);
                        explored = null;
                    }

                    Node stepNode = hillsearch.searchone();

                    if (stepNode != null)
                    {
                        explored = stepNode;
                        pictureBox1.Refresh();

                        if (stepNode.Goal)
                            MessageBox.Show("Goal reached: " + stepNode.Name);
                    }
                    else
                    {
                        MessageBox.Show("No further steps possible (local maximum reached).");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Start or Goal nodes not set.");
            }
        }

        private async void runWithDelayToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (start != -1 && goal != -1)
            {
                ArrayList tnodes = new ArrayList(nodes);
                hillsearch = new HillSearch(tnodes, start, goal);
                showHillDistances = true;

                Node current = hillsearch.searchone();
                explored = current;
                int delay = 1000;

                while (current != null)
                {
                    ArrayList neighbors = current.getNeighbor();

                    foreach (Node neighbor in neighbors)
                    {
                        neighbor.Checking = true;

                        pictureBox1.Refresh();
                        Application.DoEvents();
                        await Task.Delay(delay / 2); 

                        Node goalNode = (Node)tnodes[goal];
                        neighbor.TempDistance = Math.Sqrt(
                            Math.Pow(neighbor.X - goalNode.X, 2) +
                            Math.Pow(neighbor.Y - goalNode.Y, 2)
                        );

                        pictureBox1.Refresh();
                        Application.DoEvents();
                        await Task.Delay(delay / 2);

                        neighbor.Checking = false;

                        pictureBox1.Refresh();
                        Application.DoEvents();
                    }

                    Node next = hillsearch.searchone();

                    if (next != null)
                    {
                        next.Origin = explored;
                    }

                    explored = next;
                    current = next;

                    pictureBox1.Refresh();
                    Application.DoEvents();
                    await Task.Delay(delay);

                    if (current != null && current.Goal)
                    {
                        MessageBox.Show(current.Name + " found");
                        break;
                    }
                }

                if (current == null || !current.Goal)
                {
                    MessageBox.Show("Reached local maximum, goal not found.");
                }
                showHillDistances = false;
            }
            else
            {
                MessageBox.Show("Start and Goal Nodes not set.");
            }
        }

    }
}
