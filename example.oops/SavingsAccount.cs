namespace example.oops;

/// <summary>
/// A bank account that earns interest at a configurable annual rate.
/// </summary>
public sealed class SavingsAccount : BankAccount
{
    private readonly decimal _annualInterestRate;

    /// <summary>Creates a savings account, using a 3% annual rate by default.</summary>
    /// <param name="owner">The person who owns the account.</param>
    /// <param name="openingBalance">The starting balance.</param>
    /// <param name="annualInterestRate">An annual rate from 0 to 1; for example, 0.03 means 3%.</param>
    public SavingsAccount(string owner, decimal openingBalance, decimal annualInterestRate = 0.03m)
        : base(owner, openingBalance)
    {
        if (annualInterestRate < 0 || annualInterestRate > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(annualInterestRate), "The rate must be between 0 and 1.");
        }

        _annualInterestRate = annualInterestRate;
    }

    /// <summary>Gets the display name for this account type.</summary>
    public override string AccountType => "Savings";

    /// <summary>Returns the balance multiplied by the annual rate and divided by 12.</summary>
    public override decimal CalculateMonthlyInterest() => Balance * _annualInterestRate / 12;
}