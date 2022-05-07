using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    [SerializeField]
    private TextAsset jsonFile;
    [SerializeField]
    private Transform studentsPanel;
    [SerializeField]
    private GameObject studentRow;

    private StudentModel[] students;
    private List<GameObject> studentsRow;

    private static object syncLock = new();

    private void Awake()
    {
        if (Instance == null)
        {
            lock (syncLock)
            {
                if (Instance == null)
                {
                    Instance = this;
                }
            }
        }
    }

    private void Start()
    {
        studentsRow = new();
        string jsonString = jsonFile.text;
        students = JsonUtility.FromJson<StudentList>(jsonString).students;
    }

    private void OnEnable()
    {
        InvokeRepeating("UpdateRows", 5, 7);
    }

    private void OnDisable()
    {
        CancelInvoke("UpdateRows");
    }

    private void UpdateRows()
    {
        if (students == null)
        {
            Debug.LogError("Students not found");
            return;
        }

        if (studentsRow.Count == 0 || studentsRow.Count < students.Length)
        {
            var difference = students.Length - studentsRow.Count;

            for (int i = 0; i < difference; i++)
            {
                GameObject obj = Instantiate(studentRow);
                obj.transform.SetParent(studentsPanel,false);
                studentsRow.Add(obj);
            }
        }
        else if (studentsRow.Count > students.Length)
        {

            var difference = studentsRow.Count - students.Length;

            for (int i = 0; i < difference; i++)
            {
                studentsRow[i].SetActive(false);
            }
        }

        UpdateData();
    }

    private void UpdateData()
    {
        for (int i = 0; i < students.Length; i++)
        {
            RowStudent row = studentsRow[i].GetComponent<RowStudent>();
            row.SetStudent(students[i]);
        }
    }
}