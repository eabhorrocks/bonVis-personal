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
public class genDots
{
    public float Scale { get; set; }

    public IObservable<Vector2[]> Process()
    {
        var random = new Random();
        var result = new Vector2[1000];
        for (int i = 0; i < result.Length; i++)
        {
            result[i].X = (float)(random.NextDouble() * 2 - 1) * Scale;
            result[i].Y = (float)(random.NextDouble() * 2 - 1) * Scale;
        }
        return Observable.Return(result);
    }

    public IObservable<Vector2[]> Process<TSource>(IObservable<TSource> source)
    {
        var random = new Random();
        return source.Select(input =>
        {
            var result = new Vector2[1000];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].X = (float)(random.NextDouble() * 2 - 1) * Scale;
                result[i].Y = (float)(random.NextDouble() * 2 - 1) * Scale;
            }
            return result;
        });
    }
}
