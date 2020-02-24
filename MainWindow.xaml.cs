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

namespace GradeClassifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> columns;
        List<Student> studentInfo;
        List<string> targets;

        public MainWindow()
        {
            InitializeComponent();

            columns = new List<int>();
            studentInfo = new List<Student>();
            targets = new List<string>() {
                "Last Name", "First Name", "Username", "Student ID"
            };
        }

        private string GetFileName()
        {
            string fileName = "";
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV (*.csv)|*.csv|XLSX (*.xlsx)|*.xlsx|XLS (*.xls)|*.xls";
                if (openFileDialog.ShowDialog() == true)
                {
                    fileName = openFileDialog.FileName;
                    //txtEditor.Text = "successfully load " + fileName;
                    //DataTable data = new DataTable(fileName);
                    //data = NewDataTable(fileName, ",", true);
                    //grdDataImp.DataSource = data;

                    Console.WriteLine(fileName);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fileName;
        }

        // get basic column indices of basic student informantion
        // including: last name, first name, username, id
        private void targetColumns(string line)
        {
            string[] cols;
            if (line.IndexOf(',') != -1)
            {
                cols = line.Split(',');
            }
            else
            {
                cols = line.Split('\t');
            }
            for (int i = 0; i < cols.Length; i++)
            {
                string col = cols[i];
                foreach (string target in targets)
                {
                    if (col != null && col.IndexOf(target) != -1)
                    {
                        columns.Add(i);
                    }
                }
            }
        }

        private void ParseData(String line) {
            string[] cols;
            if (line.IndexOf(',') != -1)
            {
                cols = line.Split(',');
            }
            else
            {
                cols = line.Split('\t');
            }
            Student st = new Student(cols[0], cols[1], cols[2], int.Parse(cols[3]));
            studentInfo.Add(st);
        }

        private void ReadData(string fileName)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    for (int i = 0; (line = sr.ReadLine()) != null; i++)
                    {
                        if (i == 0)
                        {
                            targetColumns(line);
                        }
                        else
                        {
                            ParseData(line);
                        }
                    }
                }
                // use default comparator implemented in Student class
                studentInfo.Sort();
            }
            catch (Exception e)
            {
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


        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            columns.Clear();
            studentInfo.Clear();

            string fileName = GetFileName();
            ReadData(fileName);
            GenerateCSV(fileName);

            TextBox TextBoxItem = new TextBox();
            TextBoxItem.Text = fileName;
            loadingFiles.Items.Add(TextBoxItem);
        }

        private void renameFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}