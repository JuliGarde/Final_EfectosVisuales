using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    private Rigidbody rb;
    private PlayerController controller;
    public void Initialize(Rigidbody rb, PlayerController pc)
    {
        this.rb = rb;
        controller = pc;
    }

    #region Movements Animations
    public void Tick()
    {
        HandleMovementAnimation();
    }

    private void HandleMovementAnimation()
    {
        float rbvelocity = rb.velocity.magnitude;
        float movementVelocity = controller.CurrentSpeed;
        float normalVelocity = Mathf.Clamp01(rbvelocity / movementVelocity);

        if (normalVelocity > 0.1f)
        {
            anim.SetFloat("Speed", normalVelocity);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    #endregion
    
}
