namespace example.oops;

/// <summary>
/// Defines the shared operations and state for every bank account in the example.
/// </summary>
/// <remarks>
/// The private balance demonstrates encapsulation. The abstract members define the
/// abstraction that each specific account type must complete.
/// </remarks>
public abstract class BankAccount
{
    private decimal _balance;

    /// <summary>
    /// Creates an account with an owner and a non-negative opening balance.
    /// </summary>
    /// <param name="owner">The person who owns the account.</param>
    /// <param name="openingBalance">The starting balance.</param>
    /// <exception cref="ArgumentException">The owner is empty or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The opening balance is negative.</exception>
    protected BankAccount(string owner, decimal openingBalance)
    {
        if (string.IsNullOrWhiteSpace(owner))
        {
            throw new ArgumentException("An account owner is required.", nameof(owner));
        }

        if (openingBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(openingBalance), "The opening balance cannot be negative.");
        }

        Owner = owner;
        _balance = openingBalance;
    }

    /// <summary>Gets the account owner's name.</summary>
    public string Owner { get; }

    /// <summary>Gets the current balance without exposing a way to change it directly.</summary>
    public decimal Balance => _balance;

    /// <summary>Gets the account type name supplied by the specific account class.</summary>
    public abstract string AccountType { get; }

    /// <summary>Adds a positive amount to the account balance.</summary>
    /// <param name="amount">The amount to deposit.</param>
    /// <exception cref="ArgumentOutOfRangeException">The amount is zero or negative.</exception>
    public void Deposit(decimal amount)
    {
        ValidatePositiveAmount(amount);
        _balance += amount;
    }

    /// <summary>
    /// Removes a positive amount when enough money is available.
    /// </summary>
    /// <param name="amount">The amount to withdraw.</param>
    /// <returns><see langword="true"/> if the withdrawal succeeds; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The amount is zero or negative.</exception>
    public virtual bool Withdraw(decimal amount)
    {
        ValidatePositiveAmount(amount);

        if (amount > _balance)
        {
            return false;
        }

        _balance -= amount;
        return true;
    }

    /// <summary>Calculates the interest for one month according to this account type.</summary>
    public abstract decimal CalculateMonthlyInterest();

    private static void ValidatePositiveAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The amount must be greater than zero.");
        }
    }
}