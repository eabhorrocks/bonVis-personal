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
public class movingDotsFLow
{
    
    public IObservable<Vector2[]> Process(IObservable<Tuple<Vector2[], Vector2[]>> source)
    {
        return source.Select(value => 
        { 
            var flowField = value.Item2;
            var dotPos = value.Item1;
            var nDots = dotPos.Length;

            for (int i = 0; i < nDots; i++)
            {
                if (dotPos[i].X < 0)
                {
                    if (dotPos[i].Y < 0)
                    {
                        dotPos[i].X = dotPos[i].X + flowField[0].X;
                        dotPos[i].Y = dotPos[i].Y + flowField[0].Y;
                    }
                    else if (dotPos[i].Y >= 0)
                    {
                        dotPos[i].X = dotPos[i].X + flowField[1].X;
                        dotPos[i].Y = dotPos[i].Y + flowField[1].Y;
                    }
                }
                if (dotPos[i].X >= 0)
                {
                    if (dotPos[i].Y < 0)
                    {
                        dotPos[i].X = dotPos[i].X + flowField[2].X;
                        dotPos[i].Y = dotPos[i].Y + flowField[2].Y;
                    }
                    else if (dotPos[i].Y >= 0)
                    {
                    
                        dotPos[i].X = dotPos[i].X + flowField[3].X;
                        dotPos[i].Y = dotPos[i].Y + flowField[3].Y;
                    }
                }

                if(dotPos[i].X > 1)
                {
                dotPos[i].X = (float)(dotPos[i].X-2); // wrap around viewport
                }
                else if(dotPos[i].X < -1)
                {
                    dotPos[i].X = (float)(dotPos[i].X+2);
                }
                if(dotPos[i].Y > 1)
                {
                    dotPos[i].Y = (float)(dotPos[i].Y-2);
                }
                else if(dotPos[i].Y < -1)
                {
                    dotPos[i].Y = (float)(dotPos[i].Y+2);
                }
                
            }
            return dotPos;
            
        }
        );
    }
}
