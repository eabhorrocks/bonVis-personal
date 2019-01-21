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
public class moveDot_test
{

        public float Velocity { get; set; }
        public float Direction { get ; set; }
        public int Coherence { get ; set; }


    public IObservable<Vector2[]> Process(IObservable<Vector2[]> source)
    {
        return source.Select(value => 
        {
            var nDots = value.Length;
            int nCoh = nDots/100 * Coherence;
            var result = value;
            Random rng = new Random(25);

            for (int i = 0; i < nCoh; i++)
        {
            result[i].X = (float)(result[i].X + Velocity * Direction);
            if(result[i].X > 1)
            {
                result[i].X = (float)(result[i].X-2); // wrap around viewport
            }
            else if(result[i].X < -1)
            {
                result[i].X = (float)(result[i].X+2);
            }
        }

        for (int i = nCoh; i < nDots; i++)
        {
            float rNum = rng.Next(-100, 100);
            rNum = rNum/100;
            double rNum2 = Math.Sqrt(1 - rNum*rNum);
            float rdir = rng.Next(0, 2) * 2 - 1;
        
            result[i].X = (float)(result[i].X + rNum*Velocity);
            result[i].Y = (float)(result[i].Y + rdir*rNum2*Velocity);

            if(result[i].X > 1)
            {
                result[i].X = (float)(result[i].X-2);
            }
            else if(result[i].X < -1)
            {
                result[i].X = (float)(result[i].X+2);
            }

            if(result[i].Y > 1)
            {
                result[i].Y = (float)(result[i].Y-2);
            }
            else if(result[i].Y < -1)
            {
                result[i].Y = (float)(result[i].Y+2);
            }
        }

            return result;
        });
        
        
    }
}
