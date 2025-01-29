using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetIndexWeapon(int index)
    {
        _animator.SetInteger("IndexWeapon", index);
    }
    
    public void PlayWalk()
    {
        
    }

    public void PlayIdle()
    {
        
    }
}
