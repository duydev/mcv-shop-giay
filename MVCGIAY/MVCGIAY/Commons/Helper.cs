using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace MVCGIAY.Commons
{
    public static class Helper
    {

        public static string md5(string content)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(content);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append( hashBytes[i].ToString("X2") );
                }

                content = builder.ToString().ToLower();
            }

            return content;
        }

    }
}