using Model.Definitions;
using UI.HUD.Dialogs;
using UnityEngine;

namespace Components
{
    public class DialogShowComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _localDialog;
        [SerializeField] private DialogDefinition _externalDialog;

        private DialogBoxController _dialogBox;
        
        public void Show()
        {
            if (_dialogBox == null)
            {
                _dialogBox = FindObjectOfType<DialogBoxController>();
            }

            _dialogBox.ShowDialog(Data);
        }

        public void Show(DialogDefinition definition)
        {
            _externalDialog = definition;
            Show();
        }

        private DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Local:
                        return _localDialog;
                    case Mode.External:
                        return _externalDialog.Data;
                }

                return null;
            }
        }

        public enum Mode
        {
            Local,
            External,
        }
    }
}