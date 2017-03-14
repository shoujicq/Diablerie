﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour {

    Character character;
    Iso iso;
    Character target;

    static GameObject[] siblings = new GameObject[1024];

	void Awake() {
        iso = GetComponent<Iso>();
    }

    void Start()
    {
        character = GetComponent<Character>();
        character.OnTakeDamage += OnTakeDamage;
        StartCoroutine(Roam());
    }

    void OnTakeDamage(Character originator, int damage)
    {
        Attack(originator);

        int siblingsCount = Tilemap.OverlapBox(iso.pos, new Vector2(20, 20), siblings);
        for(int i = 0; i < siblingsCount; ++i)
        {
            DummyController sibling = siblings[i].GetComponent<DummyController>();
            if (sibling != null && sibling != this)
            {
                sibling.Attack(originator);
            }
        }
    }

    IEnumerator Roam()
    {
        yield return new WaitForEndOfFrame();
        while (!this.target)
        {
            var target = iso.pos + new Vector2(Random.Range(-8f, 8f), Random.Range(-8f, 8f));
            character.GoTo(target);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            while (!isActiveAndEnabled) yield return null;
        }
    }

    void Attack(Character target)
    {
        this.target = target;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            character.Attack(target);
            yield return new WaitForSeconds(Random.Range(0.65f, 1f));
        }
    }
}
