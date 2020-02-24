using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

namespace GradeClassifier
{
    class CSV
    {
        String fileName;
        Dictionary<int, Property> propertyMap; // key is column index, value is property object
        CompareInfo Compare; // ignore upper or lower case

        public CSV() { }

        public CSV(string fileName) {
            this.fileName = fileName;
            propertyMap = new Dictionary<int, Property>();
        }

        private void ReadData(string fileName) {
            try {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(fileName)) {
                    string line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    for (int i = 0; (line = sr.ReadLine()) != null; i++) {
                        string[] items = ParseData(line);
                        if (i == 0) {
                            parseHeader(items);
                        }
                        else {
                            
                        }
                    }
                }
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        // split line into items
        private string[] ParseData(String line)
        {
            string[] items;
            if (line.IndexOf(',') != -1)
            {
                items = line.Split(',');
            }
            else
            {
                items = line.Split('\t');
            }
            return items;
        }

        // iterate all headers and parse them
        private void parseHeader(string[] headers) {
            for (int i = 4; i < headers.Length; i++) {
                string header = headers[i];
                string type = headerType(header);
                if (!type.Equals("None")) {
                    Property p = new Property(i);
                    string ptsType = pointsType(header);
                    int ptsMax = pointsMax(header);

                    p.setType(type);
                    p.setPtsType(ptsType);
                    p.setPtsMax(ptsMax);

                    propertyMap.Add(i, p);
                }
            }
        }

        // find out which type header is
        // return Weight, Total, Attendance, Lab, Assignment, Quiz, Exam, or None
        private String headerType(string header)
        {
            int bracket = header.IndexOf('[');
            String colType = header.Substring(0,bracket);
            if (Compare.IndexOf(header, "Weight", CompareOptions.IgnoreCase) != -1)
            {
                colType = "Weight";
            }
            else if (Compare.IndexOf(header, "Total", CompareOptions.IgnoreCase) != -1)
            {
                colType = "Total";
            }
            else if (Compare.IndexOf(header, "attend", CompareOptions.IgnoreCase) != -1)
            {
                colType = "Attendance";
            }
            else if (Compare.IndexOf(header, "lab", CompareOptions.IgnoreCase) != -1)
            {
                colType = "Lab";
            }
            else if ((Compare.IndexOf(header, "homework", CompareOptions.IgnoreCase) != -1) ||
                (Compare.IndexOf(header, "assign", CompareOptions.IgnoreCase) != -1))
            {
                colType = "Assignment";
            }
            else if (Compare.IndexOf(header, "quiz", CompareOptions.IgnoreCase) != -1)
            {
                colType = "Quiz";
            }
            else if (Compare.IndexOf(header, "exam", CompareOptions.IgnoreCase) != -1)
            {
                colType = "Exam";
            }
            else
            {
                colType = "None";
            }

            return colType;
        }

        // find out which type point is
        // return Score, Complete/Incomplete, Letter, or ?
        private String pointsType(string header)
        {
            int bracket1 = header.IndexOf('[');
            int bracket2 = header.LastIndexOf(']');
            String grade = header.Substring(bracket1, bracket2 - bracket1);
            String pointsType;
            if (Compare.IndexOf(grade, "score", CompareOptions.IgnoreCase) != -1)
            {
                pointsType = "Score";
            }
            else if (Compare.IndexOf(grade, "Complete", CompareOptions.IgnoreCase) != -1)
            {
                pointsType = "Complete/Incomplete";
            }
            else if (Compare.IndexOf(grade, "letter", CompareOptions.IgnoreCase) != -1)
            {
                pointsType = "Letter";
            }
            else
            {
                pointsType = "?";
            }

            return pointsType;
        }

        // get maximum range of score
        private int pointsMax(string header) {
            int bracket1 = header.IndexOf('[');
            int bracket2 = header.LastIndexOf(']');
            string subHeader = header.Substring(bracket1, bracket2 - bracket1);
            string pointStr = new String(subHeader.Where(Char.IsDigit).ToArray());
            int point = Int32.Parse(pointStr);
            return point;
        }

        private void selfTest() { 
            
        }
    }
}
