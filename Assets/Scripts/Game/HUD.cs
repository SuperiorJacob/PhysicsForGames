using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public float lerpSpeed = 1;

    public Image healthBar;
    public Text primaryAmmoCount;
    public Text secondaryAmmoCount;

    public float healthTo = 0;
    public float ammoTo = 0;
    public float clipTo = 0;
    public float secondaryTo = 0;

    public float maxHealth = 100;
    private RectTransform healthBarRect;
    private float hbarW = 100;

    public void Assign(float health, float ammo, float clip, float secondary)
    {
        healthTo = health;
        ammoTo = ammo;
        clipTo = clip;
        secondaryTo = secondary;
    }

    void Start()
    {
        hbarW = healthBar.rectTransform.rect.width;
        healthBarRect = healthBar.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarRect.sizeDelta = Vector2.Lerp(healthBarRect.sizeDelta, new Vector2(hbarW * (healthTo / maxHealth), healthBarRect.rect.height), lerpSpeed * Time.deltaTime);

        primaryAmmoCount.text = ammoTo.ToString() + " / " + clipTo.ToString();
        secondaryAmmoCount.text = secondaryTo.ToString();
    }
}
