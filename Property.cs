using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeClassifier
{
    public class Property {
        int column; // which column
        //public string colType; // Weight, Total, Attendance, Lab, Assignment, Quiz, Exam
        string ptsType; // Score, Complete/Incomplete, Letter, or?
        int ptsMax;
        //public int weight;
        int index;

        

        public string bnVisible { get; set; }
        public string isVisible { get; set; }
        public int weight { get; set; }

        public string colType { get; set; }

        public Property() {
            column = -1;
            index = 0;
            //type = "";
            ptsType = "";
            ptsMax = 0;
            weight = 1;
        }

        public Property(int column) {
            this.column = column;
            index = 0;
            //type = "";
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

        public void setIndex(int index) {
            this.index = index;
        }

        //public void setWeight(int weight) {
        //    this.weight =  weight;
        //}

        public string toString() {
            string str;
            if (index <= 0)
                str = column + ": " + colType + "[" + "Total Pts: " + ptsMax + " " + ptsType + "], weight:" + weight;
            else
                str = column + ": " + colType + index + "[" + "Total Pts: "+ ptsMax + " "+ ptsType + "], weight:" + weight;
            return str;
        }
    }
}

