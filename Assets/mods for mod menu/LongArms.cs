using UnityEngine;

public class LongArms : MonoBehaviour
{
    [SerializeField] private string handTag = "HandTag";
    [SerializeField] private GameObject gorillaRig;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            gorillaRig.transform.localScale *= 1.35f; 
        }
    }
}
