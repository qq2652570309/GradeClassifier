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
        String fileName;
        public Dictionary<int, Property> propertyMap; // key is column index, value is property object
        CompareInfo Compare; // ignore upper or lower case

        public Parser() {
            propertyMap = new Dictionary<int, Property>();
            Compare = CultureInfo.InvariantCulture.CompareInfo;
        }

        public Parser(string fileName) {
            this.fileName = fileName;
            propertyMap = new Dictionary<int, Property>();
            Compare = CultureInfo.InvariantCulture.CompareInfo;
        }

        public void ReadData(string fileName) {
            try {
                this.fileName = fileName;
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(fileName)) {
                    string line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    for (int i = 0; (line = sr.ReadLine()) != null; i++) {
                        string[] items = splitLine(line);
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
                Console.WriteLine("File could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        // split line into items
        private string[] splitLine(String line) {
            string[] items;
            if (fileName.IndexOf("csv") != -1){
                items = csvSplitLine(line);
            }
            else {
                items = line.Split('\t');
            }
            return items;
        }

        // split line in CSV file
        private string[] csvSplitLine(String line) {
            string[] items = line.Split(',');
            List<String> merge = new List<string>();
            for (int i = 0; i < items.Length; i++) {
                string item = items[i];
                if (item.IndexOf('\"') != -1) {
                    item += items[i + 1];
                    i++;
                }
                merge.Add(item);
            }
            return merge.ToArray<string>();
        }


        //------------------------------------------------------------------------
        //----------------------------- Header Parse -----------------------------
        //------------------------------------------------------------------------

        // parse first row, iterate all headers and create Property classes for them
        private void parseHeader(string[] headers) {
            for (int i = 4; i < headers.Length; i++) {
                string header = headers[i];
                int index = -1;
                string type = headerType(header, ref index);
                if (!type.Equals("None")) {
                    Property p = new Property(i);
                    string ptsType = pointsType(header);
                    int ptsMax = pointsMax(header);

                    p.setColType(type);
                    p.setIndex(index);
                    p.setPtsType(ptsType);
                    p.setPtsMax(ptsMax);

                    propertyMap.Add(i, p);
                }
            }
        }

        // find out which type header is
        // return Weight, Total, Attendance, Lab, Assignment, Quiz, Exam, or None
        private String headerType(string header, ref int index) {
            int bracket = header.IndexOf('[');
            if (bracket == -1)
                return "None";

            String colType;
            int pos;
            header = header.Substring(0, bracket);
            if ((pos = Compare.IndexOf(header, "Weight", CompareOptions.IgnoreCase)) != -1) {
                colType = "Weight";
            }
            else if ((pos = Compare.IndexOf(header, "Total", CompareOptions.IgnoreCase)) != -1) {
                colType = "Total";
            }
            else if ((pos = Compare.IndexOf(header, "attend", CompareOptions.IgnoreCase)) != -1) {
                colType = "Attendance";
            }
            else if ((pos = Compare.IndexOf(header, "lab", CompareOptions.IgnoreCase)) != -1) {
                colType = "Lab";
            }
            else if ((pos = Compare.IndexOf(header, "homework", CompareOptions.IgnoreCase)) != -1) {
                colType = "Assignment";
            }
            else if ((pos = Compare.IndexOf(header, "assign", CompareOptions.IgnoreCase)) != -1) {
                colType = "Assignment";
            }
            else if ((pos = Compare.IndexOf(header, "quiz", CompareOptions.IgnoreCase)) != -1) {
                colType = "Quiz";
            }
            else if ((pos = Compare.IndexOf(header, "exam", CompareOptions.IgnoreCase)) != -1) {
                colType = header;
            }
            else {
                colType = "None";
            }

            if (!colType.Equals("None") && bracket > pos) {
                string num = Regex.Match(header.Substring(pos, bracket - pos), @"\d+").Value;
                if (num != null && num.Length > 0)
                    index = Int32.Parse(num);
            }
            return colType;
        }

        // find out which type point is
        // return Score, Complete/Incomplete, Letter, or ?
        private String pointsType(string header) {
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


        //------------------------------------------------------------------------
        //---------------------------- Student Parse -----------------------------
        //------------------------------------------------------------------------

        // parse all rows except first one, create Student class for them
        private void parseStudent(string[] items) { 
            
        }


        public void print() {
            foreach (KeyValuePair<int, Property> item in propertyMap) {
                Console.WriteLine(item.Value.toString());
            }
        }
    }
}
