using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Devmoba.ToolManager
{
    public static class CommentSplit
    {
        private static readonly Regex commentPattern = new
            Regex(@"descriptions:(.+)params:(.+)returns:(.+)\*\*\*\/([\s])function ([a-zA-Z0-9(),\s]+)", RegexOptions.Singleline);
        private const int DescriptionGroupId = 1;
        private const int ParamsGroupId = 2;
        private const int ReturnGroupId = 3;
        private const int FuncGroupId = 5;
        private const string BrTag = "<br>";
        private const string HrTag = "<hr>";
        private static readonly char[] trimChars = new char[] { ' ', '*', '\n', '\r', '\t' };

        public static string GetComment(string content)
        {
            var splitContent = content.Split(new String[] { "/***" }, StringSplitOptions.RemoveEmptyEntries);
            var stringBuilder = new StringBuilder();

            foreach (var item in splitContent)
            {
                if (commentPattern.IsMatch(item))
                {
                    var match = commentPattern.Match(item);

                    var param = match.Groups[ParamsGroupId].Value.Trim(trimChars);
                    var ret = match.Groups[ReturnGroupId].Value.Trim(trimChars);
                    var description = match.Groups[DescriptionGroupId].Value.Trim(trimChars);
                    var function = match.Groups[FuncGroupId].Value.Trim(trimChars);

                    stringBuilder.Append($"<b>Function: </b>{function}{BrTag}")
                        .Append($"<b>Descriptions: </b>{description}{BrTag}")
                        .Append($"<b>Params: </b>{param}{BrTag}")
                        .Append($"<b>Returns: </b>{ret}{BrTag}{HrTag}");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
