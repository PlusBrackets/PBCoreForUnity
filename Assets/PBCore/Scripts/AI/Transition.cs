using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore.AI
{

    [System.Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        [Range(0, 100)]
        public int trueWeight = 50;
        public State falseState;
        [Range(0, 100)]
        public int falseWeight = 0;
    }
}
