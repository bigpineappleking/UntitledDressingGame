using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera _characterCam;

    [SerializeField]
    private GameObject _introUI;

    [SerializeField]
    private GameObject _dressingUI;

    [SerializeField]
    private GameObject _dialogueUI;
    void Awake()
    {
        LoadIntroPage();
    }

    void LoadIntroPage()
    {
        if(!_introUI.IsUnityNull()){
            Instantiate(_introUI);
        }
    }

    void LoadDialogue()
    {
        
    }

    void LoadDressing()
    {
        
    }

    void RenderCharacterFinalDress()
    {
        _characterCam.Render();
    }
}
