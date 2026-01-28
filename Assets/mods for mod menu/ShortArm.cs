using UnityEngine;

public class ShortArm : MonoBehaviour
{
    public GameObject targetObject;

    private Vector3 originalScale;
    private Vector3 resetScale;

    private void Awake()
    {
        originalScale = targetObject.transform.localScale;
        resetScale = new Vector3(0.4f, 0.4f, 0.4f);
        ResizeObject();
    }

    private void OnEnable()
    {
        ResizeObject();
    }

    private void OnDisable()
    {
        ResetObject();
    }

    private void ResizeObject()
    {
        targetObject.transform.localScale = resetScale;
    }

    private void ResetObject()
    {
        targetObject.transform.localScale = originalScale;
    }
}
