using UnityEngine;

public static class Logger
{
    // Enable / disable log sources to print
    // Warnings and assertions are always printed
    static bool enablePlayerLogs = false;
    static bool enableSaveLogs = false;
    static bool enableGeneralLogs = true;

    // Send string to be logged to the console if conditions are met
    public static void Send(string txt, string source = "general", string type = "standard") {
        if (!shouldMessageBeLogged(source, type)) {
            return;
        }

        string log = $"{txt} - {source}";

        switch (type) {
            case "warning":
                Debug.LogWarning(log);
                break;
            case "assertion":
                Debug.LogAssertion(log);
                break;
            default:
                Debug.Log(log);
                break;
        }
    }

    // Whether or not the current message should be logged based on source and type
    static bool shouldMessageBeLogged(string source, string type) {
        if (type == "warning" || type == "assertion") {
            return true;
        }

        switch (source) {
            case "player":
                return enablePlayerLogs;
            case "save":
                return enableSaveLogs;
            case "general":
                return enableGeneralLogs;
            default:
                return false;
        }
    }
}
