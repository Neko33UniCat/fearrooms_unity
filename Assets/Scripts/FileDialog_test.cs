using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using VRM;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class FileDialog_test : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void FileImporterCaptureClick();

    public string path;
    public GameObject Character;
    public RuntimeAnimatorController animator;
    public GameObject[] CameraPos = new GameObject[3];
    public GameObject StartButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadFromFile()
    {
        //VRMファイルのパスを指定します
        //var path = Application.dataPath + "/Resources/AliciaSolid.vrm";
        var path = UnityEditor.EditorUtility.OpenFilePanel("open", "", "vrm");

        //ファイルをByte配列に読み込みます
        var bytes = System.IO.File.ReadAllBytes(path);

        var context = new VRMImporterContext();

        // GLB形式でJSONを取得しParseします
        context.ParseGlb(bytes);

        context.Load();
        OnLoaded(context);
    }


    public void FileSelected(string url)
    {
        StartCoroutine(LoadJson(url));
    }

    private IEnumerator LoadJson(string url)
    {
        print(url);
        WWW www = new WWW(url);

        while (!www.isDone)
        {
            yield return null;
        }
        if (www.error == null)
        {
            LoadVRMClicked_WebGL(www.bytes);
        }
        else
        {
            print(www.error);
        }
    }

    void LoadVRMClicked_WebGL(Byte[] bytes)
    {
        var context = new VRMImporterContext();

        // GLB形式でJSONを取得しParseします
        context.ParseGlb(bytes);

        context.Load();
        OnLoaded(context);
    }

    private void OnLoaded(VRMImporterContext context)
    {
        //読込が完了するとcontext.RootにモデルのGameObjectが入っています
        var root = context.Root;

        //モデルをワールド上に配置します
        root.transform.position = new Vector3(0, 0, 0);
        root.transform.Rotate(new Vector3(0, 180, 0));
        Character = root;
        //メッシュを表示します
        context.ShowMeshes();
        LoadChar();
    }
    public void LoadModel()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FileImporterCaptureClick();
#else
        LoadFromFile();
#endif
    }
    public void LoadChar()
    {
        StartButton.SetActive(false);
        //path = UnityEditor.EditorUtility.OpenFilePanel("open", "", "vrm");
        try
        {
            //Character = (GameObject)VRMImporter.LoadFromPath(path);
            Character.tag = "Player";
            Character.AddComponent<CapsuleCollider>();
            CapsuleCollider collider = Character.GetComponent<CapsuleCollider>();
            collider.height = 1.5f;
            collider.center = new Vector3(0f, 0.75f, 0f);
            foreach (GameObject x in CameraPos)
            {
                x.transform.parent = Character.transform;
            }
            //Character.AddComponent<CharacterController>();
            //Character.GetComponent<CharacterController>().center = new Vector3(0f, 1f, 0f);
            Character.AddComponent<LocomotionScript>();
            Character.AddComponent<PlayerScript>();
            Character.GetComponent<Animator>().runtimeAnimatorController = animator;
            Character.GetComponent<Rigidbody>().freezeRotation = true;
        }
        catch
        {
            Debug.Log("Error");
        }
    }
}
