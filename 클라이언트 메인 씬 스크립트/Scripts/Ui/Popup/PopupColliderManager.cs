using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupColliderManager : MonoBehaviour
{
    public PopupColliderList<GameObject> popupColliderList = new PopupColliderList<GameObject>();

    private void Awake()
    {
        popupColliderList.onListChanged += HandleListChanged;
    }

    private void HandleListChanged()
    {
        List<GameObject> updateList = popupColliderList.GetList();
        bool enableColliders = false;

        for (int i = 0; i < updateList.Count; i++)
        {
            enableColliders = i == updateList.Count - 1;
            SetChildCollidersEnabled(updateList[i], enableColliders);
        }
    }

    private void SetChildCollidersEnabled(GameObject obj, bool enable)
    {
        // 오브젝트가 이미 삭제되었는지 확인
        if (obj != null)
        {
            BoxCollider[] childColliders = obj.GetComponentsInChildren<BoxCollider>();            
            foreach (BoxCollider childCollider in childColliders)
            {
                if (childCollider != null)
                {
                    childCollider.enabled = enable;
                }
            }
        }
    }

    private void OnDisable()
    {
        popupColliderList.onListChanged -= HandleListChanged;
    }
}