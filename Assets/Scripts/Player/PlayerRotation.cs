using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerRotation : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField]
    private float speed = 7;

    [SerializeField]
    private Transform model;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.MovementInput == Vector2.zero)
            return;
        Vector3 lookDir = playerMovement.MovementDir;

        model.forward = Vector3.Slerp(model.forward, lookDir, Time.deltaTime * speed);
    }
}
