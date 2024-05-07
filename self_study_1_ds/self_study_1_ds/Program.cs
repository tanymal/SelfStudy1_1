using System;
using System.Collections.Generic;
using System.Linq;

class Graph
{
    public int V;
    private List<(int, int)>[] adj;

    public Graph(int v)
    {
        V = v;
        adj = new List<(int, int)>[V];
        for (int i = 0; i < V; ++i)
        {
            adj[i] = new List<(int, int)>();
        }
    }

    public void PrintGraph()
    {
        for (int i = 0; i < V; i++)
        {
            Console.Write("Вершина " + (i + 1) + ": ");
            foreach ((int vertex, int weight) in adj[i])
            {
                Console.Write((vertex + 1) + "(" + weight + ") ");
            }
            Console.WriteLine();
        }
    }

    // Якщо граф неорiєнтований
    public void AddEdge(int v, int w, int weight)
    {
        v--;
        w--;
        adj[v].Add((w, weight));
        adj[w].Add((v, weight));
    }

    // Якщо граф орiєнтований
    public void AddEdgeOne(int v, int w, int weight)
    {
        v--;
        w--;
        adj[v].Add((w, weight));
    }

    // Матриця сумiжностi графа
    public int[,] AdjacencyMatrix()
    {
        int[,] matrix = new int[V, V];
        for (int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                matrix[i, j] = adj[i].Any(x => x.Item1 == j) ? 1 : 0;
            }
        }
        return matrix;
    }

    // Матриця iнцидентностi графа
    public int[,] IncidenceMatrix()
    {
        int[,] matrix = new int[V, adj.Sum(x => x.Count)];
        int edgeIndex = 0;
        for (int i = 0; i < V; i++)
        {
            foreach (var edge in adj[i])
            {
                matrix[i, edgeIndex] = 1;
                matrix[edge.Item1, edgeIndex] = 1;
                edgeIndex++;
            }
        }
        return matrix;
    }

    // Хроматичне число графа
    public int ChromaticNumber()
    {
        return V;
    }

    // Реберне хроматичне число графа
    public int EdgeChromaticNumber()
    {
        return adj.Max(x => x.Count);
    }

    // Рекурсивна функцiя для знаходження Ейлерового шляху у графi
    private void EulerUtil(int v, bool[] visited, List<int> circuit)
    {
        foreach ((int u, int _) in adj[v])
        {
            if (!visited[u])
            {
                visited[u] = true;
                EulerUtil(u, visited, circuit);
            }
        }
        circuit.Add(v);
    }

    // Знаходження всiх Ейлерових ланцюгiв та циклiв
    public List<List<int>> FindEulerianCircuitsAndPaths()
    {
        List<List<int>> circuits = new List<List<int>>();

        // Починаємо з вершини 0
        for (int i = 0; i < V; i++)
        {
            List<int> circuit = new List<int>();
            bool[] visited = new bool[V];
            EulerUtil(i, visited, circuit);

            // Якщо ми знайшли цикл, перевiряємо, чи мiстить вiн всi вершини
            if (circuit[0] == circuit[circuit.Count - 1])
            {
                bool containsAllVertices = true;
                for (int j = 0; j < V; j++)
                {
                    if (!visited[j])
                    {
                        containsAllVertices = false;
                        break;
                    }
                }
                if (containsAllVertices)
                {
                    circuits.Add(circuit);
                }
            }
            else // Якщо це ланцюг, ми його також додаємо
            {
                circuits.Add(circuit);
            }
        }

        return circuits;
    }

    public void DepthFirstSearch(int v, bool[] visited)
    {
        visited[v] = true;
        Console.Write((v + 1) + " ");

        foreach ((int u, int _) in adj[v])
        {
            if (!visited[u])
            {
                DepthFirstSearch(u, visited);
            }
        }
    }

    // Алгоритм Дейкстри для знаходження найкоротших вiдстаней вiд заданої вершини
    public void DijkstraShortestPath(int source)
    {
        int[] dist = new int[V];
        bool[] visited = new bool[V];

        for (int i = 0; i < V; i++)
        {
            dist[i] = int.MaxValue;
        }

        dist[source] = 0;

        for (int count = 0; count < V - 1; count++)
        {
            int u = MinDistance(dist, visited);

            visited[u] = true;

            foreach ((int, int) v in adj[u])
            {
                int neighbor = v.Item1;
                int weight = v.Item2;
                if (!visited[neighbor] && dist[u] != int.MaxValue && dist[u] + weight < dist[neighbor])
                {
                    dist[neighbor] = dist[u] + weight;
                }
            }
        }

        Console.WriteLine("Найкоротшi шляхи вiд вершини " + (source + 1) + ":");
        for (int i = 0; i < V; i++)
        {
            Console.WriteLine("Вершина " + (i + 1) + ": Вiдстань = " + dist[i]);
        }
    }

    // Допомiжна функцiя для знаходження вершини з мiнiмальною вiдстанню
    private int MinDistance(int[] dist, bool[] visited)
    {
        int min = int.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < V; v++)
        {
            if (!visited[v] && dist[v] <= min)
            {
                min = dist[v];
                minIndex = v;
            }
        }

        return minIndex;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Graph g1 = new Graph(6);
        g1.AddEdge(1, 3, 1);
        g1.AddEdge(1, 5, 1);
        g1.AddEdge(2, 4, 1);
        g1.AddEdge(2, 5, 2);
        g1.AddEdge(3, 4, 1);
        g1.AddEdge(4, 6, 1);
        g1.AddEdge(5, 6, 1);

        Console.WriteLine("Граф 1:");
        g1.PrintGraph();
        Console.WriteLine();

        AllInformationGraph(g1);

        Graph g2 = new Graph(6);
        g2.AddEdge(1, 2, 7);
        g2.AddEdge(1, 3, 2);
        g2.AddEdge(1, 4, 4);
        g2.AddEdge(1, 5, 8);
        g2.AddEdge(1, 6, 2);
        g2.AddEdge(2, 4, 3);
        g2.AddEdge(2, 5, 3);
        g2.AddEdge(3, 6, 5);
        g2.AddEdge(4, 5, 7);
        g2.AddEdge(4, 6, 8);
        g2.AddEdge(5, 6, 3);

        Console.WriteLine();

        Console.WriteLine("Граф 2:");
        g2.PrintGraph();
        Console.WriteLine();

        AllInformationGraph(g2);

        Graph g3 = new Graph(8);
        g3.AddEdge(1, 2, 3);
        g3.AddEdge(1, 5, 3);
        g3.AddEdge(1, 8, 1);

        g3.AddEdge(2, 3, 11);
        g3.AddEdge(2, 4, 4);
        g3.AddEdge(2, 8, 5);

        g3.AddEdge(4, 5, 2);
        g3.AddEdge(4, 8, 6);
        g3.AddEdge(5, 6, 4);

        g3.AddEdge(6, 7, 5);
        g3.AddEdge(6, 8, 12);
        g3.AddEdge(7, 8, 1);

        Console.WriteLine();

        Console.WriteLine("Граф 3:");
        g3.PrintGraph();
        Console.WriteLine();

        AllInformationGraph(g3);
    }

    static void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void AllInformationGraph(Graph g)
    {
        Console.WriteLine("Матриця сумiжностi:");
        int[,] adjacencyMatrix = g.AdjacencyMatrix();
        PrintMatrix(adjacencyMatrix);

        Console.WriteLine("\nМатриця iнцидентностi:");
        int[,] incidenceMatrix = g.IncidenceMatrix();
        PrintMatrix(incidenceMatrix);

        Console.WriteLine("\nХроматичне число: " + g.ChromaticNumber());

        Console.WriteLine("Реберне хроматичне число: " + g.EdgeChromaticNumber() + "\n");

        List<List<int>> eulerCircuits = g.FindEulerianCircuitsAndPaths();

        Console.WriteLine("Всi ейлеровi кола та шляхи:");
        foreach (List<int> circuit in eulerCircuits)
        {
            foreach (int vertex in circuit)
            {
                Console.Write((vertex + 1) + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        Console.WriteLine("Пошук в глиб:");
        bool[] visited = new bool[g.V];
        g.DepthFirstSearch(0, visited);

        Console.WriteLine("\n");

        int source = 0;

        g.DijkstraShortestPath(source);
    }
}
