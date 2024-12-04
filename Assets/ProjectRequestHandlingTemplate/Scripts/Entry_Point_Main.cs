using UnityEngine;

public class Entry_Point_Main : MonoBehaviour
{
    [SerializeField] bool hasOnboarding;

    //void Start()
    //{
    //    Debug.Log("Entering App");
    //    Entry();

    //}

    async void Entry()
    {
        if (await DATA.Decide())
        {
            Debug.Log("decide true");
            if (hasOnboarding)
            {

            }
            else
            {
                Debug.Log("uni webb");
                WebView.InstanceWeb.ShowWebContent();
            }
        }
        else
        {

        }
    }

    public void Show()
    {
        WebView.InstanceWeb.ShowWebContent();
    }

    public static void ChangeOrientation(bool top, bool left, bool right, bool bottom)
    {
        Screen.autorotateToPortrait = top;
        Screen.autorotateToLandscapeLeft = left;
        Screen.autorotateToLandscapeRight = right;
        Screen.autorotateToPortraitUpsideDown = bottom;

        CheckOrientation();
    }

    private static void CheckOrientation()
    {
        if (Screen.autorotateToPortrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (Screen.autorotateToLandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else if (Screen.autorotateToPortraitUpsideDown)
        {
            Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        }
        else if (Screen.autorotateToLandscapeRight)
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}