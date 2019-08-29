using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Manager : MonoBehaviour
{
    // [SerializeField]
    #if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713"; // Test App ID
#elif UNITY_IPHONE
        string appId = "ca-app-pub-3940256099942544~1458002511";
#else
        string appId = "unexpected_platform";
#endif

    [SerializeField]
    NativeAdsMethod nativeAdPrefab;
    [SerializeField]
    Transform content;

    public void Initialize(){
        Debug.Log("Initializing admob");
        MobileAds.Initialize(appId);
    }

    public void CreateNativeAd(){
        var ad = GameObject.Instantiate(nativeAdPrefab, content);
        // ad.transform.SetParent(content);
    }
}
