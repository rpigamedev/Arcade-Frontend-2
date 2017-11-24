using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEventTrigger : EventTrigger
{
    // Variables
    bool isSelected = false;

    // References 
    MenuNavigator menuNav;
    Image thisImage;
    Outline thisOutline;

    // Data
    public System.Guid gameInfoHandle;

    void Awake() {
        // Grab the icon's main image component: "Icon Mask" -> "Game Icon Image" -> "Image"
        thisImage = transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
        thisOutline = transform.GetChild(0).gameObject.GetComponent<Outline>();
    }

    void Start() {
        menuNav = EventSystem.current.gameObject.GetComponent<MenuNavigator>();
    }

    public void SetGuid(System.Guid newGuid) {
        if (gameInfoHandle == System.Guid.Empty) {
            gameInfoHandle = newGuid;
        }
    }
    
    public void SetIcon(Sprite newSprite) {
        if (thisImage == null)
            thisImage = transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
        thisImage.sprite = newSprite;
    }

    public override void OnPointerClick(PointerEventData data) {
        menuNav.SelectByIndex(transform.GetSiblingIndex());
    }

    public override void OnSelect(BaseEventData eventData) {
        isSelected = true;
        thisImage.CrossFadeColor(Color.yellow, 0.25f, false, false);
        thisOutline.effectColor = Color.red;
        // Show the About screen instead of game info if selecting 
        DisplayManager.instance.SetInfoPanelState(tag == "AboutIcon" ? InfoPanelState.AboutPanel : InfoPanelState.GamePanel);
        if (gameInfoHandle != System.Guid.Empty) {
            DisplayManager.instance.SetInfo(gameInfoHandle);
        }
    }

    public override void OnDeselect(BaseEventData eventData) {
        isSelected = false;
        thisImage.CrossFadeColor(Color.white, 0.25f, false, false);
        thisOutline.effectColor = Color.gray;
    }

    public override void OnMove(AxisEventData eventData) {
        switch (eventData.moveDir) {
            case MoveDirection.Left:
                menuNav.SelectPreviousItem();
                break;
            case MoveDirection.Right:
                menuNav.SelectNextItem();
                break;
        }
    }

    public override void OnSubmit(BaseEventData eventData) { 
        if (tag != "AboutIcon")
            GameLauncher.instance.requestLaunch(DataManager.instance.GetInfo(gameInfoHandle));
    }
    
    public override void OnCancel(BaseEventData eventData) {
        GameLauncher.instance.cancelLaunch();
    }
}