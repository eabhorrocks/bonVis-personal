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
public class GenerateDotFieldLife
{
    public int numDots { get; set; }
    public float dotLifetime { get; set; }
    public float left { get; set; }
    public float right { get; set;}
    public float top { get; set; }
    public float bottom { get; set; }

    public IObservable<Vector2[]> Process()
    {
        var random = new Random(32);
        var result = new Vector2[numDots];
        var life = new double[numDots];
        for (int i = 0; i < result.Length; i++)
        {
            result[i].X = (float)(random.NextDouble() * 2 - 1) * Math.Abs(right - left) /2f;
            result[i].Y = (float)(random.NextDouble() * 2 - 1) * Math.Abs(top - bottom) /2f;
            life[i] = (double)(random.NextDouble() * dotLifetime);
        }

        var output = new Tuple<Vector2[], double[]>(result, life);
        return Observable.Return(result);
    }

   // public IObservable<Vector2[]> Process<TSource>(IObservable<TSource> source)
   // {
  //      var random = new Random(32);
  //      return source.Select(input =>
     //   {
  //          var result = new Vector2[numDots];
  //          for (int i = 0; i < result.Length; i++)
   //         {
   //             result[i].X = (float)(random.NextDouble() * 2 - 1) * Scale;
    //            result[i].Y = (float)(random.NextDouble() * 2 - 1) * Scale;
   //         }
        //    return result;
   //     });
 //   }
}
