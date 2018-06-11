using Battlehub.UIControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : ItemsControl
{
    private TreeViewItem TreeViewItem;
    private TreeView TreeView;
    private TreeViewItem firstItem, lastItem;

    private void Start()
    {
        if (GetComponent<Button>().name.Equals("ExpandButton"))
        {
            TreeViewItem = transform.parent.parent.parent.GetComponent<TreeViewItem>();
            TreeView = transform.parent.parent.parent.parent.parent.parent.GetComponent<TreeView>();
            GetComponent<Button>().onClick.AddListener(SelectExpanded);
        }
        if (GetComponent<Button>().name.Equals("ExpandSelfButton"))
        {
            TreeViewItem = transform.parent.parent.parent.GetComponent<TreeViewItem>();
            TreeView = transform.parent.parent.parent.parent.parent.parent.GetComponent<TreeView>();
            GetComponent<Button>().onClick.AddListener(SelectExpandedPlusSelf);
        }
        if (GetComponent<Button>().name.Equals("ShowHideButton"))
        {
            TreeViewItem = transform.parent.parent.parent.GetComponent<TreeViewItem>();
            TreeView = transform.parent.parent.parent.parent.parent.parent.GetComponent<TreeView>();
            GetComponent<Button>().onClick.AddListener(ShowHide);
        }
        if (GetComponent<Button>().name.Equals("TeleportButton"))
        {
            TreeView = transform.parent.parent.GetComponent<TreeView>();
            GetComponent<Button>().onClick.AddListener(Teleport);
        }
        if (GetComponent<Button>().name.Equals("HighlightButton"))
        {
            TreeView = transform.parent.parent.GetComponent<TreeView>();
            GetComponent<Button>().onClick.AddListener(Highlight);
        }


    }

    private void ExpandOneLevel(TreeViewItem theItem)
    {
        TreeViewItem firstChild = theItem.FirstChild();
        TreeViewItem lastChild = theItem.LastChild();
        TreeViewItem currentChild = firstChild;

        firstChild.IsExpanded = true;
        if (firstChild.HasChildren)
            ExpandOneLevel(firstChild);

        while (currentChild.Item.ToString() != lastChild.Item.ToString())
        {
            currentChild = theItem.NextChild(currentChild);
            currentChild.IsExpanded = true;
            if (currentChild.HasChildren)
                ExpandOneLevel(currentChild);
        }

        lastChild.IsExpanded = true;
        lastItem = lastChild;

        if (lastChild.HasChildren)
            ExpandOneLevel(lastChild);

    }

    private int FindItemIndex(TreeViewItem item)
    {
        List<int> instancesIDList = new List<int>();

        foreach (GameObject itemGO in TreeView.Items.OfType<GameObject>())
            instancesIDList.Add(itemGO.GetInstanceID());

        GameObject itemGameObject = (GameObject)item.Item;
        int instanceID = itemGameObject.GetInstanceID();

        int res = instancesIDList.FindIndex(a => a == instanceID);

        return res;
    }

    public void SelectExpanded()
    {
        TreeViewItem.IsExpanded = true;

        if (!TreeViewItem.HasChildren)
            return;

            TreeViewItem firstChild = TreeViewItem.FirstChild();
        firstItem = firstChild;

        if (TreeViewItem.FirstChild() != null)
            ExpandOneLevel(TreeViewItem);

        int firstIndex = FindItemIndex(firstItem);
        int lastIndex = FindItemIndex(lastItem) + 1;

        TreeView.SelectedItems = TreeView.Items.OfType<object>().Skip(firstIndex).Take(lastIndex - firstIndex);
    }

    public void SelectExpandedPlusSelf()
    {
        TreeViewItem.IsExpanded = true;

        if (!TreeViewItem.HasChildren)
            return;

        TreeViewItem firstChild = TreeViewItem.FirstChild();
        firstItem = firstChild;

        if (TreeViewItem.FirstChild() != null)
            ExpandOneLevel(TreeViewItem);

        int firstIndex = FindItemIndex(firstItem) - 1;
        int lastIndex = FindItemIndex(lastItem) + 1;

        TreeView.SelectedItems = TreeView.Items.OfType<object>().Skip(firstIndex).Take(lastIndex - firstIndex);
    }

    public void ShowHide()
    {
        GameObject itemGameObject = (GameObject)TreeViewItem.Item;
        if (itemGameObject.activeSelf)
            itemGameObject.SetActive(false);
        else
            itemGameObject.SetActive(true);
    }

    public void Teleport()
    {
        GameObject firstSelectedGameObject = (GameObject)TreeView.SelectedItem;
        if (firstSelectedGameObject != null)
            Debug.Log("Teleporting to " + firstSelectedGameObject.transform.position.ToString());
    }

    public void Highlight()
    {
        GameObject firstSelectedGameObject = (GameObject)TreeView.SelectedItem;
        if (firstSelectedGameObject != null)
        {
            Debug.Log("Highlighting " + firstSelectedGameObject.name);
            foreach (GameObject itemGO in TreeView.Items.OfType<GameObject>())
            {
                if (itemGO.name != firstSelectedGameObject.name)
                    itemGO.SetActive(false);
            }
        }
    }
}