using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour {

    public string Name;
    public int Score;

    public Text NameText;
    public Text ScoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInfo(string name, int score)
    {
        Name = name;
        Score = score;

        NameText.text = Name;
        ScoreText.text = Score.ToString();
    }
}
