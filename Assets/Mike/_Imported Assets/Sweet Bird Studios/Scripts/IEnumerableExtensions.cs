using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class IEnumerableExtensions
{
    /// <summary>
    /// Calculate the distance between an enumerable collection of transforms.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transforms"></param>
    /// <returns></returns>
    public static Vector3 Midpoint<T>(this IEnumerable<T> transforms) where T : Transform
    {
        Vector3 sum = Vector3.zero;
        foreach (T transform in transforms)
        {
            sum += transform.position;
        }
        return sum / transforms.Count<T>();
    }

    /// <summary>
    /// Compares each transform against each other transform in an Enumerable list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="collection"></param>
    /// <param name="reducer"></param>
    /// <returns></returns>
    public static List<U> CompareAll<T,U>(this IEnumerable<T> collection, Func<T, T, U> reducer) where T : Transform{
        List<U> list = new List<U>();
        for (int i = 0; i < collection.Count() - 1; i++)
        {
            for(int j = i + 1; j < collection.Count(); j++)
            {
                list.Add(reducer(collection.ElementAt(i),collection.ElementAt(j)));
            }
        }
        return list;
    }

    /// <summary>
    /// Calculates the maximum distance between the 2 farthest apart objects of a collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transforms"></param>
    /// <returns></returns>
    public static float MaximumDistance<T>(this IEnumerable<T> transforms) where T : Transform
    {
        return transforms.CompareAll((transformA, transformB) => (transformA.position - transformB.position).sqrMagnitude).Max();
    }
}