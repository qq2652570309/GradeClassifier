using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeClassifier
{
    public class Property {
        int column; // which column
        string ptsType; // Score, Complete/Incomplete, Letter, or?
        int ptsMax;
        

        public string bnVisible { get; set; }
        public string isVisible { get; set; }
        public int weight { get; set; }

        public string colType { get; set; } // Weight, Total, Attendance, Lab, Assignment, Quiz, Exam

        public string colName { get; set; } // original name in file

        public Property() {
            column = -1;
            colName = "";
            ptsType = "";
            ptsMax = 0;
            weight = 1;
        }

        public Property(int column) {
            this.column = column;
            colName = "";
            ptsType = "";
            ptsMax = 0;
            weight = 1;
        }

        public void setColType(String colType) {
            this.colType = colType;
        }

        public String getColType() {
            return colType;
        }

        public void setPtsType(string ptsType) {
            this.ptsType = ptsType;
        }

        public void setPtsMax(int ptsMax) {
            this.ptsMax = ptsMax;
        }

        public void setName(string name) {
            this.colName = name;
        }

        //public void setWeight(int weight) {
        //    this.weight =  weight;
        //}

        public string toString() {
            string str = 
                column + " " + colType + ": " + colName + " [" + "Total Pts: "+ ptsMax + " "+ ptsType + "], weight:" + weight;
            return str;
        }
    }
}

