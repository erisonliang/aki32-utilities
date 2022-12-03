using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Aki32Utilities.WFAAppUtilities.NodeController
{
    /// <summary>
    /// STNode节点特性
    /// 用于描述STNode开发者信息 以及部分行为
    /// </summary>
    public class STNodeAttribute : Attribute
    {

        // ★★★★★★★★★★★★★★★ props


        private string _Path;
        public string Path => _Path;

        private string _Author;
        public string Author => _Author;

        private string _Mail;
        public string Mail => _Mail;

        private string _Link;
        public string Link => _Link;

        private string _Description;
        public string Description => _Description;


        private static char[] m_ch_splitter = new char[] { '/', '\\' };
        private static Regex m_reg = new Regex(@"^https?://", RegexOptions.IgnoreCase);

        private static Dictionary<Type, MethodInfo> m_dic = new Dictionary<Type, MethodInfo>();




        // ★★★★★★★★★★★★★★★ init

        public STNodeAttribute(string strPath) : this(strPath, null, null, null, null) { }
        public STNodeAttribute(string strPath, string strDescription) : this(strPath, null, null, null, strDescription) { }
        public STNodeAttribute(string strPath, string strAuthor, string strMail, string strLink, string strDescription)
        {
            if (!string.IsNullOrEmpty(strPath))
                strPath = strPath.Trim().Trim(m_ch_splitter).Trim();

            _Path = strPath;

            _Author = strAuthor;
            _Mail = strMail;
            _Description = strDescription;

            if (string.IsNullOrEmpty(strLink) || strLink.Trim() == string.Empty) return;
            strLink = strLink.Trim();
            _Link = m_reg.IsMatch(strLink) ? strLink : "http://" + strLink;
        }


        // ★★★★★★★★★★★★★★★ methods

        public static MethodInfo GetHelpMethod(Type stNodeType)
        {
            if (m_dic.ContainsKey(stNodeType)) return m_dic[stNodeType];
            var mi = stNodeType.GetMethod("ShowHelpInfo");
            if (mi == null) return null;
            if (!mi.IsStatic) return null;
            var ps = mi.GetParameters();
            if (ps.Length != 1) return null;
            if (ps[0].ParameterType != typeof(string)) return null;
            m_dic.Add(stNodeType, mi);
            return mi;
        }

        public static void ShowHelp(Type stNodeType)
        {
            var mi = GetHelpMethod(stNodeType);
            if (mi == null) return;
            mi.Invoke(null, new object[] { stNodeType.Module.FullyQualifiedName });
        }


        // ★★★★★★★★★★★★★★★

    }
}
