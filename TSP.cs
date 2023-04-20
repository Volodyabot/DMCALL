
namespace DMCALL
{
    class TSP
    {
        public static int[] NearestNeighbor(int[,] graph)
        {
            int n = graph.GetLength(0); // кількість вершин
            int[] path = new int[n + 1]; // масив для зберігання шляху
            bool[] visited = new bool[n]; // масив для відстеження відвіданих вершин

            // починаємо з вершини 0
            int current = 0;
            path[0] = 0;
            visited[0] = true;

            // додаємо до шляху найближчу невідвідану вершину
            for (int i = 1; i < n; i++)
            {
                int next = -1;
                int minDist = int.MaxValue;
                for (int j = 0; j < n; j++)
                {
                    if (!visited[j] && graph[current, j] != 0 && graph[current, j] < minDist)
                    {
                        next = j;
                        minDist = graph[current, j];
                    }
                }
                current = next;
                visited[current] = true;
                path[i] = current;
            }

            // повертаємо шлях, що повертає до початкової вершини
            path[n] = 0;
            return path;
        }
        public static int[] NearestNeighbor(int[,] graph, int start)
        {
            int n = graph.GetLength(0); // кількість вершин
            int[] path = new int[n + 1]; // масив для зберігання шляху
            bool[] visited = new bool[n]; // масив для відстеження відвіданих вершин

            // починаємо з визначеної початкової вершини
            int current = start;
            path[0] = start;
            visited[start] = true;

            // додаємо до шляху найближчу невідвідану вершину
            for (int i = 1; i < n; i++)
            {
                int next = -1;
                int minDist = int.MaxValue;
                for (int j = 0; j < n; j++)
                {
                    if (!visited[j] && graph[current, j] != 0 && graph[current, j] < minDist)
                    {
                        next = j;
                        minDist = graph[current, j];
                    }
                }
                current = next;
                visited[current] = true;
                path[i] = current;
            }

            // повертаємо шлях, що повертає до початкової вершини
            path[n] = start;
            return path;
        }
    }
}
