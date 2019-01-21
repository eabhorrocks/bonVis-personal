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
public class addVec2Arrays
{
    public IObservable<Vector2[]> Process(IObservable<Tuple<Vector2[], Vector2[]>> source)
    {
        return source.Select(value => 
        {
            int n = value.Item2.Length;
            Vector2[] result = new Vector2[n];

            for (int i=0; i < n; i++)
            {
            result[i] = Vector2.Add(value.Item1[i], value.Item2[i]);
            }
            
            //Console.WriteLine("Vel" + value.Item2[10]);
            //Console.WriteLine("Pos" + value.Item1[10]);
            //Console.WriteLine("Output" + result[10]);

            return result;
        });
    }
}
