using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        static RGBPixel[,] ImageMatrix;
        Quantize Quantization;
        List<RGBPixel> colors;
        List<KeyValuePair<KeyValuePair<int, int>, double>> edges;

        public void Run_Quatize()
        {
            Quantization = new Quantize(ImageMatrix);
            //Get The Distincit Colors
            colors = Quantization.Find_Distinct();
            //List To Save The Edges The Resulting Minimum Spanning Tree
            edges = new List<KeyValuePair<KeyValuePair<int, int>, double>>();
            //The Total Sum Of The MST
            double sum = Quantization.Build_Mst(ref edges, colors.Count, colors);
            //Display The # Distinct Colors In The Width's Text Box
            txtWidth.Text = (colors.Count - 1).ToString();
            //Display The Sum Of The MST In The Height's Text Box
            txtHeight.Text = sum.ToString();

        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            double before = System.Environment.TickCount;
            Run_Quatize();
            double after = System.Environment.TickCount;
            double result = after - before;
            msec.Text = result.ToString() + " M-Sec";
            result /= 1000;
            sec.Text = result.ToString() + " Sec";
            mst_sum.Text = Quantize.MST_var.tree_Cost.ToString("#.##");
            N_Dcolors.Text = (edges.Count + 1).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            int K = int.Parse(ClusterNumber.Text);
            Cluster c = new Cluster(colors, ImageMatrix);
            c.Identify_Clusters(edges, colors.Count, K);
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image image = pictureBox2.Image;
            SaveImageCapture(image);
        }

        public static void SaveImageCapture(System.Drawing.Image image)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image";
            s.DefaultExt = ".Jpg";
            s.Filter = "Image (.jpg)|*.jpg";
            s.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            s.RestoreDirectory = true;
            if (s.ShowDialog() == DialogResult.OK)
            {
                string filename = s.FileName;
                using (System.IO.FileStream fstream = new System.IO.FileStream(filename, System.IO.FileMode.Create))
                {
                    image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fstream.Close();
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Steps : \n" +
                "===== \n\n\n"+
                "1-) Enter number of Clusters. \n\n"+
                "2-) Click on (Open Image) Button And choose the image you want. \n\n"+
                "3-) Click on (Gauss Smooth) Button.\n\n","Welcome Page"
                );
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click_1(object sender, EventArgs e)
        {

        }
    }
}