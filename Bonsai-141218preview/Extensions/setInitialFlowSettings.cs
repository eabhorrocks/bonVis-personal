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
public class setInitialFlowSettings
{
    public float Left { get; set; } 
    public float Right { get; set; }
    public float Top { get; set; } 
    public float Bottom { get; set; }
    public IObservable<Tuple<Vector2[], Vector2[], Vector2[], Vector2[], int[], List<int>[,], Vector2[,]>> Process(IObservable<Tuple<Vector2[], Vector2[,]>> source)
    {
        return source.Select(value => 
        {
            // Inputs
            Vector2[] dotPos = value.Item1;
            Vector2[,] FlowField = value.Item2;

            // Params
            int nDots = dotPos.Length;
            int rows = FlowField.GetLength(0);
            int cols = FlowField.GetLength(1);            
            float xSize = Math.Abs(Left-Right);
            float ySize = Math.Abs(Top-Bottom);
            float xbinsize = xSize/(float)cols;
            float ybinsize = ySize/(float)rows;
            float[] borders = { Left, Right, Top, Bottom };

            // Intermediate
            Vector2[] flowVelocity = new Vector2[nDots]; // This is velocity at t=0.

            // Outputs
            Vector2[] Velocity = new Vector2[nDots];
            Vector2[] Acceleration = new Vector2[nDots];
            Vector2[] Desired = new Vector2[nDots];
            Vector2[] binPos = new Vector2[nDots];
            int[] sepCounter = new int[nDots];
            Random rng = new Random();

            // initialise dot list array            
            List<int>[,] dotLists = new List<int>[rows,cols];  
            for (int irow=0; irow<rows; irow++)
            {
                for (int icol=0; icol<cols; icol++)
                {
                    dotLists[icol,irow] = new List<int>();
                }
            }
            
            // Get binPos, flowVelocity, compute 'desired', create dotlists grid
            for (int i = 0; i < nDots; i++)
            {
                binPos[i].X = (int)Math.Floor((dotPos[i].X+(xSize/2f))/xbinsize);
                binPos[i].Y = (int)Math.Floor((dotPos[i].Y+(ySize/2f))/ybinsize);

                flowVelocity[i] = FlowField[(int)binPos[i].X,(int)binPos[i].Y];

                dotLists[(int)binPos[i].X, (int)binPos[i].Y].Add(i);
                sepCounter[i] = ((int)rng.Next(0, 10));

            }

            Velocity = flowVelocity;
            for (int i = 0; i < nDots; i++)
            {
                Desired[i] = Vector2.Subtract(flowVelocity[i],Velocity[i]);
            }

            Acceleration = Desired;
            
            // wrap around
            wrapAround(dotPos, nDots, borders);


            var result = new Tuple<Vector2[], Vector2[], Vector2[], Vector2[], int[], List<int>[,], Vector2[,]>
            (dotPos, Velocity, binPos, Desired, sepCounter, dotLists, FlowField);

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
