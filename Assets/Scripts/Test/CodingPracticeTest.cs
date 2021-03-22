using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Superior
{
    public struct Table<T>
    {
        private T[] arr;

        public T this[int i]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }
    }
}