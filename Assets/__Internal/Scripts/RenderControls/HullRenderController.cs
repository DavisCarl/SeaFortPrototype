using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullRenderController : MonoBehaviour
{
    public List<DeckRenderRoot> decks;
    public Renderer hullRenderer;
    private void Start()
    {
        RenderDeck(0);
    }
    public void RenderHull(bool v) { hullRenderer.enabled = v; }
    public void RenderDeck(int index)
    {
        for(int i = 0; i < decks.Count; i++) { if(index == i) SetRendering(decks[i].transform, true); else SetRendering(decks[i].transform, false); }
    }
    void SetRendering(Transform target, bool v)
    {
        foreach (Renderer r in target.GetComponentsInChildren<Renderer>()) { r.enabled = v; }
        foreach (Collider r in target.GetComponentsInChildren<Collider>()) { r.enabled = v; }
    }

}
