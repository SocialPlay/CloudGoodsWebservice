using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.Enums;
using CloudGoods.Services;

namespace CloudGoods.SDK.Utilities
{
    public class ItemTextureCache : MonoBehaviour
    {

        static public Dictionary<string, Texture2D> ItemTextures = new Dictionary<string, Texture2D>();

        private static ItemTextureCache _instance;

        private static ItemTextureCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject cloudGoodsObject = new GameObject("ItemTextureCache");
                    cloudGoodsObject.AddComponent<ItemTextureCache>();
                    _instance = cloudGoodsObject.GetComponent<ItemTextureCache>();
                }

                return _instance;
            }
        }

        public static void GetItemTexture(string URL, Action<Texture2D> callback)
        {
            Instance._GetItemTexture(URL, callback);
        }

        void _GetItemTexture(string URL, Action<Texture2D> callback)
        {
            try
            {
                if (ItemTextures.ContainsKey(URL))
                {
                    callback(ItemTextures[URL]);
                }
                else
                    GetItemTextureFromWeb(URL, callback);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                callback(null);
            }
        }

        void GetItemTextureFromWeb(string URL, Action<Texture2D> callback)
        {
            WWW www = new WWW(URL);

            Instance.StartCoroutine(Instance.OnReceivedItemTexture(www, callback, URL));
        }

        IEnumerator OnReceivedItemTexture(WWW www, Action<Texture2D> callback, string imageURL)
        {
            yield return www;

            if (www.error == null)
            {
                if (ItemTextures.ContainsKey(imageURL))
                {
                    callback(ItemTextures[imageURL]);
                }
                else
                {
                    ItemTextures.Add(imageURL, www.texture);
                    callback(www.texture);
                }
            }
            else
            {
                if (CloudGoodsSettings.DefaultTexture != null)
                    callback(CloudGoodsSettings.DefaultTexture);
                else
                    callback(null);
            }
        }
    }
}
