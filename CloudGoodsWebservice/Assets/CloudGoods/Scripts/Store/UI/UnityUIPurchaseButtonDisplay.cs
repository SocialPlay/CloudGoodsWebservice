using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.Enums;

namespace CloudGoods.Store.UI
{
    public class UnityUIPurchaseButtonDisplay : MonoBehaviour
    {

        public CurrencyType currencyType;
        public Button ActiveButton;
        public Text InsufficientFundsLabel;
        public Text CurrencyText;
        public string InsufficientFundsTextOverride = "";

        public void SetInactive()
        {
            if (!string.IsNullOrEmpty(InsufficientFundsTextOverride) && InsufficientFundsTextOverride != InsufficientFundsLabel.text)
            {
                InsufficientFundsLabel.text = InsufficientFundsTextOverride;
            }
            ActiveButton.gameObject.SetActive(false);
            InsufficientFundsLabel.text = "Insufficent Funds";
            InsufficientFundsLabel.gameObject.SetActive(true);
        }

        public void SetActive()
        {
            if (!string.IsNullOrEmpty(InsufficientFundsTextOverride) && InsufficientFundsTextOverride != InsufficientFundsLabel.text)
            {
                InsufficientFundsLabel.text = InsufficientFundsTextOverride;
            }
            ActiveButton.gameObject.SetActive(true);
            InsufficientFundsLabel.gameObject.SetActive(false);
        }

        public void SetNotApplicable()
        {
            ActiveButton.transform.parent.gameObject.SetActive(false);
        }

        public void SetState(int itemCost)
        {
            Debug.Log("Item Cost: " + itemCost);

            CurrencyText.text = itemCost.ToString();

            if (itemCost < 0)
            {
                SetNotApplicable();
            }
            else if (currencyType == CurrencyType.Standard)
            {
                if (itemCost <= CallHandler.StandardCurrency)
                    SetActive();
                else
                    SetInactive();
            }
            else if (currencyType == CurrencyType.Premium)
            {
                if (itemCost <= CallHandler.PremiumCurrency)
                    SetActive();
                else
                    SetInactive();
            }
        }
    }
}