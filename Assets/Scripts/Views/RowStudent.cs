using UnityEngine;
using Models;
using UnityEngine.UI;

public class RowStudent : MonoBehaviour, IStudent
{
    [SerializeField]
    private Text firstname;
    [SerializeField]
    private Text surname;
    [SerializeField]
    private Text balance;
    [SerializeField]
    private Toggle check;
    [SerializeField]
    private Button more;

    private AudioSource audioSource;

    public StudentModel Student { get; private set; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        check.onValueChanged.AddListener(delegate { ToggleValueChange(); });
        more.onClick.AddListener(delegate { Message.Instance.SetDataStudent(Student); });
    }

    public void SetStudent(StudentModel student)
    {
        Student = student;
        firstname.text = Student.firtsname;
        surname.text = Student.surname;
        balance.text = Student.balance.ToString();
    }

    public bool ValidateFinalGrade()
    {
        return (Student.balance > 2.9f && check.isOn) || (Student.balance < 3 && !check.isOn);
    }

    private void ToggleValueChange()
    {
        audioSource.Play();
    }

}
