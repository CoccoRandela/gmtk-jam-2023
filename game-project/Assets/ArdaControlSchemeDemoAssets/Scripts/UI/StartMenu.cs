using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject moleText;
    public GameObject aText;
    public GameObject whackText;
    public Button startGameBtn;

    void Awake()
    {
        startGameBtn.enabled = false;
    }

    public async void Fall()
    {
        await transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutBounce).AsyncWaitForCompletion();

        moleText.SetActive(true);

        await moleText.transform.DOScale(new Vector3(5, 5), 0.4f).AsyncWaitForCompletion();

        aText.SetActive(true);

        await aText.transform.DOScale(new Vector3(2, 2), 0.4f).AsyncWaitForCompletion();

        whackText.SetActive(true);

        await whackText.transform.DOScale(new Vector3(5, 5), 0.4f).AsyncWaitForCompletion();

        startGameBtn.gameObject.SetActive(true);

        await startGameBtn.transform.DOScale(new Vector3(3, 3), 0.4f).AsyncWaitForCompletion();

        startGameBtn.enabled = true;

    }

    public async void Remove(TweenCallback callback)
    {
        await transform.DOLocalMove(new Vector3(0, 800), 1).AsyncWaitForCompletion();
        callback();
    }
}
