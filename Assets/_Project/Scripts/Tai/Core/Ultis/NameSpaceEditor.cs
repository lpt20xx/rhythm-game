#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using File = UnityEngine.Windows.File;


public static class NameSpaceEditor
{
    public const string NameSpaceTemplate = "Templates/NameSpace";
    public const string BaseUITemplate = "Templates/BaseUITemplate";

    [MenuItem("Assets/Create/Create New Script", priority = 0)]
    public static void CreateNameSpaceScript()
    {
        var assetTemplate = NameSpaceTemplate;
        CreateFMScript(assetTemplate, "NewScript.cs");
    }

    [MenuItem("Assets/Create/Create/Create Base UI", priority = 0)]
    public static void CreateBaseUIScript()
    {
        var assetTemplate = BaseUITemplate;
        CreateFMScript(assetTemplate, "NewScript.cs");
    }

    static void CreateFMScript(string assetTemplate, string defaultName)
    {
        var path = "";
        var t = Resources.Load(assetTemplate) as TextAsset;

        if (Selection.activeObject != null)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
        }

        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path);
        }

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets/";
        }

        Resources.UnloadAsset(t);
        CreateScriptAsset(AssetDatabase.GetAssetPath(t.GetInstanceID()), GetDestinPath()
                                                                         + "/" + defaultName);
        AssetDatabase.Refresh();
    }

    static string GetDestinPath()
    {
        string path = "Asset";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object),
            SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);

            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            break;
        }

        return path;
    }


#if UNITY_2018_4_OR_NEWER
    static void CreateScriptAsset(string templatePath, string desName)
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, desName);
    }
#endif
}

#endif
