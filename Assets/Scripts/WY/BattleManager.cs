using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary> 전투 씬 제어 클래스 </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    ///<summary> 공격 방향 조절하는 조이스틱 </summary>
    [Header("Joystick")]
    [SerializeField] Joystick attackJoystick;
    ///<summary> 가드 이동 방향 조절하는 조이스틱 </summary>
    [SerializeField] Joystick guardJoystick;

    ///<summary> 사격 총 회전을 처리 </summary>
    [Header("Character")]
    [SerializeField] Transform gunnerRect;
    ///<summary> 가드 위치 처리 </summary>
    [SerializeField] Transform guardRect;
    ///<summary> 가드 위치 표시 vector(normalized) </summary>
    Vector2 guardPosVector = new Vector2(1, 0);
    ///<summary> 사격 방향 벡터 </summary>
    Vector2 shoterAttackVector = new Vector2(1, 0);

    [Header("Player Stat")]
    float shotDelay = 7f;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletParent;
    [HideInInspector] public Pool<Bullet> bulletPool = new Pool<Bullet>();

    [Header("Bound")]
    [SerializeField] GameObject[] bounds;

    ///<summary> 옵션 버튼 클릭 시 UI 보임 </summary>
    [Header("Option")]
    [SerializeField] GameObject optionPanel;
    bool isPause = false;

    private void Awake() => instance = this;

    void Start() 
    {
        SetBound();
        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Debug.Log(pos);

        StartCoroutine(MoveCoroutine());
        StartCoroutine(ShotCoroutine());
    }

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
            yield return new WaitForSeconds(1 / shotDelay);
            Shot();
        }
    }
    void Shot()
    {
        Bullet b = bulletPool.GetToken(bulletPrefab, bulletParent);
        b.transform.position = gunnerRect.position;

        b.SetDirection(shoterAttackVector);
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
        gunnerRect.rotation = Quaternion.AngleAxis(Mathf.Atan2(attackJoystick.joystickVector.y, attackJoystick.joystickVector.x) * Mathf.Rad2Deg, Vector3.forward);
    }
    #endregion PlayerMove

    #region EnemySpawn
    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
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