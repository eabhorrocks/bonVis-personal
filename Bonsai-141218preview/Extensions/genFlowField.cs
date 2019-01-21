using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using OpenTK;


[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Source)]
public class genFlowField
{
    public int nBins = 4;
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public int maxVar { get; set; }
    public IObservable<Vector2[]> Process()
    {
        var result = new Vector2[nBins];
        Random rng = new Random(2);

        for (int i = 0; i < nBins; i++)
        {

        int rdir = rng.Next(0, 2) * 2 - 1;
        float rNum = rng.Next(0, maxVar);
        rNum = rNum/100;
 
        float vel = (float)Math.Sqrt(VelocityX*VelocityX + VelocityY*VelocityY);

        Vector2 angleVector = new Vector2(VelocityX, VelocityY);
        float angle =
        (float)Math.Atan2(angleVector.X, -angleVector.Y);

        angle = angle + rNum*rdir;
        Vector2 angleVector2 = new Vector2(
        (float)Math.Cos(angle),
        (float)Math.Sin(angle));

        result[i] = Vector2.Multiply(angleVector2, vel);
        

        }
        return Observable.Return(result);
    }
}
