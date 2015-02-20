﻿using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Security.Cryptography;

public class StandardHashCreator : HashCreator {

    public string CreateHash(params string[] values)
    {
        string signatureRawData = "";

        foreach (string value in values)
        {
            signatureRawData += value;
        }

        byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

        using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(CloudGoods.Instance().appSecret)))
        {
            byte[] signatureBytes = hmac.ComputeHash(signature);
            string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

            return requestSignatureBase64String;
        }
    }
}
