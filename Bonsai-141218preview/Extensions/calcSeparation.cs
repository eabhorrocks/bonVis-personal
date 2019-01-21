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
public class calcSeparation
{
    public float Left { get; set; } 
    public float Right { get; set; }
    public float Top { get; set; } 
    public float Bottom { get; set; }
    public float sepRadius { get; set; }
    public float sepWeight { get; set; }
    public int sepFreq { get; set; }
    public float maxSpeed {get; set;}
    public IObservable<Tuple<Vector2[], int[]>> Process(IObservable<Tuple<Vector2[], Vector2[], List<int>[,], int[]>> source)
    {
        return source.Select(value => 
        {
            // Inputs
            Vector2[] dotPos = value.Item1;
            Vector2[] binPos = value.Item2;
            List<int>[,] dotLists = value.Item3;
            int[] sepCounter = value.Item4;
            
            // Intermediates
            int rows = 10;
            int cols = 10;
            int nDots = binPos.Length;
            List<int> closeDots = new List<int>();
            float a = 1f/(sepRadius*sepRadius);
            Vector2[] Steer = new Vector2[nDots];            
            
            // Outputs
            Vector2[] Steering = new Vector2[nDots];

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

                //Console.WriteLine(closeDots.Count);

                foreach (var dotid in closeDots)
                {
                    
                    float dx = dotPos[i].X - dotPos[dotid].X;
                    float dy = dotPos[i].X - dotPos[dotid].Y;
                    //Console.WriteLine("dx" + dx + "dy" + dy);



                    if (Math.Abs(dx) > Right)
                    {
                        if (dx < 0) dx += 2*Right;
                        else dx -= 2*Right;
                    }

                    if (Math.Abs(dy) > Top)
                    {
                        if (dy < 0) dy+= 2*Top;
                        else dy -= 2*Top;
                    }
                    
                    if (dx*dx + dy*dy < sepRadius*sepRadius)
                    {
                    float d2 = a * dx *dx + a * dy * dy; // will be larger for further objects
                    //float d2 = dx *dx + dy * dy;

                    if (d2 > 0)
                    {
                    Vector2 diff = new Vector2(dx,dy);
                    diff.Normalize();
                    diff = Vector2.Multiply(-diff, 1f/d2);                
                    Steer[i] = Vector2.Subtract(Steer[i], diff);
                    }
                }
                    
                }
                if (Steer[i].LengthSquared > 0)

                {Steer[i].Normalize();
                Steer[i] = Vector2.Multiply(Steer[i], maxSpeed);
                }
                Steer[i] = Vector2.Multiply(Steer[i], sepWeight);
                }
            }
            
            var result = new Tuple<Vector2[],int[]>(Steer, sepCounter);
            return result;

        });
    }
}
