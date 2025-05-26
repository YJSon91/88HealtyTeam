using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;// CharacterManager의 인스턴스를 저장할 정적 변수
    public static CharacterManager Instance// CharacterManager의 인스턴스를 반환하는 프로퍼티
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return instance;
        }
    }

    public Player player;// 현재 플레이어 캐릭터를 나타내는 Player 클래스의 인스턴스

    public Player Player// 플레이어 캐릭터를 가져오거나 설정하는 프로퍼티
    {
        get { return player; }
        set { player = value; }
    }

    private void Awake()// CharacterManager의 인스턴스를 초기화하는 메서드, 방어적 싱글턴 패턴을 사용하여 인스턴스를 관리합니다.
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}