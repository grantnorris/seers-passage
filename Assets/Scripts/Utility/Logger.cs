using UnityEngine;

public static class Logger
{
    // Enable / disable log sources to print
    // Warnings and assertions are always printed
    static bool enablePlayerLogs = false;
    static bool enableSaveLogs = false;
    static bool enableGeneralLogs = true;

    public static void Send(string txt, string source = "general", string type = "") {
        if (type != "warning" && type != "assertion") {
            switch (source)
            {
                case "player":
                    if (!enablePlayerLogs) {
                        return;
                    }
                    
                    break;

                case "save":
                    if (!enableSaveLogs) {
                        return;
                    }

                    break;

                case "general":
                    if (!enableGeneralLogs) {
                        return;
                    }

                    break;
                default:
                    return;
            }
        }

        string log = $"{txt} - {source}";

        switch (type)
        {
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
}
