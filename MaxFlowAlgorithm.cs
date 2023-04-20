using System;
using System.Collections.Generic;

namespace DMCALL
{
    class MaxFlowAlgorithm
    {
        private static int BFS(int[,] rGraph, int s, int t, int[] parent)
        {
            int V = rGraph.GetLength(0);
            bool[] visited = new bool[V];

            for (int i = 0; i < V; ++i)
            {
                visited[i] = false;
            }

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            visited[s] = true;
            parent[s] = -1;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < V; ++v)
                {
                    if (!visited[v] && rGraph[u, v] > 0)
                    {
                        queue.Enqueue(v);
                        visited[v] = true;
                        parent[v] = u;
                    }
                }
            }

            return (visited[t] == true) ? 1 : 0;
        }

        public static int FordFulkersonAlgo(int[,] graph, int s, int t)
        {
            int V = graph.GetLength(0);
            int[,] rGraph = new int[V, V];
            int u, v;

            for (u = 0; u < V; u++)
            {
                for (v = 0; v < V; v++)
                {
                    rGraph[u, v] = graph[u, v];
                }
            }

            int[] parent = new int[V];

            int max_flow = 0;

            // Зберігаємо шлях потоку
            List<int> path = new List<int>();

            while (BFS(rGraph, s, t, parent) > 0)
            {
                int path_flow = int.MaxValue;

                // Знаходимо мінімальну пропускну здатність шляху
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u, v]);
                }

                // Додаємо вершини шляху до списку path
                path.Clear();
                int path_node = t;
                path.Add(path_node);
                while (path_node != s)
                {
                    path_node = parent[path_node];
                    path.Add(path_node);
                }
                path.Reverse();

                // Оновлюємо пропускну здатність ребер шляху та ребра, які протікають у зворотному напрямку
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                }

                // Додаємо максимальний потік шляху до потоку
                max_flow += path_flow;
            }

            // Виводимо шлях потоку
            Console.Write("Шлях потоку: ");
            foreach (int node in path)
            {
                Console.Write(node + " ");
            }
            Console.WriteLine();

            return max_flow;
        }

        public static List<int> FordFulkersonAlgo(int[,] graph, int s, int t, bool ReturnList)
        {
            int V = graph.GetLength(0);
            int[,] rGraph = new int[V, V];
            int u, v;

            for (u = 0; u < V; u++)
            {
                for (v = 0; v < V; v++)
                {
                    rGraph[u, v] = graph[u, v];
                }
            }

            int[] parent = new int[V];

            int max_flow = 0;

            // Зберігаємо шлях потоку
            List<int> path = new List<int>();

            while (BFS(rGraph, s, t, parent) > 0)
            {
                int path_flow = int.MaxValue;

                // Знаходимо мінімальну пропускну здатність шляху
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u, v]);
                }

                // Додаємо вершини шляху до списку path
                path.Clear();
                int path_node = t;
                path.Add(path_node);
                while (path_node != s)
                {
                    path_node = parent[path_node];
                    path.Add(path_node);
                }
                path.Reverse();

                // Оновлюємо пропускну здатність ребер шляху та ребра, які протікають у зворотному напрямку
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                }

                // Додаємо максимальний потік шляху до потоку
                max_flow += path_flow;
            }

            return path;
        }
    }
}