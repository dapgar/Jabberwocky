using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trigger: X
// Button 2: A
// Button 3: B
// Button 4: Y
// Button 5: L Trigger
// Button 6: R Trigger

// Button 9: Select
// Button 10: Start

public class PlayerInputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButton()
    {
        transform.Translate(new Vector3(0, .5f, 0));
    }
}
