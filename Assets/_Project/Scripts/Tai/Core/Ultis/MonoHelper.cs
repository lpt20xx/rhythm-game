using System.Collections;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public static class MonoHelper
{
    #region Graphics
    /// <summary>
    /// Textures the circle mask.
    /// </summary>
    /// <returns>The circle mask.</returns>
    /// <param name="h">The height.</param>
    /// <param name="w">The width.</param>
    /// <param name="r">The red component.</param>
    /// <param name="cx">Cx.</param>
    /// <param name="cy">Cy.</param>
    /// <param name="sourceTex">Source tex.</param>
    public static Texture2D TextureCircleMask(int h, int w, float r, float cx, float cy, Texture2D sourceTex)
    {
        Color[] c = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
        Texture2D b = new Texture2D(h, w);
        for (int i = 0; i < (h * w); i++)
        {
            int y = Mathf.FloorToInt(((float)i) / ((float)w));
            int x = Mathf.FloorToInt(((float)i - ((float)(y * w))));
            if (r * r >= (x - cx) * (x - cx) + (y - cy) * (y - cy))
            {
                b.SetPixel(x, y, c[i]);
            }
            else
            {
                b.SetPixel(x, y, Color.clear);
            }
        }
        b.Apply();
        return b;
    }
    #endregion

    #region Code Mechanic
    public static IEnumerator DoSomeThing(float seconds, UnityEngine.Events.UnityAction callback)
    {
        yield return new WaitForSeconds(seconds);

        if (callback != null)
            callback.Invoke();
    }

    public static IEnumerator DoSomeThingAfterFrame(int frameWait, UnityEngine.Events.UnityAction callback)
    {
        for (int i = 0; i < frameWait; i++)
            yield return null;

        if (callback != null)
            callback.Invoke();
    }
    #endregion

    public static IEnumerator DownloadTexture(string url, Action<Texture2D> callback)
    {
        // Start a download of the given URL
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        // Wait for download to complete
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            callback(((DownloadHandlerTexture)www.downloadHandler).texture);
        }
    }

    public static IEnumerator Download(string url, Action<bool, object> onResult)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                onResult?.Invoke(false, null);
            }
            else
            {
                onResult?.Invoke(true, webRequest.downloadHandler.data);
            }
        }
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    // First launch
    public static bool IsFirstLaunch(string name)
    {
        return (PlayerPrefs.GetInt(name + "_first_launch_pref_string", 1) == 1) ? true : false;
    }

    public static void SetIsFirstLaunchDone(string name)
    {
        PlayerPrefs.SetInt(name + "_first_launch_pref_string", 0);
    }

    public static IEnumerator DownloadFileCoroutine(string url, Action<string> successCallback, Action<string> failCallback)
    {
        WWW www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            if (failCallback != null)
                failCallback.Invoke(www.error);
        }
        else if (www.bytesDownloaded == 0) // Cheat as error
        {
            if (failCallback != null)
                failCallback.Invoke("(DownloadFileCoroutine) File empty!");
        }
        else
        {
            if (successCallback != null)
                successCallback(www.text);
        }
    }

    public static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static Color ColorFromHexan(string Idcolor)
    {
        Color _color;
        ColorUtility.TryParseHtmlString("#" + Idcolor, out _color);
        return _color;
    }

    //public static IEnumerator HopperDoSomething(float seconds, UnityEngine.Events.UnityAction callback)
    //{
    //    float _elapsedTime = 0f;
    //    while (true)
    //    {
    //        while (GameStatics.playStatus != PlayStatus.Playing)
    //        {
    //            yield return new WaitForSeconds(GameConstants.PAUSE_DELAY);
    //        }

    //        _elapsedTime += Time.deltaTime;
    //        if (_elapsedTime >= seconds)
    //        {
    //            if(callback != null)
    //            {
    //                callback.Invoke();
    //            }

    //            break;
    //        }

    //        yield return 0;
    //    }

    //    yield return null;
    //}
}