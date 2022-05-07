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
    [SerializeField]
    private GameObject[] levels;

    private short currentLevel;
    private StudentModel[] students;
    private List<RowStudent> studentsRow;

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
        currentLevel = 0;
        studentsRow = new();
        string jsonString = jsonFile.text;
        students = JsonUtility.FromJson<StudentList>(jsonString).students;
    }

    private void OnEnable()
    {
        InvokeRepeating("UpdateRows", 0, 7);
    }

    private void OnDisable()
    {
        CancelInvoke("UpdateRows");
    }

    #region data
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
                obj.transform.SetParent(studentsPanel, false);
                studentsRow.Add(obj.GetComponent<RowStudent>());
            }
        }
        else if (studentsRow.Count > students.Length)
        {

            var difference = studentsRow.Count - students.Length;

            for (int i = 0; i < difference; i++)
            {
                studentsRow[i].gameObject.SetActive(false);
            }
        }

        UpdateData();
    }

    private void UpdateData()
    {
        for (int i = 0; i < students.Length; i++)
        {
            studentsRow[i].SetStudent(students[i]);
        }
    }
    #endregion

    public void ValidateStudents()
    {
        foreach (var student in studentsRow)
        {
            if (!student.ValidateNote())
            {
                //TODO: Agregar show message
                return;
            }
        }
        NextLevel();
    }

    #region Levels
    private void NextLevel()
    {
        if (currentLevel < levels.Length)
        {
            levels[currentLevel].SetActive(false);
            currentLevel++;
            levels[currentLevel].SetActive(true);
        }
    }

    private void ResetLevel()
    {

    }

    private void RestartGame()
    {

    }
    #endregion
}
