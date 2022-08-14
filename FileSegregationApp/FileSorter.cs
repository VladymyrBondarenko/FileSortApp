using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSortApp
{
    internal sealed class FileSorter
    {
        public void SortFile(string fileName, int partLinesCount)
        {
            // cut file into separate files
            var files = separateFile(fileName, partLinesCount);

            // sort separated files
            sortFiles(files);

            // write sorted files to result file
            sortResult(files);
        }

        private void sortFiles(List<string> files)
        {
            foreach (var file in files)
            {
                var sortedLines = File.ReadAllLines(file)
                    .Select(x => new FileLine(x))
                    .OrderBy(x => x);
                File.WriteAllLines(file, sortedLines.Select(x => x.Line));
            }
        }

        private void sortResult(List<string> files)
        {
            var readers = files.Select(x => new StreamReader(x));
            try
            {
                var lines = readers.Select(x => new FileLineState
                {
                    FileLine = new FileLine(x.ReadLine()),
                    StreamReader = x
                }).ToList();

                using var streamWriter = new StreamWriter("result.txt");
                while (lines.Count > 0)
                {
                    var currentLine = lines.OrderBy(x => x.FileLine).First();
                    streamWriter.WriteLine(currentLine.FileLine.Line);

                    if (currentLine.StreamReader.EndOfStream)
                    {
                        lines.Remove(currentLine);
                    }
                    else
                    {
                        currentLine.FileLine = new FileLine(currentLine.StreamReader.ReadLine());
                    }
                }
            }
            finally
            {
                foreach (var reader in readers)
                {
                    reader.Dispose();
                }
            }
        }

        private List<string> separateFile(string fileName, int partLinesCount)
        {
            var files = new List<string>();

            using var streamReader = new StreamReader(fileName);
            var partNumber = 0;
            while (!streamReader.EndOfStream)
            {
                partNumber++;
                var partFileName = partNumber + ".txt";
                files.Add(partFileName);

                using var streamWriter = new StreamWriter(partFileName);
                for (int i = 0; i < partLinesCount; i++)
                {
                    if (streamReader.EndOfStream)
                    {
                        break;
                    }

                    streamWriter.WriteLine(streamReader.ReadLine());
                }
            }

            return files;
        }
    }
}
