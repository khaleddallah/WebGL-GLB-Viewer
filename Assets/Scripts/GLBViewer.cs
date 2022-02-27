using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Siccity.GLTFUtility;
using UnityEngine.Networking;


public class GLBViewer : MonoBehaviour
{
    [HideInInspector] public bool buttonPressed;
    public GameObject clearButtonCanvas;
    public Vector3 objectPositionOffset = new Vector3(0f, -0.5f, 1f);
    public float objectScaleOffset = 0.1f;

    public GameObject displayedObject;

    private Camera cam;
    Vector3 objCenter;

    void Start(){
        buttonPressed = false;
        cam = Camera.main;
    }



    public void DisplayGLBModel(string filePath){
        displayedObject = Importer.LoadFromFile(filePath);
        displayedObject.transform.tag = "item";
        displayedObject.transform.position = Vector3.zero;
        displayedObject.transform.forward = cam.transform.forward;

        foreach(MeshRenderer r in displayedObject.GetComponentsInChildren<MeshRenderer>()){
            BoxCollider c = displayedObject.AddComponent<BoxCollider>();
            c.size = r.bounds.size;
            c.center = r.bounds.center;
        }
        foreach(SkinnedMeshRenderer r in displayedObject.GetComponentsInChildren<SkinnedMeshRenderer>()){
            BoxCollider c = displayedObject.AddComponent<BoxCollider>();
            c.size = r.bounds.size;
            c.center = r.bounds.center;
        }

        displayedObject.transform.localScale = Vector3.one * objectScaleOffset;
        Vector3 sum = Vector3.zero;
        foreach(BoxCollider c in displayedObject.GetComponentsInChildren<BoxCollider>()){
            sum += c.center*objectScaleOffset;
        }
        objCenter = sum/displayedObject.GetComponents<BoxCollider>().Length;

        Debug.Log("colliders length: "+ displayedObject.GetComponents<BoxCollider>().Length);
        displayedObject.transform.position = - objCenter + cam.transform.position + objectPositionOffset;
    }


    void OnDrawGizmos(){
        if(objCenter!=null){
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(objCenter, 0.02f);
        }
    }


}
