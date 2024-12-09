using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckView : BaseView
{
    public DeckUIControl deckUIControl_;
    public DeckCollectionControl deckCollectionControl;
    public override void Setup(ViewParam param)
    {
        base.Setup(param);
        deckUIControl_.Setup();
        deckCollectionControl.Setup();
    }
    public void OnBack()
    {
        ViewManager.instance.SwitchView(ViewIndex.HomeView);
    }
  
}
