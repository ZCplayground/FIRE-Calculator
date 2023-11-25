using System;

public static class Solution
{
    public static long HowMuchFIREMoneyDoINeed(long yearlyExpense)
    {
        /* According to FIRE, to quit your day job, you need to have 25 times your annual expenses in investments, where you only withdraw 4% of the total each year. 
         * While you take out your living expenses, the investments are also replenishing that money through compound interest or growing in value or dividends */

        long FIREMoney = yearlyExpense * 25;

        return FIREMoney;
    }

    public static long HowManyYearsMySavingRunsOut(int saving, int yearlyExpense, bool output = false)
    {
        var FIREMoney = HowMuchFIREMoneyDoINeed(yearlyExpense);
        if (saving >= FIREMoney)
        {
            return long.MaxValue;
        }
        int res = 0;
        double savingFloatingPoint = (double)saving;

        if (output)
        {
            Console.WriteLine("{0}\t{1}", "Year", "Savings");
            Console.WriteLine("{0}\t{1}", "0", saving);
        }

        while (savingFloatingPoint > 0)
        {
            savingFloatingPoint = savingFloatingPoint * 1.04;
            savingFloatingPoint -= yearlyExpense;
            savingFloatingPoint = Math.Round(savingFloatingPoint, 2);
            if (savingFloatingPoint < 0) break;
            res++;
            if (output) Console.WriteLine("{0}\t{1}", res, savingFloatingPoint);
        }
        return res;
    }

    public static double HowMuchMoneyICanSpendPerYear(int saving, int currentAge, int lifeExpectency)
    {
        int yearsToLive = lifeExpectency - currentAge;

        // Use binary search. Guess value is bigger than left value, and smaller than right value;
        double leftValue = (double)saving / yearsToLive;
        double rightValue = (double)saving;
        double guess = (leftValue + rightValue) / 2;

        while (rightValue - leftValue >= 1e-6)
        {
            guess = (leftValue + rightValue) / 2;
            bool canISurvive = HowManyYearsMySavingRunsOut(saving, (int)(guess)) > yearsToLive ? true : false;
            if (canISurvive)
            {
                leftValue = guess;
            }
            else rightValue = guess;
        }
        guess = Math.Round(guess, 2);
        return guess;
    }
}
class Program
{
    static void Main()
    {
        Console.Write($"Welcome to FIRE Cauculator\n" + "Please input your monthly expense: ");
        int monthlyExpense;
        monthlyExpense = int.Parse(Console.ReadLine());
        Console.WriteLine("So your annual expense is {0}.", monthlyExpense * 12);
        var FIREMoney = Solution.HowMuchFIREMoneyDoINeed(monthlyExpense * 12);
        Console.WriteLine("This is your FIRE money: {0}.", FIREMoney);


        Console.Write($"Please input your saving: ");
        int saving;
        saving = int.Parse(Console.ReadLine());
        var yearsToLive = Solution.HowManyYearsMySavingRunsOut(saving, monthlyExpense * 12, true);
        if (yearsToLive == long.MaxValue)
        {
            Console.WriteLine("You are already FIRE!!");
        }
        else
        {
            Console.WriteLine("Current saving will run out in {0} years", yearsToLive);
        }

        // another way to FIRE is lower your spend
        if (yearsToLive != long.MaxValue)
        {
            Console.Write("What is your age: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("What is your life expectency: ");
            int lifeExpectency = int.Parse(Console.ReadLine());
            Console.WriteLine("So, you have {0} more years to live. ", lifeExpectency - age);
            var yearlySpend = Solution.HowMuchMoneyICanSpendPerYear(saving, age, lifeExpectency);
            Console.Write("Given your age, saving, and life expectency, you can spend: \n" +
                "{0} per year, i.e {1} per month,\n" +
                "so that at the end of your life, your saving will run out.", yearlySpend, Math.Round(yearlySpend / 12, 2));
        }
    }
}

