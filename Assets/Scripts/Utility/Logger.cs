using UnityEngine;

public static class Logger
{
    public static void Send(string txt, string type) {
        if (!GameManager.instance.enableLogs) {
            return;
        }

        Debug.Log(txt + " - player");
    }
}
