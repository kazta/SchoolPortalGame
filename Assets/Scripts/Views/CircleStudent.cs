using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;
using Utils;

public class CircleStudent : MonoBehaviour, IStudent, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    private Text fullname;
    [SerializeField]
    private Text balance;

    private Canvas canvas;
    private Transform dragZone;
    private RectTransform rTransform;
    private CanvasGroup canvasGroup;

    public StudentModel Student { get; private set; }

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        dragZone = GameObject.Find("Level2").transform;
        rTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(dragZone);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void SetStudent(StudentModel student)
    {
        Student = student;
        fullname.text = $"{Student.firtsname} {Student.surname}";
        balance.text = Student.finalGrade.ToString();
    }

    public bool ValidateFinalGrade()
    {
        var zoneType = GetComponentInParent<DropZone>().zoneType;
        if (zoneType == ZoneType.Neutral)
            return false;
        return (Student.finalGrade > 2.9f && zoneType == ZoneType.Approved)
            || (Student.finalGrade < 3 && zoneType == ZoneType.Failed);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Message.Instance.SetDataStudent(Student);
    }
}
