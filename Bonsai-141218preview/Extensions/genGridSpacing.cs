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
public class genGridSpacing
{
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }
    public float posStd { get; set; }
    public float dotSize { get; set; }
    public float dotSpacing { get; set; }

    public IObservable<Tuple<Vector2[], int>> Process()
    {
        float Scalex = Math.Abs(Left - Right);
        float Scaley = Math.Abs(Top - Bottom);
        float binSize = dotSize * dotSpacing;
        var nDots1dx = (int)(Scalex/binSize);
        var nDots1dy = (int)(Scaley/binSize);
        int nDots = nDots1dx*nDots1dy;
        var dotPos = new Vector2[nDots];
        Console.WriteLine("nDots:" + nDots);

        int c = 0;
        

        Random rand = new Random(); //reuse this if you are generating many

        for (int i = 0; i < nDots1dx; i++) {
            for (int j = 0; j < nDots1dy; j++) {

            double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0-rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
            Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)\
            float randNormal = (float)(posStd * randStdNormal); //random normal(mean,stdDev^2)

                dotPos[c].X = (float)((0.5f * binSize) + Left + i * binSize + randNormal*binSize);
                dotPos[c].Y = (float)((0.5f *binSize) + Bottom + j * binSize + randNormal*binSize);
                c++;
            }
        }

        var result = new Tuple<Vector2[], int>(dotPos,nDots);
    
        return Observable.Return(result);
    }

}