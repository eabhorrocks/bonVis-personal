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
public class limitVector2Array
{
    public float maxval {get; set; }
    public IObservable<Vector2[]> Process(IObservable<Vector2[]> source)
    {
        return source.Select(value => 
        {
        Vector2[] theArray = value;
        int nDots = theArray.Length;
        for (int i = 0; i < nDots; i++)
        {
            if(theArray[i].LengthSquared > maxval * maxval)
            {
                theArray[i].Normalize();
                theArray[i] = Vector2.Multiply(theArray[i], maxval);            
            }
        }
        
        return theArray;
        }
        );
    }
}
