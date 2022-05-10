using System.Collections.Generic;
using UnityEngine;
using Models;
using Utils;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private TextAsset jsonFile;
    [SerializeField]
    private Transform[] studentsPanel;
    [SerializeField]
    private GameObject[] levels;
    [SerializeField]
    private GameObject[] studentUI;

    private int currentLevel;
    private List<int> rowsDisplayed;
    private StudentList students;
    private List<List<GameObject>> studentsRow;
    private Object synclock = new();
    private string path;


    private void Awake()
    {
        if (Instance == null)
            lock (synclock)
                if (Instance == null)
                    Instance = this;
    }


    private void Start()
    {
        path = $"{Application.persistentDataPath}/students.json";
        currentLevel = 0;
        studentsRow = new();
        rowsDisplayed = new();

        for (int i = 0; i < levels.Length; i++)
        {
            studentsRow.Add(new());
            rowsDisplayed.Add(0);
        }
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(LoadJsonFile), 0, 5);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(LoadJsonFile));
    }

    #region data
    private void LoadJsonFile()
    {
        if (!File.Exists(path))
        {
            Debug.Log($"{path} has been created");
            if (students?.students != null && students.students.Length > 0)
            {
                File.WriteAllText(path, JsonUtility.ToJson(new StudentList { students = students.students }, true));
                Debug.Log("Based on previous data");
            }
            else
            {
                File.WriteAllText(path, jsonFile.text);
                Debug.Log("Based on default data");
            }
        }
        students = JsonUtility.FromJson<StudentList>(File.ReadAllText(path));
        UpdateRows();
    }

    private void UpdateRows()
    {
        if (students == null)
        {
            Debug.LogError("Students not found");
            return;
        }

        var rows = studentsRow[currentLevel];

        if (rows.Count < students.students.Length)
        {
            var difference = students.students.Length - rows.Count;

            for (int i = 0; i < difference; i++)
            {
                GameObject obj = Instantiate(studentUI[currentLevel]);
                obj.SetActive(false);
                obj.transform.SetParent(studentsPanel[currentLevel], false);
                rows.Add(obj);
            }
        }
        if (rowsDisplayed[currentLevel] > students.students.Length)
        {

            var difference = rowsDisplayed[currentLevel] - students.students.Length;

            for (int i = 0; i < difference; i++)
            {
                rows[^(i + 1)].SetActive(false);
                rowsDisplayed[currentLevel]--;
            }
        }
        else if (rowsDisplayed[currentLevel] < students.students.Length)
        {
            var difference = students.students.Length - rowsDisplayed[currentLevel];

            for (int i = 0; i < difference; i++)
            {
                rows[rowsDisplayed[currentLevel]].SetActive(true);
                rowsDisplayed[currentLevel]++;
            }
        }

        UpdateData();
    }

    private void UpdateData()
    {
        var rows = studentsRow[currentLevel];
        for (int i = 0; i < students.students.Length; i++)
        {
            rows[i].GetComponent<IStudent>().SetStudent(students.students[i]);
        }
    }

    public void ValidateStudents()
    {
        var rows = studentsRow[currentLevel];
        foreach (var student in rows)
        {
            if (!student.GetComponent<IStudent>().ValidateFinalGrade())
            {
                Message.Instance.SetShow(MessageType.Error, true);
                return;
            }
        }
        NextLevel();
    }

    public void OpenJson()
    {
        Message.Instance.SetJsonEdit(JsonUtility.ToJson(new StudentList { students = students.students }, true));
    }

    public void UpdateJson(string jsonString)
    {
        File.WriteAllText(path, jsonString);
        LoadJsonFile();
    }
    #endregion

    #region Levels
    private void NextLevel()
    {
        if (currentLevel < levels.Length)
        {
            levels[currentLevel].SetActive(false);
            currentLevel++;
            if (currentLevel == levels.Length)
            {
                CancelInvoke(nameof(LoadJsonFile));
                return;
            }
            LoadJsonFile();
            levels[currentLevel].SetActive(true);
        }
    }
    #endregion
}
