using JetBrains.Annotations;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public int health;
    public int attack;
    public float critDamage = 1.5f; // tỉ lệ đòn chí mạng
    public float critChance = 0.5f;// xác suất ra đòn chí mạng
    public int armor; // Giap, giúp giảm sát thương
    //Hàm định đòn tấn công, truyền vào chỉ số bị tấn công
    public void TakeDamage(int amount)
    {
        health -= amount-(amount *armor/100);
        if(gameObject.CompareTag("Enemy"))
        {
            if (health <= 0)
            {
                EnemyDie();
            }
        }
    }

    public void EnemyDie()
    {
        Debug.Log("Kẻ thù die");
    }
    //Gây damage cho target
    public void DealDamage(GameObject target)
    {
        //Lấy AttributesManager của target truyền vào
        var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            float totalDamage = attack;
            //Tạo ra số ngẫu nhiên mà nhỏ hơn xác suất thì thêm crit
            if (Random.Range(0f, 1f) < critChance)
                totalDamage *= critDamage;
            //Lấy attack của chính mình truyền vào target
            atm.TakeDamage((int)totalDamage);


        }
        
       
        
    }
}
