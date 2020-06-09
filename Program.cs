using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EnumExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> parsedEnums = new List<string>();

            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, args[0]);
                var fileNames = GetFileNames(path, "*.cs");

                foreach (var fileName in fileNames)
                {
                    var filePath = Path.Combine(path, fileName);
                    string fileContent = File.ReadAllText(filePath);

                    Console.WriteLine($"-------- Parsing {fileName} start --------");

                    EnumListener extractor = ExtractEnums(fileContent);
                    parsedEnums.AddRange(ExportAllEnumsToFiles(extractor.ParsedEnums, fileName, args[1]));
                                   
                    Console.WriteLine($"-------- Parsing {fileName} end ----------\n");
                }

                Console.WriteLine($"\nParsing enums done. Following enums has been parsed:\n\t- {string.Join("\n\t- ", parsedEnums)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        static List<string> GetFileNames(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter);
            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Length; i++)
                fileNames.Add(Path.GetFileName(files[i]));

            fileNames.Sort();

            return fileNames;
        }

        static EnumListener ExtractEnums(string fileContent)
        {
            AntlrInputStream inputStream = new AntlrInputStream(fileContent);
            EnumLexer lexer = new EnumLexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            EnumParser parser = new EnumParser(tokens);
            IParseTree tree = parser.prog();
            ParseTreeWalker walker = new ParseTreeWalker();
            EnumListener extractor = new EnumListener(parser);
            walker.Walk(extractor, tree);

            return extractor;
        }

        static IEnumerable<string> ExportAllEnumsToFiles(List<ParsedEnum> parsedEnums, string fileName, string path)
        {
            foreach (var parsedEnum in parsedEnums)
            {
                CheckDuplicates(parsedEnum.ParsedEnumIds, fileName);
                SaveToFile(parsedEnum.EnumName, parsedEnum.ParsedEnumContent.ToString(), path);
                yield return parsedEnum.EnumName;
            }
        }

        static void CheckDuplicates(IList<int> enumsIds, string fileName)
        {
            var groupedEnums = enumsIds.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.FirstOrDefault());

            if (groupedEnums.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n\n\t{fileName} has duplicated ids:\n\t- {string.Join("\n\t- ", groupedEnums)}\n\n");
                Console.ResetColor();
            }
        }

        static void SaveToFile(string enumName, string content, string path)
        {
            if (!string.IsNullOrWhiteSpace(enumName))
            {
                var pathToFile = Path.Combine(Environment.CurrentDirectory, path, enumName + ".js");

                using (FileStream fs = new FileStream(pathToFile, FileMode.OpenOrCreate))
                using (StreamWriter file = new StreamWriter(fs))
                {
                    file.Write(content);
                    file.Close();
                    fs.Close();
                }
            }
        }       
    }
}
