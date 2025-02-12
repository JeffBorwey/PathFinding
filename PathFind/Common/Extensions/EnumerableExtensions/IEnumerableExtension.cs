﻿using Common.Attrbiutes;
using Common.Extensions.EnumerableExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common.Extensions.EnumerableExtensions
{
    class MatchComparer<T> : IEqualityComparer<T>
    {
        public MatchComparer(Func<T, T, bool> predicate)
        {
            this.predicate = predicate;
        }

        public bool Equals(T x, T y)
        {
            return predicate(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        private readonly Func<T, T, bool> predicate;
    }

    public static class IEnumerableExtension
    {
        /// <summary>
        /// Distincts <paramref name="collection"/> by <paramref name="selector"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="collection"></param>
        /// <param name="selector"></param>
        /// <returns>A distincted <paramref name="collection"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> 
        /// is null or <paramref name="selector"/> is null</exception>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> selector)
        {
            #region InvariantsObservance
            if (collection == null || selector == null)
            {
                throw new ArgumentNullException("Bad incoming arguments");
            }
            #endregion

            T FirstOfTheGroup(IGrouping<TKey, T> item) => item.First();

            return collection
                .GroupBy(selector)
                .Select(FirstOfTheGroup);
        }

        /// <summary>
        /// Applies an accumulation function over the collection 
        /// or returns default value if collection is empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns>A result of aggregation of the collection</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AggregateOrDefault<T>(this IEnumerable<T> collection, Func<T, T, T> func)
        {
            return collection.Any() ? collection.Aggregate(func) : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Without<T>(this IEnumerable<T> self, IEnumerable<T> items)
        {
            return self.Where(item => !items.Contains(item));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Without<T>(this IEnumerable<T> self, params T[] items)
        {
            return self.Without(items.AsEnumerable());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMultiplication(this IEnumerable<int> array)
        {
            return array.AggregateOrDefault((x, y) => x * y);
        }

        /// <summary>
        /// Applies delegate <paramref name="action"/> 
        /// to each element of <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns>The same <paramref name="collection"/> with elements 
        /// to which <paramref name="action"/> was applied</returns>
        public static IEnumerable<T> ForAll<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.ForEach(action);
            return collection;
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSingle<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Count(predicate) == 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SumOrDefault(this IEnumerable<double> collection)
        {
            return collection.Any() ? collection.Sum() : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SumOrDefault(this IEnumerable<int> collection)
        {
            return collection.Any() ? collection.Sum() : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MaxOrDefault(this IEnumerable<double> collection)
        {
            return collection.Any() ? collection.Max() : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MinOrDefault<T>(this IEnumerable<T> collection, Func<T, double> selector)
        {
            return collection.Any() ? collection.Min(selector) : default;
        }

        /// <summary>
        /// Applies <paramref name="predicate"/> to corresponding 
        /// elements of sequencies and returns true if <paramref name="predicate"/> 
        /// is true for all pairs of elements and false if not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="second"></param>
        /// <param name="predicate"></param>
        /// <returns>true if <paramref name="predicate"/> is true 
        /// for each corresponding elements in two sequencies and false if not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Juxtapose<T>(this IEnumerable<T> self, IEnumerable<T> second, Func<T, T, bool> predicate)
        {
            return self.SequenceEqual(second, new MatchComparer<T>(predicate));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Juxtapose<T>(this IEnumerable<T> self, IEnumerable<T> second)
        {
            return self.Juxtapose(second, (a, b) => a.Equals(b));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>
            (this IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            return collection.ToDictionary(item => item.Key, item => item.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<string, T> ToNameInstanceDictionary<T>(this IEnumerable<T> collection)
        {
            return collection.ToDictionary(item => item.GetDescriptionAttributeValueOrEmpty());
        }

        public static Tuple<string, T>[] ToNameInstanceTuples<T>(this IEnumerable<T> collection)
        {
            return collection
                .Select(item => new Tuple<string, T>(item.GetDescriptionAttributeValueOrEmpty(), item))
                .ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T1> GetItems1<T1, T2>(this IEnumerable<Tuple<T1, T2>> tuples)
        {
            return tuples.Select(item => item.Item1);
        }

        public static IOrderedEnumerable<T> OrderByOrderAttribute<T>(this IEnumerable<T> collection)
        {
            return collection.OrderBy(item => item.GetOrderAttributeValueOrMaxValue());
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }

        public static IEnumerable<T> GroupByGroupAttribute<T>(this IEnumerable<T> collection)
        {
            return collection
                .GroupBy(item => item.GetAttributeOrNull<GroupAttribute>())
                .Select(item => item.OrderByOrderAttribute())
                .SelectMany(item => item);
        }

        public static IList<T> Shuffle<T>(this IList<T> list, Func<int> randomFunction)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int index = randomFunction() % list.Count;
                var temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
            return list;
        }

        public static IEnumerable<T> TakeOrDefault<T>(this IEnumerable<T> collection, int number)
        {
            int count = 0;
            using (var iterator = collection.GetEnumerator())
            {
                while (iterator.MoveNext())
                {
                    count++;
                    yield return iterator.Current;
                }
            }
            if (count != number)
            {
                int remained = number - count;
                while (remained-- > 0)
                {
                    yield return default;
                }
            }
        }

        /// <summary>
        /// Creates a new queue from specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns>An instance of queue, that contains 
        /// elements from the specified collection</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
        {
            return new Queue<T>(collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToHashCode(this IEnumerable<int> array)
        {
            return array.AggregateOrDefault(IntExtensions.Xor);
        }

        public static bool ContainsUniqueValues<T>(this IEnumerable<T> collection)
        {
            return collection.ContainsUniqueValues(EqualityComparer<T>.Default);
        }

        public static bool ContainsUniqueValues<T>(this IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            return collection.Distinct(comparer).Count() == collection.Count();
        }
    }
}
