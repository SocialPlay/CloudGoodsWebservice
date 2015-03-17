﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.Emuns;
using CloudGoods;

namespace CloudGoods.Utilities
{
    public class ItemTextureCache : MonoBehaviour
    {

        static public Dictionary<string, Texture2D> ItemTextures = new Dictionary<string, Texture2D>();

        private static ItemTextureCache _instance;

        public static ItemTextureCache Instance
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

        public void GetItemTexture(string URL, Action<ImageStatus, Texture2D> callback)
        {
            try
            {
                if (ItemTextures.ContainsKey(URL))
                {
                    callback(ImageStatus.Cache, ItemTextures[URL]);
                }
                else
                    GetItemTextureFromWeb(URL, callback);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                callback(ImageStatus.Error, null);
            }
        }

        void GetItemTextureFromWeb(string URL, Action<ImageStatus, Texture2D> callback)
        {
            WWW www = new WWW(URL);

            Instance.StartCoroutine(Instance.OnReceivedItemTexture(www, callback, URL));
        }

        IEnumerator OnReceivedItemTexture(WWW www, Action<ImageStatus, Texture2D> callback, string imageURL)
        {
            yield return www;

            if (www.error == null)
            {
                if (ItemTextures.ContainsKey(imageURL))
                {
                    callback(ImageStatus.Cache, ItemTextures[imageURL]);
                }
                else
                {
                    ItemTextures.Add(imageURL, www.texture);
                    callback(ImageStatus.Web, www.texture);
                }
            }
            else
            {
                if (CloudGoodsSettings.DefaultTexture != null)
                    callback(ImageStatus.Cache, CloudGoodsSettings.DefaultTexture);
                else
                    callback(ImageStatus.Error, null);
            }
        }
    }
}
