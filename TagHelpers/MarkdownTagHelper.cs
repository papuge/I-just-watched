using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IJustWatched.TagHelpers
{
    [HtmlTargetElement("markdown")]
    public class MarkdownTagHelper: TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "p";
            var content = await output.GetChildContentAsync();
            if (string.IsNullOrEmpty(content.GetContent()))
            {
                return;
            }
            var markdownText = content.GetContent().Split("&#xD;&#xA;");
            string html = markdownText.Aggregate<string, string>
                (null, (current, mString) => current + ProcessStringToHtml(mString));

            output.Content.SetHtmlContent(html);
        }

        private static string ProcessStringToHtml(string mString)
        {
            if (mString.StartsWith("######"))
            {
                mString = "<h6>" + mString.TrimStart('#') + "</h6>";
            }
            else if (mString.StartsWith("#####"))
            {
                mString = "<h5>" + mString.TrimStart('#') + "</h5>";
            }
            else if (mString.StartsWith("####"))
            {
                mString = "<h4>" + mString.TrimStart('#') + "</h4>";
            }
            else if (mString.StartsWith("###"))
            {
                mString = "<h3>" + mString.TrimStart('#') + "</h3>";
            }
            else if (mString.StartsWith("##"))
            {
                mString = "<h2>" + mString.TrimStart('#') + "</h2>";
            }
            else if (mString.StartsWith("#"))
            {
                mString = "<h1>" + mString.TrimStart('#') + "</h1>";
            }

            if (mString.Contains("**"))
            {
                var parts = mString.Split("**");
                parts = parts.Where(i => i != "").ToArray();
                if (mString.StartsWith("**"))
                {
                    parts.Prepend("");
                }
                mString = null;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        mString += "<b>" + parts[i] + "</b>";
                    }
                    else
                    {
                        mString += parts[i];
                    }
                }
                mString += "&#xD;&#xA;";
            }
            
            if (mString.Contains("*"))
            {
                var parts = mString.Split("*");
                parts = parts.Where(i => i != "").ToArray();
                if (mString.StartsWith("*"))
                {
                    parts.Prepend("");
                }
                mString = null;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        mString += "<i>" + parts[i] + "</i>";
                    }
                    else
                    {
                        mString += parts[i];
                    }
                }
                mString += "&#xD;&#xA;";
            }
            return mString;
        }
    }
}