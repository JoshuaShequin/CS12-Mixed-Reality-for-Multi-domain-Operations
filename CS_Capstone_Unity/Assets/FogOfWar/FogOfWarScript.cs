﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarScript : MonoBehaviour
{

    public GameObject m_fogOfWarPlane;
    public string AllyUnitName;
    public string EnemyUnitName;
    public GameObject[] units;
    public GameObject[] EnemyUnits;
    public LayerMask m_fogLayer;
    public float m_radius = 5f;
    private float m_radiusSqr {  get { return m_radius * m_radius; } }

    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private Color[] m_colors;

    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        units = GameObject.FindGameObjectsWithTag(AllyUnitName);
        EnemyUnits = GameObject.FindGameObjectsWithTag(EnemyUnitName);

        

        foreach (GameObject Ally in units)
        {
            Vector3 above_pos = Ally.transform.position;
            above_pos.y = transform.position.y;
            Ray r = new Ray(above_pos, Ally.transform.position - above_pos);
            RaycastHit hit;
            
            if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
            {
                //Debug.Log("length of vertices");
                //Debug.Log(m_vertices.Length);
                for (int i = 0; i < m_vertices.Length; i++)
                {
                    
                    Vector3 v = m_fogOfWarPlane.transform.TransformPoint(m_vertices[i]);
                    //Debug.Log("v");
                    //Debug.Log(v);
                    //Debug.Log("hit point");
                    //Debug.Log(hit.point);
                    float dist = Vector3.SqrMagnitude(v - hit.point);
                    //Debug.Log("DIST");
                    //Debug.Log(dist);
                    if (dist < m_radiusSqr)
                    {
                        
                        float alpha = Mathf.Min(m_colors[i].a, dist / m_radiusSqr);
                        m_colors[i].a = alpha;
                        
                    }
                }
                UpdateColor();
            }
        }
        
        foreach (GameObject Enemy in EnemyUnits)
        {
            EnemyBehavior eb = Enemy.GetComponent<EnemyBehavior>();
            // Debug.Log(eb);
            if (eb.state == EnemyBehavior.STATE.ATTACKING)
            {
                Vector3 above_pos = Enemy.transform.position;
                above_pos.y = transform.position.y;
                Ray r = new Ray(above_pos, Enemy.transform.position - above_pos);
                RaycastHit hit;

                if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
                {
                    //Debug.Log("length of vertices");
                    //Debug.Log(m_vertices.Length);
                    for (int i = 0; i < m_vertices.Length; i++)
                    {

                        Vector3 v = m_fogOfWarPlane.transform.TransformPoint(m_vertices[i]);
                        //Debug.Log("v");
                        //Debug.Log(v);
                        //Debug.Log("hit point");
                        //Debug.Log(hit.point);
                        float dist = Vector3.SqrMagnitude(v - hit.point);
                        //Debug.Log("DIST");
                        //Debug.Log(dist);
                        if (dist < m_radiusSqr)
                        {

                            float alpha = Mathf.Min(m_colors[i].a, dist / m_radiusSqr);
                            m_colors[i].a = alpha;

                        }
                    }
                    UpdateColor();
                }
            }
            
        }
        


    }

    void Initialize()
    {
        m_mesh = m_fogOfWarPlane.GetComponent<MeshFilter>().mesh;
        m_vertices = m_mesh.vertices;
        m_colors = new Color[m_vertices.Length];
        for (int i=0; i < m_colors.Length; i++)
        {
            m_colors[i] = Color.black;
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        m_mesh.colors = m_colors;
    }

    public void toggleFogOfWar()
    {
        active = !active;
        m_fogOfWarPlane.SetActive(active);
    }
}
