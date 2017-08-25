using System.Runtime.Serialization;
using System.Text;
using System.Web;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

using Newtonsoft.Json;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    /// <summary>
    /// </summary>
    /// <remarks>
    ///     <input type="hidden" name="cmd" value="_xclick" />
    ///     <input type="hidden" name="business" value="youremailaddress@yourdomain.com" />
    ///     <input type="hidden" name="item_name" value="My painting" />
    ///     <input type="hidden" name="amount" value="10.00" />
    ///     <input type="hidden" name="shipping" value="3.00" />
    ///     <input type="hidden" name="handling" value="2.00" />
    ///     <input type="submit" value="Buy with additional parameters!" />
    /// </remarks>
    public class PayPalPaymentCommand  {

        public PayPalPaymentCommand(double amount, string itemName, User recipient, string successUrl, string cancelUrl) {
            Amount = amount.ToString();
            Handling = (0).ToString();
            ItemName = itemName;
            SuccessUrl = successUrl;
            CancelUrl = cancelUrl;
            Business = recipient.PayPalBusinessName;
        }

        [JsonProperty("amount")]
        public string Amount { get; private set; }

        [JsonProperty("business")]
        public string Business { get; private set; }

        [JsonProperty("cmd")]
        public string Command {
            get { return "_donations"; }
        }

        [JsonProperty("handling")]
        public string Handling { get; private set; }

        [JsonProperty("item_name")]
        public string ItemName { get; private set; }

        [JsonProperty("return")]
        public string SuccessUrl { get; private set; }

        [JsonProperty("cancel_return")]
        public string CancelUrl { get; private set; }

        public override string ToString() {
            
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}={1}", "cmd", HttpUtility.HtmlEncode(Command));
            sb.AppendFormat("&{0}={1}", "business", HttpUtility.HtmlEncode(Business));
            sb.AppendFormat("&{0}={1}", "amount", HttpUtility.HtmlEncode(Amount));
            sb.AppendFormat("&{0}={1}", "currency_code", HttpUtility.HtmlEncode("EUR"));
            sb.AppendFormat("&{0}={1}", "lc", HttpUtility.HtmlEncode("de_DE"));
            //sb.AppendFormat("&{0}={1}", "currency_code", HttpUtility.HtmlEncode("EUR"));
            //sb.AppendFormat("&{0}={1}", "currency_code", HttpUtility.HtmlEncode("EUR"));
            //sb.AppendFormat("&{0}={1}", "handling", HttpUtility.HtmlEncode(Handling));
            sb.AppendFormat("&{0}={1}", "item_name", HttpUtility.HtmlEncode(ItemName));
            sb.AppendFormat("&{0}={1}", "return", HttpUtility.HtmlEncode(SuccessUrl));
            sb.AppendFormat("&{0}={1}", "cancel_return", HttpUtility.HtmlEncode(CancelUrl));
            
            return sb.ToString();
        }
    }
}