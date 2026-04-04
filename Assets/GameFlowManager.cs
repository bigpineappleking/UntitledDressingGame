using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public enum UIType
    {
        Start,
        Dressing,
        Dialogue
    }

    private UIType _currUI = UIType.Start;
    // Start is called before the first frame update
    [SerializeField]
    private Camera _characterCam;

    [SerializeField]
    private GameObject _introUI;

    [SerializeField]
    private GameObject _dressingUI;

    [SerializeField]
    private GameObject _dialogueUI;

    [SerializeField]
    private Image _loadUI;

    [SerializeField]
    private SpriteRenderer _background;
    [SerializeField]
    private float _loadDuration;
    public static GameFlowManager instance;
    [SerializeField]
    private StoryNode _currStory;
    void Awake()
    {
        instance = this;
        LoadIntroPage();
    }

    void LoadIntroPage()
    {
        _introUI.SetActive(true);
        _dressingUI.SetActive(false);
        _dialogueUI.SetActive(false);
        _loadUI.gameObject.SetActive(false);
    }

    public void LoadDressingUI()
    {
        StartCoroutine(Loading(UIType.Dressing));
    }

    public void LoadDialogueUI()
    {
        StartCoroutine(Loading(UIType.Dialogue));
    }

    private IEnumerator Loading(UIType targetUI)
    {
        float elapsed = 0f;
        bool loadui = false;
        _loadUI.gameObject.SetActive(true);

        while (elapsed < _loadDuration)
        {
            elapsed += Time.deltaTime;
            var currProgress = Mathf.Lerp(-0.2f, 1.0f, elapsed / _loadDuration);
            SetLoadingProgress(currProgress);

            if(currProgress > 0.4f && !loadui){ 
                LoadUI(targetUI);
                loadui = true;
            }
            yield return null;
        }
        SetLoadingProgress(1.0f);
        if(targetUI == UIType.Dialogue){ 
            _dialogueUI.SetActive(true);
            _dialogueUI.GetComponent<DialogueUIManager>().SetInputText(_currStory.story);
        }
        _loadUI.gameObject.SetActive(false);
        
    }

    private void LoadUI(UIType targetUI)
    {
        if(targetUI == _currUI) return;
        if(targetUI == UIType.Dressing)
        {
            _introUI.SetActive(false);
            _dressingUI.SetActive(true);
            _dialogueUI.SetActive(false);
        }else if (targetUI == UIType.Dialogue)
        {
            _introUI.SetActive(false);
            _dressingUI.SetActive(false);
            _dialogueUI.SetActive(false);
        }
        else
        {
            //todo: would it be back to start page?? maybe but deal with this later
        }
        _currUI = targetUI;
    }

    void SetLoadingProgress(float progress)
    {
        _loadUI.material.SetFloat("_Progress", progress);
    }

    public void RenderCharacterFinalDress()
    {
        _characterCam.Render();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
