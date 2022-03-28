using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExercises
{
    public class NumbersGenerator
    {
        public static IEnumerable<long> Generate()
        {
            long start = 0;
            while (true)
            {
                //Console.WriteLine($"Infinite generator: {start}");
                yield return start;

                if(start < int.MaxValue)
                {
                    start++;
                }
                else
                {
                    start = 0;
                }                
            }
        }
    }
}
