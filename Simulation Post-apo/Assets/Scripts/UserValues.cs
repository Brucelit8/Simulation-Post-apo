using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UserValues : MonoBehaviour {

    public Slider M, A, R;
    public Text tM, tA, tR;
    private static bool onMenu = true;
    private static bool refresh = false;
    public static int sizeM = 0, nbA = 0, nbR = 0;

    private static UserValues instance;
    // Use this for initialization

    void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

	void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(M == null)
        {
            M = GameObject.Find("SliderMap").GetComponent<Slider>();
            A = GameObject.Find("SliderAgent").GetComponent<Slider>();
            R = GameObject.Find("SliderResources").GetComponent<Slider>();
            tM = GameObject.Find("TextM").GetComponent<Text>();
            tA = GameObject.Find("TextA").GetComponent<Text>();
            tR = GameObject.Find("TextR").GetComponent<Text>();
            GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(() => { this.Launch(); });
        }
        if (onMenu)
        {
            sizeM = (int)(M.value);
            nbA = (int)(A.value);
            nbR = (int)(R.value);
            tA.text = ""+nbA;
            tR.text = ""+nbR;
            tM.text = ""+sizeM;
        }
    }

    public void Launch()
    {
        onMenu = false;
        SceneManager.LoadScene("Main");
    }

    public static void GoMenu()
    {
        onMenu = true;
        SceneManager.LoadScene("Menu");
        refresh = true;
    }
}
