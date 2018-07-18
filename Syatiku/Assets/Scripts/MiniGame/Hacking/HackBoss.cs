﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackBoss : MonoBehaviour {

    [SerializeField, Tooltip("メーターの上司Object")]
    private GameObject Boss;
    [SerializeField, Tooltip("ドアの上司Object")]
    private GameObject ComeBoss;
    [SerializeField, Tooltip("EventSystem")]
    private EventSystem event_system;
    [SerializeField, Tooltip("Zoom object")]
    private GameObject Zoom;
    [SerializeField, Header("ボスの質問に答える時の選択Object")]
    private GameObject ChooseObject;
    [SerializeField, Header("ボスの質問に答える時のText")]
    private Text chose_text;

    private HackTap hack_tap;
    private HackMain hack_main;
    [Header("上司が待機してる時間")]
    public float BossTimer = 5.0f;
    private float Bosswait;
    private bool _chooseTap = false;
    private bool _commingboss = false;
    private bool _gameover = false;

    // Use this for initialization
    void Start () {
        hack_tap = GetComponent<HackTap>();
        hack_main = GetComponent<HackMain>();

        ChooseObject.SetActive(false);
        ComeBoss.SetActive(false);
        _commingboss = false;
        _gameover = false;
        Bosswait = BossTimer + 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (_commingboss)
        {
            Bosswait -= Time.deltaTime;
            chose_text.text = Bosswait.ToString("f1");
            if (_chooseTap)
            {
                Boss.transform.localPosition = new Vector2(-365, -130);
                ComeBoss.SetActive(false);
                hack_tap.PlaceButton(12);
                _chooseTap = false;
                _commingboss = false;
                Bosswait = BossTimer + 0.1f;
            }
            else if(Bosswait <= 0.0f)
            {
                Bosswait = 0.0f;
                if (!_gameover)
                {
                    _gameover = true;
                    Common.Instance.clearFlag[Common.Instance.isClear] = false;
                    Common.Instance.ChangeScene(Common.SceneName.Result);
                }
            }
        }
	}

    /// <summary>
    /// 上司が来て待ってる時の処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator WatchBoss(float time)
    {
        yield return new WaitForSeconds(time);
        event_system.enabled = true;
        _commingboss = true;
        ChooseObject.SetActive(true);
    }

    /// <summary>
    /// メーターの上司が動く処理 
    /// </summary>
    public void MoveBoss()
    {
        Boss.transform.localPosition = new Vector2(Boss.transform.localPosition.x + 265/2, -130);
        hack_main.comingCount++;
        if (hack_main.comingCount % 4 == 0)
        {
            hack_tap.PlaceButton(11);
            Zoom.transform.GetChild(3).gameObject.SetActive(false);
            Zoom.transform.GetChild(4).gameObject.SetActive(false);
            event_system.enabled = false;
            ComeOnBoss();
        }
    }

    /// <summary>
    /// 上司が部屋に来た時の処理
    /// </summary>
    public void ComeOnBoss()
    {
        hack_tap.PlaceButton(13);
        if (!ComeBoss.activeSelf)
        {
            ComeBoss.SetActive(true);
            StartCoroutine(WatchBoss(1.5f));
        }
    }

    /// <summary>
    /// 選択ボタン処理
    /// </summary>
    public void ChooseButton()
    {
        _chooseTap = true;
        ChooseObject.SetActive(false);
    }
}