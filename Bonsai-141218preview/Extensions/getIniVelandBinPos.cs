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
public class getIniVelandBinPos
{
    public IObservable<Tuple<Vector2[], Vector2[,], Vector2[], Vector2[], List<int>>> Process(IObservable<Tuple<Vector2[], Vector2[,], Vector2, float, float>> source)
    {
        return source.Select(value => 
        {
            Vector2[] dotPos = value.Item1; // x,y positions            
            int nDots = dotPos.GetLength(0);
            Vector2[,] FlowField = value.Item2;
            Vector2 v0 = value.Item3;


            Vector2[] Velocity = new Vector2[nDots]; // tracks velocity (vx,vy)
            Vector2[] Acceleration = new Vector2[nDots]; // acceleration vector, set to 0 each frame
            Vector2[] flowVelocity = new Vector2[nDots]; // 'desired': velocity vector for flow field
            Vector2[] Separating = new Vector2[nDots]; // separating force to add
            List<int> sepCounter = new List<int>(); // only perform separation every sepFreq frames            
            
            Vector2[] binPos = new Vector2[nDots]; // binned positions, determined bins of flow field atm.

            // binsize for flow and borders
            int rows = FlowField.GetLength(0);
            int cols = FlowField.GetLength(1);
            float xsize = value.Item4;
            float ysize = value.Item5;
            float xbinsize = xsize/(float)cols;
            float ybinsize = ysize/(float)rows;
            float[] borders = new float[4];
            borders[0] = -(xsize/2f); borders[1] = xsize/2f;
            borders[2] = -(ysize/2f); borders[3] = ysize/2f;

            Random rng = new Random();

            for (int i = 0; i < nDots; i++)
            {
            sepCounter.Add((int)rng.Next(0, 10));
            }

            List<int>[,] jaggedArray = new List<int>[rows,cols];
            for(int irow = 0; irow < rows; irow++)
            {
                for(int icol = 0; icol < cols; icol++)
                {
                jaggedArray[irow,icol] = new List<int>();
                }
            }

            //wrapAround(dotPos, nDots, borders);
            
           // for (int i=0; i < nDots; i++)
          //  {
          //      binPos[i].X = (int)Math.Floor((dotPos[i].X+1)/xbinsize);
          //      binPos[i].Y = (int)Math.Floor((dotPos[i].Y+1)/ybinsize);
          //      jaggedArray[(int)binPos[i].X,(int)binPos[i].Y].Add(i);                 
                
          //      Velocity[i] = v0;
          //      Acceleration[i] = new Vector2(0,0);
          //      flowVelocity[i] = FlowField[(int)binPos[i].X,(int)binPos[i].Y];

          //      sepCounter.Add((int)rng.Next(0, 10));

 
         //   }


            //var result = new Tuple<Vector2[],Vector2[,],Vector2[],Vector2[],List<int>[,]>
            //(dotPos, FlowField, Velocities, binPos, jaggedArray);
            var result = new Tuple<Vector2[], Vector2[,], Vector2[], Vector2[], List<int>>(dotPos, FlowField, Velocity, Acceleration, sepCounter);

            return result;
        });
    }

    private void wrapAround(Vector2[] dotPos, int nDots, float[] borders)
    {
        for (int i=0; i < nDots; i++)
        {
        if(dotPos[i].X >= borders[1]) {dotPos[i].X = borders[0]+0.01f;}
        else if(dotPos[i].X <= borders[0]) {dotPos[i].X = borders[1]-0.01f;}
        if(dotPos[i].Y >= borders[2]){dotPos[i].Y = borders[3]-0.01f;}
        else if(dotPos[i].Y <= borders[3]) {dotPos[i].Y = borders[2]+0.01f;}
        }
    }
}
