using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private float horizontalInput;
    private float verticalInput;

    private GameObject selectedTile;
    [SerializeField] private CanvasRenderer BuildPanel;

    private bool buildMode = false;
    private int selectedTowerID = 0;

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (buildMode)
            {
                buildMode = false;
                BuildPanel.gameObject.SetActive(false);
            }
            else
            {
                buildMode = true;
                BuildPanel.gameObject.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.gameObject.CompareTag("Tile"))
                {
                    hit.collider.gameObject.GetComponent<TileScript>().BuildTower(selectedTowerID);
                }
            }
        }
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

    public bool getBuildMode()
    {
        return buildMode;
    }

    public void setSelectedTowerID(int id)
    {
        this.selectedTowerID = id;
    }

    public int getSelectedTowerID()
    {
        return selectedTowerID;
    }
}
