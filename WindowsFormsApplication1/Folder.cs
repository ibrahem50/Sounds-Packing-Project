using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Folder
    {
        public List<Sound> FolderSounds;
        private string FolderName;
        private string path;
        public double SecondsCapacity;

       public Folder()
        {
            FolderSounds = new List<Sound>();
            FolderName = "";
            SecondsCapacity = 0;
            path = "";
        }
       public Folder(string FolderName,string path,double SecondsCapacity)
        {
            FolderSounds = new List<Sound>();
            this.FolderName = FolderName;
            this.path = path;
            this.SecondsCapacity = SecondsCapacity;
        }

        public string GetFolderName()
        {
            return FolderName;
        }
        public void SetFolderName(string FolderName)
        {
            this.FolderName = FolderName;
        }

        public string GetPath()
        {
            return path;
        }

        public void SetPath(string path)
        {
            this.path = path;
        }

        public void writing(string InputFolder,double MaxSeconds)
        {
            Directory.CreateDirectory(path + @"\" + FolderName);
            string MetaData = FolderName + "_METADATA.txt";
            FileStream fs = new FileStream(path+MetaData, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(FolderName);
            for(int i=0;i<FolderSounds.Count;i++)
            {
                string temp=FolderSounds[i].GetName()+" "+(TimeSpan.FromSeconds(FolderSounds[i].GetSeconds()).Duration().ToString());
                sw.WriteLine(temp);
                sw.Flush();
            }

            sw.WriteLine(TimeSpan.FromSeconds(MaxSeconds - SecondsCapacity).Duration().ToString());
            sw.Close();
            fs.Close();

           for (int i = 0; i < FolderSounds.Count; i++)
            {

                File.Copy(InputFolder + @"\Audios" + @"\" + FolderSounds[i].GetName(), path + FolderName + @"\"+FolderSounds[i].GetName(),true);
            }            
            

        }

    }
}
