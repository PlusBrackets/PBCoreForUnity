using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBCore;
using PBCore.Utils;
using System;
using PBCore.Aid;

namespace PBCore.UI
{

    public class UIManager : SingleClass<UIManager>
    {
        readonly Dictionary<object, BaseUI> _dictionary = new Dictionary<object, BaseUI>();
        readonly List<object> uiStack = new List<object>();
        private UnscaleTimeCounter backspaceUICoolTime = new UnscaleTimeCounter();

        /// <summary>
        /// 当前uiStack中有东西
        /// </summary>
        public bool hasStack
        {
            get
            {
                return uiStack.Count > 0;
            }
        }

        /// <summary>
        /// 注册UI
        /// </summary>
        /// <param name="uiId"></param>
        /// <param name="ui"></param>
        public void RegisterUI(object uiId, BaseUI ui, BaseUI.UiType type)//,BaseUI.Type type)
        {
            if (_dictionary.ContainsKey(uiId))
            {
                Debug.Log(uiId + " 已有相同ID注册");
                return;
            }
            else
            {
                _dictionary.Add(uiId, ui);
            }
            ui.uiID = uiId;
            ui.uiType = type;
            ui.OnRegister();
            ui.gameObject.SetActive(false);
        }

        public bool IsRegister(object uiId)
        {
            return _dictionary.ContainsKey(uiId);
        }

        /// <summary>
        /// 注销并销毁UI
        /// </summary>
        /// <param name="uiId"></param>
        public void UnRegisterUI(object uiId)
        {
            BaseUI ui = CloseUI(uiId);
            if (ui!=null)
            {
                ui.OnUnRegister();
                if (uiStack.Contains(uiId))
                {
                    uiStack.Remove(uiId);
                }
                _dictionary.Remove(uiId);
                GameObject.Destroy(ui.gameObject, 0.1f);
            }
        }

        /// <summary>
        /// 注销所有UI并清除UI池
        /// </summary>
        public void UnRegisterAll()
        {
            //UIList.Clear();
            Dictionary<object, BaseUI>.KeyCollection keys = _dictionary.Keys;
            foreach (string key in keys)
            {
                UnRegisterUI(key);
            }
        }

        /// <summary>
        /// 关闭所有UI
        /// </summary>
        public void CloseAll(bool useAnim = true)
        {
            CloseUIStack();
            foreach (KeyValuePair<object, BaseUI> obj in _dictionary)
            {
                if (obj.Value.gameObject.activeInHierarchy)
                    CloseUI(obj.Key, useAnim);
            }
        }

        /// <summary>
        /// 关闭一个类型的UI
        /// </summary>
        public void CloseAllType(BaseUI.UiType type,bool useAnim = true)
        {
            if(type == BaseUI.UiType.PANEL_STACK)
            {
                CloseUIStack();
            }
            foreach (KeyValuePair<object, BaseUI> obj in _dictionary)
            {
                if (obj.Value.uiType == type)
                {
                    CloseUI(obj.Key, useAnim);
                }
            }
        }

        /// <summary>
        /// 依次关闭栈中的UI
        /// </summary>
        public void CloseUIStack()
        {
            for(int i = 0; i < uiStack.Count; i++)
            {
                BackspaceUI(false);
            }
        }

        /// <summary>
        /// 关闭一个UI
        /// </summary>
        /// <param name="uiId"></param>
        public BaseUI CloseUI(object uiId, bool useAnim = true)
        {
            BaseUI ui = GetUI(uiId);
            if (ui == null)
            {
                Debug.Log(uiId + "未注册");
            }
            else
            {
                if (uiStack.Contains(uiId))
                {
                    if(uiStack[uiStack.Count - 1] == uiId)
                    {
                        BackspaceUI(useAnim);
                    }
                    //else
                    //{
                    //    Debug.Log(uiId + "在栈中且不在栈顶");
                    //}
                }
                else
                {
                    CloseAUI(ui, useAnim);
                }
            }
            return ui;
        }

        ///// <summary>
        ///// 关闭一个UI
        ///// </summary>
        ///// <param name="uiId"></param>
        //public BaseUI CloseUI(System.Enum uiId, bool useAnim = true)
        //{
        //    return CloseUI(uiId.ToString(), useAnim);
        //}

        /// <summary>
        /// 关闭一个UI
        /// </summary>
        /// <param name="uiName"></param>
        public BaseUI CloseUI(BaseUI ui, bool useAnim = true)
        {
            return CloseUI(ui.uiID, useAnim);
        }

