using System;
using System.Linq;

namespace LinqExercises
{
    class Program
    {
        static void Main(string[] args)
        {

            int n = 20;

            /*
            var query = from nr in NumbersGenerator.Generate()
                        where nr % 2 == 0
                        select nr;
            */


            var query = NumbersGenerator.Generate()
                .Where(nr => nr % 2 == 0);


            int i = 0;
            foreach (int nr in query)
            {
                i++;

                Console.WriteLine(nr);

                if (i >= n)
                {
                    break;
                }
            }

        }
    }
}
