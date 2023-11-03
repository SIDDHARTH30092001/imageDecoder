using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Decoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //BROWSE BUTTON CLICK EVENT
        private void browse_Click(object sender, EventArgs e)
        {
            //OPEN FILE MANAGER AND GET IMAGE FILE
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;...";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {

                if (openFileDialog.FileName.Contains("enc_"))
                {
                    String full = Path.GetFileName(openFileDialog.FileName);
                    String replaced = full.Replace("enc_", "");
                    String imgname = replaced;
                    pictureBox1.Image = new Bitmap("D:/Encrypt/" + imgname);
                    textBox1.Text = imgname;
                    coder.Text = "Decrypt";
                }
                else if (openFileDialog.FileName.Contains("dec_"))
                {
                    String full = Path.GetFileName(openFileDialog.FileName);
                    String replaced = full.Replace("dec_", "");
                    String imgname = replaced;
                    pictureBox1.Image = new Bitmap("D:/Encrypt/" + imgname);
                    textBox1.Text = imgname;
                    coder.Text = "Encrypt";
                }
                else
                {
                    textBox1.Text = openFileDialog.FileName;
                    coder.Text = "Encrypt";
                }


                File.WriteAllText("D:/Encrypt/ImagePath.txt", String.Empty);
                FileStream fs1 = new FileStream("D:/Encrypt/ImagePath.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs1);
                writer.Write(textBox1.Text);
                writer.Close();

                String filename = Path.GetFileNameWithoutExtension(textBox1.Text);
                File.WriteAllText("D:/Encrypt/ImageName.txt", String.Empty);
                FileStream fs = new FileStream("D:/Encrypt/ImageName.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer1 = new StreamWriter(fs);
                writer1.Write(filename);
                writer1.Close();

                String fl = Path.GetFileName(textBox1.Text);
                File.WriteAllText("D:/Encrypt/ImageExt.txt", String.Empty);
                FileStream fs0 = new FileStream("D:/Encrypt/ImageExt.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer0 = new StreamWriter(fs0);
                writer0.Write(fl);
                writer0.Close();

                backgroundWorker2.RunWorkerAsync(2000);
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
               
            }
        }


        //ENCRYPTION BUTTON CLICK EVENT
        private void coder_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "Please Select Image!" || textBox1.Text == "Decryption File Does Not Exists!" || textBox1.Text == "Encryption File Does Not Exists!" || textBox1.Text.Contains("key_"))
            {
                textBox1.Text = "Please Select Image!";
            }
            else if (textBox1.Text.Contains("key_"))
            {
                textBox1.Text = "Please Select Image!";
            }
            else if (textBox1.Text.Contains("dec_"))
            {
                
                String lines = System.IO.File.ReadAllText("D:/Encrypt/ImageExt.txt");
                String replaced = lines.Replace("dec_", "");
                String imgname = "enc_" + replaced;
                if (File.Exists("D:/Encrypt/" + imgname))
                {
                    pictureBox1.Image = new Bitmap("D:/Encrypt/" + imgname);
                    textBox1.Text = imgname;
                    coder.Text = "Decrypt";
                }
                else
                {
                    textBox1.Text = "Encryption File Does Not Exists!";
                }
            }
            else if (textBox1.Text.Contains("enc_"))
            {
                
                String lines = System.IO.File.ReadAllText("D:/Encrypt/ImageExt.txt");
                String replaced = lines.Replace("enc_", "");
                String imgname = "dec_" + replaced;
                if (File.Exists("D:/Encrypt/" + imgname))
                {
                    pictureBox1.Image = new Bitmap("D:/Encrypt/" + imgname);
                    textBox1.Text = imgname;
                    coder.Text = "Encrypt";
                }
                else
                {
                    textBox1.Text = "Decryption File Does Not Exists!";
                }
            }
            else
            {
                textBox1.Text = "";
                coder.Text = "Decrypt";
                backgroundWorker1.RunWorkerAsync(2000);
                
            }
         
        }


        //BACKGROUND WORKER 1 FOR IMAGE ENCRYPTION
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:/Users/siddh/AppData/Local/Programs/Python/Python39/python.exe";
            start.Arguments = string.Format("{0}", "D:/Encrypt/enc.py");
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!textBox1.Text.Contains("enc_") || !textBox1.Text.Contains("dec_"))
            {
                String rep = System.IO.File.ReadAllText("D:/Encrypt/ImageExt.txt");
                try
                {
                    String imgname = "enc_" + rep;
                    pictureBox1.Image = new Bitmap("D:/Encrypt/" + imgname);
                    textBox1.Text = imgname;
                    backgroundWorker3.RunWorkerAsync(5000);
                }
                catch
                {
                    String imgname = "dec_" + rep;
                    pictureBox1.Image = new Bitmap("D:/Encrypt/" + imgname);
                    textBox1.Text = imgname;
                    backgroundWorker3.RunWorkerAsync(5000);
                }

            }
            

        }


        //BACKGROUND WORKER 2 FOR HASH GENERATION
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:/Users/siddh/AppData/Local/Programs/Python/Python39/python.exe";
            start.Arguments = string.Format("{0}", "D:/Encrypt/hash.py");
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            String md5 = System.IO.File.ReadAllText("D:/Encrypt/MD5.txt");
            String sha1 = System.IO.File.ReadAllText("D:/Encrypt/SHA1.txt");
            textBox2.Text = md5;
            textBox3.Text = sha1;
            
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:/Users/siddh/AppData/Local/Programs/Python/Python39/python.exe";
            start.Arguments = string.Format("{0}", "D:/Encrypt/grph.py");
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox3.Image = new Bitmap("D:/Encrypt/origin.png");
            pictureBox4.Image = new Bitmap("D:/Encrypt/encrpt.png");
            //String reps = System.IO.File.ReadAllText("D:/Encrypt/tx.txt");
            //textBox4.Text = reps;
        }

    }
}

/*
 * image = "enc_"+name
ImageMatrix,image_size = getImageMatrix_gray(image)
samples_x = []
samples_y = []
for i in range(1024):
  x = random.randint(0,image_size-2)
  y = random.randint(0,image_size-1)
  samples_x.append(ImageMatrix[x][y])
  samples_y.append(ImageMatrix[x+1][y])
plt.figure(figsize=(10,8))
plt.scatter(samples_x,samples_y,s=2)
plt.title('Adjacent Pixel Autocorrelation - Encrypted Image', fontsize=20)
plt.savefig('encrpt.png',bbox_inches='tight')
#plt.show()


 */
