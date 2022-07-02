using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary> 전투 씬 제어 클래스 </summary>
public class BattleManager : MonoBehaviour
{
    ///<summary> 공격 방향 조절하는 조이스틱 </summary>
    [Header("Joystick")]
    [SerializeField] Joystick attackJoystick;
    ///<summary> 가드 이동 방향 조절하는 조이스틱 </summary>
    [SerializeField] Joystick guardJoystick;

    #region Player
    ///<summary> 사격 총 회전을 처리 </summary>
    [Header("Shoter")]
    [SerializeField] Transform[] shoterArmRect;
    ///<summary> 캐릭터 몸 이미지, 사격 방향에 따라 좌우 반전 처리 </summary>
    [SerializeField] SpriteRenderer shoterBodyRenderer;
    ///<summary> 사격 출발 위치 </summary>
    [SerializeField] Transform shotPoint;

    [Header("Guard")]
    ///<summary> 가드 위치 처리 </summary>
    [SerializeField] Transform guardRect;
    ///<summary> 가드 좌우반전 처리 </summary>
    [SerializeField] SpriteRenderer guardRenderer;
    ///<summary> 가드 위치 표시 vector(normalized) </summary>
    Vector2 guardPosVector = new Vector2(1, 0);
    ///<summary> 사격 방향 벡터 </summary>
    Vector2 shoterAttackVector = new Vector2(1, 0);

    ///<summary> 플레이어 공격력 </summary>
    [Header("Player Stat")]
    public int atk;
    ///<summary> 플레이어 연사 속도 </summary>
    public int fireRate = 4;
    ///<summary> 플레이어 체력 </summary>
    public int health;
    ///<summary> 반사 데미지 </summary>
    public int reflect;
    ///<summary> 쉴더 이동 속도 </summary>
    public int speed;
    ///<summary> 쉴더 방패 내구도 </summary>
    public int shield;

    ///<summary> 총알 프리팹 </summary>
    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    ///<summary> 총알 부모 오브젝트 </summary>
    [SerializeField] Transform bulletParent;
    ///<summary> 총알 풀 </summary>
    Pool<Bullet> bulletPool = new Pool<Bullet>();
    #endregion Player

    ///<summary> 총알 외부 경계 </summary>
    [Header("Bound")]
    [SerializeField] GameObject[] bounds;

    ///<summary> 강한 적 프리팹 </summary>
    [Header("Enemy")]
    [SerializeField] GameObject strongEnemyPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemyParent;

    Pool<Enemy> enemyPool = new Pool<Enemy>();
    Pool<Enemy> strongEnemyPool = new Pool<Enemy>();


    ///<summary> 옵션 버튼 클릭 시 UI 보임 </summary>
    [Header("Option")]
    [SerializeField] GameObject optionPanel;
    bool isPause = false;

    int stageLvl = 1;


    [Header("Score")]
    public int kill = 0;
    int time = 0;
    public bool isEnd = false;

    void Start() 
    {
        StatLoad();
        SetBound();

        StartCoroutine(MoveCoroutine());
        StartCoroutine(ShotCoroutine());
        StartCoroutine(SpawnCoroutine());
    }

    ///<summary> 플레이어 스텟 정보 로드 </summary>
    void StatLoad()
    {
        atk = GameManager.instance.GetCurrStat(0);
        fireRate = GameManager.instance.GetCurrStat(1);
        health = GameManager.instance.GetCurrStat(2);
        reflect = GameManager.instance.GetCurrStat(3);
        speed = GameManager.instance.GetCurrStat(4);
        shield = GameManager.instance.GetCurrStat(5);
    }
    ///<summary> 총알 외부 경계 설정 </summary>
    void SetBound()
    {
        Vector2 scale = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        bounds[0].transform.localScale = new Vector3(-2 * scale.x + 2, 1, 1);
        bounds[0].transform.position = new Vector3(0, 1 - scale.y, 0);

        bounds[1].transform.localScale = new Vector3(1, -2 * scale.y + 2, 1);
        bounds[1].transform.position = new Vector3(1 - scale.x, 0, 0);
        
        bounds[2].transform.localScale = new Vector3(-2 * scale.x + 2, 1, 1);
        bounds[2].transform.position = new Vector3(0, -1 + scale.y, 0);

        bounds[3].transform.localScale = new Vector3(1, -2 * scale.y + 2, 1);
        bounds[3].transform.position = new Vector3(-1 + scale.x, 0, 0);
    }

    #region Player
    #region PlayerShot
    ///<summary> 주기적으로 사격 호출 </summary>
    IEnumerator ShotCoroutine()
    {
        while(!isEnd)
        {
            yield return new WaitForSeconds(1f / fireRate);
            Shot();
        }
    }
    ///<summary> 사격 </summary>
    void Shot()
    {
        Bullet b = bulletPool.GetToken(bulletPrefab, bulletParent);
        b.transform.position = shotPoint.position;

        b.Initialize(atk, bulletPool, shoterAttackVector);
        b.gameObject.SetActive(true);
        b.StartMove();
    }
    #endregion PlayerShot

