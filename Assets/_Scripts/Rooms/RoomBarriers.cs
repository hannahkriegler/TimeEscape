using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
/// <summary>
/// A room barrier consist of two parts, the right and the left part.
/// It inherits from the enemy class, but has no die functionality
/// The pars will move to their close positions, which can be manually set in the editor, depending on the corridor distance
/// </summary>
public class RoomBarriers : Enemy
{

    public GameObject rightSpike;
    public GameObject leftSpike;

    public Vector2 right_closePosition;
    public Vector2 right_openPosition;
    public Vector2 left_closePosotion;
    public Vector2 left_openPosition;

    public float timeBetweenShots = 2f;
    private float currentTimeBetweenShots;

    private bool toClose = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        right_openPosition = new Vector2(rightSpike.transform.localPosition.x, rightSpike.transform.localPosition.y);   
        left_openPosition = new Vector2(leftSpike.transform.localPosition.x, leftSpike.transform.localPosition.y);
    }

    protected override void Tick()
    {
        if (currentTimeBetweenShots <= 0)
        {
            toClose = !toClose;
            currentTimeBetweenShots = timeBetweenShots;
        }
        else
        {
            currentTimeBetweenShots -= Time.deltaTime * Game.instance.worldTimeScale;
        }

        if (toClose)
        {
            MoveSpikes(rightSpike, right_closePosition);
            MoveSpikes(leftSpike, left_closePosotion);
            

        }else
        {
            MoveSpikes(rightSpike, right_openPosition);
            MoveSpikes(leftSpike, left_openPosition);
            
        }
        
    }

    void MoveSpikes(GameObject spike, Vector2 direction)
    {
        spike.transform.localPosition = Vector2.MoveTowards(new Vector2(spike.transform.localPosition.x, spike.transform.localPosition.y), direction, 6 * Time.deltaTime);

    }

    protected override void Attack(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            Debug.Log("hit it");
            IHit hit = target.GetComponent<IHit>();
            hit.OnHit(damageAmount, gameObject);
        }
    }
    
    public override void OnHit(int damage, GameObject attacker, bool knockBack)
    {
        Debug.Log("You can't destroy a trap");
    }

    public override bool IsDead()
    {
        return true;
    }
}
