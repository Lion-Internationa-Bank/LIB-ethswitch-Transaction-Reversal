using System;
using System.Collections.Generic;
using System.Text;

namespace LIB_Usermanagement.DAL.Common
{
    public static class StaticDataHelper
    {

        public static string GetNationalityFieldName(string strLang)
        {
            string strResult = string.Empty;
            switch (strLang)
            {
                case "et":
                    strResult = "Amharic";
                    break;
                case "en":
                    strResult = "English";
                    break;
                case "or":
                    strResult = "AfanOromo";
                    break;
                case "tg":
                    strResult = "Tigrigna";
                    break;
                case "af":
                    strResult = "Afaraf";
                    break;
                case "sm":
                    strResult = "Somali";
                    break;
                default:
                    strResult = "English";
                    break;
            }
            return strResult;
        }

        // There should be a better way
        public static string GetGenderDesc(string strLang, int intValue)
        {
            string strResult = string.Empty;
            switch (strLang)
            {
                case "et":
                    switch (intValue)
                    {
                        case 1:
                            strResult = "ወንድ";
                            break;
                        case 2:
                            strResult = "ሴት";
                            break;
                        default:
                            break;
                    }
                    break;
                case "en":
                    switch (intValue)
                    {
                        case 1:
                            strResult = "Male";
                            break;
                        case 2:
                            strResult = "Female";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return strResult;
        }

        public static string GetTitleDesc(string strLang, int intValue)
        {
            string strResult = string.Empty;
            // put code here
            return strResult;
        }

    }
}
