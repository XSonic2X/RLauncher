using System;
using System.IO;

namespace RLauncher
{
    public class DFille :IDisposable
    {
        public DFille(string path, string name) 
        {
            this.Path = path;
            this.Name = name;
            if (IfFille()) { file = File.Open($"{Path}\\{Name}", FileMode.Open); }
            else { file = File.Create($"{Path}\\{Name}"); }
        }
        string Name;
        string Path;
        public FileStream file;
        private bool IfFille()
        {

            string[] dir1 = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
            string txt = $"{Path}\\{Name}";
            for (int i = 0; i < dir1.Length; i++) { if (dir1[i] == txt) { return true; }}
            return false;
        }
        public override string ToString()
        {
            return Name;
        }
        public void Dispose()
        {
            file.Close();
            file.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
