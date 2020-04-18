using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float time = 300f;
    public float LimitTime;
    public Text Timetext;
    // Start is called before the first frame update
    void Start()
    {
        Timetext = GameObject.Find("TimeText").GetComponent<Text>();
        LimitTime = time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LimitTime -= Time.fixedDeltaTime;
        Timetext.text = "Limmit : " + LimitTime.ToString("F2") + "sec";
        if(LimitTime <= 0f)
        {
            GameObject.Find("Crear").GetComponent<Text>().text = "G A M E O V E R";
            GameObject[] enemis = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject x in enemis)
            {
                Destroy(x);
            }
            Destroy(gameObject.GetComponent<PlayerScript>());
            Destroy(gameObject.GetComponent<LocomotionScript>());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Enemy")
        {
            LimitTime -= 20.0f;
        }
        else if (collision.gameObject.tag == "Finish")
        {
            GameObject.Find("Crear").GetComponent<Text>().text = "C R E A R";
            GameObject[] enemis = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject x in enemis)
            {
                Destroy(x);
            }
            Destroy(gameObject.GetComponent<PlayerScript>());
            Destroy(gameObject.GetComponent<LocomotionScript>());
        }
    }
}
