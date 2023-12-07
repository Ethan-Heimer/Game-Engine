using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Forms;
using GameEngine.Engine;
using GameEngine.Debugging;

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    [Note(note ="Crash when loading scene")]
    public class GameManagerWindow : EditorWindow
    {
        public GameManagerWindow() 
        {
            Height = 300;
        }

        public override void OnGUI(EditorGUIDrawer editorGUI)
        {
            editorGUI.StartHorizontalGroup();
            editorGUI.DrawText("Scene");
            editorGUI.DrawButton("Load Scene", LoadScene);
            editorGUI.DrawButton("Save Scene", SaveScene);
            editorGUI.EndGroup();

            editorGUI.StartHorizontalGroup();
            editorGUI.DrawText("Play Modes");
            editorGUI.DrawButton("Enter Play", Play);
            editorGUI.DrawButton("Exit Play", ExitPlay);
            editorGUI.DrawButton("Pause", EnterPause);
            editorGUI.EndGroup();
        }

        private void LoadScene(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "SCENE (.scene)|*.scene";

            try
            {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Console.WriteLine(openFileDialog.FileName);
                    string name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    SceneManager.LoadScene(name);
                }
            }
            catch (Exception err)
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
            PlayModeManager.SetMode(PlayModeManager.PlayMode.Play);
        }

        private void ExitPlay(object sender, RoutedEventArgs e)
        {
            PlayModeManager.SetMode(PlayModeManager.PlayMode.Edit);
        }
        private void EnterPause(object sender, RoutedEventArgs e)
        {
            PlayModeManager.SetMode(PlayModeManager.PlayMode.Pause);
        }
    }
}
