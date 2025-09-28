using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Object to follow
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //add stuff here if needed to initialize camera
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //Update the camera's position to follow the target horizontally
            //get current position of transform
            var position = transform.position;
            //overwrite only x component
            position.x = player.transform.position.x;
            //assign the new position back
            transform.position = position;
        }
    }
}
