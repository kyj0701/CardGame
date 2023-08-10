using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject twirlPrefab;
    float spawnsTime;
    public float defaultTime = 0.05f;

    void Update()
    {
        if(Input.GetMouseButton(0) && spawnsTime >= defaultTime) 
        {
            twirlCreat();
            spawnsTime = 0;
        }
        spawnsTime += Time.deltaTime;
    }

    // Update is called once per frame
    void twirlCreat()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0;
        Instantiate(twirlPrefab, mPosition, Quaternion.identity);

    }
}
