using UnityEngine;

public class ConditionDoor : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform unlockPoint;

    public Transform UnlockPoint => unlockPoint;


    public void OpenDoor()
    {
        anim.SetBool("Start", true);
    }

  

}
