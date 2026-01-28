using UnityEngine;
using UnityEngine.UI;

public class NextPage : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPageIndex = 0;

    private void Start()
    {
        for (int i = 1; i < pages.Length; i++)
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pages[currentPageIndex].SetActive(false);
        currentPageIndex = (currentPageIndex + 1) % pages.Length;
        pages[currentPageIndex].SetActive(true);
    }
}
