using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Scenario;
using UnityEngine.UI;

public class ScenarioTest : MonoBehaviour
{

    public ScenarioData playScenario;

    public Image[] charaImg;
    public Text speakerName;
    public Text dialogue;
    public GameObject dialoguePanel;
    public GameObject selectionPanel;
    public Button[] selectionBtns;
    public Text[] selectionTexts;
    ReadData readData = new ReadData();
    private bool waiting = true;

    public enum State
    {
        Close,
        Dialogue,
        WaitToSeletion,
        Seletion
    }
    public State state = State.Close;

    private void Awake()
    {
        SetBtns(0);
        CloseUI();
        //selectionPanel.SetActive(false);
    }

    public void OnSelectionClick(int index)
    {
        if (ScenarioReader.Next(index, ref readData))
        {
            UpdateUI();
        }
        else
        {
            CloseUI();
        }
    }

    private void SetBtns(int num)
    {
        for (int i = 0; i < selectionBtns.Length; i++)
        {
            if (i < num)
            {
                selectionBtns[i].gameObject.SetActive(true);
            }
            else
                selectionBtns[i].gameObject.SetActive(false);
        }
    }

    public void PlayScenario()
    {
        if (ScenarioReader.Read(playScenario, ref readData))
        {
            OpenUI();
            UpdateUI();
        }
        else
        {
            CloseUI();
        }
    }

    private void OpenUI()
    {
        dialoguePanel.SetActive(true);
        selectionPanel.SetActive(true);
    }

    private void CloseUI()
    {
        state = State.Close;
        dialoguePanel.SetActive(false);
        selectionPanel.SetActive(false);
    }

    private void UpdateUI()
    {
        StartCoroutine(WaitReleaseWaiting());
        selectionPanel.SetActive(false);
        speakerName.text = null;
        for (int i = 0; i < readData.charaName.Length; i++)
        {
            if (readData.charaIsSpeaker[i])
            {
                speakerName.text = readData.charaName[i];
                break;
            }
        }
        for(int i = 0; i < charaImg.Length; i++)
        {
            SetCharaImage(charaImg[i], null, false);
        }
        for(int i = 0; i < readData.charaPortarit.Length&&i<charaImg.Length; i++)
        {
            SetCharaImage(charaImg[i], readData.charaPortarit[i], readData.charaIsSpeaker[i]);
        }
        dialogue.text = readData.dialogueText;
        SetBtns(readData.selectionText.Length);
        if (readData.selectionText.Length > 0)
        {
            for (int i = 0; i < selectionTexts.Length && i < readData.selectionText.Length; i++)
            {
                selectionTexts[i].text = readData.selectionText[i];
            }
            state = State.Seletion;
        }
        else
        {
            state = State.Dialogue;
        }
    }
    
    private void ShowSelectionPanel()
    {
        selectionPanel.SetActive(true);
        state = State.WaitToSeletion;
    }

    private void Update()
    {
        if (!waiting&&(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            switch (state)
            {
                case State.Dialogue:
                    if (ScenarioReader.Next(ref readData))
                    {
                        UpdateUI();
                    }
                    else
                    {
                        CloseUI();
                    }
                    break;
                case State.Seletion:
                    ShowSelectionPanel();
                    break;
                case State.WaitToSeletion:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayScenario();
        }
    }

    IEnumerator WaitReleaseWaiting()
    {
        waiting = true;
        yield return new WaitForSeconds(0.2f);
        waiting = false;
    }

    private void SetCharaImage(Image img,Sprite chara,bool speaker)
    {
        img.sprite = chara;
        if(chara == null)
        {
            img.color = Color.clear;
        }
        else
        {
            if (speaker)
                img.color = Color.white;
            else
                img.color = Color.gray;
        }
    }
}
