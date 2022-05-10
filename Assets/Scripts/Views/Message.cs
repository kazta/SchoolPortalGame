using System;
using System.Collections.Generic;
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
    private GameObject editJson;
    [SerializeField]
    private InputField firstname;
    [SerializeField]
    private InputField surname;
    [SerializeField]
    private InputField age;
    [SerializeField]
    private InputField balance;
    [SerializeField]
    private InputField jsonEdit;
    [SerializeField]
    private Button cancelStudent;
    [SerializeField]
    private Button cancelJson;
    [SerializeField]
    private Button okError;

    private StudentModel Student;
    private UnityEngine.Object synclock = new();
    private Dictionary<MessageType, ShowMessage> dictionary;

    private delegate void ShowMessage(bool isActive);
    private ShowMessage showMessage;

    private void Awake()
    {
        if (Instance == null)
            lock (synclock)
                if (Instance == null)
                    Instance = this;
    }

    private void Start()
    {
        showMessage += (bool isActive) => { gameObject.SetActive(isActive); };

        dictionary = new();
        dictionary.Add(MessageType.Error, (bool isActive) => { modal.SetActive(isActive); });
        dictionary.Add(MessageType.EditStudent, (bool isActive) => { editStudent.SetActive(isActive); });
        dictionary.Add(MessageType.EditJson, (bool isActive) => { editJson.SetActive(isActive); });

        cancelStudent.onClick.AddListener(delegate { SetShow(MessageType.EditStudent, false); });
        cancelJson.onClick.AddListener(delegate { SetShow(MessageType.EditJson, false); });
        okError.onClick.AddListener(delegate { SetShow(MessageType.Error, false); });

        gameObject.SetActive(false);
    }

    public void SetShow(MessageType messageType, bool isShow)
    {
        showMessage += dictionary[messageType];
        showMessage(isShow);
        showMessage -= dictionary[messageType];
    }

    public void SetDataStudent(StudentModel student)
    {
        Student = student;
        firstname.text = Student.firtsname;
        surname.text = Student.surname;
        age.text = Student.age.ToString();
        balance.text = Student.finalGrade.ToString();
        SetShow(MessageType.EditStudent, true);
    }

    public void SaveDataStudent()
    {
        Student.firtsname = firstname.text;
        Student.surname = surname.text;
        Student.age = short.Parse(age.text);
        Student.finalGrade = float.Parse(balance.text);
        GameManager.Instance.UpdateStudent(Student);
        SetShow(MessageType.EditStudent, false);
    }

    public void SetJsonEdit(string jsonString)
    {
        SetShow(MessageType.EditJson, true);
        jsonEdit.text = jsonString;
    }

    public void SaveJson()
    {
        GameManager.Instance.UpdateJson(jsonEdit.text);
        SetShow(MessageType.EditJson, false);
    }
}
