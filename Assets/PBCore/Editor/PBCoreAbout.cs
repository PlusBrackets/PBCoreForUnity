using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PBCore.CEditor
{

    public class PBCoreAbout : EditorWindow
    {
        private const string ver = "v0.6.9";
        private static Rect windowRect = new Rect(500, 400, 400, 280);

        [MenuItem("PBCore/About",false,20000)]
        static void ShowAboutWin()
        {
            PBCoreAbout win = (PBCoreAbout)EditorWindow.GetWindowWithRect(typeof(PBCoreAbout), windowRect, true, "Plus Brackets Core "+ver);
            win.Show();
        }

        private void OnGUI()
        {
            EditorGUI.LabelField(new Rect(windowRect.width/2-130,10,260,20),"A simple framework to make a simple game");

            

            EditorGUI.LabelField(new Rect(windowRect.width - 110, windowRect.height -40, 150, 20), "plusBracket/星月");
            EditorGUI.SelectableLabel(new Rect(windowRect.width - 165, windowRect.height - 25, 165, 20), "liu997139467@Gmail.com");
        }
    }
}