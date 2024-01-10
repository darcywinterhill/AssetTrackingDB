
using AssetTrackingDB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

CRUD crud = new CRUD();
Message message = new Message();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(" >>> WELCOME! <<<");
Console.ResetColor();
Console.WriteLine();
Console.WriteLine(" Here you can view and manage your company assets.");

RunApp();

void RunApp()
{
    while (true)
    {
        ShowOptions();
        ChooseOption();
    }
}

void ShowOptions()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine(" Choose what you want to do -->");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine(" (1) Show list with assets");
    Console.WriteLine(" (2) Add an asset to the list");
    Console.WriteLine(" (3) Edit asset information");
    Console.WriteLine(" (4) Delete an asset");
    Console.WriteLine(" (5) View statistics");
    Console.WriteLine(" (6) Exit application");
}

void ChooseOption()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write(" My choice: ");
    Console.ResetColor();

    string optionInput = Console.ReadLine();

    bool isOptionInputInt = int.TryParse(optionInput, out int optionInputInt);

    try
    {
        optionInputInt = Convert.ToInt32(optionInput);
    }
    catch (FormatException)
    {
        while (!isOptionInputInt)
        {
            message.DisplayErrorMessage(" Make a choice - (1), (2), (3), (4), (5) or (6): ");
            optionInput = Console.ReadLine();
            isOptionInputInt = int.TryParse(optionInput, out optionInputInt);
        }
    }

    while (isOptionInputInt && optionInputInt < 1 || optionInputInt > 6)
    {
        message.DisplayErrorMessage(" You can only choose (1), (2), (3), (4), (5) or (6): ");
        optionInput = Console.ReadLine();
        isOptionInputInt = int.TryParse(optionInput, out optionInputInt);
    }

    switch (optionInputInt)
    {
        case 1:
            HandleEmptyList();
            crud.ShowList();
            break;
        case 2:
            crud.AddAsset();
            break;
        case 3:
            HandleEmptyList();
            crud.EditAsset();
            break;
        case 4:
            HandleEmptyList();
            crud.DeleteAsset();
            break;
        case 5:
            ChooseStats();
            break;
        case 6:
            ExitApp();
            break;
    }
}

void ChooseStats()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine(" Choose what statistics to view -->");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine(" (1) Total number of assets");
    Console.WriteLine(" (2) Number of Laptops/Mobile Phones");
    Console.WriteLine(" (3) Total cost of assets");
    Console.WriteLine(" (4) Cost Laptops/Mobile Phones");
    Console.WriteLine(" (5) Back to main page");

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write(" My choice: ");
    Console.ResetColor();

    string optionInput = Console.ReadLine();

    bool isOptionInputInt = int.TryParse(optionInput, out int optionInputInt);

    try
    {
        optionInputInt = Convert.ToInt32(optionInput);
    }
    catch (FormatException)
    {
        while (!isOptionInputInt)
        {
            message.DisplayErrorMessage(" Make a choice - (1), (2), (3), (4) or (5): ");
            optionInput = Console.ReadLine();
            isOptionInputInt = int.TryParse(optionInput, out optionInputInt);
        }
    }

    while (isOptionInputInt && optionInputInt < 1 || optionInputInt > 5)
    {
        message.DisplayErrorMessage(" You can only choose (1), (2), (3), (4) or (5): ");
        optionInput = Console.ReadLine();
        isOptionInputInt = int.TryParse(optionInput, out optionInputInt);
    }

    switch (optionInputInt)
    {
        case 1:
            HandleEmptyList();
            TotalAssets();
            ChooseStats();
            break;
        case 2:
            HandleEmptyList();
            SplitAssets();
            ChooseStats();
            break;
        case 3:
            HandleEmptyList();
            TotalCost();
            ChooseStats();
            break;
        case 4:
            HandleEmptyList();
            SplitCost();
            ChooseStats();
            break;
        case 5:
            RunApp();
            break;
    }
}
void TotalAssets()
{
    Console.WriteLine();
    message.StatisticsMessage($" >> Total nr of assets: [{CountAssets()}]");
}

void SplitAssets()
{
    MyDbContext Context = new MyDbContext();

    int laptopCount = Context.Assets.Where(x => x.Type.Equals("Laptop")).Count();
    int mobilePhoneCount = Context.Assets.Where(x => x.Type.Equals("MobilePhone")).Count();

    Console.WriteLine();
    message.StatisticsMessage($" >> Nr of Laptops: [{laptopCount}]");
    message.StatisticsMessage($" >> Nr of Mobile Phones: [{mobilePhoneCount}]");
}

void TotalCost()
{
    MyDbContext Context = new MyDbContext();

    int totalCost = Context.Assets.Sum(x => x.Price);

    Console.WriteLine();
    message.StatisticsMessage($" >> Total cost: [{totalCost} SEK]");
}

void SplitCost()
{
    MyDbContext Context = new MyDbContext();

    int laptopCost = Context.Assets.Where(x => x.Type == "Laptop").Sum(x => x.Price);
    int mobilePhoneCost = Context.Assets.Where(x => x.Type == "MobilePhone").Sum(x => x.Price);

    Console.WriteLine();
    message.StatisticsMessage($" >> Laptop cost: [{laptopCost} SEK]");
    message.StatisticsMessage($" >> Mobile Phone cost: [{mobilePhoneCost} SEK]");
}

int CountAssets()
{
    MyDbContext Context = new MyDbContext();

    int assetCount = Context.Assets.Count();
    return assetCount;
}

//Handle empty list
void HandleEmptyList()
{
    int assetCount = CountAssets();

    if (assetCount == 0)
    {
        message.DisplayErrorMessage(" The list is empty.");
        RunApp();
    }
}
void ExitApp()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(" >>> GOODBYE! <<<");
    Console.ResetColor();
    System.Environment.Exit(1);
}
