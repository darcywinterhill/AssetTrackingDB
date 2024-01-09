
using AssetTrackingDB;
using System.Collections.Generic;

CRUD crud = new CRUD();
Message message = new Message();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(" >>> WELCOME! <<<");
Console.ResetColor();
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
    Console.WriteLine(" (1) Show list with assets.");
    Console.WriteLine(" (2) Add an asset to the list.");
    Console.WriteLine(" (3) Edit asset information.");
    Console.WriteLine(" (4) Delete an asset.");
    Console.WriteLine(" (5) Exit application.");
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
            //HandleEmptyList();
            crud.ShowList();
            break;
        case 2:
            crud.AddAsset();
            break;
        case 3:
            //HandleEmptyList();
            crud.EditAsset();
            break;
        case 4:
            //HandleEmptyList();
            //DeleteCar();
            break;
        case 5:
            //();
            break;
    }
}