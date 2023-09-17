using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ConsoleUtils
{
    public static void ClearConsole()
    {
        var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}
