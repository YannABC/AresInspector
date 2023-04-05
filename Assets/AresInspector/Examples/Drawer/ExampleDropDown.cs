using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleDropDown : ScriptableObject, IAresObjectV
    {
        #region string
        [ADDropDown("m_Choices1_string")]
        public string string_array;

        [ADDropDown("m_Choices2_string")]
        public string string_list_field;

        [ADDropDown("m_Choices2_string")]
        public string string_list_property;

        [ADDropDown("GetLstChoices_string")]
        public string string_list_method;

        [ADDropDown("GetDicChoices_string")]
        public string string_dic;

        string[] m_Choices1_string = new string[] { "x1", "x2", "x3" };
        List<string> m_Choices2_string = new List<string>() { "x4", "x5", "x6" };
        List<string> Choices3_string => m_Choices2_string;
        List<string> GetLstChoices_string() { return m_Choices2_string; }

        Dictionary<string, string> GetDicChoices_string()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("aaaa", "x1");
            ret.Add("bbbb", "x2");
            ret.Add("cccc", "x3");
            return ret;
        }
        #endregion

        #region int
        [ADDropDown("m_Choices1_int")]
        public int int_array;

        [ADDropDown("m_Choices2_int")]
        public int int_list_field;

        [ADDropDown("m_Choices2_int")]
        public int int_list_property;

        [ADDropDown("GetLstChoices_int")]
        public int int_list_method;

        [ADDropDown("GetDicChoices_int")]
        public int int_dic;

        int[] m_Choices1_int = new int[] { 1, 2, 3 };
        List<int> m_Choices2_int = new List<int>() { 4, 5, 6 };
        List<int> Choices3_int => m_Choices2_int;
        List<int> GetLstChoices_int() { return m_Choices2_int; }

        Dictionary<string, int> GetDicChoices_int()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            ret.Add("aaaa", 1);
            ret.Add("bbbb", 2);
            ret.Add("cccc", 3);
            return ret;
        }
        #endregion

        #region float
        [ADDropDown("m_Choices1_float")]
        public float float_array;

        [ADDropDown("m_Choices2_float")]
        public float float_list_field;

        [ADDropDown("m_Choices2_float")]
        public float float_list_property;

        [ADDropDown("GetLstChoices_float")]
        public float float_list_method;

        [ADDropDown("GetDicChoices_float")]
        public float float_dic;

        float[] m_Choices1_float = new float[] { 1.1f, 2.2f, 3 };
        List<float> m_Choices2_float = new List<float>() { 4.33f, 5, 6.1f };
        List<float> Choices3_float => m_Choices2_float;
        List<float> GetLstChoices_float() { return m_Choices2_float; }

        Dictionary<string, float> GetDicChoices_float()
        {
            Dictionary<string, float> ret = new Dictionary<string, float>();
            ret.Add("aaaa", 1.2f);
            ret.Add("bbbb", 2);
            ret.Add("cccc", 3);
            return ret;
        }
        #endregion
    }
}
