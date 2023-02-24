using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaChat.ClassHelper
{
    class PasswordTestClass
    {
        public static int Verify(string pass) //прохождение всех тестов (возвращает номер не пройденного теста, если все тесты были пройденны возвращает 0)
        {
            if (!testLength(pass))
            {
                return 1;
            }
            if (!testCharacters(pass))
            {
                return 2;
            }
            if (!testStrength(pass))
            {
                return 3;
            }
            return 0;
        }
        static bool testLength(string pass) //тест длинны пароля (от 6 до 12 символов)
        {
            return pass.Length > 5 && pass.Length < 13;
        }
        static bool testCharacters(string pass) //тест символов (только символы в этом наборе достуны для пароля)
        {
            char[] C = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '?', '@',
                '#'
            }; //доступные символы без учета регистра
            foreach (char c in pass.ToUpper())
            {
                if (!C.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
        static bool testStrength(string pass) //тест силы пароля (пароль должен включать заглавную букву, прописную букву, цифру и спец символ)
        {
            char[] upperCase = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            }; //символы верхнего регистра
            char[] lowerCase = {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            }; //символы нижнего регистра
            char[] numbers = {
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
            }; //доступные цифры
            char[] special = {
                '!', '?', '@', '#'
            }; //специальные символы
            bool temp = false;
            foreach (char c in pass)
            {
                if (upperCase.Contains(c))
                {
                    temp = true;
                }
            }
            if (!temp)
            {
                return false;
            }
            temp = false;
            foreach (char c in pass)
            {
                if (lowerCase.Contains(c))
                {
                    temp = true;
                }
            }
            if (!temp)
            {
                return false;
            }
            temp = false;
            foreach (char c in pass)
            {
                if (numbers.Contains(c))
                {
                    temp = true;
                }
            }
            if (!temp)
            {
                return false;
            }
            temp = false;
            foreach (char c in pass)
            {
                if (special.Contains(c))
                {
                    temp = true;
                }
            }
            return temp;
        }
    }
}
