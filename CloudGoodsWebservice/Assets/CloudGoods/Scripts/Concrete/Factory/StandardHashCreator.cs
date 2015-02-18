using UnityEngine;
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

        var secretKeyByteArray = Convert.FromBase64String("");

        byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

        using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
        {
            byte[] signatureBytes = hmac.ComputeHash(signature);
            string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

            return requestSignatureBase64String;
        }

        
    }
}
