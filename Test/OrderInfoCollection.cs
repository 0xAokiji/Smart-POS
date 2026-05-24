using System.Collections.ObjectModel;

namespace pos.Test
{
    internal class OrderInfoCollection
    {
        private ObservableCollection<OrderInfo> _orders;
        public ObservableCollection<OrderInfo> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
        public OrderInfoCollection()
        {
            _orders = new ObservableCollection<OrderInfo>();
            this.GenerateOrders();
        }
        private void GenerateOrders()
        {
            _orders.Add(new OrderInfo(1002, "Khaleesi", "Mexico", "QUEEN", "Mexico D.F."));
            _orders.Add(new OrderInfo(1002, "Drogo", "Mexico", "DOTHRAKI", "Mexico D.F."));
            _orders.Add(new OrderInfo(1001, "Maria Anders", "Germany", "ALFKI", "Berlin"));
            _orders.Add(new OrderInfo(1001, "Antonio Moreno", "Germany", "ANTON", "Berlin"));
            _orders.Add(new OrderInfo(1001, "Martin Sommer", "Germany", "BOLID", "Berlin"));
            _orders.Add(new OrderInfo(1001, "Sommer", "Germany", "BOL", "Berlin"));
            _orders.Add(new OrderInfo(1001, "Jon Snow", "Germany", "BOLD", "Berlin"));
            _orders.Add(new OrderInfo(1001, "Arya Stark", "Germany", "NOONE", "Berlin"));
            _orders.Add(new OrderInfo(1002, "Ana Trujilo", "Mexico", "ANATR", "Mexico D.F."));
            _orders.Add(new OrderInfo(1002, "Styles", "Mexico", "HAHI", "Mexico D.F."));
            _orders.Add(new OrderInfo(1002, "Ana Trujilo", "Mexico", "ANATR", "Mexico D.F."));
            _orders.Add(new OrderInfo(1002, "Antonio Moreno", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1002, "Christina Berglund", "Mexico", "ANATR", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Antonio Moreno", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Ronaldo", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Cersei", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Jack", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Antony", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Caesar", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Lanister", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Sansa", "Mexico", "BLAUS", "Mexico D.F."));
            _orders.Add(new OrderInfo(1004, "Thomas Hardy", "UK", "AROUT", "London"));
            _orders.Add(new OrderInfo(1005, "Christina Berglund", "Sweden", "BERGS", "Lula"));
            _orders.Add(new OrderInfo(1005, "Hanna Moos", "Sweden", "BERGS", "Lula"));
            _orders.Add(new OrderInfo(1006, "Hanna Moos", "Germany", "BLAUS", "Mannheim"));
            _orders.Add(new OrderInfo(1007, "Frederique Citeaux", "France", "BLONP", "Strasbourg"));
            _orders.Add(new OrderInfo(1007, "Martin Sommer", "France", "BOTTM", "Strasbourg"));
            _orders.Add(new OrderInfo(1007, "Elizabeth Lincoln", "France", "BLONP", "Strasbourg"));
            _orders.Add(new OrderInfo(1008, "Martin Sommer", "Spain", "BOLID", "Madrid"));
            _orders.Add(new OrderInfo(1009, "Laurence Lebihan", "France", "BONAP", "Marseille"));
            _orders.Add(new OrderInfo(1010, "Elizabeth Lincoln", "Canada", "BOTTM", "Tsawassen"));
        }
    }
}