using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Assets.Editor
{
    class ExportAB
    {
        public enum E_ABBUNLDE_TYPE
        {
            E_SHADER,
            E_TEXTURE,
            E_EFFECT,
            E_SHARED, //把冗余的asset打成公用的bundle
            E_GlobalCommon, //临时用的，放跨type的公用asset

            E_UI,
            E_BT,

            E_SCENECOMMON,
            E_SCENE,
            E_CHARACTER,
            E_ACTION,
            E_ACTORINFO,
            E_GAMEDATA,
            E_FONT,
            E_SOUND,
            E_WEAPON,
            E_GEM,

            E_ILLEGAL,
        };

        private static string msSceneLocation = "Assets/Scenes/";
        //static List<string> msSceneList = new List<string>() { "puzzle1.unity", "puzzle2.unity", "puzzle3.unity" };
        //static List<string> msSceneList = new List<string>() { "BattleSmall.unity", "BornVillage.unity"};
        static string[] msSceneList = { "Assets/Scenes/BattleSmall.unity", "Assets/Scenes/BornVillage.unity" };
        public static string AB_PATH = "Assets/StreamingAssets/AssetBundle/";
        //private static Dictionary<E_ABBUNLDE_TYPE, List<AssetBundleBuild>> mAssetBundleMap = new Dictionary<E_ABBUNLDE_TYPE, List<AssetBundleBuildEX>>();

        //static void ExportResourceRGB2()
        //{
        //    string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        //    if (path.Length != 0)
        //    {
        //        // 选择的要保存的对象  
        //        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        //        //打包  
        //        //BuildPipeline.BuildAssetBundles(AB_PATH, "", BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        //    }
        //}
        //[MenuItem("Custom/Build All LZ4(Unity 5)")]
        static string BuildAll5LZ4()
        {
            return BuildAll5WithOption(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression);
        }

        static string BuildAll5WithOption(BuildAssetBundleOptions opt)
        {
            //EditorUtility.DisplayProgressBar("Build All", "Init All New Hero.....", 0);
            ////bOldBuildMethod = false;
            ////AB_SKIP_BUILD = false;
            ////AB_SKIP_COOK_AGE = false;
            ////InitAll();
            //EditorUtility.DisplayProgressBar("Build All", "Build puzzle1 scene.....", 0.15f);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //AB_AssetBuildMgr.Clear();
            ////AB_ShaderList.BuildShader();
            //AB_ShaderBuild.BuildShader();
            //AB_SharedRes.BuildSharedRes();
            //AB_GameDataBuild.BuildGameData();

            ////BuildScene("puzzle1");
            //watch.Stop();
            //Debug.Log("Pass Time puzzle1 : " + watch.ElapsedMilliseconds);
            //EditorUtility.DisplayProgressBar("Build All", "Build puzzle2.....", 0.3f);

            //watch.Reset();
            //watch.Start();
            ////AB_HeroBuildMgr.BuildHero();
            ////BuildScene("puzzle2");
            //watch.Stop();
            //Debug.Log("Pass Time puzzle2: " + watch.ElapsedMilliseconds);
            //EditorUtility.DisplayProgressBar("Build All", "Build Scene.....", 0.45f);

            watch.Reset();
            watch.Start();
            //AB_SceneBuild.BuildScene();
            BuildScene();
            watch.Stop();
            Debug.Log("Pass Time Scene: " + watch.ElapsedMilliseconds);

            //watch.Reset();
            //watch.Start();
            ////AB_UIList.BuildUI();
            //AB_UIBuild.BuildUI();
            ////remove sound from ab 
            ////AB_SoundBuild.BuildSound();
            //watch.Stop();
            //Debug.Log("Pass Time UI: " + watch.ElapsedMilliseconds);
            //EditorUtility.DisplayProgressBar("Build All", "Gather Info.....", 0.75f);

            //watch.Reset();
            //watch.Start();
            //AB_GatherResInfo.GatherResInfo();
            //watch.Stop();
            //Debug.Log("Pass Time GatherResInfo: " + watch.ElapsedMilliseconds);
            //AB_Encrypt.DeleteEncryptBundleFile();
            //AssetDatabase.Refresh();
            //BuildPipeline.BuildAssetBundles(AB_PATH, AB_AssetBuildMgr.Parse(), opt, BUILD_TARGET);
            //AB_GatherResInfo.PostBuild();
            //AB_AssetBuildMgr.Write();
            ////Debug.Break();
            //AB_Encrypt.EncryptFile();

            //EditorUtility.ClearProgressBar();
            //AB_Analyze.CheckAB();
            //if (AB_Analyze.mErrorMessageList.Count > 0)
            //{
            //    EditorUtility.DisplayDialog("Error", "AB存在相互依赖", "OK");
            //    return "Error";
            //}
            //else
            //{
            //    EditorUtility.DisplayDialog("Build All", "Build Complete!!!!" + AB_GatherResInfo.GetPublish(), "OK");
            //    return AB_GatherResInfo.GetPublish();
            //}
            return "";
        }
        [@MenuItem("AssetBundles/Build Scenes")]
        static void BuildScene()
        {
            foreach (string strScene in msSceneList)
            {
                // Create the array of bundle build details.
                //AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

                //buildMap[0].assetBundleName = strScene;//打包的资源包名称 随便命名
                //string[] resourcesAssets = new string[1];//此资源包下面有多少文件
                //resourcesAssets[0] = "Scenes/" + strScene;
                //buildMap[0].assetNames = resourcesAssets;

                //BuildPipeline.BuildAssetBundles("Assets/streamingAssets", buildMap, BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);

                UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Scenes/" + strScene);
                //BuildPipeline.BuildAssetBundle(obj, null,
                //                                  Application.streamingAssetsPath + "/" + strScene + ".assetbundle",
                //                                 BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets
                //                                 | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
                BuildPipeline.BuildStreamedSceneAssetBundle(msSceneList, AB_PATH + strScene + ".unity3d", 
                    BuildTarget.StandaloneWindows, BuildOptions.CompressWithLz4);

            }
            //BuildPipeline.BuildStreamedSceneAssetBundle(msSceneList, AB_PATH, BuildTarget.StandaloneWindows, BuildOptions.CompressWithLz4);
        }

        //public static AssetBundleBuild[] Parse()
        //{
        //    //AutoCollectRedundancyData();
        //    int iCount = Count();
        //    AssetBundleBuild[] abArray = new AssetBundleBuild[iCount];
        //    int i = 0;
        //    foreach (List<AssetBundleBuild> abList in mAssetBundleMap.Values)
        //    {
        //        foreach (AssetBundleBuild ab in abList)
        //        {
        //            abArray[i] = ab;
        //            i++;
        //        }
        //    }
        //    return abArray;
        //}

        //static int Count()
        //{
        //    int iCount = 0;
        //    foreach (List<AssetBundleBuild> abList in mAssetBundleMap.Values)
        //    {
        //        iCount += abList.Count;
        //    }
        //    return iCount;
        //}
    }


}
