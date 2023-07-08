using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartMenu : MonoBehaviour
{
    public TMPro.TMP_Text moleText;
    public TMPro.TMP_Text aText;
    public TMPro.TMP_Text whackText;

    void Awake()
    {
        Debug.Log(transform.position);
        Debug.Log(transform.localPosition);
    }

    public async void Fall()
    {
        await transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutBounce).AsyncWaitForCompletion();

        moleText.gameObject.SetActive(true);

        await moleText.transform.DOScale(Vector3.one, 0.4f).AsyncWaitForCompletion();

        aText.gameObject.SetActive(true);

        await aText.transform.DOScale(Vector3.one, 0.4f).AsyncWaitForCompletion();

        whackText.gameObject.SetActive(true);

        await whackText.transform.DOScale(Vector3.one, 0.4f).AsyncWaitForCompletion();

    }
}
