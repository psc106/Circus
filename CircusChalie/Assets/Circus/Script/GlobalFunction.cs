using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public static class GlobalFunction
{
    public static Vector2 AddVector(this Vector3 origin, Vector2 addVector)
    {
        Vector2 result = new Vector2(origin.x, origin.y);
        result += addVector;
        return result;
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message)
    {
#if DEBUG_MODE
        Debug.Log(message);
#endif
    }


    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogWarning(object message)
    {
#if DEBUG_MODE
        Debug.LogWarning(message);
#endif
    }

    public static void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static bool isValid<T>(this T target) where T : Component
    {
        if (target == null || target == default) { return false; }
        else
        {
            return true;
        }
    }

    public static bool isValid<T>(this List<T> target) where T : Component
    {
        bool isInvalid = (target == null || target == default);
        isInvalid = (isInvalid || target.Count == 0);

        if (isInvalid) { return false; }
        else { return true; }
    }

}
