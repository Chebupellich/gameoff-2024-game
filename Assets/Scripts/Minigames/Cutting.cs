using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cutting : MonoBehaviour
{
    [SerializeField] private List<ProductData> productsToCut;

    private int currentProductIndex = 0;

    private int clicksCount = 0;

    [SerializeField] private TextMeshProUGUI productText;
    [SerializeField] private RectTransform imageRectTransform;
    [SerializeField] private float targetScale;

    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    private bool allProductsCut = false;

    void Start()
    {
        InitializeCurrentProduct();
    }

    void Update()
    {
        if (!allProductsCut && Input.GetKeyDown(KeyCode.G))
        {
            Cut();
        }
    }

    private void Cut()
    {
        ProductData currentProduct = productsToCut[currentProductIndex];

        clicksCount++;

        Vector3 currentScale = imageRectTransform.localScale;
        Vector3 newScale = Vector3.Lerp(currentScale, Vector3.one * targetScale, 1f / currentProduct.needClicks);
        imageRectTransform.localScale = newScale;

        UpdateProgress(currentProduct);

        if (clicksCount >= currentProduct.needClicks)
        {
            CompleteCutting();
        }
    }

    private void CompleteCutting()
    {
        if (currentProductIndex < productsToCut.Count - 1)
        {
            imageRectTransform.localScale = Vector3.one;
            progressBar.value = 1f;

            clicksCount = 0;

            currentProductIndex++;

            InitializeCurrentProduct();
        }
        else
        {
            AllProductsCut();
        }
    }

    private void InitializeCurrentProduct()
    {
        ProductData currentProduct = productsToCut[currentProductIndex];
        productText.text = $"{currentProductIndex + 1}/{productsToCut.Count}";
        progressText.text = currentProduct.needClicks.ToString();
        progressBar.value = 1f;
    }

    private void UpdateProgress(ProductData currentProduct)
    {
        if (clicksCount > currentProduct.needClicks)
        {
            return;
        }

        float normalizedValue = (float)(currentProduct.needClicks - clicksCount) / currentProduct.needClicks;
        progressBar.value = normalizedValue;

        progressText.text = (currentProduct.needClicks - clicksCount).ToString();
    }

    private void AllProductsCut()
    {
        allProductsCut = true;
    }
}

[System.Serializable]
public class ProductData
{
    public string name;
    public int needClicks;
}