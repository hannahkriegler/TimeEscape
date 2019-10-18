using System;
using System.Collections;
using System.Collections.Generic;
using TE;
using UnityEngine;
/// <summary>
/// This calculates the path of the shroomie spit. it represents a bezier curve between shroomies mouth, the players position and 1/3 of the distance with a random height
/// </summary>
public class Boss_Projectile : MonoBehaviour
{
    [SerializeField] private Transform[] routes;

    public int damageAmount = 3;

    private int routeToGo;

    private float tParam;

    private Vector2 spitPosition;

    private float speedModifier;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;

    //public bool canSpit;
    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 1f;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(GoByTheRoute(routeToGo));
    }

    private void OnDisable()
    {
        StopCoroutine(GoByTheRoute(routeToGo));
    }
    

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        Debug.Log("running");
        //canSpit = false;
        routes[routeNumber].GetComponent<Route>().Calculate();

        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(1).position;
        Vector2 p3 = routes[routeNumber].GetChild(2).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            spitPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                           3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                           3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                           Mathf.Pow(tParam, 3) * p3;

            transform.position = spitPosition;
            yield return  new WaitForEndOfFrame();
        }

        tParam = 0f;

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IHit hit = other.gameObject.GetComponent<IHit>();
            hit.OnHit(damageAmount, gameObject);
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
        }
    }
    
    float destroyTick = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grid"))
        {
            destroyTick += Time.deltaTime;
            if (destroyTick > 0.1f)
            {
                _spriteRenderer.enabled = false;
                _collider2D.enabled = false;
            }
        }
    }
}
