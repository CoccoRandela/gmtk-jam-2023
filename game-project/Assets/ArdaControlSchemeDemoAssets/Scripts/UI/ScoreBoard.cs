using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TMPro.TMP_Text scoreText;
    private int score;
    public AudioClip CoinSound;

    void Start()
    {
        StartCoroutine("CountTheSeconds");
        GameStateManager.GameEnded += OnGameEnded;
        GameStateManager.CoinCollected += OnCoinPickedUp;
        GameStateManager.GameStarted += () => { scoreText.text = 0.ToString(); };
    }

    void OnGameEnded()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    IEnumerator CountTheSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            score += 1;
            scoreText.text = score.ToString();
        }
    }

    void OnCoinPickedUp()
    {
        SoundManager.Instance.PlayEffect(CoinSound);
        score += 20;
        scoreText.text = score.ToString();
    }
}
