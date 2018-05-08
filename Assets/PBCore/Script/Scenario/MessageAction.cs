using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore.Event;
using System;

namespace PBCore.Scenario
{
    /// <summary>
    /// 发送信息的选项动作
    /// </summary>
    [CreateAssetMenu(menuName = "PBCore/Scenario/Action/MessageAction", fileName = "MessageAction")]
    public class MessageAction : ScenarioAction
    {
        public class EventMessage : EventArgs
        {
            public string message;
            public EventMessage(string message) { this.message = message; }
        }
        public string message;

        public override void DoAction()
        {
            EventManager.Dispatch(new EventMessage(message));
        }
    }
}