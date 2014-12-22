using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics; //Stopwatch
namespace LZWCoding_Picture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //讀取檔案位址(need OpenFileDialog)
            openFileDialog1.Filter = "影像檔(*.jpg,*.jpge,*.bmp,*.gif,*.ico,*.png,*.tif,*.wmf)|*.jpg;*.jpge;*.bmp;*.gif;*.ico;*.png;*.tif;*.wmf";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
            //textBox.Text.Substring(textBox1.Text.LastIndexOf(@"\") + 1, textBox1.Text.Count() - textBox1.Text.LastIndexOf(@"\") - 1)
            try
            {
                Image picture = Image.FromFile(textBox1.Text);
                pictureBox1.Image = picture;
                textBox4.Text = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf(@"\"));
            }
            catch (Exception ex) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox4.Text.Equals(""))
            {
                MessageBox.Show("尚有位置沒有填寫");
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Image pictureEncode = Image.FromFile(textBox1.Text);
                MemoryStream pictureStream = new MemoryStream();
                pictureEncode.Save(pictureStream, pictureEncode.RawFormat);
                Byte[] pictureByte = pictureStream.ToArray();
                pictureStream.Close();
                List<string> pictureList = pictureByte.Select(c => Convert.ToString(c) + ",").ToList();
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                string outputColor = string.Empty;
                List<string> pictureListDecode = new List<string>();
                LZWEncoder.LZW_Encode encoder = new LZWEncoder.LZW_Encode(pictureList, textBox4.Text);
                dictionary = encoder.getDictionary;
                outputColor = encoder.getOutput;
                textBox3.Text = encoder.getOutput;
                using (StreamWriter sw1 = new StreamWriter(textBox4.Text + @"\Output.txt"))
                {
                    sw1.Write(encoder.getOutput);
                }
                LZWDecoder.LZW_Decode decoder = new LZWDecoder.LZW_Decode(dictionary, outputColor, textBox4.Text);
                pictureListDecode = decoder.getInput.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                MemoryStream pictureStreamDecode = new MemoryStream(pictureListDecode.Select(c => Convert.ToByte(c)).ToArray());
                Image pictureDecode = Image.FromStream(pictureStreamDecode);
                pictureDecode.Save(textBox4.Text + @"\" + textBox1.Text.Substring(textBox1.Text.LastIndexOf(@"\") + 1, textBox1.Text.LastIndexOf(@".") - textBox1.Text.LastIndexOf(@"\") - 1) + "_decode" + ".png", ImageFormat.Png);
                pictureBox2.Image = pictureDecode;
                FileInfo f1 = new FileInfo(textBox4.Text + @"\Output.txt");
                FileInfo f2 = new FileInfo(textBox4.Text + @"\" + textBox1.Text.Substring(textBox1.Text.LastIndexOf(@"\") + 1, textBox1.Text.LastIndexOf(@".") - textBox1.Text.LastIndexOf(@"\") - 1) + "_decode" + ".png");
                textBox7.Text = f2.Length.ToString();
                textBox8.Text = f1.Length.ToString();
                double CompressionRate = (double)f2.Length / (double)f1.Length;
                textBox6.Text = CompressionRate.ToString();
                sw.Stop();
                MessageBox.Show((Convert.ToDecimal(sw.ElapsedMilliseconds) / 1000).ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //讀取資料夾位址(need FolderBrowserDialog)
            folderBrowserDialog1.SelectedPath = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox4.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
