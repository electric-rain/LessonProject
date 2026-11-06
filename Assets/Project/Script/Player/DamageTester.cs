using Unity.VisualScripting;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Health>().TakeDamage(10);
    }
}
