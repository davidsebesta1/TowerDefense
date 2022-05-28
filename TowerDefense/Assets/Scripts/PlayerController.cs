using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CanvasRenderer BuildPanel;
    [SerializeField] private Button startButton;

    private GameObject selectedTile;

    private float horizontalInput;
    private float verticalInput;
    private int selectedTowerID = 0;

    private int money = 99999;

    private int UILayer;

    private bool buildMode = false;
    private bool isGameActive = false;
    private bool isOverBuildModePanel = false;

    public void StartGame()
    {
        UILayer = LayerMask.NameToLayer("UI");
        isGameActive = true;
        startButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isGameActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            isOverBuildModePanel = IsPointerOverUIElement();

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
                    if (hit.collider.gameObject.CompareTag("Tile") && !isOverBuildModePanel)
                    {
                        hit.collider.gameObject.GetComponent<TileScript>().BuildTower(selectedTowerID);
                    }
                }
            }
        }
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    private void LateUpdate()
    {
        Vector3 dir = new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0);
        transform.Translate(dir);
    }

    public void SetSelectedTile(GameObject tile)
    {
        this.selectedTile = tile;
    }

    public GameObject GetSelectedTile()
    {
        return this.selectedTile;
    }

    public bool GetBuildMode()
    {
        return buildMode;
    }

    public void SetSelectedTowerID(int id)
    {
        this.selectedTowerID = id;
    }

    public int GetSelectedTowerID()
    {
        return selectedTowerID;
    }

    public int GetMoneyAmount()
    {
        return money;
    }

    public void AddMoneyAmount(int add)
    {
        money += add;
    }
}
