using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GameEngine.Engine
{
    [Serializable]
    public class Icon
    {
        string path;
        public Icon(string path)
        {
            this.path = path;
        } 

        public string GetPath() => path;
        public BitmapImage GetImage() => new BitmapImage(new Uri(path, UriKind.Relative));
    }
}
