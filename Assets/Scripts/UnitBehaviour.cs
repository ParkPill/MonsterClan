using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UnitMoveType
{
    Walker,
    Stander
}
public class UnitBehaviour : MonoBehaviour
{
    public Teams TheTeam = Teams.Hero;
    private int _hp = 100;
    public int HP {
        get { return _hp; }
        set
        {
            _hp = value;
        }
    }
    
    public UnitMoveType MoveType = UnitMoveType.Walker;
    public int FrameCountLeftToArrive = 60;
    public int Index = 0;
    public int HPMax = 100;
    public int Level = 0;
    public int ATT = 11;
    public bool IsDead = false;
    public UnitBehaviour Target;
    public float DetectRadius = 100;
    public float Speed = 10;
    public float Range = 1; // 1 for near 6 for range
    private float _coolTime = 0;
    public float CoolTime = 2;
    public BattleScript TheBattleScript;
    bool _gettingBigger = false;
    public Attributes Attri;
    //Vector3 _scaleFactor = new Vector3(1, 1, 1);
    //public float ScaleSpeed = 0.1f;
    //public float ScaleMax = 1.1f;
    //public float ScaleMin = 0.9f;
    public float HitEffectColorTime = 0.2f;
    private float _hitColorTime = 0;
    public Material HitColorMat;
    public Material NormalColorMat;
    private float _originalHpBarScaleX = 6;
    private float _originalHpBarScaleY = 6;
    private GameObject _hpBar;
    public Transform Img;
    public bool IsCastle = false;
    private UnitBehaviour _shootedTarget;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D box2d = GetComponent<BoxCollider2D>();
        if (box2d == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        TextMeshPro lbl = transform.Find("lblLevel").GetComponent<TextMeshPro>();
        lbl.text = (Level + 1).ToString();
        _hpBar = transform.Find("hpBar").gameObject;
        _originalHpBarScaleX = _hpBar.transform.localScale.x;
        _originalHpBarScaleY = _hpBar.transform.localScale.y;
        //ScaleSpeed -= 0.05f * Index;
        if (IsCastle)
        {
            Img.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(TheTeam == Teams.Hero?"wizardOnCastle": "wizardOnCastleRed");
        }
        else
        {
            //string prefix = GameManager.Instance.GetSpritePath(Group);
            //string path = string.Format("monster/{0}/{1}{2:00}", int.Parse(prefix.Substring(prefix.Length - 1)), prefix, Index + 1);
            //Debug.Log("path: " + path);
            Img.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameManager.Instance.GetSpritePath(Index));
        }
    }
    public void OnNewEnemyAppeared(UnitBehaviour unit)
    {
        Debug.Log("new unit!~");
        if (unit.TheTeam != TheTeam)
        {
            Debug.Log("diff team");
            DetectEnemiesNearby(DetectRadius);
        }
    }
    public void Init(int index, int level, Teams team)
    {
        Index = index;
        Level = level;
        TheTeam = team;
        HP = DataManager.Instance.GetHP(index, level);
        HPMax = HP;
        ATT = DataManager.Instance.GetATT(index, level);
        Range = DataManager.Instance.GetRange(index, level);
        Speed = DataManager.Instance.GetSpeed(index, level);
    }
    private void Update()
    {
        if (!IsCastle && Target != null)
        {
            transform.Find("img").GetComponent<Animator>().SetBool("flip", Target.transform.position.x < transform.position.x);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("FrameCountLeftToArrive: " + FrameCountLeftToArrive);
        if (FrameCountLeftToArrive > 0)
        {
            FrameCountLeftToArrive--;
            if (FrameCountLeftToArrive <= 0)
            {
                TheBattleScript.FireNewEnemy(this);
            }
            else{
                return;
            }
        }
        
        float dt = Time.deltaTime;

        if (_hitColorTime > 0)
        {
            _hitColorTime -= dt;
            if (_hitColorTime <= 0)
            {
                Img.GetComponent<SpriteRenderer>().material = NormalColorMat;

                if (HP <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            //if (!IsCastle)
            //{
            //    if (_gettingBigger)
            //    {
            //        _scaleFactor.x += dt * ScaleSpeed;
            //        _scaleFactor.y -= dt * ScaleSpeed;
            //        if (_scaleFactor.x > ScaleMax)
            //        {
            //            _scaleFactor.x = ScaleMax;
            //            _scaleFactor.y = ScaleMin;
            //            _gettingBigger = !_gettingBigger;
            //        }
            //    }
            //    else
            //    {
            //        _scaleFactor.x -= dt * ScaleSpeed;
            //        _scaleFactor.y += dt * ScaleSpeed;
            //        if (_scaleFactor.x < ScaleMin)
            //        {
            //            _scaleFactor.x = ScaleMin;
            //            _scaleFactor.y = ScaleMax;
            //            _gettingBigger = !_gettingBigger;
            //        }
            //    }
            //}
        }
        //Img.localScale = _scaleFactor;
        if (_coolTime > 0)
        {
            _coolTime -= dt;
        }
        if (IsTargetEmpty())
        {
            DetectEnemiesNearby(DetectRadius); 
        }
        else 
        {
            if (MoveType != UnitMoveType.Stander)
            {
                float distance = (transform.position - Target.transform.position).sqrMagnitude;
                if (distance < Range)
                {
                    if (_coolTime <= 0)
                    {
                        Attack();
                    }
                }
                else
                {
                    float step = Speed * dt;
                    transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, step);
                }
            }
            //Debug.Log(string.Format("target enemy distance: {0}", distance));
        }
    }
    public bool IsStrong(Attributes me, Attributes attacker)
    {
        int meNum = (int)me;
        int attackerNum = (int)attacker;
        //if ()
        //{

        //}
        return true;
    }
    public void GetHit(int dmg, Attributes attri)
    {
        if (IsStrong(Attri, attri))
        {

        }
        HP -= dmg;
        _hitColorTime = HitEffectColorTime;
        Img.GetComponent<SpriteRenderer>().material = HitColorMat;
        UpdateHPBar();
        if (HP <= 0)
        {
            TheBattleScript.RemoveUnit(this);
            // remove from world when effect is done
        }
    }
    private bool IsRange()
    {
        Debug.Log("isRange : " + Range);
        return Range > 2;
    }
    public void UpdateHPBar()
    {
        _hpBar.transform.localScale = new Vector3(_hp * _originalHpBarScaleX / HPMax, _originalHpBarScaleY, 1);
    }
    public void Attack()
    {
        float delay = 0;
        if (!IsCastle)
        {
            if (IsRange())
            {
                delay = 40 / 60f + 0.2f;
                transform.Find("img").GetComponent<Animator>().SetBool("rangeAttack" + (Target.transform.position.x > transform.position.x ? "" : "Flip"), true);
                //TheBattleScript.ThePool.GetPooledObject(TheBattleScript.MissileList[, tf,
            }
            else
            {
                delay = 42 / 60f;
                transform.Find("img").GetComponent<Animator>().SetBool("meleeAttack" + (Target.transform.position.x > transform.position.x ? "" : "Flip"), true);
            }
        }
        
        _coolTime += CoolTime;
        _shootedTarget = Target;
        StartCoroutine(AttackLater(delay));
    }
    IEnumerator AttackLater(float dt)
    {
        yield return new WaitForSeconds(dt);
        if (!IsCastle)
        {
            transform.Find("img").GetComponent<Animator>().SetBool("meleeAttack", false);
            transform.Find("img").GetComponent<Animator>().SetBool("meleeAttackFlip", false);
            transform.Find("img").GetComponent<Animator>().SetBool("rangeAttack", false);
            transform.Find("img").GetComponent<Animator>().SetBool("rangeAttackFlip", false);
        }
        if (_shootedTarget == Target)
        {
            Target.GetHit(ATT, Attri);
        }
    }

    public bool IsTargetEmpty()
    {
        return Target == null || Target.IsDead || Target.HP <= 0;
    }
    public void DetectEnemiesNearby(float radius)
    {
        //if (IsTargetEmpty())
        {
            //Debug.DrawLine(transform.position, new Vector3(transform.position.x + radius, transform.position.y, transform.position.z));
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2( transform.position.x, transform.position.y), radius);
            //senseEnemies = true;
            //savedLoc = loc;
            UnitBehaviour unit = GetClosestEnemy(hitColliders, radius);
            //Debug.Log("targets " + hitColliders.Length);
            if (unit != null)
            {
                Target = unit;
                Vector3 scale = Img.transform.localScale;
                scale.x *= Target.transform.position.x > transform.position.x ? -1 : 1;
                Img.transform.localScale = scale;
                //transform.Find("img").GetComponent<Animator>().SetBool("meleeIdle" + (scale.x < 0?"":"Flip"), false);
                //transform.Find("img").GetComponent<Animator>().SetBool("meleeIdle" + (scale.x < 0 ? "Flip":""), true);
            }
        }
    }

    private UnitBehaviour GetClosestEnemy(Collider2D[] enemies, float radius)
    {
        UnitBehaviour theUnit = null;
        float minDist = radius;
        Vector3 currentPos = transform.position;
        //Debug.Log("1");
        foreach (Collider2D c in enemies)
        {
            if (c.gameObject == gameObject)
            {
                //Debug.Log("2");
                continue;
            }
            //Debug.Log(string.Format("{0} _ {1}", gameObject.name, c.gameObject.name));
            UnitBehaviour unit = c.GetComponent<UnitBehaviour>();
            if (unit != null && unit.TheTeam != TheTeam)
            {
                //Debug.Log("4");
                Vector3 t = c.transform.position - currentPos;
                float dist = t.x * t.x + t.y * t.y;  // Same as "= t.sqrMagnitude;" but faster
                //Debug.Log("in1 " + dist + " " + minDist);
                if (dist < minDist)
                {
                    //Debug.Log("5");
                    theUnit = unit;
                    minDist = dist;
                }
            }
        }
        return theUnit;
    }
}
