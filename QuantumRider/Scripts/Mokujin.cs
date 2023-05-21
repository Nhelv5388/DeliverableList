using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mokujin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject bomb;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            Instantiate(bomb,this.transform.position,Quaternion.identity);
            this.gameObject.SetActive(false);
            //this.gameObject.SetActive(false);
        }

    }
}
