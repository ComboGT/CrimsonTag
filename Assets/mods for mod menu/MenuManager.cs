using UnityEngine;
using easyInputs;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public EasyHand Hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        menu.SetActive(EasyInputs.GetPrimaryButtonDown(Hand));
    }
}
