using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace PBCore.Threading
{
    /// <summary>
    /// 可与游戏主线程交互的线程工具
    /// </summary>
    //void ScaleMesh(Mesh mesh, float scale)
    //{
    //    //Get the vertices of a mesh
    //    var vertices = mesh.vertices;
    //    //Run the action on a new thread
    //    Loom.RunAsync(() => {
    //        //Loop through the vertices
    //        for (var i = 0; i < vertices.Length; i++)
    //        {
    //            //Scale the vertex
    //            vertices[i] = vertices[i] * scale;
    //        }
    //        //Run some code on the main thread
    //        //to update the mesh
    //        Loom.QueueOnMainThread(() => {
    //            //Set the vertices
    //            mesh.vertices = vertices;
    //            //Recalculate the bounds
    //            mesh.RecalculateBounds();
    //        });

    //    });
    //}
    public class Loom : MonoBehaviour
    {
        #region StructMember
        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }
        #endregion

        #region Member
        public static int mMaxThreads = 8;

        private static Loom _current;
        public static Loom Current
        {
            get
            {
                Initialize();
                return _current;
            }
        }

        private static int mNumThreads;
        private static bool initialized;

        private int _count;
        private List<Action> _actions = new List<Action>();
        private List<Action> _currentActions = new List<Action>();
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
        private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        #endregion
        #region MonoLife

        void Awake()
        {
            _current = this;
            initialized = true;
        }

        void OnEnable()
        {
            if (null == _current)
            {
                _current = this;
            }
        }

        void OnDisable()
        {
            if (this == _current)
            {
                _current = null;
            }
        }

        void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }

            for (int i = 0; i < _currentActions.Count; i++)
            {
                _currentActions[i]();
            }

            lock (_delayed)
            {
                _currentDelayed.Clear();

                for (int i = _delayed.Count - 1; i > 0; i--)
                {
                    if (_delayed[i].time > Time.time)
                    {
                        continue;
                    }

                    _currentDelayed.Add(_delayed[i]);
                    _delayed.RemoveAt(i);

                }
            }

            for (int i = 0; i < _currentDelayed.Count; i++)
            {
                _currentDelayed[i].action();
            }

        }

        #endregion
        #region BusinessLogic

        private static void Initialize()
        {
            if (initialized == true) return;
            if (Application.isPlaying == false) return;

            initialized = true;
            var g = new GameObject("Loom");
            _current = g.AddComponent<Loom>();

        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Interlocked.Decrement(ref mNumThreads);
            }
        }

        #endregion
        #region PublicTools
        public static void QueueOnMainThread(Action action, float time = 0.0f)
        {
            if (time != 0)
            {
                lock (Current._delayed)
                {
                    Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (Current._actions)
                {
                    Current._actions.Add(action);
                }
            }
        }

        public static Thread RunAsync(Action varAction)
        {
            Initialize();
            while (mNumThreads >= mMaxThreads)
            {
                System.Threading.Thread.Sleep(1);
            }
            Interlocked.Increment(ref mNumThreads);
            ThreadPool.QueueUserWorkItem(RunAction, varAction);
            return null;
        }
        #endregion
    }
}
