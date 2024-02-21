using GameEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GameEngine.Game
{
    public class SceneSwitcher : Behavior
    {
        public string SceneName;

        Collider collider;

        public void Start()
        {
            collider = gameObject.GetComponent<Collider>();
        }

        public void Update() 
        {
            if (collider.IsColliding)
                SceneManager.LoadScene(SceneName);
        }
    }
}
