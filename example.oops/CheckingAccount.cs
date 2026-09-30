namespace example.oops;

/// <summary>A bank account that supports deposits and withdrawals but earns no interest.</summary>
public sealed class CheckingAccount(string owner, decimal openingBalance) : BankAccount(owner, openingBalance)
{
    /// <summary>Gets the display name for this account type.</summary>
    public override string AccountType => "Checking";

    /// <summary>Checking accounts in this example do not earn interest.</summary>
    public override decimal CalculateMonthlyInterest() => 0;
}