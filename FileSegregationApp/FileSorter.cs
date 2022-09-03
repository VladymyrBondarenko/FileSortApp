using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSortApp
{
    internal sealed class FileSorter
    {
        public async Task SortFile(string fileName, int partLinesCount)
        {
            // cut file into separate files and sort them
            var files = await separateAndSortFile(fileName, partLinesCount);

            // write sorted files to result file
            await sortResult(files);
        }

        private async Task<List<string>> separateAndSortFile(string fileName, int partLinesCount)
        {
            var fileLines = new FileLine[partLinesCount];
            var partNumber = 0;
            var files = new List<string>();
            int i = 0;

            using var reader = new StreamReader(fileName);
            for (string line = await reader.ReadLineAsync(); ; line = await reader.ReadLineAsync())
            {
                fileLines[i] = new FileLine(line);
                i++;
                if(i == partLinesCount)
                {
                    partNumber++;
                    var partFileName = partNumber + ".txt";
                    files.Add(partFileName);

                    Array.Sort(fileLines);
                    await File.WriteAllLinesAsync(partFileName, fileLines.Select(x => x.Line));
                    i = 0;
                }

                if (reader.EndOfStream)
                    break;
            }

            if(i != 0)
            {
                Array.Resize(ref fileLines, i + 1);
                partNumber++;
                var partFileName = partNumber + ".txt";
                files.Add(partFileName);
                await File.WriteAllLinesAsync(partFileName, fileLines.Select(x => x.Line));
            }

            return files;
        }

        private async Task sortResult(List<string> files)
        {
            var readers = files.Select(x => new StreamReader(x)).ToArray();
            try
            {
                var lines = readers.Select(x => new FileLineState
                {
                    FileLine = new FileLine(x.ReadLine()),
                    StreamReader = x
                }).OrderBy(x => x.FileLine).ToList();

                using var streamWriter = new StreamWriter("result.txt");
                while (lines.Count > 0)
                {
                    var currentLine = lines[0];
                    streamWriter.WriteLine(currentLine.FileLine.Line);

                    if (currentLine.StreamReader.EndOfStream)
                    {
                        lines.Remove(currentLine);
                    }
                    else
                    {
                        currentLine.FileLine = new FileLine(await currentLine.StreamReader.ReadLineAsync());
                        Reorder(lines);
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

        private void Reorder(List<FileLineState> lines)
        {
            if (lines.Count == 1)
                return;

            int i = 0;
            while (lines[i].FileLine.CompareTo(lines[i + 1].FileLine) > 0)
            {
                var t = lines[i];
                lines[i] = lines[i + 1];
                lines[i + 1] = t;
                i++;
                if (i + 1 == lines.Count)
                    return;
            }
        }
    }
}
