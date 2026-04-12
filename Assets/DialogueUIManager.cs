using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    [Header("Speed Settings")]
    [SerializeField] private float normalCharDelay = 0.05f;
    [SerializeField] private float fastCharDelay = 0.01f;

    private string[] _paragraphs;
    private int _currentParagraphIndex = 0;
    private bool _isTyping = false;
    private bool _isFastMode = false;
    private bool _finishCurrentStory = true;
    private Coroutine _typingCoroutine;
    //[TextArea(10,50)]
    //public string test;
    void Start()
    {
        //SetInputText(test);
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            HandleClick();
        }
    }

    private void ParseParagraphs(string inputText)
    {
        _currentParagraphIndex = 0;
        string normalizedInput = inputText.Replace("\r\n", "\n").Replace("\r", "\n");
        string[] lines = normalizedInput.Split('\n');
        
        var paragraphList = new System.Collections.Generic.List<string>();
        
        // Check if input uses empty lines as separators
        bool hasEmptyLineSeparators = false;
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                hasEmptyLineSeparators = true;
                break;
            }
        }
        
        if (hasEmptyLineSeparators)
        {
            string currentParagraph = "";
            
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                
                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    if (!string.IsNullOrWhiteSpace(currentParagraph))
                    {
                        paragraphList.Add(currentParagraph.Trim());
                        currentParagraph = "";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentParagraph))
                    {
                        currentParagraph += " ";
                    }
                    currentParagraph += trimmedLine;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(currentParagraph))
            {
                paragraphList.Add(currentParagraph.Trim());
            }
        }
        else
        {
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedLine))
                {
                    paragraphList.Add(trimmedLine);
                }
            }
        }
        _paragraphs = paragraphList.ToArray();
    }

    private void HandleClick()
    {
        if (_isTyping)
        {
            // Speed up text display
            _isFastMode = true;
        }
        else
        {
            if (!_finishCurrentStory)
            {
                // Load next paragraph
                LoadNextParagraph();
            }
            else
            {
                //paragraph finish. click to enter dressing
                GameFlowManager.instance.LoadDressingUI();
            }

        }
    }

    private void LoadNextParagraph()
    {
        _currentParagraphIndex++;
        if (_currentParagraphIndex < _paragraphs.Length)
        {
            StartTypingParagraph(_currentParagraphIndex);
        }
        else
        {
            Debug.Log("Finish typing story node");
            _finishCurrentStory = true;
        }
    }

    private void StartTypingParagraph(int index)
    {
        if (_typingCoroutine != null)
        {
            _isTyping = false;
            StopCoroutine(_typingCoroutine);
        }
        
        _isFastMode = false;
        _typingCoroutine = StartCoroutine(TypeText(_paragraphs[index]));
    }

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        textDisplay.text = "";

        foreach (char c in text)
        {
            textDisplay.text += c;
            
            float delay = _isFastMode ? fastCharDelay : normalCharDelay;
            yield return new WaitForSeconds(delay);
        }

        _isTyping = false;
    }

    public void SetInputText(string text)
    {
        ParseParagraphs(text);
        _finishCurrentStory = false;
        if (_paragraphs.Length > 0)
        {
            StartTypingParagraph(0);
        }
    }
    public void Reset()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }
        _currentParagraphIndex = 0;
        _isTyping = false;
        _isFastMode = false;
        textDisplay.text = "";
    }
}
