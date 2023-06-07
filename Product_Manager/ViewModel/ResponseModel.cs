namespace Product_Manager.ViewModel
{
    public class ResponseModel
    {
        public bool IsSuccess
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string InternalExc { get; set; }
    }
}
