//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraSwitcher : MonoBehaviour
//{
//    public GameObject cam1;
//    public GameObject cam2;
//    public GameObject cam3;
//    public GameManager gameManager;
//    void Update()
//    {
//        if (Input.GetButtonDown("1Key"))
//        {
//            cam1.SetActive(true);
//            cam2.SetActive(false);
//            cam3.SetActive(false);
//            gameManager.activeCamera = cam1;

//        }
//        else if (Input.GetButtonDown("2Key"))
//        {
//            cam1.SetActive(false);
//            cam2.SetActive(true);
//            cam3.SetActive(false);
//            gameManager.activeCamera = cam2;

//        }
//        else if (Input.GetButtonDown("3Key"))
//        {
//            cam1.SetActive(false);
//            cam2.SetActive(false);
//            cam3.SetActive(true);
//            gameManager.activeCamera = cam3;

//        }
//    }
//}
