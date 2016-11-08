using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VariablesUI : MonoBehaviour {

    public static Text t_type;
    public static Text t_safety;
    public static Text t_bed;
    public static Text t_water;
    public static Text t_food;
    public static Text t_bandage;

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

    }

    // Update is called once per frame
    void Update () {
	
	}
}
