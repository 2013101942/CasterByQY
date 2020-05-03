namespace CaterModel {
    public partial class MemberInfo
   {
       public string MTypeTitle { get; set; }
       public decimal MDiscount { get; set; }
   }

    public partial class DishInfo
    {
        public string DTypeTitle { get; set; }
    }

    public partial class TableInfo
    {
        public string HTitle { get; set; }
    }

    public partial class OrderInfo
    {
        //菜品编号
        public string DId { get; set; }
        //菜品名称
        public string DTitle { get; set; }
        //某菜品数量
        public int DNumber { get; set; }
        //价格
        public decimal DPrice { get; set; }
    }
}
