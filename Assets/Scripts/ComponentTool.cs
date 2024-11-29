using UnityEngine;

public static class ComponentTool
{
    public static void SetActiveSelf(this Component component, bool active)
    {
        if (component == null)
        {
            return;
        }

        var go = component.gameObject;
        if (go.activeSelf == active)
        {
            return;
        }

        go.SetActive(active);
    }

    public static void SetActiveSelf(this GameObject go, bool active)
    {
        if (go == null)
        {
            return;
        }
        if (go.activeSelf == active)
        {
            return;
        }

        go.SetActive(active);
    }
}
