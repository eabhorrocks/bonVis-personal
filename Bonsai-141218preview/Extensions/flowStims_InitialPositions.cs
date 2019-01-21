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
public class flowStims_IntialPositions
{
    public float Scale { get; set; }
    public float posStd { get; set; }
    public float binSize { get; set; }

    public IObservable<Vector2[]> Process()
    {
        var nDots1d = (int)(2*Scale/binSize);
        var nDots = nDots1d*nDots1d;
        var poses = new Vector2[nDots];
        int c = 0;

        Random rand = new Random(); //reuse this if you are generating many

        for (int i = 1; i < nDots1d; i++) {
            for (int j = 1; j < nDots1d; j++) {

            double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0-rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
            Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            float randNormal = (float)(posStd * randStdNormal); //random normal(mean,stdDev^2)

                poses[c].X = (float)(i * binSize* Scale - nDots1d*binSize/2 + randNormal*binSize);
                poses[c].Y = (float)(j * binSize* Scale - nDots1d*binSize/2 + randNormal*binSize);
                c++;

            }
        }
    
        return Observable.Return(poses);
    }

}