/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Threading;

namespace magic.lambda.slots.utilities
{
    /*
     * Helper class to synchronize access to some shared resource, potentially shared among
     * multiple threads.
     */
    internal class Synchronizer<TImpl>
    {
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        readonly TImpl _shared;

        public Synchronizer(TImpl shared)
        {
            _shared = shared;
        }

        /*
         * Allows read only access inside of your lambda functor, while
         * expecting you to return some object during invocation.
         */
        public T Read<T>(Func<TImpl, T> functor)
        {
            _lock.EnterReadLock();
            try
            {
                return functor(_shared);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /*
         * Allows read and write access inside of your lambda functor.
         */
        public void Write(Action<TImpl> functor)
        {
            _lock.EnterWriteLock();
            try
            {
                functor(_shared);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}