using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SilentWeb.Services
{
    public static class Graphviz
    {
        public const string LIB_GVC = @".\external\gvc.dll";
        public const string LIB_GRAPH = @".\external\cgraph.dll";
        public const int SUCCESS = 0;

        ///
        /// Creates a new Graphviz context.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gvContext();

        ///
        /// Releases a context's resources.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvFreeContext(IntPtr gvc);

        ///
        /// Reads a graph from a string.
        ///
        [DllImport(LIB_GRAPH, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr agmemread(string data);

        ///
        /// Releases the resources used by a graph.
        ///
        [DllImport(LIB_GRAPH, CallingConvention = CallingConvention.Cdecl)]
        public static extern void agclose(IntPtr g);

        ///
        /// Applies a layout to a graph using the given engine.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvLayout(IntPtr gvc, IntPtr g, string engine);

        ///
        /// Releases the resources used by a layout.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvFreeLayout(IntPtr gvc, IntPtr g);

        ///
        /// Renders a graph to a file.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvRenderFilename(IntPtr gvc, IntPtr g,
              string format, string fileName);

        ///
        /// Renders a graph in memory.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvRenderData(IntPtr gvc, IntPtr g,
             string format, out IntPtr result, out int length);

        ///
        /// Release render resources.
        ///
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gvFreeRenderData(IntPtr result);


        public static Image RenderImage(string source, string format)
        {
            // Create a Graphviz context
            IntPtr gvc = gvContext();
            if (gvc == IntPtr.Zero)
                throw new Exception("Failed to create Graphviz context.");

            // Load the DOT data into a graph
            IntPtr g = agmemread(source);
            if (g == IntPtr.Zero)
                throw new Exception("Failed to create graph from source. Check for syntax errors.");

            // Apply a layout
            if (gvLayout(gvc, g, "dot") != SUCCESS)
                throw new Exception("Layout failed.");

            IntPtr result;
            int length;

            // Render the graph
            if (gvRenderData(gvc, g, format, out result, out length) != SUCCESS)
                throw new Exception("Render failed.");

            // Create an array to hold the rendered graph
            byte[] bytes = new byte[length];

            // Copy the image from the IntPtr
            Marshal.Copy(result, bytes, 0, length);

            // Free up the resources
            gvFreeLayout(gvc, g);
            agclose(g);
            gvFreeContext(gvc);
            gvFreeRenderData(result);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return Image.FromStream(stream);
            }
        }

        public static string GenerateInput(string connectionString, string username)
        {
            DataTable smartphones = SqlHelper.GetSpecificInformation(connectionString, username, "GetRegisteredPhones");
            DataTable calls = SqlHelper.GetSpecificInformation(connectionString, username, "GetCallHistoryRelations");
            List<double[]> edges = GenerateEdgeList(smartphones, calls);
            List<int> smart = new List<int>();
            StringBuilder input = new StringBuilder("digraph G {labelloc=t; label=\"Call relations between registerd smartphones\"; node [color=blue,style=bold,shape=ellipse]; ");
            foreach (DataRow row in smartphones.Rows)
            {
                smart.Add(int.Parse(row["Id"].ToString()));
                input.Append(row["Id"].ToString() + " [label=\"" + row["Manufacturer"].ToString() + "\n" +
                    row["Model"].ToString() + "\n" + row["IMEI"].ToString() + "\n" + row["Number"].ToString() + "\"]; ");
            }

            List<List<int>> solutions = Match.Start(edges, smart);
            input.Append("edge [fontsize=10,style=bold]; ");
            int colorIndex = 0;
            string[] colors = {"purple", "red", "green", "yellow", "orange", "blue"};
            foreach (List<int> solution in solutions)
            {
                double[] firstEdge = new double[4];
                foreach (double[] edge in edges)
                {
                    if (solution[0] == edge[0] && solution[1] == edge[1])
                    {
                        firstEdge = edge;
                    }
                }
                for (int i=0; i<solution.Count; i++)
                {
                    if (i == solution.Count - 1)
                    {
                        input.Append(solution[i] + " [color=" + colors[colorIndex] + "]; ");
                    }
                    else if (i == 1)
                    {
                        TimeSpan time = TimeSpan.FromMilliseconds(firstEdge[2]);
                        DateTime startdate = new DateTime(1970, 1, 1) + time;
                        input.Append(solution[i] + "[label=\"" + startdate.ToString() +"\",color=" + colors[colorIndex] + "]; " + solution[i] + "->");
                    }
                    else
                    {
                        input.Append(solution[i] + "->");
                    }
                }
                colorIndex++;
            }
            input.Append("}");
            return input.ToString();
        }

        public static List<double[]> GenerateEdgeList(DataTable smartphones, DataTable calls)
        {
            List<double[]> result = new List<double[]>();
            foreach (DataRow row in calls.Rows)
            {
                double[] edge = { 0, 0, 0, 0 };
                edge[0] = Int32.Parse(row["PhoneId"].ToString());
                foreach (DataRow roww in smartphones.Rows)
                {
                    if (Reverse(roww["Number"].ToString()) == Reverse(row["Number"].ToString()))
                    {
                        edge[1] = Int32.Parse(roww["Id"].ToString());
                    }
                }
                DateTime called = Convert.ToDateTime(row["Date"]);
                DateTime endCall = called.AddSeconds(Double.Parse(row["Duration"].ToString()));
                edge[2] = called.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                edge[3] = endCall.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                result.Add(edge);
            }
            return result;
        }

        public static string Reverse(string s)
        {
            if (s.Length < 10)
            {
                return "False";
            }
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            char[] result = new char[10];
            for (int i = 0; i < 10; i++)
            {
                result[i] = charArray[i];
            }
            return new string(result);
        }
    }
}