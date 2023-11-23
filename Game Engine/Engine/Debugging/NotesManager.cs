using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Debugging
{
    public static class NotesManager
    {
        public static void Init()
        {
            TakeNotes();
        }

        static void TakeNotes()
        {
            List<Type> typesWithNotes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttribute(typeof(NoteAttribute)) != null).ToList();

            string note = "";
            foreach (Type type in typesWithNotes) 
            {
                AddNote(type, ref note);
            }

            WriteNotes(note);
        }

        static void AddNote(Type type, ref string note)
        {
            NoteAttribute[] notes = (NoteAttribute[])type.GetCustomAttributes(typeof(NoteAttribute));

            foreach(NoteAttribute noteAttribute in notes) 
            {
                note += noteAttribute.TimeStamp + ": In class " + type.Name + ": " + noteAttribute.note + "\n";
            }
        }

        static void WriteNotes(string note)
        {
            using (StreamWriter writer = new StreamWriter("../../Notes.txt", false))
            {
                writer.Write(note);
            }
        }
    }

    public class NoteAttribute : Attribute
    {
        public string note;
        public string TimeStamp = DateTime.Now.ToString();
    }
}
