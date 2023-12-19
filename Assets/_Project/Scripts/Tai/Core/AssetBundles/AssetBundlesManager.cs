using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tai_Core;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundlesManager : SingletonMono<AssetBundlesManager>
{
    string LocalPath
    {
        get
        {
            return "AssetBundles";
        }
    }

    public bool IsDownloading
    {
        get => isDownloading;
        set => isDownloading = value;
    }

    private bool isDownloading;

    public event Action onStartDownload;

    public Dictionary<string, AssetBundleItem> songDatabase = new Dictionary<string, AssetBundleItem>();
    // Start is called before the first frame update
    void Start()
    {
        DownloadAsset_WWW("https://github.com/hoanghiep258/AmongUs/raw/dev_Hiep_TestAssetBundle/Assets/StreamingAssets/Android/inst-ballistic",
            "/inst-ballistic", () =>
            {
                Debug.Log("Success download bundle from github");
                StartCoroutine(GetSongFromBundle("/inst-ballistic", "inst-ballistic"));
            }, () =>
            {
                Debug.Log("Failed download bundle from github");
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoteConfigUpdate()
    {
        // Get config asset bundle into song database
    }

    private IEnumerator Download(string url, Action<bool, object> onResult)
    {
        IsDownloading = true;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            AsyncOperation request = webRequest.SendWebRequest();
            while (!request.isDone)
            {
                // Show UI Loading
                yield return null;
            }

            if(webRequest.result == UnityWebRequest.Result.ConnectionError ||
               webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Failed Reason: " + webRequest.result);
                onResult?.Invoke(false, null);
            }
            else
            {
                onResult?.Invoke(true, webRequest.downloadHandler.data);
            }
        }
    }

    public void DownloadAsset_WWW(string url, string path, Action onSuccess, Action onFail, bool isSaveLocal = true) 
    {
        path = path.ToLower().Trim();
        // Show UI Loading
        string filePath = FileHelper.GetWritablePath(LocalPath) + path;
        Debug.Log("File path: " + filePath);
        if (File.Exists(filePath))
        {
            onSuccess();
        }
        else
        {
            Action<bool, object> func = (success, data) =>
            {
                if (success)
                {
                    Debug.Log("Asset Bundle download Success");
                    if (isSaveLocal)
                    {
                        try
                        {
                            FileHelper.SaveFile((byte[])data, filePath, true);
                            Debug.Log("Save ok " + filePath);
                            onSuccess?.Invoke();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Failed to download asset bundle");
                        }
                    }
                }
                else
                {
                    onFail?.Invoke();
                }
            };

            StartCoroutine(Download(url, (suc, dt) =>
            {
                func.Invoke(suc, dt);
            }));

            onStartDownload?.Invoke();
        }
    }

    public void StartDownloadAssetBundle(string nameBundle, Action finish)
    {
        string bundleURL = songDatabase[nameBundle].androidURL;
#if UNITY_IOS
        bundleURL = songDatabase[nameBundle].iosURL;
#endif
        DownloadAsset_WWW(bundleURL, "/" + nameBundle, finish, () =>
        {
            Debug.LogError("Download Asset Bundle Failed");
        });
    }

    public IEnumerator GetSongFromBundle(string path, string objectNameToLoad)
    {
        string filePath = FileHelper.GetWritablePath(LocalPath) + path;
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);

        yield return assetBundleCreateRequest != null;

        var assetBundle = assetBundleCreateRequest.assetBundle;
        var asset = assetBundle.LoadAssetAsync<AudioClip>(objectNameToLoad);
        yield return asset != null;
        float value = 0;
        if (!isDownloading)
        {
            DOTween.To(() => value, x => value = x, 1, 1).OnUpdate(() =>
            {
                // Show Text UI Loading
            });

            yield return new WaitForSeconds(1);
        }

        Tai_SoundManager.Instance.AddSoundBGM(asset.asset as AudioClip);

        Tai_SoundManager.Instance.StopAllSoundFX();
        Tai_SoundManager.Instance.PlaySoundBGM();

        assetBundle.Unload(false);
    }
}

[Serializable]
public class AssetBundleItem
{
    public string name;
    public string androidURL;
    public string iosURL;

    public AssetBundleItem(string name, string androidURL, string iosURL)
    {
        this.name = name;
        this.androidURL = androidURL;
        this.iosURL = iosURL;
    }
}
