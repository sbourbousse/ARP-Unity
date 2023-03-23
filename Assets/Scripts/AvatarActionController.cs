using Mediapipe;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mediapipe.Unity
{
    
	public class AvatarActionController : MonoBehaviour {
        [SerializeField] private List<Material> _shirts ;
        private int _currentShirtIndex;
		public AvatarActionController()
		{
            _currentShirtIndex = 0;
            _shirts = new List<Material>();
		}

        public void PutNextShirtOn() 
        {
            _currentShirtIndex++;
            if(_currentShirtIndex >= _shirts.Count)
                _currentShirtIndex = 0;
            var currentshirt = _shirts[_currentShirtIndex];
            var shirt = GameObject.FindWithTag("avatar-shirt");
            var shirtRenderer = shirt.GetComponent<Renderer>();
            shirtRenderer.material = currentshirt;    
        }

        public void PutPreviousShirtOn() 
        {
            _currentShirtIndex--;
            if(_currentShirtIndex < 0)
                _currentShirtIndex = _shirts.Count - 1;
            var currentshirt = _shirts[_currentShirtIndex];
            var shirt = GameObject.FindWithTag("avatar-shirt");
            var shirtRenderer = shirt.GetComponent<Renderer>();
            shirtRenderer.material = currentshirt;    
        }
    }
}


/*
public class AvatarActionController {
    [SerializeField] private List<Material> _shirts ;


    bool IsSwordLeftActive = false;
    bool IsSwordRightActive = false;
    public AvatarActionController()
    {
        IsSwordLeftActive = true;
        IsSwordRightActive = true;
        RightSword = GameObject.FindWithTag("rightsword");
        LeftSword = GameObject.FindWithTag("leftsword");

    }
    public bool IsSwordActive { get; set; }
    public GameObject RightSword { get; set; }
    public GameObject LeftSword { get; set; }
    public string GetTagBySide(HumanPartSide side)
    {
        switch (side)
        {
            case HumanPartSide.Left:
                return "leftsword";
            case HumanPartSide.Right:
                return "rightsword";
            default:
                throw new ArgumentOutOfRangeException(nameof(side), side, null);

        }
    }
    public void TriggerSword(HumanPartSide side)
    {
        bool swordActive = false;
        if (side == HumanPartSide.Left)
        {
            swordActive = IsSwordLeftActive;
            IsSwordLeftActive = !IsSwordLeftActive;
        }
        else if (side == HumanPartSide.Right)
        {
            swordActive = IsSwordRightActive;
            IsSwordRightActive = !IsSwordRightActive;
        }
        
        if (swordActive)
        {
            HideSword(side);
        }
        else
        {
            ShowSword(side);
        }
    }

    public void ShowSword(HumanPartSide side)
    {
        if (side == HumanPartSide.Left)
            LeftSword.SetActive(true);
        else if (side == HumanPartSide.Right)
            RightSword.SetActive(true);
    }

    public void HideSword(HumanPartSide side)
    {
        if (side == HumanPartSide.Left)
            LeftSword.SetActive(false);
        else if (side == HumanPartSide.Right)
            RightSword.SetActive(false);
    }
}*/