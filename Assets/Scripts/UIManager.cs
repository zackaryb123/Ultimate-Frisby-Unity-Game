using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour {
	
	//Canvas canvas;
    public Frisby frisby;

	void Start()
	{
		//canvas = GetComponent<Canvas>();
        if (SceneManager.GetActiveScene().ToString() == "Game") 
            frisby = GameObject.Find("Frisby").GetComponent<Frisby>();
	}

    void Update()
    {

    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Pause()
	{
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}
	
	public void Quit()
	{
		#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
		#else 
		Application.Quit();
		#endif
	}

    public void ShortPass()
    {
        frisby.ShortPass = true;
        frisby.LongPass = false;
        frisby.GoalPass = false;

        GameManager.instance.HideGameImage();
        frisby.GetFrisbyPass();
    }

    public void LongPass()
    {
        frisby.LongPass = true;
        frisby.ShortPass = false;
        frisby.GoalPass = false;

        GameManager.instance.HideGameImage();
        frisby.GetFrisbyPass();
    }

    public void GoalPass()
    {
        frisby.GoalPass = true;
        frisby.ShortPass = false;
        frisby.LongPass = false;

        GameManager.instance.HideGameImage();
        frisby.GetFrisbyPass();
    }
}
