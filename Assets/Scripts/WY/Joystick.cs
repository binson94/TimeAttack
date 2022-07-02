using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ///<summary> 현재 joystick 조종 중 상태 표시 </summary>
    public bool isInput = false;

    ///<summary> 조이스틱 위치 조절을 위한 파라미터, 왼쪽 기준 정렬일 시 1, 오른쪽 기준 정렬일 시 -1 </summary>
    [SerializeField] int xVector;

    ///<summary> 조이스틱 크기 설정 값 </summary>
    float size = 1;
    float radius;
    [SerializeField] RectTransform lever;
    [SerializeField] RectTransform joystickBG;

    public Vector2 joystickVector;

    ///<summary> 조이스틱 크기 설정 불러오기 </summary>
    private void Start() 
    {
        //레버 사이즈 조절
        lever.sizeDelta = new Vector2(75 * (1 + size), 75 * (1 + size));
        //레버 배경 사이즈 조절
        joystickBG.sizeDelta = new Vector2(200 * (1 + size), 200 * (1 + size));
        //레버 배경 위치 조절
        joystickBG.anchoredPosition = new Vector2(xVector * (50 + joystickBG.rect.width / 2), 50 + joystickBG.rect.height / 2);
        //반지름 계산
        radius = (joystickBG.rect.width - lever.rect.width) / 2;
    }

    ///<summary> 조이스틱 드래그 시작 </summary>
    public void OnBeginDrag(PointerEventData pointData)
    {
        isInput = true;
        MoveJoystickHandle(pointData);
    }

    ///<summary> 조이스틱 드래그 중 </summary>
    public void OnDrag(PointerEventData pointData)
    {
        MoveJoystickHandle(pointData);
    }

    ///<summary> 조이스틱 드래그 끝 </summary>
    public void OnEndDrag(PointerEventData pointData)
    {
        lever.anchoredPosition = Vector2.zero;
        joystickVector = Vector2.zero;
        isInput = false;
    }

    ///<summary> 조이스틱 드래그 중인 경우, 위치 설정 및 벡터 계산 </summary>
    void MoveJoystickHandle(PointerEventData pointData)
    {
        Vector2 pointPos =  xVector > 0 ? pointData.position : pointData.position - new Vector2(Screen.width, 0);

        Vector2 inputVector = pointPos - joystickBG.anchoredPosition;
        if(inputVector.magnitude > radius)
            inputVector = inputVector.normalized * radius;
        lever.anchoredPosition = inputVector;

        joystickVector = inputVector.normalized;
    }
}
