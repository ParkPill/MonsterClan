using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UnitStates
{
    Prepare,
    Idle,
    MoveToAttackTarget,
    MoveToPosition,
    MoveAndAttack,
    Attack,
    Recover,
    Dead
}
public enum UnitSubStates
{
    SearchForTarget,
    MoveToPosition,
    MoveAndAttack,
    MoveToTarget
}
public enum AnimationStates
{
    idle,
    run,
    attack,
    dead,
    skill
}
//public enum Occupations
//{
//    Idle,
//    Busy
//}
public enum Rooms
{
    Stage,
    MergeRoom
}

public class UnitBase : MonoBehaviour
{
    public string AnimationNamePreset = "";
    public float AttackHappenTime = 0.3f;
    public BigNum HP;
    public BigNum HPMax;
    public BigNum ATT;
    public BigNum Defense;
    //public BigNum GoldReward;
    public float CriticalRate = 0;
    public float CriticalDamageRate = 0;
    public float AttributeDamage;
    public float IgnoreDefense;
    public string HP_data;
    public float AttackCoolTime = 1;
    protected float _attackCoolTimeElapsed;
    public Rooms BelongTo = Rooms.MergeRoom;
    
    public int SlotIndex;
    public int Index;
    public bool IsHero = false;
    public UnitStates UnitState = UnitStates.Idle;
    public UnitStates PreviousUnitState = UnitStates.Idle;
    public GameScript TheGameScript;
    public Teams WhichTeam;
    public GameObject MissilePrefab;
    public UnityEvent DeadEvent = new UnityEvent();
    public float RecoverDur = 5;
    public float AttackRange = 10;
    public float ArriveRange = 0.1f;
    public UnitBase Target = null;
    public Vector3 TargetPosition;
    public bool IsTargetInRange = false;
    public float MoveSpeed = 1;
    public List<UnitBase> UnitListWhoAttackMe = new List<UnitBase>();
    public List<UnitBase> UnitsOnSight = new List<UnitBase>();
    public UnitSubStates SubState;
    public AnimationStates AniState = AnimationStates.idle;
    protected Animator Animator;
    //protected SkeletonAnimation SkAni;
    private bool _isInitialized = false;
    public bool IsForMerge = false;
    // Start is called before the first frame update
    public void StartHandler()
    {
        //TriggerDetector td = transform.Find("Range").GetComponent<TriggerDetector>();
        //td.TriggerEnter.AddListener(OnRangeEnter);
        //td.TriggerExit.AddListener(OnRangeExit);
        Transform trSight = transform.Find("Sight");
        if(trSight != null)
        {
            TriggerDetector td = trSight.GetComponent<TriggerDetector>();
            if (td != null)
            {
                td.TriggerEnter += OnSightEnter;
                td.TriggerExit += OnSightExit;
            }
        }
        Init();
    }
    public void Init()
    {
        if (!_isInitialized)
        {
            Animator = GetComponent<Animator>();
            //SkAni = GetComponent<SkeletonAnimation>();
            _isInitialized = true;
        }
    }
    public void Init(int unitType)
    {
        Init();
        Index = unitType;
    }
    public virtual void SetUnitState(UnitStates state)
    {
        UnitState = state;
    }
    public void SetAnimationState(AnimationStates state)
    {
        AniState = state;
        string name = state.ToString();
        //Debug.Log("set ani state: " + name);
        if (Animator != null)
        {
            if (state == AnimationStates.run)
            {
                name = "Run";
            }
            else if (state == AnimationStates.idle)
            {
                name = "";
            }
            else if (state == AnimationStates.attack)
            {
                name = "Attack";
            }
            else if (state == AnimationStates.dead)
            {
                name = "Dead";
            }
            Animator.Play(string.Format("{0}{1}", AnimationNamePreset, name));
            //Debug.Log(string.Format("play: {0}", AnimationNamePreset + name));
        }
        //else if(SkAni != null)
        //{
        //    SkAni.loop = state != AnimationStates.dead;
        //    SkAni.AnimationName = name;
        //    //Debug.Log("SkAni set ani state: " + name);
        //}

        if (state == AnimationStates.dead)
        {
            RemoveThis();
        }
    }
    public void RemoveThis()
    {
        StartCoroutine(RemoveLater(3));
    }
    IEnumerator RemoveLater(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(this.gameObject);
    }
    public void UpdateHandler()
    {
        if (IsForMerge)
        {
            return;
        }
        if (UnitState == UnitStates.Idle || UnitState == UnitStates.MoveAndAttack)
        {
            SearchForEnemyAndAttack();
        }
        else if(UnitState == UnitStates.Dead)
        {
            return;
        }

        if (UnitState == UnitStates.MoveToAttackTarget)
        {
            if (!IsTargetInRange)
            {
                Vector3 distance = (Target.transform.position - transform.position);
                transform.position += distance.normalized * MoveSpeed * Time.deltaTime;
                IsTargetInRange = distance.sqrMagnitude <= AttackRange;
            }
            else
            {
                if (Target != null)
                {
                    if (IsTargetInRange)
                    {
                        _attackCoolTimeElapsed -= Time.deltaTime;
                        if (_attackCoolTimeElapsed <= 0)
                        {
                            if (Attack())
                            {
                                _attackCoolTimeElapsed += AttackCoolTime;
                            }
                        }
                    }
                    else
                    {
                        IsTargetInRange = (transform.position - Target.transform.position).sqrMagnitude <= AttackRange;
                    }
                }
                else
                {

                }
            }
        }
        else if (UnitState == UnitStates.MoveToPosition || UnitState == UnitStates.MoveAndAttack)
        {

            Vector3 distance = (TargetPosition - transform.position);
            //Debug.Log(string.Format("distance: {0}", distance));
            transform.position += distance.normalized * MoveSpeed * Time.deltaTime;
            if (distance.sqrMagnitude <= ArriveRange)
            {
                OnArrived();
            }
            else
            {
                if(AniState != AnimationStates.run)
                {
                    SetAnimationState(AnimationStates.run);
                }
            }
        }
    }
    public virtual void Die()
    {
        SetUnitState(UnitStates.Dead);
        foreach(UnitBase unit in UnitListWhoAttackMe)
        {
            if(unit != null && unit.UnitState != UnitStates.Dead)
            {
                unit.OnTargetDead();
            }
        }
        UnitListWhoAttackMe.Clear();
        //TheGameScript.RemoveUnit(WhichTeam, this);
    }
    public void RegisterAttacker(UnitBase attacker)
    {
        UnitListWhoAttackMe.Add(attacker);
    }
    public virtual bool Attack()
    {
        SetAnimationState(AnimationStates.attack);
        return false;
    }
    public void OnAttacked(BigNum dmg)
    {
        HP.SubtractNum(dmg);
        HP_data = HP.ToString();
        //Debug.Log(string.Format("I am attacked! {0}, {1}", dmg, HP));
        if (HP.IsZeroOrUnder() && UnitState != UnitStates.Dead)
        {
            Die();
        }
    }
    public void OnTargetDead()
    {
        if (PreviousUnitState == UnitStates.MoveAndAttack)
        {
            //Debug.Log(string.Format("{0} move and attack {1} ", gameObject.name, TargetPosition));
            MoveAndAttack(TargetPosition);
            //Debug.Log("set privous idle 1");
            //PreviousUnitState = UnitStates.Idle;
        }
        else
        {
            SetUnitState(UnitStates.Idle);
        }
        SetAnimationState(AnimationStates.idle);
    }
    public virtual void OnAttacked(UnitBase attacker, BigNum damage)
    {
        if(UnitState == UnitStates.Idle || UnitState == UnitStates.MoveAndAttack)
        {
            //Debug.Log("set privous idle 2");
            PreviousUnitState = UnitState;
            MoveToAttackTarget(attacker);
        }
    }
    public virtual void MoveToAttackTarget(UnitBase target)
    {
        Target = target;
        if(Target != null && !Target.HP.IsZeroOrUnder())
        {
            SubState = UnitSubStates.MoveToTarget;
            //Debug.Log(string.Format("{0} attack {1}", gameObject.name, target.gameObject.name));
            SetUnitState(UnitStates.MoveToAttackTarget);
            SetAnimationState(AnimationStates.run);
            target.RegisterAttacker(this);
        }
    }
    public virtual void MoveToPosition(Vector3 position)
    {
        SubState = UnitSubStates.MoveToPosition;
        TargetPosition = position;
        SetAnimationState(AnimationStates.run);
        SetUnitState(UnitStates.MoveToPosition);
    }
    public virtual void MoveAndAttack(Vector3 position)
    {
        SubState = UnitSubStates.MoveAndAttack;
        //Debug.Log("unitbase move and attack");
        TargetPosition = position;
        SetAnimationState(AnimationStates.run);
        SetUnitState( UnitStates.MoveAndAttack);
    }
    public virtual void SearchForEnemyAndAttack()
    {
        SubState = UnitSubStates.SearchForTarget;
        //Debug.Log(gameObject.name + " search for enemy among " + UnitsOnSight.Count);
        float minDistance = -1;
        float distanceFromUnit;
        IsTargetInRange = false;
        UnitBase nearestUnit = null;
        foreach (UnitBase unit in UnitsOnSight)
        {
            if (unit.UnitState == UnitStates.Dead || unit.WhichTeam == WhichTeam || unit.GetComponent<UnitBehaviour>().FrameCountLeftToArrive > 0)
            {
                continue;
            }
            distanceFromUnit = (unit.transform.position - transform.position).sqrMagnitude;
            if (minDistance < 0 || distanceFromUnit < minDistance)
            {
                minDistance = distanceFromUnit;
                nearestUnit = unit;
            }
        }
        if (nearestUnit != null)
        {
            //Debug.Log(string.Format("{0} found {1}", gameObject.name, nearestUnit.name));
            if (UnitState == UnitStates.MoveAndAttack)
            {
                PreviousUnitState = UnitStates.MoveAndAttack;
            }
            MoveToAttackTarget(nearestUnit);
        }
    }
    public virtual void OnArrived()
    {
        SetUnitState(UnitStates.Idle);
    }
    private void OnRangeEnter(Collider2D collision)
    {

    }
    private void OnRangeExit(Collider2D collision)
    {

    }
    private void OnSightEnter(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            UnitBase unit = collision.GetComponent<UnitBase>();
            if (unit.WhichTeam != WhichTeam)
            {
                    //Debug.Log(string.Format("{0} on trigger enter UnitBase {1}", gameObject.name, collision.gameObject.name));
                UnitsOnSight.Add(collision.GetComponent<UnitBase>());
            }
        }
    }
    private void OnSightExit(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            UnitBase unit = collision.GetComponent<UnitBase>();
            if (unit.WhichTeam != WhichTeam)
            {
                //Debug.Log(string.Format("{0} on trigger exit UnitBase {1}", gameObject.name, collision.gameObject.name));
                UnitsOnSight.Remove(collision.GetComponent<UnitBase>());
            }
        }
    }
}
