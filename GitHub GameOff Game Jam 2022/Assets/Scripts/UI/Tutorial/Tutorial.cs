using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TimeManagement;

public class Tutorial : ComputerPhaseStep
{
   [SerializeField]
   private TutorialSO _TutorialDialogue;
   [SerializeField]
   private TextMeshProUGUI tutorialTextBox;
   [SerializeField]
   private Image tutorialCharacterImage;
   [SerializeField]
   private Image tutorialImage;
   private int index = 0; 

   [SerializeField]
   private GameObject tutorialPanel;
   private bool _shownAtStart = false;

   public void OnEnable(){
    tutorialTextBox.text = _TutorialDialogue.lines[0];
    tutorialCharacterImage.sprite = _TutorialDialogue.speakers[0];
    tutorialImage.sprite = _TutorialDialogue.screenshots[0];
   }
   public  void NextPanel(){
    if(_TutorialDialogue==null) return;
    index++;
    if(index >= _TutorialDialogue.lines.Length) index = 0;
    tutorialTextBox.text = _TutorialDialogue.lines[index];
    tutorialCharacterImage.sprite = _TutorialDialogue.speakers[index];
    tutorialImage.sprite = _TutorialDialogue.screenshots[index];
   
   }

   public void PreviousPanel(){
    if(_TutorialDialogue==null) return;
    index--;
    if(index < 0) index = _TutorialDialogue.lines.Length-1;
    tutorialTextBox.text = _TutorialDialogue.lines[index];
    tutorialCharacterImage.sprite = _TutorialDialogue.speakers[index];
    tutorialImage.sprite = _TutorialDialogue.screenshots[index];

   
   }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit){
        if(!_shownAtStart)TurnOnTimePanel();
        if(_shownAtStart)OnFinishProcessing.Invoke();
    }
    
    public void TurnOnTimePanel(){
        tutorialPanel.SetActive(true);
    }

    public void StartGame(){
        if(_shownAtStart) return;
        _shownAtStart = true;
        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences()=> new object[]{};
}
