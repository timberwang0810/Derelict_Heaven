using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    public float xRange;
    public float yRange;
    public float shotCoolDown;
    public LayerMask aimImpedeLayers;

    public GameObject projectile;
    public float shotSpeed;
    private float coolDownTimer;
    private GameObject playerObject;

    private LineRenderer aimLaser;

    private int triggerCount;
    public override void ResetState()
    {
        throw new System.NotImplementedException();
    }

    protected override void EnemyPhysicsUpdate()
    {
        // nothing
    }

    protected override void EnemyStart()
    {
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(xRange, yRange);
        aimLaser = gameObject.GetComponent<LineRenderer>();
        aimLaser.positionCount = 2;
        aimLaser.material = new Material(Shader.Find("Sprites/Default"));
        aimLaser.startColor = Color.white;
        aimLaser.endColor = Color.red;
        aimLaser.enabled = false;
    }

    protected override void EnemyUpdate()
    {
        coolDownTimer += Time.deltaTime;
        if (playerObject)
        {
            AimAtTarget(playerObject);
        }

    }

    protected override void EnemyTriggerEvent(Collider2D collision)
    {
        triggerCount = Mathf.Clamp(triggerCount + 1, 0, 2);
        if (triggerCount == 2)
        {
            base.EnemyTriggerEvent(collision);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("saved");
            playerObject = collision.gameObject;
        }
    }

    protected override void EnemyTriggerExitEvent(Collider2D collision)
    {
        triggerCount = Mathf.Clamp(triggerCount - 1, 0, 2);
        base.EnemyTriggerExitEvent(collision);
        if (triggerCount == 0 && collision.gameObject.CompareTag("Player"))
        {
            playerObject = null;
        }
    }

    private void AimAtTarget(GameObject target)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target.transform.position, aimImpedeLayers);
      
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log(hit.collider.gameObject.name);
            aimLaser.enabled = true;
            aimLaser.SetPosition(0, transform.position);
            aimLaser.SetPosition(1, hit.collider.gameObject.transform.position);
            if (coolDownTimer >= shotCoolDown) ShootAtTarget(target);
        }
        else
        {
            aimLaser.enabled = false;
        }
    }

    private void ShootAtTarget(GameObject target)
    {
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        Vector2 instantiateLocation = new Vector2(transform.position.x + direction.x, transform.position.y);

        GameObject arrowObject = Instantiate(projectile, instantiateLocation, Quaternion.identity);

        arrowObject.GetComponent<Rigidbody2D>().velocity = direction * shotSpeed;
        coolDownTimer = 0;
        aimLaser.enabled = false;
    }
}
