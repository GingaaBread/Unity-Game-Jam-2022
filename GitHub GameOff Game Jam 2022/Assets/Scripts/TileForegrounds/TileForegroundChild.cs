using System;
using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class TileForegroundChild : MonoBehaviour {

    [Header("sprite renderers - backmost first")]
    [Tooltip("list all SpriteRenderers in order they should be painted, where backmost is first")] [SerializeField] private SpriteRenderer[] _spritesInOrder;

    [SerializeField] private ForegroundType foregroundType;

    [Header("sprites per age - if variable by age, otherwise empty")]
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _sproutSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _growingSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _finishedSpritesInOrder;
    [Tooltip("at what age percenage to transition to growing")] [Range(0, 1)] [SerializeField] private float _agePercentMaxForSprout;
    [Tooltip("at what age percenage to transition to finished")] [Range(0, 1)] [SerializeField] private float _agePercentMaxForGrowing;

    [Header("sprites per season - if variable by season, otherwise empty")]
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _summerSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _winterSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _springSpritesInOrder;
    [Tooltip("list all sprites in same order as spritesInOrder")] [SerializeField] private Sprite[] _autumnSpritesInOrder;

    [Header("animation offset per SpriteRenderer")]
    [Tooltip("list all animation offset times from 0 to 1, in same order as spritesInOrder")] [SerializeField] private float[] _animationOffsetsInSortOrder;

    public enum ForegroundType { NotVariable, VariableBySeason, VariableByAge }
    private const string ANIMATION_PARAM_FOR_CYCLE_OFFSET = "CycleOffset";

    private void Awake() {
        foreach (SpriteRenderer s in _spritesInOrder)
            Assert.IsNotNull(s, "TileForegroundSortOrderPositioner didn't expect any nulls in the spritesInOrder array");

        if (foregroundType == ForegroundType.VariableBySeason) {
            Assert.IsTrue(_summerSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized summer sprites array");
            Assert.IsTrue(_winterSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized winter sprites arrays");
            Assert.IsTrue(_springSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized spring sprites arrays");
            Assert.IsTrue(_autumnSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized autumn sprites arrays");
            AssertSpriteArayHasNoNulls(_summerSpritesInOrder, "TileForegroundSortOrderPositioner didn't expect any nulls in the summerSpritesInOrder array");
            AssertSpriteArayHasNoNulls(_winterSpritesInOrder, "TileForegroundSortOrderPositioner didn't expect any nulls in the winterSpritesInOrder array");
            AssertSpriteArayHasNoNulls(_springSpritesInOrder, "TileForegroundSortOrderPositioner didn't expect any nulls in the springSpritesInOrder array");
            AssertSpriteArayHasNoNulls(_autumnSpritesInOrder, "TileForegroundSortOrderPositioner didn't expect any nulls in the autumnSpritesInOrder array");
        } else if(foregroundType == ForegroundType.VariableByAge) {
            Assert.IsTrue(_sproutSpritesInOrder.Length   == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized sprout sprites array");
            Assert.IsTrue(_growingSpritesInOrder.Length  == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized growing sprites arrays");
            Assert.IsTrue(_finishedSpritesInOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized finished sprites arrays");
            AssertSpriteArayHasNoNulls(_sproutSpritesInOrder,   "TileForegroundSortOrderPositioner didn't expect any nulls in the _sproutSpritesInOrder array");
            AssertSpriteArayHasNoNulls(_growingSpritesInOrder,  "TileForegroundSortOrderPositioner didn't expect any nulls in the _growingSpritesInOrder array");
            AssertSpriteArayHasNoNulls(_finishedSpritesInOrder, "TileForegroundSortOrderPositioner didn't expect any nulls in the _finishedSpritesInOrder array");
        } else if (foregroundType == ForegroundType.NotVariable) {
            // no asserting needed
        }

        Assert.IsTrue(_animationOffsetsInSortOrder.Length == _spritesInOrder.Length, "TileForegroundSortOrderPositioner didn't expect different sized _animationOffsetsInSortOrder array");
        foreach (int offset in _animationOffsetsInSortOrder)
            Assert.IsTrue(offset >= 0 && offset <= 1, "TileForegroundSortOrderPositioner offsets must be between 0 and 1 but found "+offset);

        SetAnimationCycleOffsets();
    }

    private void SetAnimationCycleOffsets() {
        for (int i = 0; i < _spritesInOrder.Length; i++) {
            _spritesInOrder[i].GetComponent<Animator>().SetFloat(ANIMATION_PARAM_FOR_CYCLE_OFFSET, _animationOffsetsInSortOrder[i]);
        }
    }
    
    public void UpdateForSeasonAndAgePercentage(SeasonType season, float agePercentage) {
        Sprite[] spriteArrayToUse;
        if (foregroundType == ForegroundType.VariableBySeason) {
            switch (season) {
                case SeasonType.SUMMER: spriteArrayToUse = _summerSpritesInOrder; break;
                case SeasonType.FALL:   spriteArrayToUse = _autumnSpritesInOrder; break;
                case SeasonType.WINTER: spriteArrayToUse = _winterSpritesInOrder; break;
                case SeasonType.SPRING: spriteArrayToUse = _springSpritesInOrder; break;
                default: throw new System.Exception($"TileForegroundChild didn't expect season {season}");
            }
            for (int i = 0; i < _spritesInOrder.Length; i++) {
                _spritesInOrder[i].sprite = spriteArrayToUse[i];
            }
        } else if (foregroundType == ForegroundType.VariableByAge) {
            Assert.IsTrue(agePercentage >= 0 && agePercentage <= 1);
            if(agePercentage <= _agePercentMaxForSprout) {
                spriteArrayToUse = _sproutSpritesInOrder;
            } else if (agePercentage <= _agePercentMaxForGrowing) {
                spriteArrayToUse = _growingSpritesInOrder;
            } else {
                spriteArrayToUse = _finishedSpritesInOrder;
            }
            for (int i = 0; i < _spritesInOrder.Length; i++) {
                _spritesInOrder[i].sprite = spriteArrayToUse[i];
            }
        } else if (foregroundType == ForegroundType.NotVariable) {
            // no update needed
        }
    }

    public void PositionSpritesBasedOnTileSortOrder(int tileSortOrder) {
        for (int i = 0; i < _spritesInOrder.Length; i++) {
            _spritesInOrder[i].sortingOrder = tileSortOrder + 1 + i;
        }
    }

    public void StartAnimating() {
        for (int i = 0; i < _spritesInOrder.Length; i++) {
            _spritesInOrder[i].GetComponent<Animator>().enabled = true;
        }
    }

    public void StopAnimating() {
        for (int i = 0; i < _spritesInOrder.Length; i++) {
            _spritesInOrder[i].GetComponent<Animator>().enabled = false;
        }
    }

    private void AssertSpriteArayHasNoNulls(Sprite[] sa, string message) {
        foreach (Sprite s in sa)
            Assert.IsNotNull(s, message);
    }

}
