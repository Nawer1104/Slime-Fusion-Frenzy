using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public List<Sprite> sprites;

    public SpriteRenderer renderer;

    private Animator anim;

    public GameObject vfxOnSuccess;

    private Vector2 mousePos;

    public Color color;

    private void Awake()
    {
        renderer.sprite = sprites[Random.Range(0, sprites.Count)];

        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        DrawWithMouse.Instance.StartLine(transform.position);
    }

    private void OnMouseDrag()
    {
        DrawWithMouse.Instance.Updateline();
    }

    private void OnMouseUp()
    {
        DrawWithMouse.Instance.ResetLine();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID()) return;
            Slime orbConnected = hit.collider.gameObject.GetComponent<Slime>();
            if (color == orbConnected.color)
            {
                StartCoroutine(AnimCoroutine());
                orbConnected.StartCoroutine(orbConnected.AnimCoroutine());
            }
        }
    }



    public IEnumerator AnimCoroutine()
    {
        anim.SetTrigger("Scale");

        yield return new WaitForSeconds(1);

        GameObject explosion = Instantiate(vfxOnSuccess, transform.position, transform.rotation);
        Destroy(explosion, .75f);

        yield return new WaitForSeconds(1);

        RemoveFromList();
        Destroy(gameObject);

    }

    public void RemoveFromList()
    {
        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].gameObjects.Remove(this.gameObject);
        GameManager.Instance.CheckLevelUp();
    }
}
