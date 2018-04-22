using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{

    public float time = 120;

	public GameObject explosion;

	bool playing = true;

    private float timer;
    public float Timer
    {
        get { return timer; }
        set
        {
            timer = value;

            int min = (int)timer / 60;
            int sec = (int)timer % 60;

            GameObject.Find("Timer").GetComponent<Text>().text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;

            GameObject.Find("Level").GetComponent<Text>().text = "Level " + level;
        }
    }

    private static GManager instance;
    public static GManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GManager();
            }
            return instance;
        }
    }


    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

        DontDestroyOnLoad(transform.gameObject);

        Timer = time;

        Level = level;

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(playing)
		Timer -= Time.deltaTime;

		if(Timer + 0.49f < 0.01)
		{
			Death();
		}
    }

    public void Death()
    {
        Camera.main.transform.parent = null;

		var f = Instantiate(explosion, GameObject.Find("Car(Clone)").transform.position, explosion.transform.rotation);

		Destroy(f, 0.5f);

        GameObject.Destroy(GameObject.Find("Car(Clone)"));

		playing = false;

        StartCoroutine("GameOver");
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Find("Game Over").GetComponent<Text>().text = "GAME OVER";
        GameObject.Find("Last Level").GetComponent<Text>().text = "You have reached level " + Level;
        GameObject.Find("Restart").GetComponent<Text>().text = "Press R to restart game";

		DestroyImmediate(gameObject);

        yield break;
    }
}
