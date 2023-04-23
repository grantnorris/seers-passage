using UnityEngine;

public static class Logger
{
    public static void Send(string txt, string type, string logType = "") {
        if (!GameManager.instance.enableLogs) {
            return;
        }

        if (logType == "assertion") {
            Debug.LogAssertion(txt + " - " + type);
        } else {
            Debug.Log(txt + " - " + type);
        }
    }
}
