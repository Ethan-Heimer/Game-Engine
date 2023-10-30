using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public class ChangeLog : EditorWindow
    {
        public ChangeLog() 
        {
            Width = 500; Height = 600;  
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            drawer.DrawLabel("Change Log V d0.02");
            drawer.DrawText("Phoenix Engine Development Milestone 2 introduces an API for editor windows: 'Blocks!'");

            drawer.DrawLabel("What Is Blocks?");
            drawer.DrawText("Blocks is a component based UI API for making Editor windows and tools. To get started, create a class that inherents from 'EditorWindow'. " +
                "All drawing is done with the 'EditorGUIDrawer' class. The class offers base building blocks for all UI, including Labels, Buttons, and Layout tools. \n\n " +
                "Components can be made by inharenting from the 'Component' class, and defining an OnDraw Method with the first paramater being an 'EditorGUIDrawer'." +
                "any paramaters defined afterwords are paramaters used to pass data into the component. \n\n" +
                "To Draw A Component, use the 'Draw' generic method on the 'EditorGUIDrawer', pass the type of component to draw, and pass the paramaters the component needs");

            drawer.DrawButton("Button For Fun!", (e, s) => Console.WriteLine("Hype!!!"));
        }
    }
}
