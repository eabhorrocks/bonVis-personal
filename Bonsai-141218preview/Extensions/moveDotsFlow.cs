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
public class moveDotsFlow
{
    public IObservable<Tuple<Vector2[], Vector2[]>> Process(IObservable<Tuple<Vector2[], Vector2[]>> source)
    {
        return source.Select(value => value);
    }
}
