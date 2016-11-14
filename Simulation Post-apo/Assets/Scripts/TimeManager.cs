using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

    public Slider inputSlider;
    public Text inputTime;
    private float timeThreshold;
    private float currentTime;
    private System.DateTime currentDate;
    
    void Start ()
    {
        currentDate = System.DateTime.Now;
        currentTime = Time.time;
        timeThreshold = currentTime;
        UpdateTime();
	}
	
	void Update ()
    {
        Time.timeScale = inputSlider.value;

        if(timeThreshold > currentTime + 10f)
        {
            currentDate = currentDate.AddDays(1);
            UpdateTime();
            currentTime = Time.time;
            timeThreshold = currentTime;
        }
        else
        {
            timeThreshold += 0.05f * Time.timeScale;
        }
	}

    void UpdateTime()
    {
        inputTime.text = "Time : " + currentDate.ToString();
    }
}
