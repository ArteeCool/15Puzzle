using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Int32> _number;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<AudioClip> _buttonsSound;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource1;
    
    [SerializeField] private GameObject _winMenu;


    [SerializeField] private Image _soundImage;
    [SerializeField] private Image _muteSoundImage;

    [SerializeField] private Image _noteImage;
    [SerializeField] private Image _muteNoteImage;
    
    private Boolean _isShuffled;

    [SerializeField] private Boolean _isSoundTurnedOn = true;
    [SerializeField] private Boolean _isMusicTurnedOn = true;
    
    private void Start()
    {
        for (var i = 0; i < 16; i++)
        {
            _number.Add(i + 1); 
        }
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            var squarePosition = i + 1;
            _buttons[i].onClick.AddListener(() =>
            {
                GetSquarePosition(squarePosition);
            });
        }
        
        Random();
    }

    private void GetSquarePosition(Int32 numberOfClicked)
    {
        var index = _number.IndexOf(numberOfClicked);

        var x = index % 4;
        var y = index / 4;
        
        var top = y > 0 ? _number[index - 4] : 0;
        var bottom = y < 3 ? _number[index + 4] : 0;

        var right = x < 3 ? _number[index + 1] : 0;
        var left = x > 0  ? _number[index - 1] : 0;
        
        if (top == 16)         SwapNumbers(index, index - 4, numberOfClicked, new Vector2(0f, 250f));
        else if (bottom == 16) SwapNumbers(index, index + 4, numberOfClicked, new Vector2(0f, -250f));
        else if (right == 16)  SwapNumbers(index, index + 1, numberOfClicked, new Vector2(250f, 0f));
        else if (left == 16)   SwapNumbers(index, index - 1, numberOfClicked, new Vector2(-250f, 0f));
        
        CheckForWin();
    }
    
    private void SwapNumbers(Int32 indexA, Int32 indexB, Int32 numOfBtn, Vector2 offset)
    {
        (_number[indexA], _number[indexB]) = (_number[indexB], _number[indexA]);
        _buttons[numOfBtn - 1].GetComponent<RectTransform>().anchoredPosition += offset;
        
        if(!_isShuffled) return;
        PlaySound();
    }

    public void Random()
    {
        _isShuffled = false;
        for (int i = 0; i < 300; i++)
        {
            var rand = UnityEngine.Random.Range(1, _buttons.Count);
            GetSquarePosition(rand);
        }
        _isShuffled = true;
    }

    private void CheckForWin()
    {
        if(!_isShuffled) return;

        for (int i = 0; i < _number.Count; i++)
        {
            if (_number[i] != i + 1) return;
        }

        _winMenu.SetActive(true);
    }

    public void Restart()
    {
        _winMenu.SetActive(false);
        Random();
    }
    
    private void PlaySound()
    {
        if (!_isSoundTurnedOn) return;
            
        var rand = UnityEngine.Random.Range(0, _buttonsSound.Count);
        _audioSource.PlayOneShot(_buttonsSound[rand]);   
    }

    public void TurnSound()
    {
        if (_isSoundTurnedOn)
        {
            _soundImage.gameObject.SetActive(false);
            _muteSoundImage.gameObject.SetActive(true);
        }
        else
        {
            _muteSoundImage.gameObject.SetActive(false);
            _soundImage.gameObject.SetActive(true);
        }

        _isSoundTurnedOn = !_isSoundTurnedOn;
    }
    
    public void TurnMusic()
    {
        if (_isMusicTurnedOn)
        {
            _noteImage.gameObject.SetActive(false);
            _muteNoteImage.gameObject.SetActive(true);
            _audioSource1.volume = 0;
        }
        else
        {
            _noteImage.gameObject.SetActive(true);
            _muteNoteImage.gameObject.SetActive(false);
            _audioSource1.volume = 100;
        }

        _isMusicTurnedOn = !_isMusicTurnedOn;
    }
}
