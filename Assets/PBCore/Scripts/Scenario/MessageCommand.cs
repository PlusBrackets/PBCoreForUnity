using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Event;

namespace PBCore.Scenario
{
    
    /// <summary>
    /// 发送信息的对话指令
    /// </summary>
    [CreateAssetMenu(menuName ="PBCore/Scenario/Command/MessageCommand",fileName ="MessageCommand")]
    public class MessageCommand : ScenarioCommand
    {
        public class EventMessage : EventArgs
        {
            public string message;
            public EventMessage(string message) { this.message = message; }
        }

        public override IEnumerator DoCommand(string message)
        {
            EventManager.Dispatch(new EventMessage(message));
            yield break ;
        }

        public override void EndCommand(string message)
        {
            
        }
    }
}