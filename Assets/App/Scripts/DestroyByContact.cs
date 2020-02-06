using UnityEngine;

namespace SpaceShooterPlus
{
    public class DestroyByContact : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
