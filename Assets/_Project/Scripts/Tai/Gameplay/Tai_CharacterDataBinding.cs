using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tai_CharacterDataBinding : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private bool isBoy = false;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
    }

    public void SetAnimatonCharacter(float index)
    {
        characterAnimator.SetFloat("Index", index);
        if(isBoy && index >= 5)
        {
            //Delay back to idle animation
            CancelInvoke("DelayFinishAnimationCharacter");
            Invoke("DelayFinishAnimationCharacter", 0.5f);
        }
    }

    private void DelayFinishAnimationCharacter()
    {
        characterAnimator.SetFloat("Index", 0);
    }
}


