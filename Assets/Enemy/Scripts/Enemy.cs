using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public abstract class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        Idle,
        Hurt,
        Patrol,
        Chase,
        Attack,
        AttackFinish,
    }
    public enemyStates statevisualizer;
    [Header("Component")]
    [SerializeField] public Rigidbody2D enemyRB;
    [SerializeField] public Animator enemyAnim;
    [SerializeField] public SpriteRenderer enemySP;
    [SerializeField] public SoundManager_Enemy soundM;
    [SerializeField] protected Hp myHp;
    [SerializeField] protected MoreMountains.Feedbacks.MMFeedbacks damageFeedback;

    [Header("Hit")]
    public bool allowHitRecover = true;
    [SerializeField] public Vector2 hitDir;
    [SerializeField] public float flashTime = 0.1f;
    //[SerializeField] protected GameObject blood;

    [Header("Ground check")]
    [SerializeField] protected Transform groundSensorStartPos;
    [SerializeField] protected float groundHitDis = 0.2f;
    protected LayerMask groundMask;

    [Header("Player check")]
    [SerializeField] protected Transform eyePos;
    public GameObject target;
    [SerializeField] protected float sightDis = 5f;
    public LayerMask playerMask;
    // Start here is just for sample when creating new enemies
    // the following code should appear in every kind of enemies
    // copy and paste
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemySP = GetComponent<SpriteRenderer>();

        //ChangeState(idleState);
    }

    public abstract void OnHit(int damage, Vector2 hitBackDir, float hitBackSpeed, Weapons.PushType pushtype, float hitRecover);

    protected abstract void flip();

    protected abstract void checkGround();

    protected abstract void checkPlayer();

    public abstract void turn();

    #region coroutines
    public void HitFlash()
    {
        StartCoroutine(HitFlashIE(flashTime));
    }

    IEnumerator HitFlashIE(float duration)
    {
        enemySP.material.SetInt("_Hit", 1);
        enemySP.material.SetInt("_HitBlack", 1);
        yield return new WaitForSeconds(duration*0.1f);
        enemySP.material.SetInt("_HitBlack", 0);
        yield return new WaitForSeconds(duration);
        enemySP.material.SetInt("_Hit", 0);
    }

    #endregion
}
