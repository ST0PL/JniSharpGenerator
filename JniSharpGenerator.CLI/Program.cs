using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using JniSharpGenerator.Exceptions;
using JniSharpGenerator.Parser;

namespace JniSharpGenerator.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Options options = Options.Parse(args);

            if(options.UnknownOptions.Count > 0)
            {
                Console.WriteLine(string.Join("\n", options.UnknownOptions.Select(o => $"Error: unknown option: {o}")));
                WriteUsage();
                return;
            }

            List<JavaClass> klasses;

            try
            {
                klasses = [..GetClasses(options.JavapOptions).OfType<JavaClass>().DistinctBy(k => $"{k.Package}.{k.Name}")] ;
            }
            catch(JavaPrinterException e) { Console.Write(e.Message); return; }

            if(options.SaveToFile)
                klasses.ForEach(k => SaveClass(k, options));
            else
                klasses.ForEach(k => PrintClass(k, options));
        }

        static List<JavaClass?> GetClasses(string javapArgs)
        {
            var (stdOutput, stdError) = RunJavap(javapArgs);

            if (!string.IsNullOrWhiteSpace(stdError))
                throw new JavaPrinterException(stdError);
            
            return Regex.Matches(stdOutput, Expressions.ClassSignature, RegexOptions.Multiline).Select(m => JavaClass.Parse(m.Value)).ToList();
        }

        static void SaveClass(JavaClass klass, Options options)
        {
            string package = klass.Package.Replace(".", @"\");
            string directory = string.IsNullOrWhiteSpace(options.Destination) ? (string.IsNullOrWhiteSpace(package) ? Directory.GetCurrentDirectory() : package) : options.Destination;
            string fileName = $"{klass.Name}.cs";

            try
            {
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, fileName);

                File.WriteAllText(path, Syntax.GetSharpClassDefinition(klass, options.GenerateAccessors));
                Console.WriteLine($"Class \"{path}\" was successfully saved.");
            }
            catch(Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }

        static void PrintClass(JavaClass klass, Options options)
            => Console.WriteLine(Syntax.GetSharpClassDefinition(klass, options.GenerateAccessors));

        static void WriteUsage()
        {
            Console.WriteLine(
                "Usage: JniSharpGenerator <options>" +
                "\nWhere possible options include:" +
                "\n  -a                               Generate field accessors" +
                "\n  -s                               Save classes to files" +
                "\n  -d <directory>                   Specify where to place generated source files" +
                "\n  --class-path <path>              Specify where to find user class files" +
                "\n  -classpath <path>                Specify where to find user class files" +
                "\n  -cp <path>                       Specify where to find user class files");
        }

        static (string stdOutput, string stdError) RunJavap(string javapArgs)
        {
            using Process process = new();

            process.EnableRaisingEvents = true;

            process.StartInfo = new()
            {
                FileName = "javap",
                Arguments = javapArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            StringBuilder standardOutput = new();
            StringBuilder standardError = new();

            process.OutputDataReceived += (_, e) => standardOutput.AppendLine(e.Data ?? string.Empty);
            process.ErrorDataReceived += (_, e) => standardError.AppendLine(e.Data ?? string.Empty);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return (standardOutput.ToString(), standardError.ToString());
        }
    }
}
