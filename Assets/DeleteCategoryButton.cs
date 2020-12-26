using UnityEngine.EventSystems;
using UnityEngine;

public class DeleteCategoryButton : MonoBehaviour, IPointerClickHandler
{
    public MenuButton menuButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        DataLoader.DeleteData(menuButton.index);
    }
}
