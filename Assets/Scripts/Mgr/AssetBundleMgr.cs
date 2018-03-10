using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Mgr
{
    public enum eFileOperation
    {
        ReadFile,
        WriteFile,
        DeleteFile,
        CreateDirectory,
        DeleteDirectory
    }

    class AssetBundlMgr : Singleton<AssetBundlMgr>
    {
        public delegate void DelegateOnOperateFileFail(string fullPath, eFileOperation fileOperation);

        public static DelegateOnOperateFileFail s_delegateOnOperateFileFail = delegate { };

        public void DoLoad(string ifsExtractPath, string pathInIFS)
        {
            AssetBundle resBundle = null;
            string text = CombinePath(ifsExtractPath, pathInIFS);
            if (!System.IO.File.Exists(text))
            {
                DebugHelper.Log("File " + text + " can not be found!!!");
                return;
            }

            int num = 0;
            while (true)
            {
                try
                {
                    //if (HasFlag(eBundleFlag.EncryptBundle))
                    //{
                    //    byte[] rawdata = CFileManager.ReadFile(text);
                    //    byte[] decodeBytes = CFileManager.AESDecrypt(rawdata, "forthelichking!!");
                    //    if (decodeBytes != null)
                    //    {
                    //        resBundle = AssetBundle.LoadFromMemory(decodeBytes);
                    //    }
                    //    else
                    //    {
                    //        DebugHelper.LogFile("AES Error: " + text);
                    //    }
                    //}
                    //else
                    {
                        resBundle = AssetBundle.LoadFromFile(text);
                    }
                }
                catch (System.Exception e)
                {
                    DebugHelper.Log(string.Format(@"{0} => failed DoLoad: {1}", pathInIFS, e.ToString()));
                    resBundle = null;
                }
                if (resBundle != null)
                {
                    break;
                }
                num++;
                DebugHelper.Log(string.Concat(new object[]
                {
                "Create AssetBundle ",
                text,
                " From File Error! Try Count = ",
                num
                }));
                if (num >= 3)
                {
                    s_delegateOnOperateFileFail(text, eFileOperation.ReadFile);
                    break;
                }
            }
        }

        public static string CombinePath(string firstPath, string secondPath)
        {
            if (firstPath.LastIndexOf('/') != firstPath.Length - 1)
            {
                firstPath += "/";
            }

            if (secondPath.IndexOf('/') == 0)
            {
                secondPath = secondPath.Substring(1);
            }
            return firstPath + secondPath;
        }
    }


}
