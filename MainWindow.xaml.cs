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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV (*.csv)|*.csv|XLSX (*.xlsx)|*.xlsx";
                if (openFileDialog.ShowDialog() == true)
                {
                    fileName = openFileDialog.FileName;
                    txtEditor.Text = "successfully load " + fileName;

                    //DataTable data = new DataTable(fileName);
                    //data = NewDataTable(fileName, ",", true);
                    //grdDataImp.DataSource = data;

                    String str = File.ReadAllText(fileName);
                    System.Diagnostics.Debug.WriteLine("\n\n");
                    System.Diagnostics.Debug.WriteLine("-----------------------------------");
                    //System.Diagnostics.Debug.WriteLine(str);
                    Test t = new Test(1, "one");
                    t.toString();

                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Program not work");
                Console.WriteLine(exc.Message);
            }
        }
    }
}
