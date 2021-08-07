namespace LegoM.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMaxLength = 40;
            public const int FullNameMinLength = 5;

            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;

        }

        public class Product
        {
            public const int TittleMaxLength = 100;
            public const int TittleMinLength = 5;

            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 10;

            public const int QuantityMinLength = 1;

            public const double PriceMinLength = 0.10;
            public const double PriceMaxLength = 1000;

        }


      

        public class Category
        {

        public const int NameMaxLength = 40;

        }

        public class Merchant
        {

            public const int NameMaxLength = 20;
            public const int NameMinLength = 5;
            public const int TelephoneNumberMaxLength = 30;
            public const int TelephoneNumberMinLength = 6;

        }


        public const int AnswerTextMaxLength = 300;

        public const int OrderAreaMaxLength = 20;
        public const int OrderTownMaxLength = 30;

       



        public const int QuestionTextMaxLength = 200;
        public const int QuestionTextMinLength = 10;

        public const int ReportDescriptionMaxLength = 250;

        public class Review
        {
          public const int ContentMaxLength = 250;
          public const int ContentMinLength = 10;

          public const int TitleMaxLength = 20;
          public const int TitleMinLength = 5;
         

        }



       

       

    }
}
