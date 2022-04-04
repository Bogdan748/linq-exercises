using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExercises
{
	public class Category
	{
		public Category(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public int Id
		{
			get;
		}

		public string Name
		{
			get;
		}
	}

}
