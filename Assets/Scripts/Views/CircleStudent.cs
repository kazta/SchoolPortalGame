using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;
using Utils;

public class CircleStudent : MonoBehaviour, IStudent, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Text fullname;
    [SerializeField]
    private Text balance;
    [SerializeField]
    private Transform dragZone;

    private CanvasGroup canvasGroup;

    public StudentModel Student { get; private set; }

    private void Start()
    {
        dragZone = GameObject.Find("Level2").transform;
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
        transform.position = Input.mousePosition;
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
        balance.text = Student.balance.ToString();
    }

    public bool ValidateFinalGrade()
    {
        var zoneType = GetComponentInParent<DropZone>().zoneType;
        if (zoneType == ZoneType.Neutral)
            return false;
        return (Student.balance > 2.9f && zoneType == ZoneType.Approved)
            || (Student.balance < 3 && zoneType == ZoneType.Failed);
    }
}
