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
public class calcDesired
{

    public float Left { get; set; } 
    public float Right { get; set; }
    public float Top { get; set; } 
    public float Bottom { get; set; }

    public IObservable<Vector2[]> Process(IObservable<Tuple<Vector2[], Vector2[], Vector2[,]>> source)
    {
        return source.Select(value => 
        {
            // Inputs
            Vector2[] binPos = value.Item1;
            Vector2[] Velocity = value.Item2;
            Vector2[,] FlowField = value.Item3;

            int nDots = binPos.Length;
            float rows = FlowField.GetLength(0);
            float cols = FlowField.GetLength(1);            
            float xbinsize = Math.Abs(Left - Right)/cols;
            float ybinsize = Math.Abs(Top - Bottom)/rows;
            Vector2[] flowVelocity = new Vector2[nDots];


            // calc bin pos, get flow velocity, desired = flow - current
            // output is an acceleratory force
 
            // Outputs
            Vector2[] Desired = new Vector2[nDots];
            
            for (int i = 0; i < nDots; i++)
            {                
                //Console.WriteLine("flowvel error");
                flowVelocity[i] = FlowField[(int)binPos[i].X,(int)binPos[i].Y];
                //Console.WriteLine("sub error");
                Desired[i] = Vector2.Subtract(flowVelocity[i], Velocity[i]);
                //dotLists[(int)binPos[i].X, (int)binPos[i].Y].Add(i);
            }

            return Desired;
        }
        );
    }
}
