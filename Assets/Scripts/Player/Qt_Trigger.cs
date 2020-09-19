using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qt_Trigger : MonoBehaviour
{
    public enum QTState { Ready, Delay, Ongoing };
    public QTState qtState = QTState.Ready;
    public enum QTResponse { Null, Success, Fail };
    public QTResponse qtResponse = QTResponse.Null;
    public float DelayTimer = 0f;
    public float CountTimer = 2f;
    public string PlayerTag = "Player";
    private GameObject spoted, question;

    void Start()
    {
        spoted = GameObject.Find("spoted");
        question = GameObject.Find("questionmark");
    }

    void Update()
    {
        //lmao
        if (qtState == QTState.Ongoing) 
        {
            if(Input.GetButtonDown("Stab"))
            {
				Destroy(gameObject);
                qtResponse = QTResponse.Success;
				StopCoroutine(StateChange());
            } else if(qtResponse == QTResponse.Fail)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
    private IEnumerator StateChange()
	{
		qtState = QTState.Ongoing;
		yield return new WaitForSeconds (CountTimer);
		// If the timer is over and the event isn't over? Fix it! because most likely they failed.
		if (qtState == QTState.Ongoing) {
			qtResponse = QTResponse.Fail;
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (qtState == QTState.Ready && other.tag == "Knife") 
        {
			StartCoroutine(StateChange());
		}
    }
}
