using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBCore.UI
{

    public interface IUIState
    {
        void OnUIOpen();
        void OnUIOpenEnd();
        void OnUIClose();
        void OnUICloseEnd();
    }
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract class BaseUI : MonoBehaviour
    {

        // public bool defaultOpen = false;
        private bool _opened = false;
        /// <summary>
        /// 是否已经完全打开
        /// </summary>
        public bool isOpened
        {
            get
            {
                return _opened;
            }
            protected set
            {
                _opened = value;
            }
        }
        private bool _activeUI = false;
        /// <summary>
        /// UI是否处于活跃状态
        /// </summary>
        public bool isUiActive
        {
            get
            {
                return _activeUI;
            }
            protected set
            {
                _activeUI = value;
            }
        }
        /// <summary>
        /// 是否在UI栈中
        /// </summary>
        public bool isInStatck
        {
            get
            {
                return UIManager.Ins.IsInStack(uiID);
            }
        }

        public bool isRegister
        {
            get
            {
                return UIManager.Ins.IsRegister(uiID);
            }
        }

        public object uiID { get; set; }

        [HideInInspector]
        public UiType uiType;
        [Tooltip("控制UI开关待机的动画,Trigger params { Opne, Close, Idle }")]
        public Animator uiAnimator;
        [Tooltip("ui打开时首次选择的object")]
        public GameObject firstSelected;

        protected virtual void Awake()
        {
            if (uiAnimator == null)
            {
                uiAnimator = GetComponent<Animator>();
            }
        }

        /// <summary>
        /// 注册进UIManager时调用
        /// </summary>
        public abstract void OnRegister();
        /// <summary>
        /// 注销时调用
        /// </summary>
        public abstract void OnUnRegister();

        /// <summary>
        /// 进入活跃状态
        /// </summary>
        public virtual void InActive()
        {
            isUiActive = true;
        }

        /// <summary>
        /// 退出活跃状态
        /// </summary>
        public virtual void OutActive()
        {
            isUiActive = false;
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="args"></param>
        public void Open(bool useAnim = true)
        {
            if (!gameObject.activeInHierarchy)
            {
                //if (firstSelected != null)
                if(EventSystem.current!=null)
                    EventSystem.current.SetSelectedGameObject(null);
                //打开UI
                gameObject.SetActive(true);
                OnOpen();
                BroadcastMessage("OnUIOpen",SendMessageOptions.DontRequireReceiver);
                if (useAnim && uiAnimator != null)
                {
                    uiAnimator.SetTrigger("Open");
                }
                else
                {
                    EndOpen();
                }
            }
        }
        
        // 等待开启结束
        protected void EndOpen()
        {
            OnOpenEnd();
            BroadcastMessage("OnUIOpenEnd", SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="args"></param>
        public void Close(bool useAnim = true)
        {
            if (gameObject.activeInHierarchy)
            {
                isOpened = false;
                OnClose();
                BroadcastMessage("OnUIClose", SendMessageOptions.DontRequireReceiver);
                if (useAnim && uiAnimator != null)
                {
                    uiAnimator.SetTrigger("Close");
                }
                else
                {
                    EndClose();
                }
            }
        }

        protected void EndClose()
        {
            OnCloseEnd();
            BroadcastMessage("OnUICloseEnd", SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 调用Open时调用
        /// </summary>
        /// <param name="args"></param>
        protected abstract void OnOpen();

        /// <summary>
        /// 开启完成后调用
        /// </summary>
        protected virtual void OnOpenEnd()
        {
            isOpened = true;
            //设置首选
            if (firstSelected != null && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
        }

        /// <summary>
        /// 窗口关闭时调用
        /// </summary>
        /// <param name="args"></param>
        protected abstract void OnClose();

        /// <summary>
        /// 最终关闭完成时调用
        /// </summary>
        protected virtual void OnCloseEnd()
        {

        }

        /// <summary>
        /// UI动画回调
        /// </summary>
        /// <param name="message"></param>
        protected virtual void UIAnimMessage(string message)
        {
            switch (message)
            {
                case "OpenEnd":
                    EndOpen();
                    break;
                case "CloseEnd":
                    EndClose();
                    break;
            }
        }

        /// <summary>
        /// 设置当前UI下的所有SelectabeItem是否可以互动
        /// </summary>
        /// <param name="flag"></param>
        public void SetAllSelectableInteract(bool flag)
        {
            Selectable[] selectables = GetComponentsInChildren<Selectable>();
            if (selectables.Length > 0)
            {
                foreach (Selectable s in selectables)
                {
                    s.interactable = flag;
                }
            }
        }
        public void SetSelectableListInteract(Selectable[] list, bool flag)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    list[i].interactable = flag;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            if (UIManager.IsIns)
            {
                UIManager.Ins.UnRegisterUI(uiID);
            }
        }

        public enum UiType
        {
            /// <summary>
            /// 游戏内UI，不会获得foucs，不被其他UI打开关闭所影响
            /// </summary>
            GAME,
            /// <summary>
            /// 普通窗口UI
            /// </summary>
            PANEL,
            /// <summary>
            /// 叠加UI，打开时不关闭原本Panel
            /// </summary>
            POP,
            /// <summary>
            /// 窗口UI，打开时会入栈并关闭上一个入栈的UI
            /// </summary>
            PANEL_STACK,
            /// <summary>
            /// 开启后保持开启
            /// </summary>
            AWAYS_KEEP,
        }
    }
}
