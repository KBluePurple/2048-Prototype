using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AchromaticDev.Extensions
{
    public static class LinkedListExtension
    {
        public static int IndexOf<T>(this LinkedList<T> list, T item)
        {
            return list.TakeWhile(x => !x.Equals(item)).Count();
        }
    }
}
