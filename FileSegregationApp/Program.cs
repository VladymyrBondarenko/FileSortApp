
using FileSortApp;

var fileName = "L.txt";

var fileGenerator = new FileGenerator(fileName);
fileGenerator.GenerateFile(10000000);

var fileSorter = new FileSorter();
fileSorter.SortFile(fileName, 100000);