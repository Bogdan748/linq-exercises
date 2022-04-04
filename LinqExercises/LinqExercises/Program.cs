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
            //V1
            /*
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories
                        on product.CategoryId equals category.Id into categoriesGroup 
                        select new
                        {
                            Product = product,
                            Category = categoriesGroup.DefaultIfEmpty(new Category(-1, "N/A")).Single()
                        };
            */

            //V2
            /*
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories
                        on product.CategoryId equals category.Id into categoriesGroup
                        from catOrDefault in categoriesGroup.DefaultIfEmpty(new Category(-1, "N/A"))
                        select new
                        {
                            Product = product,
                            Category = catOrDefault
                        };
            */
            var query = ProductsDatabase.AllProducts.GroupJoin(
                ProductsDatabase.AllCategories,
                product => product.CategoryId,
                category => category.Id,
                (product, categories) => new
                {
                    Product = product,
                    Categories = categories
                }).SelectMany(
                    groupJoin => groupJoin.Categories.DefaultIfEmpty(new Category(-1, "N/A")),
                    (groupJoin, category) => new
                    {
                        Product = groupJoin.Product,
                        Category = category
                    }
                );

            foreach (var pair in query)
            {
                Console.WriteLine($"{pair.Product.Id}) {pair.Product.Name}, category: {pair.Category.Name}");
            }
        }


        private static bool EvaluateFunction(Person p, Func<Person, bool> personFilter)
        {
            Console.WriteLine($"Evaluating filter for person {p.FullName}");
            return personFilter(p);
        }

        private static void PrintSeparator(string label)
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

        private static void AggregateFunctions_Count_Example()
        {
            int[] array = new[] { 1, 2, 3, 4 };
            Console.WriteLine(array.Length);

            List<string> genericList = new List<string>();
            genericList.AddRange(new[] { "test1", "test2" });
            Console.WriteLine(genericList.Count);

            int nrOfPersons = PersonsDatabase.AllPersons.Count();

            int filterPersons = PersonsDatabase.AllPersons.Count(
                person => person.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(nrOfPersons);
            Console.WriteLine(filterPersons);
        }

        private static void Aggregate_Min()
        {

            int[] array = { 5, 4, 3, 2, 1 };

            int min = array.Min();

            Console.WriteLine(min);

            int minAge = PersonsDatabase.AllPersons.Min(person => person.Age);

            var query = PersonsDatabase.AllPersons.Where(person => person.Age == minAge);

            Console.WriteLine($"Min age={minAge}");
            foreach (var person in query)
            {
                person.Print();
            }
        }

        private static void Aggregate_Average()
        {
            int[] array = { 5, 4, 3, 2, 1 };

            double avg = array.Average();

            Console.WriteLine(avg);

            double avgAge = PersonsDatabase.AllPersons.Average(person => person.Age);

            var query = PersonsDatabase.AllPersons.Where(person => person.Age == avgAge);

            Console.WriteLine($"Min age={avgAge}");
            foreach (var person in query)
            {
                person.Print();
            }
        }

        private static void ElementReturn_First_FirstOrDefaulr()
        {
            //Throw error:
            //int[] array = { 5,  3, 1 };
            //int first = array.First(nr=> nr%2==0);


            int[] array = { 5, 3, 1 };
            int first = array.FirstOrDefault(nr => nr % 2 == 0);

            Console.WriteLine(first);


            Person p = PersonsDatabase.AllPersons.FirstOrDefault(person => person.LastName.StartsWith("Q", StringComparison.OrdinalIgnoreCase));
            if (p is not null)
            {
                p.Print();
            }
            else
            {
                Console.WriteLine("No match!");
            }
        }


        private static void ElementReturn_Single_SingleOrDefault()
        {
            int[] array = { 5, 4, 3 };
            int single = array.Single(nr => nr % 2 == 0);

            //in cazul in care are mai multe tot da eroare
            //default daca nu se potrivenste nici un element sau nu am elemente
            int singleOrD = array.SingleOrDefault(nr => nr % 6 == 0);

            Console.WriteLine(single);
            Console.WriteLine(singleOrD);
        }

        private static void EmenentReturn_ElementAt_ElementAtOrDefault()
        {
            int[] array = { 5, 4, 3 };
            int value = array.ElementAt(2);


            int valueDe = array.ElementAtOrDefault(20);

            Console.WriteLine(value);
            Console.WriteLine(valueDe);
        }

        private static void QuantifierFunctions_Any()
        {
            int[] array = { 1, 5 };
            bool atLeastOne = array.Any(nr => nr % 2 == 0);

            Console.WriteLine(atLeastOne);

            bool AnyPerson = PersonsDatabase.AllPersons.Any(person => person.Age == 45);
            if (AnyPerson)
            {
                Person p = PersonsDatabase.AllPersons.First(person => person.Age == 45);
                p.Print();
            }
            else
            {
                Console.WriteLine("No person of 45");
            }
        }

        private static void QuantifierFunctions_All()
        {
            bool allPersons = PersonsDatabase.AllPersons.All(person => person.Gender == Gender.Female);
            if (allPersons)
            {
                Console.WriteLine("All females");
            }
            else
            {
                Console.WriteLine("Not all females");
            }
        }

        private static void QuantifierFuncions_Contains()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            bool containsNr = array.Contains(13);
            Console.WriteLine(containsNr);

            Person p = PersonsDatabase.AllPersons.First();
            bool containtPerson = PersonsDatabase.AllPersons.Contains(p);
            Console.WriteLine($"Contains person {containtPerson}");

            //trebuie implementat IEquatable, altfel compara referinte
            Person clone = p.Clone();
            bool containsClone = PersonsDatabase.AllPersons.Contains(clone, new PersonsComparer());
            Console.WriteLine($"Contains Clone {containsClone}");
        }


        private static void _cointainsWithCompare()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            bool containsNr = array.Contains(13);
            Console.WriteLine(containsNr);

            Person p = PersonsDatabase.AllPersons.First();
            bool containtPerson = PersonsDatabase.AllPersons.Contains(p);
            Console.WriteLine($"Contains person {containtPerson}");

            //trebuie implementat IEquatable, altfel compara referinte
            Person clone = p.Clone();
            bool containsClone = PersonsDatabase.AllPersons.Contains(
                clone,
                new LambdaEqualityComparer<Person>(
                    (x, y) => (x is not null) && (y is not null) &&
                    string.Equals(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(x.LastName, y.LastName, StringComparison.OrdinalIgnoreCase) &&
                   x.Gender == y.Gender &&
                   x.DateOfBirth == y.DateOfBirth
                    ));
            Console.WriteLine($"Contains Clone {containsClone}");
        }

        private static void Joins_Join()
        {
            //(produc,category)
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories
                        on product.CategoryId equals category.Id
                        select new
                        {
                            Product = product,
                            Category = category
                        };

            foreach (var pair in query)
            {
                Console.WriteLine($"{pair.Product.Id}) {pair.Product.Name}, category: {pair.Category.Name}");
            }
        }

        private static void Joins_GroupJoin()
        {
            /*
            //(produc,category)
            var query = from category in ProductsDatabase.AllCategories
                        join product in ProductsDatabase.AllProducts
                        on category.Id equals product.CategoryId into categoryGrup
                        select new
                        {
                            Id = category.Id,
                            CategoryName = category.Name,
                            Products = categoryGrup
                        };
            */

            var query = ProductsDatabase.AllCategories.GroupJoin(
                ProductsDatabase.AllProducts,
                category => category.Id,
                product => product.CategoryId,
                (category, categoryGrup) => new
                {
                    Id = category.Id,
                    CategoryName = category.Name,
                    Products = categoryGrup
                });


            foreach (var group in query)
            {
                Console.WriteLine($"{group.Id}) {group.CategoryName}");
                foreach (var product in group.Products)
                {
                    Console.WriteLine($"    - {product.Id}) {product.Name}");
                }
            }
        }

        private static void Joins_LeftOuterJoin()
        {
            //V1
            /*
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories
                        on product.CategoryId equals category.Id into categoriesGroup 
                        select new
                        {
                            Product = product,
                            Category = categoriesGroup.DefaultIfEmpty(new Category(-1, "N/A")).Single()
                        };
            */

            //V2
            /*
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories
                        on product.CategoryId equals category.Id into categoriesGroup
                        from catOrDefault in categoriesGroup.DefaultIfEmpty(new Category(-1, "N/A"))
                        select new
                        {
                            Product = product,
                            Category = catOrDefault
                        };
            */
            var query = ProductsDatabase.AllProducts.GroupJoin(
                ProductsDatabase.AllCategories,
                product => product.CategoryId,
                category => category.Id,
                (product, categories) => new
                {
                    Product = product,
                    Categories = categories
                }).SelectMany(
                    groupJoin => groupJoin.Categories.DefaultIfEmpty(new Category(-1, "N/A")),
                    (groupJoin, category) => new
                    {
                        Product = groupJoin.Product,
                        Category = category
                    }
                );

            foreach (var pair in query)
            {
                Console.WriteLine($"{pair.Product.Id}) {pair.Product.Name}, category: {pair.Category.Name}");
            }
        }
    }
}
