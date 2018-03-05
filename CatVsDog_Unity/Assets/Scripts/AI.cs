using UnityEngine;
using System.Collections;


public class AI : MonoBehaviour
{
    public Transform throwPoint; // the position where the object is thrown
    public Transform target;
    public GameObject bone;
    private float xVelocity, yVelocity;
    public float timeHit = 3f; // in second
    public float accuracyDiviation = 2f;

    private Transform boneTransform;
    private Movement pMovement;
    private float walkingDistance;
    private bool _foundBone = false;

    public bool foundBone
    {
        get { return _foundBone; }
        set
        {
            _foundBone = value;
        }
    }

    public void Throw()
    {
        float xdistance;
        xdistance = target.position.x - throwPoint.position.x;

        //Make the throwing less accurate
        xdistance = Random.Range(xdistance - accuracyDiviation, xdistance + accuracyDiviation);

        float ydistance;
        ydistance = target.position.y - throwPoint.position.y;

        float throwAngle; // in radian
        throwAngle = Mathf.Atan((ydistance + 4.905f * (Mathf.Pow(timeHit, 2))) / xdistance);

        float totalVelocity = xdistance / (Mathf.Cos(throwAngle) * timeHit);

        xVelocity = totalVelocity * Mathf.Cos(throwAngle);
        yVelocity = totalVelocity * Mathf.Sin(throwAngle);

        GameObject boneInstance = Instantiate(bone, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        boneInstance.GetComponent<Ammo>().throwingObj = this.gameObject;

        Rigidbody2D rigid;
        rigid = boneInstance.GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector2(xVelocity, yVelocity);
    }

    public void Walk()
    {
        pMovement = GetComponent<Movement>();
        boneTransform = GameObject.FindGameObjectWithTag("Bone-static").transform;

        walkingDistance = boneTransform.position.x - throwPoint.position.x;

        if (!_foundBone)
        {
            if (walkingDistance < 0)
            {
                pMovement.WalkLeft();
            }

            else if (walkingDistance > 0)
            {
                pMovement.WalkRight();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!this.enabled) // Don't throw bones when disabled
            return;
        
        if (coll.gameObject.tag == "Bone-static" && !foundBone)
        {
            Destroy(coll.gameObject);
            _foundBone = true;
            pMovement = GetComponent<Movement>();
            pMovement.WalkLeft();
        }
        
    }

    void Update()
    {
        Walk();
    }
}
