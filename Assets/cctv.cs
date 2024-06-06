using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using Cinemachine;

public class cctv : MonoBehaviour
{
    public string m_ScreenCapturePath;
    public Camera screenshotCamera;
    public Image prefab;
    int textureWidth = UnityEngine.Device.Screen.width; // 렌더 텍스처의 가로 크기
    int textureHeight = UnityEngine.Device.Screen.height; // 렌더 텍스처의 세로 크기
   public CinemachineMixingCamera mixingCamera;

    // Start is called before the first frame update
    private void MakeImage()
    {
        DirectoryInfo di = new DirectoryInfo(m_ScreenCapturePath);

        foreach (FileInfo file in di.GetFiles())
        {
            var texture = new Texture2D(0, 0);
            Debug.Log("파일명 : " + file.Name);
             var f = File.ReadAllBytes(file.FullName);
              texture.LoadImage(f);
            prefab.sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
            prefab.GetComponentInChildren<Text>().text = file.Name;
            GameObject.Instantiate(prefab,prefab.transform.parent);

        }
    }
    private void TakeScreenshot()
    {
        if (!Directory.Exists(m_ScreenCapturePath))
            Directory.CreateDirectory(m_ScreenCapturePath);
        if (screenshotCamera != null)
        {
            screenshotCamera.rect = new Rect(0, 0, 1, 1);
            screenshotCamera.targetTexture = new RenderTexture(textureWidth, textureHeight, 24);
            // 카메라 렌더링
            screenshotCamera.Render();

            // 현재 렌더러 텍스처를 가져와 스크린샷 찍기
            Texture2D screenshotTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            // RenderTexture에서 Texture2D로 변환
            RenderTexture.active = screenshotCamera.targetTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0, false);
            // RenderTexture 및 카메라 설정 초기화
            RenderTexture.active = null;
            screenshotCamera.targetTexture = null;
            // PNG 형식으로 스크린샷 저장
            byte[] bytes = screenshotTexture.EncodeToJPG();
            File.WriteAllBytes(m_ScreenCapturePath + DateTime.Now.ToString("yyyyMMddhhmmss")+".jpg", bytes);

            Debug.Log("Screenshot saved to: " + m_ScreenCapturePath + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg");
        }
        else
        {
            Debug.LogError("Screenshot camera is not assigned!");
        }
    }
    public void cameraset1()
    {
        mixingCamera.m_Weight0 = 2.0f;
        mixingCamera.m_Weight1 = 0.0f;
        mixingCamera.m_Weight2 = 0.0f;
        mixingCamera.m_Weight3 = 0.0f;
    }
    public void cameraset2()
    {
        mixingCamera.m_Weight0 = 0.0f;
        mixingCamera.m_Weight1 = 2.0f;
        mixingCamera.m_Weight2 = 0.0f;
        mixingCamera.m_Weight3 = 0.0f;
    }
    public void cameraset3()
    {
        mixingCamera.m_Weight0 = 0.0f;
        mixingCamera.m_Weight1 = 0.0f;
        mixingCamera.m_Weight2 = 2.0f;
        mixingCamera.m_Weight3 = 0.0f;
    }
    public void cameraset4()
    {
        mixingCamera.m_Weight0 = 0.0f;
        mixingCamera.m_Weight1 = 0.0f;
        mixingCamera.m_Weight2 = 0.0f;
        mixingCamera.m_Weight3 = 2.0f;
    }
}
