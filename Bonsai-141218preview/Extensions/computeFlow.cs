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
public class computeFlow
{
    // Border params
    public float left { get; set; }  public float right { get; set; }
    public float top { get; set; }  public float bottom { get; set; }

    public float maxForce { get; set; }  public float maxSpeed { get; set; }
    public float sepRadius { get; set; } public float sepWeight { get; set; }
    public int sepFreq { get; set; }
    public IObservable<Tuple<Vector2[], Vector2[], Vector2[], Vector2[], int[], List<int>[,], Vector2[,]>> Process(IObservable<Tuple<Vector2[], Vector2[], Vector2[], Vector2[], int[], List<int>[,], Vector2[,]>> source)
    {
        return source.Select(value => 
        {
            // Variables
            Vector2[] dotPos = value.Item1;
            Vector2[] Velocity = value.Item2;
            Vector2[] Desired = value.Item4;
            int[] sepCounter = value.Item5;
            List<int>[,] dotLists = value.Item6;
            Vector2[,] FlowField = value.Item7;
            int nDots = dotPos.Length;
            // Intermediate
            Vector2[] flowVelocity = new Vector2[nDots]; // computed each frame
            List<int> closeDots = new List<int>();
            Vector2[] binPos = new Vector2[nDots];
            Vector2[] Steer = new Vector2[nDots];
            Vector2[] Separating = new Vector2[nDots];
            Vector2[] Acceleration = new Vector2[nDots];


            // Params
            int rows = FlowField.GetLength(0);
            int cols = FlowField.GetLength(1);            
            float xSize = Math.Abs(left-right);
            float ySize = Math.Abs(top-bottom);
            float xbinsize = xSize/(float)cols;
            float ybinsize = ySize/(float)rows;
            float[] borders = { left, right, top, bottom };
            float a = sepRadius * sepRadius;
            float sepTerm = 1f/a;

            
            // Get binPos, flowVelocity, compute 'desired', create dotlists grid
            for (int i = 0; i < nDots; i++)
            {
                binPos[i].X = (int)Math.Floor((dotPos[i].X+(xSize/2f))/xbinsize);
                binPos[i].Y = (int)Math.Floor((dotPos[i].Y+(ySize/2f))/ybinsize);
                flowVelocity[i] = FlowField[(int)binPos[i].X,(int)binPos[i].Y];
                Steer[i].X = 0;
                Steer[i].Y = 0;

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
                foreach (int otherdot in closeDots)
                {
                    float dx = dotPos[i].X - dotPos[otherdot].X;
                    float dy = dotPos[i].X - dotPos[otherdot].Y;                 

                    if (Math.Abs(dx) > xSize/2f)
                    {
                        if (dx < 0) dx += xSize;
                        else dx -= xSize;
                    }
                    if (Math.Abs(dy) > 1)
                    {
                        if (dy < 0) dy += ySize;
                        else dy -= ySize;
                    }
                    float d2 = dx*dx + dy*dy;
                    if (d2 < sepRadius)
                    {
                    if (d2 > 0) // only do for forces within a close range...
                    {
                    Vector2 diff = new Vector2(dx,dy);
                    diff = Vector2.Multiply(diff, 1f/d2);  
                    //Console.WriteLine("diff" + diff);       
                    Steer[i] = Vector2.Add(Steer[i], diff);
                    }

                    }
                }
                if (Steer[i].X + Steer[i].Y > 0)
                {
                Steer[i].Normalize();
                }
                Separating[i] = Vector2.Multiply(Steer[i], sepWeight);
                }
                else
                {
                   //Separating[i] = Vector2.Zero;
                }

              //  Desired[i] = Vector2.Subtract(flowVelocity[i], Velocity[i]);
              //  Acceleration[i] = Vector2.Add(Desired[i], Separating[i]);
                
                Velocity[i] = flowVelocity[i] + Separating[i]; // limit to max speed?   
                if (Velocity[i].LengthSquared > maxSpeed * maxSpeed)             
                {
                    Velocity[i].Normalize();
                    Velocity[i] = Vector2.Multiply(Acceleration[i], maxSpeed);
                }
                Console.WriteLine(Velocity[i]);
                dotPos[i] = dotPos[i] + Velocity[i];
            }
            
            wrapAround(dotPos, nDots, borders);

            var result = new Tuple<Vector2[], Vector2[], Vector2[], Vector2[], int[], List<int>[,], Vector2[,]>
            (dotPos, Velocity, Acceleration, Desired, sepCounter, dotLists, FlowField);

            return result;
            });
    }

    private void wrapAround(Vector2[] dotPos, int nDots, float[] borders)
    {
        for (int i=0; i < nDots; i++)
        {
        if(dotPos[i].X >= borders[1]) {dotPos[i].X = borders[0]+0.01f;}
        else if(dotPos[i].X <= borders[0]) {dotPos[i].X = borders[1]-0.01f;}
        if(dotPos[i].Y >= borders[2]){dotPos[i].Y = borders[3]+0.01f;}
        else if(dotPos[i].Y <= borders[3]) {dotPos[i].Y = borders[2]-0.01f;}
        }
    }

}