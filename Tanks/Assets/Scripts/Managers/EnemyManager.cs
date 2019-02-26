using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] //si no estiguera aço no se vorien les variables publiques perque TankManager no es Monodevelop
public class EnemyManager{

    public Color m_EnemyColor;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_EnemyNumber;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public int m_Wins;


    private TankMovement m_Movement;
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        //m_Movement.m_PlayerNumber = m_EnemyNumber;
        //m_Shooting.m_PlayerNumber = m_EnemyNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_EnemyColor) + ">ENEMY " + m_EnemyNumber + "</color>"; //colorejem el text de forma HTML

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_EnemyColor;
        }
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
