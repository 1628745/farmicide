using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Target target;
    public GameObject bullet;
    public Transform shootPoint, gun;
    public float reloadTime = 1f;
    public int tooFar = 10;
    public int segments = 50; // Number of line segments to approximate the circle
    public LineRenderer line;
    public Material lineMaterial;
    
    private bool _canShoot = true;
    private Target _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = GetComponent<Target>();
        line = gameObject.GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        DrawPolygon(segments, Mathf.Sqrt(tooFar), Vector3.zero, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target && (target.transform.position - transform.position).sqrMagnitude > tooFar)
        {
            target = null;
        }
        if (target)
        {
            var dir = target.transform.position-transform.position;
            gun.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg);
            if (_canShoot) Shoot();
        }
        else
        {
            gun.rotation = Quaternion.Euler(0f, 0f, 0f);
            FindTarget();
        }
    }

    void Shoot()
    {
        if (!_canShoot) return;
        var b = Instantiate(bullet, shootPoint.position, Quaternion.identity, transform);
        b.transform.rotation = gun.rotation;
        b.GetComponent<Bullet>().owner = _target.owner;
        
        _canShoot = false;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        _canShoot = true;
    }

    void FindTarget()
    {
        target = null;
        var dist = float.MaxValue;

        foreach (var t in FindObjectsOfType<Target>())
        {
            if (t.owner != _target.owner)
            {
                var sqm = (t.transform.position - transform.position).sqrMagnitude;
                if (sqm < dist && sqm < tooFar)
                {
                    target = t;
                    dist = sqm;
                }
            }
        }
    }
    
    void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth)
    {
        line.startWidth = startWidth;
        line.endWidth = endWidth;
        line.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        line.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1));
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            line.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));

        }
    }
}
