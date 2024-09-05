using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMove : MonoBehaviour
{
    public float speed; // Adjust this value to control the speed of the movement
    private Vector2 offset; // Declare offset as private

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(0, 0); // Initialize offset here instead of in Update
    }

    // Update is called once per frame
    void Update()
    {
        offset.y += Time.deltaTime * speed; // Use Time.deltaTime for smoother movement
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
