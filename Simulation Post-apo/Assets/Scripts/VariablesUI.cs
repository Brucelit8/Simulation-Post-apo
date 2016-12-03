using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VariablesUI : MonoBehaviour {

    public static Slider t_slider, th_slider, hu_slider, he_slider;

    public static float a_thirst, a_tiredness, a_hunger, a_health;

    public static Text t_type, t_safety, t_bed, t_water, t_food, t_bandage, t_scrap;

    static VariablesUI instance;
    // Use this for initialization

    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start ()
    {
        t_type = GameObject.Find("Type").GetComponent<Text>();
        t_safety = GameObject.Find("Safety").GetComponent<Text>();
        t_bed = GameObject.Find("Bed").GetComponent<Text>();
        t_water = GameObject.Find("Water").GetComponent<Text>();
        t_food = GameObject.Find("Food").GetComponent<Text>();
        t_bandage = GameObject.Find("Bandage").GetComponent<Text>();
        t_scrap = GameObject.Find("Scrap").GetComponent<Text>();

        th_slider = GameObject.Find("Thirst Slider").GetComponent<Slider>();
        t_slider = GameObject.Find("Tiredness Slider").GetComponent<Slider>();
        hu_slider = GameObject.Find("Hunger Slider").GetComponent<Slider>();
        he_slider = GameObject.Find("Health Slider").GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update () {
        th_slider.value = a_thirst;
        he_slider.value = a_health;
        hu_slider.value = a_hunger;
        t_slider.value = a_tiredness;
	}
}
