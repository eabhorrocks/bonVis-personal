using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using OpenTK;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class flowMove
{
    public float sepRadius { get; set; }
    public float sepWeight { get; set; }
    public int sepFreq { get; set; }
    public IObservable<Tuple<Vector2[], Vector2[,], Vector2[], Vector2[], List<int>[,]>> Process(IObservable<Tuple<Vector2[], Vector2[,], Vector2[], Vector2[], List<int>>> source)
    {
        return source.Select(value => 
        {
            Vector2[] dotPos = value.Item1;
            Vector2[,] FlowField = value.Item2;
            List<int> sepCounter = value.Item5;
            int nDots = dotPos.Length;
            int rows = FlowField.GetLength(0);
            int cols = FlowField.GetLength(1);            
            float xbinsize = 2f/cols;
            float ybinsize = 2f/rows;

            Vector2[] binPos = new Vector2[nDots];
            Vector2[] flowVelocity = new Vector2[nDots]; // desired
            Vector2[] Separating = new Vector2[nDots];
            Vector2[] Velocity = new Vector2[nDots]; // tracks actual velocity? (t-1?)


            List<int>[,] dotLists = new List<int>[rows,cols];                
            List<int> closeDots = new List<int>();
            float a = 1f/(sepRadius*sepRadius);
            Vector2[] vel = new Vector2[nDots];
            Vector2[] Steer = new Vector2[nDots];


            // initialise dot list array
            for (int irow=0; irow<rows; irow++)
            {
                for (int icol=0; icol<cols; icol++)
                {
                    dotLists[icol,irow] = new List<int>();
                }
            }
            
            //wrapAround(dotPos, nDots);


            // Get bin poses, update dot lists in bin grid
            for (int i = 0; i < nDots; i++)
            {
                binPos[i].X = (int)Math.Floor((dotPos[i].X+1)/xbinsize);
                binPos[i].Y = (int)Math.Floor((dotPos[i].Y+1)/ybinsize);
                flowVelocity[i] = FlowField[(int)binPos[i].X,(int)binPos[i].Y];                
                dotLists[(int)binPos[i].X, (int)binPos[i].Y].Add(i);
            }


            // Separate
            for (int i = 0; i < nDots; i++)
            {
                sepCounter[i] = (sepCounter[i] + 1) % sepFreq;
                if(sepCounter[i] == 0) //only separate every sepFreq frames
                {
                
                closeDots.Clear();
                int xbin = (int)binPos[i].X;
                int ybin = (int)binPos[i].Y;

                for (int ix = xbin-1; ix<=xbin+1; ix++)
                {
                    for (int iy = ybin-1; iy<=ybin+1; iy++)
                    {
                        int iy2 = iy;
                        int ix2 = ix;

                        if (iy < 0) { iy2 = rows-1; }
                        if (iy >= rows) { iy2 = 0; }
                        if (ix < 0) { ix2 = cols-1; }
                        if (ix >= cols) { ix2 = 0; }

                        closeDots.AddRange(dotLists[ix2,iy2]);
                        
                    }
                }

                foreach (var dotid in closeDots)
                {
                    
                    float dx = dotPos[i].X - dotPos[dotid].X;
                    float dy = dotPos[i].X - dotPos[dotid].Y;

                    if (dx*dx + dy*dy < sepRadius*sepRadius)
                    {

                    if (Math.Abs(dx) > 1)
                    {
                        if (dx < 0) dx += 2;
                        else dx -= 2;
                    }

                    if (Math.Abs(dy) > 1)
                    {
                        if (dy < 0) dy+=2;
                        else dy -=2;
                    }

                    float d2 = a * dx *dx + a * dy * dy; // will be larger for further objects

                    if (d2 > 0)
                    {
                    Vector2 diff = new Vector2(dx,dy);
                    diff.Normalize();
                    diff = Vector2.Multiply(diff, 1f/d2);                
                    Steer[i] = Vector2.Add(Steer[i], diff);
                    }
                    }
                    
                }
                if (Steer[i].LengthSquared > 0)
                {
                if (Steer[i] == Vector2.Zero)
                {
                }
                else{
                Steer[i].Normalize();                   
                //Console.WriteLine(Steer);
                Steer[i] = Vector2.Multiply(Steer[i], 0.02f); // max speed
                Steer[i] = Vector2.Subtract(Steer[i], flowVelocity[i]);
                }
                Separating[i] = Vector2.Multiply(Steer[i], sepWeight);
                }
            }
            }


            
            for (int i = 0; i < nDots; i++)
            {   
                vel[i] = Vector2.Add(flowVelocity[i], Separating[i]);
                dotPos[i] = Vector2.Add(dotPos[i], vel[i]);

            }
            
            var result = new Tuple<Vector2[], Vector2[,], Vector2[], Vector2[], List<int>[,]>
            (dotPos, FlowField, vel, binPos, dotLists);
            //var result = new Tuple<Vector2[], Vector2[,], Vector2[]>(dotPos, FlowField, Velocities);

            return result;



        });
    }

    private void wrapAround(Vector2[] dotPos, int nDots)
    {
        for (int i=0; i < nDots; i++)
        {
        if(dotPos[i].X >= 1) {dotPos[i].X = -0.999f;}
        else if(dotPos[i].X <= -1) {dotPos[i].X = 0.999f;}
        if(dotPos[i].Y >= 1){dotPos[i].Y = -0.999f;}
        else if(dotPos[i].Y <= -1){dotPos[i].Y = 0.999f;}
        }
    }
}
