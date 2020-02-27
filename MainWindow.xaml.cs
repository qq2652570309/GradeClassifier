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
        List<string> targets;
        Parser parser;

        TextBox TcTb; // TermCodeTBox
        ComboBox ScTb; // SubjectCodeTBox
        TextBox CnTb; // CatalogNumberTBox
        TextBox StpTb; // StPCodeTBox
        ComboBox gcb; // GradeCBox

        public MainWindow() {
            InitializeComponent();

            columns = new List<int>();
            targets = new List<string>() {
                "Last Name", "First Name", "Username", "Student ID"
            };

            parser = new Parser();

            initTextBoxes();
        }

        // initialize several component variables
        private void initTextBoxes() {
            TcTb = TermCodeTBox;
            DateTime thisDay = DateTime.Today;
            string month = thisDay.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            string day = thisDay.Day.ToString();
            if (day.Length == 1)
                day = "0" + day;
            TcTb.Text = month+day;
            
            ScTb = SubjectCodeCBox;
            CnTb = CatalogNumberTBox;
            StpTb = StPCodeTBox;
            gcb = GradeCBox;
        }

        // clear content of focused textbox
        private void SetNullGotFocus(object sender, RoutedEventArgs e) {
            TextBox tb = sender as TextBox;
            tb.Text = "";
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
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        // Browse button event
        private void btnOpenFile_Click(object sender, RoutedEventArgs e) {
            string fileName = GetFileName();
            parser.ReadData(fileName);
            parser.print();

            CurrPath.Text = fileName;
        }

        private void renameFile_Click(object sender, RoutedEventArgs e) {

        }

        //TextBox TcTb; // TermCodeTBox
        //ComboBox ScTb; // SubjectCodeTBox
        //TextBox CnTb; // CatalogNumberTBox
        //TextBox StpTb; // StPCodeTBox
        //ComboBox gcb; // GradeCBox

        private void GenerateCSV(String oldFile, string fileName) {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            List<string> aList = new List<string>() { "946630449", "338835543", "903951927" };
            dict.Add("A", aList);
            List<string> aMinusList = new List<string>() { "946630449", "338835543", "903951927" };
            dict.Add("A-", aMinusList);
            List<string> bPlusList = new List<string>() { "946630449", "338835543", "903951927" };
            dict.Add("B+", bPlusList);
            List<string> bList = new List<string>() { "946630449", "338835543", "903951927" };
            dict.Add("B", bList);
            List<string> bMinusList = new List<string>() { "946630449", "338835543", "903951927" };
            dict.Add("B-", bMinusList);
            List<string> cList = new List<string>() { "946630449", "338835543", "903951927" };
            dict.Add("C", cList);
            List<string[]> others = new List<string[]>();

            int pos = oldFile.LastIndexOf("\\");
            //String newFile = oldFile.Substring(0, pos + 1) + "out.csv";
            fileName = oldFile.Substring(0, pos + 1) + fileName;
            string grade = gcb.Text;
            string spc = StpTb.Text;

            foreach (KeyValuePair<string, List<string>> entry in dict) {
                string newName = fileName + "_" + entry.Key + ".csv";
                List<string> ids = entry.Value;

                File.AppendAllText(newName, "SU,GRAD,Student¡¯s program code,Student¡¯s SUID number,Final Exam Grade\n");
                foreach (string id in ids) {
                    string insert = string.Join(",", new string[] { "SU", grade, spc, id, entry.Key}) + "\n";
                    File.AppendAllText(newName, insert);
                }
                Console.WriteLine("grade {0} finish!", entry.Key);
            }

            if (others.Count > 0) {
                string newName = fileName + "_others.csv";
                File.AppendAllText(newName, "SU,GRAD,Student¡¯s program code,Student¡¯s SUID number,Final Exam Grade\n");
                foreach (string[] strs in others) {
                    string insert = string.Join(",", new string[] { "SU", grade, spc, strs[0], strs[1] }) + "\n";
                    File.AppendAllText(newName, insert);
                }
                MessageBox.Show("There are problems with the data of " + others.Count + " students. Students' data is stored in "+ newName);
                Console.WriteLine("others finish!");
            }

        }

        // verify 3 textbox, guarantee none is empty. Gnerate output CSV
        private void PublishClick(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(CurrPath.Text)) {
                MessageBox.Show("Please choose a file");
                return;
            }

            if (string.IsNullOrWhiteSpace(TcTb.Text)) {
                MessageBox.Show("Term Code can not be blank");
                return;
            }
            if (string.IsNullOrWhiteSpace(ScTb.Text)) {
                MessageBox.Show("Subject Code can not be blank");
                return;
            }
            if (string.IsNullOrWhiteSpace(CnTb.Text)) {
                MessageBox.Show("Catalog Number can not be blank");
                return;
            }
            if (string.IsNullOrWhiteSpace(StpTb.Text)) {
                MessageBox.Show("Student¡¯s program code can not be blank");
                return;
            }

            string fileName = TcTb.Text + "_" + ScTb.Text + "_" + CnTb.Text;
            Console.WriteLine(fileName);

            GenerateCSV(CurrPath.Text, fileName);
        }

    }
}