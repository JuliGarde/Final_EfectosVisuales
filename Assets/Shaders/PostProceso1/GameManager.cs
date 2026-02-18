using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (URPPostProcessFeature.Instance == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            URPPostProcessFeature.Instance.activeEffect =
                URPPostProcessFeature.EffectType.Effect1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            URPPostProcessFeature.Instance.activeEffect =
                URPPostProcessFeature.EffectType.Effect2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            URPPostProcessFeature.Instance.activeEffect =
                URPPostProcessFeature.EffectType.None;
        }
    }
}
