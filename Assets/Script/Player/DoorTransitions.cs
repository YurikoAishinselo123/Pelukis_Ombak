using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Office1"))
        {
            Debug.Log("office1");
            SceneLoader.Instance.LoadOffice1();
        }
        else if (other.CompareTag("Office2"))
        {
            Debug.Log("office2");
            SceneLoader.Instance.LoadOffice2();
        }
        else if (other.CompareTag("Office3"))
        {
            Debug.Log("office3");
            SceneLoader.Instance.LoadOffice3();
        }
        else if (other.CompareTag("Ocean"))
        {
            Debug.Log("Ocean");
            SceneLoader.Instance.LoadOcean();
        }
    }
}
