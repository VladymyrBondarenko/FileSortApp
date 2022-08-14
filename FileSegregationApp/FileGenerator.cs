using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSortApp
{
    internal sealed class FileGenerator
    {
        private readonly Random random = new();

        private readonly string[] words;
        private readonly string fileName;

        public FileGenerator(string fileName)
        {
            words = Enumerable.Range(0, 10000)
                .Select(x =>
                {
                    var range = Enumerable.Range(0, random.Next(20, 100));
                    var chars = range.Select(x => (char)random.Next('A', 'Z')).ToArray();
                    var str = new string(chars);
                    return str;
                }).ToArray();

            this.fileName = fileName;
        }

        public void GenerateFile(int fileLength)
        {
            using var streamWriter = new StreamWriter(fileName);

            for (int i = 0; i < fileLength; i++)
            {
                streamWriter.WriteLine(generateNumber() + ". " + generateString());
            }
        }

        private string generateString()
        {
            return words[random.Next(words.Length)];
        }

        private int generateNumber()
        {
            return random.Next(0, 10000);
        }
    }
}