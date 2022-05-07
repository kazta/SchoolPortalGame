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
    private Transform[] studentsPanel;
    [SerializeField]
    private GameObject[] levels;
    [SerializeField]
    private GameObject[] studentUI;

    private short currentLevel;
    private StudentModel[] students;
    private List<List<GameObject>> studentsRow;

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

        for (int i = 0; i < levels.Length; i++)
            studentsRow.Add(new());
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

        var rows = studentsRow[currentLevel];

        if (rows.Count == 0 || rows.Count < students.Length)
        {
            var difference = students.Length - rows.Count;

            for (int i = 0; i < difference; i++)
            {
                GameObject obj = Instantiate(studentUI[currentLevel]);
                obj.transform.SetParent(studentsPanel[currentLevel], false);
                rows.Add(obj);
            }
        }
        else if (rows.Count > students.Length)
        {

            var difference = rows.Count - students.Length;

            for (int i = 0; i < difference; i++)
            {
                rows[i].SetActive(false);
            }
        }

        UpdateData();
    }

    private void UpdateData()
    {
        var rows = studentsRow[currentLevel];
        for (int i = 0; i < students.Length; i++)
        {
            rows[i].GetComponent<IStudent>().SetStudent(students[i]);
        }
    }

    public void ValidateStudents()
    {
        var rows = studentsRow[currentLevel];
        foreach (var student in rows)
        {
            if (!student.GetComponent<IStudent>().ValidateFinalGrade())
            {
                Debug.Log("Paila");//TODO: Agregar show message
                return;
            }
        }
        Debug.Log("Brevas");
        NextLevel();
    }
    #endregion

    #region Levels
    private void NextLevel()
    {
        if (currentLevel < levels.Length)
        {
            levels[currentLevel].SetActive(false);
            currentLevel++;
            if(currentLevel == levels.Length)
            {
                return;
            }
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
