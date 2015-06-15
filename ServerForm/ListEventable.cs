using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerForm
{
    class ListEventable<T> : List<T>
    {

        public event EventHandler OnBeforeAdd;
        public event EventHandler OnAdd;

        public void Add(T item)
        {
            if (OnBeforeAdd != null)
                OnBeforeAdd(this, null);
            base.Add(item);
            if (OnAdd != null)
                OnAdd(this, null);
        }

        public void AddRange(T[] items)
        {
            if (OnBeforeAdd != null)
                OnBeforeAdd(this, null);
            base.AddRange(items);
            if (OnAdd != null)
                OnAdd(this, null);
        }
    }
}
