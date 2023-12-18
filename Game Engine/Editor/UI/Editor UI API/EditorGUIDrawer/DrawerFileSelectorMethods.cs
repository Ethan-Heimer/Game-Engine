using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GameEngine.Editor.Windows
{
    public partial class EditorGUIDrawer
    {
        public Button FileSelector(string label, string fileType, Action<string> onFileRecieved)
        {
            return FileSelector(label, fileType, onFileRecieved, ElementStyle.DefaultButtonStyle);
        }

        public Button FileSelector(string label, string fileType, Action<string> onFileRecieved, ElementStyle style)
        {
            return FileSelector(label, fileType, onFileRecieved, "", style);
        }

        public Button FileSelector(string label, string fileType, Action<string> onFileRecieved, string tag, ElementStyle style)
        {
            var button = DrawButton(label, (s, e) =>
            {
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.Filter = $"{fileType.ToUpper()} (.{fileType.ToLower()})|*.{fileType.ToLower()}";

                try
                {
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        onFileRecieved(openFileDialog.FileName);
                        string name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    }
                }
                catch (Exception err)
                {
                    throw new Exception("File Failed To Open: " + err);
                }

            }, tag, style);

            return button;
        }
    }
}