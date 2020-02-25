using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeClassifier
{
    class Property {
        int column; // which column
        string type; // Weight, Total, Attendance, Lab, Assignment, Quiz, Exam
        string ptsType; // Score, Complete/Incomplete, Letter, or?
        int ptsMax;
        int weight;
        int index;

        public Property() {
            column = -1;
            index = 0;
            type = "";
            ptsType = "";
            ptsMax = 0;
            weight = 1;
        }

        public Property(int column) {
            this.column = column;
            index = 0;
            type = "";
            ptsType = "";
            ptsMax = 0;
            weight = 1;
        }

        public void setType(String type) {
            this.type = type;
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

        public void setWeight(int weight) {
            this.weight =  weight;
        }

        public string toString() {
            string str;
            if (index <= 0)
                str = column + ": " + type + "[" + "Total Pts: " + ptsMax + " " + ptsType + "], weight:" + weight;
            else
                str = column + ": " + type + index + "[" + "Total Pts: "+ ptsMax + " "+ ptsType + "], weight:" + weight;
            return str;
        }
    }
}

