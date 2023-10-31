using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public class TestButtonComponent : Component
    {
        public void OnDraw(EditorGUIDrawer editorGui, string ButtonOneName)
        {
            editorGui.DrawText("Text Buttons");
            editorGui.DrawButton(ButtonOneName, (e, s) => Console.WriteLine("Test Works!!"));
            editorGui.DrawButton("Test", (e, s) => Console.WriteLine("Test Works!!"));
        }
    }
}
