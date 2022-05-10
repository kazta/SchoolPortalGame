using System;

namespace Models
{
    [Serializable]
    public class StudentModel
    {
        public string id;
        public short index;
        public float finalGrade;
        public short age;
        public string firtsname;
        public string surname;
    }

    [Serializable]
    public class StudentList
    {
        public StudentModel[] students;
    }
}
