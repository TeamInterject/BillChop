using System;
using System.Collections.Generic;
using System.Text;

namespace BillChopBETests
{
    public interface ISutBuilder<TSut> where TSut : class
    {
        public TSut CreateSut();
    }
}
