using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    ///<summary> 가드 위치 표시 vector(normalized) </summary>
    Vector2 guardPosVector = new Vector2(1, 0);
    ///<summary> 사격 방향 벡터 </summary>
    Vector2 shoterAttackVector = new Vector2(1, 0);

    ///<summary> 플레이어 공격력 </summary>
    [Header("Player Stat")]
    int atk;
    ///<summary> 플레이어 연사 속도 </summary>
    float fireRate = 4f;
    ///<summary> 플레이어 체력 </summary>
    int health;
    ///<summary> 반사 데미지 </summary>
    int reflect;
    ///<summary> 쉴더 이동 속도 </summary>
    float speed;
    ///<summary> 쉴더 방패 내구도 </summary>
    int shield;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletParent;
    [HideInInspector] public Pool<Bullet> bulletPool = new Pool<Bullet>();
    #endregion Player

    [Header("Bound")]
    [SerializeField] GameObject[] bounds;

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

    #region PlayerShot
    ///<summary> 주기적으로 사격 호출 </summary>
    IEnumerator ShotCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1 / fireRate);
            Shot();
        }
    }
    void Shot()
    {
        Bullet b = bulletPool.GetToken(bulletPrefab, bulletParent);
        b.transform.position = shotPoint.position;

        b.Initialize(bulletPool, shoterAttackVector);
        b.gameObject.SetActive(true);
        b.StartMove();
    }
    #endregion PlayerShot

    #region PlayerMove
    ///<summary> 주기적으로 가드 이동, 사격 회전 호출 </summary>
    IEnumerator MoveCoroutine()
    {
        while(true)
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
           guardPosVector  = Quaternion.AngleAxis(2, Vector3.forward) * guardPosVector;
        else if (angle < 0)
            guardPosVector = Quaternion.AngleAxis(-2, Vector3.forward) * guardPosVector;

        guardRect.position = guardPosVector;
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

    #region EnemySpawn
    IEnumerator SpawnCoroutine()
    {
        int lvlCount = 0;
        while (true)
        {
            lvlCount++;
            if (lvlCount >= 5) { stageLvl++; lvlCount = 0; }

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

    void Spawn(Vector3 spawnPos, bool isWeak)
    {
        Enemy ene = enemyPool.GetToken(enemyPrefab, enemyParent);
        ene.transform.SetParent(enemyParent);
        ene.Initialize(stageLvl, isWeak ? enemyPool : strongEnemyPool, spawnPos);
        ene.gameObject.SetActive(true);
        ene.StartMove();
    }
    #endregion EnemySpawn

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