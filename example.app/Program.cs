using example.oops;
using example.solid;

Console.WriteLine("OBJECT-ORIENTED PROGRAMMING");
Console.WriteLine("----------------------------");

// A shared base type lets the app handle different account types through one API.
BankAccount[] accounts =
[
	new SavingsAccount("Avery", 100m),
	new CheckingAccount("Jordan", 100m)
];

foreach (var account in accounts)
{
	account.Deposit(25m);
	account.Withdraw(10m);
	Console.WriteLine(
		$"{account.Owner}'s {account.AccountType} account: " +
		$"balance {account.Balance:C}, monthly interest {account.CalculateMonthlyInterest():C}");
}

Console.WriteLine();
Console.WriteLine("SOLID PRINCIPLES");
Console.WriteLine("----------------");

var order = new Order(
	new OrderItem("Notebook", 4.50m, 2),
	new OrderItem("Pen", 1.50m, 3));
var calculator = new OrderTotalCalculator();
var subtotal = calculator.CalculateSubtotal(order);

// Replace the policy without changing CheckoutService or the order.
IDiscountPolicy discountPolicy = new NoDiscountPolicy();
var noDiscountTotal = discountPolicy.Apply(subtotal);
discountPolicy = new PercentageDiscountPolicy(0.10m);
var total = discountPolicy.Apply(subtotal);

Console.WriteLine($"The same subtotal with no discount: {noDiscountTotal:C}");
Console.WriteLine($"The same subtotal with 10% off: {total:C}");
Console.WriteLine(new ReceiptFormatter().Format(order, subtotal, total));
new CheckoutService(calculator, discountPolicy, new ConsolePaymentProcessor()).Checkout(order);

Console.WriteLine();
Console.WriteLine("A basic printer only needs the print operation:");
IPrinter printer = new BasicPrinter();
printer.Print("Beginner's guide");
