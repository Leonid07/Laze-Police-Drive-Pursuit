using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Services
{
    public class Remote_Config_Manager : MonoBehaviour
    {
        private struct userAttributes
        {
        }

        private struct appAttributes
        {
        }

        public Action OnInitialized;

        async void Start()
        {
            await InitializeRemoteConfig();

            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        public static async Task InitializeRemoteConfig()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Unity Services init.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"init fail Unity Services: {ex.Message}");
            }
        }

        private async void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            if (configResponse.requestOrigin == ConfigOrigin.Remote)
            {
                Debug.Log("Remote");
            }
            else if (configResponse.requestOrigin == ConfigOrigin.Default)
            {
                Debug.Log("Default");
            }
            else if (configResponse.requestOrigin == ConfigOrigin.Cached)
            {
                Debug.Log("Cached");
            }

            if (await Yes_No.Decide())
            {
                WebView.InstanceWeb.ShowWebContent();
            }
        }

        public string GetStringByKey(string key)
        {
            if (RemoteConfigService.Instance.appConfig.HasKey(key))
            {
                var result = RemoteConfigService.Instance.appConfig.GetString(key);
                Debug.Log(key + ": " + result);
                return result;
            }

            Debug.LogWarning($"Key {key} not found");
            return string.Empty;
        }

        public bool GetBooleanByKey(string key)
        {
            if (RemoteConfigService.Instance.appConfig.HasKey(key))
            {
                var result = RemoteConfigService.Instance.appConfig.GetBool(key);
                Debug.Log(key + ": " + result);
                return result;
            }

            Debug.LogWarning($"Key {key} not found");
            return false;
        }

        public bool IsVpn()
        {
            bool isVPN = false;
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface Interface in interfaces)
                {
                    if (Interface.OperationalStatus == OperationalStatus.Up)
                    {
                        if (((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback)) || Interface.Description.Contains("VPN") || Interface.Description.Contains("vpn"))
                        {
                            IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                            isVPN = true;
                        }
                    }
                }
            }
            return isVPN;
        }
        #region
        public static Remote_Config_Manager InstanceRemote { get; private set; }

        private void Awake()
        {
            if (InstanceRemote != null && InstanceRemote != this)
            {
                Destroy(gameObject);
            }
            else
            {
                InstanceRemote = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        #endregion
    }
}
