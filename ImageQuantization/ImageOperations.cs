using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Globalization;
///Algorithms Project
///Intelligent Scissors
///

namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
    }
    public struct RGBPixelD
    {
        public double red, green, blue;
    }
    public class Quantize
    {
        private RGBPixel[,] ImageMatrix;
        //Array To Check The Repeatition Of Colors
        public static int[] check;
        public Quantize(RGBPixel[,] Image)
        {
            ImageMatrix = Image;
        }
        //Function To Get The Distincit Color From 2d Matrix
        public void Check(int[] check, RGBPixel[,] ImageMatrix, int i, List<RGBPixel> Distinct)
        {
            int x = 65536;//------> O(1)
            int j = 0;//------> O(1)
            while (j < ImageOperations.GetWidth(ImageMatrix))  //------> O(n)*O(1) the whole loop * body 
            {
                int g = ImageMatrix[i, j].green;//------> O(1)
                int r = ImageMatrix[i, j].red;//------> O(1)
                int b = ImageMatrix[i, j].blue;//------> O(1)
                r = g + b * x / 256 + r * x;//------> O(1)
                if (check[r] == 0) //------> O(1)
                {
                    check[r] = Distinct.Count;//------> O(1)
                    Distinct.Add(ImageMatrix[i, j]); //------> O(1)
                }
                j++;//------> O(1)+O(1)
            }
        }
        public List<RGBPixel> Find_Distinct()
        {
            check = new int[200000000];//------> O(1)
            List<RGBPixel> Distinct = new List<RGBPixel>();//------> O(1)
            for (int i = 0; i < ImageOperations.GetHeight(ImageMatrix); i++)//------> O(n)
            {
                Check(check, ImageMatrix, i, Distinct);//------> O(n)
            }
            //------> O(n2) loop *body   
            return Distinct ;//------> O(1)
        }

 //===========================================================================================================================

        // MST Prim Algorithm Step :


        //Function to Calculate The Distance Between Any 2 Colors
        public double ECl_Distance(RGBPixel col_1, RGBPixel col_2)
        {
            return Math.Sqrt(((col_1.red - col_2.red) * (col_1.red - col_2.red)) + ((col_1.green - col_2.green) * (col_1.green - col_2.green)) + ((col_1.blue - col_2.blue) * (col_1.blue - col_2.blue)));
        }                                                                                   //--> O(1)

        // ** Variables section ** :
        //===========================
        public struct MST_var
        {
            //Save The Total Weight Of The Minimum Spanning Tree Cost
            public static double tree_Cost;                                                 //--> O(1)
            //Save The Minimum Weight Cost Of Each Node
            public static double[] Node_Cost;                                               //--> O(1)
            //Save The Parent Index Of Each Node
            public static int[] parent;                                                     //--> O(1)
            //Array To Know The Visited Nodes
            public static bool[] marked_vis;                                                //--> O(1)

            // Temp Variables
            public static int child;                                                        //--> O(1)
            public static double min_weight;                                                //--> O(1)
            public static int cursor;                                                       //--> O(1)
        }

        // Function To Calculate The Minimum Spanning Tree :
        //====================================================
        public double Build_Mst(ref List<KeyValuePair<KeyValuePair<int, int>, double>> edges, int Dcol_count, List<RGBPixel> Col)
        {
            // ** Intialization Section ** :
            //==============================
            // MST_var Intialization
            MST_var.tree_Cost = 0;                                                            //--> O(1)
            MST_var.Node_Cost = new double[Dcol_count + 1];                                  //--> O(1)
            MST_var.parent = new int[Dcol_count + 1];                                       //--> O(1)
            MST_var.cursor = 1;                                                            //--> O(1)
            MST_var.marked_vis = new bool[Dcol_count + 1];                                //--> O(1)


            //Initialize The Minimum Weight Cost Of Each Node With 10^9
            for (int i = 1; i <= Dcol_count; i++)
            {
                MST_var.Node_Cost[i] = 1000000000;
            } //--> O(N) : where N is number of distinct colors

            MST_var.Node_Cost[1] = 0; //--> O(1)

            // MST Logic 
            while (MST_var.cursor < Dcol_count) //--> O(Log(V)) : where V is the number of vertices
            {
                MST_var.marked_vis[MST_var.cursor] = true; //--> O(1)
                MST_var.min_weight = 1000000000; //--> O(1)
                MST_var.tree_Cost += MST_var.Node_Cost[MST_var.cursor]; //--> O(1)
                MST_var.child = 0; //--> O(1)

                for (int ch = 2; ch < Dcol_count; ch++)//--> O(E) : where E is the number of Edges
                {
                    // If this node not Visited yet
                    if (MST_var.marked_vis[ch] == false) //--> O(1)
                    {
                        //Calculate The Weight On The Edge Between The Current Vertix And its Children

                        double weight = ECl_Distance(Col[MST_var.cursor], Col[ch]); //--> O(1)

                        //Check If I Pushed The Same Vertix With A Smaller Cost
                        if (MST_var.Node_Cost[ch] > weight) //--> O(1)   
                        {
                            //Updating The Weight Of The Child Node
                            MST_var.Node_Cost[ch] = weight; //--> O(1)
                            MST_var.parent[ch] = MST_var.cursor; //--> O(1)

                        }
                        if (MST_var.Node_Cost[ch] < MST_var.min_weight) //--> O(1)
                        {
                            MST_var.min_weight = MST_var.Node_Cost[ch]; //--> O(1)
                            MST_var.child = ch; //--> O(1)
                        }
                    }
                } //--> O(N) : where N is number of distinct colors

                if (MST_var.child == 0) break;

                edges.Add(new KeyValuePair<KeyValuePair<int, int>, double>(new KeyValuePair<int, int>(MST_var.cursor, MST_var.child), MST_var.min_weight));
                MST_var.cursor = MST_var.child;

            } //--> O(E Log(v)) 

            //Return The Total Weight Cost Of MST

            return MST_var.tree_Cost;
        }

    }

    // Total Complexity of the MST Prim Algorism is --> O(E Log(V))

    //===========================================================================================================================

    public class Cluster
    {
        struct info
        {
            public static bool[] visited;
            public static int count;
            public static int[] hold;
            public static int current;
            public static int reds, greens, blues;
        }
        List<RGBPixel> Clusters = new List<RGBPixel>();//------> O(1)
        public List<int>[] adjList;//------> O(1)
        List<RGBPixel> Colours;//------> O(1)
        RGBPixel[,] ImageMatrix;//------> O(1)
        public Cluster(List<RGBPixel> l, RGBPixel[,] matrix)
        {
            Colours = l;
            ImageMatrix = matrix;
        }
        void Breadth_First_Search(int j) //---->O(V+E)
        {
            Queue<int> q = new Queue<int>();//------> O(1)
            q.Enqueue(j);//------> O(1)
            while (q.Count > 0)//------> O(N)
            {
                j = q.Dequeue();
                info.visited[j] = true;
                info.count++;//------> O(1)
                info.hold[j] = info.current;//------> O(1)
                info.reds += Colours[j].red;//------> O(1)
                info.greens += Colours[j].green;//------> O(1)
                info.blues += Colours[j].blue;//------> O(1)
                for (int o = 0; o < adjList[j].Count; o++)//------> O(N)
                {
                    if (info.visited[adjList[j][o]] == false)//------> O(1)
                        q.Enqueue(adjList[j][o]);//------> O(1)
                }
            }
        }
        public void calc(int j)
        {
            if (info.visited[j] == false)//------> O(1)
            {
                info.count = 0;//------> O(1)
                info.reds = 0;//------> O(1)
                info.greens = 0; //------> O(1)
                info.blues = 0;//------> O(1)
                Breadth_First_Search(j);
                info.reds /= info.count;//------> O(1)
                info.greens /= info.count;//------> O(1)
                info.blues /= info.count;//------> O(1)
                RGBPixel tmp = new RGBPixel();//------> O(1)
                tmp.red = (byte)info.reds;//------> O(1)
                tmp.green = (byte)info.greens;//------> O(1)
                tmp.blue = (byte)info.blues;//------> O(1)
                info.current++;//------> O(1)
                Clusters.Add(tmp);//------> O(1)
            }
        }
        public List<int>[] clustr(List<int>[] adjList, List<KeyValuePair<KeyValuePair<int, int>, double>> edges, int num_of_clusters, int K, int i)
        {

            while (num_of_clusters > K)//------> O(K)
            {
                KeyValuePair<int, int> Edge = new KeyValuePair<int, int>(edges[i].Key.Key, edges[i].Key.Value);//------> O(1)
                adjList[Edge.Key].Add(Edge.Value);//------> O(1)
                adjList[Edge.Value].Add(Edge.Key);//------> O(1)
                i++;//------> O(1)
                num_of_clusters--;//------> O(1)
            }
            return adjList;
        }
        public void Identify_Clusters(List<KeyValuePair<KeyValuePair<int, int>, double>> edges, int D, int K)
        {
            MergeSort(edges, 0, edges.Count - 1);
            adjList = new List<int>[D + 1];//------> O(1)
            info.visited = new bool[D + 1];//------> O(1)
            info.hold = new int[D + 1];//------> O(1)
            int num_of_clusters = D - 1, i = 0;//------> O(1)
            for (int j = 0; j < D; j++)
            {
                adjList[j] = new List<int>();
            }
            clustr(adjList, edges, num_of_clusters, K, i);
            info.current = 1;//------> O(1)
            Clusters.Add(new RGBPixel());
            for (int j = 1; j < D; j++)//------> O(D)
            {
                calc(j);
            }
            //===========================================================================================
            for (int h = 0; h < ImageOperations.GetHeight(ImageMatrix); h++)
            {
                // Replaces the pixels of the image matrix
                for (int j = 0; j < ImageOperations.GetWidth(ImageMatrix); j++)
                {
                    int x = 65536;
                    int r = ImageMatrix[h, j].red;
                    int g = ImageMatrix[h, j].green;
                    int b = ImageMatrix[h, j].blue;
                    r = g + b * x / 256 + r * x;
                    ImageMatrix[h, j] = Clusters[info.hold[Quantize.check[r]]];
                }
            }
            //=============================================================================================
        }

        private void MergeSort(List<KeyValuePair<KeyValuePair<int, int>, double>> input, int left, int right)
        {
            if (left < right)//------> O(1)
            {
                int middle = (left + right) / 2;//------> O(1)
                MergeSort(input, left, middle);
                MergeSort(input, middle + 1, right);
                KeyValuePair<KeyValuePair<int, int>, double>[] leftArraytmp = new KeyValuePair<KeyValuePair<int, int>, double>[middle - left + 1];//------> O(1)
                KeyValuePair<KeyValuePair<int, int>, double>[] rightArraytmp = new KeyValuePair<KeyValuePair<int, int>, double>[right - middle];//------> O(1)
                input.CopyTo(left, leftArraytmp, 0, middle - left + 1);
                input.CopyTo(middle + 1, rightArraytmp, 0, right - middle);
                int i = 0;//------> O(1)
                int j = 0;//------> O(1)
                for (int k = left; k < right + 1; k++)//------> O(Right)
                {
                    if (i == leftArraytmp.Length)//------> O(1)
                    {
                        input[k] = rightArraytmp[j];//------> O(1)
                        j++;//------> O(1)+O(1)
                    }
                    else if (j == rightArraytmp.Length)//------> O(1)
                    {
                        input[k] = leftArraytmp[i];//------> O(1)+O(1)
                        i++;//------> O(1)+O(1)
                    }
                    else if (leftArraytmp[i].Value <= rightArraytmp[j].Value)//------> O(1)
                    {
                        input[k] = leftArraytmp[i];//------> O(1)+O(1)
                        i++;//------> O(1)+O(1)
                    }
                    else
                    {
                        input[k] = rightArraytmp[j];//------> O(1)
                        j++;//------> O(1)+O(1)
                    }
                }

            }
        }
    }

    // END OF OUR CODE ELHAMDLALLAH
    //===========================================================================================================================

    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            RGBPixel[,] Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>

        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[2] = ImageMatrix[i, j].red;
                        p[1] = ImageMatrix[i, j].green;
                        p[0] = ImageMatrix[i, j].blue;
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }


        /// <summary>
        /// Apply Gaussian smoothing filter to enhance the edge detection 
        /// </summary>
        /// <param name="ImageMatrix">Colored image matrix</param>
        /// <param name="filterSize">Gaussian mask size</param>
        /// <param name="sigma">Gaussian sigma</param>
        /// <returns>smoothed color image</returns>
        public static RGBPixel[,] GaussianFilter1D(RGBPixel[,] ImageMatrix, int filterSize, double sigma)
        {
            int Height = GetHeight(ImageMatrix);
            int Width = GetWidth(ImageMatrix);
            RGBPixelD[,] VerFiltered = new RGBPixelD[Height, Width];
            RGBPixel[,] Filtered = new RGBPixel[Height, Width];
            // Create Filter in Spatial Domain:
            //=================================
            //make the filter ODD size
            if (filterSize % 2 == 0) filterSize++;
            double[] Filter = new double[filterSize];
            //Compute Filter in Spatial Domain :
            //==================================
            double Sum1 = 0;
            int HalfSize = filterSize / 2;
            for (int y = -HalfSize; y <= HalfSize; y++)
            {
                //Filter[y+HalfSize] = (1.0 / (Math.Sqrt(2 * 22.0/7.0) * Segma)) * Math.Exp(-(double)(y*y) / (double)(2 * Segma * Segma)) ;
                Filter[y + HalfSize] = Math.Exp(-(double)(y * y) / (double)(2 * sigma * sigma));
                Sum1 += Filter[y + HalfSize];
            }
            for (int y = -HalfSize; y <= HalfSize; y++)
            {
                Filter[y + HalfSize] /= Sum1;
            }

            //Filter Original Image Vertically:
            //=================================
            int ii, jj;
            RGBPixelD Sum;
            RGBPixel Item1;
            RGBPixelD Item2;

            for (int j = 0; j < Width; j++)
                for (int i = 0; i < Height; i++)
                {
                    Sum.red = 0;
                    Sum.green = 0;
                    Sum.blue = 0;
                    for (int y = -HalfSize; y <= HalfSize; y++)
                    {
                        ii = i + y;
                        if (ii >= 0 && ii < Height)
                        {
                            Item1 = ImageMatrix[ii, j];
                            Sum.red += Filter[y + HalfSize] * Item1.red;
                            Sum.green += Filter[y + HalfSize] * Item1.green;
                            Sum.blue += Filter[y + HalfSize] * Item1.blue;
                        }
                    }
                    VerFiltered[i, j] = Sum;
                }

            //Filter Resulting Image Horizontally:
            //===================================
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    Sum.red = 0;
                    Sum.green = 0;
                    Sum.blue = 0;
                    for (int x = -HalfSize; x <= HalfSize; x++)
                    {
                        jj = j + x;
                        if (jj >= 0 && jj < Width)
                        {
                            Item2 = VerFiltered[i, jj];
                            Sum.red += Filter[x + HalfSize] * Item2.red;
                            Sum.green += Filter[x + HalfSize] * Item2.green;
                            Sum.blue += Filter[x + HalfSize] * Item2.blue;
                        }
                    }
                    Filtered[i, j].red = (byte)Sum.red;
                    Filtered[i, j].green = (byte)Sum.green;
                    Filtered[i, j].blue = (byte)Sum.blue;
                }

            return Filtered;
        }
        //=========================================================================================================================
        ////  HENAAA EL COODE BT3NAAA

        //public static List<RGBPixel> Distinct = new List<RGBPixel>();
        //public static void check(bool[] check, RGBPixel[,] ImageMatrix, long ID, int i)
        //{
        //    int j = 0;
        //    while (j < GetWidth(ImageMatrix))
        //    {

        //        ID = ImageMatrix[i, j].red + ImageMatrix[i, j].green * 123 + ImageMatrix[i, j].blue * 456 * 456; //------> O(1)

        //        if (check[ID] == false) //------> O(1)
        //        {
        //            Distinct.Add(ImageMatrix[i, j]); //------> O(1)
        //            check[ID] = true; //------> O(1)
        //        }
        //        j++;
        //    }
        //}
        //public static void Find_Distinct(RGBPixel[,] ImageMatrix)
        //{
        //    bool[] c = new bool[100000000];
        //    long num = 0;
        //    for (int i = 0; i < GetHeight(ImageMatrix); i++)
        //    {
        //        check(c, ImageMatrix, num, i);
        //    }
        //}
        //public static void Euclidean(double[,] distances, List<RGBPixel> Distinct, int i, int j)
        //{
        //    distances[i, j] = Math.Sqrt((Distinct[j].red - Distinct[i].red) * (Distinct[j].red - Distinct[i].red) +
        //                                (Distinct[j].blue - Distinct[i].blue) * (Distinct[j].blue - Distinct[i].blue) +
        //                                (Distinct[j].green - Distinct[i].green) * (Distinct[j].green - Distinct[i].green));
        //}
        //public static void CalcDist()
        //{
        //    int D = Distinct.Count;
        //    double[,] distances = new double[D, D];
        //    List<KeyValuePair<int, double>>[] adj = new List<KeyValuePair<int, double>>[D];
        //    for (int i = 0; i < D; i++)
        //    {
        //        adj[i] = new List<KeyValuePair<int, double>>();
        //    }

        //    for (int i = 0; i < D; i++)
        //    {
        //        for (int j = 0; j < D; j++)
        //        {
        //            if (i != j)
        //            {
        //                Euclidean(distances, Distinct, i, j);
        //            }
        //        }
        //    }
        //}






    }
}