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
public class jaggedArrayTest
{
    public IObservable<List<Vector2>[,]> Process()
    {
        int rows = 10;
        int cols = 10;

        List<Vector2>[,] jaggedArray = new List<Vector2>[rows,cols];

        for(int irow = 0; irow < rows; irow++)
        {
            for(int icol = 0; icol < cols; icol++)
            {
                jaggedArray[irow,icol] = new List<Vector2>();
            }
        }


        return Observable.Return(jaggedArray);
    }
}
