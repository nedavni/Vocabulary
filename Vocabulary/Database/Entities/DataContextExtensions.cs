using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Vocabulary.Database.Entities
{
    internal static class DataContextExtensions
    {
        public static T FindOrThrow<T>(this IQueryable<T> set, Expression<Func<T, bool>> predicate, string errorMessage) where T : class
        {
            var value = set.SingleOrDefault(predicate);
            if (value == null)
            {
                throw new InvalidOperationException(errorMessage);
            }

            return value;
        }

        public static T FindOrThrow<T>(this IEnumerable<T> set, Func<T, bool> predicate, string errorMessage) where T : class
        {
            var value = set.SingleOrDefault(predicate);
            if (value == null)
            {
                throw new InvalidOperationException(errorMessage);
            }

            return value;
        }
    }
}
