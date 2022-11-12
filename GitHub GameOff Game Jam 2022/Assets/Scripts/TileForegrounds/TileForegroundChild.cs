using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class TileForegroundChild : MonoBehaviour {

    [Header("sprite renderers - backmost first")]
    [Tooltip("list all SpriteRenderers in order they should be painted, where backmost is first")] [SerializeField] private SpriteRenderer[] _spritesInOrder;

    [Header("sprites per season - same order as renderers")]
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _summerSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _winterSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _springSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _autumnSpritesInOrder;

    private void Awake() {
        Assert.IsTrue(_summerSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized summer sprites array");
        Assert.IsTrue(_winterSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized winter sprites arrays");
        Assert.IsTrue(_springSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized spring sprites arrays");
        Assert.IsTrue(_autumnSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized autumn sprites arrays");

        foreach (SpriteRenderer s in _spritesInOrder)
            Assert.IsNotNull(s, "TileForegroundSortOrderPositioner didn't expect any nulls in the spritesInOrder array");
        foreach (Sprite s in _summerSpritesInOrder)
            Assert.IsNotNull(s, "TileForegroundSortOrderPositioner didn't expect any nulls in the summerSpritesInOrder array");
        foreach (Sprite s in _winterSpritesInOrder)
            Assert.IsNotNull(s, "TileForegroundSortOrderPositioner didn't expect any nulls in the winterSpritesInOrder array");
        foreach (Sprite s in _springSpritesInOrder)
            Assert.IsNotNull(s, "TileForegroundSortOrderPositioner didn't expect any nulls in the springSpritesInOrder array");
        foreach (Sprite s in _autumnSpritesInOrder)
            Assert.IsNotNull(s, "TileForegroundSortOrderPositioner didn't expect any nulls in the autumnSpritesInOrder array");
    }

    public void PositionSpritesBasedOnTileSortOrder(int tileSortOrder) {
        for (int i = 0; i < _spritesInOrder.Length; i++) {
            _spritesInOrder[i].sortingOrder = tileSortOrder + 1 + i;
        }
    }

    public void UpdateSpritesBasedOnParams(SeasonType season) {
        Sprite[] spriteArrayForSeason;
        switch (season) {
            case SeasonType.SUMMER: spriteArrayForSeason = _summerSpritesInOrder; break;
            case SeasonType.FALL:   spriteArrayForSeason = _autumnSpritesInOrder; break;
            case SeasonType.WINTER: spriteArrayForSeason = _winterSpritesInOrder; break;
            case SeasonType.SPRING: spriteArrayForSeason = _springSpritesInOrder; break;
            default: throw new System.Exception($"TileForegroundChild didn't expect season {season}");
        }

        for (int i = 0; i < _spritesInOrder.Length; i++) {
            _spritesInOrder[i].sprite = spriteArrayForSeason[i];
        }
    }

}
