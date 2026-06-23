using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] EditSkillsWindow editSkillsWindow;


    public void OpenEditSkills()
    {
        editSkillsWindow.Open();
    }

    private readonly List<UIWindow>
        openedWindows = new();

    public void Open(
        UIWindow window)
    {
        if (!openedWindows.Contains(window))
        {
            openedWindows.Add(window);
        }

        window.Open();
    }

    public void Close(
        UIWindow window)
    {
        openedWindows.Remove(window);
        window.Close();
    }

    public void Focus(
        UIWindow window)
    {
        window.Focus();
    }
}