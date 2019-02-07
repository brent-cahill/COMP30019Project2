using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour {

    public Transform background1;
    public Transform background2;
    public Transform cam;

    private float currentHeight = 15;
    private bool flag = true;
	
	// Update is called once per frame
	void Update () {
		
        if (currentHeight < cam.position.y) {
            if (flag)
                background1.localPosition = new Vector3(0, background1.localPosition.y + 30, 0);
            else
                background2.localPosition = new Vector3(0, background2.localPosition.y + 30, 0);
           
            currentHeight += 15;
            flag = !flag;
        }
        if (currentHeight > cam.position.y + 15) {
            if (flag)
                background2.localPosition = new Vector3(0, background2.localPosition.y - 30, 0);
            else
                background1.localPosition = new Vector3(0, background1.localPosition.y - 30, 0);

            currentHeight -= 15;
            flag = !flag;
        }
	}
}
