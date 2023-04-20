using System.Collections.Generic;

namespace DMCALL
{
    internal class VF2
    {
        public static bool Main(int[,] graphMatrix1, int[,] graphMatrix2)
        {
            if (graphMatrix1.GetLength(0) != graphMatrix2.GetLength(0))
            { return false; }
            VF2 vf2 = new VF2(graphMatrix1, graphMatrix2);
            bool isomorphic = vf2.IsIsomorphic();
            return isomorphic;
        }

        int[,] g1, g2; // матриці суміжності графів
        int n1, n2; // кількість вершин графів
        List<int>[] adj1, adj2; // списки суміжності графів
        bool[,] m; // матриця відповідностей вершин
        bool[] used1, used2; // масиви відвідування вершин графів
        int[] p1, p2; // масиви для зберігання відповідностей вершин графів

        public VF2(int[,] g1, int[,] g2)
        {
            this.g1 = g1;
            this.g2 = g2;
            n1 = g1.GetLength(0);
            n2 = g2.GetLength(0);
            adj1 = new List<int>[n1];
            adj2 = new List<int>[n2];
            for (int i = 0; i < n1; i++)
            {
                adj1[i] = new List<int>();
                for (int j = 0; j < n1; j++)
                    if (g1[i, j] == 1) adj1[i].Add(j);
            }
            for (int i = 0; i < n2; i++)
            {
                adj2[i] = new List<int>();
                for (int j = 0; j < n2; j++)
                    if (g2[i, j] == 1) adj2[i].Add(j);
            }
            m = new bool[n1, n2];
            used1 = new bool[n1];
            used2 = new bool[n2];
            p1 = new int[n1];
            p2 = new int[n2];
        }

        public bool IsIsomorphic()
        {
            for (int i = 0; i < n1; i++)
                for (int j = 0; j < n2; j++)
                    m[i, j] = false;
            return Recurse(0);
        }

        bool Recurse(int depth)
        {
            if (depth == n1) return true;
            for (int i = 0; i < n2; i++)
            {
                if (!IsFeasible(depth, i)) continue;
                p1[depth] = i;
                used1[depth] = true;
                for (int j = 0; j < adj1[depth].Count; j++)
                {
                    int u = adj1[depth][j];
                    if (!used1[u]) m[u, i] = true;
                }
                for (int j = 0; j < adj2[i].Count; j++)
                {
                    int v = adj2[i][j];
                    if (!used2[v]) m[depth, v] = true;
                }
                p2[i] = depth;
                used2[i] = true;
                if (Recurse(depth + 1)) return true;
                used1[depth] = false;
                used2[i] = false;
                for (int j = 0; j < adj1[depth].Count; j++)
                {
                    int u = adj1[depth][j];
                    if (!used1[u]) m[u, i] = false;
                }
                for (int j = 0; j < adj2[i].Count; j++)
                {
                    int v = adj2[i][j];
                    if (!used2[v]) m[depth, v] = false;
                }
            }
            return false;
        }

        bool IsFeasible(int depth, int i)
        {
            if (used2[i]) return false;
            for (int j = 0; j < adj1[depth].Count; j++)
            {
                int u = adj1[depth][j];
                if (m[u, i]) return false;
            }
            for (int j = 0; j < adj2[i].Count; j++)
            {
                int v = adj2[i][j];
                if (used1[v] && !m[v, i]) return false;
            }
            return true;
        }
    }
}
