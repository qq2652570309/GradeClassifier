using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeClassifier
{
    class Test
    {
        public int id;
        public String firstName;
        public Test(int id, String firstName) {
            this.id = id;
            this.firstName = firstName;
        }

        public void toString() {
            System.Diagnostics.Debug.WriteLine(id + ": " + firstName);
        }
    }
}
