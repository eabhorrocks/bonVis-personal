using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class tuple_in_test
{
    public IObservable<int> Process(IObservable<Tuple<int, int>> source)
    {

        return source.Select(value => value.Item1);
    }
}
