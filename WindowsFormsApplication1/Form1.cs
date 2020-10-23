using projectalgo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        List<Sound> sounds = new List<Sound>();
        int SoundsCount = 0;
        string InputFolder;
        string OutputFolder;
        double MaxFolerSeconds;
        public Form1()
        {

            InitializeComponent();
        }

        //MergeSort in descending order
        private static void MergeSortSounds(List<Sound> temp, int low, int high)
        {
            if (low == high)
                return;
            int mid = (low + high) / 2;
            MergeSortSounds(temp, low, mid);
            MergeSortSounds(temp, mid + 1, high);

            MergeSounds(temp, low, mid, high);
        }

        //MergeSort Algorithm Descending order (recuresion)
        private static void MergeSounds(List<Sound> s, int low, int mid, int high)
        {

            List<Sound> temp = new List<Sound>();

            // 'i' tracks the index for the head of low half of the range.
            // 'j' tracks the index for the head of upper half of the range.
            int i = low, j = mid + 1, n = 0;

            // While we still have a entry in one of the halves.
            while ((i <= mid || j <= high))
            {
                // Lower half is exhausted.  Just copy from the upper half.
                if (i > mid)
                {
                    temp.Add(s[j]);
                    j++;
                }
                // Upper half is exhausted. Just copy from the lower half.
                else if (j > high)
                {
                    temp.Add(s[i]);
                    i++;
                }
                // Compare the two Sounds objects at the head of the lower and upper halves.
                // If lower is less than upper copy from lower.
                else if (s[i].GetSeconds() > s[j].GetSeconds())
                {
                    temp.Add(s[i]);
                    i++;
                }
                // Lower is is greater than upper.  Copy from upper.
                else
                {
                    temp.Add(s[j]);
                    j++;
                }
                n++;
            }

            // Copy from the temp buffer back into the 'sounds' list.
            for (int k = low; k <= high; k++)
            {
                s[k] = temp[k - low];
            }
        }


        //Choosing Input Folder Button
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                InputFolder = folderBrowserDialog1.SelectedPath;
            }
            Read_Sounds_Info();

        }


        //Function to read soundsInfo from Input Folder and store them in public sounds list
        public void Read_Sounds_Info()
        {
            sounds.Clear();
            string file = InputFolder + @"\AudiosInfo.txt";

            if (!File.Exists(file))
            {
                MessageBox.Show("You Selected Wrong Folder\nOr No Folder Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                SoundsCount = int.Parse(sr.ReadLine());

                while (sr.Peek() != -1)
                {
                    string[] temp = sr.ReadLine().Split(' ');
                    Sound t = new Sound(temp[0], TimeSpan.Parse(temp[1]).TotalSeconds);
                    //view stored sounds
                    //MessageBox.Show(t.GetName() + " -- " + t.GetSeconds().ToString());
                    sounds.Add(t);

                }

                sr.Close();
                fs.Close();
            }


        }

        //Choosing OutputFolder path Button
        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog2.SelectedPath == null)
                {
                    MessageBox.Show("No Folder Selected\nPlease Choose Output Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    textBox2.Text = folderBrowserDialog2.SelectedPath;
                    OutputFolder = folderBrowserDialog2.SelectedPath;
                    OutputFolder += @"\OUTPUT";
                    if (Directory.Exists(OutputFolder))
                    {
                        Directory.Delete(OutputFolder, true);

                    }
                    Directory.CreateDirectory(OutputFolder);
                }
            }
        }


        //FirstFit Decreasing Algorithm Using Linear Search button
        private void button3_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {
                List<Sound> FirstFitDec = new List<Sound>(sounds); //O(1) 
                MergeSortSounds(FirstFitDec, 0, FirstFitDec.Count - 1); //mergesort O(nlogn)
                //Displaying Sounds after sorting them in descending order
                /*for (int i = 0; i < FirstFitDec.Count; i++)
                {
                    MessageBox.Show(FirstFitDec[i].GetName() + " -- " + FirstFitDec[i].GetSeconds().ToString());
                }
                */
                string firstDir = OutputFolder + @"\[5] Firstfit Decreasing (L-S)"; //O(1)
                if (Directory.Exists(firstDir)) //O(1)
                {
                    Directory.Delete(firstDir, true); //O(1*1)
                }
                timer1.Start();

                Directory.CreateDirectory(firstDir); //O(1)
                List<Folder> folders = new List<Folder>(); //O(1)
                Folder f = new Folder("F1", firstDir + @"\", MaxFolerSeconds); //O(1)
                folders.Add(f); //O(1)
                folders[0].FolderSounds.Add(FirstFitDec[0]); //O(1)
                folders[0].SecondsCapacity = folders[0].SecondsCapacity - FirstFitDec[0].GetSeconds();//O(1)

                for (int i = 1; i < FirstFitDec.Count; i++) //O(n)*(entire)=O(n*m) n: number of sounds, m=number of	folders
                {
                    bool found = false; //O(1*n)
                    int j;
                    for (j = 0; j < folders.Count; j++) //O(m*n*1)
                    {
                        if (FirstFitDec[i].GetSeconds() <= folders[j].SecondsCapacity) //O(1*m*n)
                        {
                            folders[j].FolderSounds.Add(FirstFitDec[i]);//O(1*1*m*n)
                            folders[j].SecondsCapacity = folders[j].SecondsCapacity - FirstFitDec[i].GetSeconds();//O(1*1*m*n)
                            found = true;//O(1*1*m*n)
                            break;//O(1*1*m*n)
                        }
                    }
                    if (found == false) //O(1*n)
                    {
                        Folder s = new Folder("F" + (folders.Count + 1).ToString(), firstDir + @"\", MaxFolerSeconds);//O(1*1*n)
                        folders.Add(s);// O(1 * 1 * n)
                        folders[folders.Count - 1].FolderSounds.Add(FirstFitDec[i]);// O(1 * 1 * n)
                        folders[folders.Count - 1].SecondsCapacity = folders[folders.Count - 1].SecondsCapacity - FirstFitDec[i].GetSeconds();// O(1 * 1 * n)
                    }
                    else
                    {
                        continue;// O(1*1 * n)
                    }
                }

                //Display the folders names and the count of stored seconds after filling them
                for (int i = 0; i < folders.Count; i++) //O(n) * (entire) = O(n * m)
                {
                    // MessageBox.Show(folders[i].GetFolderName() + " -- " + TimeSpan.FromSeconds(MaxFolerSeconds - folders[i].SecondsCapacity).Duration().ToString());
                    folders[i].writing(InputFolder, MaxFolerSeconds); //this function is O(m) * O(n), = number of sounds in the folder
                }

                timer1.Stop();
                label6.Text = TimeSpan.FromSeconds(time).Duration().ToString();
                time = 0;
                MessageBox.Show("FirstFit Decreasing Algorithm\nUsing Linear Serch\nIs Done", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            MaxFolerSeconds = double.Parse(textBox3.Text);
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        //WorstFit Algorihm Using Linear Search Button 
        private void button4_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {


                List<Sound> WorstFit = new List<Sound>(sounds);
                //Display WorstFit sounds list before filling the folders
                /* for (int i = 0; i < WorstFit.Count; i++)
                 {
                     MessageBox.Show(WorstFit[i].GetName() + " -- " + WorstFit[i].GetSeconds().ToString());
                 }
                 */

                string WorstFitDir = OutputFolder + @"\[1] WorstFit (L-S)";

                if (Directory.Exists(WorstFitDir))
                {
                    Directory.Delete(WorstFitDir, true);
                }

                timer1.Start();
                Directory.CreateDirectory(WorstFitDir);
                List<Folder> folders = new List<Folder>();
                Folder f = new Folder("F1", WorstFitDir + @"\", MaxFolerSeconds);
                folders.Add(f);
                folders[0].FolderSounds.Add(WorstFit[0]);
                folders[0].SecondsCapacity = folders[0].SecondsCapacity - WorstFit[0].GetSeconds();

                for (int i = 1; i < WorstFit.Count; i++)
                {

                    int index = -1;
                    double c = 0;
                    for (int j = 0; j < folders.Count; j++)
                    {
                        if (WorstFit[i].GetSeconds() <= folders[j].SecondsCapacity && folders[j].SecondsCapacity >= c)
                        {
                            index = j;
                            c = folders[j].SecondsCapacity;
                        }
                    }
                    if (index > -1)
                    {

                        folders[index].FolderSounds.Add(WorstFit[i]);
                        folders[index].SecondsCapacity = folders[index].SecondsCapacity - WorstFit[i].GetSeconds();

                    }

                    else
                    {
                        Folder s = new Folder("F" + (folders.Count + 1).ToString(), WorstFitDir + @"\", MaxFolerSeconds);
                        folders.Add(s);
                        folders[folders.Count - 1].FolderSounds.Add(WorstFit[i]);
                        folders[folders.Count - 1].SecondsCapacity = folders[folders.Count - 1].SecondsCapacity - WorstFit[i].GetSeconds();

                    }
                }

                //Display the folders names and the count of stored seconds after filling them
                for (int i = 0; i < folders.Count; i++)
                {
                    folders[i].writing(InputFolder, MaxFolerSeconds);
                    // MessageBox.Show(folders[i].GetFolderName() + " -- " + TimeSpan.FromSeconds(MaxFolerSeconds - folders[i].SecondsCapacity).Duration().ToString());

                }

                timer1.Stop();
                label7.Text = TimeSpan.FromSeconds(time).Duration().ToString();
                time = 0;
                MessageBox.Show("WorstFit Algorithm\nUsing Linear Serch\nIs Done", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        //WorstFit Decreasing Algorithm Using Linear Search Button
        private void button5_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {


                List<Sound> WorstFitDec = new List<Sound>(sounds); //O(1)

                MergeSortSounds(WorstFitDec, 0, WorstFitDec.Count - 1); //mergesort O(nlogn)
                //Displaying Sounds after sorting them in descending order                for (int i = 0; i < WorstFitDec.Count; i++)
                /* for (int i = 0; i < WorstFitDec.Count; i++)
                 {
                     MessageBox.Show(WorstFitDec[i].GetName() + " -- " + WorstFitDec[i].GetSeconds().ToString());
                 }
                 */

                string WorstFitDecDir = OutputFolder + @"\[3] WorstFit Decreasing (L-S)";//O(1)

                if (Directory.Exists(WorstFitDecDir))//O(1)
                {
                    Directory.Delete(WorstFitDecDir, true); //O(1)
                }

                timer1.Start(); 
                Directory.CreateDirectory(WorstFitDecDir); //O(1)
                List<Folder> folders = new List<Folder>(); //O(1)
                Folder f = new Folder("F1", WorstFitDecDir + @"\", MaxFolerSeconds); //O(1)
                folders.Add(f); //O(1)
                folders[0].FolderSounds.Add(WorstFitDec[0]); //O(1)
                folders[0].SecondsCapacity = folders[0].SecondsCapacity - WorstFitDec[0].GetSeconds(); //O(1)

                for (int i = 1; i < WorstFitDec.Count; i++) //O(n)*(entire)=O(n*m) n: number of sounds, m=number of folders
                {

                    int index = -1; //O(1*n)
                    double c = 0;   //O(1*n)
                    for (int j = 0; j < folders.Count; j++) //O(m*n)
                    {
                        if (WorstFitDec[i].GetSeconds() <= folders[j].SecondsCapacity && folders[j].SecondsCapacity >= c) //O(1*m*n)
                        {
                            index = j;//O(1*1*1m*n)
                            c = folders[j].SecondsCapacity; //O(1*1*m*n)
                        }
                    }
                    if (index > -1)//O(1*n)
                    {

                        folders[index].FolderSounds.Add(WorstFitDec[i]); //O(1*1*n)
                        folders[index].SecondsCapacity = folders[index].SecondsCapacity - WorstFitDec[i].GetSeconds();//O(1*1*n)

                    }

                    else
                    {
                        Folder s = new Folder("F" + (folders.Count + 1).ToString(), WorstFitDecDir + @"\", MaxFolerSeconds);//O(1*n)
                        folders.Add(s);//O(1*n)
                        folders[folders.Count - 1].FolderSounds.Add(WorstFitDec[i]);//O(1*n)
                        folders[folders.Count - 1].SecondsCapacity = folders[folders.Count - 1].SecondsCapacity - WorstFitDec[i].GetSeconds();//O(1*n)

                    }
                }

                //Display the folders names and the count of stored seconds after filling them
                for (int i = 0; i < folders.Count; i++)//O(n)*(entire)=O(n*m)
                {
                    folders[i].writing(InputFolder, MaxFolerSeconds);//this function is O(m)*O(n), =number of sounds in the folder
                    // MessageBox.Show(folders[i].GetFolderName() + " -- " + TimeSpan.FromSeconds(MaxFolerSeconds - folders[i].SecondsCapacity).Duration().ToString());

                }

                timer1.Stop();
                label9.Text = TimeSpan.FromSeconds(time).Duration().ToString();
                time = 0;
                MessageBox.Show("WorstFit Decreasing Algorithm\nUsing Linear Search\nIs Done", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


            }

        }

        //WorstFit Decreasing Algorithm Using Priority Queue Button
        private void button6_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {
                functions f = new functions();
                string pathfortxt = InputFolder;
                string filename = "AudiosInfo";
                List<int> l = new List<int>();
                f.readinformation(ref l, pathfortxt, filename);
                string oldpath = InputFolder;
                string name = "Audios";
                string newpath = OutputFolder;
                timer1.Start();
                f.using_priortyqueue(ref l, 100, oldpath, name, newpath);
                timer1.Stop();
                label8.Text = TimeSpan.FromSeconds(time).Duration().ToString();
                time = 0;
                MessageBox.Show("WorstFit Algorithm\nUsing Priority Queue\nIs Done", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        //BestFit Algorithm Using Linear Search Button
        private void button7_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {


                List<Sound> BestFit = new List<Sound>(sounds);
                //Display WorstFit sounds list before filling the folders
                /* for (int i = 0; i < BestFit.Count; i++)
                 {
                     MessageBox.Show(BestFit[i].GetName() + " -- " + BestFit[i].GetSeconds().ToString());
                 }
                 */
                string BestFitDir = OutputFolder + @"\[7] BesttFit (L-S)";

                if (Directory.Exists(BestFitDir))
                {
                    Directory.Delete(BestFitDir, true);
                }

                timer1.Start();
                Directory.CreateDirectory(BestFitDir);
                List<Folder> folders = new List<Folder>();
                Folder f = new Folder("F1", BestFitDir + @"\", MaxFolerSeconds);
                folders.Add(f);
                folders[0].FolderSounds.Add(BestFit[0]);
                folders[0].SecondsCapacity = folders[0].SecondsCapacity - BestFit[0].GetSeconds();

                for (int i = 1; i < BestFit.Count; i++)
                {

                    int index = -1;
                    double c = 1000000;
                    for (int j = 0; j < folders.Count; j++)
                    {
                        if (BestFit[i].GetSeconds() <= folders[j].SecondsCapacity && folders[j].SecondsCapacity <= c)
                        {
                            index = j;
                            c = folders[j].SecondsCapacity;
                        }
                    }
                    if (index > -1)
                    {

                        folders[index].FolderSounds.Add(BestFit[i]);
                        folders[index].SecondsCapacity = folders[index].SecondsCapacity - BestFit[i].GetSeconds();

                    }

                    else
                    {
                        Folder s = new Folder("F" + (folders.Count + 1).ToString(), BestFitDir + @"\", MaxFolerSeconds);
                        folders.Add(s);
                        folders[folders.Count - 1].FolderSounds.Add(BestFit[i]);
                        folders[folders.Count - 1].SecondsCapacity = folders[folders.Count - 1].SecondsCapacity - BestFit[i].GetSeconds();

                    }
                }

                //Display the folders names and the count of stored seconds after filling them
                for (int i = 0; i < folders.Count; i++)
                {
                    folders[i].writing(InputFolder, MaxFolerSeconds);
                    //MessageBox.Show(folders[i].GetFolderName() + " -- " + TimeSpan.FromSeconds(MaxFolerSeconds - folders[i].SecondsCapacity).Duration().ToString());

                }

                timer1.Stop();
                label10.Text = TimeSpan.FromSeconds(time).Duration().ToString();
                time = 0;
                MessageBox.Show("BestFit Algorithm\nUsing Linear Search\nIs Done", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        //BestFit Decreasing Algorithm Using Linear Search Button
        private void button8_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {


                List<Sound> BestFitDec = new List<Sound>(sounds);
                MergeSortSounds(BestFitDec, 0, BestFitDec.Count - 1);
                //Displaying Sounds after sorting them in descending order
                /*for (int i = 0; i < BestFitDec.Count; i++)
                {
                    MessageBox.Show(BestFitDec[i].GetName() + " -- " + BestFitDec[i].GetSeconds().ToString());
                }
                */
                string BestFitDecDir = OutputFolder + @"\[8] BesttFit Decreasing (L-S)";

                if (Directory.Exists(BestFitDecDir))
                {
                    Directory.Delete(BestFitDecDir, true);
                }

                timer1.Start();
                Directory.CreateDirectory(BestFitDecDir);
                List<Folder> folders = new List<Folder>();
                Folder f = new Folder("F1", BestFitDecDir + @"\", MaxFolerSeconds);
                folders.Add(f);
                folders[0].FolderSounds.Add(BestFitDec[0]);
                folders[0].SecondsCapacity = folders[0].SecondsCapacity - BestFitDec[0].GetSeconds();

                for (int i = 1; i < BestFitDec.Count; i++)
                {

                    int index = -1;
                    double c = 1000000;
                    for (int j = 0; j < folders.Count; j++)
                    {
                        if (BestFitDec[i].GetSeconds() <= folders[j].SecondsCapacity && folders[j].SecondsCapacity <= c)
                        {
                            index = j;
                            c = folders[j].SecondsCapacity;
                        }
                    }
                    if (index > -1)
                    {

                        folders[index].FolderSounds.Add(BestFitDec[i]);
                        folders[index].SecondsCapacity = folders[index].SecondsCapacity - BestFitDec[i].GetSeconds();

                    }

                    else
                    {
                        Folder s = new Folder("F" + (folders.Count + 1).ToString(), BestFitDecDir + @"\", MaxFolerSeconds);
                        folders.Add(s);
                        folders[folders.Count - 1].FolderSounds.Add(BestFitDec[i]);
                        folders[folders.Count - 1].SecondsCapacity = folders[folders.Count - 1].SecondsCapacity - BestFitDec[i].GetSeconds();

                    }
                }

                //Display the folders names and the count of stored seconds after filling them
                for (int i = 0; i < folders.Count; i++)
                {
                    folders[i].writing(InputFolder, MaxFolerSeconds);
                    //MessageBox.Show(folders[i].GetFolderName() + " -- " + TimeSpan.FromSeconds(MaxFolerSeconds - folders[i].SecondsCapacity).Duration().ToString());

                }

                timer1.Stop();
                label13.Text = TimeSpan.FromSeconds(time).Duration().ToString();
                time = 0;
                MessageBox.Show("BestFit Decreasing Algorithm\nUsing Linear Search\nIs Done", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        double time = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
        }


        //Folder Filling Algorithm Button
        private void button9_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {

            }

        }

        // Worst Fit Decreasing Using Priority Queue Button
        private void button10_Click(object sender, EventArgs e)
        {
            if (InputFolder == null)
            {
                button1.Focus();
                errorProvider1.SetError(textBox1, "Please Choose Input Path");
            }
            else if (OutputFolder == null)
            {
                button2.Focus();
                errorProvider1.SetError(textBox2, "Please Choose Input Path");
            }
            else if (textBox3 == null)
            {
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Please Enter Folder Capacity");
            }
            else
            {

            }
        }
    }

}
