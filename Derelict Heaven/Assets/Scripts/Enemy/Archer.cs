using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    public float shotCoolDown;
    public LayerMask aimImpedeLayers;

    public GameObject projectile;
    public float shotSpeed;
    public float inaccurateOffset;
    private float coolDownTimer;
    private GameObject playerObject;

    private LineRenderer aimLaser;

    public override void ResetState()
    {
        // nothing
    }

    public override Form GetForm()
    {
        return Form.archer;
    }

    public GameObject GetArrowObject()
    {
        return projectile;
    }

    protected override void EnemyPhysicsUpdate()
    {
        // nothing
    }

    protected override void EnemyStart()
    {
        aimLaser = gameObject.GetComponent<LineRenderer>();
        aimLaser.positionCount = 2;
        aimLaser.material = new Material(Shader.Find("Sprites/Default"));
        aimLaser.startColor = Color.white;
        aimLaser.endColor = Color.red;
        aimLaser.enabled = false;
    }

    protected override void EnemyUpdate()
    {
        if (playerObject)
        {
            coolDownTimer += Time.deltaTime;
            AimAtTarget(playerObject);
        }

    }

    public void FoundPlayer(GameObject player)
    {
        playerObject = player;
        if (!player) aimLaser.enabled = false;
    }

    private void AimAtTarget(GameObject target)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target.transform.position, aimImpedeLayers);
        Debug.DrawLine(transform.position, target.transform.position, Color.yellow);
      
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            //Debug.Log(hit.collider.gameObject.name);
            aimLaser.enabled = true;
            aimLaser.SetPosition(0, transform.position);
            aimLaser.SetPosition(1, hit.collider.gameObject.transform.position);
            if (coolDownTimer >= shotCoolDown) ShootAtTarget(target);
        }
        else
        {
            Debug.Log(hit.collider.gameObject.name);
            aimLaser.enabled = false;
            coolDownTimer = shotCoolDown / 2;
        }
    }

    private void ShootAtTarget(GameObject target)
    {
        Vector2 offsetLocation = (Vector2) target.transform.position + new Vector2(Random.Range(-inaccurateOffset, inaccurateOffset), Random.Range(-inaccurateOffset, inaccurateOffset));
        Vector2 direction = offsetLocation - (Vector2)transform.position;
        direction.Normalize();
        Vector2 instantiateLocation = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);

        GameObject arrowObject = Instantiate(projectile, instantiateLocation, Quaternion.identity);

        arrowObject.GetComponent<Rigidbody2D>().velocity = direction * shotSpeed;
        coolDownTimer = 0;
        aimLaser.enabled = false;
    }
}
