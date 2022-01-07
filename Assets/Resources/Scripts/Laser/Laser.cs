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
                    Vector3 reflectionDir = (hit.point - mirrorCamera.transform.position).normalized;

                    reflected.gameObject.transform.position = mirrorCamera.transform.localPosition;
                    reflected.gameObject.transform.LookAt(hit.point + reflectionDir);

                    Ray ray = new Ray(mirrorCamera.gameObject.transform.position, reflectionDir);

                    // raycast from mirrorCamera
                    reflectionHits = Physics.RaycastAll(ray);

                    reflected.SetPosition(0, new Vector3(0, 0, reflectionHits[0].distance));


                    if (reflectionHits.Length > 1)
                    {
                        //TODO: ordenar hits
                        reflected.SetPosition(0, new Vector3(0, 0, reflectionHits[0].distance));
                        reflected.SetPosition(1, new Vector3(0, 0, reflectionHits[1].distance));
                    }
                    else
                    {
                        reflected.SetPosition(0, new Vector3(0, 0, reflectionHits[0].distance));
                        reflected.SetPosition(1, new Vector3(0, 0, 5000));
                    }

                }
                else
                {
                    reflected.SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
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

    public RaycastHit[] OrderByDistance(RaycastHit[] hits)
    {
        List<RaycastHit> hitsList = hits.ToList<RaycastHit>();

        hitsList.Sort((hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

        return hitsList.ToArray();
    }
}
