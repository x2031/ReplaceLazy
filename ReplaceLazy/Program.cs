using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ReplaceLazy
{
    class Program
    {
        private static string html = "";
        private static string path = "";
        static void Main(string[] args)
        {
            Console.WriteLine("请输入替换文件的路径:");
            path = Console.ReadLine();

            string content = File.ReadAllText(path);
            ReplaceStr(content);

            Console.ReadKey();
        }
        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="content">html内容</param>
        /// <param name="index">开始索引位置</param>
        public static void ReplaceStr(string content, int index = 0)
        {
            int startindex = content.IndexOf("<img", index);

            if (startindex > 0)
            {
                //判断是否有src属性
                int endindex = content.IndexOf(">", startindex);
                int strLength = endindex - startindex + 1;

                int srcindex = content.IndexOf(" src", startindex);


                //可以替换
                if (srcindex < endindex)
                {
                    //替换src
                    content = content.Remove(srcindex, 4);
                    content = content.Insert(srcindex, " data-original");

                    endindex = content.IndexOf(">", startindex);
                    strLength = endindex - startindex + 1;

                    //添加class
                    //判断是否有class
                    int classindex = content.IndexOf("class=\"", startindex, strLength);
                    int lazyindex = content.IndexOf("lazy", startindex, strLength);
                    if (classindex > 0 && lazyindex == -1)
                    {
                        content = content.Insert(classindex + 7, "lazy ");
                    }
                    else if (classindex == -1 && lazyindex == -1)
                    {
                        content = content.Insert(startindex + 4, " class=\"lazy\"");
                    }

                }
                //搜寻下一个img标签进行替换
                ReplaceStr(content, startindex + 3);
            }
            else
            {
                File.WriteAllText(path, content);
                Console.WriteLine("！！！！！替换完成！！！！！");
            }
        }
    }
}
