using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputModuleListener))]
public class Menu : MonoBehaviour {
    private Menu parentMenu = null;
//    private Menu submenu = null;
    public bool fromNullSelectFirst = false; // If true, will use default "SelectFirstGameObject" from menuListener
    public bool openAtInit = false;
    public GameObject menuObjToDisable = null; // If set, will be SetActive()
    protected InputModuleListener menuInputListener;

    /***********************************/

    public bool IsMenuOpen() {
        return this.gameObject.activeSelf;
    }

    /* Open menu so user can see the menu.
     */
    virtual public void OpenMenu() {
        menuInputListener.SelectGameObject(null);
        SetMenuActive(true);
    }
    /* Close menu, so user can go back to game / parent menu
     */
    virtual public void CloseMenu() {
        menuInputListener.SelectGameObject(null);
        SetMenuActive(false);
        if (parentMenu != null) {
            parentMenu.OnSubmenuClosed();
            parentMenu = null;
        }
    }

    public void OpenSubmenu(Menu submenu) {
        SetMenuActive(false);
        // Create two-way link between menus
        submenu.parentMenu = this;
//        this.submenu = submenu;
        submenu.OpenMenu();
    }

    /***********************************/

    protected bool IsMenuActive() {
        return menuInputListener.isActiveAndEnabled;
    }

    /* Enable / disable the menu listener...
     * So user can use submenu / exit menu.
     */
    protected void SetMenuActive(bool nowActive) {
//        print("Set menu active");
        if (nowActive)
            menuInputListener.enabled = true;
        else
            menuInputListener.onLastUpdate = true;

        // Disable menuObj
        if (menuObjToDisable != null)
            menuObjToDisable.SetActive(nowActive);
    }

    /* Runs when a submenu is closed, and this menu should be re-activated.
     */
    virtual protected void OnSubmenuClosed() {
        SetMenuActive(true);
//        submenu = null;
    }

    /***********************************/

    virtual protected void Awake() {
        menuInputListener = GetComponent<InputModuleListener>();
        menuInputListener.onCancelEvent.AddListener(CloseMenu);
        // If option isn't selected, but want to select one... Select the default option
        if (fromNullSelectFirst)
            menuInputListener.onMovedFromNullEvent.AddListener(menuInputListener.SelectFirstGameObject);
    }

    virtual protected void Start() {
        SetMenuActive(openAtInit);
    }
}
