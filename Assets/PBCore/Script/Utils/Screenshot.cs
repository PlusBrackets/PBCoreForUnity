using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBCore
{
    /// <summary>
    /// 屏幕截图工具
    /// </summary>
    public static class Screenshot
    {
        /// <summary>
        /// 从Camera中截取
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="rect">截取的区域</param>
        /// <param name="clarity">清晰度[0.01,1]</param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Texture2D FromCamera(Camera camera, Rect rect, float clarity = 1, TextureFormat textureFormat = TextureFormat.RGB24, bool mipmap = false, bool linear = false, int depth = 16)
        {
            return FromCamera(new Camera[] { camera }, rect, clarity, textureFormat, mipmap, linear, depth);
        }

        /// <summary>
        /// 从多个camera中截取
        /// </summary>
        /// <param name="cameras"></param>
        /// <param name="rect">截取的区域</param>
        /// <param name="clarity">清晰度[0.01,1]</param>
        /// <param name="textureFormat"></param>
        /// <param name="mipmap"></param>
        /// <param name="linear"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Texture2D FromCamera(Camera[] cameras, Rect rect, float clarity = 1, TextureFormat textureFormat = TextureFormat.RGB24, bool mipmap = false, bool linear = false, int depth = 16)
        {
            clarity = Mathf.Clamp(clarity, 0.01f, 1f);
            rect.x *= clarity;
            rect.y *= clarity;
            rect.width *= clarity;
            rect.height *= clarity;
            RenderTexture rt = new RenderTexture((int)(Screen.width * clarity), (int)(Screen.height * clarity), depth);
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].targetTexture = rt;
                cameras[i].Render();
                cameras[i].targetTexture = null;
            }
            RenderTexture temp = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D screenshot = new Texture2D((int)rect.width, (int)rect.height, textureFormat, mipmap, linear);
            screenshot.ReadPixels(rect, 0, 0);
            screenshot.Apply();
            RenderTexture.active = temp;
            Object.Destroy(rt);
            return screenshot;
        }
    }
}