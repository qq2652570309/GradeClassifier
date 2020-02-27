using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace GradeClassifier
{
    class Parser
    {
        CompareInfo Compare; // ignore upper or lower case
        int idIndex;
        String fileName;
        Dictionary<String, List<String>> classifier;
        List<String[]> others;
        int code;   // 200 everything is OK
                    // 401 no "Final Exame Grade" column 

        public Parser() {
            Compare = CultureInfo.InvariantCulture.CompareInfo;
            idIndex = 3;
            classifier = new Dictionary<string, List<string>>();
            others = new List<string[]>();
            code = 200;
        }

        public Parser(string fileName) {
            this.fileName = fileName;
            Compare = CultureInfo.InvariantCulture.CompareInfo;
            classifier = new Dictionary<string, List<string>>();
            others = new List<string[]>();
            code = 200;
        }

        public Dictionary<String, List<String>> getClassifier() {
            return classifier;
        }

        public List<String[]> getOthers() {
            return others;
        }

        public int getCode() {
            return code;
        }


        // Initialize all global variables
        public void initialization() {
            classifier.Clear();
            others.Clear();
            idIndex = 3;
            fileName = "";
            code = 200;
        }

        // Read given file (default is CSV file) and parse it.
        public void ReadData(string fileName) {
            initialization();

            try {
                using (StreamReader sr = new StreamReader(fileName)) {
                    string line;
                    char delimiter = ',';
                    string[] cells;
                    for (int i = 0; (line = sr.ReadLine()) != null; i++) {
                        if (i == 0) {
                            delimiter = findDelimiter(line);
                            cells = splitLine(line, delimiter);
                            parseHeader(cells);
                        }
                        else if (code == 200) {
                            cells = splitLine(line, delimiter);
                            parseStudent(cells);
                        }
                        else {
                            break;
                        }
                    }
                }
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("File could not be read:");
                Console.WriteLine(e.Message);
                code = 405;
            }
        }

        // find first char which potentially works as delimiter.
        private char findDelimiter(String header) {
            String delimiter = ",\t;|^";
            foreach (char c in header.ToCharArray()) {
                if (delimiter.IndexOf(c) != -1)
                    return c;
            }
            return ',';
        }

        // split line into items
        private string[] splitLine(String line, char delimiter) {
            string[] cells = line.Split(delimiter);
            return cells;
        }

        // parse first row, iterate each cells and find which column is student id
        private void parseHeader(string[] headers) {
            for (int i = 0; i < headers.Length; i++) {
                string header = headers[i];
                if (Compare.IndexOf(header, "Student ID", CompareOptions.IgnoreCase) != -1) {
                    idIndex = i;
                }
            }

            if (Compare.IndexOf(headers[headers.Length - 1], "Final Exam Grade", CompareOptions.IgnoreCase) == -1) {
                code = 401;
            }
        }

        // parse rows and get student id and grade
        private void parseStudent(string[] cells) {
            String studentId = trimQuotes(cells[idIndex]);
            String grade = trimQuotes(cells[cells.Length-1]);
            grade = grade.ToUpper();
            if (isGradeLetter(grade)) {
                if (!classifier.ContainsKey(grade)) {
                    classifier[grade] = new List<string>();
                }
                classifier[grade].Add(studentId);
            } else {
                grade = trimQuotes(cells[cells.Length - 1]);
                if (string.IsNullOrEmpty(grade) || string.IsNullOrWhiteSpace(grade))
                    grade = "no grade";
                others.Add(new string[] { studentId, grade });
            }
        }

        // Check if final grade is a letter, e.g. A B C D F 
        private Boolean isGradeLetter(String grade) {
            if (string.IsNullOrEmpty(grade) || string.IsNullOrWhiteSpace(grade))
                return false;
            char[] letters = { 'A', 'B', 'C', 'D', 'F' };
            if (!letters.Contains(grade[0]))
                return false;
            return true;
        }

        // remove quotes (") from cell
        private String trimQuotes(String cell) {
            return cell.Replace("\"", "");
        }

        public void print() {
            foreach(String key in classifier.Keys) {
                Console.Write(key + ": ");
                foreach (string id in classifier[key])
                    Console.Write(id+" ");
                Console.WriteLine();
            }

            if (others.Count > 0) {
                Console.WriteLine("Others:");
                foreach (String[] other in others) {
                    Console.WriteLine(other[0] + ": " + other[1]);
                }
            }
        }
    }
}
