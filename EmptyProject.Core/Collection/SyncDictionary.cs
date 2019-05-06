using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace EmptyProject.Core.Collection
{
    public class SyncDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public new void Add(TKey key, TValue value)
        {
            _lock.EnterWriteLock();
            base.Add(key, value);
            _lock.ExitWriteLock();
        }

        public new void Clear()
        {
            _lock.EnterWriteLock();
            base.Clear();
            _lock.ExitWriteLock();

        }
        public new bool ContainsKey(TKey key)
        {
            _lock.EnterReadLock();
            var rv = base.ContainsKey(key);
            _lock.ExitReadLock();
            return rv;
        }



        public new bool ContainsValue(TValue value)
        {
            _lock.EnterReadLock();
            var rv = base.ContainsValue(value);
            _lock.ExitReadLock();
            return rv;
        }


        public new Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            _lock.EnterReadLock();
            var rv = new Dictionary<TKey, TValue>(this).GetEnumerator();
            _lock.ExitReadLock();
            return rv;
        }

        public new bool Remove(TKey key)
        {
            _lock.EnterWriteLock();
            var rv = base.Remove(key);
            _lock.ExitWriteLock();
            return rv;
        }

        public new bool TryGetValue(TKey key, out TValue value)
        {
            _lock.EnterReadLock();
            var rv = base.TryGetValue(key, out value);
            _lock.ExitReadLock();
            return rv;
        }

        public new int Count
        {
            get
            {
                _lock.EnterReadLock();
                var rv = base.Count;
                _lock.ExitReadLock();
                return rv;
            }
        }


        public new TValue this[TKey key]
        {
            get
            {
                _lock.EnterReadLock();
                var rv = base[key];
                _lock.ExitReadLock();
                return rv;
            }
            set
            {
                _lock.EnterWriteLock();
                base[key] = value;
                _lock.ExitWriteLock();

            }
        }


        public new Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                _lock.EnterReadLock();
                var rv = base.Keys;
                _lock.ExitReadLock();
                return rv;
            }
        }

        public new Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                _lock.EnterReadLock();
                var rv = base.Values;
                _lock.ExitReadLock();
                return rv;
            }

        }
    }
}
