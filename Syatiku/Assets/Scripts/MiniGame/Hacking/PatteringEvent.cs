﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PatteringEvent : MonoBehaviour {

    [SerializeField, Tooltip("LowのPaper_1")]
    private RectTransform Low_Paper_1;
    [SerializeField, Tooltip("SpeedyのPaper_1")]
    private RectTransform Speedy_Paper_1;
    [SerializeField, Tooltip("LowのPaper_2")]
    private RectTransform Low_Paper_2;
    [SerializeField, Tooltip("SpeedyのPaper_2")]
    private RectTransform Speedy_Paper_2;
    [SerializeField, Tooltip("LowのPaper_0")]
    private RectTransform Low_Paper_0;
    [SerializeField, Tooltip("SpeedyのPaper_0")]
    private RectTransform Speedy_Paper_0;
    [SerializeField, Tooltip("取得するDocument")]
    private RectTransform getDocument;
    [SerializeField, Tooltip("取得するDocument")]
    private GameObject getDocument_obj;
    [SerializeField, Tooltip("place_button_6")]
    private GameObject place_button_6;
    [SerializeField, Tooltip("LowAnimationで使うObject")]
    private GameObject LowObject;
    [SerializeField, Tooltip("SpeedyAnimationで使うObject")]
    private GameObject SpeedyObject;
    [SerializeField, Tooltip("Low Title Object")]
    private RectTransform Low_Title;
    [SerializeField, Tooltip("Speedy Title Object")]
    private RectTransform Speedy_Title;
    [SerializeField]
    private EventSystem event_system;

    private HackTap hack_tap;
    private HackBoss hack_boss;
    private int successCount = 0;

    //いいタイミングかどうか
    private bool _success = false;

    [HideInInspector]
    public bool _lowAnimClear = false;

    private Sequence quen;
    private Sequence sequen;

    // Use this for initialization
    void Start ()
    {
        quen = DOTween.Sequence();
        _success = false;
        //Low_Title.GetComponent<Text>().text = "黄色のページをタップしよう！";
        //Speedy_Title.GetComponent<Text>().text = "黄色のページをタップしよう！";
        hack_tap = GetComponent<HackTap>();
        hack_boss = GetComponent<HackBoss>();
        getDocument_obj.SetActive(false);
        SpeedyObject.SetActive(false);
        LowObject.SetActive(false);
        _lowAnimClear = false;
        successCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

    }

    /// <summary>
    /// アニメーション中の色変更
    /// </summary>
    /// <param name="num">0.黄色,1.白</param>
    private void ChangeColor(int num)
    {
        switch (num)
        {
            case 0:
                Low_Paper_1.GetComponent<Image>().color = new Color(255, 255, 0);
                Low_Paper_2.GetComponent<Image>().color = new Color(255, 255, 0);
                Low_Paper_0.GetComponent<Image>().color = new Color(255, 255, 0);
                break;
            case 1:
                Low_Paper_1.GetComponent<Image>().color = new Color(255, 255, 255);
                Low_Paper_2.GetComponent<Image>().color = new Color(255, 255, 255);
                Low_Paper_0.GetComponent<Image>().color = new Color(255, 255, 255);
                break;
            case 2:
                Speedy_Paper_1.GetComponent<Image>().color = new Color(255, 255, 0);
                Speedy_Paper_2.GetComponent<Image>().color = new Color(255, 255, 0);
                Speedy_Paper_0.GetComponent<Image>().color = new Color(255, 255, 0);
                break;
            case 3:
                Speedy_Paper_1.GetComponent<Image>().color = new Color(255, 255, 255);
                Speedy_Paper_2.GetComponent<Image>().color = new Color(255, 255, 255);
                Speedy_Paper_0.GetComponent<Image>().color = new Color(255, 255, 255);
                break;
            default:
                Debug.Log("ColorNum :" + num);
                break;
        }
    }

    /// <summary>
    /// タップした時のテキスト処理
    /// </summary>
    /// <param name="time">テキストが変わる時間</param>
    /// <returns></returns>
    private IEnumerator Wait_Time(float time)
    {
        yield return new WaitForSeconds(time);
        getDocument_obj.SetActive(false);
    }

    /// <summary>
    /// LowAnimationをスタートさせる時の処理
    /// </summary>
    /// <param name="time">待ち時間</param>
    /// <returns></returns>
    public IEnumerator Start_LowWaitTime(float time)
    {
        LowObject.SetActive(true);
        event_system.enabled = false;
        sequen.Append(Low_Title.DOLocalMoveX(0f, 1.0f));
        yield return new WaitForSeconds(time);
        sequen.Append(Low_Title.DOLocalMoveX(-600f, 1.0f)
            .OnComplete(()=> event_system.enabled = true));
        yield return new WaitForSeconds(0.5f);
        LowAnim();
    }

    /// <summary>
    /// SpeedyAnimationをスタートさせる時の処理
    /// </summary>
    /// <param name="time">待ち時間</param>
    /// <returns></returns>
    public IEnumerator Start_SpeedyWaitTime(float time)
    {
        SpeedyObject.SetActive(true);
        event_system.enabled = false;
        quen.Append(Speedy_Title.DOLocalMoveX(0f, 1.0f));
        yield return new WaitForSeconds(time);
        quen.Append(Speedy_Title.DOLocalMoveX(-600f, 1.0f)
            .OnComplete(()=> event_system.enabled = true));
        yield return new WaitForSeconds(0.5f);
        SpeedyAnim();
    }

    /// <summary>
    /// アニメーション中のタップの時処理
    /// </summary>
    public void TapResult()
    {
        Debug.Log("タップ");
        if (!_success)
        {
            hack_boss.MoveBoss();
        }
        else
        {
            successCount++;
            getDocument_obj.SetActive(true);
        }
        StartCoroutine(Wait_Time(2f));
    }

    /// <summary>
    /// アニメーション終了時処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator End_Anim()
    {
        event_system.enabled = false;
        Speedy_Title.transform.GetComponentInChildren<Text>().text = "終了";
        Low_Title.transform.GetComponentInChildren<Text>().text = "終了";
        quen.Append(Speedy_Title.DOLocalMoveX(600f, 2.5f));
        sequen.Append(Low_Title.DOLocalMoveX(600f, 2.5f));
        yield return new WaitForSeconds(3f);
        PatteResult();
        event_system.enabled = true;
        SpeedyObject.SetActive(false);
    }

    /// <summary>
    /// パラパラの結果
    /// </summary>
    private void PatteResult()
    {
        if(successCount >= 1)
            _lowAnimClear = true;
        else
            _lowAnimClear = false;
        hack_tap.PlaceButton(11);
    }

    /// <summary>
    /// 遅めのアニメーション処理
    /// </summary>
    private void LowAnim()
    {
        Sequence se = DOTween.Sequence();
        se.Append(Low_Paper_1.DOLocalRotate(new Vector2(0, Low_Paper_1.localRotation.y + 180), 0.3f).SetDelay(0.5f).SetLoops(67, LoopType.Restart))
           .InsertCallback(3.7f, () => ChangeColor(0))
           .InsertCallback(3.7f, () => _success = true)
           .InsertCallback(4.3f, () => _success = false)
           .InsertCallback(4.3f, () => ChangeColor(1))
           .InsertCallback(10.4f, () => ChangeColor(0))
           .InsertCallback(10.4f, () => _success = true)
           .InsertCallback(11.0f, () => _success = false)
           .InsertCallback(11.0f, () => ChangeColor(1))
           .InsertCallback(18.5f, () => ChangeColor(0))
           .InsertCallback(18.5f, () => _success = true)
           .InsertCallback(19.1f, () => _success = false)
           .InsertCallback(19.1f, () => ChangeColor(1))
           .OnComplete(() => StartCoroutine(End_Anim()));
    }

    /// <summary>
    /// Animationのイベント処理（Loopバージョン）
    /// </summary>
    private void SpeedyAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Speedy_Paper_1.DOLocalRotate(new Vector2(0, Speedy_Paper_1.localRotation.y + 180), 0.2f).SetDelay(0.1f).SetLoops(90, LoopType.Restart))
           .InsertCallback(2.8f, () => ChangeColor(2))
           .InsertCallback(2.8f, () => _success = true)
           .InsertCallback(3.3f, () => _success = false)
           .InsertCallback(3.3f, () => ChangeColor(3))
           .InsertCallback(8.4f, () => ChangeColor(2))
           .InsertCallback(8.4f, () => _success = true)
           .InsertCallback(8.9f, () => _success = false)
           .InsertCallback(8.9f, () => ChangeColor(3))
           .InsertCallback(10.6f, () => ChangeColor(2))
           .InsertCallback(10.6f, () => _success = true)
           .InsertCallback(11.1f, () => _success = false)
           .InsertCallback(11.1f, () => ChangeColor(3))
           .InsertCallback(16.8f, () => ChangeColor(2))
           .InsertCallback(16.8f, () => _success = true)
           .InsertCallback(17.3f, () => _success = false)
           .InsertCallback(17.3f, () => ChangeColor(3))
           .OnComplete(() => StartCoroutine(End_Anim()));
    }
}