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
public class getBinPosandDotLists
{
    public float Left { get; set; } 
    public float Right { get; set; }
    public float Top { get; set; } 
    public float Bottom { get; set; }
    public IObservable<Tuple<Vector2[], List<int>[,]>> Process(IObservable<Tuple<Vector2[], Vector2[,]>> source)
    {
        return source.Select(value => 
        {
            Vector2[] dotPos = value.Item1;
            Vector2[,] FlowField = value.Item2;
            int nDots = dotPos.Length;
            int rows = FlowField.GetLength(0);
            int cols = FlowField.GetLength(1);
            float xSize = Math.Abs(Left-Right);
            float ySize = Math.Abs(Top-Bottom);
            float xbinsize = xSize/(float)cols;
            float ybinsize = ySize/(float)rows;            


            // initialise dot list array            
            List<int>[,] dotLists = new List<int>[rows,cols];  
            for (int irow=0; irow<rows; irow++)
            {
                for (int icol=0; icol<cols; icol++)
                {
                    dotLists[icol,irow] = new List<int>();
                }
            }

            Vector2[] binPos = new Vector2[nDots];

            for (int i = 0; i < nDots; i++)
            {
                binPos[i].X = (int)Math.Floor((dotPos[i].X+Right)/xbinsize);
                binPos[i].Y = (int)Math.Floor((dotPos[i].Y+Top)/ybinsize);
                dotLists[(int)binPos[i].X, (int)binPos[i].Y].Add(i);
            }

            var result = new Tuple<Vector2[],List<int>[,]>
            (binPos, dotLists);

            return result;
        }
        );
    }
}
