using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // ****** The DLL is copied to bin of website so it will give the path of bin of website ***
            string dllsFolder = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(dllsFolder));

            Assembly LoadedAssembly = Assembly.LoadFile(di.Parent.FullName.Replace("\\bin", "\\RefDLLs") + "\\DLLForReflection.dll");


            Console.WriteLine("Details of the Assembly Loaded:");
            Console.WriteLine(" ");
            var AssemblyTypes = LoadedAssembly.GetTypes();

            Dictionary<string, List<string>> dcnyNameSpaces = new Dictionary<string, List<string>>();


            foreach (var assemblyType in AssemblyTypes)
            {
                if (!dcnyNameSpaces.ContainsKey(assemblyType.Namespace))
                    dcnyNameSpaces.Add(assemblyType.Namespace, new List<string>());

                dcnyNameSpaces[assemblyType.Namespace].Add(assemblyType.ToString().Split('.').Last());
            }

            foreach (var NM in dcnyNameSpaces)
            {
                Console.WriteLine("");
                Console.WriteLine("Namespace:" + NM.Key);

                foreach (var ty in (List<string>)NM.Value)
                {
                    Console.WriteLine("\tClass:" + ty);

                    object tempObj = Activator.CreateInstance(LoadedAssembly.GetType(NM.Key + "." + ty));
                    foreach (var prop in tempObj.GetType().GetProperties())
                        Console.WriteLine("\t\tProperties:" + prop.Name);

                }
            }

            Console.Read();

        }
    }
}
