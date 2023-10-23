
using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace GameEngine.Editor.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SceneLoader : Window
    {
        public SceneLoader()
        {
            InitializeComponent();
        }

        private void LoadScene(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new  OpenFileDialog();

            openFileDialog.Filter = "SCENE (.scene)|*.scene";

            try
            {
                if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Console.WriteLine(openFileDialog.FileName);
                    string name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    SceneManager.LoadScene(name);
                }
            }
            catch(Exception err)
            {
                throw new Exception("Scene Failed To Load: " + err);
            }
        }

        private void SaveScene(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "SCENE (.scene)|*.scene";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SceneManager.SaveScene(saveFileDialog.FileName);
            }
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            Editor.EnterPlayMode();
        }

        private void ExitPlay(object sender, RoutedEventArgs e)
        {
            Editor.ExitPlayMode();
        }
    }
}
