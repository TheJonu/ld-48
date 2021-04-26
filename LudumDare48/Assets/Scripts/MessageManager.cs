using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Levels;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [Serializable]
    public class State
    {
        public float posY;
        public float duration;
        public Ease ease;
    }
        
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private RectTransform panel;
    [SerializeField] private State enter;
    [SerializeField] private State exit;
    [SerializeField] private float floorEnterMessageDuration;
    [SerializeField] private float deathMessageDuration;
    [SerializeField] private float pickleAcquiredMessageDuration;
    [SerializeField] private float thanksMessageDuration;
    [TextArea] [SerializeField] private string pickleAcquiredMessage;
    [TextArea] [SerializeField] private string thanksMessage;
    [TextArea] [SerializeField] private List<string> floorEnterMessages;
    [TextArea] [SerializeField] private List<string> deathMessages;

    private const float WidthMargin = 50f;
    private const float AvgLetterWidth = 9f;
    
    public static MessageManager Instance;

    private Coroutine _currentRoutine;
            

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LevelManager.Instance.CurrentLevelChanged += ShowLevelEnteredMessage;
        CheckpointManager.Instance.ReturnedToCheckpoint += ShowRandomDeathMessage;
        PickleRack.Instance.PickleAcquired += () => ShowMessage(pickleAcquiredMessage, pickleAcquiredMessageDuration);
        PickleRack.Instance.GameEnded += () => ShowMessage(thanksMessage, thanksMessageDuration);
    }

    public void ShowMessage(string text, float time)
    {
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        
        _currentRoutine = StartCoroutine(Routine(enter, exit, text, time));
    }

    private void ShowLevelEnteredMessage(Level level)
    {
        int id = level.Data.Id - 1;
        string text = string.Empty;
        
        if(floorEnterMessages.Count > id)
            text = floorEnterMessages[id];

        if(text != string.Empty)
            ShowMessage(text, floorEnterMessageDuration);
    }

    private void ShowRandomDeathMessage()
    {
        string text = deathMessages.GetRandom();
        ShowMessage(text, deathMessageDuration);
    }

    private IEnumerator Routine(State enter, State exit, string text, float time)
    {
        title.text = text;
        panel.DOMoveY(enter.posY, enter.duration);
        panel.sizeDelta = new Vector2(WidthMargin + text.Length * AvgLetterWidth, panel.sizeDelta.y);

        yield return new WaitForSeconds(time);

        panel.DOMoveY(exit.posY, exit.duration);
    }
}