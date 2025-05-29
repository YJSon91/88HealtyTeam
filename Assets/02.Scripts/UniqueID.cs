// UniqueID.cs (새 C# 스크립트, PickupableItem 오브젝트에 붙여줌)
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    public string id;

#if UNITY_EDITOR
    // 에디터에서 값이 바뀔 때 또는 오브젝트가 복제될 때 ID 자동 생성/유지
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id) || UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
        {
            // 프리팹 상태에서는 ID를 생성하지 않거나, 프리팹 인스턴스화 시점에 생성하도록 유도할 수 있음
            // 간단하게는, 씬에 배치된 후 ID가 비어있으면 그때 생성
            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && gameObject.scene.name != null)
            { // 씬에 있는 오브젝트일 경우
                id = System.Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
    }
    // 프리팹 인스턴스가 씬에 추가될 때 ID를 새로 발급받도록 하는 로직 (더 복잡할 수 있음)
    // 현재 OnValidate는 프리팹 에셋 자체를 수정할 수 있으므로 주의해서 사용하거나,
    // 런타임에 ID를 관리하는 별도 시스템을 두는 것이 더 안전할 수 있습니다.
    // 가장 간단한 방법은 씬에 배치된 각 아이템의 Inspector에서 uniqueID를 수동으로 입력하거나,
    // 위 OnValidate를 사용하되, 프리팹을 수정하지 않도록 주의하는 것입니다.
#endif
}