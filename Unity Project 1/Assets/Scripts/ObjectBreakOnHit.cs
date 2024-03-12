using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreakOnHit : MonoBehaviour
{
    public float health = 30f;
    public GameObject mainObject;
    public GameObject[] objectPartsArray;
    
    static List<GameObject> objectParts;

    int j = 0;
    int n = 0;

    void Start()
    {
        objectParts = new List<GameObject>(objectPartsArray);
        mainObject.SetActive(true);
        foreach (GameObject currentObject in objectParts)
        {
            currentObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            health -= 10f;
            if (health <= 0)
            {
                mainObject.SetActive(false);
                foreach (GameObject currentObject in objectParts)
                {
                    currentObject.SetActive(true);
                }

                StartCoroutine(DestoryObjects());
            }
        }
    }

    IEnumerator DestoryObjects()
    {
        n = objectParts.Count;
        yield return new WaitForSeconds(3);
        for (int i = 0; i < n; i++)
        {
            j = Random.Range(0, objectParts.Count);
            Destroy(objectParts[j]);
            objectParts.RemoveAt(j);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
