using UnityEngine;
using easyInputs;

public class TriggerToTurnPage : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPageIndex = 0;
    public bool useLeftHand = false;
    private bool triggerPressed = false;

    void Start()
    {
        for (int i = 1; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
    }

    void Update()
    {
        if (useLeftHand)
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand) && !triggerPressed)
            {
                triggerPressed = true;
                TurnBackPage();
            }
            else if (!EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                triggerPressed = false;
            }
        }
        else
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) && !triggerPressed)
            {
                triggerPressed = true;
                TurnPage();
            }
            else if (!EasyInputs.GetTriggerButtonDown(EasyHand.RightHand))
            {
                triggerPressed = false;
            }
        }
    }

    void TurnPage()
    {
        pages[currentPageIndex].SetActive(false);

        currentPageIndex++;
        if (currentPageIndex >= pages.Length)
        {
            currentPageIndex = 0;
        }

        pages[currentPageIndex].SetActive(true);
    }

    void TurnBackPage()
    {
        pages[currentPageIndex].SetActive(false);

        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = pages.Length - 1;
        }

        pages[currentPageIndex].SetActive(true);
    }
}