    #region PlayerMove
    ///<summary> 주기적으로 가드 이동, 사격 회전 호출 </summary>
    IEnumerator MoveCoroutine()
    {
        while(!isEnd)
        {
            if(guardJoystick.joystickVector != Vector2.zero)
                MoveGuardPlayer();
            if(attackJoystick.joystickVector != Vector2.zero)
                RotateGunner();
            yield return new WaitForSeconds(0.03f);
        }
        
    }
    ///<summary> 방어하는 캐릭터 이동 </summary>
    void MoveGuardPlayer()
    {
        //이동 벡터
        Vector2 moveVector = guardJoystick.joystickVector - guardPosVector;
        //이동 벡터 임계값보다 작을 시, 종료
        if(moveVector.magnitude < 0.03f) return;

        //이동 각도 계산
        float angle = Vector3.SignedAngle(guardPosVector, guardJoystick.joystickVector, transform.forward);

        if(angle > 0)
            guardPosVector = Quaternion.AngleAxis(speed * 0.03f, Vector3.forward) * guardPosVector;
        else if (angle < 0)
            guardPosVector = Quaternion.AngleAxis(-speed * 0.03f, Vector3.forward) * guardPosVector;

        guardRect.position = guardPosVector * 1.25f;
        guardRenderer.flipX = guardRect.position.x >= 0;
    }
    ///<summary> 공격하는 캐릭터 회전 </summary>
    void RotateGunner()
    {
        shoterAttackVector = attackJoystick.joystickVector;
        foreach (Transform tr in shoterArmRect)
        {
            tr.GetComponent<SpriteRenderer>().flipY = shoterAttackVector.x < 0;
            tr.rotation = Quaternion.AngleAxis(Mathf.Atan2(attackJoystick.joystickVector.y, attackJoystick.joystickVector.x) * Mathf.Rad2Deg, Vector3.forward);
        }
        shotPoint.localPosition = new Vector3(5.15f, shoterAttackVector.x < 0 ? -0.73f : 0.73f, 0);
        shoterBodyRenderer.flipX = shoterAttackVector.x > 0;
    }
    #endregion PlayerMove

    #region PlayerDamaged
    ///<summary> 적 접촉 시 호출, 쉴드 데미지 </summary>
    public void GetDamage_Guard(int dmg)
    {
        shield -= dmg;
        guardRect.gameObject.SetActive(shield > 0);
    }
    ///<summary> 적 접촉 시 호출, 슈터 데미지 </summary>
    public void GetDamage_Shoter(int dmg)
    {
        health -= dmg;
        if(health <= 0)
            EndGame();
    }
    #endregion PlayerDamaged
    #endregion Player

    #region EnemySpawn
    ///<summary> 적 생성 코루틴 </summary>
    IEnumerator SpawnCoroutine()
    {
        while (!isEnd)
        {
            time++;
            if (time % 5 == 0) stageLvl++;

            int amount = (int)Random.Range(stageLvl * 5f / 6 + 0.5f, stageLvl * 3f / 2 + 0.5f); 
            for(int i = 0; i < amount;i++)
                Spawn(GetPos(Random.Range(0, 5)), true);

            amount = (int)Random.Range(stageLvl * 0.1f, stageLvl * 0.25f);
            for(int i = 0; i < amount;i++)
                Spawn(GetPos(Random.Range(0, 5)), false);
            yield return new WaitForSeconds(1f);
        }

        Vector3 GetPos(int rand)
        {
            Vector2 scale = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            switch(rand)
            {
                case 0:
                    return new Vector3(Random.Range(scale.x, -scale.x), -scale.y + 1, 0);
                case 1:
                    return new Vector3(-scale.x + 1, Random.Range(scale.y, -scale.y), 0);
                case 2:
                    return  new Vector3(Random.Range(scale.x, -scale.x), scale.y - 1, 0);
            }
            return new Vector3(scale.x - 1, Random.Range(scale.y, -scale.y), 0);
        }
    }

    ///<summary> 적 생성 </summary>
    ///<param name="spawnPos"> 생성 위치 </param>
    ///<param name="isWeak"> 강적 여부 </param>
    void Spawn(Vector3 spawnPos, bool isWeak)
    {
        Enemy ene = isWeak ? enemyPool.GetToken(enemyPrefab, enemyParent) : strongEnemyPool.GetToken(strongEnemyPrefab, enemyParent);
        ene.transform.SetParent(enemyParent);
        ene.Initialize(this, stageLvl, isWeak ? enemyPool : strongEnemyPool, spawnPos);
        ene.gameObject.SetActive(true);
        ene.StartMove();
    }
    #endregion EnemySpawn

    ///<summary> 슈터 체력 0 이하일 때 호출, 게임 종료 </summary>
    void EndGame()
    {
        isEnd = true;
        Debug.Log("game End");
        Debug.Log($"time : {time}, kill : {kill}");
    }

    ///<summary> 옵션 버튼 </summary>
    public void Btn_Pause()
    {
        isPause = ! isPause;
        //시간 정지
        Time.timeScale = isPause ? 0 : 1;
        //옵션 UI 보이기/끄기
        optionPanel.SetActive(isPause);
    }
}