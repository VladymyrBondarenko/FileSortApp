using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSortApp
{
    internal class FileLine : IComparable<FileLine>
    {
        public FileLine(string line)
        {
            var pos = line.IndexOf('.');
            Number = int.Parse(line.Substring(0, pos));
            Word = line.Substring(pos + 2);
            Line = line;
        }

        public string Word { get; set; }

        public int Number { get; set; }

        public string Line { get; }

        public int CompareTo(FileLine other)
        {
            var result = Word.CompareTo(other.Word);
            if (result != 0)
            {
                return result;
            }
            return Number.CompareTo(other.Number);
        }
    }
}
