using UnityEngine;

public class MenuRouter : MonoBehaviour
{
    public enum Page
    {
        Home,
        Collection,
        LevelSelect
    }

    [Header("Pages")]
    [SerializeField] private GameObject homePage;
    [SerializeField] private GameObject collectionPage;
    [SerializeField] private GameObject levelSelectPage;

    [Header("Home-only UI")]
    [SerializeField] private GameObject settingsButtonOnHome;

    [Header("Popup")]
    [SerializeField] private GameObject settingsPopup;

    private Page currentPage;

    private void Awake()
    {
        // Ensure popup starts closed
        if (settingsPopup != null) settingsPopup.SetActive(false);

        NavigateTo(Page.Home);
    }

    public void NavigateToHome() => NavigateTo(Page.Home);
    public void NavigateToCollection() => NavigateTo(Page.Collection);
    public void NavigateToLevelSelect() => NavigateTo(Page.LevelSelect);

    public void NavigateTo(Page page)
    {
        currentPage = page;

        // Turn pages on/off
        if (homePage != null) homePage.SetActive(page == Page.Home);
        if (collectionPage != null) collectionPage.SetActive(page == Page.Collection);
        if (levelSelectPage != null) levelSelectPage.SetActive(page == Page.LevelSelect);

        // Home-only settings button visibility
        if (settingsButtonOnHome != null)
            settingsButtonOnHome.SetActive(page == Page.Home);

        // If you leave Home, close popup automatically
        if (page != Page.Home)
            CloseSettings();
    }

    public void OpenSettings()
    {
        // Only allow opening on Home
        if (currentPage != Page.Home) return;

        if (settingsPopup != null)
            settingsPopup.SetActive(true);
    }

    public void CloseSettings() 
    {
        if (settingsPopup != null)
            settingsPopup.SetActive(false);
    }
}