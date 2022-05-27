using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject[] towerTemplates;
    [SerializeField] private GameObject[] towerObjects;

    private GameObject playerCamera;

    private bool isClickedOn = false;
    private bool buildMode = false;
    private bool isOccupied = false;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void ShowBlueprint(int towerID)
    {
        if (isClickedOn && buildMode && towerID != 0)
        {
            towerID--;
            Debug.Log(towerID);
            Instantiate(towerTemplates[towerID], transform.position + new Vector3(0, 0.1f, 0), towerTemplates[towerID].transform.rotation);
        }
    }

    private void HideBlueprint()
    {
        Destroy(GameObject.FindGameObjectWithTag("Blueprint"));
    }

    public void BuildTower(int id)
    {
        if(buildMode && !isOccupied)
        {
            id--;
            isOccupied = true;
            Instantiate(towerObjects[id], transform.position + new Vector3(0, 0.1f, 0), towerObjects[id].transform.rotation);
            HideBlueprint();
        }
    }

    private void OnMouseEnter()
    {
        isClickedOn = true;
        PlayerController playerController = playerCamera.GetComponent<PlayerController>();
        buildMode = playerController.getBuildMode();

        if (playerController.getSelectedTile() != null)
        {
            playerController.getSelectedTile().gameObject.GetComponent<TileScript>().HideBlueprint();
        }

        playerController.setSelectedTile(this.gameObject);

        if (!isOccupied)
        {
        ShowBlueprint(playerController.getSelectedTowerID());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isOccupied)
        {
            Debug.Log("Building tower");
            BuildTower(playerController.getSelectedTowerID());
        }

    }

    private void OnMouseExit()
    {
        isClickedOn = false;
    }
}
