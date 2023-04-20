using System.Collections.Generic;

namespace DMCALL
{
    internal class PostManAlgorithm
    {
        public static List<int> Main(int[,] matrix)
        {
            List<int> path = NearestNeighbor(matrix);
            return path;
        }
        public static List<int> NearestNeighbor(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            var visited = new bool[n];
            var path = new List<int>();

            // Починаємо з вузла 0
            int currentNode = 0;
            path.Add(currentNode);
            visited[currentNode] = true;

            // Продовжуємо до тих пір, поки всі вузли не будуть відвідані
            while (path.Count < n)
            {
                int nextNode = -1;
                int minDistance = int.MaxValue;

                // Знаходимо найближчий невідвіданий вузол
                for (int i = 0; i < n; i++)
                {
                    if (!visited[i] && matrix[currentNode, i] < minDistance && matrix[currentNode, i] != 0)
                    {
                        nextNode = i;
                        minDistance = matrix[currentNode, i];
                    }
                }

                // Якщо найближчий вузол знайдено, додаємо його до маршруту
                if (nextNode != -1)
                {
                    path.Add(nextNode);
                    visited[nextNode] = true;
                    currentNode = nextNode;
                }
                // Якщо найближчий вузол не знайдено, повертаємося до початкового вузла
                else
                {
                    path.Add(0);
                    currentNode = 0;
                }
            }

            return path;
        }

    }
}
