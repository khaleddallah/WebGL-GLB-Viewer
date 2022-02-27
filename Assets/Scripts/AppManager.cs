using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;


public class AppManager : MonoBehaviour
{
    public Slider progressBar;
    public float progressBarOffset = 0.05f;


    public Button loadClearButton;
    public TMP_InputField urlInput;
    public TextMeshProUGUI noteText;
    public Slider scaleSlider;

    GLBDownloader glbDownloader;
    GLBViewer glbViewer;

    Camera cam;


    void Start()
    {
        cam = Camera.main;
        glbDownloader = GetComponent<GLBDownloader>();
        glbViewer = GetComponent<GLBViewer>();
        loadClearButton.onClick.AddListener(() => {
            if(glbViewer.displayedObject) HandleClearClick();
            else StartCoroutine(HandleLoadClick());
        });
        scaleSlider.onValueChanged.AddListener(delegate {HandleScalingChange();});
        scaleSlider.gameObject.SetActive(false);
    }


    void Update(){
        if(glbDownloader.progress>0){
            progressBar.value = Mathf.Lerp(progressBar.value, glbDownloader.progress+progressBarOffset, 0.1f);
        }
        else{
            progressBar.value = 0;
        }
    }



    public IEnumerator HandleLoadClick(){
        string localPathTmp = "";
        string url = urlInput.text;

        if(url.StartsWith("http")){ 

            string fileName = Path.GetFileName(url);
            Debug.Log("filename is " + fileName);

            localPathTmp= Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log("localPath is " + localPathTmp);

            Debug.Log("Downloading start");
            StartCoroutine(glbDownloader.DownloadGLBRoutine(url, localPathTmp));

            Debug.Log("Monitoring start");
            loadClearButton.interactable = false;
            yield return StartCoroutine(glbDownloader.MonitorProgressRoutine());

            Debug.Log("reset UI");
            progressBar.value = 0f;
            loadClearButton.interactable = true;


            if(glbDownloader.errorMsg.Length==0){
                Debug.Log("Display Model");
                glbViewer.DisplayGLBModel(localPathTmp);
                loadClearButton.GetComponentInChildren<TextMeshProUGUI>().text = "Clear";
                loadClearButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                scaleSlider.gameObject.SetActive(true);
                scaleSlider.value = glbViewer.objectScaleOffset;

            } else {
                Debug.Log("Error");
                StartCoroutine(DisplayMsgError(glbDownloader.errorMsg));
                yield break;
            }

        }
        else{
            Debug.Log("Error");
            StartCoroutine(DisplayMsgError(glbDownloader.errorMsg));
            yield break;
        }

    }


    public void HandleClearClick(){
        Destroy(glbViewer.displayedObject);
        glbViewer.displayedObject = null;
        loadClearButton.GetComponentInChildren<TextMeshProUGUI>().text = "Load";
        loadClearButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        scaleSlider.gameObject.SetActive(false);
    }


    IEnumerator DisplayMsgError(string msg){
        noteText.text = "Error: " + msg;
        yield return new WaitForSecondsRealtime(3);
        noteText.text = "";
    }


    void HandleScalingChange(){
        glbViewer.displayedObject.transform.localScale = Vector3.one * scaleSlider.value;
    }

}
