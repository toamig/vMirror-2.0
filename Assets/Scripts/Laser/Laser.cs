using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{

    public Transform planeTransform;

    private LineRenderer lr;

    public LineRenderer reflected;

    public GameObject mirrorCamera;

    private LayerMask rayIgnore;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.startWidth = 0.100f;
        lr.endWidth = 0.100f;

        reflected.startWidth = 0.100f;
        reflected.endWidth = 0.100f;

        rayIgnore |= (1 << LayerMask.NameToLayer("Mirror"));
        rayIgnore |= (1 << LayerMask.NameToLayer("Laser"));

    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;

        RaycastHit[] reflectionHits;

        // Raycast laser from hand
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(0, new Vector3(0, 0, transform.localPosition.z));
                lr.SetPosition(1, new Vector3(0, 0, hit.distance));

                // Laser hits the mirror
                if (hit.collider.gameObject.layer == 6)
                {
                    reflected.gameObject.SetActive(true);

                    Vector3 reflectionDir = (hit.point - mirrorCamera.transform.position).normalized;

                    

                    //reflected.gameObject.transform.position = mirrorCamera.transform.position;
                    reflected.gameObject.transform.LookAt(hit.point + reflectionDir);

                    Ray ray = new Ray(mirrorCamera.gameObject.transform.position, reflectionDir);

                    // raycast from mirrorCamera
                    reflectionHits = Physics.RaycastAll(ray);

                    RaycastHit[] orderedHits = OrderByDistance(reflectionHits);


                    int startIndex = FindMirrorIndex(orderedHits);

                    if (reflectionHits.Length > startIndex)
                    {
                        reflected.SetPosition(0, new Vector3(0, 0, orderedHits[startIndex].distance));
                        reflected.SetPosition(1, new Vector3(0, 0, orderedHits[startIndex + 1].distance));

                        AddOutline(orderedHits[startIndex + 1].collider.gameObject);
                        if (orderedHits[startIndex + 1].transform.gameObject.tag.Equals("Ball"))
                        {
                            SelectEventSystem.current.TargetSelected(orderedHits[startIndex + 1].transform.gameObject.name);
                        }
                        else
                        {
                            SelectEventSystem.current.TargetSelected("Miss");
                        }
                    }
                    else
                    {
                        reflected.SetPosition(0, new Vector3(0, 0, orderedHits[0].distance));
                        reflected.SetPosition(1, new Vector3(0, 0, 5000));
                    }

                }
                else
                {
                    reflected.SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
                    AddOutline(hit.collider.gameObject);
                }

            }
            else
            {
                reflected.SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
            }
        }
        else
        {
            lr.SetPosition(1, new Vector3(0, 0, 5000));
            reflected.SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
        }

    }

    public void AddOutline(GameObject go)
    {
        DisablePreviousOutlines();

        if (go.TryGetComponent(out Outline outline))
        {
            outline.enabled = true;
        }
        else
        {
            go.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineWidth = 3;
        }

        if (gameObject.tag == "Ball")
        {
            outline.OutlineColor = Color.green;

            //TODO increment found balls
        }
        else
        {
            outline.OutlineColor = Color.red;

            //TODO increment missed 
        }
    }

    public void DisablePreviousOutlines()
    {
        Outline[] outlines = GameObject.FindObjectsOfType<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }

    public int FindMirrorIndex(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.gameObject.layer == 6)
            {
                return i;
            }
        }
        return -1;
    }

    public void OnDisable()
    {
        reflected.gameObject.SetActive(false);
    }

    public RaycastHit[] OrderByDistance(RaycastHit[] hits)
    {
        List<RaycastHit> hitsList = hits.ToList<RaycastHit>();

        hitsList.Sort((hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

        RaycastHit[] orderedHits = hitsList.ToArray();

        return orderedHits;
    }
}
