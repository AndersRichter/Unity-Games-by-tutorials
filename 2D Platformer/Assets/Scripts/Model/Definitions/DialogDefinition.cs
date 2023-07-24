using UI.HUD.Dialogs;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/DialogDefinition", fileName = "DialogDefinition")]
    public class DialogDefinition: ScriptableObject
    {
        [SerializeField] private DialogData _dialogData;

        public DialogData Data => _dialogData;
    }
}