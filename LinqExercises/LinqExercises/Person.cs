using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExercises
{
	public class Person : IEquatable<Person>
	{
		public Person(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
		{
			this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			this.Gender = gender;
			this.DateOfBirth = dateOfBirth;
		}

		public string FirstName
		{
			get;
		}

		public string LastName
		{
			get;
		}

		public string FullName
			=> $"{FirstName} {LastName}";

		public Gender Gender
		{
			get;
		}

		public DateTime DateOfBirth
		{
			get;
		}

		public int Age
			=> DateTime.Today.Year - DateOfBirth.Year;

        public bool Equals(Person other)
        {
            if (other is null)
            {
				return false;
            }

			return string.Equals(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase) &&
				   string.Equals(LastName, other.LastName, StringComparison.OrdinalIgnoreCase) &&
				   Gender == other.Gender &&
				   DateOfBirth == other.DateOfBirth;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Person);
		}

		public void Print()
		{
			Print(-1);
		}

		public void Print(int index)
		{
			Console.WriteLine(
				index >= 0
				? $"{index}) {FullName} date of birth: {DateOfBirth}, age: {Age}"
				: $"{FullName} date of birth: {DateOfBirth}, age: {Age}");
		}

        public override int GetHashCode()
        {
			return HashCode.Combine(FirstName, LastName, Gender, DateOfBirth);
		}
    }
}
