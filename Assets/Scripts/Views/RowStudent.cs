using UnityEngine;
using Models;
using UnityEngine.UI;

public class RowStudent : MonoBehaviour
{
    [SerializeField]
    private Text firstname;
    [SerializeField]
    private Text surname;
    [SerializeField]
    private Text balance;
    [SerializeField]
    private Toggle check;

    public StudentModel Student { get; private set; }

    public void SetStudent(StudentModel student)
    {
        Student = student;
        firstname.text = Student.firtsname;
        surname.text = Student.surname;
        balance.text = Student.balance.ToString();
    }
}
