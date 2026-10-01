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

Console.WriteLine("S - Single Responsibility Principle");
var order = new Order(
	new OrderItem("Notebook", 4.50m, 2),
	new OrderItem("Pen", 1.50m, 3));
var calculator = new OrderTotalCalculator();
var subtotal = calculator.CalculateSubtotal(order);
Console.WriteLine($"OrderTotalCalculator calculates the subtotal: {subtotal:C}");

Console.WriteLine();
Console.WriteLine("O - Open-Closed Principle");
var fixedAmountPolicy = new FixedAmountDiscountPolicy(2m);
Console.WriteLine(
	$"Added {nameof(FixedAmountDiscountPolicy)} without changing {nameof(CheckoutService)}: " +
	$"{fixedAmountPolicy.Apply(subtotal):C}");

Console.WriteLine();
Console.WriteLine("L - Liskov Substitution Principle");
IDiscountPolicy discountPolicy = new NoDiscountPolicy();
foreach (var policy in new IDiscountPolicy[]
{
	discountPolicy,
	new PercentageDiscountPolicy(0.10m),
	fixedAmountPolicy
})
{
	discountPolicy = policy;
	Console.WriteLine($"{policy.GetType().Name} works as IDiscountPolicy: {discountPolicy.Apply(subtotal):C}");
}

discountPolicy = new PercentageDiscountPolicy(0.10m);
var total = discountPolicy.Apply(subtotal);
Console.WriteLine(new ReceiptFormatter().Format(order, subtotal, total));

Console.WriteLine();
Console.WriteLine("I - Interface Segregation Principle");
Console.WriteLine("A basic printer only needs the print operation:");
IPrinter printer = new BasicPrinter();
printer.Print("Beginner's guide");
Console.WriteLine($"An office machine also scans: {new OfficeMachine().Scan()}");

Console.WriteLine();
Console.WriteLine("D - Dependency Inversion Principle");
Console.WriteLine("CheckoutService receives abstractions through its constructor:");
new CheckoutService(calculator, discountPolicy, new ConsolePaymentProcessor()).Checkout(order);
