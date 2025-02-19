using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    [SerializeField] GameObject maximizedPanel;
    [SerializeField] Image maximizedImage;
    [SerializeField] RectTransform targetGalleryRectTransform;
    [SerializeField] GameObject GalleryButtonPrefab;
    [SerializeField] string imageFolderPath;
    
    [SerializeField] int buttonWidth;

    List<Texture2D> baseImages;
    int currentId = 0;

    void Awake(){
        baseImages = new List<Texture2D>();
        GetButtonImages();
        GenerateButtons();
    }

    void GenerateButtons(){
        int imageCount = 0;
        Action<int> buttonFunction = LoadMaximizedPanel;
        foreach (Texture2D image in baseImages){
            
            int imageWidth = image.width;
            int imageHeight = image.height;
            float ratio = (float)imageWidth / (float)imageHeight;

             Sprite imageSprite = Sprite.Create(
                    image, 
                    new Rect(0.0f, 0.0f, imageWidth, imageHeight), 
                    new Vector2(0.5f, 0.5f)
                );

            GameObject galleryButtonMask = Instantiate(GalleryButtonPrefab);
            GameObject galleryButton = galleryButtonMask.transform.GetChild(0).GameObject();

            galleryButton.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
            galleryButton.GetComponent<Image>().sprite = imageSprite;

            galleryButtonMask.transform.SetParent(targetGalleryRectTransform);

            int loopCount = imageCount;
            galleryButtonMask.GetComponent<Button>().onClick.AddListener(() => buttonFunction(loopCount));
            imageCount++;
        }
    }

    void LoadMaximizedPanel(int imageId){
        currentId = imageId;
        RefreshImage();
        maximizedPanel.SetActive(true);
    }

    void RefreshImage(){
        Texture2D newImage = baseImages[currentId];

        float imageWidth = newImage.width;
        float imageHeight = newImage.height;
        maximizedImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)newImage.width / (float)newImage.height;
        Sprite imageSprite = Sprite.Create(
                    newImage, 
                    new Rect(0.0f, 0.0f, imageWidth, imageHeight), 
                    new Vector2(0.5f, 0.5f)
                );
        maximizedImage.rectTransform.sizeDelta = new Vector2(imageWidth, imageWidth);
        maximizedImage.sprite = imageSprite;
    }

    public void ChangeImage(int ammount){
        currentId += ammount;
        if(currentId < 0){
            currentId = baseImages.Count;
        }
        else if(currentId > baseImages.Count-1){
            currentId = 0;
        }
        RefreshImage();
    }
    public void TurnOffMaximizedPanel(){
        maximizedPanel.SetActive(false);
    }
    void GetButtonImages(){
        if (Directory.Exists(imageFolderPath)){
            string[] images = Directory.GetFiles(imageFolderPath);

            foreach (string img in images){
                try{
                    byte[] imageData = File.ReadAllBytes(img);
                    Texture2D imageTexture = new Texture2D(2,2);
                    imageTexture.LoadImage(imageData);
                    baseImages.Add(imageTexture);
                }
                catch (Exception e){
                    Debug.Log(e.Message);
                }
            }
        }
    }

}
