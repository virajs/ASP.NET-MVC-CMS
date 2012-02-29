using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Models
{
    public class JsonManageViewModel
    {
        public string Name;
        public string ColumnNames;
        public string ColumnProps;
        public string ColumnWidths;
        public string ColumnTypes;
        public string RowBuilder;
        public bool IsSortable;
        public bool IsCloneable;
        public bool IsActivatable;
        public bool HasDetailsPage;
        public bool HasRowBuilder
        {
            get { return !string.IsNullOrEmpty(RowBuilder); }
        }
    }
}
