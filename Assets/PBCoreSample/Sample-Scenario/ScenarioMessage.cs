using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Scenario;
using PBCore.Event;

public class ScenarioMessage : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddListener<MessageCommand.EventMessage>(OnCommandMessage);
        EventManager.AddListener<MessageAction.EventMessage>(OnSelectionMessage);
    }

    private void OnCommandMessage(MessageCommand.EventMessage e)
    {
        Debug.Log("Command: " + e.message);
    }

    private void OnSelectionMessage(MessageAction.EventMessage e)
    {
        Debug.Log("Selection: " + e.message);
    }

}
