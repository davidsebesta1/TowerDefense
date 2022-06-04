using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Serialized Variables
    [SerializeField] private float speed;

    [SerializeField] private CanvasRenderer BuildPanel;
    [SerializeField] private CanvasRenderer ArtyPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private GameObject artyTargetPrefab;

    //Rigidbody
    private Rigidbody rb;

    //Selections
    private GameObject selectedTile;
    private GameObject selectedArty;

    //Inputs
    private float horizontalInput;
    private float verticalInput;

    private int selectedTowerID = 0;

    //Money
    private int money = 150;

    private int UILayer;

    //Bool stuff
    private bool buildMode = false;
    private bool isGameActive = false;
    private bool isOverBuildModePanel = false;
    private bool artySelectionMode = false;

    //Creative mode bool
    private readonly bool creativeMode = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        UILayer = LayerMask.NameToLayer("UI");

        if (creativeMode)
        {
            this.money = 999999;
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        startButton.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        isGameActive = false;
        verticalInput = 0f;
        horizontalInput = 0f;
        restartButton.gameObject.SetActive(true);
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
                {
                    if (hit.collider.gameObject.CompareTag("Artillery") && !isOverBuildModePanel)
                    {
                        selectedArty = hit.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<ArtilleryScript>().gameObject;
                        ArtyPanel.gameObject.SetActive(true);
                        SetArtySelectionMode(true);
                    }

                    if (hit.collider.gameObject.CompareTag("Tile") && !isOverBuildModePanel)
                    {
                        hit.collider.gameObject.GetComponent<TileScript>().BuildTower(selectedTowerID);
                    }

                    if (hit.collider.gameObject.CompareTag("Path") && !isOverBuildModePanel && artySelectionMode)
                    {
                        Vector3 localHit = hit.point;
                        selectedArty.GetComponent<ArtilleryScript>().SetTarget(Instantiate(artyTargetPrefab, localHit + new Vector3(0, 0.01f, 0), Quaternion.Euler(90, 0, 0)));

                        SetArtySelectionMode(false);
                        ArtyPanel.gameObject.SetActive(false);
                    }
                }
            }
        }

        if(gameObject.transform.position.x > 20f)
        {
            horizontalInput += -1.5f;
        }

        if(gameObject.transform.position.x < 0f)
        {
            horizontalInput += 1.5f;
        }

        if (gameObject.transform.position.z > 25f)
        {
            verticalInput += -1.5f;
        }

        if (gameObject.transform.position.z < 0f)
        {
            verticalInput += 1.5f;
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
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> raysastResults = new();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    private void LateUpdate()
    {
        Vector3 dir = speed * Time.deltaTime * new Vector3(horizontalInput, 0, verticalInput);
        rb.AddForce(dir, ForceMode.Impulse);
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

    public void SetArtySelectionMode(bool newMode)
    {
        this.artySelectionMode = newMode;
       if(selectedArty != null)
        {
            Debug.Log(selectedArty.name);
            selectedArty.GetComponent<ArtilleryScript>().SetEditMode(newMode);
        }
    }
}