        /// <summary>
        /// 打开一个UI
        /// </summary>
        /// <param name="uiId"></param>
        /// <param name="useAnim"></param>
        /// <param name="closeCurrentStackUI">是否关闭上一个入栈的UI</param>
        /// <returns></returns>
        public BaseUI OpenUI(object uiId, bool useAnim = true, bool closeCurrentStackUI = true)
        {
            BaseUI ui = GetUI(uiId);
            if (ui == null)
            {
                Debug.Log(uiId + "UI未注册");
            }
            else
            {
                if (ui.uiType == BaseUI.UiType.PANEL_STACK)
                {
                    if (uiStack.Count > 0)
                    {
                        object preUiId = PopStack();
                        BaseUI preUi = GetUI(preUiId);
                        if (preUi != null)
                        {
                            PushStack(preUiId);
                            //关闭上一个入栈的UI
                            if (closeCurrentStackUI)
                                CloseAUI(preUi, useAnim);
                            else
                                preUi.OutActive();
                        }
                    }
                    //入栈一个Panel_Stack类型的UI
                    PushStack(uiId);
                }
                OpenAUI(ui, useAnim);
            }
            return ui;
        }

        /// <summary>
        /// 回退一个Panel_Stack的UI
        /// </summary>
        /// <param name="useAnim"></param>
        /// <param name="coolTime">回退的冷却时间</param>
        public void BackspaceUI(bool useAnim = true, float coolTime = 0)
        {
            if (backspaceUICoolTime.Tick())
            {
                backspaceUICoolTime.Counter(coolTime);
                object uiId = PopStack();
                BaseUI cui = GetUI(uiId);
                CloseAUI(cui, useAnim);
                uiId = PopStack();
                BaseUI ui = GetUI(uiId);
                if (ui != null)
                {
                    OpenAUI(ui, useAnim);
                    PushStack(uiId);
                }
            }
        }

        ///// <summary>
        ///// 打开一个UI
        ///// </summary>
        ///// <param name="uiId"></param>
        //public BaseUI OpenUI(System.Enum uiId,bool useAnim = true)
        //{
        //    return OpenUI(uiId.ToString(),useAnim);
        //}

        /// <summary>
        /// 打开一个UI
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="useAnim"></param>
        /// <returns></returns>
        public BaseUI OpenUI(BaseUI ui,bool useAnim = true)
        {
            return OpenUI(ui.uiID, useAnim);
        }
        
        private void OpenAUI(BaseUI ui,bool useAnim)
        {
            ui.Open(useAnim);
            ui.InActive();
        }

        private void CloseAUI(BaseUI ui,bool useAnim)
        {
            ui.OutActive();
            ui.Close(useAnim);
        }

        /// <summary>
        /// 根据Id取得UI
        /// </summary>
        /// <param name="uiId"></param>
        /// <returns></returns>
        public BaseUI GetUI(object uiId)
        {
            if (uiId!=null&&_dictionary.ContainsKey(uiId))
            {
                return _dictionary[uiId];
            }
            else
            {
                //Debug.Log("Dont found UI:" + uiId);
                return null;
            }
        }

        ///// <summary>
        ///// 根据Id取得UI
        ///// </summary>
        ///// <param name="uiId"></param>
        ///// <returns></returns>
        //public BaseUI GetUI(System.Enum uiId)
        //{
        //    return GetUI(uiId.ToString());
        //}

        /// <summary>
        /// 判断一个注册ui是否开启
        /// </summary>
        /// <param name="uiId"></param>
        /// <returns></returns>
        public bool IsOpen(object uiId)
        {
            BaseUI ui = GetUI(uiId);
            if (ui != null)
            {
                return ui.gameObject.activeInHierarchy;
            }
            else
                return false;
        }

        ///// <summary>
        ///// 判断一个注册ui是否开启
        ///// </summary>
        ///// <param name="uiId"></param>
        ///// <returns></returns>
        //public bool IsOpen(System.Enum uiId)
        //{
        //    return IsOpen(uiId.ToString());
        //}

        /// <summary>
        /// 某一个type的ui是否在Open
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool TypeHasOpen(params BaseUI.UiType[] types)
        {
            Dictionary<object, BaseUI>.ValueCollection values = _dictionary.Values;
            foreach (BaseUI ui in values)
            {
                if(Array.BinarySearch(types, ui.uiType) >= 0 && ui.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            return false;
        }

        ///// <summary>
        ///// 是否在ui栈里
        ///// </summary>
        ///// <param name="uiId"></param>
        ///// <returns></returns>
        //public bool IsInStack(System.Enum uiId)
        //{
        //    return IsInStack(uiId.ToString());
        //}

        /// <summary>
        /// 是否在ui栈里
        /// </summary>
        /// <param name="uiId"></param>
        /// <returns></returns>
        public bool IsInStack(object uiId)
        {
            return uiStack.Contains(uiId);
        }
        
        //入栈
        private void PushStack(object uiId)
        {
            if (uiStack.Contains(uiId))
                uiStack.Remove(uiId);
            uiStack.Add(uiId);
        }

        //出栈
        private object PopStack()
        {
            object item = null;
            if (uiStack.Count > 0)
            {
                item = uiStack[uiStack.Count - 1];
                uiStack.Remove(item);
            }
            return item;
        }
    }
}
