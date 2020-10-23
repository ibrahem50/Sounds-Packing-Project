using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Sound
    {
        private string name;
        private double seconds;

        public Sound()
        {
            name="";
            seconds = 0;
        }

        public Sound(string name,double seconds)
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
            this.seconds=seconds;
        }

    }
}
