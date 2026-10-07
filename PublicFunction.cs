using System;
using System.IO;
using System.Reflection;

namespace GuruminTrainerSteam
{
    internal class PublicFunction
    {
        ///<summary>
        /// &lt;文本型&gt; 取软件基本信息 
        /// <param name="paramcode">(整数型 要获取的信息代码)<para>参数代码含义:</para>1：取软件名称；2:取软件版本；3:取软件开发者；4、取软件产品名称。<para></para></param>
        /// <returns><para></para>返回文本型基本信息结果，失败使用了除1-4以外的参数则返回"InvalidRequest"。</returns> 
        /// </summary>
        public static string GetAPPInformation(int paramcode)
        {
            //取软件程序集
            Assembly asm = Assembly.GetExecutingAssembly();
            //取软件标题、版本、公司、产品名称
            AssemblyTitleAttribute asmdis = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute));
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            AssemblyCompanyAttribute asmcpn = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCompanyAttribute));
            AssemblyProductAttribute Product = (AssemblyProductAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyProductAttribute));
            string ver = "Ver " + version.ToString();
            string title = asmdis.Title;
            string company = asmcpn.Company;
            string appEnglishNane = Product.Product;
            if (paramcode == 1)
            {
                return title;
            }
            else if (paramcode == 2)
            {
                return ver;
            }
            else if (paramcode == 3)
            {
                return company;
            }
            else if (paramcode == 4)
            {
                return appEnglishNane;
            }
            else
            {
                return "InvalidRequest";
            }
        }



    }
}

