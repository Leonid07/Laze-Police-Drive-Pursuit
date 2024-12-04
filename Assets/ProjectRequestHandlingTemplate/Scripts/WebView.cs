using Services;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (UniWebView))]
public class WebView : MonoBehaviour
{
    UniWebView uniWebView;

    public static WebView InstanceWeb { get; private set; }

    private void Awake()
    {
        if (InstanceWeb != null && InstanceWeb != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceWeb = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowWebContent()
    {
        Entry_Point_Main.ChangeOrientation(true, true, true, true);
        uniWebView.gameObject.SetActive(true);
        uniWebView.Load(DATA.CachedLink);
        uniWebView.Show();
        Debug.Log(DATA.CachedLink);
        Debug.Log("show web content");
    }

    void Start()
    {
        uniWebView = GetComponent<UniWebView>();
        uniWebView.SetContentInsetAdjustmentBehavior(UniWebViewContentInsetAdjustmentBehavior.Always);
        uniWebView.SetSupportMultipleWindows(true,true);
        uniWebView.SetBackButtonEnabled(false);
        uniWebView.OnShouldClose += (view) =>
        {
            uniWebView.Hide();
            return false;
        };
        uniWebView.OnPageFinished += (view, code, url) =>
        {
            DATA.CachedLink = url;
            DATA.WasShown = true;
        };
        uniWebView.OnOrientationChanged += (view, orientation) => 
        { uniWebView.Frame = new Rect(Vector2.zero, new(Screen.width, Screen.height)); };
    }
}