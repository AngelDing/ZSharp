using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Helpers for safely using Task libraries. 
    /// </summary>
    public static class TaskHelpers
    {
        private static readonly Task _defaultCompleted = Task.FromResult<AsyncVoid>(default(AsyncVoid));
        private static readonly Task<object> _completedTaskReturningNull = Task.FromResult<object>(null);
        /// <summary>
        /// We want to prevent callers hijacking the reader thread; this is a bit nasty, but works;
        /// see http://stackoverflow.com/a/22588431/23354 for more information; a huge
        /// thanks to Eli Arbel for spotting this (even though it is pure evil; it is *my kind of evil*)
        /// </summary>
        private static readonly Func<Task, bool> IsSyncSafe;

        static TaskHelpers()
        {
            try
            {
                var taskType = typeof(Task);
                var continuationField = taskType.GetField("m_continuationObject", BindingFlags.Instance | BindingFlags.NonPublic);
                var safeScenario = taskType.GetNestedType("SetOnInvokeMres", BindingFlags.NonPublic);
                if (continuationField != null && continuationField.FieldType == typeof(object) && safeScenario != null)
                {
                    var method = new DynamicMethod("IsSyncSafe", typeof(bool), new[] { typeof(Task) }, typeof(Task), true);
                    var il = method.GetILGenerator();
                    //var hasContinuation = il.DefineLabel();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, continuationField);
                    Label nonNull = il.DefineLabel(), goodReturn = il.DefineLabel();
                    // check if null
                    il.Emit(OpCodes.Brtrue_S, nonNull);
                    il.MarkLabel(goodReturn);
                    il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Ret);

                    // check if is a SetOnInvokeMres - if so, we're OK
                    il.MarkLabel(nonNull);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, continuationField);
                    il.Emit(OpCodes.Isinst, safeScenario);
                    il.Emit(OpCodes.Brtrue_S, goodReturn);

                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Ret);

                    IsSyncSafe = (Func<Task, bool>)method.CreateDelegate(typeof(Func<Task, bool>));

                    // and test them (check for an exception etc)
                    var tcs = new TaskCompletionSource<int>();
                    var expectTrue = IsSyncSafe(tcs.Task);
                    tcs.Task.ContinueWith(delegate { });
                    var expectFalse = IsSyncSafe(tcs.Task);
                    tcs.SetResult(0);
                    if (!expectTrue || expectFalse)
                    {
                        Debug.WriteLine("IsSyncSafe reported incorrectly!");
                        Trace.WriteLine("IsSyncSafe reported incorrectly!");
                        // revert to not trusting /them
                        IsSyncSafe = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Trace.WriteLine(ex.Message);
                IsSyncSafe = null;
            }
            if (IsSyncSafe == null)
                IsSyncSafe = t => false; // assume: not
        }

        /// <summary>
        /// Returns a completed task that has no result. 
        /// </summary>        
        public static Task Completed()
        {
            return _defaultCompleted;
        }

        /// <summary>
        /// Returns a canceled Task. The task is completed, IsCanceled = True, IsFaulted = False.
        /// </summary>
        public static Task Canceled()
        {
            return CancelCache<AsyncVoid>.Canceled;
        }

        /// <summary>
        /// Returns a canceled Task of the given type. The task is completed, IsCanceled = True, IsFaulted = False.
        /// </summary>
        public static Task<TResult> Canceled<TResult>()
        {
            return CancelCache<TResult>.Canceled;
        }       

        /// <summary>
        /// Returns an error task. The task is Completed, IsCanceled = False, IsFaulted = True
        /// </summary>
        public static Task FromError(Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        /// <summary>
        /// Returns an error task of the given type. The task is Completed, IsCanceled = False, IsFaulted = True
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        public static Task<object> NullResult()
        {
            return _completedTaskReturningNull;
        }

        /// <summary>
        /// Used as the T in a "conversion" of a Task into a Task{T}
        /// </summary>
        private struct AsyncVoid
        {
        }

        /// <summary>
        /// This class is a convenient cache for per-type cancelled tasks
        /// </summary>
        private static class CancelCache<TResult>
        {
            public static readonly Task<TResult> Canceled = GetCancelledTask();

            private static Task<TResult> GetCancelledTask()
            {
                TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
                tcs.SetCanceled();
                return tcs.Task;
            }
        }

        public static Task ExecuteSynchronously(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            try
            {
                action();
                tcs.SetResult(null);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
            return tcs.Task;
        }

        public static Task<T> FromResult<T>(T result)
        {
            return Task.FromResult(result);
        }

        public static Task FromException(Exception ex)
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetException(ex);
            return tcs.Task;
        }

        public static Task Delay(TimeSpan delay, CancellationToken cancellation)
        {
            return Task.Delay(delay, cancellation);
        }

        public static Task<Task> WhenAny(params Task[] tasks)
        {
            return Task.WhenAny(tasks);
        }

        public static void TrySetResultSafe<T>(this TaskCompletionSource<T> source, T result)
        {
            if (IsSyncSafe(source.Task))
            {
                source.TrySetResult(result);
            }
            else
            {
                Task.Run(() => source.TrySetResult(result));
            }
        }

        public static void TrySetCanceledSafe<T>(this TaskCompletionSource<T> source)
        {
            if (IsSyncSafe(source.Task))
            {
                source.TrySetCanceled();
            }
            else
            {
                Task.Run(() => source.TrySetCanceled());
            }
        }

        public static void TrySetExceptionSafe<T>(this TaskCompletionSource<T> source, Exception exception)
        {
            if (IsSyncSafe(source.Task))
            {
                source.TrySetException(exception);
            }
            else
            {
                Task.Run(() => source.TrySetException(exception));
            }
        }
    }
}
