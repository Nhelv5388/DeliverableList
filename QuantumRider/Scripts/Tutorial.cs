using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] wall;
    [SerializeField] private GameObject[] enemy;
    private bool sound=false;
    private bool sound2 = false;
    private bool sound3 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemy.Length <= 4)
        {
            if (!sound) 
            {
                Semanager.instance.Play("Explosion");
                sound = true;
            }

            wall[0].SetActive(false);
        }
        if (enemy.Length <= 1)
        {
            if (!sound2)
            {
                Semanager.instance.Play("Explosion");
                sound2 = true;
            }
            wall[1].SetActive(false);
        }
        if (enemy.Length <= 0)
        {
            if (!sound3)
            {
                Semanager.instance.Play("Explosion");
                sound3 = true;
            }
            wall[2].SetActive(false);
        }

    }
}
