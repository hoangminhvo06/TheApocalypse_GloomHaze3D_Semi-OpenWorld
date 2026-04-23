using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    //Đối tượng quản lý máu và tấn công
    //Nó chính là người chơi để lấy chỉ số tấn công
    AttributesManager atm;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            //Lấy quản lý gói takeDamage để nhận sát thương từ người chơi
            other.GetComponent<AttributesManager>().TakeDamage(atm.attack); 
        }
    }
}
