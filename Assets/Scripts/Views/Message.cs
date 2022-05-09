using Models;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Message : MonoBehaviour
{
    public static Message Instance { get; private set; }
    [SerializeField]
    private GameObject modal;
    [SerializeField]
    private GameObject editStudent;
    [SerializeField]
    private InputField firstname;
    [SerializeField]
    private InputField surname;
    [SerializeField]
    private InputField age;
    [SerializeField]
    private InputField balance;

    private Object synclock = new();

    private void Awake()
    {
        if (Instance == null)
            lock (synclock)
                if (Instance == null)
                    Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string messageType)
    {
        gameObject.SetActive(true);
        editStudent.SetActive(messageType.ToLower().Equals(MessageType.Edit.ToString().ToLower()));
        modal.SetActive(messageType.ToLower().Equals(MessageType.Error.ToString().ToLower()));
    }

    public void Hide(string messageType)
    {
        gameObject.SetActive(false);
        if (messageType.ToLower().Equals(MessageType.Edit.ToString().ToLower()))
            editStudent.SetActive(false);
        else modal.SetActive(false);
    }

    public void SetDataToEdidt(StudentModel student)
    {
        firstname.text = student.firtsname;
        surname.text = student.surname;
        age.text = student.age.ToString();
        balance.text = student.balance.ToString();
        Show(MessageType.Edit.ToString());
    }
}
