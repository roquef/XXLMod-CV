using System.IO;
using System.Reflection;
using UnityEngine;

namespace XXLModCV
{
    public static class AssetBundleHelper
    {
        public static GameObject SphereIndicatorPrefab { get; set; }

        public static void LoadUIBundle()
        {
            var assetBundle = AssetBundle.LoadFromMemory(ExtractResources("XXLModCV.Resources.stances"));
            SphereIndicatorPrefab = assetBundle.LoadAsset<GameObject>("Assets/Mods/Stances/Indicator.prefab");
            assetBundle.Unload(false);
        }

        public static byte[] ExtractResources(string filename)
        {
            using (Stream resFileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename))
            {
                if (resFileStream == null)
                {
                    return null;
                }
                else
                {
                    byte[] ba = new byte[resFileStream.Length];
                    resFileStream.Read(ba, 0, ba.Length);
                    return ba;
                }
            }
        }
    }
}
