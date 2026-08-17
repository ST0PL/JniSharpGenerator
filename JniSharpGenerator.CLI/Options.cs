using System.Text;

namespace JniSharpGenerator.CLI
{
    internal class Options(bool generateAccessors, bool saveToFile, string destination, string javapOptions, IReadOnlyList<string> unknownOptions)
    {
        private static readonly string[] classpathOptions = ["--class-path", "-classpath", "-cp"];
        public bool GenerateAccessors => generateAccessors;
        public bool SaveToFile => saveToFile;
        public string Destination => destination;
        public string JavapOptions => javapOptions;
        public IReadOnlyList<string> UnknownOptions => unknownOptions;

        public static Options Parse(string[] args)
        {
            bool generateAccessors = false;
            bool saveToFile = false;
            string destination = string.Empty;
            StringBuilder javapOptionsBuilder = new("-p ");

            List<string> unknownOptions = [];

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith('-'))
                {
                    if (!generateAccessors && args[i] == "-a")
                        generateAccessors = true;

                    else if (!saveToFile && args[i] == "-s")
                        saveToFile = true;
                    else if (args[i] == "-d")
                    {
                        if (args.Length == (i + 1))
                            unknownOptions.Add(args[i]);
                        else
                            destination = args[++i];
                    }
                    else if (classpathOptions.Contains(args[i]))
                    {
                        javapOptionsBuilder.Append($"{string.Join(" ", args[i..])}");
                        break;
                    }
                    else
                        unknownOptions.Add(args[i]);
                }
                else
                    javapOptionsBuilder.Append(args[i]  +" ");
            }

            return new(generateAccessors, saveToFile, destination, javapOptionsBuilder.ToString(), unknownOptions.AsReadOnly());
        }
    }
}
