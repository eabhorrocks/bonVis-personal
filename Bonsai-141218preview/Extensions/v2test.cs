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
public class v2test
{
    public int n { get; set; }
    public IObservable<Vector2[]> Process()
    {
        Vector2[] test = new Vector2[n];
        for (int i = 0; i < n; i++ )
        {
            test[i].X = test[i].X + 10;
        }


        return Observable.Return(test);
    }
}
