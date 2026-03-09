using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class Myui : CharacterBase
{
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer hatRenderer;
    [SerializeField] private SpriteRenderer topRenderer;
    [SerializeField] private SpriteRenderer dressRenderer;
    [SerializeField] private SpriteRenderer bottomRenderer;
    [SerializeField] private SpriteRenderer gloveRenderer;
    [SerializeField] private SpriteRenderer shoeRenderer;
    [SerializeField] private SpriteRenderer sockRenderer;
    private Dictionary<SpriteRenderer, Sprite[]> renderers = new Dictionary<SpriteRenderer, Sprite[]>();

    private Sprite[] Bottoms;
    private Sprite[] Dresses;
    private Sprite[] Gloves;
    private Sprite[] Hairs;
    private Sprite[] Hats;
    private Sprite[] Shoes;
    private Sprite[] Socks;
    private Sprite[] Tops;

    

    protected override void Start()
    {
        Bottoms = Resources.LoadAll<Sprite>("Clothing/Bottoms");
        Dresses = Resources.LoadAll<Sprite>("Clothing/Dresses");
        Gloves = Resources.LoadAll<Sprite>("Clothing/Gloves");
        Hairs = Resources.LoadAll<Sprite>("Clothing/Hair");
        Hats = Resources.LoadAll<Sprite>("Clothing/Hats");
        Shoes = Resources.LoadAll<Sprite>("Clothing/Shoes");
        Socks = Resources.LoadAll<Sprite>("Clothing/Socks");
        Tops = Resources.LoadAll<Sprite>("Clothing/Tops");

        renderers.Add(hairRenderer, Hairs);
        renderers.Add(hatRenderer, Hats);
        renderers.Add(topRenderer, Tops);
        renderers.Add(dressRenderer, Dresses);
        renderers.Add(bottomRenderer, Bottoms);
        renderers.Add(gloveRenderer, Gloves);
        renderers.Add(shoeRenderer, Shoes);
        renderers.Add(sockRenderer, Socks);

        base.Start();

    }

    public void MoveMyui()
    {
        transform.position = targetPosition;
    }

    protected override void FixedUpdate()
    {
        if (currentState == CharacterState.Moving)
        {
            animator.SetTrigger("isMoving");
        }
    }

    protected override IEnumerator PerformUniqueAction()
    {
        if (currentState == CharacterState.Moving)
            yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("isMoving");
        yield return new WaitForSeconds(0.4f);
        int randomClothingOption;
        Sprite clothingSelected;
        int coinFlip;
        bool wearingDress = true;
        foreach (KeyValuePair<SpriteRenderer, Sprite[]> renderer in renderers)
        {
            randomClothingOption = Random.Range(0, renderer.Value.Length);
            clothingSelected = renderer.Value[randomClothingOption];
            if (renderer.Key.gameObject.name == "Dresses")
            {
                coinFlip = Random.Range(0, 2);
                if (coinFlip == 0)
                {
                    clothingSelected = null;
                    wearingDress = false;
                }
            }
            renderer.Key.sprite = clothingSelected;
        }
        if (wearingDress)
        {
            topRenderer.sprite = null;
            bottomRenderer.sprite = null;
        }
        yield return null;
    }
}
