﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Event;
using System.Collections.Generic;
using System;
using Assets.Scripts.Role;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SosHandle : MonoBehaviour
{
    public float shieldArea = 5;

    public Transform m_Transform;
    public float m_Radius = 2; // 圆环的半径
    public float m_Theta = 0.1f; // 值越低圆环越平滑
    public Color m_Color = Color.green; // 线框颜色
    private bool mInInterestRegion = false;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 pos = transform.position;
        pos.y += 1;
        //Gizmos.DrawSphere(pos, 0.3f);
        //Gizmos.DrawLine(new Vector3(-100, 0, -100), new Vector3(100, 0, 100));
        //DrawGizmosCircle(0.01f);

#if UNITY_EDITOR
        //Handles.color = Color.blue;
        //Handles.ArrowCap(0, transform.position, transform.rotation, transform.localScale.z);
        //Handles.Disc(transform.rotation, transform.position, Vector3.up, transform.localScale.z * 0.5f, false, 1);
#endif
    }

    public void OnDrawGizmosSelected()
    {
        //Debug.Log("OnDrawGizmosSelected begin");
        Gizmos.color = Color.blue;
        Vector3 pos = transform.position;
        pos.y += 1;
        //Gizmos.DrawSphere(pos, 0.3f);
        //Gizmos.DrawLine(new Vector3(-100, 0, -100), new Vector3(100, 0, 100));
        DrawGizmosCircle(0.01f);
        if (mInInterestRegion)
        {
            Vector3 spos = transform.position;
            spos.x -= m_Radius;
            spos.y += 0.01f;
            Vector3 epos = transform.position;
            epos.x += m_Radius;
            epos.y += 0.01f;
            Gizmos.color = new Color(64, 255, 0);
            Gizmos.DrawLine(spos, epos);

            spos = transform.position;
            spos.z -= m_Radius;
            spos.y += 0.01f;
            epos = transform.position;
            epos.z += m_Radius;
            epos.y += 0.01f;
            Gizmos.DrawLine(spos, epos);
        }
    }

    void Update()
    {
        ICollection<SosObject> targets = SosEventMgr.GetInstance().GetTargetList();
        IEnumerator i = targets.GetEnumerator();
        i.Reset();

        while (i.MoveNext())
        {
            SosObject obj = (SosObject)i.Current;
            if ((obj.transform.position - transform.position).magnitude <= m_Radius)
            {
                mInInterestRegion = true;
                break;
            }
            else
            {
                mInInterestRegion = false;
            }
        }

        if (mInInterestRegion)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //ICollection<Transform> targets = SosEventMgr.GetInstance().GetTargetList();
                //IEnumerator i = targets.GetEnumerator();
                i.Reset();
                float angleOff = 0.0f;
                while (i.MoveNext())
                {
                    SosObject player = (SosObject)i.Current;
                    Transform playerTrans = player.transform;
                    Debug.Log("SosHandle Update 1======================== " + transform.rotation.eulerAngles + " " + playerTrans.rotation.eulerAngles);
                    //check if is inverse direction
                    angleOff = (transform.rotation.eulerAngles.y - playerTrans.rotation.eulerAngles.y);
                    if (angleOff > 90 || angleOff < -90)
                    {
                        //check who is after who.
                        Vector3 playerToHost = transform.position - playerTrans.position;
                        Quaternion quat = Quaternion.LookRotation(playerToHost);
                        angleOff = quat.eulerAngles.y - transform.rotation.eulerAngles.y;
                        if (angleOff > 90 || angleOff < -90)
                        {
                            Debug.Log("SosHandle Update 2========================");
                        }
                    }
                    else
                    {
                        //if player is after eventHost, eventHost turn around
                        Vector3 playerToHost = transform.position - playerTrans.position;
                        Quaternion quat = Quaternion.LookRotation(playerToHost);
                        angleOff = quat.eulerAngles.y - transform.rotation.eulerAngles.y;
                        if (angleOff < 90 && angleOff > -90)
                        {
                            Debug.Log("SosHandle Update 3========================");
                        }
                    }
                    //if player is back to eventHost, nothing happen
                }
            }
        }
    }

    private void DrawGizmosCircle(float yOff)
    {
        if (transform == null) return;
        if (m_Theta < 0.0001f) m_Theta = 0.0001f;

        // 设置矩阵
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        // 设置颜色
        Color defaultColor = Gizmos.color;
        Gizmos.color = m_Color;

        // 绘制圆环
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = m_Radius * Mathf.Cos(theta);
            float z = m_Radius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, yOff, z);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }

        // 绘制最后一条线段
        Gizmos.DrawLine(firstPoint, beginPoint);

        // 恢复默认颜色
        Gizmos.color = defaultColor;

        // 恢复默认矩阵
        Gizmos.matrix = defaultMatrix;
    }
}