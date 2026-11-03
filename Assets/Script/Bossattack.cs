using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;   
    public GameObject warningPrefab;      

    public float minX = -8f;              
    public float maxX = 8f;
    public float spawnY = 7f;             
    public float warningY = 3f;          
    public float warnTime = 1f;           

    public float firstDelay = 2f;
    public float rest = 3f;               
    public float restPhase2 = 1.8f;      
    public float fanSpeed = 6f;

    private BossHealth boss;
    private SpriteRenderer sr;
    private Collider2D bossCol;

    void Start()
    {
        boss = GetComponent<BossHealth>();
        sr = GetComponent<SpriteRenderer>();
        bossCol = GetComponent<Collider2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(firstDelay);

        while (!boss.IsDead)
        {
            if (Random.value < 0.5f)
                yield return StartCoroutine(Rain());
            else
                yield return StartCoroutine(Fan());

            yield return new WaitForSeconds(boss.Phase2 ? restPhase2 : rest);
        }
    }

    IEnumerator Rain()
    {
        int count = boss.Phase2 ? 5 : 3;
        float[] xs = new float[count];
        GameObject[] warns = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            xs[i] = (i == 0 && player != null)
                ? Mathf.Clamp(player.position.x, minX, maxX)   
                : Random.Range(minX, maxX);

            warns[i] = Instantiate(warningPrefab, new Vector3(xs[i], warningY, 0f), Quaternion.identity);
        }

        yield return new WaitForSeconds(warnTime);

        for (int i = 0; i < count; i++)
        {
            Destroy(warns[i]);
            if (!boss.IsDead)
                Spawn(new Vector3(xs[i], spawnY, 0f), Vector2.zero, 1.2f);
        }
    }

    IEnumerator Fan()
    {
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.6f);
        if (boss.IsDead) yield break;
        sr.color = Color.white;

        int n = boss.Phase2 ? 7 : 5;
        float spread = boss.Phase2 ? 100f : 60f;

        Vector2 toPlayer = player != null
            ? (Vector2)(player.position - transform.position)
            : Vector2.down;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < n; i++)
        {
            float a = baseAngle - spread / 2f + spread * i / (n - 1);
            Vector2 dir = new Vector2(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad));
            Spawn(transform.position, dir * fanSpeed, 0f);
        }
    }

    void Spawn(Vector3 pos, Vector2 velocity, float gravity)
    {
        GameObject p = Instantiate(projectilePrefab, pos, Quaternion.identity);
        p.GetComponent<BossProjectile>().Launch(velocity, gravity);

        Collider2D pc = p.GetComponent<Collider2D>();
        if (pc != null && bossCol != null)
            Physics2D.IgnoreCollision(pc, bossCol);   
    }
}