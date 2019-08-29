using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class NativeAdsMethod : MonoBehaviour
{

    private UnifiedNativeAd nativeAd;
    private bool unifiedNativeAdLoaded;

    // Below are the UI element that you need to assign from Unity Editor
    public Image adChoiceTexture;//you can display image by Image component
    public RawImage appIcon;//or RawImage component
    public TextMeshProUGUI headlines;
    public GameObject starRating;
    public TextMeshProUGUI store;
    public TextMeshProUGUI bodyText;
    public RawImage bigImage;
    public TextMeshProUGUI lbCall2Action;


    // Use this for initialization
    IEnumerator Start()
    {
        // MobileAds.Initialize(appId);
        yield return null;
        PrepareForInteraction();
        yield return null;

        RequestNativeAd();
        // gameObject.SetActive(false);
    }

    void PrepareForInteraction()
    {
        AddColliderToRectTransform(adChoiceTexture.rectTransform);
        AddColliderToRectTransform(appIcon.rectTransform);
        AddColliderToRectTransform(headlines.rectTransform);
        AddColliderToRectTransform(store.rectTransform);
        AddColliderToRectTransform(bodyText.rectTransform);
        AddColliderToRectTransform(bigImage.rectTransform);
        AddColliderToRectTransform(lbCall2Action.rectTransform);
    }

    void DisplayAd(UnifiedNativeAd ad)
    {

        Texture2D adChoiceLogoTexture = this.nativeAd.GetAdChoicesLogoTexture();
        if (adChoiceLogoTexture != null)
        {
            //demo showing image by Image component
            adChoiceTexture.sprite = Sprite.Create(adChoiceLogoTexture, new Rect(0.0f, 0.0f, adChoiceLogoTexture.width, adChoiceLogoTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            if (!this.nativeAd.RegisterAdChoicesLogoGameObject(adChoiceTexture.gameObject))
            {
                Debug.Log("RegisterAdChoicesLogoGameObject Unsuccessfull");
            }
        }

        Texture2D iconTexture = this.nativeAd.GetIconTexture();
        if (iconTexture != null)
        {
            //showing image by RawImage component is easier
            appIcon.texture = iconTexture;
            if (!this.nativeAd.RegisterIconImageGameObject(appIcon.gameObject))
            {
                Debug.Log("RegisterIconImageGameObject Unsuccessfull");
            }
        }

        string headline = this.nativeAd.GetHeadlineText();
        if (headline != null)
        {
            headlines.text = headline;
            if (!this.nativeAd.RegisterHeadlineTextGameObject(headlines.gameObject))
            {
                Debug.Log("RegisterHeadlineTextGameObject Unsuccessfull");
            }
        }

        string storeName = this.nativeAd.GetStore();
        if (storeName != null)
        {
            store.text = storeName;
            if (!this.nativeAd.RegisterStoreGameObject(store.gameObject))
            {
                Debug.Log("RegisterStoreGameObject Unsuccessfull");
            }
        }

        string bodyText = this.nativeAd.GetBodyText();
        if (bodyText != null)
        {
            this.bodyText.text = bodyText;
            if (!this.nativeAd.RegisterBodyTextGameObject(this.bodyText.gameObject))
            {
                Debug.Log("RegisterBodyTextGameObject Unsuccessfull");

            }
        }

        double starRating = this.nativeAd.GetStarRating();
        if (starRating >= 0)
        {
            this.starRating.SetActive(true);
            this.store.gameObject.SetActive(true);
            this.bodyText.gameObject.SetActive(false);

            if (starRating >= 0 && starRating < 2) { this.starRating.transform.GetChild(0).gameObject.SetActive(true); }
            else if (starRating > 1 && starRating < 3) { this.starRating.transform.GetChild(1).gameObject.SetActive(true); }
            else if (starRating > 2 && starRating < 4) { this.starRating.transform.GetChild(2).gameObject.SetActive(true); }
            else if (starRating > 3 && starRating < 5) { this.starRating.transform.GetChild(3).gameObject.SetActive(true); }
            else if (starRating > 4 && starRating < 6) { this.starRating.transform.GetChild(4).gameObject.SetActive(true); }
        }
        else
        {
            this.starRating.SetActive(false);
            this.store.gameObject.SetActive(false);
            this.bodyText.gameObject.SetActive(true);
        }

        if (this.nativeAd.GetImageTextures().Count > 0)
        {
            List<Texture2D> goList = this.nativeAd.GetImageTextures();
            bigImage.texture = goList[0];
            List<GameObject> list = new List<GameObject>();
            list.Add(bigImage.gameObject);
            this.nativeAd.RegisterImageGameObjects(list);

        }
        string buttonTextString = this.nativeAd.GetCallToActionText();
        if (buttonTextString != null)
        {
            lbCall2Action.text = buttonTextString;
            this.lbCall2Action.gameObject.AddComponent<BoxCollider>();
        }
        if (!this.nativeAd.RegisterCallToActionGameObject(lbCall2Action.gameObject))
        {
            Debug.Log("RegisterCallToActionGameObject Unsuccessfull");
        }

        Debug.Log("Headline is " + headline);
        Debug.Log("Advitiser Text is " + this.nativeAd.GetAdvertiserText());

        Debug.Log("GetBodyText is " + this.nativeAd.GetBodyText());
        Debug.Log("GetCallToActionText is " + buttonTextString);

        Debug.Log("GetPrice is " + this.nativeAd.GetPrice());
        Debug.Log("GetStarRating is " + starRating);
        Debug.Log("GetStore is " + storeName);
    }

    private void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder("ca-app-pub-3940256099942544/2247696110")
            .ForUnifiedNativeAd()
            .Build();
        adLoader.OnUnifiedNativeAdLoaded += this.HandleUnifiedNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

        adLoader.LoadAd(new AdRequest.Builder().Build());
    }

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Native ad failed to load: " + args.Message);
        bodyText.text = "Native ad failed to load: " + args.Message;
        // gameObject.SetActive(false);
    }

    private void AddColliderToRectTransform(RectTransform tf)
    {
        var col = tf.gameObject.AddComponent<BoxCollider>();
        StartCoroutine(C_FitColliderAfterUIUpdated(col, tf));
    }

    private IEnumerator C_FitColliderAfterUIUpdated(BoxCollider collider, RectTransform tf)
    {
        yield return null;
        yield return null;
        FitColliderToUGUIObject(collider, tf);
    }

    public void FitColliderToUGUIObject(BoxCollider collider, RectTransform tf)
    {
        var width = tf.rect.width;
        var height = tf.rect.height;

        Vector2 offset = new Vector2(width * (0.5f - tf.pivot.x), height * (0.5f - tf.pivot.y));
        Vector3 size = new Vector3(width, height, 10);
        collider.center = offset;
        collider.size = size;
    }


    private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
    {
        Debug.Log("Unified Native Ad Loaded");
        this.nativeAd = args.nativeAd;
        DisplayAd(this.nativeAd);
        gameObject.SetActive(true);
    }


}
