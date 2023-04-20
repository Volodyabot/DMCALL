using System;
using System.Collections.Generic;

namespace DMCALL
{
    internal class DijkstraAlgorithm
    {
        public static int[] Dijkstra(int[,] graph, int fromIndex)
        {

            int[] distances = new int[graph.GetLength(0)];

            for (int i = 0; i < distances.Length; i++)
            { 
                distances[i] = int.MaxValue; 
            }

            bool[] visited = new bool[graph.GetLength(0)];

            int baseIndex = fromIndex;
            distances[baseIndex] = 0;
            visited[baseIndex] = true;

            for (int i = 0; i < distances.Length; i++)
            {
                Console.WriteLine("[{0}], {1}", i, distances[i]);
            }

            bool stopCycle = false; int c = 0;
            while (!stopCycle)
            {
                SetDistances(graph, distances, baseIndex, visited);
                Console.WriteLine("/////////");
                for (int i = 0; i < distances.Length; i++)
                {
                    Console.WriteLine("[{0}], {1}", i, distances[i]);
                }

                int minDistance = FindUnvisitedMinDistanceIndex(distances, visited);
                Console.WriteLine("\nMinDistanceIndex: {0}\n", minDistance);
                visited[minDistance] = true;
                baseIndex = minDistance;

                stopCycle = true;
                for (int i = 0; i < visited.Length; i++)
                {
                    if (!visited[i])
                    {
                        stopCycle = false;
                        break;
                    }
                }
            }

            return distances;

        }

        private static int[] SetDistances(int[,] graph, int[] distances, int baseIndex, bool[] visited)
        {
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                if (graph[baseIndex, i] > 0 && distances[baseIndex] + graph[baseIndex, i] <= distances[i] && !visited[i])
                {
                    distances[i] = distances[baseIndex] + graph[baseIndex, i];
                }
            }
            return distances;
        }

        private static int FindUnvisitedMinDistanceIndex(int[] distance, bool[] visited)
        {
            int minDistanceIndex = -1;
            int min = int.MaxValue;
            for (int i = 0; i < distance.Length; i++)
            {
                if (distance[i] <= min && distance[i] > 0 && !visited[i])
                {
                    min = distance[i];
                    minDistanceIndex = i;
                }
            }

            return minDistanceIndex;
        }

        public static List<int> Dijkstra(int[,] graph, int fromIndex, int toIndex)
        {
            List<int> verticesPathIndex= new List<int>();
            List<List<int>> resultVerticesPathIndex = new List<List<int>>();

            int[] distances = new int[graph.GetLength(0)];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = int.MaxValue;
            }

            bool[] visited = new bool[graph.GetLength(0)];

            int baseIndex = fromIndex;
            distances[baseIndex] = 0;
            visited[baseIndex] = true;

            bool stopCycle = false;
            while (!stopCycle)
            {
                visited[baseIndex] = true;
                verticesPathIndex.Add(baseIndex);

                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    if (graph[baseIndex, i] > 0 && distances[baseIndex] + graph[baseIndex, i] <= distances[i] && !visited[i])
                    {
                        distances[i] = distances[baseIndex] + graph[baseIndex, i];
                        if (i == toIndex)
                        {
                            List<int> tempList = new List<int>();
                            foreach(int e in verticesPathIndex)
                            {
                                tempList.Add(e);
                            }
                            tempList.Add(toIndex);
                            resultVerticesPathIndex.Add(tempList);
                        }
                    }
                }

                int minDistance = FindUnvisitedMinDistanceIndex(distances, visited);

                baseIndex = minDistance;

                stopCycle = true;
                for (int i = 0; i < visited.Length; i++)
                {
                    if (!visited[i])
                    {
                        stopCycle = false;
                        break;
                    }
                }
            }

            return resultVerticesPathIndex[resultVerticesPathIndex.Count - 1];
        }

    }
}
