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
public class SeparatingForce
{
    public IObservable<Vector2[]> Process(IObservable<Tuple<Vector2[], Vector2[,], Vector2[], Vector2[], List<Vector2>[,]>> source)
    {
        return source.Select(value => 
        {
            Vector2[] dotPos = value.Item1;
            var nDots = dotPos.Length;
            Vector2[,] FlowField = value.Item2;
            Vector2[] Velocities = value.Item3;
            Vector2[] binPos = value.Item4;
            List<Vector2>[,] jaggedArray = value.Item5;
            int rows = FlowField.GetLength(0);
            int cols = FlowField.GetLength(1);
            //List<Vector2> closeDots = new List<Vector2>();

            for (int i = 0; i < nDots; i++)
            {
                int xbin = (int)binPos[i].X;
                int ybin = (int)binPos[i].Y;
                //closeDots.AddRange(jaggedArray[xbin,ybin]);


                //closeDots.Clear();
                for (int ix = xbin-1; ix<=xbin+1; ix++)
                {
                    for (int iy = ybin-1; iy<=ybin+1; iy++)
                    {
                        if (iy < 0) { iy = rows-1; }
                        if (iy >= rows) { iy = 0; }
                        if (ix < 0) { ix = cols-1; }
                        if (ix >= cols) { ix = 0; }
                    }
                }
                

           //     for (int ixbin = xbin-1; ixbin <= xbin+1; ixbin++)
          //      {
          //          for (int iybin = ybin-1; iybin <= ybin+1; iybin++)
          ////          {
           //             if(ixbin<0) ixbin = ixbin+cols;
          //              if(ixbin>cols-1) ixbin = ixbin - cols;
          //              if(iybin<0) iybin = iybin+rows;
          //              if(iybin>rows-1) iybin = iybin - rows;

         //           }
         //       }            
                
            }


            

            return dotPos;
        }


        );
    }
}
