using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExercises
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {

        private readonly Func<T, T, bool> _lambdaComparer;

        public LambdaEqualityComparer(Func<T, T, bool> lambdaComparer)
        {
            _lambdaComparer = lambdaComparer;
        }
        public bool Equals(T? x, T? y)
        {
            return _lambdaComparer?.Invoke(x,y) ?? false;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode(); 
        }
    }
}
