namespace Devmoba.ToolClient.Models
{
    public class FunctionDeclaration
    {
        public string Name { get; set; }

        public int ColumnStart { get; set; }

        public int LineStart { get; set; }

        public int ColumnEnd { get; set; }

        public int LineEnd { get; set; }
    }
}
