using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    public AliveObject target;
    public GameObject bar;

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTrans = bar.GetComponent<RectTransform>();

        if (!rectTrans)
        {
            Debug.LogWarning("Can't find RectTransform in bar");

            return;
        }

        float p = 0.0f;

        if (target.maxHealth > 0.0f)
        {
            p = target.health / target.maxHealth;
        }

        rectTrans.localScale = new Vector3(p, 1, 1);
    }
}
