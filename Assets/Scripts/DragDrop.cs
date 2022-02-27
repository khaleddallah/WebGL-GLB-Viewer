using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotation;

    private Camera mainCamera;
    private bool isHoldItem;
    private GameObject item;
    private Vector3 itemPosition;
    private Vector3 itemScreenPosition;


    void Start()
    {
        isHoldItem = false;
        mainCamera = Camera.main;

    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.CompareTag("item"))
                {
                    isHoldItem = true;
                    item = hit.transform.gameObject;
                    offsetPosition = hit.transform.position-hit.point;
                    offsetPosition.z = 0f;
                    itemScreenPosition = mainCamera.WorldToScreenPoint(hit.transform.position);
                }
            }
        }

        if(Input.GetMouseButtonUp(0)){
            if(isHoldItem){
                Debug.Log("Reset");
                isHoldItem = false;
            }
        }
        
        if(isHoldItem)
        {
            item.transform.position = mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, itemScreenPosition.z)) +
                offsetPosition;
        }
    }


}
