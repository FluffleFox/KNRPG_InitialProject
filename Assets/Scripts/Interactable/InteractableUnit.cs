using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUnit : MonoBehaviour
{
    [SerializeField] protected GraphGrid grid;
    [SerializeField] protected ItemData itemData;
    [SerializeField] protected ItemUIData itemUIData;
    [SerializeField] protected GameObject itemUIPrefab;
    [SerializeField] protected Vector3 UIOffset;
    protected Node currentNode;
    protected List<Node> rangeNodes;
    protected bool inRange;

    private GameObject itemUI;
    private bool isUIinitialized;

    protected virtual void Awake()
    {
        inRange = false;
        isUIinitialized = false;
        currentNode = grid.FindNode(transform.position);
        StartCoroutine(InitRangeNodes());
    }

    private IEnumerator InitRangeNodes()
    {
        yield return null; // wait for grid initialization

        rangeNodes = new List<Node>();
        List<Node> helperList = new List<Node>();
        List<Node> containerList = new List<Node>();
        rangeNodes.Add(currentNode);
        helperList.Add(currentNode);

        for (int i = 0; i < itemData.RangeToInteract; i++)
        {
            foreach (var nodeMain in helperList)
            {
                foreach (var nodeNeighbour in grid.GetNeighbours(nodeMain))
                {
                    if (!rangeNodes.Contains(nodeNeighbour))
                    {
                        containerList.Add(nodeNeighbour);
                    }
                }
            }
            helperList.Clear();
            helperList = new List<Node>(containerList);
            rangeNodes.AddRange(containerList);
            containerList.Clear();
        }
    }

    protected virtual void Update()
    {
        if (rangeNodes != null)
        {
            inRange = IsPlayerInRange();
        }

        if (inRange && itemUIData)
        {
            DisplayUI();
        }
        else
        {
            HideUI();
        }
    }

    public virtual bool IsPlayerInRange()
    {
        foreach(var node in rangeNodes)
        {
            if (node.IsPlayerOnNode())
            {
                return true;
            }
        }
        return false;
    }

    protected void InitUI(GameObject UIelement)
    {
        ItemUIPlaceHolder placeHolder = UIelement.GetComponent<ItemUIPlaceHolder>();
        foreach (ItemUIPlaceHolder.UIComponent childUI in placeHolder.Components)
        {
            switch (childUI.type)
            {
                case ItemUIPlaceHolder.UIType.TITLE:
                    childUI.component.GetComponent<TextMeshProUGUI>().text = itemUIData.Title;
                    childUI.component.GetComponent<TextMeshProUGUI>().color = itemUIData.TextColor;
                    break;
                case ItemUIPlaceHolder.UIType.IMAGE:
                    childUI.component.GetComponent<Image>().sprite = itemUIData.ItemImage;
                    break;
                case ItemUIPlaceHolder.UIType.DESCRIPTION:
                    childUI.component.GetComponent<TextMeshProUGUI>().text = itemUIData.Description;
                    childUI.component.GetComponent<TextMeshProUGUI>().color = itemUIData.TextColor;
                    break;
                case ItemUIPlaceHolder.UIType.BACKGROUND:
                    childUI.component.GetComponent<Image>().color = itemUIData.BackgroundColor;
                    break;
                case ItemUIPlaceHolder.UIType.BUTTON:
                    childUI.component.GetComponent<Image>().color = itemUIData.ButtonColor;
                    childUI.component.GetComponent<Button>().onClick.AddListener(Interact);
                    break;
                case ItemUIPlaceHolder.UIType.BUTTONTEXT:
                    childUI.component.GetComponent<TextMeshProUGUI>().text = itemUIData.ButtonText;
                    childUI.component.GetComponent<TextMeshProUGUI>().color = itemUIData.ButtonTextColor;
                    break;
            }
        }
        itemUI.transform.parent = this.transform;
        isUIinitialized = true;
    }

    protected virtual void DisplayUI()
    {
        if (!isUIinitialized)
        {
            itemUI = Instantiate(itemUIPrefab, transform.position + UIOffset, Quaternion.identity);
            InitUI(itemUI);
        }
        if (itemUI)
        {
            itemUI.active = true;
        }
    }
    protected virtual void HideUI()
    {
        if (!isUIinitialized)
        {
            itemUI = Instantiate(itemUIPrefab, transform.position + UIOffset, Quaternion.identity);
            InitUI(itemUI);
        }
        if (itemUI)
        {
            itemUI.active = false;
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Base !");
    } 

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        // Only in game mode, when grid is initialized
        if (Application.isPlaying)
        {
            try
            {
                Gizmos.color = Color.blue;

                currentNode = grid.FindNode(transform.position);
                InitRangeNodes();
                foreach (var node in rangeNodes)
                {
                    Vector3 positionNode = new Vector3(node.transform.position.x, node.transform.position.y + grid.SphereRadius + node.ModelHeight, node.transform.position.z);
                    Gizmos.DrawSphere(positionNode, grid.SphereRadius);
                }
            }
            catch (System.NullReferenceException e) { }
        }
    }
#endif
}
