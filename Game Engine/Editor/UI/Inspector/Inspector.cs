using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Editor.UI.Inspector
{
    public class Inspector
    {
        List<IFieldTemplate> fields = new List<IFieldTemplate>(); 

        InspectorUIFactory factory = new InspectorUIFactory();
        DataBinderFactory binderFactory = new DataBinderFactory();

        public Inspector(GameObject gameObject, EditorGUIDrawer drawer) 
        {

            foreach (GameEngine.Component o in gameObject.GetAllComponents())
            {

                drawer.DrawText(o.BindingBehavior.GetType().Name, new ElementStyle()
                {
                    FontSize = ElementStyle.MediumTextSize,
                    Margin = new System.Windows.Thickness(5)
                }) ;

                //DrawNameField(gameObject, drawer);
            
                foreach (FieldInfo f in o.GetFields())
                {
                    Type templateType = factory.TryGetTemplate(f);
                    Type binderType = binderFactory.TryGetBinder(f);
                    
                    Console.WriteLine(templateType == null);

                    if (templateType == null)
                        continue;

                    IFieldTemplate template = (IFieldTemplate)Activator.CreateInstance(templateType, new object[] {binderType, f, o.BindingBehavior });
                    template.Display(drawer);

                    fields.Add(template);
                    /*
                    if (f)
                    {
                        drawer.Draw(component, binderType, f, o.BindingBehavior);
                    }
                    Console.WriteLine(component + " Component");
                    */
                }
            }
        }

        public void Update(EditorGUIDrawer drawer)
        {
            fields.ForEach(x =>
            {
                x.Update(drawer);
            });
        }

        void DrawNameField(GameObject gameObject, EditorGUIDrawer drawer)
        {
            FieldInfo name = gameObject.GetType().GetField("Name");


            Type templateType = factory.TryGetTemplate(name);
            Type binderType = binderFactory.TryGetBinder(name);

            Console.WriteLine(templateType + " name field");
            //make string field

            IFieldTemplate template = (IFieldTemplate)Activator.CreateInstance(templateType, new object[] { binderType, name, gameObject });
            template.Display(drawer);

            fields.Add(template);
        }
    }
}
