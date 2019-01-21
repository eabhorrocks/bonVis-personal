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
public class FlowField
{
    public int rows { get; set; }
    public int cols { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public float dirStd { get; set; }
    public IObservable<Tuple<Vector2[,], Vector2>> Process()
    {
        var FlowField = new Vector2[rows,cols];
        float vel = (float)Math.Sqrt(VelocityX*VelocityX + VelocityY*VelocityY);
        Random rng = new Random();
        Vector2 angleVector = new Vector2(VelocityX, VelocityY);
        float angle =
        (float)Math.Atan2(angleVector.X, angleVector.Y);


        for (int irow = 0; irow < rows; irow++)
        {
            for (int icol = 0; icol < cols; icol++)
            {
            //int rdir = rng.Next(0, 2) * 2 - 1;
            
            // gaussian random number 
            double u1 = 1.0-rng.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0-rng.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
            Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            float rNum = (float)(dirStd * randStdNormal); //random normal(mean,stdDev^2)

            //float rNum = rng.Next(0, dirStd);
            //rNum = rNum/100;

            float angle2 = angle + rNum;//*rdir;
            Vector2 angleVector2 = new Vector2(
            (float)Math.Sin(angle2),
            (float)Math.Cos(angle2));

            FlowField[irow,icol] = Vector2.Multiply(angleVector2, vel);
            }
        }            
        Vector2 v0 = new Vector2(VelocityX,VelocityY);

        var result = new Tuple<Vector2[,], Vector2>(FlowField,v0);

        return Observable.Return(result);
    }
}
