using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

public class DotMotionParameters
{
    public float? Duration { get; set; }
    
    public float? Size { get; set; }

    public float? Colour1 { get; set; }
    public float? Colour2 { get; set; }

    public float? Contrast { get; set; }
        
    public float? VelocityX { get; set; }
    public float? VelocityY { get; set; }
    public float? VelocityZ { get; set; }
    public float? Coherence { get; set; }
    public float? numDots1 { get; set; }
    public float? numDots2 { get; set; }
    public int? dotLifeBool { get; set; }
    public int? dotLifetime { get; set; }

}

[Combinator]
[Description("Creates a sequence of dot motion parameters used for stimulus presentation.")]
[WorkflowElementCategory(ElementCategory.Source)]
public class DotMotionSpecification
{
    private List<DotMotionParameters> trials = new List<DotMotionParameters>();

    public List<DotMotionParameters> Trials
    {
        get { return trials;}
    }
    
    public IObservable<DotMotionParameters> Process()
    {
        return trials.ToObservable();
    }

    public IObservable<DotMotionParameters> Process<TSource>(IObservable<TSource> source)
    {
        return source.SelectMany(input => trials);
    }
}
