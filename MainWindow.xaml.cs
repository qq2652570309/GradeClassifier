using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GradeClassifier {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<int> columns;
        List<Student> studentInfo;
        List<string> targets;
        Parser parser;

        Dictionary<Property, List<Property>> gradeDict;
        Dictionary<string, Property> existHead;

        public MainWindow() {
            InitializeComponent();

            columns = new List<int>();
            studentInfo = new List<Student>();
            targets = new List<string>() {
                "Last Name", "First Name", "Username", "Student ID"
            };

            parser = new Parser();

            gradeDict = new Dictionary<Property, List<Property>>();
            existHead = new Dictionary<string, Property>();

            //List<GradeItem> hwList = new List<GradeItem>();
            //GradeItem hw1 = new GradeItem();
            //hw1.type = "homework";
            //hw1.level = 1;
            //hw1.title = hw1.type;
            //hw1.weight = "0";

            //for (int i = 0; i < 3; i++) {
            //    GradeItem tmp = new GradeItem();
            //    tmp.type = "homework";
            //    tmp.level = 2;
            //    tmp.title = tmp.type + i;
            //    tmp.weight = "0";
            //    hwList.Add(tmp);
            //}
            //gradeDict.Add(hw1, hwList);

            //List<GradeItem> quizList = new List<GradeItem>();
            //GradeItem quiz1 = new GradeItem();
            //quiz1.type = "quiz";
            //quiz1.level = 1;
            //quiz1.title = quiz1.type;
            //quiz1.weight = "0";

            //for (int i = 0; i < 3; i++) {
            //    GradeItem tmp = new GradeItem();
            //    tmp.type = "quiz";
            //    tmp.level = 2;
            //    tmp.title = tmp.type + i;
            //    tmp.weight = "0";
            //    quizList.Add(tmp);
            //}
            //gradeDict.Add(quiz1, quizList);
        }

        private string GetFileName() {
            string fileName = "";
            try {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV (*.csv)|*.csv|XLSX (*.xlsx)|*.xlsx|XLS (*.xls)|*.xls";
                if (openFileDialog.ShowDialog() == true) {
                    fileName = openFileDialog.FileName;
                    //txtEditor.Text = "successfully load " + fileName;
                    //DataTable data = new DataTable(fileName);
                    //data = NewDataTable(fileName, ",", true);
                    //grdDataImp.DataSource = data;

                    Console.WriteLine(fileName);
                }
            }
            catch (Exception) {
                throw;
            }
            return fileName;
        }

        // get basic column indices of basic student informantion
        // including: last name, first name, username, id
        private void targetColumns(string line) {
            string[] cols;
            if (line.IndexOf(',') != -1) {
                cols = line.Split(',');
            }
            else {
                cols = line.Split('\t');
            }
            for (int i = 0; i < cols.Length; i++) {
                string col = cols[i];
                foreach (string target in targets) {
                    if (col != null && col.IndexOf(target) != -1) {
                        columns.Add(i);
                    }
                }
            }
        }

        private void ParseData(String line) {
            string[] cols;
            if (line.IndexOf(',') != -1) {
                cols = line.Split(',');
            }
            else {
                cols = line.Split('\t');
            }
            Student st = new Student(cols[0], cols[1], cols[2], int.Parse(cols[3]));
            studentInfo.Add(st);
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
                        if (i == 0) {
                            targetColumns(line);
                        }
                        else {
                            ParseData(line);
                        }
                    }
                }
                // use default comparator implemented in Student class
                studentInfo.Sort();
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void GenerateCSV(String oldFile) {
            int pos = oldFile.LastIndexOf("\\");
            String newFile = oldFile.Substring(0, pos + 1) + "out.csv";
            File.AppendAllText(newFile, "Last Name,First Name,UserName,Student ID\n");
            foreach (Student st in studentInfo) {
                File.AppendAllText(newFile, st.CsvFormat());
            }
        }

        /*
        check whether input grading value is valid. If input is null or can not be
        converted to integer, it is invalid 
        */
        private bool isValidGrade(string str, ref int num, String grade) {
            bool isNullEmpty = string.IsNullOrEmpty(str);
            if (isNullEmpty) {
                MessageBox.Show("Grading " + grade + " can't be blank");
                return false;
            }
            bool isNum = Int32.TryParse(str, out num);
            if (!isNum) {
                MessageBox.Show("Grading " + grade + " must be Numeric");
                return false;
            }
            return true;
        }

        /*
        check whether input scales are valid. If there exit an overlapping, scale is invalid 
        */
        private bool isValidScale(int ceiling, int floor, int preFloor, string grade) {
            if (ceiling <= 0 || floor <= 0 || preFloor <= 0) {
                MessageBox.Show("Scale must be positive and non-zero");
                return false;
            }
            if (preFloor <= ceiling || ceiling <= floor) {
                MessageBox.Show("There is overlapping with scale " + grade);
                return false;
            }
            return true;
        }

        // check grade scale is valid without overlapping. verfiy continuity
        private bool checkGradingScale() {
            string AMinValue = Amin.Text;
            int AMinNum = 0;

            string BMaxValue = Bmax.Text;
            string BMinValue = Bmin.Text;
            int BMaxNum = 0;
            int BMinNum = 0;

            string CMaxValue = Cmax.Text;
            string CMinValue = Cmin.Text;
            int CMaxNum = 0;
            int CMinNum = 0;

            string DMaxValue = Dmax.Text;
            string DMinValue = Dmin.Text;
            int DMaxNum = 0;
            int DMinNum = 0;
            // verify Grade Scale A
            if (!isValidGrade(AMinValue, ref AMinNum, "A")) {
                return false;
            }
            if (AMinNum >= 100) {
                MessageBox.Show("Grading A must less than 100");
                return false;
            }
            // verify Grade Scale B
            if (!isValidGrade(BMaxValue, ref BMaxNum, "B")
                || !isValidGrade(BMinValue, ref BMinNum, "B")) {
                return false;
            }
            if (!isValidScale(BMaxNum, BMinNum, AMinNum, "B")) {
                return false;
            }
            // verify Grade Scale C
            if (!isValidGrade(CMaxValue, ref CMaxNum, "C")
                || !isValidGrade(CMinValue, ref CMinNum, "C")) {
                return false;
            }
            if (!isValidScale(CMaxNum, CMinNum, BMinNum, "C")) {
                return false;
            }
            // verify Grade Scale D
            if (!isValidGrade(DMaxValue, ref DMaxNum, "D")
                || !isValidGrade(DMinValue, ref DMinNum, "D")) {
                return false;
            }
            if (!isValidScale(DMaxNum, DMinNum, CMinNum, "D")) {
                return false;
            }
            // verify continuity
            if (AMinNum != BMaxNum + 1 || BMinNum != CMaxNum + 1 || CMinNum != DMaxNum + 1) {
                MessageBox.Show("There are gaps among grade scales");
                return false;
            }
            return true;
        }

        // Browse button event
        private void btnOpenFile_Click(object sender, RoutedEventArgs e) {
            string fileName = GetFileName();
            parser.ReadData(fileName);
            parser.print();

            gradingColmums.Items.Clear();
            gradeDict.Clear();
            existHead.Clear();

            //Dictionary<Property, List<Property>> gradeDict;

            foreach (KeyValuePair<int, Property> entry in parser.propertyMap) {
                Property p = entry.Value;
                if (existHead.ContainsKey(p.colType)) {
                    gradeDict[existHead[p.colType]].Add(p);
                }
                else {
                    Property head = new Property();
                    head.colType = p.colType;
                    head.colName = head.colType;
                    //head.setType(p.getColType());
                    List<Property> list = new List<Property>();
                    list.Add(p);
                    gradeDict.Add(head, list);
                    existHead.Add(head.colType, head);
                }
            }

            foreach (KeyValuePair<Property, List<Property>> entry in gradeDict) {
                Property keyItem = entry.Key;
                List<Property> listItems = entry.Value;

                keyItem.isVisible = "Visible";
                keyItem.bnVisible = "Visible";
                gradingColmums.Items.Add(keyItem);
                Console.WriteLine(keyItem.colType);

                foreach (Property gi in listItems) {
                    gi.isVisible = "Collapsed";
                    gi.bnVisible = "Hidden";
                    gradingColmums.Items.Add(gi);
                    Console.WriteLine(gi.colType);
                }
            }
            gradingColmums.Items.Refresh();
        }

        private void renameFile_Click(object sender, RoutedEventArgs e) {

        }

        private void ExpandClick(object sender, RoutedEventArgs e) {
            Button bn = sender as Button;
            int index = gradingColmums.Items.IndexOf(bn.DataContext);
            Property keyItem = (Property)gradingColmums.Items.GetItemAt(index);

            List<Property> list = gradeDict[keyItem];
            foreach (Property gi in list) {
                if (gi.isVisible == "Visible") {
                    gi.isVisible = "Collapsed";
                }
                else {
                    gi.isVisible = "Visible";
                }
            }
            gradingColmums.Items.Refresh();
        }


        private void PublishClick(object sender, RoutedEventArgs e) {
            // check grading scales are valid
            if (!checkGradingScale()) {
                return;
            }

        }

    }

    public class GradeItem {
        public string type { get; set; }
        public int level { get; set; }
        public string bnVisible { get; set; }

        public string title { get; set; }
        public string weight { get; set; }
        public string isVisible { get; set; }
    }
}