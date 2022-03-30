using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqExercises
{
    class Program
    {
        static void Main(string[] args)
        {

            SetOperators_Union_Example();
        }


        private static bool EvaluateFunction(Person p, Func<Person, bool> personFilter)
        {
            Console.WriteLine($"Evaluating filter for person {p.FullName}");
            return personFilter(p);
        }

        private static void PrintSeparator (string label)
        {
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine(label);
            Console.WriteLine("----------------------------------------------------");
        }

        private static void GettingStarted_MyFirstLinq()
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

        private static void FilterOperators_Were_Example()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");

            /*var query = from person in PersonsDatabase.AllPersons
                        where person.Age > 14 && person.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase)
                        //where EvaluateFunction(person, p => p.Age > 14 && p.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                        select person;
            */

            var query = PersonsDatabase.AllPersons
                .Where((person, idx) => (person.Age > 14) &&
                                        person.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase) &&
                                        idx % 2 == 1);


            foreach (Person p in query)
            {
                p.Print();
            }
        }

        private static void ArrayList_Example()
        {
            ArrayList list = new ArrayList();

            list.Add(1);
            list.Add("test");
            list.Add(new Person("John", "Dow", new DateTime(1990, 3, 30), Gender.Male));

            IEnumerable<int> numbers = list.OfType<int>();

            foreach (int nr in numbers)
            {
                Console.Write($"{nr}, ");
            }
        }


        private static void Inheritance()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");

            var query = PersonsDatabase.AllPersons.OfType<Student>();

            foreach (Person p in query)
            {
                p.Print();
            }
        }


        private static void ProjectionOperator_Select()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");
            /*
            var query = from person in PersonsDatabase.AllPersons
                        select person.FullName;
            */

            var query = PersonsDatabase.AllPersons.Select(person => person.FullName);

            foreach (string fullName in query)
            {
                Console.WriteLine(fullName);
            }
        }

        private static void ProjectionOperator_Select_WithIndex()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");
            /*
            var query = (from person in PersonsDatabase.AllPersons
                        where person.Age>14
                        select person.FullName)
                        .Select((fullName, idx)=> new { FullName= fullName, Index=idx});
           */

            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 14)
                .Select((person, idx) => new { FullName = person.FullName, Index = idx });




            foreach (var personAndIndex in query)
            {
                Console.WriteLine($"Person at index {personAndIndex.Index} is {personAndIndex.FullName}");
            }
        }

        private static void ProjectionOperator_SelectMany1()
        {
            int[] numbers = { 1, 2, 3, 4 };

            //Result: 1,1,1,2,4,8,3,9,27,....

            /*
            var query = from nr in numbers
                        from powers in new[] { nr, nr * nr, nr * nr * nr }
                        select powers;
            */

            var query = numbers.SelectMany(nr => new[] { nr, nr * nr, nr * nr * nr });


            foreach (int n in query)
            {
                Console.Write($"{n}, ");
            }

        }


        private static void ProjectionOperator_SelectMany2()
        {
            int[] collection1 = { 1, 2, 3, 4 };
            int[] collection2 = { 4, 5 };
            /*
            var query = from elem1 in collection1
                        from elem2 in collection2
                        where Math.Abs(elem1-elem2)==1
                        select new Tuple<int, int>(elem1, elem2);
            */

            var query = collection1
                .SelectMany(
                    elem1 => collection2,
                    (elem1, elem2) => new Tuple<int, int>(elem1, elem2))
                .Where(pair => Math.Abs(pair.Item1 - pair.Item2) == 1);

            foreach (var result in query)
            {
                Console.Write($"({result.Item1}, {result.Item2}), ");
            }
        }


        private static void SortingOperator_OrderBy()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");
            /*
            var query = from person in PersonsDatabase.AllPersons
                        where person.Age > 20 && person.Age < 40
                        orderby person.Age ascending, person.LastName descending
                        select person;
            */

            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 20 && person.Age < 40)
                .OrderBy(person => person.Age)
                .ThenByDescending(person => person.LastName);


            foreach (Person p in query)
            {
                p.Print();
            }
        }


        private static void GrupingOperator_GroupBy()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");
            /*
            var query = from person in PersonsDatabase.AllPersons
                        where person.Age > 30
                        group person by person.DateOfBirth.Year into groups
                        orderby groups.Key ascending
                        select groups;
             */


            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 30)
                .GroupBy(person => person.DateOfBirth.Year)
                .OrderBy(group => group.Key);


            foreach (IGrouping<int, Person> group in query)
            {
                Console.WriteLine($"Person born in {group.Key}: ");
                foreach (Person p in group)
                {
                    p.Print();
                }
            }
        }

        private static void PartitionOperator_Take_TakeWhile()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results");



            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 30)
                .Take(10);

            Console.WriteLine(PersonsDatabase.AllPersons.Where(person => person.Age > 30).Count());

            foreach (Person p in query)
            {
                p.Print();
            }

            PrintSeparator("Query Results");

            var query2 = PersonsDatabase.AllPersons
                .OrderBy(person => person.Age)
                .TakeWhile(person => person.Age < 30);

            foreach (Person p in query2)
            {
                p.Print();
            }
        }


        private static void PartitionOperator_Skip()
        {
            int index = 0;

            var sortedSet = PersonsDatabase.AllPersons
                .OrderBy(person => person.LastName)
                .ThenBy(person => person.FirstName);


            foreach (Person p in sortedSet)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query Results (1)");

            int pageSize = 10;

            int totalPages = PersonsDatabase.AllPersons.Count();

            int totalPagesCount = (totalPages / pageSize) + (totalPages % pageSize == 0 ? 0 : 1);

            //Math.Round()

            for (int pageNo = 1; pageNo <= totalPagesCount; pageNo++)
            {
                Console.WriteLine("----------------");
                Console.WriteLine($"Page nr {pageNo}");

                var query = sortedSet.Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);

                foreach (Person p in query)
                {
                    p.Print();
                }
            }
        }

        private static void SetOperators_Union_Example()
        {
            Person p1 = new Person("Person1", "LastName1", new DateTime(1985, 3, 15), Gender.Male);
            Person p2 = new Person("Person2", "LastName2", new DateTime(1994, 3, 20), Gender.Female);
            Person p3 = new Person("Person3", "LastName3", new DateTime(1985, 3, 15), Gender.Male);
            Person p4 = new Person("Person2", "LastName2", new DateTime(1994, 3, 20), Gender.Female);

            List<Person> personsList1 = new List<Person>
            {
                p1,
                p2
            };

            List<Person> personsList2 = new List<Person>
            {
                p3,
                p4
            };

            var query = personsList1.Union(personsList2);

            foreach (Person p in query)
            {
                p.Print();
            }

            int[] numbers1 = { 1, 2, 3 };
            int[] numbers2 = { 2, 3, 4 };
            var unionNumbers = numbers1.Union(numbers2);
            foreach (int nr in unionNumbers)
            {
                Console.Write($"{nr}, ");
            }
        }

        private static void SetOperators_Zip_Example()
        {
            int[] numbers = { 1, 2, 3 };
            string[] labels = { "label", "test", "hello", "another" };
            var query = numbers.Zip(labels, (elem1, elem2) => $"{elem2}{elem1}");

            foreach (var element in query)
            {
                Console.Write($"{element}, ");
            }
        }
    }
}
