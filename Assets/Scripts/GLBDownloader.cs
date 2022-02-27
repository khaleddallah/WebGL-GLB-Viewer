using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Siccity.GLTFUtility;
using UnityEngine.Networking;


public class GLBDownloader : MonoBehaviour
{

    [HideInInspector] public float progress = 0;
    [HideInInspector] public string errorMsg;


    private UnityWebRequest uwr;


    private void Start()
    {
        progress = 0 ;
        uwr = null;
        errorMsg = "";
    }


    public IEnumerator DownloadGLBRoutine(string url, string localPath) {
        errorMsg = "";
        progress+=0.001f; // Indicate that we start downloading
        uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        
        uwr.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        uwr.SetRequestHeader("Access-Control-Allow-Headers", "Origin, Content-Type, X-Auth-Token, Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        uwr.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS"); 
        uwr.SetRequestHeader("Access-Control-Allow-Origin", "*");
        uwr.SetRequestHeader("Cache-Control", "no-store, no-cache, must-revalidate");

        Debug.Log("Downloading " + url + " to " + localPath);
        uwr.downloadHandler = new DownloadHandlerFile(localPath);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success){
            Debug.Log(uwr.error);
            errorMsg = uwr.error;
            uwr = null;
            progress = 0 ;
        } else {
            Debug.Log("File successfully downloaded and saved to " + localPath);
            uwr = null;
            progress = 0 ;
        }
    }

    public void DownloadGLB(string url, string localPath) {
        StartCoroutine(DownloadGLBRoutine(url, localPath));
    }


    public IEnumerator MonitorProgressRoutine(){
        while(uwr!=null){
            progress = uwr.downloadProgress;
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }
}
