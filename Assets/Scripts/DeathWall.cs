using UnityEngine;

public class DeathWall : MonoBehaviour 
{
    
    private void OnTriggerEnter(Collider other) 
    {
        if(!other.isTrigger && other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            GameManager.Instance.Restart();
        }    
    }
}