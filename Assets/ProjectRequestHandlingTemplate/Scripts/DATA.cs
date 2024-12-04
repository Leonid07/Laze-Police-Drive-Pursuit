using System.Threading.Tasks;
using UnityEngine;

public class DATA : MonoBehaviour
{

    [SerializeField]public string appID;
    [SerializeField]public string onezeroKey;

    public static string CachedLink
    {
        get
        {
            return PlayerPrefs.GetString("CachedLink", string.Empty);
        }
        set
        {
            PlayerPrefs.SetString("CachedLink", value);
        }
    }

    public static bool WasShown
    {
        get
        {
            return PlayerPrefs.HasKey("WasShown");
        }
        set
        {
            if(value)
                PlayerPrefs.SetInt("WasShown", 1);
            else
                PlayerPrefs.DeleteKey("WasShown");
        }
    }

    public static async Task<bool> Decide()
    {
        Debug.Log("Deciding...");
        return await Yes_No.Decide();
    }
}