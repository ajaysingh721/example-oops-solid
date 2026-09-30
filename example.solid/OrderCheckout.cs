namespace example.solid;

/// <summary>One named product and its price and quantity in an order.</summary>
public sealed record OrderItem
{
    /// <summary>Creates an order item with a name, non-negative price, and positive quantity.</summary>
    /// <param name="name">The product name.</param>
    /// <param name="unitPrice">The price of one unit.</param>
    /// <param name="quantity">The number of units ordered.</param>
    public OrderItem(string name, decimal unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("An item name is required.", nameof(name));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "The unit price cannot be negative.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "The quantity must be greater than zero.");
        }

        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    /// <summary>Gets the product name.</summary>
    public string Name { get; }

    /// <summary>Gets the price of one unit.</summary>
    public decimal UnitPrice { get; }

    /// <summary>Gets the number of units ordered.</summary>
    public int Quantity { get; }
}

/// <summary>An order containing the items that will be checked out.</summary>
public sealed class Order(params OrderItem[] items)
{
    /// <summary>Gets the order items as a read-only list.</summary>
    public IReadOnlyList<OrderItem> Items { get; } = Array.AsReadOnly(items.ToArray());
}

/// <summary>Calculates an order subtotal, without applying discounts or processing payment.</summary>
public interface IOrderTotalCalculator
{
    /// <summary>Returns the sum of each item's unit price multiplied by its quantity.</summary>
    decimal CalculateSubtotal(Order order);
}

/// <summary>The standard calculator for order subtotals.</summary>
public sealed class OrderTotalCalculator : IOrderTotalCalculator
{
    public decimal CalculateSubtotal(Order order) =>
        order.Items.Sum(item => item.UnitPrice * item.Quantity);
}

/// <summary>Turns order and price data into text suitable for displaying as a receipt.</summary>
public sealed class ReceiptFormatter
{
    /// <summary>Formats the items, subtotal, and final total as a multi-line receipt.</summary>
    public string Format(Order order, decimal subtotal, decimal total)
    {
        var lines = order.Items.Select(item =>
            $"{item.Name} x {item.Quantity}: {item.UnitPrice * item.Quantity:C}");

        return string.Join(Environment.NewLine, lines)
            + $"{Environment.NewLine}Subtotal: {subtotal:C}"
            + $"{Environment.NewLine}Total after discount: {total:C}";
    }
}

/// <summary>
/// Defines a replaceable rule for turning a non-negative subtotal into a final total.
/// Implementations should return a value from zero to the subtotal.
/// </summary>
public interface IDiscountPolicy
{
    /// <summary>Applies this policy to a non-negative subtotal.</summary>
    decimal Apply(decimal subtotal);
}

/// <summary>A discount policy that leaves the subtotal unchanged.</summary>
public sealed class NoDiscountPolicy : IDiscountPolicy
{
    public decimal Apply(decimal subtotal) => subtotal;
}

/// <summary>A discount policy that subtracts a fixed fraction of the subtotal.</summary>
/// <param name="discountRate">A fraction from 0 to 1; for example, 0.10 means 10% off.</param>
public sealed class PercentageDiscountPolicy(decimal discountRate) : IDiscountPolicy
{
    /// <summary>Gets the configured fraction to subtract from the subtotal.</summary>
    public decimal DiscountRate { get; } = ValidateRate(discountRate);

    /// <summary>Returns the subtotal after subtracting the configured percentage.</summary>
    public decimal Apply(decimal subtotal) => subtotal * (1 - DiscountRate);

    private static decimal ValidateRate(decimal rate)
    {
        if (rate < 0 || rate > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "The discount rate must be between 0 and 1.");
        }

        return rate;
    }
}

/// <summary>Defines how a checkout sends its final amount for payment.</summary>
public interface IPaymentProcessor
{
    /// <summary>Processes a payment for the supplied amount.</summary>
    void ProcessPayment(decimal amount);
}

/// <summary>A demonstration payment processor that writes the payment to the console.</summary>
public sealed class ConsolePaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount) => Console.WriteLine($"Payment processed: {amount:C}");
}

/// <summary>
/// Coordinates checkout by asking abstractions to calculate, discount, and process payment.
/// </summary>
/// <param name="totalCalculator">The subtotal calculator to use.</param>
/// <param name="discountPolicy">The discount rule to use.</param>
/// <param name="paymentProcessor">The payment implementation to use.</param>
public sealed class CheckoutService(
    IOrderTotalCalculator totalCalculator,
    IDiscountPolicy discountPolicy,
    IPaymentProcessor paymentProcessor)
{
    /// <summary>Calculates the order total, processes payment, and returns the paid amount.</summary>
    public decimal Checkout(Order order)
    {
        var subtotal = totalCalculator.CalculateSubtotal(order);
        var total = discountPolicy.Apply(subtotal);
        paymentProcessor.ProcessPayment(total);
        return total;
    }
}