using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeClassifier
{
    class Property {
        int index; // which column
        string type; // Weight, Total, Attendance, Lab, Assignment, Quiz, Exam
        string ptsType; // Score, Complete/Incomplete, Letter, or?
        int ptsMax;

        public Property() {
            index = -1;
            type = "";
            ptsType = "";
            ptsMax = 0;
        }

        public Property(int index) {
            this.index = index;
            type = "";
            ptsType = "";
            ptsMax = 0;
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


        public string toString() { 
            string str = index + ": " + type + "[" + "Total Pts: "+ ptsMax + " "+ ptsType + "]";
            return str;
        }
    }
}

