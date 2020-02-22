﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeClassifier
{
    class Student : IEquatable<Student>, IComparable<Student>
    {
        private String lastName;
        private String firstName;
        private String userName;
        private int id;

        public Student() { }

        public Student(String lastName, String firstName, String userName, int id) {
            this.lastName = lastName;
            this.firstName = firstName;
            this.userName = userName;
            this.id = id;
        }

        public String CsvFormat() {
            StringBuilder sb = new StringBuilder();
            sb.Append(lastName+","+firstName+","+userName+ ","+id+"\n");
            return sb.ToString();
        }

        public int CompareTo(Student other) {
            if (other == null) {
                return -1;
            }
            return other.GetID() - this.id;
        }

        public bool Equals(Student other)
        {
            if (other == null) return false;
            return this.id == other.GetID() && this.userName.Equals(other.GetUserName());
        }

        public String GetLastName() {
            return lastName;
        }

        public String GetFirstName()
        {
            return firstName;
        }

        public String GetUserName()
        {
            return userName;
        }

        public int GetID()
        {
            return id;
        }

        public void PrintInfo() {
            Console.WriteLine("{0} {1}", firstName, lastName);
            Console.WriteLine("    UserName: {0}", userName);
            Console.WriteLine("    ID: {0}", id);
        }
    }
}
