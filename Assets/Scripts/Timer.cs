using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    private bool isGameStopped = false;
    private float timer = 0.0f;
    private float timeDelta = 0.0f;
    private IEnumerator coroutine;
    [SerializeField] private Text GetTime;
    [SerializeField] GameObject Ghour, Gminute, Gsecond;
    [SerializeField] float hour, minute, second;
    [SerializeField] GameObject Parent;
    [SerializeField] Dropdown dropdownHour, dropdownminute;
    [SerializeField] ArrowHourAndMinute CurrentArrow;
    [SerializeField] GameObject ArrowHour, ArrowMinute;
    string[] sites = { "https://yandex.ru", "https://google.com" };
    DateTime AChechGlobalTime()
    {
        var www = new WWW(sites[UnityEngine.Random.Range(0,sites.Length)]);
        while (!www.isDone && www.error == null)
        {
            Thread.Sleep(1);
        }
        if (www.error != null) return DateTime.Now;
        var str = www.responseHeaders["Date"];
        DateTime dateTime;
        DateTime.TryParse(str, out dateTime);
        return dateTime;
    }
    IEnumerator WaitAndPrint()
    {
        while (true)
        {
            timer = Time.time - timeDelta;

            yield return null;
        }
    }
    DateTime _time;
    [SerializeField]float hourTimer, minuteTimer;
    public void setHour()
    {
        this.hourTimer = float.Parse(dropdownHour.value.ToString());
        ArrowHour.transform.parent.rotation = Quaternion.Euler(0, 0, hourTimer * (360 / 12));
    }
    public void setMinute()
    {
        this.minuteTimer = float.Parse(dropdownminute.value.ToString());
        ArrowMinute.transform.parent.rotation = Quaternion.Euler(0, 0, (minuteTimer) * (360 / 60));
    }
    // Use this for initialization
    void Start()
    {
        dropdownHour.options = new List<Dropdown.OptionData>();
        dropdownminute.options = new List<Dropdown.OptionData>();
        hourTimer = 5;
        minuteTimer = 5;
        for (int i = 0; i < 23; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = i.ToString();
            dropdownHour.options.Add(data);
        }
        for (int i = 0; i < 59; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = i.ToString();
            dropdownminute.options.Add(data);
        }
        coroutine = WaitAndPrint();
        StartCoroutine(coroutine);
        _time = AChechGlobalTime();
        ArrowHour.transform.parent.rotation = Quaternion.Euler(0, 0, (hourTimer) * (360 / 12));
        ArrowMinute.transform.parent.rotation = Quaternion.Euler(0, 0, minuteTimer * (360 / 60));

    }
    // Update is called once per frame
    float starpos = 0;
    GameObject arrow; float arrowTime = 1, timekoef = 0;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            starpos = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                var txtmesh = raycastHit.collider.GetComponent<ArrowHourAndMinute>();
                if (txtmesh != null)
                {
                    if (txtmesh._Unit == UnitTime.Hour) {  }
                    if (txtmesh._Unit == UnitTime.Minute) {  }
                    //txtmesh.transform.parent.LookAt(new Vector3(raycastHit.point.x,raycastHit.point.y, 6.983363f));
                    CurrentArrow = txtmesh;
                }
            }
            if (CurrentArrow != null)
            {
                
                if (CurrentArrow._Unit == UnitTime.Hour) {
                    if (Input.mousePosition.x > starpos)
                    {
                        arrowTime += 0.1f;
                    }
                    else
                    {
                        arrowTime -= 0.1f;
                    }
                    arrow = CurrentArrow.gameObject; hourTimer = arrowTime; timekoef = 12;
                    if (arrow != null) arrow.transform.parent.rotation = Quaternion.Euler(0, 0, (arrowTime) * (360 / timekoef));
                    if (arrowTime >= 24) { arrowTime = 0; }
                    if (arrowTime < 0) { arrowTime = 23; }
                }
                if (CurrentArrow._Unit == UnitTime.Minute) {
                    if (Input.mousePosition.x > starpos)
                    {
                        arrowTime += 1.0f;
                    }
                    else
                    {
                        arrowTime -= 1.0f;
                    }
                    arrow = CurrentArrow.gameObject; minuteTimer = arrowTime; timekoef = 60;
                    if (arrow != null) arrow.transform.parent.rotation = Quaternion.Euler(0, 0, (arrowTime) * (360 / timekoef));
                    if (arrowTime >= 60) { arrowTime = 0; }
                    if (arrowTime < 0) { arrowTime = 59; }
                }
               
                //if (arrow != null) arrow.transform.parent.rotation = Quaternion.Euler(0, 0, (arrowTime) * (360 / timekoef));
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            CurrentArrow = null;arrow = null;
        }
        var _timespan =_time.AddSeconds(timer);
        float koef = (1 / 60) * _timespan.Minute;
        hour = (float)_timespan.Hour;
        minute = _timespan.Minute;
        second = _timespan.Second;
        GetTime.text = _timespan.ToString();
        Ghour.transform.rotation = Quaternion.Euler(0, 0, (hour) * (360 / 12));
        Gminute.transform.rotation = Quaternion.Euler(0, 0, minute * (360 / 60));
        Gsecond.transform.rotation = Quaternion.Euler(0, 0, second * (360 / 60));
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            Parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            Parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void OnMouseDrag()
    {
        
    }
}
