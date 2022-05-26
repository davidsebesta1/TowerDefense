using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private float horizontalInput;
    private float verticalInput;

    private GameObject selectedTile;

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0);
        transform.Translate(dir);
    }

    public void setSelectedTile(GameObject tile)
    {
        this.selectedTile = tile;
    }

    public GameObject getSelectedTile()
    {
        return this.selectedTile;
    }
}
