using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CloudGoods.Enums;
using CloudGoods.SDK;

namespace CloudGoods.SDK.Store.UI
{
    public class UnityUIPurchaseButtonDisplay : MonoBehaviour
    {

        public CurrencyType currencyType;
        public Button ActiveButton;
        public Text InsufficientFundsLabel;
        public Text CurrencyText;
        public string InsufficientFundsTextOverride = "";
        private int ItemCost;

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
            ItemCost = itemCost;
            CurrencyText.text = itemCost.ToString();

            if (itemCost < 0)
            {
                SetNotApplicable();
            }
            else if (currencyType == CurrencyType.Standard)
            {
                SetInactive();
                CurrencyManager.GetStandardCurrencyBalance(0, RecivedCurrencyInfo);
              
            }
            else if (currencyType == CurrencyType.Premium)
            {
                SetInactive();
                CurrencyManager.GetPremiumCurrencyBalance( RecivedCurrencyInfo);
            }
        }

        public void RecivedCurrencyInfo(int amount)
        {
            if (ItemCost <= amount)
                SetActive();
            else
                SetInactive();
        }
    }
}