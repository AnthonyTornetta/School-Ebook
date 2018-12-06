using System;

namespace LibraryManager
{
    public class EBook
    {
        public string Name { get; set; }
        public string RedemptionCode { get; set; }
        public DateTime ExpiresOn { get; set; }

        public EBook(string name, string redemptionCode, DateTime expiresOn)
        {
            Name = name;
            RedemptionCode = redemptionCode;
            ExpiresOn = expiresOn;
        }

        public override bool Equals(object obj)
        {
            if(obj is EBook book)
                if (book.Name.Equals(Name))
                    if (book.RedemptionCode != null && RedemptionCode != null && book.RedemptionCode.Equals(RedemptionCode) || book.RedemptionCode == null && RedemptionCode == null)
                        if (book.ExpiresOn.Equals(ExpiresOn))
                            return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashcode = 1;
            const int prime = 31;

            hashcode = hashcode * prime + Name.GetHashCode();
            hashcode = hashcode * prime + RedemptionCode.GetHashCode();
            hashcode = hashcode * prime + ExpiresOn.GetHashCode();

            return hashcode;
        }

        public static string GenerateRedmptionCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            return new String(stringChars);
        }

        public void Set(EBook book)
        {
            Name = book.Name;
            RedemptionCode = book.RedemptionCode;
            ExpiresOn = book.ExpiresOn;
        }

        public bool Redeem(string code, DateTime newDate)
        {
            if (RedemptionCode == null || code == null)
                return false;

            if (code.Equals(RedemptionCode))
            {
                RedemptionCode = null;
                ExpiresOn = newDate;
                return true;
            }
            
            return false;
        }

        public bool Redeemed()
        {
            return RedemptionCode == null;
        }
    }
}
