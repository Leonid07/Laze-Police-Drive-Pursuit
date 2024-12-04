using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;

public static class Unity_Remote_Config_Template
{
    private struct userAttributes { }

    private struct appAttributes { }


    public static async Task<bool> InitializeRemote()
    {
        await InitializeRemoteConfig();

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());

        return true;
    }

    public static async Task InitializeRemoteConfig()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            Debug.Log("Unity Services init.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"init fail Unity Services: {ex.Message}");
        }
    }

    private static void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Remote:
                Debug.Log("Remote");
                break;
            case ConfigOrigin.Default:
                Debug.Log("Default");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("Cached");
                break;
            default:
                Debug.Log("Unknown origin");
                break;
        }
    }


    public static string GetStringByKey(string key)
    {
        if (RemoteConfigService.Instance.appConfig.HasKey(key))
        {
            string result = RemoteConfigService.Instance.appConfig.GetString(key);
            Debug.Log(key + ": " + result);
            return result;
        }
        else
        {
            Debug.LogWarning($"Key {key} not found");
            return string.Empty;
        }
    }

    public static bool GetBooleanByKey(string key)
    {
        if (RemoteConfigService.Instance.appConfig.HasKey(key))
        {
            bool result = RemoteConfigService.Instance.appConfig.GetBool(key);
            Debug.Log(key + ": " + result);
            return result;
        }
        else
        {
            Debug.LogWarning($"Key {key} not found");
            return false;
        }
    }
}