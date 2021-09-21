namespace BillChopBETests
{
    public interface ISutBuilder<TSut> where TSut : class
    {
        public TSut CreateSut();
    }
}
