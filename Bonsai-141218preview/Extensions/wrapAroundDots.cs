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
public class wrapAroundDots
{

    public float Left { get; set; } 
    public float Right { get; set; }
    public float Top { get; set; } 
    public float Bottom { get; set; }
    public IObservable<Vector2[]> Process(IObservable<Vector2[]> source)
    {
        return source.Select(value => 
        {
            float[] borders = { Left, Right, Top, Bottom };
            Vector2[] dotPos = value;
            int nDots = dotPos.Length;

            for (int i=0; i < nDots; i++)
            {
            if(dotPos[i].X >= borders[1]) {dotPos[i].X = borders[0]+0.01f;}
            else if(dotPos[i].X <= borders[0]) {dotPos[i].X = borders[1]-0.01f;}
            if(dotPos[i].Y >= borders[2]){dotPos[i].Y = borders[3]+0.01f;}
            else if(dotPos[i].Y <= borders[3]) {dotPos[i].Y = borders[2]-0.01f;}
            }

        return dotPos;

        });
    }
}
