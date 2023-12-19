using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tai_TargetArrow : MonoBehaviour
{
    private bool isPress;

    public bool IsPress
    {
        get => isPress;
        set
        {
            isPress = value;
            if (isPress)
            {
                int countCollider = 0;
                for (int i = 0; i < lsArrows.Count; i++)
                {
                    if (lsArrows[i].isCollider)
                    {
                        countCollider++;
                    }
                }

                if (countCollider == 0)
                {
                    //Set animation failed for Main
                    Tai_GameManager.Instance.SetAnimationBoy(index + 5);
                    //Sub HP bar for Main
                    Tai_GameManager.Instance.SubScore();
                }

                if (lsArrows.Count > 0)
                {
                    //Set correct for the first arrow
                    lsArrows[0].SetCorrect();
                }
            }
            else
            {
                if (lsArrows.Count > 0)
                {
                    if (lsArrows[0].isCorrectArrow && lsArrows[0].timerAnim > 0)
                    {
                        lsArrows[0].SetInvisibleTail();
                    }
                }
            }
        }
    }
    public List<Tai_Arrow> lsArrows = new List<Tai_Arrow>();

    private int index;

    public int countCorrect;
    public void SetCollider(Tai_Arrow arrow)
    {
        if(arrow != null)
        {
            Debug.Log("Collider: " + arrow.name);
        }

        if (lsArrows.Count == 0 || lsArrows.Contains(arrow))
        {
            lsArrows.Add(arrow);
        }
    }

    public void ExitCollider(Tai_Arrow arrow)
    {
        if (arrow != null)
        {
            Debug.Log("Exit: " + arrow.name);
        }

        if (lsArrows.Contains(arrow))
        {
            lsArrows.Remove(arrow);
        }
    }

    public void SetCorrectCollider(int index, float timerAnim)
    {
        countCorrect++;
        Tai_GameManager.Instance.SetAnimationBoy(index, timerAnim);
    }
}
