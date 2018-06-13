using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilentWeb.Services
{
    public static class Match
    {
        //Inputs
        public static List<double[]> edges;
        public static List<int> nodes; //SmartPhones Ids

        public static List<List<int>> solutions = new List<List<int>>();

        public static void GoThroughGraph(List<int> sol, int currentIndex)
        {
            //Check solution
            if (CheckSolution(sol))
            {
                solutions.Add(new List<int>(sol));
                return;
            }

            for (int index = 0; index < nodes.Count; index ++)
            {
                sol.Add(nodes[index]);
                if (Check(sol,currentIndex))
                {
                    GoThroughGraph(sol, currentIndex + 1);
                }
                sol.RemoveAt(sol.Count - 1);
            }
        }

        public static bool Check(List<int> sol, int currentIndex)
        {
            if (currentIndex == 0)
            {
                return true;
            }
            //Toate distincte
            for (int i = 0; i < currentIndex; i++)
            {
                if (sol[i] == sol[currentIndex])
                {
                    return false;
                }
            }
            //Edge existence
            bool ok = false;
            foreach (double[] edge in edges)
            {
                if (edge[0] == sol[currentIndex - 1] && edge[1] == sol[currentIndex])
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                return false;
            }

            //Date diff
            if (currentIndex >= 2)
            {
                double[] edge1 = null;
                double[] edge2 = null;
                foreach (double[] edge in edges)
                {
                    if (edge[0] == sol[currentIndex - 1] && edge[1] == sol[currentIndex])
                    {
                        edge1 = edge;
                    }
                    if (edge[0] == sol[currentIndex - 2] && edge[1] == sol[currentIndex - 1])
                    {
                        edge2 = edge;
                    }
                }

                if (edge1 != null && edge2 != null)
                {
                    if (edge2[2] - edge1[3] > 300000)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckSolution(List<int> sol)
        {
            if (sol.Count >= 3)
            {
                foreach (List<int> solution in solutions)
                {
                    int size;
                    if (sol.Count < solution.Count)
                    {
                        size = sol.Count;
                    }
                    else
                    {
                        size = solution.Count;
                    }
                    int counter = 0;
                    for (int i=0; i<size; i++)
                    {
                        if (sol[i] == solution[i])
                        {
                            counter++;
                        }
                    }
                    if (counter == size && sol.Count == solution.Count)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static List<List<int>> Start(List<double[]> list, List<int> nodList)
        {
            edges = list;
            nodes = nodList;
            List<int> sol = new List<int>();

            GoThroughGraph(sol, 0);
            return solutions;
        }
    }
}