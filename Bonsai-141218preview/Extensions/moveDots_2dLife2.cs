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
public class moveDots_2dLife2
{
        public float VelocityX {get; set; }
        public float VelocityY { get; set; }
        
        public float Coherence { get ; set; }
        public int dotLifetimeBool {get; set; }
        public int dotLifetime { get; set; }
        public float left { get; set; }
        public float right { get; set;}
        public float top { get; set; }
        public float bottom { get; set; }
         



    public IObservable<Tuple<Vector2[], int[]>> Process(IObservable<Tuple<Vector2[], int[]>> source)
    {
        return source.Select(value => 
        {
            Random rng = new Random(25);
            var pos = value.Item1;
            var life = value.Item2;            
            float nDotsf = pos.Length;   
            int nDots = pos.Length;         
            float nCohf = nDotsf/100f * Coherence;
            int nCoh = (int)Math.Floor(nCohf);
            
            //Console.WriteLine("coh: " + Coherence);
            //Console.WriteLine("nDots: " + nDots);
            //Console.WriteLine("nCoh + " + nCoh);

            double vel = Math.Sqrt(VelocityX*VelocityX + VelocityY*VelocityY);
            float[] borders = { left, right, top, bottom };

        // Coherent dots
            for (int i = 0; i < nCoh; i++)
        {
            pos[i].X = (float)(pos[i].X + VelocityX);
            pos[i].Y = (float)(pos[i].Y + VelocityY);
        }
       

        // Incoherent Dots
        for (int i = nCoh; i < nDots; i++)
        {
            float rNum = rng.Next(0, 100);
            rNum = rNum/100;
            double rNum2 = Math.Sqrt(1 - rNum*rNum);
            int rdir = rng.Next(0, 2) * 2 - 1;
            int rdir2 = rng.Next(0, 2) * 2 - 1;
        
            pos[i].X = (float)(pos[i].X + rdir*rNum*vel);
            pos[i].Y = (float)(pos[i].Y + rdir2*rNum2*vel);
        }
       
           

            //wrapAround(pos, nDots, borders);
            if (dotLifetimeBool == 1)
            {
            dotDeath(pos, life, nDots, dotLifetime, left, right, top, bottom);
            }


        var result = new Tuple<Vector2[], int[]>(pos, life);
        return result;
        });

        
        
    }
        private void wrapAround(Vector2[] dotPos, int nDots, float[] borders)
    {
        for (int i=0; i < nDots; i++)
        {
        if(dotPos[i].X > borders[1]) {dotPos[i].X = borders[0]+0.05f;}
        else if(dotPos[i].X < borders[0]) {dotPos[i].X = borders[1]-0.05f;}
        if(dotPos[i].Y > borders[2]+1){dotPos[i].Y = borders[3]-0.05f;}
        else if(dotPos[i].Y < borders[3]) {dotPos[i].Y = borders[2]+0.05f;}
        }
    }

    private void dotDeath(Vector2[] pos, int[] life, int nDots, int dotLifetime, float left, float right, float top, float bottom)
    {
        Random rng = new Random((int)DateTime.Now.Ticks);
        for (int i=0; i < nDots; i++)
        {            
                life[i] = life[i] + 1;
                //Console.WriteLine(life[i]);

        if(life[i] >= dotLifetime)
        {
            pos[i].X = (float)(rng.NextDouble() * 2 - 1) * Math.Abs(right - left) /2f;
            pos[i].Y = (float)(rng.NextDouble() * 2 - 1) * Math.Abs(top - bottom) /2f;
            life[i] = 0;
        }
        }

    }
}
