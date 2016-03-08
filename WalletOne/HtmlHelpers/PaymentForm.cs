using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using WalletOne.Domain.Entities;

namespace WalletOne.HtmlHelpers {
    public class PaymentForm : IHttpHandler {
        public void ProcessRequest(HttpContext context)
        {
            string merchantKey = "62777b435f4f7b5f5c5c6d634f39306b5970677344326055586133";
              // Добавление полей формы в словарь, сортированный по именам ключей.

            SortedDictionary<string, string> formField = new SortedDictionary<string, string>();
            
            formField.Add("WMI_MERCHANT_ID", "131323876406");
            formField.Add("WMI_PAYMENT_AMOUNT", ((Cart)context.Items["Cart"]).ComputeTotalValue().ToString());
            formField.Add("WMI_CURRENCY_ID", "643");
            //formField.Add("WMI_PAYMENT_NO", "12345-001");
            //formField.Add("WMI_DESCRIPTION", "BASE64:" + Convert.ToBase64String(Encoding.UTF8.GetBytes("Payment for order #12345-001 in MYSHOP.com")));
            formField.Add("WMI_EXPIRED_DATE", DateTime.Now.AddDays(1.0).ToString());
            //formField.Add("WMI_SUCCESS_URL", "https://myshop.com/w1/success.php");
            //formField.Add("WMI_FAIL_URL", "https://myshop.com/w1/fail.php");
            formField.Add("Name", ((ShippingDetails)context.Items["ShippingDetails"]).Name); // Дополнительные параметры
            formField.Add("City", ((ShippingDetails)context.Items["ShippingDetails"]).City); // магазина тоже участвуют
            formField.Add("Address", ((ShippingDetails)context.Items["ShippingDetails"]).Address); // при формировании подписи!

            // Формирование сообщения, путем объединения значений формы, 
            // отсортированных по именам ключей в порядке возрастания и
            // добавление к нему "секретного ключа" интернет-магазина

            StringBuilder signatureData = new StringBuilder();

            foreach (string key in formField.Keys) {
                signatureData.Append(formField[key]);
            }

            // Формирование значения параметра WMI_SIGNATURE, путем 
            // вычисления отпечатка, сформированного выше сообщения, 
            // по алгоритму MD5 и представление его в Base64

            string message = signatureData.ToString() + merchantKey;
            Byte[] bytes = Encoding.GetEncoding(1251).GetBytes(message);
            Byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string signature = Convert.ToBase64String(hash);

            // Добавление параметра WMI_SIGNATURE в словарь параметров формы

            formField.Add("WMI_SIGNATURE", signature);

            // Формирование платежной формы

            StringBuilder output = new StringBuilder();

            output.AppendLine(String.Format(new System.Globalization.CultureInfo("ru-RU"), 
            "Заказ {0} на сумму {1,10:C}",
                ((ShippingDetails) context.Items["ShippingDetails"]).Name,
                ((Cart) context.Items["Cart"]).ComputeTotalValue()));

            output.AppendLine("<form method=\"POST\" action=\"https://wl.walletone.com/checkout/checkout/Index\">");


            foreach (string key in formField.Keys) {
                output.AppendLine(String.Format("<input name=\"{0}\" value=\"{1}\" type=\"hidden\"/>", key, formField[key]));
            }

            output.AppendLine("<input type=\"submit\" value=\"Оплатить\"/></form>");

            context.Response.ContentType = "text/html; charset=UTF-8";
            context.Response.Write(output.ToString());
        }

        public bool IsReusable { get; }
    }
}