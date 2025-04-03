
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Extensions
{
    public static class TransformExtensions
    {
        
        public static T GetRandom<T>(this List<T> list)
        {
            var listLength = list.Count;

            var random = Random.Range(0, listLength);

            return list[random];
        }
        
    }
}