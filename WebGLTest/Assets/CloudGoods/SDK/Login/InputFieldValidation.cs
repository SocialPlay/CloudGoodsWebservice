using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CloudGoods.SDK.Login
{
    public abstract class InputFieldValidation : MonoBehaviour
    {

        InputField uiInput;

        void Start()
        {
            if (this.GetComponent<InputField>())
            {
                uiInput = this.GetComponent<InputField>();
            }
        }

        public bool IsValidCheck(bool isSecondcheck = false)
        {
            if (Validate(uiInput.text, isSecondcheck))
            {
                foreach (Text sprite in uiInput.GetComponentsInChildren<Text>())
                {
                    sprite.color = Color.white;
                }
                return true;
            }
            else
            {
                foreach (Text sprite in uiInput.GetComponentsInChildren<Text>())
                {
                    sprite.color = Color.red;
                }
                return false;
            }
        }



        protected abstract bool Validate(string currentInput, bool isSecondcheck = false);
    }
}
