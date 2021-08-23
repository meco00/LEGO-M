namespace LegoM.Data
{
    public class DataConstants
    {
        public class Answer
        {
            public const int ContentMaxLength = 250;
            public const int ContentMinLength = 8;
        }

        public class Comment
        {
            public const int ContentMaxLength = 250;
            public const int ContentMinLength = 8;
        }

        public class Category
        {

            public const int NameMaxLength = 40;

        }

        public class Trader
        {

            public const int NameMaxLength = 20;
            public const int NameMinLength = 5;
            public const int TelephoneNumberMaxLength = 10;

        }

        public class User
        {
            public const int FullNameMaxLength = 40;
            public const int FullNameMinLength = 5;

            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;

        }

        public class Product
        {
            public const int TittleMaxLength = 40;
            public const int TittleMinLength = 5;

            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 7;

            public const int QuantityMinLength = 1;

            public const double PriceMinLength = 0.10;
            public const double PriceMaxLength = 1000;

        }

      
        public class Review
        {
            public const int ContentMaxLength = 250;
            public const int ContentMinLength = 10;

            public const int TitleMaxLength = 20;
            public const int TitleMinLength = 3;


        }

        public class Report
        {
            public const int ContentMaxLength = 250;
            public const int ContentMinLength = 10;
        }

        public class Question
        {
            public const int ContentMaxLength = 250;
            public const int ContentMinLength = 10;


        }

     


        public class Order
        {
            public const int FullNameMaxLength = 30;
            public const int FullNameMinLength = 5;
            public const int ZipCodeMaxLength = 4;
            public const int CityMinLength = 4;
            public const int CityMаxLength = 15;
            public const int StateMinLength = 4;
            public const int StateMаxLength = 15;
            public const int AddressMinLength = 5;
            public const int AddressMаxLength = 30;
            public const int TelephoneNumberMaxLength = 10;
        }

    }
}
