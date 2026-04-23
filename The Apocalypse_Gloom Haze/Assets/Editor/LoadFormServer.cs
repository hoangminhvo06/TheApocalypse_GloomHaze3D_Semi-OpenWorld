using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFormServer : MonoBehaviour
{
    private string bundleURl =
    "https://drive.google.com/uc?export=download&id=1kLjJA_TBf_1bcY1Yt61tZttRRofUO_IK";

    void Start()
    {
        //StartCoroutine(DownloadAndLoadBundle());
    }

    IEnumerator DownloadAdnLoadBundle()
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURl))
        {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download AssetBundle: " + www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                if (bundle != null)
                {
                    string[] assetNames = bundle.GetAllAssetNames();
                    foreach (string assetName in assetNames)
                    {
                        Debug.Log("Asset found: " + assetName);
                    }

                    // Load an asset example (assuming prefab)
                    for (int i = 0; i<assetNames.Length; i++)
                    {
                        GameObject prefab = bundle.LoadAsset<GameObject>(assetNames[i]);
                        Instantiate(prefab);
                        Debug.Log("Load được dữ liệu");
                    }

                    bundle.Unload(false);
                }
                else
                {
                    Debug.LogError("Failed to load AssetBundle");
                }
            }
        }
    }
}
