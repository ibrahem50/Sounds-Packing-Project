using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace projectalgo
{
    public class Sound
    {
        private string name;
        private double seconds;

        public Sound()
        {
            name = "";
            seconds = 0;
        }

        public Sound(string name, double seconds)
        {
            this.name = name;
            this.seconds = seconds;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public double GetSeconds()
        {
            return seconds;
        }

        public void SetSeconds(double seconds)
        {
            this.seconds = seconds;
        }

    }

    class functions
    {

        //read file
        public void readinformation(ref List<int> l, string path, string filename)
        {
            path += @"\" + filename + ".txt";
            FileStream fs = new FileStream(@path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            int s = Convert.ToInt32(sr.ReadLine());
            //Console.WriteLine(s);
            int i = 0;
            while (sr.Peek() != -1)
            {
                string str = sr.ReadLine();
                string[] fields;
                fields = str.Split(' ');
                int sec;
                DateTime d = Convert.ToDateTime(fields[1]);
                sec = d.Second + (d.Minute * 60) + (d.Hour * 60 * 60);
                //Console.WriteLine(i + " : " + sec);
                l.Add(sec);
                i++;
            }
            sr.Close();
        }

        //Function to read soundsInfo from Input Folder and store them in public sounds list
        int n;
        List<Sound> snd;
        List<Sound> rem;
        public void Read_Sounds_Info(string path, ref List<Sound> sounds)
        {
            string file = path;

            if (!File.Exists(file))
            {
                Console.WriteLine("file not exist");
            }
            else
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                sr.ReadLine();
                while (sr.Peek() != -1)
                {
                    string[] temp = sr.ReadLine().Split(' ');
                    Sound t = new Sound(temp[0], TimeSpan.Parse(temp[1]).TotalSeconds);
                    Console.WriteLine(t.GetSeconds());
                    //view stored sounds 
                    sounds.Add(t);
                }
                sr.Close();

            }
        }
        public void folderfilling(List<Sound> sounds, double size, string oldpath, string newpath)
        {
            int si = Convert.ToInt32(size);
            string path = @newpath + @"\folderfilling";
            Directory.CreateDirectory(path);
            snd = sounds;
            dp = new int[si + 1][];
            memo = new int[si + 1][];

            //to know number of folder
            int cnt = 0;
            while (sounds.Count() != 0)
            {
                for (int i = 0; i < si + 1; i++)
                {
                    dp[i] = new int[sounds.Count()];
                    memo[i] = new int[sounds.Count()];
                    for (int j = 0; j < sounds.Count(); j++) { dp[i][j] = -1; }
                }
                n = sounds.Count;
                rem = new List<Sound>();
                int a = solve(si, 0);
                visited(si, 0);
                copy_to(rem, cnt, oldpath, path);
                //send to write in file
                for (int i = 0; i < rem.Count; i++)
                {
                    sounds.Remove(rem[i]);
                }
                rem = null;
                cnt++;
            }

        }
        void visited(int w, int i)
        {
            if (i == n) return;
            if (memo[w][i] == 1) { visited(w, i + 1); return; }

            if (memo[w][i] == 2)
            {
                rem.Add(snd[i]);
                visited(w - (int)snd[i].GetSeconds(), i + 1);
            }
        }
        int[][] dp;
        int[][] memo;
        int solve(int w, int i)
        {
            if (i == n) return 0;
            if (dp[w][i] != -1) return dp[w][i];
            int opt1 = solve(w, i + 1), opt2 = 0;
            if ((int)snd[i].GetSeconds() <= w) { opt2 = (int)snd[i].GetSeconds() + solve(w - (int)snd[i].GetSeconds(), i + 1); }
            if (opt1 >= opt2) { memo[w][i] = 1; return dp[w][i] = opt1; }
            memo[w][i] = 2;
            return dp[w][i] = opt2;
        }
        //write in file
        public void copy_to(List<Sound> l, int c, string oldpath, string newpath)
        {
            string path = @newpath + @"\f" + c.ToString();
            Directory.CreateDirectory(path);
            for (int i = 0; i < l.Count; i++)
            {
                string sourceFile = Path.Combine(oldpath, l[i].GetName());
                string destFile = Path.Combine(path, l[i].GetName());
                File.Copy(sourceFile, destFile, true);
            }
        }

        //using priorty queue in sound packing
        public void using_priortyqueue(ref List<int> l, int size, string oldpath, string nameofildpath, string newpath)
        {

            string path = @newpath + @"\[2] WorstFit (P-Q)";
            //Console.WriteLine(path);
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
            int number_folder = 0;
            PriorityQueue<string> q = new PriorityQueue<string>();
            //Console.WriteLine(l.Count);
            for (int i = 0; i < l.Count; i++)
            {
                if (q.Count == 0)
                {
                    number_folder++;
                    string str = "0#f" + number_folder.ToString();
                    q.Enqueue(str);
                    string newone = path + @"\f" + number_folder.ToString();
                    Directory.CreateDirectory(newone);
                }
                string[] fields = q.Peek().Split('#');
                int sec = Convert.ToInt32(fields[0]);

                if ((size - sec) < l[i])
                {
                    number_folder++;
                    string newone = path + @"\f" + number_folder.ToString();
                    Directory.CreateDirectory(newone);
                    string newfol = "0#f" + number_folder.ToString();
                    q.Enqueue(newfol);

                }
                string[] field = q.Peek().Split('#');
                q.Dequeue();
                int seco = Convert.ToInt32(fields[0]);
                string fileName = (i + 1).ToString() + ".mp3";
                string sourcePath = @oldpath + @"\" + nameofildpath;
                string targetPath = path + @"\" + field[1];
                string sourceFile = Path.Combine(sourcePath, fileName);
                string destFile = Path.Combine(targetPath, fileName);
                File.Copy(sourceFile, destFile, true);
                int totalsecound = l[i] + seco;
                if (totalsecound < size)
                {
                    string name = (l[i] + seco).ToString() + "#" + field[1];
                    q.Enqueue(name);
                }
            }
            //move from old folder to new one
        }

    }
}
