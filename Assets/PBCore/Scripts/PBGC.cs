using PBCore.Event;
using PBCore.Localization;
using PBCore.UI;
//using PBCore.Audio;
using PBCore.Utils;
/// <summary>
/// 功能管理
/// </summary>
public static class PBGC
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public static EventManager EventManager
    {
        get
        {
            return EventManager.Ins;
        }
    }

    /// <summary>
    /// UI管理器
    /// </summary>
    public static UIManager UIManager
    {
        get
        {
            return UIManager.Ins;
        }
    }

    ///// <summary>
    ///// 本地化管理器
    ///// </summary>
    //[System.Obsolete]
    //public static LocalizationManagerOb LocalizationManagerOb
    //{
    //    get
    //    {
    //        return LocalizationManagerOb.Ins;
    //    }
    //}

    public static LocalizationManager LocalizationManager{
        get
        {
            return LocalizationManager.Ins;
        }
     }

    /// <summary>
    /// 文本替换器
    /// </summary>
    public static TextReplacer TextRelacer
    {
        get
        {
            return TextReplacer.Ins;
        }
    }

    ///// <summary>
    ///// 声音播放器
    ///// </summary>
    //public static AudioPlayer AudioPlayer
    //{
    //    get
    //    {
    //        return AudioPlayer.Ins;
    //    }
    //}
    
}
