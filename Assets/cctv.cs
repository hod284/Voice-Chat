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
    int textureWidth = UnityEngine.Device.Screen.width; // ���� �ؽ�ó�� ���� ũ��
    int textureHeight = UnityEngine.Device.Screen.height; // ���� �ؽ�ó�� ���� ũ��
   public CinemachineMixingCamera mixingCamera;

    // Start is called before the first frame update
    private void MakeImage()
    {
        DirectoryInfo di = new DirectoryInfo(m_ScreenCapturePath);

        foreach (FileInfo file in di.GetFiles())
        {
            var texture = new Texture2D(0, 0);
            Debug.Log("���ϸ� : " + file.Name);
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
            // ī�޶� ������
            screenshotCamera.Render();

            // ���� ������ �ؽ�ó�� ������ ��ũ���� ���
            Texture2D screenshotTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            // RenderTexture���� Texture2D�� ��ȯ
            RenderTexture.active = screenshotCamera.targetTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0, false);
            // RenderTexture �� ī�޶� ���� �ʱ�ȭ
            RenderTexture.active = null;
            screenshotCamera.targetTexture = null;
            // PNG �������� ��ũ���� ����
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
