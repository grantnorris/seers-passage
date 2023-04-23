using UnityEngine;

public static class Logger
{
    public static void Send(string txt, string type, string logType = "") {
        if (GameManager.instance != null) {
            switch (type)
            {
                case "player":
                    if (!GameManager.instance.enablePlayerLogs) {
                        return;
                    }
                    
                    break;

                case "save":
                    if (!GameManager.instance.enableSaveLogs) {
                        return;
                    }

                    break;
                default:
                    return;
            }
        }

        if (logType == "assertion") {
            Debug.LogAssertion(txt + " - " + type);
        } else {
            Debug.Log(txt + " - " + type);
        }
    }
}
