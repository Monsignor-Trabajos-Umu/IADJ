using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    public class PathRequestManagerB : MonoBehaviour
    {
        private static PathRequestManagerB _instance;
        private PathRequest currentPathRequest;

        private bool isProcessingPath;
        [SerializeField] private PathFindingA pathfinding;

        private readonly Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();

        private void Awake()
        {
            _instance = this;
        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd,Heuristic heuristic,AgentNpc agente,
            Action<Vector3[], bool> callback)
        {
            var newRequest = new PathRequest(pathStart, pathEnd,heuristic,agente, callback);
            _instance.pathRequestQueue.Enqueue(newRequest);
            _instance.TryProcessNext();
        }

        private void TryProcessNext()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.pathStart,
                    currentPathRequest.pathEnd,currentPathRequest.heuristic,currentPathRequest.agente);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        private struct PathRequest
        {
            public readonly Vector3 pathStart;
            public readonly Vector3 pathEnd;

            public readonly Heuristic heuristic;
            public readonly AgentNpc agente;
            public readonly Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end,Heuristic _heuristic,AgentNpc _agente,
                Action<Vector3[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                agente = _agente;
                heuristic = _heuristic;
                callback = _callback;
            }
        }
    }
}